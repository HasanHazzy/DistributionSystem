function Purchase() {
    var $Purchase = this, Purchase = new Object(), _$ = new Extention();
    Products: [];
    PurchaseMaster = {
        Invoicetotal: 0,
        PurchaseInvoiceDetail: [],
        TotalDiscount: 0,
        TotalTax:0

    };
    
    DropDownData = {
        Products: [],
        Routes: [],
        Stock: [],
        Tax: [],
        Discount: []
    };
        

        

    $Purchase.GetDropDownData = function () {

        _$.Post('GetDropDownData', "", function (result) {
            switch (result.status) {
                case 100:

                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
                    break;

                case 200:
                    DropDownData = result.data;
                    Products = result.data.products;
                    $Purchase.FillDropDowns(DropDownData);
                    //$("#Custdropdown").chosen();

                    // $Extention.HideLoader();
                    // _$.Notification(result.message, result.status);
                    break;
                case 300:
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
                    break;

                case 400:
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
                    break;

                case 500:
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
                    break;
            }
        });

    };

   

    $Purchase.FillDropDowns = function (data) {
      
        var Stockdropdown = $("#Stockdropdown");
        Stockdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
        $.each(data.stock, function (index,stock) {
           // console.log(data.routes['routeId']);
            Stockdropdown.append($("<option></option>").val(stock.id).html(stock.descp));
        });
        $("#Stockdropdown").chosen();
        var Taxdropdown = $("#Taxdropdown");
        Taxdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
        $.each(data.tax, function (index, tax) {
            // console.log(data.routes['routeId']);
            Taxdropdown.append($("<option></option>").val(tax.percentage).html(tax.taxDescp));
        });

        var Discountdropdown = $("#Discountdropdown");
        Discountdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
        $.each(data.discount, function (index, discount) {
            // console.log(data.routes['routeId']);
            Discountdropdown.append($("<option></option>").val(discount.percentage).html(discount.descp));
        });

        var Productdropdown = $("#Productdropdown");
        Productdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
        $.each(data.products, function (index, product) {
            // console.log(data.routes['routeId']);
            Productdropdown.append($("<option></option>").val(product.itemid).html(product.itemdescp));
        });
        $("#Productdropdown").chosen();

    };

   

    $(document).ready(function () {

        $Purchase.GetDropDownData();
        //$Purchase.GetProducts();

        //$('#Stockdropdown').change(function () {
        //    var Productdropdown = $("#Productdropdown");
        //    Productdropdown.empty().append('<option selected="selected" value="0">Please select</option>');

        //    var selectedStockId = $(this).val();
        //    var StockProducts = DropDownData.products.filter(item => item.fk_StockId == selectedStockId);
        //    if (StockProducts.length == 0) {

        //        _$.Notification("No Product Available In This Stock", 500);
        //        return false;
        //    }
        //    //Productdropdown.append('<option selected="selected" value="0">Please select</option>');
        //    $.each(StockProducts,function (index, product) {
        //        // console.log(data.routes['routeId']);
        //        Productdropdown.append($("<option></option>").val(product.itemid).html(product.itemdescp));
        //    });
            

        //});
        



    });


    $Purchase.AddPurchaseDetails = function () {
        
        var Form = "#Purchaseform";
        var itemId = $(Form + ' #Productdropdown').val();
        var stockId = $(Form + ' #Stockdropdown').val();
        itemId = parseInt(itemId);
        var product_obj = Products.filter(x => x.itemid === itemId);
        var Quantity = $(Form + ' #Quantity').val();
        var unitPrice = product_obj[0].unitPrice;
        unitPrice = parseFloat(unitPrice);
        
        var Discount = $(Form + ' #Discountdropdown').val();
        Discount = parseFloat(Discount);
        var Tax = $(Form + ' #Taxdropdown').val();
        Tax = parseFloat(Tax);
        var total = parseFloat(unitPrice * Quantity);
        var taxAmount = $Purchase.CalculateTaxAmount(total,Tax);
        var discountAmount = $Purchase.CalculateDiscountAmount(total, Discount);
        taxAmount = taxAmount.toFixed(2);
        discountAmount = discountAmount.toFixed(2);
        if (total !== 0) {
            total -= parseFloat(taxAmount) + parseFloat(discountAmount);

        }

        var existingItem = PurchaseMaster.PurchaseInvoiceDetail.find(item => item.itemid === product_obj[0].itemid);
        total = parseFloat(total.toFixed(2));
        if (existingItem) {
            // Item already exists, update the quantity, total, and profit
            existingItem.Quantity = parseInt(existingItem.Quantity) + parseInt(Quantity);
            existingItem.Total += parseFloat(total);
                     

            //existingItem.Profit += profit;
        }
        else {
            PurchaseMaster.PurchaseInvoiceDetail.push({
                productCode: product_obj[0].productCode,
                itemdescp:product_obj[0].itemdescp,
                itemid: product_obj[0].itemid,
                Quantity: Quantity,
                unitcost: product_obj[0].unitPrice,
                Total: total,
                DiscountPercentage: Discount,
                TaxPercentage: Tax,
                Discount_Amount: discountAmount,
                Tax_Amount: taxAmount,
                FkStockId: stockId
            });
        }
        _$.Notification("Added " + product_obj[0].itemdescp, 200);
        $Purchase.MakePurchaseDetails();
        // _$.Notification(_$._messages.Add, 100);
        //ClearFieldsAddLifeInsurancePlans();

    };

    $Purchase.MakePurchaseDetails = function () {
        $('#Purchasetbl').DataTable().clear().draw();
        var total = 0;
        var totalDiscount = 0.00;
        var TotalTax = 0.00;
        
        for (var i = 0; i < PurchaseMaster.PurchaseInvoiceDetail.length; i++) {
            $('#Purchasetbl > tbody').append('<tr>' +
                '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].productCode + '</td>' +
                '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].itemdescp + '</td>' +
                '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].unitcost + '</td>' +
                '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].Quantity + '</td>' +
                '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].TaxPercentage + '% </td>' +
                '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].DiscountPercentage + '% </td>' +
                '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].Total + '</td>' +
                '<td>' + '<a href="#" id="' + PurchaseMaster.PurchaseInvoiceDetail[i].itemid + '" data-toggle="modal" class="btn btn-icon btn-danger waves-effect waves-light m-l-5"  onclick="$Purchase.DeleteItem(this.id)" rel="tooltip" data-original-title="Delete"><i class="fa fa-trash"></i></a>' + '</td>' +
                '</tr>');
            var subtotal = PurchaseMaster.PurchaseInvoiceDetail[i].Total;
           // var profit = SalesMaster.SaleInvoiceDetail[i].Profit;
            total += parseFloat(subtotal);
            TotalTax += parseFloat(PurchaseMaster.PurchaseInvoiceDetail[i].Tax_Amount);
            totalDiscount += parseFloat(PurchaseMaster.PurchaseInvoiceDetail[i].Discount_Amount);
        }

        PurchaseMaster.Invoicetotal = total;
        PurchaseMaster.TotalDiscount = totalDiscount;
        PurchaseMaster.TotalTax = TotalTax;
        var $tfoot = $('#Purchasetbl').find('tfoot');
        $tfoot.empty().append('<tr>' +
            '<th colspan="4"></th>' +
            '<th>TotalTax: ' + TotalTax.toFixed(2) + '</th>' +
            '<th>TotalDiscount:' + totalDiscount.toFixed(2) + '</th>' +
            '<th>Total:' + total.toFixed(2) + '</th>' +
            '</tr>');
        _$.DataTable("#Purchasetbl");
    };

    $Purchase.DeleteItem = function (id) {
        id = parseInt(id);
        for (var i = 0; i < PurchaseMaster.PurchaseInvoiceDetail.length; i++) {
            if (PurchaseMaster.PurchaseInvoiceDetail[i].itemid === id) {
                PurchaseMaster.PurchaseInvoiceDetail.splice(i, 1);
                _$.Notification(_$._messages.Delete, 200);
                $Purchase.MakePurchaseDetails();
                break;
            }
        }
    };

    $Purchase.CalculateTaxAmount = function (total, taxpercentage) {
        if (total !== null && taxpercentage !== null) {
            return total * (taxpercentage / 100);
        }
        return 0;
    };

    $Purchase.CalculateDiscountAmount = function (total, discountpercentage) {
        if (total !== null && discountpercentage !== null) {
            return total * (discountpercentage / 100);
        }
        return 0;
    };



  
    $Purchase.SavePurchaseInvoice = function () {

        //var customerId = $('#salesform #Custdropdown').val();
        //if (customerId === "0") {
        //    _$.Notification("Please Select Customer", 500);
        //    return false;

        //}
        //else {
        //    SalesMaster.Customerid = customerId;

        //}

        $Extention.ShowLoader();

        _$.Post('SavePurchaseInvoice', PurchaseMaster, function (result) {
            switch (result.status) {
                case 100:
                    //$Product.LoadProduct(result.Data);
                    _$.ResetAllContent("#ProductForm");
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);

                    break;

                case 200:
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
                    location.reload();
                    break;
                case 300:
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
                    break;

                case 400:
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
                    break;

                case 500:
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
                    break;
            }
        });

    };
    $Purchase.ShowModal = function (SaleInvoiceDetail) {
        var saleInvoiceDetail = JSON.parse(SaleInvoiceDetail);
        $('#salesdetailstbl tbody').empty();

        for (var i = 0; i < saleInvoiceDetail.length; i++) {
            $('#salesdetailstbl > tbody').append('<tr>' +
                '<td>' + saleInvoiceDetail[i].Saleinvoiceid + '</td>' +
                '<td>' + saleInvoiceDetail[i].Itemid + '</td>' +
                '<td>' + saleInvoiceDetail[i].Quantity + '</td>' +
                '<td>' + saleInvoiceDetail[i].CartonName + '</td>' +
                '<td>' + saleInvoiceDetail[i].Actualprice + '</td>' +
                '<td>' + saleInvoiceDetail[i].Unitcost + '</td>' +
                '<td>' + saleInvoiceDetail[i].Profit + '</td>' +
                '<td>' + saleInvoiceDetail[i].Total + '</td>' +
                '<td>' + saleInvoiceDetail[i].Time + '</td>' +

                '</tr>');
            $("#SalesDetailsModal").modal("show");

        }


    }
}







