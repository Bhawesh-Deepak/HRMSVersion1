
//First Row of Table Set Selected Functionality
$(document).ready(function () {
    setTimeout(function () {
        $('table.singleSelect tbody tr:first').addClass("selected").click();
        $('table.multipleSelect tbody tr:first').addClass("selected").click();
    }, 500);


});

var jqGrigTable = {
    "fn_GetTablePageIndex": function (pageIndex, url, divHolder = "#divEmployeeDetails", formId = "SearchForm") {
        var $container = $(divHolder);
        var pageSize = parseInt($container.find('#pagesizevalue option:selected').text());
        this.fn_GetTableSortingOnEachPage(pageIndex, pageSize, url, divHolder, formId);
    },

    "fn_GetTablePageSize": function (obj, url, divHolder = "#divEmployeeDetails", formId = "SearchForm") {
        var pageSize = parseInt($(obj).find('option:selected').text());
        var pageIndex = 1;
        this.fn_GetTableSortingOnEachPage(pageIndex, pageSize, url, divHolder, formId);
    },

    "fn_GetTableSortingOnEachPage": function (pageIndex, pageSize, url, divHolder = "#divEmployeeDetails", formId = "SearchForm") {
        var sortingName = "";
        var sortingIco = "Ascending";
        var $container = $(divHolder);
        var isSortingFound = false;
        $.each($container.find('table .sortingIco'),
            function (index, value) {
                //debugger;
                if ($(value).attr('class') === 'sortingIco desc') {
                    sortingName = $(value).parent().parent().find('span').text();
                    sortingIco = "ASC";
                    isSortingFound = true;
                    return false;
                } else if ($(value).attr('class') === 'sortingIco ace') {
                    sortingName = $(value).parent().parent().find('span').text();
                    sortingIco = "DESC";
                    isSortingFound = true;
                    return false;
                }

            });
        if (!isSortingFound) {
         
            sortingName = $container.find('table .th:first').parent().find('span').text();
            sortingIco = "Ascending";
        }
     
        this.fn_GetTableSearchList(sortingName.trim(), sortingIco, pageIndex, pageSize, url, divHolder, formId);
    },

    "fn_GetSortingItem": function (sortBy, sortOrder, url, divHolder = "#divEmployeeDetails", formId = "SearchForm") {
        var $container = $(divHolder);
        var pageIndex = parseInt($container.find('.pagination .active').text());
        var pageSize = parseInt($container.find('#pagesizevalue option:selected').text());
        this.fn_GetTableSearchList(sortBy, sortOrder, pageIndex, pageSize, url, divHolder, formId);
    },

    "fn_GetTableSearchList": function (sortBy, sortOrder, pageindex, pageSize, url, divHolder = "#divEmployeeDetails", formId = "SearchForm") {
     
        if (sortOrder === "ASC") {
            sortOrder = "DESC";
        }
        else if (sortOrder === "DESC") {
            sortOrder = "ASC";
        }
        else
            sortOrder = "ASC";



        var obj = {};
        $("#" + formId).serializeArray().map(function (x) {
            obj[x.name] = x.value;
        });
        parameter = obj;

        $(divHolder).addClass('ajaxLoading');
        $.ajax({
            type: "POST",
            url: url,
            data: { searchModelEntity: parameter, searchBy: "", sortBy: sortBy, sortOrder: sortOrder, pageIndex: pageindex, pageSize: pageSize },
            cache: false,
            success: function (data) {
                $("#divEmployeeDetails").html(data);
                debugger;
                $("#divEmployeeDetails").removeClass('ajaxLoading');
                $("#pagesizevalue option").filter(function (index) { return $(this).text() === String(pageSize); }).attr('selected', 'selected');
                if ($(data).find('tbody tr:not(".norecord")').length == 0) {
                    setTimeout(function () {
                        $(divHolder).find('fieldset [type="text"]:first').focus();
                    }, 200);
                }
            }
        });
    },

    "fn_GetLoadingDiv": function (strLoadindDiv) {
        if (typeof strLoadindDiv === "undefined") {
            return "#divHRMSDetails";
        }
        return strLoadindDiv;
    }
};
