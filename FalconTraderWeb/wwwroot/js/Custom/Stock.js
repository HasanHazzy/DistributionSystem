function Stock() {
    var $Stock = this, Stock = new Object(), _$ = new Extention();


    $(document).ready(function () {
            $('#productsdropdown').chosen();
       
    });

    $Stock.SaveStock = function () {

        Stock["PId"] = $("#productsdropdown").val();
        Stock["PStock"] = $("#PStock").val();

            //Product.Id = $('#ProductForm #Id').val();

            $Extention.ShowLoader();

            _$.Post('UpdateInventory', Stock, function (result) {
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


}

