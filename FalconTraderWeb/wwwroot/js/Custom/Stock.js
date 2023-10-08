//function Stock() {
var $Stock = this, Stock = new Object(), _$ = new Extention();
Products: [];
var StockReturnli= [];
$(document).ready(function () {
    $Stock.GetDropDownData();
    $Stock.LoadTable();
    $('#Stock_Return_dropdown').change(function () {
        var Productdropdown = $("#Stock_Return_Product_dropdown");
        Productdropdown.empty().append('<option selected="selected" value="0">Please select</option>');

        var selectedStockId = $(this).val();
        var StockProducts = Products.filter(item => item.fK_StockId == selectedStockId);
        if (StockProducts.length == 0) {

            _$.Notification("No Product Available In This Stock", 500);
            return false;
        }
        $.each(StockProducts, function (index, product) {
            // console.log(data.routes['routeId']);
            Productdropdown.append($("<option></option>").val(product.itemid).html(product.itemdescp));
        });
        $("#Productdropdown").chosen();

    });

    $('#Stock_Return_Product_dropdown').change(function () {
        var selectedStockId = $("#Stock_Return_dropdown").val();

        var selectedProductId = $(this).val();
        var StockProducts = Products.filter(item => item.itemid == selectedProductId && item.fK_StockId == selectedStockId);
        $("#ProductCurrentStock").val(StockProducts[0].currentStock);

    });
});

