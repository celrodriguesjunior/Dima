using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Orders;

public partial class CheckoutPage : ComponentBase
{

    #region Parameters

    [Parameter]
    public string Slug { get; set; } = string.Empty;
    [SupplyParameterFromQuery(Name = "voucher")]
    public string? VoucherNumber { get; set; }

    #endregion

    #region Properties

    public PatternMask Mask { get; set; } = new("####-####")
    {
        MaskChars = [new MaskChar('#', @"[0-9a-fA-F]")],
        Placeholder = '_',
        CleanDelimiters = true,
        Transformation = AllUpperCase
    };

    public bool IsBusy { get; set; }
    public bool IsValid { get; set; }
    public CreateOrderRequest InputModel { get; set; } = new();
    public Product? Product { get; set; }
    public Voucher? Voucher { get; set; }
    public decimal Total { get; set; }

    #endregion

    #region Services

    [Inject] public IProductHandler ProductHandler { get; set; } = null!;
    [Inject] public IOrderHandler OrderHandler { get; set; } = null!;
    [Inject] public IVoucherHandler VoucherHandler { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var result = await ProductHandler.GetBySlugAsync(new GetProductBySlugRequest { Slug = Slug });
            if (result.IsSuccess == false)
            {
                Snackbar.Add(result.Message, Severity.Error);
                IsValid = false;
                return;
            }

            Product = result.Data;
        }
        catch (Exception)
        {
            Snackbar.Add("Não foi possível carregar o produto.", Severity.Error);
            IsValid = false;
            return;
        }

        if (Product is null)
        {
            Snackbar.Add("Produto não encontrado.", Severity.Error);
            IsValid = false;
            return;
        }

        if (string.IsNullOrEmpty(VoucherNumber))
        {
            try
            {
                var result = await VoucherHandler.GetByCodeAsync(new GetVoucherByNumberRequest() { Number = VoucherNumber?.Replace("-", "") ?? "" });
                if (result.IsSuccess == false)
                {
                    VoucherNumber = string.Empty;
                    Snackbar.Add(result.Message, Severity.Error);
                }

                if (result.Data is null)
                {
                    VoucherNumber = string.Empty;
                    Snackbar.Add("Voucher não encontrado.", Severity.Error);
                }

                Voucher = result.Data;
            }
            catch (Exception)
            {
                Snackbar.Add("Não foi possível carregar o voucher.", Severity.Error);
                VoucherNumber = string.Empty;
            }
        }

        IsValid = true;
        Total = Product.Price - (Voucher?.Amount ?? 0);

    }

    #endregion

    private static char AllUpperCase(char c) => c.ToString().ToUpperInvariant()[0];

}
