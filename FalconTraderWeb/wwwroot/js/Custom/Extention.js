function Extention() {
    var _$ = this;
    var ToggleInitializerCheck = false;
    skipAjax = false;

    _$._messages = {
        Success: "",
        Eror: "",
        Add: "RECORD HAS BEEN ADDED",
        Update: "RECORD HAS BEEN UPDATED",
        Delete: "RECORD HAS BEEN DELETED",
        Required: "PLEASE FILL DESIRED FIELDS TO CONTINUE.",
        InProcess: "YOUR DESIRED REQUEST IS IN PROCESS, WE WILL NOTIFY YOU ONCE THE PROCESS IS COMPLETE.",
        NotFound: "NO RECORDS FOUND IN SHEET."
    };

    _$.getId = function () {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (var i = 0; i < 5; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        return text;
    }

    _$.PostAsync = function (_url, _data, async, _onsuccess) {
        $.ajax({
            url: _url,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(_data),
            async: async,
            success: _onsuccess,
            error: function (jqXHR, textStatus, errorThrown) {
                //.Notification(jqXHR, result.Status)
                console.log(textStatus, errorThrown);
            }
        });
    }

    _$.Post = function (_url, _data, _onsuccess) {
        $.ajax({
            url: _url,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(_data),
            success: _onsuccess,
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown);
            }
        });
    }

    _$.PostFormData = function (_url, _data, _onsuccess) {
        
        $.ajax({
            url: _url,
            type: "POST",
            //dataType: "json",
            contentType: false,
            processData: false,
            data: _data,
            success: _onsuccess,
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown);
            }
        });
    }

    _$.PostFormDataHTML = function (_url, _data, _onsuccess) {

        $.ajax({

            url: _url,
            type: "POST",
            contentType: false,
            processData: false,
            data: _data,
            success: _onsuccess,
            error: function (jqXHR, textStatus, errorThrown) {

                console.log(textStatus, errorThrown);
            }
        });
    }

    _$.GETHTML = function (_url, _data, async, _onsuccess) {
        $.ajax({
            url: _url,
            type: "POST",
            dataType: "html",
            contentType: "application/json",
            data: JSON.stringify(_data),
            async: async,
            success: _onsuccess,
            error: function (jqXHR, textStatus, errorThrown) {
                _$.HideLoader();
                console.log(textStatus, errorThrown);
            }
        });
    }
    ;
    _$.GetTextCombobox = function (id, Key) {
        ;
        return $(id).find('option[value=' + Key + ']').text();
    }

    _$.SetTextCombobox = function (id, key) {
        return $(id).val(key).change();
    }

    _$.ModalShow = function (id) {
        new Custombox.modal({
            content:{
                target: "#" + id,
                effect: 'door',
                overlaySpeed: 100,
                overlayColor: "#36404a"
            }   
        }).open();

    }

    _$.ModalHide = function () { 
        Custombox.modal.close()
    }

    _$.ModalHideByID = function (id) {
        //Custombox.close(id);
        Custombox.modal.close()
    }

    _$.ConfirmationModal = function (message, title, successCallBack, args) {
        swal({
            title: title,
            text: message,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Proceed!",
            cancelButtonText: "No, Cancel!",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {

                $Extention.ShowLoader();

                var result = successCallBack.apply(this, args);


                // swal("Success!",  ".", "success");
            } else {
                swal("Cancelled", "Operation has been Cancelled :)", "error");
            }
        });

    }

    _$.parseJsonDate = function (jsonDate) {
        if (jsonDate != null) {
            var dateString = jsonDate.substr(6);
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();

            day = ((day) > 9 ? (day) : "0" + (day));
            month = ((month) > 9 ? (month) : "0" + (month));

            var date = day + "/" + month + "/" + year;
            return date;
        }
        return jsonDate;
    };

    _$.parseJsonDateTime = function (jsonDate) {
        if (jsonDate != null) {
            var dateString = jsonDate.substr(6);
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();

            day = ((day) > 9 ? (day) : "0" + (day));
            month = ((month) > 9 ? (month) : "0" + (month));

            var date = day + "/" + month + "/" + year + " " + currentTime.toLocaleTimeString();
            return date;
        }
        return jsonDate;
    };

    _$.Notification = function (_message, _type) {
        _type = parseInt(_type);

        switch (_type) {
            case 200:
                _title = "Success";
                toastr.success(_message, _title);
                break;
            case 500:
                _title = "Error"
                toastr.error(_message, _title);
                break;
            case 300:
                _title = "Warning"
                toastr.warning(_message, _title);
                break;
            case 400:
                _title = "NotFound"
                toastr.warning(_message, _title);
                break;
            case 600:
                _title = "Update"
                toastr.success(_message, _title);
                break;
            case 700:
                _title = "IN PROCESS"
                toastr.warning(_message, _title);
                break;
        }
    }

    _$.ParseString = function (data) {
        var Value = data.split("-")[0];
        return Value.replace(/_/g, '');
    }

    _$.ExcelToJSON = function (file) {
        this.parseExcel = function (file) {
            var reader = new FileReader();

            reader.onload = function (e) {
                var data = e.target.result();
                var workbook = XLSX.read(data, { type: 'binary' });

                workbook.SheetNames.forEach(function (sheetName) {
                    // Here is your object
                    var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
                    var json_object = JSON.stringify(XL_row_object);
                    console.log(json_object);
                })
            };

            reader.onerror = function (ex) {
                console.log(ex);
            };

            reader.readAsBinaryString(file);
        };
    };

    _$.serializeForm = function (id) {
        var result = {};
        $.each($(id).serializeArray(), function (i, field) {
            result[field.name] = field.value.trim() || null;
        });

        return result;
    }

    _$.UpdateURLParameter = function (url, param, paramVal) {
        var newAdditionalURL = "";
        var tempArray = url.split("?");
        var baseURL = tempArray[0];
        var additionalURL = tempArray[1];
        var temp = "";
        if (additionalURL) {
            tempArray = additionalURL.split("&");
            for (i = 0; i < tempArray.length; i++) {
                if (tempArray[i].split('=')[0] != param) {
                    newAdditionalURL += temp + tempArray[i];
                    temp = "&";
                }
            }
        }

        var rows_txt = temp + "" + param + "=" + paramVal;
        return baseURL + "?" + newAdditionalURL + rows_txt;
    }

    _$.ResetClearContent = function (id) {
        $.each($(id + " input:not(:hidden),input[type=radio],input[type=checkbox],input[type=text],select,input[type=textarea]"), function (i, field) {
            if ($(field).is('input:checkbox')) {
                $(field).prop("checked", false);
            }
            else {
                $(field).val('');
            }
        });
    }

    _$.ResetAllContent = function (id) {
        $.each($(id + " input:hidden,input[type=radio],input[type=checkbox],input[type=text],select,input[type=textarea]"), function (i, field) {
            if ($(field).is('input:checkbox')) {
                $(field).prop("checked", false);
            }
            else {
                $(field).val('');
            }
        });
    }

    _$.GetPartialView = function (id, url) {
        $(id).load(url);
    }

    _$.CheckLineBox = function (prop) {
        switch (prop) {
            case "add":
                $(".check-line div").addClass("checked");
                break;
            case "remove":
                $(".check-line div").removeClass("checked");
                break;
        }
    }

    _$.findWithAttr = function (array, attr, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][attr] === value) {
                return i;
            }
        }
        return -1;
    }

    _$.SetCheckBox = function (id, status) {
        return $(id).prop("checked", status);
    }

    _$.DataTable = function (Selector, groupcolumn, colspan) {
        
        var flag = false;
        var rows = [];
        if ($(Selector).length > 0) {
            $(Selector).addClass('compact');
            $(Selector).each(function () {
                $(Selector).find('tbody > tr').each(function () {
                    if ($(this).find('td').length == 1) {
                        this.innerHTML = '';
                        flag = true;
                    }
                    else {
                        rows.push(this);
                    }
                })
                if (!$(this).hasClass("dataTable-custom")) {
                    var opt = {
                        "pagingType": "full_numbers",
                        "destroy": true,
                        "oLanguage": {
                            "search": "<span>Search:</span> ",
                            "info": "Showing <span>_START_</span> to <span>_END_</span> of <span>_TOTAL_</span> entries",
                            "lengthMenu": "_MENU_ <span>entries per page</span>"
                        },
                        "autoWidth": false,
                        'dom': "lfrtip",
                        "drawCallback": function (settings) {
                            if ($(this).hasClass("dataTable-grouping")) {
                                var _table = this;
                                var api = this.api();
                                var rows = api.rows({ page: 'current' }).nodes();
                                var last = null;

                                api.column(groupcolumn, { page: 'current' }).data().each(function (group, i) {
                                    if (last !== group) {
                                        if ($(_table).hasClass("dataTable-checkbox")) {
                                            $(rows).eq(i).before(
                                                '<tr class="group"><td colspan="' + colspan + '"><button onclick="$Report.GetMonthlyInvoicePDF(this.value)" class="btn vertical_align_middle display_inline m-r-5" rel="tooltip" data-original-title="Print" type = "button" value=' + group + '><i class="fa fa-print"></i></button><span class="vertical_align_middle display_inline" style="line-height: normal;">' + group + '</span></td> </tr>'
                                                );
                                        }
                                        else {
                                            $(rows).eq(i).before(
                                                '<tr class="group"><td colspan="' + colspan + '"><STRONG>' + group + '</STRONG></td></tr>'
                                                );
                                        }


                                        last = group;
                                    }
                                });
                            }

                        }
                    };

                    if ($(this).hasClass("dataTable-noheader")) {
                        opt.searching = false;
                        opt.lengthChange = false;
                    }
                    if ($(this).hasClass("dataTable-nofooter")) {
                        opt.info = false;
                        opt.paging = false;
                    }
                    if ($(this).hasClass("dataTable-nosort")) {
                        var column = $(this).attr('data-nosort');
                        column = column.split(',');
                        for (var i = 0; i < column.length; i++) {
                            column[i] = parseInt(column[i]);
                        };
                        opt.columnDefs = [{
                            'orderable': false,
                            'targets': column
                        }];
                    }
                    if ($(this).hasClass("dataTable-no-Initial-sort")) {
                        opt.order = []
                    }

                    if ($(this).hasClass("dataTable-sort")) {
                        var column = $(this).attr('data-sort');
                        opt.order = [[column, "desc"]]
                    }

                    if ($(this).hasClass("dataTable-scroll-x")) {
                        opt.scrollX = "100%";
                        opt.scrollCollapse = true;
                        $(window).resize(function () {

                        });
                    }
                    if ($(this).hasClass("dataTable-scroll-y")) {
                        opt.scrollY = "300px";
                        opt.paging = false;
                        opt.scrollCollapse = true;
                        $(window).resize(function () {
                            //   oTable.columns.adjust().draw();
                        });
                    }
                    if ($(this).hasClass("dataTable-reorder")) {
                        opt.dom = "R" + opt.dom;
                    }
                    if ($(this).hasClass("dataTable-colvis")) {
                        opt.dom = "C" + opt.dom;
                        opt.oColVis = {
                            "buttonText": "Change columns <i class='icon-angle-down'></i>"
                        };
                    }
                    if ($(this).hasClass("dataTable-excel")) {
                        opt.dom = 'Bfrtip',
                        opt.buttons = [{
                            extend: "excel",
                            className: "btn-sm"
                        }]
                    }
                    if ($(this).hasClass('dataTable-tools')) {
                        //opt.sDom = "T" + opt.sDom;
                        //opt.oTableTools = {
                        //    "sSwfPath": "js/plugins/datatable/swf/copy_csv_xls_pdf.swf"
                        //};

                        opt.dom = "T" + opt.dom;
                        opt.oTableTools = {
                            "aButtons": [
                                "xls"
                            ]
                        };
                    }
                    if ($(this).hasClass("dataTable-scroller")) {
                        opt.sScrollY = "300px";
                        opt.bDeferRender = true;
                        if ($(this).hasClass("dataTable-tools")) {
                            opt.dom = 'TfrtiS';
                        } else {
                            opt.dom = 'frtiS';
                        }
                        opt.sAjaxSource = "js/plugins/datatable/demo.txt";
                    }
                    //if ($(this).hasClass("dataTable-grouping") && $(this).attr("data-grouping") == "expandable") {
                    //    opt.lengthChange = false;
                    //    opt.paging = false;
                    //}

                    var oTable = $(Selector).DataTable(opt);
                    oTable.columns.adjust().draw();
                    $(this).css("width", '100%');
                    $('.dataTables_filter input').attr("placeholder", "Search here...");
                    //$(".dataTables_length select").wrap("<div class='input-mini'></div>").chosen({
                    //    disable_search_threshold: 9999999
                    //});
                    $("#check_all").click(function (e) {
                        $('input', oTable.rows().nodes()).prop('checked', this.checked);
                    });
                    if ($(this).hasClass("dataTable-fixedcolumn")) {
                        new FixedColumns(oTable);
                    }
                    if ($(this).hasClass("dataTable-columnfilter")) {
                        oTable.columnFilter({
                            "sPlaceHolder": "head:after"
                        });
                    }
                    //if ($(this).hasClass("dataTable-grouping")) {
                    //    var rowOpt = {};

                    //    if ($(this).attr("data-grouping") == 'expandable') {
                    //        rowOpt.bExpandableGrouping = true;
                    //    }
                    //    oTable.rowGrouping(rowOpt);
                    //}

                    if (flag == true)
                        $(rows).each(function () {

                            oTable.rows.add($(this)).draw();
                        })
                    oTable.columns.adjust().draw();
                    _$.Tooltip();
                }
            });
        }
    }

    _$.SplitCamelCase = function (word) {
        if (word != null) {
            return word.replace(/([A-Z]+)/g, "$1").replace(/([A-Z][a-z])/g, " $1");
        }
        else {
            return "";
        }
    }

    _$.Tooltip = function () {
        var mobile = false, tooltipOnlyForDesktop = true, notifyActivatedSelector = 'button-active';

        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
            mobile = true;
        }

        if (tooltipOnlyForDesktop) {
            if (!mobile) {
                $('[rel=tooltip]').tooltip();
            }
        }
    }

    _$.DatePick = function () {
        $('.datepick').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
        }).on('change', function (ev) {
            var m = $(this).val().match(/^(\d{1,2})\/(\d{1,2})\/(\d{4})$/);
            var a = (m) ? new Date(m[3], m[2] - 1, m[1]) : null;
            if (a == null) {
                $(this).val("");
            }
        });
    }

    _$.formWizard = function () {
        $(".form-wizard").formwizard({
            formPluginEnabled: true,
            validationEnabled: true,
            focusFirstInput: false,
            disableUIStyles: true,
            validationOptions: {
                errorElement: 'span',
                errorClass: 'help-block error',
                errorPlacement: function (error, element) {
                    element.parents('.controls').append(error);
                },
                highlight: function (label) {
                    $(label).closest('.control-group').removeClass('error success').addClass('error');
                },
                success: function (label) {
                    label.addClass('valid').closest('.control-group').removeClass('error success').addClass('success');
                }
            },
            formOptions: {
                success: function (data) {
                    alert("Response: \n\n" + data.say);
                },
                dataType: 'json',
                resetForm: true
            }
        });
    }

    _$.SetComboboxByValue = function (Id, theText) {
        $(Id + " option").each(function () {
            if ($(this).text() == theText) {
                return $(Id).val($(this).val()).change();
            }
        });
    }

    _$.ResetClearContentbenfits = function (id) {
        $.each($(id + "input:not(:hidden),input[type=radio],input[type=checkbox],input[type=text]:not([name='Name']),select,input[type=textarea]"), function (i, field) {
            if (field.name != "ParentId") {
                $(field).val('');
            }
        });
    }

    _$.RemoveValidation = function (Id) {
        //    $('.clearcontent').parent().parent().removeClass('success');
        $(Id).find('.error span').remove();
        $(Id).find('.error').removeClass('error')
    }

    _$.ClearMultiSelect = function () {
        $("#Disease").select2("val", "");
    }

    _$.isRealValue = function (obj) {
        return obj && obj !== 'null' && obj !== 'undefined';
    }

    _$.Disabled = function (Id) {
        $('#' + Id).addClass('disabledfield');
    }

    _$.removeDisabled = function (Id) {
        $('#' + Id).removeClass('disabledfield');
    }

    _$.RemoveDuplicates = function (arr, prop) {
        var new_arr = [];
        var lookup = {};
        for (var i in arr) {
            lookup[arr[i][prop]] = arr[i];
        }
        for (i in lookup) {
            new_arr.push(lookup[i]);
        }

        return new_arr;
    }

    _$.tableToExcel = (function () {
        var uri = 'data:application/vnd.ms-excel;base64,'
          , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>'
          , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
          , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
        return function (table, name) {
            if (!table.nodeType) table = document.getElementById(table)
            var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
            window.location.href = uri + base64(format(template, ctx))
        }
    })();

    _$.numberWithCommas = function (x) {

        return x.toString().replace(/,/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    _$.isInArray = function (value, array) {
        return array.indexOf(value) > -1;
    }

    _$.GetCurrentDate = function () {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();

        return ('0' + dd).slice(-2) + "/" + ('0' + mm).slice(-2) + "/" + yyyy;
    }

    _$.ParseStringToDate = function (date) {

        if (date == null || date == undefined)
            return null;

        var _Date = date.split('/');

        var Day = _Date[0];
        var Month = _Date[1];
        var Year = _Date[2];
        if (_Date.length = 3 && Day <= 31 && Month <= 12) {
            return new Date(Year, (parseInt(Month) - 1), Day);
        }
        else {
            return new Date("1900/01/01");
        }
    }

    _$.ParseStringToJSONDate = function (strDate) {
        var newDate = this.ParseStringToDate(strDate)
        return '/Date(' + newDate.getTime() + ')/';
    }

    _$.ParseDateToString = function (DateObj) {
        var datestring = DateObj.getDate() + "/" + (DateObj.getMonth() + 1) + "/" + DateObj.getFullYear()
    }

    _$.DateDifference = function (StartDate, EndDate) {

        var Start = StartDate.split('/');
        var InitialDate = StartDate.split('/')[1] + '/' + StartDate.split('/')[0] + '/' + StartDate.split('/')[2];
        InitialDate = new Date(InitialDate);

        var End = EndDate.split('/');
        var LastDate = EndDate.split('/')[1] + '/' + EndDate.split('/')[0] + '/' + EndDate.split('/')[2];
        LastDate = new Date(LastDate);

        var TimeDifference = Math.floor(LastDate.getTime() - InitialDate.getTime());
        var DaysDifference = Math.ceil(TimeDifference / (1000 * 3600 * 24));

        return DaysDifference;
    }

    _$.isEmpty = function (val) {
        return (val === undefined || val == null || val.length <= 0) ? true : false;
    }

    _$.ScrollTop = function () {
        $("html, body").animate({ scrollTop: 0 }, "slow");
    }

    _$.trimObj = function (obj) {
        if (!Array.isArray(obj) && typeof obj != 'object') return obj;
        return Object.keys(obj).reduce(function (acc, key) {
            acc[key.trim()] = typeof obj[key] == 'string' ? obj[key].trim() : trimObj(obj[key]);
            return acc;
        }, Array.isArray(obj) ? [] : {});
    }

    _$.toDate = function (dateStr) {
        var parts = dateStr.split("/");
        return new Date(parts[2], parts[1] - 1, parts[0]);
    }

    _$.GetParameterByName = function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    _$.isValidExpression = function (_text, _expression, _error) {

        var isvalid = _expression.test(_text);
        if (isvalid || _text == "") {
            return true;
        }
        else {
            _$.Notification(_error, 200);
            return false;
        }
    }

    _$.hasNull = function (target) {
        var status = false;
        for (var member in target) {
            if (target[member] != null) {
                status = true;
                break;
            }
        }
        return status;
    }

    //_$.makeDataTable = function (tbl, url, columns, params, div, fnCreatedRow) {
    //    try {

    //        $Extention.ShowLoader();

    //        var opt = {
    //            "bServerSide": true,
    //            "bProcessing": false,
    //            "bDestroy": true,
    //            "bAutoWidth": false,
    //            "oLanguage": {
    //                "sSearch": "<span>Search:</span> ",
    //                "sInfo": "Showing <span>_START_</span> to <span>_END_</span> of <span>_TOTAL_</span> entries",
    //                "sLengthMenu": "_MENU_ <span>entries per page</span>"
    //            },
    //            "sAjaxSource": url,
    //            "sServerMethod": "POST",
    //            "sPaginationType": "full_numbers",
    //            "aoColumns": columns,
    //            "fnServerParams": params,
    //            "fnDrawCallback": function () {
    //                _$.Tooltip();
    //            },
    //            //"fnCreatedRow": fnCreatedRow,
    //            "sDom": "lfrtip"
    //        };
    //        var oTable = $(tbl).dataTable(opt);
    //        if ($(tbl).hasClass("dataTable-scroll-x")) {
    //            opt.sScrollX = "100%";
    //            opt.bScrollCollapse = true;
    //            $(window).resize(function () {
    //                oTable.fnAdjustColumnSizing(false);
    //            });
    //        }
    //        if ($(tbl).hasClass("dataTable-scroll-y")) {
    //            opt.sScrollY = "300px";
    //            opt.bPaginate = false;
    //            opt.bScrollCollapse = true;
    //            $(window).resize(function () {
    //                oTable.fnAdjustColumnSizing(false);
    //            });
    //        }


    //        $(tbl).css("width", '100%');
    //        $('.dataTables_filter input').attr("placeholder", "Search here...");
    //        $(".dataTables_length select").wrap("<div class='input-mini'></div>").chosen({
    //            disable_search_threshold: 9999999
    //        });

    //        if ($(tbl).hasClass("dataTable-grouping")) {
    //            var rowOpt = {};

    //            if ($(tbl).attr("data-grouping") == 'expandable') {
    //                rowOpt.bExpandableGrouping = true;
    //            }
    //            $(tbl).rowGrouping(rowOpt);
    //        }
    //    }
    //    catch (e) {

    //        $Extention.HideLoader();
    //    }

    //    $(tbl).on('processing.dt', function (e, settings, processing) {

    //        $('#loading').css('display', processing ? 'block' : 'none');

    //        //  $Extention.HideLoader();

    //    });

    //    $(tbl).on('xhr.dt', function (e, settings, json, xhr) {

    //        $(div).show();

    //        $Extention.HideLoader();

    //        if ((typeof json.Status !== "undefined") && (json.Status != 100)) {
    //            _$.Notification(json.Message, json.Status);
    //            $(div).hide();
    //        }




    //    });
    //}




    _$.makeDataTable = function (tbl, url, columns, params, div, createdRow, sortColumnIndex, groupColumn, orderby, intializationComplete) {

        if (orderby == null) {
            orderby = 'asc';
        }
        skipAjax = false;        //skipAjax = false;
        tbl.addClass('compact')
        //str = str || 2;
        $(div).show();


        groupColumn = groupColumn | 0;


        $.each(columns, function (i, field) {
            if (tbl.hasClass("dataTable-grouping")) {
                field.orderable = false;
                field.sortable = false;
            }

        })
        //params();
        //var parameters = [];
        //$.each(aoData, function (i, field) {
        //    parameters[field.name] = field.value;
        //});
        // console.log(params())
        try {

            _$.ShowLoader();
            var opt = {

                "serverSide": true,
                "columnDefs": [{ "orderable": false }],
                "processing": true,
                "ordering": true,
                "paging": true,
                "lengthMenu": [[10, 100, 500, 500000], [10, 100, 500, "All"]],
                "order": [[sortColumnIndex, orderby]],
                "destroy": true,
                "pageLength": 10,
                "autoWidth": false,
                "pageLength": 10,
                "searchDelay": "800",
                "oLanguage": {
                    "search": "<span>Search:</span> ",
                    "info": "Showing <span>_START_</span> to <span>_END_</span> of <span>_TOTAL_</span> entries",
                    "lengthMenu": "_MENU_ <span>entries per page</span>"
                },
                "ajax": {
                    "url": url,
                    // "async":true,
                    "data": params,
                    "dataSrc": function (json) {
                        if (json.data == null) {
                            if ((typeof json.Status !== "undefined") && (json.Status != 100)) {
                                _$.Notification(json.Message, json.Status);
                                _$.HideLoader();
                                $(div).hide();
                                return [];
                            }
                        }


                        return json.data;
                    },
                    'beforeSend': function (jqXHR, settings) {
                        if (tbl.hasClass("dataTable-grouping")) {
                            if (skipAjax) {

                                skipAjax = false; //reset the flag

                                return false; //cancel current AJAX request
                            }
                            else {
                                _$.ShowLoader();
                            }
                        } else {
                            _$.ShowLoader();
                        }


                    },

                    "type": "POST",
                },
                "pagingType": "full_numbers",
                "columns": columns,
                "drawCallback": function (setting) {

                    _$.HideLoader();
                    if (tbl.hasClass("dataTable-grouping")) {

                        var api = this.api();
                        var rows = api.rows({
                            page: 'current'
                        }).nodes();
                        var last = null;


                        api.column(groupColumn, {
                            page: 'current'
                        }).data().each(function (group, i) {

                            if (last !== group) {

                                $(rows).eq(i).before(
                                    '<tr class="group group-style"><td colspan="' + setting.aoColumns.length + '"><i class="icon-chevron-right" aria-hidden="true"></i><i class="icon-chevron-down" aria-hidden="true"></i>' + group + '</td></tr>'
                                );
                                last = group;
                            }
                        });


                    }

                    _$.Tooltip();
                },
                "createdRow": createdRow,
                "dom": "lfrtip",
                "initComplete": intializationComplete,
            };

            if ($(tbl).hasClass("dataTable-scroll-x")) {

                opt.scrollX = "100%";
                opt.scrollCollapse = true;

                //oTable.fnAdjustColumnSizing();
            }
            if ($(tbl).hasClass("dataTable-scroll-y")) {

                opt.scrollY = "300px";
                opt.paginate = false;
                opt.scrollCollapse = true;


            }
            oTable = $(tbl).DataTable(opt);

            $(tbl).css("width", '100%');
            $('.dataTables_filter input').attr("placeholder", "Search here...");





        }
        catch (e) {
            alert(e);
            _$.HideLoader();
        }

    }


    //$Extention.HideLoader();



    _$.InitializeGroupToggle = function (tbl) {
        tbl.find('tbody').on('click', 'tr.group', function () {
            $(this).nextUntil('tr.group').fadeToggle(250);
            $(this).toggleClass("active");
        });
    }

    _$.tableExport = function (tbl, ignorecolumns, type, fileName) {
        $(tbl).show();
        $(tbl).tableExport({ type: type, escape: 'false', ignoreColumn: ignorecolumns, htmlContent: 'false', fileName: fileName });
        $(tbl).hide();
    }

    _$.ReplaceAllCharacters = function (target, search, replacement) {
        return target.replace(new RegExp(search, 'g'), replacement);
    };

    _$.ValidateDateRange = function (startdate, enddate) {
        var _startdate = startdate.split("/");
        var _enddate = enddate.split("/");

        startdate = new Date(_startdate[1] + '/' + _startdate[0] + '/' + _startdate[2]);
        enddate = new Date(_enddate[1] + '/' + _enddate[0] + '/' + _enddate[2]);

        if (startdate > enddate) {
            return false;
        }

        return true;
    }


    _$.GetColumnHeaders = function (tableName) {

        var columns = [];
        var obj = '#' + tableName + ' thead tr th';
        $(obj).each(function () {
            var text = $(this).attr('aria-label');
            if (text == null) {
                var text = $(this).children().text();
                columns.push(text);

            } else {

                if (text.indexOf(':') != -1) {
                    if (typeof text !== "undefined" && text.substr(0, text.indexOf(':')) != 'Action' && text.substr(0, text.indexOf(':')) != '') {
                        columns.push(text.substr(0, text.indexOf(':')));

                    }
                }
                else {
                    if (typeof text !== "undefined" && text != 'Action' && text != '')
                        columns.push(text);
                }
            }

        });

        return columns.join(", ");
        //return columns;
    };

    _$.GetTableHeaders = function (tableName) {

        var columns = [];
        var obj = '#' + tableName + ' thead tr th';
        $(obj).each(function () {



            var text = $(this).html();
            if (typeof text !== "undefined" || text == "") {
                columns.push(text);

            }


        });

        return columns.join(", ");
        //return columns;
    };

    _$.GetAgeFromDateOfBirth = function (DateOfBirth, AdditionEffectiveDate) {
        _$.ShowLoader();
        var Age = '';
        if (DateOfBirth == '') {
            _$.HideLoader();
            _$.Notification("DATE OF BIRTH CANNOT BE EMPTY", 200);
        }
        else {
            $.ajax({
                url: '/Sales/Policy/GetAgeFromDateOfBirth/?DateOfBirth=' + DateOfBirth + '&AdditionEffectiveDate=' + AdditionEffectiveDate,
                dataType: 'json',
                async: false,
                success: function (result) {
                    if (result.Status === 100) {
                        Age = result.Data;
                    } else {
                        _$.Notification(result.Message, result.Status);
                    }

                    _$.HideLoader();
                }
            });
            return Age;
        }   
    }

    _$.TableToJSON = function (Tableid) {
        var table = $('#' + Tableid);
        var headers = [];
        var data = [];
        //Get Header
        for (var i = 0; i < table.find('thead tr th').length; i++) {
            headers[i] = table.find('thead tr th')[i].innerHTML;
        }
        //Get Data
        for (var i = 0; i < table.find('tbody tr').length; i++) {
            var rowData = {};
            for (var j = 0; j < document.getElementById(Tableid).rows[i].cells.length; j++) {
                rowData[headers[j]] = table.find('tbody tr td')[j].innerHTML;
            }
            data.push(rowData);
        }
        return data
    }

    _$.isObjectEmpty = function (object) {
        var isEmpty = true;
        for (keys in object) {
            isEmpty = false;
            break; // exiting since we found that the object is not empty
        }
        return isEmpty;
    }

    _$.ShowLoader = function () {
        //locked = true;
        $('#loading').show();
        $('body').css('overflow-y', 'hidden');
    }

    _$.HideLoader = function () {
        $('#loading').hide();
        $('body').css('overflow-y', 'auto')
        //locked = false;
    }

    _$.tableToCSV = function (table, name) {
        
        //$(table).table2csv({
        //    filename: name + '.csv'
        //});

        var csv = [];
        var rows = document.querySelectorAll(table + " tr");

        for (var i = 0; i < rows.length; i++) {
            var row = [], cols = rows[i].querySelectorAll("td, th");

            for (var j = 0; j < cols.length; j++)
                row.push(cols[j].innerText.trim().replace(/  |\r\n|\n|\r/gm, ''));

            csv.push(row.join(","));
        }

        // Download CSV file
        downloadCSV(csv.join("\n"), name);

    }

    function downloadCSV(csv, filename) {
        var csvFile;
        var downloadLink;
        
        // CSV file
        csvFile = new Blob([csv], { type: "text/csv" });

        // Download link
        downloadLink = document.createElement("a");

        // File name
        downloadLink.download = filename;

        // Create a link to the file
        downloadLink.href = window.URL.createObjectURL(csvFile);

        // Hide download link
        downloadLink.style.display = "none";

        // Add the link to DOM
        document.body.appendChild(downloadLink);

        // Click download link
        downloadLink.click();
    }

    _$.CalculateNumberOfMonths = function (StartDate, EndDate) {
        var _StartDate = new Date(_$.ParseStringToDate(StartDate));
        var _EndDate = new Date(_$.ParseStringToDate(EndDate));
        return MonthDifference(_StartDate, _EndDate);
    }

    MonthDifference = function (d1, d2) {
        var months;
        months = (d2.getFullYear() - d1.getFullYear()) * 12;
        months -= d1.getMonth();
        months += d2.getMonth();
        return months <= 0 ? 0 : months;
    }

    _$.ValidateInActiveForm = function (formid) {
        $(formid).validate(
            { ignore: [] }
        );
    }

    _$.DateFormat = function (dateString) {
        var dateParts = dateString.split("/");
        return new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);
    }

};