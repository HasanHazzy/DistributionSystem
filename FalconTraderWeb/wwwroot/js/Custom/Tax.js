//function Product() {
var $Tax = this, Tax = new Object(), _$ = new Extention();

$Tax.SaveTax = function () {
    if ($Tax.Validateform()) {
        $.each($('#Tax').serializeArray(), function (i, field) {
            Tax[field.name] = field.value || null;
        });

        //Product.Id = $('#ProductForm #Id').val();

        $Extention.ShowLoader();

        _$.Post('SaveTax', Tax, function (result) {
            switch (result.status) {
                case 100:
                    //$Product.LoadProduct(result.Data);
                    _$.ResetAllContent("#TaxForm");
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

$Tax.UpdateTax = function () {
    //if ($("#ProductmdlBody").valid()) {
    $.each($('#TaxmdlForm').serializeArray(), function (i, field) {
        Tax[field.name] = field.value || null;
    });

    //Product.Id = $('#ProductForm #Id').val();

    $Extention.ShowLoader();

    _$.Post('UpdateTax', Tax, function (result) {
        switch (result.status) {
            case 100:
                //$Product.LoadProduct(result.Data);
                _$.ResetAllContent("#TaxForm");
                $Extention.HideLoader();
                _$.Notification(result.message, result.status);

                break;

            case 200:
                $Extention.HideLoader();
                _$.Notification(result.message, result.status);
                $("#TaxModal").modal("hide");

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


$Tax.ShowModal = function (control) {
    var row = $(control).closest('tr');
    var Id = row.find('td:eq(0)').text();
    var TaxDescp = row.find('td:eq(1)').text();
    var Percentage = row.find('td:eq(2)').text();

    $("#TaxmdlForm #Id").val(Id);
    $("#TaxmdlForm #TaxDescp").val(TaxDescp);
    $("#TaxmdlForm #Percentage").val(Percentage);
    $("#TaxModal").modal("show");

};
$Tax.Validateform = function () {
        var taxDescp = $("#TaxDescp").val();
        var taxPercentage = $("#Percentage").val();

        // Check if Tax Description is empty
        if (taxDescp.trim() === '') {
            _$.Notification("Tax Description cannot be empty.",500);
            return false;
        }

        // Check if Tax Percentage is empty or not a valid number
        if (isNaN(taxPercentage) || taxPercentage.trim() === '') {
            _$.Notification("Please enter a valid Tax Percentage.",500);
            return false;
        }

        // Check if Tax Percentage is within the valid range (0-100)
        if (taxPercentage > 100) {
            _$.Notification("Tax Percentage must be between 0 and 100.",500);
            return false;
        }

        // Validation passed, you can now save the data
        return true;
    
}
$(document).ready(function () {
    //var obj = $("#Producttbl");
    //_$.DataTable(obj, null, null);
});


//}

