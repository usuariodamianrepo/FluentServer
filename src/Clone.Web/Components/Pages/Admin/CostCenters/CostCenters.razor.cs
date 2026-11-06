using Clone.Models;
using Clone.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Clone.Web.Components.Pages.Admin.CostCenters
{
    public partial class CostCenters
    {
        private const string MESSAGEBAR_SECTION = "MESSAGEBAR_COST_CENTER";
        private FluentDataGrid<CostCenter>? _Grid;

        private CostCenter _CostCenter { get; set; } = new();
        private CostCenter _CostCenterDetails { get; set; } = new();
        private IQueryable<CostCenter>? _CostCenters { get; set; }

        #region Search
        private string _NameSearch = string.Empty;
        private int _MaxRowsDefined = MaxRows.MaxThirty;
        private bool _IsMaxSearch = false;
        private bool _SearchButtonLoading = false;
        private int _TotalSearched = 0;
        #endregion

        #region Filter
        private bool _CollapsedContent = true;
        private bool _CollapsedDetails = true;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await SearchData();
        }

        private async Task KeyDownHandler(FluentKeyCodeEventArgs e)
        {
            if (e.KeyCode == Constants.KeyCodeEnter)
            {
                await SearchData();
            }
        }

        private void OnAddClicked()
        {
            _CostCenter = new();
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
            if (_CostCenterDetails is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Message.NoSelection"]);
                return;
            }

            try
            {
                var json = JsonSerializer.Serialize(_CostCenterDetails, new JsonSerializerOptions { WriteIndented = true });
                await JS.InvokeVoidAsync("navigator.clipboard.writeText", json);

                await NotificationService.ShowMessageBarAsync(options =>
                {
                    options.Section = MESSAGEBAR_SECTION;
                    options.Intent = MessageBarIntent.Info;
                    options.Layout = MessageBarLayout.SingleLine;
                    options.Title = Localizer["Message.Copied", _CostCenterDetails.Id];
                    options.AllowDismiss = true;
                    options.Lifetime = TimeSpan.FromSeconds(3);
                    options.ResultTiming = MessageBarResultTiming.Closed;
                });
            }
            catch (JSException ex)
            {
                Logger.LogError(ex, "Error copying CostCenter {Id} to clipboard.", _CostCenterDetails.Id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Copy"]);
            }
        }

        private async Task OnDeleteClicked(int id)
        {
            var repository = UnitOfWork.Repository<CostCenter>();
            var toDelete = await repository.GetByIdAsync(id);

            if (toDelete is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.NotFound", id]);
                return;
            }

            var result = await DialogService.ShowConfirmationAsync(Localizer["Confirm.Delete", toDelete.Id]);
            if (result.Cancelled)
            {
                return;
            }

            try
            {
                await repository.RemoveAsync(toDelete);
                await UnitOfWork.SaveAsync();
                await SearchData();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogError(ex, "Concurrency error while deleting CostCenter {Id}.", id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.ModifiedByAnotherUser"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error while deleting CostCenter {Id}.", id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Delete"]);
            }
        }

        private async Task OnDetailsClicked(int id)
        {
            var repository = UnitOfWork.Repository<CostCenter>();
            var toDetails = await repository.GetByIdAsync(id);

            if (toDetails is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.NotFound", id]);
                return;
            }

            _CostCenterDetails = toDetails;
            _CollapsedDetails = false;
        }

        private async Task OnEditClicked(int id)
        {
            var repository = UnitOfWork.Repository<CostCenter>();
            var toEdit = await repository.GetByIdAsync(id);

            if (toEdit is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.NotFound", id]);
                return;
            }

            _CostCenter = toEdit;
            _CollapsedContent = false;
        }

        private async Task OnSaveContentClicked()
        {
            if (string.IsNullOrWhiteSpace(_CostCenter.Name))
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.NameRequired"]);
                return;
            }

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var repository = UnitOfWork.Repository<CostCenter>();

            try
            {
                if (_CostCenter.Id == 0)
                {
                    _CostCenter.InsertAudit(authState.User);
                    await repository.AddAsync(_CostCenter);
                }
                else
                {
                    _CostCenter.UpdateAudit(authState.User);
                    await repository.UpdateAsync(_CostCenter);
                }

                await UnitOfWork.SaveAsync();

                await SearchData();
                ResetForm();
                _CollapsedContent = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogError(ex, "Concurrency error CostCenter Id: {Id}.", _CostCenter.Id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Concurrency"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error CostCenter Id: {Id}.", _CostCenter.Id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Save"]);
            }
        }

        private async Task OnSearchClicked()
        {
            await SearchData();
        }

        private void ResetForm()
        {
            _CostCenter = new CostCenter();
        }

        private async Task SearchData()
        {
            _SearchButtonLoading = true;

            try
            {
                var findeds = await UnitOfWork.Repository<CostCenter>().Entities
                    .Where(d => string.IsNullOrWhiteSpace(_NameSearch) || d.Name.Contains(_NameSearch))
                    .OrderBy(d => d.Name)
                    .AsNoTracking()
                    .Take(_MaxRowsDefined)
                    .ToListAsync();

                _TotalSearched = findeds.Count;
                _IsMaxSearch = _TotalSearched >= _MaxRowsDefined;

                _CostCenters = findeds.AsQueryable();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error searching CostCenters.");
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
