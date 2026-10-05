using Clone.Models;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Clone.Web.Components.Pages
{
    public partial class Companies
    {
        FluentDataGrid<Company>? _grid;
        IQueryable<Company>? _queryableItems;
        private List<Company> _companies = [];
        private Company _company = new();
        private Company _companyDetails = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadCompaniesAsync();
        }

        private async Task LoadCompaniesAsync()
        {
            var repository = UnitOfWork.Repository<Company>();
            _companies = await repository.GetAllAsync();
            _queryableItems = _companies.AsQueryable();
        }

        private async Task SaveAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var repository = UnitOfWork.Repository<Company>();


            if (_company.Id == 0)
            {
                _company.InsertAudit(authState.User);
                await repository.AddAsync(_company);
            }
            else
            {
                _company.UpdateAudit(authState.User);
                await repository.UpdateAsync(_company);
            }

            await UnitOfWork.SaveAsync();
            await LoadCompaniesAsync();
            ResetForm();
        }

        private async Task EditAsync(Company company)
        {
            var toEdit = await UnitOfWork.Repository<Company>().GetByIdAsync(company.Id);
            if (toEdit is null)
            {
                return;
            }
            _company = toEdit;
        }

        private async Task DetailsAsync(Company company)
        {
            var toShow = await UnitOfWork.Repository<Company>().GetByIdAsync(company.Id);
            if(toShow is null)
            {
                return;
            }
            _companyDetails = toShow;
        }

        private async Task DeleteAsync(int id)
        {
            var repository = UnitOfWork.Repository<Company>();
            var toDelete = await repository.GetByIdAsync(id);

            if (toDelete is null)
            {
                return;
            }
            var result = await DialogService.ShowConfirmationAsync($"Are you <strong>sure</strong> you want to delete item Id: {toDelete.Id}?");
            if (result.Cancelled)
            {
                return;
            }

            await repository.RemoveAsync(toDelete);
            await UnitOfWork.SaveAsync();

            if (_company.Id == id)
            {
                ResetForm();
            }

            await LoadCompaniesAsync();
        }

        private void ResetForm()
        {
            _company = new Company();
        }

        protected override Task OnActionClickedAsync(bool primary)
        {
            throw new NotImplementedException();
        }
    }
}
