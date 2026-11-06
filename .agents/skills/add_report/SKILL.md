---
name: add-report
description: Create a UI from a specific `DTO` of `src/Clone.Models/Dtos`. Save the Report page at `src/Clone.Web.Components.Pages/Admin/Reports` folder. Use the Templates, replace the [Entity] name in the template to corresponding definition. Example: if {Entity Singular Name} you write the entity name at singular name, if {Entity Plural Name} you write the entity name at plural name, etc. If see {SPECIAL_CASE: } implemented the code descript. Add Localization.
argument-hint: "[EntityName]"
---

# Template

Create into de folder `src/Clone.Web.Components.Pages/Admin/Reports/{EntityName}.razor` file with the follow code
```csharp
@page "/reports/{Entity Name LowerCase Without Dto suffix}"
@rendermode InteractiveServer

@using Clone.DataAccess.Repositories.IRepository
@using Clone.Models.Dtos
@using Clone.Utility
@using Clone.Web.Components.Shared
@using Microsoft.AspNetCore.Authorization
@using Microsoft.AspNetCore.Components.Authorization

@inject AuthenticationStateProvider AuthenticationStateProvider
@inject ILogger<{Entity Name Without Dto Suffix}> Logger
@inject INotificationService NotificationService
@inject IUnitOfWork UnitOfWork
@inject IStringLocalizer<SharedResource> SharedLocalizer
@inject IStringLocalizer<{Entity Name Without Dto Suffix}> Localizer

@attribute [Authorize(Roles = RD.RoleAdmin)]

<PageTitle>@Localizer["Title"]</PageTitle>
<FluentMessageBarProvider Section="@MESSAGEBAR_SECTION" Margin="@Margin.Vertical2" />
<FluentDialogProvider />
<FluentLabel Size="@LabelSize.Large">@Localizer["Label"]</FluentLabel>

<FluentKeyCode OnKeyDown="@KeyDownHandler">
    <FluentStack Orientation="Orientation.Horizontal" HorizontalGap="4">
        <DateRangePicker @bind-Value="_range" OnRangeChanged="OnRangeChanged" />
        <FluentButton Title=@SharedLocalizer["Common.Search"]"
                      IconStart="@(new Icons.Regular.Size20.DatabaseSearch())"
                      Appearance="ButtonAppearance.Default"
                      OnClick="@OnSearchClicked"
                      Loading="@_SearchButtonLoading">@SharedLocalizer["Common.Search"]</FluentButton>
    </FluentStack>
</FluentKeyCode>
<FluentDataGrid Items="@_ReportItems" GridTemplateColumns="2fr 1fr 1fr" Style="width: 800px;">
    <ChildContent>
        <PropertyColumn Title="@SharedLocalizer["Common.Name"]" Property="@(c => c!.CostCenterName)" Align="DataGridCellAlignment.Start" />
        <PropertyColumn Title="@Localizer["Column.Count"]" Property="@(c => c!.TotalCount)" Align="DataGridCellAlignment.End" />
        <PropertyColumn Title="@Localizer["Column.SubTotal"]" Property="@(c => c!.SubTotal)" Align="DataGridCellAlignment.End" />
    </ChildContent>
    <EmptyContent>
        <FluentStack Orientation="Orientation.Horizontal" VerticalAlignment="VerticalAlignment.Center" HorizontalAlignment="HorizontalAlignment.Center" HorizontalGap="10">
            <FluentIcon Value="@(new Icons.Filled.Size24.Crown())" Color="@Color.Primary" />
            <div>@SharedLocalizer["Message.NoData"]</div>
        </FluentStack>
    </EmptyContent>
</FluentDataGrid>

<p style="font-weight:bold; padding:5px;">
    @(_TotalSearched == 1 ? SharedLocalizer["Common.Item", _TotalSearched] : SharedLocalizer["Common.Items", _TotalSearched])
</p>


@code {

    private const string MESSAGEBAR_SECTION = "MESSAGEBAR_RECEIPT";

    private FluentDataGrid<{EntityName}>? _Grid;
    IQueryable<{EntityName}> _ReportItems { get; set; } = default!;

    #region Search
    private DateRangeDto _range = DateRangeDto.Today();
    private bool _SearchButtonLoading = false;
    private int _TotalSearched = 0;
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

    private async Task OnSearchClicked()
    {
        await SearchData();
    }

    private void OnRangeChanged(DateRangeDto range) => _range = range;

    private async Task SearchData()
    {
        _SearchButtonLoading = true;

        if (_range.From > _range.To)
            return;

        try
        {
            var findeds = await UnitOfWork.{Entity Name Without Dto Suffix}ReportAsync(_range.From, _range.To);

            _ReportItems = findeds.AsQueryable();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error searching {Entity Name Without Dto Suffix}.");
            await NotificationService.ShowErrorBarAsync(MESSAGEBAR_SECTION, title: Localizer["Error.Search"]);
        }
        finally
        {
            _SearchButtonLoading = false;
            await InvokeAsync(StateHasChanged);
        }
    }
}

```