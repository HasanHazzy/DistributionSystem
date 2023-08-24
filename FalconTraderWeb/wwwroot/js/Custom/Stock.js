//function Stock() {
var $Stock = this, Stock = new Object(), _$ = new Extention();
Products: [];
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

$Stock.SaveStockReturn = function () {

    Stock["FkStockId"] = $("#Stock_Return_dropdown").val();
    Stock["FkItemId"] = $("#Stock_Return_Product_dropdown").val();
    Stock["Quantity"] = $("#Quantity").val();
    Stock["FkLoadInvoiceId"] = $("#LoadInvoiceNo").val();
    //Product.Id = $('#ProductForm #Id').val();

    $Extention.ShowLoader();

    _$.Post('SaveStockReturn', Stock, function (result) {
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

//}