$Stock.LoadTable = function () {
    var obj = $("#stocktable");
    _$.DataTable(obj, null, null);
};
$Stock.GetDropDownData = function () {

    _$.Post('StockGetDropDownData', "", function (result) {
        switch (result.status) {
            case 100:

                $Extention.HideLoader();
                _$.Notification(result.message, result.status);
                break;

            case 200:

                Products = result.data.products;

                $Stock.FillDropDowns(result.data.stock);

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

$Stock.ShowModal = function (control) {
    var row = $(control).closest('tr');
    var Id = row.find('td:eq(0)').text();
    var Descp = row.find('td:eq(1)').text();
    var Symbol = row.find('td:eq(2)').text();
    $("#StocksmdlForm #Stock_Id").val(Id);
    $("#StocksmdlForm #Update_Descp").val(Descp);
    $("#StocksmdlForm #Update_Symbol").val(Symbol);
    $("#StocksModal").modal("show");

};
$Stock.FillDropDowns = function (data) {
    var Stockdropdown = $("#Stock_Return_dropdown");
    Stockdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
    $.each(data, function (index, stock) {
        // console.log(data.routes['routeId']);
        Stockdropdown.append($("<option></option>").val(stock.id).html(stock.descp));
    });

};


$Stock.SaveStocks = function () {

    if ($Stock.ValidateForm()) {

        Stock["Descp"] = $("#Descp").val();
        Stock["Symbol"] = $("#Symbol").val();

        $Extention.ShowLoader();

        _$.Post('SaveStocks', Stock, function (result) {
            switch (result.status) {
                case 100:
                    //$Product.LoadProduct(result.Data);
                    // _$.ResetAllContent("#ProductForm");
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
                    location.reload();
                    break;

                case 200:
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
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
    }

};
$Stock.ValidateForm = function () {

    const Descp = $("#Descp").val().trim();
    const Symbol = $("#Symbol").val().trim();
    if (!Descp || !Symbol) {
        _$.Notification("Please fill in all the required fields.", 500);
        return false; 
    }
    return true;

};
$Stock.UpdateStock = function () {
    Stock["Id"] = $("#Stock_Id").val();
    Stock["Descp"] = $("#Update_Descp").val();
    Stock["Symbol"] = $("#Update_Symbol").val();

    $Extention.ShowLoader();

    _$.Post('UpdateStock', Stock, function (result) {
        switch (result.status) {
            case 100:
                //$Product.LoadProduct(result.Data);
                // _$.ResetAllContent("#ProductForm");
                $Extention.HideLoader();
                _$.Notification(result.message, result.status);
                location.reload();
                break;

            case 200:
                $Extention.HideLoader();
                _$.Notification(result.message, result.status);
                $("#StocksmdlForm").modal("hide");
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

$Stock.DeleteStock = function (element) {
    var Id = $(element).data('id');
    $.confirm({
        title: 'Confirm Delete',
        content: 'Are you sure you want to delete this WareHouse?',
        type: 'green',
        typeAnimated: true,
        buttons: {
            yes: function () {
                $Stock.DelStock(Id);
            },
            no: function () {
            }
        }
    });
};

$Stock.DelStock = function (itemId) {

    Stock["Id"] = itemId;
    _$.Post('DeleteStock', Stock, function (result) {
        switch (result.status) {
            case 100:
                //$Product.Itemid(result.Data);
                // _$.ResetAllContent("#ProdutForm");
                //$Extention.HideLoader();
                _$.Notification(result.message, result.status);

                break;

            case 200:
                $Extention.HideLoader();
                _$.Notification(result.message, result.status);
                // MakeProductTable(result.data);
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
$Stock.SaveStockReturn = function () {

    $Extention.ShowLoader();

    _$.Post('SaveStockReturn', StockReturnli, function (result) {
        switch (result.status) {
            case 100:
                //$Product.LoadProduct(result.Data);
                // _$.ResetAllContent("#ProductForm");
                $Extention.HideLoader();
                _$.Notification(result.message, result.status);
                break;

            case 200:
                $Extention.HideLoader();
                _$.Notification(result.message, result.status);
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

$Stock.AddStockReturnDetails = function () {
    if ($Stock.ValidateForm()) {
        var loadInvoiceNo= $("#LoadInvoiceNo").val();
        var Form = "#StockReturnform";
        var itemId = $(Form + ' #Stock_Return_Product_dropdown').val();
        var stockId = $(Form + ' #Stock_Return_dropdown').val();
        itemId = parseInt(itemId);

        var product_obj = Products.filter(x => x.itemid === itemId);
        var Quantity = $(Form + ' #Quantity').val();
        var unitPrice = product_obj[0].unitPrice;
        unitPrice = parseFloat(unitPrice);
        var total = parseFloat(unitPrice * Quantity);
        var existingItem = StockReturnli.find(item => item.FkItemId === product_obj[0].itemid);
        var Note = $("#Note").val();

        total = parseFloat(total.toFixed(2));
        if (existingItem) {
            // Item already exists, update the quantity, total, and profit
            existingItem.Quantity = parseInt(existingItem.Quantity) + parseInt(Quantity);
            existingItem.Total += parseFloat(total);


            //existingItem.Profit += profit;
        }
        else {
            StockReturnli.push({
                productCode: product_obj[0].productCode,
                itemdescp: product_obj[0].itemdescp,
                FkItemId: product_obj[0].itemid,
                Quantity: Quantity,
                unitcost: product_obj[0].unitPrice,
                FkLoadInvoiceId: loadInvoiceNo,
                Note:Note,
                Total: total,
                FkStockId: stockId,
                status: 0

            });
        }
        _$.Notification("Added " + product_obj[0].itemdescp, 200);
        if (!product_obj[0].margin) {
            product_obj[0].margin = 0;
        }
        $Stock.MakeStockReturnDetails(product_obj[0].margin);
    }
    
};
$Stock.MakeStockReturnDetails = function (marginpercentage) {
    $('#StockReturntbl').DataTable().clear().draw();
    var InvoiceTotal = 0;
    var SubTotal = 0;
    for (var i = 0; i < StockReturnli.length; i++) {
        $('#StockReturntbl > tbody').append('<tr>' +
            '<td>' + StockReturnli[i].productCode + '</td>' +
            '<td>' + StockReturnli[i].itemdescp + '</td>' +
            '<td>' + StockReturnli[i].unitcost + '</td>' +
            '<td>' + StockReturnli[i].Quantity + '</td>' +
            '<td>' + StockReturnli[i].Total + '</td>' +
            '<td>' + '<a href="#" id="' + StockReturnli[i].FkItemId + '" data-toggle="modal" class="btn btn-icon btn-danger waves-effect waves-light m-l-5"  onclick="$Stock.DeleteItem(this.id)" rel="tooltip" data-original-title="Delete"><i class="fa fa-trash"></i></a>' + '</td>' +
            '</tr>');
        SubTotal += StockReturnli[i].Total;
       
    }

    InvoiceTotal += parseFloat(SubTotal);
    InvoiceTotal = InvoiceTotal.toFixed(2);

    // LoadMaster.TotalDiscount = totalDiscount;
    // LoadMaster.TotalTax = TotalTax;
    var $tfoot = $('#StockReturntbl').find('tfoot');
    $tfoot.empty().append('<tr>' +
        '<th colspan="4"> </th>' +
        '<th>Sub Total: ' + SubTotal +
        '</th> <th></th>' + '</tr>');
    //$tfoot.append('<tr>' +
    //    '<th colspan="4"> </th>' +
    //    '<th>Total Discount: ' + TotalDiscount +
    //    '</th> <th></th>' + '</tr>');
    //$tfoot.append('<tr>' +
    //    '<th colspan="4"> </th>' +
    //    '<th>Total Tax: ' + taxAmount +
    //    '</th> <th></th>' + '</tr>');
    $tfoot.append('<tr>' +
        '<th colspan="4"> </th>' +
        '<th>Invoice Total: ' + InvoiceTotal +
        '</th> <th></th>' + '</tr>');
    //$tfoot.append('<tr>' +
    //    '<th colspan="4">' +
    //    'TotalDiscount: ' + total +
    //    '</th>' + '</tr>');
    _$.DataTable("#StockReturntbl");
};

$Stock.ValidateForm=function()  {
    var isValid = true;

    // Reset any previous error messages
    $(".error-message").remove();

    // Validate Select Warehouse
    var warehouseValue = $("#Stock_Return_dropdown").val();
    if (warehouseValue === "") {
        isValid = false;
        $("#Stock_Return_dropdown").after('<p class="error-message">Please select a warehouse</p>');
    }

    // Validate other form fields as needed
    // Example:
    var quantityValue = $("#Quantity").val();
    if (quantityValue < 1) {
        isValid = false;
        $("#Quantity").after('<p class="error-message">Quantity must be at least 1</p>');
    }

    var loadInvoiceNoValue = $("#LoadInvoiceNo").val();
    if (!/^\d+$/.test(loadInvoiceNoValue)) {
        isValid = false;
        $("#LoadInvoiceNo").after('<p class="error-message">Invoice number must contain only digits</p>');
    }

    var loadDateValue = $("#LoadDate").val();
    if (loadDateValue === "") {
        isValid = false;
        $("#LoadDate").after('<p class="error-message">Please select a load date</p>');
    }

    return isValid;
}
$Stock.DeleteItem = function (id) {
    id = parseInt(id);
    for (var i = 0; i < StockReturnli.length; i++) {
        if (StockReturnli[i].FkItemId === id) {
            StockReturnli.splice(i, 1);
            _$.Notification(_$._messages.Delete, 200);
            $Stock.MakeStockReturnDetails();
            break;
        }
    }
};
       



//}

