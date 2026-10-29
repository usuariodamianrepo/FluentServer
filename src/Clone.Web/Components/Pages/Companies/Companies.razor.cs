using Clone.Models;
using Clone.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Clone.Web.Components.Pages.Companies
{
    public partial class Companies
    {
        private const string MESSAGEBAR_SECTION = "MESSAGEBAR_COMPANY";
        private FluentDataGrid<Company>? _Grid;

        private Company _Company { get; set; } = new();
        private Company _CompaniesDetails { get; set; } = new();
        private IQueryable<Company>? _Companies { get; set; }

        #region Search
        private string _NameSearch = string.Empty;
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
            _Company = new();
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
            if (_CompaniesDetails is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Message.NoSelection"]);
                return;
            }

            try
            {
                var json = JsonSerializer.Serialize(_CompaniesDetails, new JsonSerializerOptions { WriteIndented = true });
                await JS.InvokeVoidAsync("navigator.clipboard.writeText", json);

                await NotificationService.ShowMessageBarAsync(options =>
                {
                    options.Section = MESSAGEBAR_SECTION;
                    options.Intent = MessageBarIntent.Info;
                    options.Layout = MessageBarLayout.SingleLine;
                    options.Title = Localizer["Message.Copied", _CompaniesDetails.Id];
                    options.AllowDismiss = true;
                    options.Lifetime = TimeSpan.FromSeconds(3);
                    options.ResultTiming = MessageBarResultTiming.Closed;
                });
            }
            catch (JSException ex)
            {
                Logger.LogError(ex, "Error copying Company {Id} to clipboard.", _CompaniesDetails.Id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Copy"]);
            }
        }

        private async Task OnDeleteClicked(int id)
        {
            var repository = UnitOfWork.Repository<Company>();
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
                Logger.LogError(ex, "Concurrency error while deleting Company {Id}.", id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.ModifiedByAnotherUser"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error while deleting Company {Id}.", id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Delete"]);
            }
        }

        private async Task OnDetailsClicked(int id)
        {
            var repository = UnitOfWork.Repository<Company>();
            var toDetails = await repository.GetByIdAsync(id);

            if (toDetails is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.NotFound", id]);
                return;
            }

            _CompaniesDetails = toDetails;
            _CollapsedDetails = false;
        }

        private async Task OnEditClicked(int id)
        {
            var repository = UnitOfWork.Repository<Company>();
            var toEdit = await repository.GetByIdAsync(id);

            if (toEdit is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.NotFound", id]);
                return;
            }

            _Company = toEdit;
            _CollapsedContent = false;
        }

        private async Task OnSaveContentClicked()
        {
            if (string.IsNullOrWhiteSpace(_Company.Name))
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.NameRequired"]);
                return;
            }

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var repository = UnitOfWork.Repository<Company>();

            try
            {
                if (_Company.Id == 0)
                {
                    _Company.InsertAudit(authState.User);
                    await repository.AddAsync(_Company);
                }
                else
                {
                    _Company.UpdateAudit(authState.User);
                    await repository.UpdateAsync(_Company);
                }

                await UnitOfWork.SaveAsync();

                await SearchData();
                ResetForm();
                _CollapsedContent = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogError(ex, "Concurrency error Company Id: {Id}.", _Company.Id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Concurrency"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error Company Id: {Id}.", _Company.Id);
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Save"]);
            }
        }

        private async Task OnSearchClicked()
        {
            await SearchData();
        }

        private void ResetForm()
        {
            _Company = new Company();
        }

        private async Task SearchData()
        {
            _SearchButtonLoading = true;

            try
            {
                var findeds = await UnitOfWork.Repository<Company>().Entities
                    .Where(d => string.IsNullOrWhiteSpace(_NameSearch) || d.Name.Contains(_NameSearch))
                    .AsNoTracking()
                    .Take(MaxRows.MaxThirty)
                    .ToListAsync();

                _TotalSearched = findeds.Count;
                _IsMaxSearch = _TotalSearched >= MaxRows.MaxThirty;

                _Companies = findeds.AsQueryable();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error searching Companies.");
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
