function Customer() {
    var $Customer = this, Customer = new Object(), _$ = new Extention();

    $Customer.SaveCustomer = function () {

        // if ($("#CustomerForm").valid()) {
        $.each($('#CustomerForm').serializeArray(), function (i, field) {
            Customer[field.name] = field.value || null;
        });
        //Product.Id = $('#ProductForm #Id').val();
        $Extention.ShowLoader();

        _$.Post('SaveCustomer', Customer, function (result) {
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

    };

    $Customer.ShowModal = function (control) {

        var row = $(control).closest('tr');
        var CId = row.find('td:eq(0)').text();
        var CName = row.find('td:eq(1)').text();
        var CMobile = row.find('td:eq(2)').text();
        var CAddress = row.find('td:eq(3)').text();
        var CType = row.find('td:eq(4)').text();
        var CEmail = row.find('td:eq(5)').text();
        var CCnic = row.find('td:eq(6)').text();
        var CMaxCredit = row.find('td:eq(7)').text();
        var CDesc = row.find('td:eq(8)').text();
        
        $("#CustomermdlForm #CID").val(CId);
        $("#CustomermdlForm #CName").val(CName);
        $("#CustomermdlForm #CMobile").val(CMobile);
        $("#CustomermdlForm #CType").val(CType);
        $("#CustomermdlForm #CEmail").val(CEmail);
        $("#CustomermdlForm #CCnic").val(CCnic);
        $("#CustomermdlForm #CMaxCredit").val(CMaxCredit);
        $("#CustomermdlForm #CDesc").val(CDesc);
        $("#CustomermdlForm #CAddress").val(CAddress);
        
        $("#CustomerModal").modal("show");

    };

    $Customer.UpdateCustomer = function () {
        //if ($("#ProductmdlBody").valid()) {
        $.each($('#CustomermdlForm').serializeArray(), function (i, field) {
            Customer[field.name] = field.value || null;
        });

        //Product.Id = $('#ProductForm #Id').val();

        $Extention.ShowLoader();

        _$.Post('UpdateCustomer', Customer, function (result) {
            switch (result.status) {
                case 100:
                    //$Product.LoadProduct(result.Data);
                    _$.ResetAllContent("#CustomermdlForm");
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);

                    break;

                case 200:
                    $Extention.HideLoader();
                    _$.Notification(result.message, result.status);
                    $("#CustomerModal").modal("hide");

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
}






