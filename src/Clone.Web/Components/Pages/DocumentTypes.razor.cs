using Clone.Models;
using Clone.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Clone.Web.Components.Pages
{
    public partial class DocumentTypes
    {
        const string MESSAGEBAR_SECTION = "MESSAGEBAR_SERVICE_DEFAULT";
        FluentDataGrid<DocumentType>? _Grid;

        [Inject]
        public IJSRuntime JS { get; set; } = default!;
        DocumentType _DocumentType { get; set; } = default!;
        DocumentType _DocumentTypeDetails { get; set; } = default!;
        IQueryable<DocumentType>? _DocumentTypes { get; set; }

        #region Search
        string _NameSearch = string.Empty;
        bool _SearchButtonLoading = false;
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
            _DocumentType = new();
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
            if (_DocumentTypeDetails is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: "No document type selected.");
            }

            var json = JsonSerializer.Serialize(_DocumentTypeDetails, new JsonSerializerOptions { WriteIndented = true });
            await JS.InvokeVoidAsync("navigator.clipboard.writeText", json);
            await NotificationService.ShowMessageBarAsync(options =>
            {
                options.Section = MESSAGEBAR_SECTION;
                options.Intent = MessageBarIntent.Info;
                options.Layout = MessageBarLayout.SingleLine;
                options.Title = $"Document Type Id: {_DocumentTypeDetails!.Id} copied to clipboard!";
                options.AllowDismiss = true;
                options.Lifetime = TimeSpan.FromSeconds(3);
                options.ResultTiming = MessageBarResultTiming.Closed;
            });
        }

        private async Task OnDeleteClicked(int id)
        {
            var repository = UnitOfWork.Repository<DocumentType>();

            var toDelete = await repository.GetByIdAsync(id);
            if (toDelete is null) return;

            var result = await DialogService.ShowConfirmationAsync($"Are you <strong>sure</strong> you want to delete item Id: {toDelete.Id}?");
            if (result.Cancelled) return;

            await repository.RemoveAsync(toDelete);
            await UnitOfWork.SaveAsync();

            await SearchData();
        }

        private async Task OnDetailsClicked(int id)
        {
            var toDetails = await UnitOfWork.Repository<DocumentType>().GetByIdAsync(id);
            if (toDetails is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: $"Document Type Id: {id} type not found");
                return;
            }

            _DocumentTypeDetails = toDetails;
            _CollapsedDetails = false;
        }

        private async Task OnEditClicked(int id)
        {
            var toEdit = await UnitOfWork.Repository<DocumentType>().GetByIdAsync(id);
            if (toEdit is null)
            {
                await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: $"Document Type Id: {id} type not found");
            }
            _DocumentType = toEdit!;
            _CollapsedContent = false;
        }

        private async Task OnSaveContentClicked()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var repository = UnitOfWork.Repository<DocumentType>();


            if (_DocumentType.Id == 0)
            {
                _DocumentType.InsertAudit(authState.User);
                await repository.AddAsync(_DocumentType);
            }
            else
            {
                _DocumentType.UpdateAudit(authState.User);
                await repository.UpdateAsync(_DocumentType);
            }

            await UnitOfWork.SaveAsync();
            await SearchData();
            ResetForm();
            _CollapsedContent = true;
        }

        private async void OnSearchClicked()
        {
            await SearchData();
        }

        private void ResetForm()
        {
            _DocumentType = new DocumentType();
        }

        private async Task SearchData()
        {
            _SearchButtonLoading = true;

            _DocumentTypes = (await UnitOfWork.Repository<DocumentType>()
                .GetBySearchAsync(filter: d => d.Name == "" || d.Name.Contains(_NameSearch)))
                .AsQueryable();

            if (_DocumentTypes.Count() >= Constants.ItemsMaxNumber)
            {
                await NotificationService.ShowWarningBarAsync(MESSAGEBAR_SECTION, title: $"Your search returned more than {Constants.ItemsMaxNumber} results. Improve your filter.");
            }

            _SearchButtonLoading = false;
            await InvokeAsync(StateHasChanged);
        }
    }
}
