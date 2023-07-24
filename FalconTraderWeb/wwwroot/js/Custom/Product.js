function Product() {
    var $Product = this, Product = new Object(), _$ = new Extention();

    $Product.SaveProduct = function () {
        if ($("#Products").valid()) {
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
  

    $Product.ShowModal = function (control) {

        var row = $(control).closest('tr');
        var itemId = row.find('td:eq(0)').text();
        var itemDescp = row.find('td:eq(1)').text();
        var productShort = row.find('td:eq(2)').text();
        var unitprice = row.find('td:eq(3)').text();
        var cashprice = row.find('td:eq(4)').text();
        var creditprice = row.find('td:eq(5)').text();
        var cartonpriceinitial = row.find('td:eq(6)').text();
        var cartonpricecash = row.find('td:eq(7)').text();
        var cartonpricecredit = row.find('td:eq(8)').text();
        var cartonpiece = row.find('td:eq(9)').text();
        var cartonname = row.find('td:eq(10)').text();
        $("#ProductmdlForm #Itemid").val(itemId);
        $("#ProductmdlForm #Itemdescp").val(itemDescp);
        $("#ProductmdlForm #Productshort").val(productShort);
        $("#ProductmdlForm #Priceunit").val(unitprice);
        $("#ProductmdlForm #Cashprice").val(cashprice);
        $("#ProductmdlForm #Creditprice").val(creditprice);
        $("#ProductmdlForm #Cartonprice").val(cartonpriceinitial);
        $("#ProductmdlForm #Cartonpricecash").val(cartonpricecash);
        $("#ProductmdlForm #Cartonpricecredit").val(cartonpricecredit);
        $("#ProductmdlForm #Cartonname").val(cartonname);
        $("#ProductmdlForm #PieceCarton").val(cartonpiece);
        $("#ProductModal").modal("show");
        
    };
    $(document).ready(function () {
        var obj = $("#Producttbl");
        _$.DataTable(obj, null, null);
    });


}
    
