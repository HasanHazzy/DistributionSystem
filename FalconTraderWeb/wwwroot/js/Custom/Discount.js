var $Discount = this, Discount = new Object(), _$ = new Extention();

$Discount.SaveDiscount = function () {
    if ($Discount.Validateform()) {
        $.each($('#Discount').serializeArray(), function (i, field) {
            Discount[field.name] = field.value || null;
        });

        //Product.Id = $('#ProductForm #Id').val();

        $Extention.ShowLoader();

        _$.Post('SaveDiscount', Discount, function (result) {
            switch (result.status) {
                case 100:
                    //$Product.LoadProduct(result.Data);
                    _$.ResetAllContent("#DiscountForm");
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

$Discount.UpdateDiscount = function () {
    //if ($("#ProductmdlBody").valid()) {
    $.each($('#DiscountmdlForm').serializeArray(), function (i, field) {
        Discount[field.name] = field.value || null;
    });

    //Product.Id = $('#ProductForm #Id').val();

    $Extention.ShowLoader();

    _$.Post('UpdateDiscount', Discount, function (result) {
        switch (result.status) {
            case 100:
                //$Product.LoadProduct(result.Data);
                _$.ResetAllContent("#DiscountForm");
                $Extention.HideLoader();
                _$.Notification(result.message, result.status);

                break;

            case 200:
                $Extention.HideLoader();
                _$.Notification(result.message, result.status);
                $("#DiscountModal").modal("hide");

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


$Discount.ShowModal = function (control) {
    var row = $(control).closest('tr');
    var Id = row.find('td:eq(0)').text();
    var DiscountDescp = row.find('td:eq(1)').text();
    var Percentage = row.find('td:eq(2)').text();

    $("#DiscountmdlForm #Id").val(Id);
    $("#DiscountmdlForm #Descp").val(DiscountDescp);
    $("#DiscountmdlForm #Percentage").val(Percentage);
    $("#DiscountModal").modal("show");

};
$Discount.Validateform = function () {
    var discountDescp = $("#Descp").val();
    var discountPercentage = $("#Percentage").val();

    // Check if Discount Description is empty
    if (discountDescp.trim() === '') {
        _$.Notification("Discount Description cannot be empty.",500);
        return false;
    }

    // Check if Discount Percentage is empty or not a valid number
    if (isNaN(discountPercentage) || discountPercentage.trim() === '') {
        _$.Notification("Please enter a valid Discount Percentage.", 500);
        return false;
    }

    // Check if Discount Percentage is within the valid range (0-100)
    if (discountPercentage < 0 || discountPercentage > 100) {
        _$.Notification("Discount Percentage must be between 0 and 100.",500);
        return false;
    }

    // Validation passed, you can now save the data
    return true;
};



$(document).ready(function () {
    //var obj = $("#Producttbl");
    //_$.DataTable(obj, null, null);
});




