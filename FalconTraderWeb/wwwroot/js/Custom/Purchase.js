//function Purchase() {
var $Purchase = this, Purchase = new Object(), _$ = new Extention();
Products: [];
PurchaseMaster = {
    Invoicetotal: 0,
    PurchaseInvoiceDetail: [],
    TotalDiscount: 0,
    TotalTax: 0

};

PurchaseEditMaster = {
    Invoicetotal: 0,
    PurchaseInvoiceDetail: [],
    TotalDiscount: 0,
    TotalTax: 0

};
PurchaseInvoiceModel = {
    Purchaseinvoiceid: 0,
    Invoicetotal: 0,
    Date: null,
    TotalDiscount: 0,
    TotalTax: 0,
    Status: 0,
    CokeInvoice: 0,
    DeliveryDate: 0,
    InvoiceDetailModel: []
};
DropDownData = {
    Products: [],
    Routes: [],
    Stock: [],
    Tax: [],
    Discount: []
};


$(document).ready(function () {
    $Purchase.GetDropDownData();

});

$Purchase.DeletePurchase = function () {
    var PurchaseInvoiceNo = $("#PurchaseInvoiceNo").val();
    var PurchaseInvoiceObj = {};
    PurchaseInvoiceObj["Purchaseinvoiceid"] = PurchaseInvoiceNo;
    _$.Post('DeletePurchaseInvoice', PurchaseInvoiceObj, function (result) {
        switch (result.status) {
            case 100:

                $Extention.HideLoader();
                _$.Notification(result.message, result.status);
                break;

            case 200:

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

                $("#Productdropdown").chosen();
                //$("#EditProductdropdown").chosen();

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
    var EditStockdropDown = $("#EditStockdropdown");
    Stockdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
    EditStockdropDown.empty().append('<option selected="selected" value="0">Please select</option>');

    $.each(data.stock, function (index, stock) {
        // console.log(data.routes['routeId']);
        Stockdropdown.append($("<option></option>").val(stock.id).html(stock.descp));
        EditStockdropDown.append($("<option></option>").val(stock.id).html(stock.descp));

    });
    $("#Stockdropdown").chosen();
    $("#EditStockdropDown").chosen();

    var Taxdropdown = $("#Taxdropdown");
    var EditTaxdropdown = $("#EditTaxdropdown");

    Taxdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
    EditTaxdropdown.empty().append('<option selected="selected" value="0">Please select</option>');

    $.each(data.tax, function (index, tax) {
        // console.log(data.routes['routeId']);
        Taxdropdown.append($("<option></option>").val(tax.percentage).html(tax.taxDescp + " (" + tax.percentage + "%)"));
        EditTaxdropdown.append($("<option></option>").val(tax.percentage).html(tax.taxDescp + " (" + tax.percentage + "%)"));

    });

    var Discountdropdown = $("#Discountdropdown");
    var EditDiscountdropdown = $("#EditDiscountdropdown");

    Discountdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
    EditDiscountdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
    $.each(data.discount, function (index, discount) {
        // console.log(data.routes['routeId']);
        Discountdropdown.append($("<option></option>").val(discount.percentage).html(discount.descp + "(" + discount.percentage + "%)"));
        EditDiscountdropdown.append($("<option></option>").val(discount.percentage).html(discount.descp + "(" + discount.percentage + "%)"));

    });

    var Productdropdown = $("#Productdropdown");
    var EditProductdropdown = $("#EditProductdropdown");

    Productdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
    EditProductdropdown.empty().append('<option selected="selected" value="0">Please select</option>');
    $.each(data.products, function (index, product) {
        // console.log(data.routes['routeId']);
        Productdropdown.append($("<option></option>").val(product.itemid).html("(" + product.productCode + ") " + product.itemdescp + " (Price) : " + product.unitPrice));
        EditProductdropdown.append($("<option></option>").val(product.itemid).html("(" + product.productCode + ") " + product.itemdescp + " (Price) : " + product.unitPrice));

    });
   
};



$Purchase.AddPurchaseDetails = function () {
    if ($Purchase.ValidateForm()) {

        var productotal = 0;
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
        var producttotal = total;
        var taxAmount = $Purchase.CalculateTaxAmount(total, Tax);
        var discountAmount = $Purchase.CalculateDiscountAmount(total, Discount);
        taxAmount = taxAmount.toFixed(2);
        discountAmount = discountAmount.toFixed(2);
        if (total !== 0) {
            total -= parseFloat(discountAmount);
            total += parseFloat(taxAmount);

        }

        var existingItem = PurchaseMaster.PurchaseInvoiceDetail.find(item => item.itemid === product_obj[0].itemid);
        total = parseFloat(total.toFixed(2));
        if (existingItem) {
            // Item already exists, update the quantity, total, and profit
            existingItem.Quantity = parseInt(existingItem.Quantity) + parseInt(Quantity);
            existingItem.producttotal = parseFloat(unitPrice * existingItem.Quantity);
            var prtotal = parseFloat(existingItem.producttotal);
            taxAmount = $Purchase.CalculateTaxAmount(prtotal, Tax);
            discountAmount = $Purchase.CalculateDiscountAmount(prtotal, Discount);

            taxAmount = taxAmount.toFixed(2);
            discountAmount = discountAmount.toFixed(2);
            prtotal -= parseFloat(discountAmount);
            prtotal += parseFloat(taxAmount);

            existingItem.DiscountAmount = parseFloat(discountAmount);
            existingItem.TaxAmount = parseFloat(taxAmount);
            existingItem.Total = parseFloat(prtotal);
            existingItem.DiscountPercentage = Discount;
            //existingItem.Profit += profit;
        }
        else {
            PurchaseMaster.PurchaseInvoiceDetail.push({
                productCode: product_obj[0].productCode,
                itemdescp: product_obj[0].itemdescp,
                itemid: product_obj[0].itemid,
                Quantity: Quantity,
                unitcost: product_obj[0].unitPrice,
                producttotal: producttotal,
                Total: total,
                TaxPercentage: Tax,
                DiscountAmount: parseFloat(discountAmount),
                DiscountPercentage: Discount,
                TaxAmount: parseFloat(taxAmount),
                FkStockId: stockId
            });
        }
        _$.Notification("Added " + product_obj[0].itemdescp, 200);
        $Purchase.MakePurchaseDetails();
    }
    // _$.Notification(_$._messages.Add, 100);
    //ClearFieldsAddLifeInsurancePlans();

};

$Purchase.AddEditPurchaseDetails = function () {
    if ($Purchase.EditValidateForm()) {

        var Form = "#PurchaseEditForm";
        var itemId = $(Form + ' #EditProductdropdown').val();
        var stockId = $(Form + ' #EditStockdropdown').val();
        itemId = parseInt(itemId);
        var product_obj = Products.filter(x => x.itemid === itemId);
        var Quantity = $(Form + ' #EditQuantity').val();
        var unitPrice = product_obj[0].unitPrice;
        unitPrice = parseFloat(unitPrice);

        var Discount = $(Form + ' #EditDiscountdropdown').val();
        Discount = parseFloat(Discount);
        var Tax = $(Form + ' #EditTaxdropdown').val();
        Tax = parseFloat(Tax);
        var total = parseFloat(unitPrice * Quantity);
        var producttotal = total;
        var taxAmount = $Purchase.CalculateTaxAmount(total, Tax);
        var discountAmount = $Purchase.CalculateDiscountAmount(total, Discount);
        taxAmount = taxAmount.toFixed(2);
        discountAmount = discountAmount.toFixed(2);
        if (total !== 0) {
            total -= parseFloat(discountAmount);
            total += parseFloat(taxAmount);

        }

        var existingItem = PurchaseEditMaster.PurchaseInvoiceDetail.find(item => item.itemid === product_obj[0].itemid);
        total = parseFloat(total.toFixed(2));
        if (existingItem) {
            // Item already exists, update the quantity, total, and profit
            existingItem.Quantity = parseInt(existingItem.Quantity) + parseInt(Quantity);
            existingItem.producttotal = parseFloat(unitPrice * existingItem.Quantity);
            var prtotal = parseFloat(existingItem.producttotal);
            taxAmount = $Purchase.CalculateTaxAmount(prtotal, Tax);
            discountAmount = $Purchase.CalculateDiscountAmount(prtotal, Discount);

            taxAmount = taxAmount.toFixed(2);
            discountAmount = discountAmount.toFixed(2);
            prtotal -= parseFloat(discountAmount);
            prtotal += parseFloat(taxAmount);

            existingItem.DiscountAmount = parseFloat(discountAmount);
            existingItem.TaxAmount = parseFloat(taxAmount);
            existingItem.Total = parseFloat(prtotal);
            existingItem.DiscountPercentage = Discount;

            //existingItem.Profit += profit;
        }
        else {
            PurchaseEditMaster.PurchaseInvoiceDetail.push({
                productCode: product_obj[0].productCode,
                itemdescp: product_obj[0].itemdescp,
                Itemid: product_obj[0].itemid,
                Quantity: Quantity,
                unitcost: product_obj[0].unitPrice,
                producttotal: producttotal,
                Total: total,
                TaxPercentage: Tax,
                DiscountAmount: parseFloat(discountAmount),
                TaxAmount: parseFloat(taxAmount),
                DiscountPercentage: Discount,
                FkStockId: stockId
            });
        }
        _$.Notification("Added " + product_obj[0].itemdescp, 200);
        $Purchase.EditMakePurchaseDetails();
    }
}
$Purchase.MakePurchaseDetails = function () {
    $('#Purchasetbl').DataTable().clear().draw();
    var total = 0;
    var totalDiscount = 0.00;
    var TotalTax = 0.00;
    var TotalQuantity = 0;
    var ProductTotal = 0.0;
    for (var i = 0; i < PurchaseMaster.PurchaseInvoiceDetail.length; i++) {
        $('#Purchasetbl > tbody').append('<tr>' +
            '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].productCode + '</td>' +
            '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].itemdescp + '</td>' +
            '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].unitcost + '</td>' +
            '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].Quantity + '</td>' +
            '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].producttotal + '</td>' +
            '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].TaxAmount + '</td>' +
            '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].DiscountAmount + '</td>' +
            '<td>' + PurchaseMaster.PurchaseInvoiceDetail[i].Total + '</td>' +
            '<td>' + '<a href="#" id="' + PurchaseMaster.PurchaseInvoiceDetail[i].itemid + '" data-toggle="modal" class="btn btn-icon btn-danger waves-effect waves-light m-l-5"  onclick="$Purchase.DeleteItem(this.id)" rel="tooltip" data-original-title="Delete"><i class="fa fa-trash"></i></a>' + '</td>' +
            '</tr>');
        var subtotal = PurchaseMaster.PurchaseInvoiceDetail[i].Total;
        // var profit = SalesMaster.SaleInvoiceDetail[i].Profit;
        total += parseFloat(subtotal);
        TotalTax += parseFloat(PurchaseMaster.PurchaseInvoiceDetail[i].TaxAmount);
        totalDiscount += parseFloat(PurchaseMaster.PurchaseInvoiceDetail[i].DiscountAmount);
        TotalQuantity += parseFloat(PurchaseMaster.PurchaseInvoiceDetail[i].Quantity);
        ProductTotal += parseFloat(PurchaseMaster.PurchaseInvoiceDetail[i].producttotal);
    }
    PurchaseMaster.Invoicetotal = total;
    PurchaseMaster.TotalDiscount = totalDiscount;
    PurchaseMaster.TotalTax = TotalTax;
    var $tfoot = $('#Purchasetbl').find('tfoot');
    $tfoot.empty().append('<tr>' +
        '<th colspan="3"></th>' +
        '<th>' + TotalQuantity + '</th>' +
        '<th>' + ProductTotal + '</th>' +
        '<th>' + TotalTax.toFixed(2) + '</th>' +
        '<th>' + totalDiscount.toFixed(2) + '</th>' +
        '<th>' + total.toFixed(2) + '</th>' +
        '</tr>');
    _$.DataTable("#Purchasetbl");
};



$Purchase.EditMakePurchaseDetails = function () {
   $('#EditPurchasetbl').DataTable().clear().draw();
    var total = 0;
    var totalDiscount = 0.00;
    var TotalTax = 0.00;
    var TotalQuantity = 0;
    var ProductTotal = 0.0;
    for (var i = 0; i < PurchaseEditMaster.PurchaseInvoiceDetail.length; i++) {
        var product_obj = Products.filter(x => x.itemid === PurchaseEditMaster.PurchaseInvoiceDetail[i].Itemid);
        PurchaseEditMaster.PurchaseInvoiceDetail[i].productCode = product_obj[0].productCode;
        PurchaseEditMaster.PurchaseInvoiceDetail[i].itemdescp = product_obj[0].itemdescp;
        PurchaseEditMaster.PurchaseInvoiceDetail[i].unitcost = product_obj[0].unitPrice;
        PurchaseEditMaster.PurchaseInvoiceDetail[i].producttotal = product_obj[0].unitPrice * PurchaseEditMaster.PurchaseInvoiceDetail[i].Quantity;
        var producttotal = PurchaseEditMaster.PurchaseInvoiceDetail[i].producttotal.toFixed(2);
        PurchaseEditMaster.PurchaseInvoiceDetail[i].producttotal = parseFloat(producttotal);
        $('#EditPurchasetbl > tbody').append('<tr>' +
            '<td>' + PurchaseEditMaster.PurchaseInvoiceDetail[i].productCode + '</td>' +
            '<td>' + PurchaseEditMaster.PurchaseInvoiceDetail[i].itemdescp + '</td>' +
            '<td>' + PurchaseEditMaster.PurchaseInvoiceDetail[i].unitcost + '</td>' +
            '<td>' + PurchaseEditMaster.PurchaseInvoiceDetail[i].Quantity + '</td>' +
            '<td>' + PurchaseEditMaster.PurchaseInvoiceDetail[i].producttotal + '</td>' +
            '<td>' + PurchaseEditMaster.PurchaseInvoiceDetail[i].TaxAmount + '</td>' +
            '<td>' + PurchaseEditMaster.PurchaseInvoiceDetail[i].DiscountAmount + '</td>' +
            '<td>' + PurchaseEditMaster.PurchaseInvoiceDetail[i].Total + '</td>' +
            '<td>' + '<a href="#" id="' + PurchaseEditMaster.PurchaseInvoiceDetail[i].Itemid + '" data-toggle="modal" class="btn btn-icon btn-danger waves-effect waves-light m-l-5"  onclick="$Purchase.DeleteEditItem(this.id)" rel="tooltip" data-original-title="Delete"><i class="fa fa-trash"></i></a>' + '</td>' +
            '</tr>');
        var subtotal = PurchaseEditMaster.PurchaseInvoiceDetail[i].Total;
        // var profit = SalesMaster.SaleInvoiceDetail[i].Profit;
        total += parseFloat(subtotal);
        TotalTax += parseFloat(PurchaseEditMaster.PurchaseInvoiceDetail[i].TaxAmount);
        totalDiscount += parseFloat(PurchaseEditMaster.PurchaseInvoiceDetail[i].DiscountAmount);
        TotalQuantity += parseFloat(PurchaseEditMaster.PurchaseInvoiceDetail[i].Quantity);
        ProductTotal += parseFloat(PurchaseEditMaster.PurchaseInvoiceDetail[i].producttotal);
       
    }
    PurchaseEditMaster.Invoicetotal = total;
    PurchaseEditMaster.TotalDiscount = totalDiscount;
    PurchaseEditMaster.TotalTax = TotalTax;
    var $tfoot = $('#EditPurchasetbl').find('tfoot');
    $tfoot.empty().append('<tr>' +
        '<th colspan="3"></th>' +
        '<th>' + TotalQuantity + '</th>' +
        '<th>' + ProductTotal.toFixed(2) + '</th>' +
        '<th>' + TotalTax.toFixed(2) + '</th>' +
        '<th>' + totalDiscount.toFixed(2) + '</th>' +
        '<th>' + total.toFixed(2) + '</th>' +
        '</tr>');
    _$.DataTable("#EditPurchasetbl");
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

$Purchase.DeleteEditItem = function (id) {
    id = parseInt(id);
    for (var i = 0; i < PurchaseEditMaster.PurchaseInvoiceDetail.length; i++) {
        if (PurchaseEditMaster.PurchaseInvoiceDetail[i].Itemid === id) {
            PurchaseEditMaster.PurchaseInvoiceDetail.splice(i, 1);
            _$.Notification(_$._messages.Delete, 200);
            $Purchase.EditMakePurchaseDetails();
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
    var deliverydate = $("#DeliveryDate").val();
    var Cokeinvoice = $("#CokeInvoice").val();
    PurchaseMaster.DeliveryDate = deliverydate;
    PurchaseMaster.CokeInvoice = Cokeinvoice;
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

                _$.Notification(result.message, result.status);
                setTimeout(function () {
                    location.reload();
                }, 1000);

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

$Purchase.ValidateForm = function ValidateForm() {
    var stockDropdown = $("#Stockdropdown").val();
    var productDropdown = $("#Productdropdown").val();
    var quantity = parseInt($("#Quantity").val());
    var cokeInvoice = $('#CokeInvoice').val();
    var deliveryDate = $('#DeliveryDate').val();


    // Perform validation checks
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

    //if (taxDropdown === "0") {
    //    _$.Notification("Please select a Tax.",500);
    //    return false;
    //}

    //if (discountDropdown === "0") {
    //    _$.Notification("Please select a Discount.",500);
    //    return false;
    //}
    if (!deliveryDate) {
        _$.Notification("Please select Delivery Date.", 500);
        return false;
    }
    if (!cokeInvoice) {
        _$.Notification("Please Fill Coke Invoice.", 500);
        return false;
    }
    // All checks passed, form is valid
    return true;
}

$Purchase.EditValidateForm = function ValidateForm() {
    var stockDropdown = $("#EditStockdropdown").val();
    var productDropdown = $("#EditProductdropdown").val();
    var quantity = parseInt($("#EditQuantity").val());
    var cokeInvoice = $('#EditCokeInvoice').val();
    var deliveryDate = $('#EditDeliveryDate').val();


    // Perform validation checks
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

    //if (taxDropdown === "0") {
    //    _$.Notification("Please select a Tax.",500);
    //    return false;
    //}

    //if (discountDropdown === "0") {
    //    _$.Notification("Please select a Discount.",500);
    //    return false;
    //}
    if (!deliveryDate) {
        _$.Notification("Please select Delivery Date.", 500);
        return false;
    }
    if (!cokeInvoice) {
        _$.Notification("Please Fill Coke Invoice.", 500);
        return false;
    }
    // All checks passed, form is valid
    return true;
}
$Purchase.LoadPurchase = function () {
    var EditPurchaseInvoiceNo = $("#EditSCokeInvoiceNo").val();
    PurchaseEditMaster.CokeInvoice = EditPurchaseInvoiceNo;
    _$.Post('GetPurchaseByInvoiceNo', PurchaseEditMaster, function (result) {
        result = JSON.parse(result);
        switch (result.Status) {


            case 100:
                //$Product.LoadProduct(result.Data);
                _$.ResetAllContent("#ProductForm");
                $Extention.HideLoader();
                _$.Notification(result.Message, result.Status);

                break;

            case 200:
                PurchaseEditMaster = result.Data;
                $("#EditCokeInvoice").val(PurchaseEditMaster.CokeInvoice);
                var date_ = _$.formatDate(PurchaseEditMaster.DeliveryDate);
                $("#EditDeliveryDate").val(date_);
                $Purchase.EditMakePurchaseDetails();
                break;
            case 300:
                $Extention.HideLoader();
                _$.Notification(result.Message, result.Status);
                break;

            case 400:
                $Extention.HideLoader();
                _$.Notification(result.Message, result.Status);
                break;

            case 404:
                $Extention.HideLoader();
                _$.Notification(result.Message, result.Status);
                break;

            case 500:
                $Extention.HideLoader();
                _$.Notification(result.Message, result.Status);
                break;
        }
    });
}

$Purchase.UpdatePurchase = function () {
    var deliverydate = $("#EditDeliveryDate").val();
    var Cokeinvoice = $("#EditCokeInvoice").val();
    PurchaseEditMaster.DeliveryDate = deliverydate;
    PurchaseEditMaster.CokeInvoice = Cokeinvoice;
    $Extention.ShowLoader();

    _$.Post('UpdatePurchaseInvoice', PurchaseEditMaster, function (result) {
        switch (result.status) {
            case 100:
                //$Product.LoadProduct(result.Data);
                _$.ResetAllContent("#ProductForm");
                $Extention.HideLoader();
                _$.Notification(result.message, result.status);

                break;

            case 200:

                _$.Notification(result.message, result.status);
                setTimeout(function () {
                    location.reload();
                }, 1000);

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










