using Dima.Core.Handlers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Reports;

public partial class ExpensesByCategoryChartComponent : ComponentBase
{

    #region Properties

    public List<double> Data { get; set; } = [];
    public List<string> Labels { get; set; } = [];

    #endregion

    #region Services

    [Inject]
    public IReportHandler Handler { get; set; } = null!;
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides   

    protected override async Task OnInitializedAsync()
    {
        await GetExpensesByCategoryAsync();
    }

    private async Task GetExpensesByCategoryAsync()
    {
        var response = await Handler.GetExpensesByCategoryReport(new());
        if (!response.IsSuccess || response.Data == null)
        {
            Snackbar.Add("Não foi possível carregar o gráfico de despesas por categoria.", Severity.Error);
            return;
        }

        foreach (var item in response.Data)
        {
            Labels.Add($"{item.Category} ({item.Expenses:C})");
            Data.Add(-(double)item.Expenses);//gráfico só espera valor positivo, por isso precisa inverter o valor
        }

    }

    #endregion

}
