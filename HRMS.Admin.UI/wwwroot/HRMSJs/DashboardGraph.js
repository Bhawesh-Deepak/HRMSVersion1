$('#AssesmentYear').change(function () {
   
    GetDetail(1, this.value)
});
$(document).ready(function () {
  
    GetDetail(1,0);
});
function GetDetail(event,val) {
    debugger;

    $('#divLoader').modal('show');
    var AssesmentYear = $('#AssesmentYear').val();
    if (AssesmentYear == "") {
        AssesmentYear = 0;
    }
    
    if (event == 1) {
        MonthlyAttendanceGraph(AssesmentYear);
        $('#li1').addClass('active');
        $('#li2').removeClass('active'); 
        $('#li3').removeClass('active'); 
        $('#li4').removeClass('active'); 
    }
    else if (event == 2) {
        MonthlyGrossSalaryAndIncentiveGraph(AssesmentYear);
    }
    else if (event == 3) {
        NoOfEmployeeWhomSalaryPaidGraph(AssesmentYear);
    }
    else if (event == 4) {
        NoOfEmployeeWhomIncentivePaidGraph(AssesmentYear);


    }
}
function MonthlyAttendanceGraph(event) {
    var months = [];
    var presents = [];
    var absents = [];
    var employees = [];
    $.get("/Home/GetAttendanceGraph/", { FinancialYear: event }, function (response) {

        for (i = 0; i < response.length; i++) {
            months.push(response[i].monthsName);
            presents.push(response[i].presentDays);
            absents.push(response[i].lopDays);
            employees.push(response[i].totalEmployee);
        }

        Highcharts.chart('container', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Monthly Attendance Graph'
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: months,
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Value'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y:.1f}  </b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            },
            series: [{
                name: 'Total Present',
                data: presents,//[85966, 84572, 94347, 113794, 128530, 126383, 139812, 120122, 120791, 119776, 0, 0]

            }, {
                name: 'Total Absent',
                data: absents,//[17814, 13415, 8847, 8391, 3980, 7587, 16314, 8122, 12295, 11178, 0, 0]

            }, {
                name: 'Total Employees',
                data: employees,//[3721, 3242, 4015, 4435, 4956, 4759, 6251, 4649, 4529, 4532, 0, 0]

            }]
        });
    });
    url = "/Home/GetAttendanceDayWiseReport/"
    $.get(url, function (data) {
        $("#div_details").html(data);
        $('#divLoader').modal('hide');

    });
}
function MonthlyGrossSalaryAndIncentiveGraph(event) {
    $("#container").empty();
   
    $('#divLoader').modal('show');
    var Months = [];
    var Gross = [];
    var PI = [];
    $.get("/Home/GetGrossSalaryAndPIGraph/", { FinancialYear: event }, function (response) {
        $('#divLoader').modal('show');
        for (i = 0; i < response.length; i++) {
            Months.push(response[i].monthsName);
            Gross.push(response[i].grossSalary);
            PI.push(response[i].piAmount);
        }
        var chart = Highcharts.chart('container', {

            chart: {
                type: 'column'
            },

            title: {
                text: 'This Year Gross Salary & Performance Insentive'
            },

            subtitle: {
                text: ' '
            },

            //legend: {
            //    align: 'top',
            //    verticalAlign: 'middle',
            //    layout: 'vertical'
            //},

            xAxis: {
                categories: Months,//['Apples', 'Oranges', 'Bananas'],
                labels: {
                    x: -10
                }
            },

            yAxis: {
                allowDecimals: false,
                title: {
                    text: 'Amount'
                }
            },

            series: [{
                name: 'Gross Salary',
                data: Gross,//[6, 4, 2]
            }, {
                name: 'Performance Incentive',
                data: PI,//[8, 4, 3]
            }],

            responsive: {
                rules: [{
                    condition: {
                        maxWidth: 500
                    },
                    chartOptions: {
                        legend: {
                            align: 'center',
                            verticalAlign: 'bottom',
                            layout: 'horizontal'
                        },
                        yAxis: {
                            labels: {
                                align: 'left',
                                x: 0,
                                y: -5
                            },
                            title: {
                                text: null
                            }
                        },
                        subtitle: {
                            text: null
                        },
                        credits: {
                            enabled: false
                        }
                    }
                }]
            }
        });
        $('#divLoader').modal('show');
        var url = "/Home/GetPaidRegister/"
        $.get(url, function (data) {
            $("#div_details").html(data);
            $('#divLoader').modal('hide');
        });
    });
    
}
function NoOfEmployeeWhomSalaryPaidGraph(event) {
    $("#container").empty();
    $("#div_details").empty();
    var chartresponse = [];
    $.get("/Home/GetNoOfEmployeeWhomSalaryPaidGraph/", { FinancialYear: event }, function (response) {
        debugger;
        for (i = 0; i < response.length; i++) {
            var a = response[i].monthsName;
            var b = response[i].noOfEmployee;
            chartresponse.push([a, b]);
        }
        Highcharts.chart('container', {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 45
                }
            },
            title: {
                text: 'Number of Employees to whom Salary Paid '
            },
            subtitle: {
                text: ' '
            },
            plotOptions: {
                pie: {
                    innerSize: 100,
                    depth: 45
                }
            },
            series: [{
                name: 'Salary Paid  Employees',
                data: chartresponse,

            }]
        });
        $('#divLoader').modal('hide');
    });
}
function NoOfEmployeeWhomIncentivePaidGraph(event) {
    $("#div_details").empty();
    $("#container").empty();
    var chartresponse = [];
    $.get("/Home/GetNoOfEmployeeWhomSalaryPaidGraph/", { FinancialYear: event }, function (response) {

        for (i = 0; i < response.length; i++) {
            var a = response[i].monthsName;
            var b = response[i].noOfEmployee;
            chartresponse.push([a, b]);
        }
        Highcharts.chart('container', {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 45
                }
            },
            title: {
                text: 'Number of Employees to whom Incentive Paid '
            },
            subtitle: {
                text: ' '
            },
            plotOptions: {
                pie: {
                    innerSize: 100,
                    depth: 45
                }
            },
            series: [{
                name: 'Incentive Paid  Employees',
                data: chartresponse,
            }]
        });
        $('#divLoader').modal('hide');
    });
}