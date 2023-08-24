//function Load() {
    var $Load = this, Load = new Object(), _$ = new Extention();
    Products: [];
    Tax: [];

    LoadMaster = {
        VehicleName: "",
        VehicleNo: "",
        DeliveryMan: "",
        DiscountFoc:0,
        DiscountHth:0,
        DiscountRegular:0,
        InvoiceTotal: 0,
        LoadInvoiceDetail: [],
        FkTaxId:0,
        Tax: 0

    };

    DropDownData = {
        Products: [],
        Routes: [],
        Stock: [],
        Tax: [],
        Discount: []
    };


    $(document).ready(function () {
        $Load.GetDropDownData();
        $Load.LoadTable();
        //$Purchase.GetProducts();

        $('#Stock_dropdown').change(function () {
            var Productdropdown = $("#Product_dropdown");
            Productdropdown.empty().append('<option selected="selected" value="0">Please select</option>');

            var selectedStockId = parseInt($(this).val());
            
            var StockProducts = DropDownData.products.filter(item => item.fK_StockId == selectedStockId);
            if (StockProducts.length == 0) {

                _$.Notification("No Product Available In This Stock", 500);
                return false;
            }

            //Productdropdown.append('<option selected="selected" value="0">Please select</option>');
            $.each(StockProducts, function (index, product) {
                // console.log(data.routes['routeId']);
                Productdropdown.append($("<option></option>").val(product.itemid).html("(" + product.productCode + ") " + product.itemdescp + " (Price) : " + product.unitPrice));

            });

            $("#Product_dropdown").chosen("destroy");
            $("#Product_dropdown").chosen();


        });
        $('#Product_dropdown').change(function () {
            var selectedStockId = $("#Stock_dropdown").val();

            var selectedProductId = $(this).val();
            var StockProducts = DropDownData.products.filter(item => item.itemid == selectedProductId && item.fK_StockId==selectedStockId);
            $("#CurrentStock").val(StockProducts[0].currentStock);

        });

    });


    $Load.GetDropDownData = function () {

        _$.Post('GetDropDownData', "", function (result) {
            switch (result.status) {
                case 100:

                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
                    break;

                case 200:
                    DropDownData = result.data;
                    Products = result.data.products;
                    Tax = result.data.tax;
                    
                    $Load.FillDropDowns(DropDownData);
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


    $Load.FillDropDowns = function (data) {

        var Stockdropdown = $("#Stock_dropdown");
        Stockdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
        $.each(data.stock, function (index, stock) {
            // console.log(data.routes['routeId']);
            Stockdropdown.append($("<option></option>").val(stock.id).html(stock.descp));
        });


        var Taxdropdown = $("#Load_Taxdropdown");
        Taxdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
        $.each(data.tax, function (index, tax) {
            // console.log(data.routes['routeId']);
            Taxdropdown.append($("<option></option>").val(tax.id).html(tax.taxDescp));
        });
     
        

        var Routedropdown = $("#Routedropdown");
        Routedropdown.empty().append('<option selected="selected" value="0">Please select</option>');
        $.each(data.routes, function (index, route) {
            // console.log(data.routes['routeId']);
            Routedropdown.append($("<option></option>").val(route.routeId).html(route.routeName));
        });

    };


    $Load.AddLoadDetails = function () {

        if ($Load.FormValidate()) {

            var Form = "#Loadform";
            var itemId = $(Form + ' #Product_dropdown').val();
            var stockId = $(Form + ' #Stock_dropdown').val();
            itemId = parseInt(itemId);
            debugger;
            var product_obj = Products.filter(x => x.itemid === itemId);
            var Quantity = $(Form + ' #Quantity').val();
            var unitPrice = product_obj[0].unitPrice;
            unitPrice = parseFloat(unitPrice);

            var total = parseFloat(unitPrice * Quantity);
            var existingItem = LoadMaster.LoadInvoiceDetail.find(item => item.FkItemId === product_obj[0].itemid);

            total = parseFloat(total.toFixed(2));
            if (existingItem) {
                // Item already exists, update the quantity, total, and profit
                existingItem.Quantity = parseInt(existingItem.Quantity) + parseInt(Quantity);
                existingItem.Total += parseFloat(total);


                //existingItem.Profit += profit;
            }
            else {
                LoadMaster.LoadInvoiceDetail.push({
                    productCode: product_obj[0].productCode,
                    itemdescp: product_obj[0].itemdescp,
                    FkItemId: product_obj[0].itemid,
                    Quantity: Quantity,
                    unitcost: product_obj[0].unitPrice,
                    Total: total,
                    FkStockId: stockId,
                    status: 0

                });
            }
            _$.Notification("Added " + product_obj[0].itemdescp, 200);
            if (!product_obj[0].margin) {
                product_obj[0].margin = 0;
            }
            $Load.MakeLoadDetails(product_obj[0].margin);
        }
        // _$.Notification(_$._messages.Add, 100);
        //ClearFieldsAddLifeInsurancePlans();

    };

$Load.MakeLoadDetails = function (marginpercentage) {
        $('#Loadtbl').DataTable().clear().draw();
        var InvoiceTotal = 0;
        var SubTotal = 0;
       // var RegularDiscount = parseFloat($("#RegularDiscount").val());
        //var HTHDiscount = parseFloat($("#DiscountHTH").val());
        //var FOCDiscount = parseFloat($("#DiscountFOC").val());
        //var TotalDiscount = RegularDiscount + HTHDiscount + FOCDiscount;
        //var taxAmount = 0;
        for (var i = 0; i < LoadMaster.LoadInvoiceDetail.length; i++) {
            $('#Loadtbl > tbody').append('<tr>' +
                '<td>' + LoadMaster.LoadInvoiceDetail[i].productCode + '</td>' +
                '<td>' + LoadMaster.LoadInvoiceDetail[i].itemdescp + '</td>' +
                '<td>' + LoadMaster.LoadInvoiceDetail[i].unitcost + '</td>' +
                '<td>' + LoadMaster.LoadInvoiceDetail[i].Quantity + '</td>' +
                '<td>' + LoadMaster.LoadInvoiceDetail[i].Total + '</td>' +
                '<td>' + '<a href="#" id="' + LoadMaster.LoadInvoiceDetail[i].FkItemId + '" data-toggle="modal" class="btn btn-icon btn-danger waves-effect waves-light m-l-5"  onclick="$Load.DeleteItem(this.id)" rel="tooltip" data-original-title="Delete"><i class="fa fa-trash"></i></a>' + '</td>' +
                '</tr>');
            SubTotal += LoadMaster.LoadInvoiceDetail[i].Total;
            var MarginAmount = $Load.CalculateMargin(LoadMaster.LoadInvoiceDetail[i].Total, marginpercentage);
            LoadMaster.LoadInvoiceDetail[i].Margin = MarginAmount;
            //taxAmount = $Load.CalculateTaxAmount(SubTotal, Tax[0].percentage);

            // var profit = SalesMaster.SaleInvoiceDetail[i].Profit;
        }

        var Tax_Id = parseInt($('#Load_Taxdropdown').val());
        Tax = Tax.filter(x => x.id === Tax_Id);

        InvoiceTotal += parseFloat(SubTotal);
       // InvoiceTotal -= TotalDiscount;
       // InvoiceTotal -= taxAmount;
       // taxAmount = taxAmount.toFixed(2);
        LoadMaster.Tax = 0;
        InvoiceTotal = InvoiceTotal.toFixed(2);
        LoadMaster.InvoiceTotal = InvoiceTotal;

       // LoadMaster.TotalDiscount = totalDiscount;
       // LoadMaster.TotalTax = TotalTax;
        var $tfoot = $('#Loadtbl').find('tfoot');
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
        _$.DataTable("#Loadtbl");
    };

    $Load.DeleteItem = function (id) {
        id = parseInt(id);
        for (var i = 0; i < LoadMaster.LoadInvoiceDetail.length; i++) {
            if (LoadMaster.LoadInvoiceDetail[i].FkItemId === id) {
                LoadMaster.LoadInvoiceDetail.splice(i, 1);
                _$.Notification(_$._messages.Delete, 200);
                $Load.MakeLoadDetails();
                break;
            }
        }
    };

    $Load.CalculateTaxAmount = function (total, taxpercentage) {
        if (total !== null && taxpercentage !== null) {
            return total * (taxpercentage / 100);
        }
        return 0;
    };
$Load.CalculateMargin = function (total, marginpercentage) {
    if (total !== null && marginpercentage !== null) {
        return total * (marginpercentage / 100);
    }
    return 0;
};

$Load.LoadTable = function () {
    var obj = $("#LoadTable");
    _$.DataTable(obj, null, null);
};
 
    $Load.SaveLoadInvoice = function () {

       
        var VehicleName = $("#VehicleName").val();
        var DeliveryMan = $("#DeliveryMan").val();
        var RouteId = parseInt($("#Routedropdown").val());
        var Fk_Tax_Id = parseInt($("#Load_Taxdropdown").val());
        LoadMaster.DiscountRegular = 0;
        LoadMaster.DiscountHth = 0;
        LoadMaster.DiscountFoc = 0;
        LoadMaster.VehicleName = VehicleName;
        LoadMaster.DeliveryMan = DeliveryMan;
        LoadMaster.FkRouteId = RouteId;
        LoadMaster.FkTaxId = Fk_Tax_Id;
        //$Extention.ShowLoader();

        _$.Post('SaveLoadInvoice', LoadMaster, function (result) {
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

$Load.FormValidate = function validateForm() {
    // Get form input values
    var vehicleName = $("#VehicleName").val().trim();
    var deliveryMan = $("#DeliveryMan").val().trim();
    var routeDropdown = $("#Routedropdown").val();
    var stockDropdown = $("#Stock_dropdown").val();
    var productDropdown = $("#Product_dropdown").val();
    var quantity = parseInt($("#Quantity").val());
    var taxDropdown = $("#Load_Taxdropdown").val();
    var CurrentStock = $("#CurrentStock").val();

    if (CurrentStock<=0) {
        _$.Notification("Curent Stock is "+CurrentStock, 500);
        return false;
    }
    // Perform validation checks
    if (vehicleName === "") {
        _$.Notification("Please enter Vehicle Name.", 500);
        return false;
    }

    if (deliveryMan === "") {
        _$.Notification("Please enter Delivery Man.", 500);
        return false;
    }

    if (routeDropdown === "0") {
        _$.Notification("Please select a Route.", 500);
        return false;
    }

    if (stockDropdown === "0") {
        _$.Notification("Please select a Stock.", 500);
        return false;
    }

    if (productDropdown === "0") {
        _$.Notification("Please select a Product.", 500);
        return false;
    }

    if (isNaN(quantity) || quantity <= 0) {
        _$.Notification("Please enter a valid Quantity.", 500);
        return false;
    }

    if (taxDropdown === "0") {
        _$.Notification("Please select a Tax.", 500);
        return false;
    }

    // All checks passed, form is valid
    return true;
}
//}







