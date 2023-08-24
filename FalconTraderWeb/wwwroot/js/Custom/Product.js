//function Product() {
var $Product = this, Product = new Object(), _$ = new Extention();

$Product.SaveProduct = function () {

    if ($Product.ValidateForm()) {
        $.each($('#Products').serializeArray(), function (i, field) {
            Product[field.name] = field.value || null;
        });

        //Product.Id = $('#ProductForm #Id').val();

        $Extention.ShowLoader();

        _$.Post('SaveProduct', Product, function (result) {
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

$Product.UpdateProduct = function () {
    //if ($("#ProductmdlBody").valid()) {
    $.each($('#ProductmdlForm').serializeArray(), function (i, field) {
        Product[field.name] = field.value || null;
    });

    //Product.Id = $('#ProductForm #Id').val();

    $Extention.ShowLoader();

    _$.Post('UpdateProduct', Product, function (result) {
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
                $("#ProductModal").modal("hide");
                $Product.FetchProducts();

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
    // }
};

$Product.MakeProductTable = function (products) {
    var data_table = $("#Producttbl").DataTable();
    data_table.clear();


    $.each(products, function (index, product) {
        var marginValue = product.margin !== null ? product.margin : 0;

        data_table.row.add([
            product.itemid,
            product.productCode,
            product.itemdescp,
            product.unitPrice,
            marginValue,
            '<div>' +
            '<a href="#" onclick="$Product.ShowModal(this);" data-id="' + product.itemid + '">' +
            '<span class="feather" data-feather="edit"></span>' +
            '</a>' +
            '<a href="#" onclick="$Product.DeleteProduct(' + product.itemid + ');" data-id="' + product.itemid + '">' +
            '<span class="feather" data-feather="trash-2"></span>' +
            '</a>' +
            '</div>'
        ]);

    });

    data_table.draw();
    feather.replace();
    data_table.on('draw.dt', function () {
        feather.replace();
    });

};


$Product.FetchProducts = function () {

    _$.Post('GetProducts', Product, function (result) {
        switch (result.status) {
            case 100:
                //$Product.LoadProduct(result.Data);
                // _$.ResetAllContent("#ProductForm");
                //$Extention.HideLoader();
                _$.Notification(result.message, result.status);

                break;

            case 200:
                $Extention.HideLoader();
                // _$.Notification(result.message, result.status);
                MakeProductTable(result.data);

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

$Product.ShowModal = function (control) {
    var row = $(control).closest('tr');
    var itemId = row.find('td:eq(0)').text();
    var ProductCode = row.find('td:eq(1)').text();
    var itemDescp = row.find('td:eq(2)').text();
    var unitprice = row.find('td:eq(3)').text();
    var margin = row.find('td:eq(4)').text();

    $("#ProductmdlForm #Itemid").val(itemId);
    $("#ProductmdlForm #ProductCode").val(ProductCode);
    $("#ProductmdlForm #Itemdescp").val(itemDescp);
    $("#ProductmdlForm #UnitPrice").val(unitprice);
    $("#ProductmdlForm #Margin").val(margin);

    $("#ProductModal").modal("show");

};
$Product.DelProduct = function (itemId) {

    Product["Itemid"] = itemId;
    _$.Post('DeleteProduct', Product, function (result) {
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
                $Product.FetchProducts();
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
$Product.DeleteProduct = function (itemId) {

    $.confirm({
        title: 'Confirm Delete',
        content: 'Are you sure you want to delete this Product?',
        type: 'green',
        typeAnimated: true,
        buttons: {
            yes: function () {
                $Product.DelProduct(itemId);
            },
            no: function () {
            }
        }
    });
};
$Product.ValidateForm = function () {
    // Get the values of the input fields
    const productCode = $("#ProductCode").val().trim();
    const productName = $("#Itemdescp").val().trim();
    const productPrice = $("#UnitPrice").val().trim();
    const Margin = $("#Margin").val().trim();
    // Check if any of the required fields are empty
    if (!productCode || !productName || !productPrice || !Margin) {
        _$.Notification("Please fill in all the required fields.", 500);
        return false; // Prevent form submission
    }
    return true;
};



$(document).ready(function () {

    $Product.FetchProducts();

});




