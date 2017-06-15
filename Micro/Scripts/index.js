//anys loading of data.


$(document).ready(function () {

    //variables

    var hits = 0;
    var Fullsearch = $('#fullsearch'),
        Formsearch = $('#formsearch'),
        Searchbox = $('#Searchbox'),
        Buttonclose = $('.close'),
        summaryContent = $('#top_table'),
        Hidecontent = 'datalist',
         Datalist = $('#datalist');


    //event handles 

    Fullsearch.keyup(function () {

        var value = $(this).val();

        if (value.length !== 0) {

            $.ajax({
                url: '/Api/GetCustomerName',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                datatype: 'json',
                data: { data: value },
                success: returndata,
                error: returnerror

            });
        }


        function returndata(data, status) {
              if (status = "success") {
                var count = data.length;

                Datalist.removeClass(Hidecontent);

                $('#datalist li').replaceWith("");
                for (var i = 0; i < count; i++) {

                    Datalist.append("<li>" + data[i].Fullname + "</li>");
                }
            }
        }



    });
   
    $('#checkstatus').submit( function(event) {
        
        var valuestring = $("Searchbox").val();

            $.ajax({
                url: '/Transaction/checkStatus',
                type:"Get",
                data: { data: valuestring },
                dataType: 'html',
            success: function(data) {
                $('#here').html(data);
            },
            
            });
            event.preventDefault();
});

    Formsearch.submit(function (event) {

        var submitedtext = Fullsearch.val();

        if (submitedtext.length !== 0) {

            $.ajax({
                url: '/Api/GetUserSummary',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                datatype: 'json',
                data: { data: submitedtext },
                success: returndata,
                error: returnerror

            });
        }


        function returndata(data, status) {
            if (status = "success") {

                summaryContent.removeClass(Hidecontent);
                var ct = data.length;

                $('#username').text(data.username);
                $('#customerid').text(data.customerid);
                $('#age').text(data.age);
                $('#nationality').text(data.nationality);
                $('#phone_number').text(data.phone_number);
                $('#address').text(data.address);
                $('#date').text(ToJavaScriptDate(data.date));
                $('#akibaid').text(data.akibaid);
                $('#akiba_balance').text(data.Akiba_balance);
                $('#loanid').text(data.loanid);
                $('#loan_balance').text(data.loan_balance);
                $('#amount_taken').text(data.amount_taken);
                $('#return_amount').text(data.return_amount);
                $('#no_hisa').text(data.no_hisa);
                $('#amount_hisa').text(data.amount_hisa);

                buttonlink(data);
                return;
            }
        };

        event.preventDefault();
    });

    Fullsearch.focusout(function () {
        hits = 0;

        addcssclass();

    });

    Searchbox.keyup(function () {


        var textthis = $(this).val();


        $.ajax({
            url: '/Api/GetLoanerStatus',
            contentType: "application/json; charset=utf-8",
            type: "GET",
            datatype: 'json',
            data: { data: textthis },
            success: returndata,
            error: returnerror

        });
        function returndata(data, status) {
            if (status = "success") {
                var count = data.length;
                for (var i = 0; i < count; i++) {
                    $('tbody').replaceWith("<tr> <td>" + data[i].First_name + "</td> <td>" + data[i].Middle_name + "</td> <td>" + data[i].Last_name + "</td>"
                        + " <td>" + data[i].CustomerId + "</td>" + "<td>" + data[i].duration + "</td>" + "<td>" + data[i].amount + "</td>"
                        + "<td>" + data[i].returnAmount + "</td>" + "<td>" + ToJavaScriptDate(data[i].date) + "</td>"
                        + "<td>"
            + "<a href=\"/Transaction/payLoan?key=" + data[i].loanid + "\">Pay Loan</a> </td> </tr>");
                }
            }
        }

    });

    Buttonclose.click(function () {

        summaryContent.addClass(Hidecontent);
        Fullsearch.text('');
    });


    //functions 
    function selecttext(valuetext) {

        Fullsearch.val(valuetext);
        addcssclass;
    };

    function addcssclass() {
        Datalist.addClass(Hidecontent);
    }

    //$(document).ajaxStart(function () {
    //   // show a progress modal of your choosing
    //    showProgressModal('loading');
    //});
    //$(document).ajaxStop(function () {
    //   // hide it
    //    hideProgressModal();
    //});

    //$.ajax({
    //    url: '/controller/create',
    //    dataType: 'html',
    //    success: function (data) {
    //        $('#myPartialContainer').html(data);
    //    }
    //});

    function buttonlink(data) {

        var disa = 'hidden';

        var link_customer = $('a.link-customer'),
            link_akiba = $('a.link-akiba'),
            link_akiba_last = link_akiba.last(),
            link_loan = $('a.link-loan'),
            link_loan_last = link_loan.last(),
            customerid = data.customerid,
                akibaid = data.akibaid,
                loanid = data.loanid,
                    nameName = data.username;

        link_akiba.each(function () {
            var href = $(this).attr('href');
            $(this).attr('href', href.slice(0, href.indexOf('?')));

        }),
        link_loan.each(function () {
            var href = $(this).attr('href');
            $(this).attr('href', href.slice(0, href.indexOf('?')));

        }),
        link_customer.each(function () {
            var href = $(this).attr('href');
            $(this).attr('href', href.slice(0, href.indexOf('?')));

        });

        link_customer.attr("href", function (i, href) {
            return href + '?key=' + customerid;
        });

        if (akibaid == null) {
            link_akiba.addClass(disa);
            link_akiba_last.removeClass(disa);

            link_akiba_last.attr("href", function (i, href) {
                return href + '?key=' + customerid;
            });
        }
        else if (loanid == null) {
            link_loan.addClass(disa);
            link_loan_last.removeClass(disa);

            link_loan_last.attr("href", function (i, href) {
                return href + '?key=' + customerid;
            });
            link_akiba.attr("href", function (i, href) {
                return href + '?key=' + akibaid+'&name='+nameName;
            });
        }
        else {
            link_akiba.attr("href", function (i, href) {
                return href + '?key=' + akibaid + '&name=' + nameName;
            });
            link_loan.attr("href", function (i, href) {
                return href + '?key=' + loanid;
            });
        }
    }

    function returnerror(error) {
        alert(error.responseText);
    }

    function ToJavaScriptDate(value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
    }
});
