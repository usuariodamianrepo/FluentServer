using Clone.Models;
using Clone.Models.Dtos;
using Clone.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Clone.Web.Components.Pages.Receipts
{
    public partial class Receipts
    {
        private const string MESSAGEBAR_SECTION = "MESSAGEBAR_RECEIPT";
        private IEnumerable<UserBySearchDto> _UsersEditSelect { get; set; } = [];
        private IEnumerable<CostCenter> _CostCenterSelect { get; set; } = [];

        private FluentDataGrid<Receipt>? _Grid;

        private Receipt _Receipt { get; set; } = new();
        private Receipt _ReceiptDetails { get; set; } = new();
        private IQueryable<Receipt>? _Receipts { get; set; }
        #region Form
        private bool _IsIngress = true;
        #endregion

        #region Search
        private bool _IsActive = true;
        private bool _IsMaxSearch = false;
        private DateRangeDto _range = DateRangeDto.Today();
        private bool _SearchButtonLoading = false;
        private int _TotalSearched = 0;
        IEnumerable<CostCenter> _CostCenterSearchSelected { get; set; } = [];
        IEnumerable<UserBySearchDto> _UsersSearchSelected { get; set; } = [];
        #endregion

        #region Filter
        private bool _CollapsedContent = true;
        private bool _CollapsedDetails = true;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            var costCenters = await UnitOfWork.Repository<CostCenter>().GetAllAsync();
            _CostCenterSelect = costCenters.OrderBy(c => c.Name);
            _UsersEditSelect = await (UnitOfWork.UserBySearch("", "", "")).ToListAsync();

            await SearchData();
        }

        private async Task KeyDownHandler(FluentKeyCodeEventArgs e)
        {
            if (e.KeyCode == Constants.KeyCodeEnter)
            {
                await SearchData();
            }
        }

        private async Task OnAddDecreaseClicked()
        {
            ResetForm();
            _IsIngress = false;
            _CollapsedContent = false;
        }

        private async Task OnAddIngressClicked()
        {
            ResetForm();
            _IsIngress = true;
            _CollapsedContent = false;
        }

        private void OnCancelContentClicked()
        {
            _CollapsedContent = true;
        }

        private void OnCancelDetailsClicked()
        {
            _CollapsedDetails = true;
        }

        private async Task OnCopyDetailsToClipboard()
        {
            if (_ReceiptDetails is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Message.NoSelection"]);
                return;
            }

            try
            {
                var json = JsonSerializer.Serialize(_ReceiptDetails, new JsonSerializerOptions { WriteIndented = true });
                await JS.InvokeVoidAsync("navigator.clipboard.writeText", json);

                await NotificationService.ShowMessageBarAsync(options =>
                {
                    options.Section = MESSAGEBAR_SECTION;
                    options.Intent = MessageBarIntent.Info;
                    options.Layout = MessageBarLayout.SingleLine;
                    options.Title = Localizer["Message.Copied", _ReceiptDetails.Id];
                    options.AllowDismiss = true;
                    options.Lifetime = TimeSpan.FromSeconds(3);
                    options.ResultTiming = MessageBarResultTiming.Closed;
                });
            }
            catch (JSException ex)
            {
                Logger.LogError(ex, "Error copying Receipt {Id} to clipboard.", _ReceiptDetails.Id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Copy"]);
            }
        }

        private async Task OnDeleteClicked(int id)
        {
            var result = await DialogService.ShowConfirmationAsync(Localizer["Confirm.Delete", id], SharedLocalizer["ShowConfirmation.Title"]);
            if (result.Cancelled)
                return;

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            try
            {
                await ReceiptRepository.DeactivateAsync(id, authState.User);    
                await UnitOfWork.SaveAsync();
                await SearchData();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogError(ex, "Concurrency error while deleting Receipt {Id}.", id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.ModifiedByAnotherUser"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error while deleting Receipt {Id}.", id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Delete"]);
            }
        }

        private async Task OnGenerateAgainstClicked(int id)
        {
            var toGenerate = await UnitOfWork.Repository<Receipt>().GetByIdAsync(id);

            if (toGenerate is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.NotFound", id]);
                return;
            }

            var result = await DialogService.ShowConfirmationAsync(Localizer["Confirm.GenerateAgainst", toGenerate.Amount], SharedLocalizer["ShowConfirmation.Title"]);
            if (result.Cancelled)
                return;

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            try
            {
                var newReceipt = await ReceiptRepository.GenerateAgainstAsync(id, authState.User);
                await UnitOfWork.Repository<Receipt>().AddAsync(newReceipt);
                await UnitOfWork.SaveAsync();
                await NotificationService.ShowWarningBarAsync(MESSAGEBAR_SECTION, title: Localizer["Info.ReceiptGenerated", newReceipt.Id]);
                await SearchData();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogError(ex, "Concurrency error while deleting Receipt {Id}.", id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.ModifiedByAnotherUser"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error while deleting Receipt {Id}.", id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Delete"]);
            }
        }

        private async Task OnDetailsClicked(int id)
        {
            var toDetails = await UnitOfWork.Repository<Receipt>().Entities
                .Include(r => r.ApplicationUser)
                .Include(r => r.CostCenter)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (toDetails is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.NotFound", id]);
                return;
            }

            _ReceiptDetails = toDetails;
            _CollapsedDetails = false;
        }

        private async Task OnEditClicked(int id)
        {
            var toEdit = await UnitOfWork.Repository<Receipt>().GetByIdAsync(id);

            if (toEdit is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.NotFound", id]);
                return;
            }

            _Receipt = toEdit;
            _IsIngress = _Receipt.IsIngress;
            _CollapsedContent = false;
        }

        private void OnRangeChanged(DateRangeDto range) => _range = range;

        private async Task OnSaveContentClicked()
        {
            if (!_Receipt.Active)
                return;

            if (string.IsNullOrEmpty(_Receipt.ApplicationUserId))
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.ApplicationUserRequired"]);
                return;
            }

            if (_Receipt.CostCenterId == 0)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.CostCenterRequired"]);
                return;
            }

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var repository = UnitOfWork.Repository<Receipt>();

            try
            {
                _Receipt.SetAmount(_IsIngress);

                if (_Receipt.Id == 0)
                {
                    _Receipt.InsertAudit(authState.User);
                    await repository.AddAsync(_Receipt);
                }
                else
                {
                    _Receipt.UpdateAudit(authState.User);
                    await repository.UpdateAsync(_Receipt);
                }

                await UnitOfWork.SaveAsync();

                await SearchData();
                ResetForm();
                _CollapsedContent = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogError(ex, "Concurrency error Receipt Id: {Id}.", _Receipt.Id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Concurrency"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error Receipt Id: {Id}.", _Receipt.Id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Save"]);
            }
        }

        private async Task OnSearchClicked()
        {
            await SearchData();
        }

        private async Task OnUserSearchAsync(OptionsSearchEventArgs<UserBySearchDto> e)
        {
            if (e.Text.Length < 3)
                return;
            e.Items = await (UnitOfWork.UserBySearch("", e.Text, "")).ToListAsync();
        }

        private void ResetForm()
        {
            _Receipt = new Receipt() { Date = DateTime.Now };
        }

        private async Task SearchData()
        {
            _SearchButtonLoading = true;

            if (_range.From > _range.To)
                return;

            var userIds = _UsersSearchSelected.Select(u => u.Id).ToList();
            var costCenterIds = _CostCenterSearchSelected.Select(c => c.Id).ToList();

            try
            {
                var findeds = await UnitOfWork.Repository<Receipt>().Entities
                    .Where(d => d.Active == _IsActive &&
                                d.Date >= _range.From && d.Date <= _range.To &&
                                (userIds.Count == 0 || userIds.Contains(d.ApplicationUserId)) &&
                                (costCenterIds.Count == 0 || costCenterIds.Contains(d.CostCenterId)))
                    .Include(d => d.ApplicationUser)
                    .Include(d => d.CostCenter)
                    .OrderByDescending(d => d.Date)
                    .AsNoTracking()
                    .Take(MaxRows.MaxThirty)
                    .ToListAsync();

                _TotalSearched = findeds.Count;
                _IsMaxSearch = _TotalSearched >= MaxRows.MaxThirty;

                _Receipts = findeds.AsQueryable();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error searching Receipts.");
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Search"]);
            }
            finally
            {
                _SearchButtonLoading = false;
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
