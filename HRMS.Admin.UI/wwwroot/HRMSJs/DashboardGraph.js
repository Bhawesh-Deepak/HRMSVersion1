
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
        categories: [

            'Apr',
            'May',
            'Jun',
            'Jul',
            'Aug',
            'Sep',
            'Oct',
            'Nov',
            'Dec',
            'Jan',
            'Feb',
            'Mar'
        ],
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
        data: [85966, 84572, 94347, 113794, 128530, 126383, 139812, 120122, 120791, 119776, 0, 0]

    }, {
        name: 'Total Absent',
            data: [17814, 13415, 8847, 8391, 3980, 7587, 16314, 8122, 12295, 11178, 0, 0]

    }, {
        name: 'Total Employees',
            data: [3721, 3242, 4015, 4435, 4956, 4759, 6251, 4649, 4529, 4532, 0, 0]

    }]
});
//------------------------
Highcharts.chart('container1', {
    data: {
        table: 'datatable'
    },
    chart: {
        type: 'column'
    },
    title: {
        text: 'Monthly Gross Salary and Incentive'
    },
    yAxis: {
        allowDecimals: false,
        title: {
            text: 'Units'
        }
    },
    tooltip: {
        formatter: function () {
            return '<b>' + this.series.name + '</b><br/>' +
                this.point.y + ' ' + this.point.name.toLowerCase();
        }
    }
});

//---------------------
Highcharts.chart('container2', {
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
        data: [
            ['Apr', 3720],
            ['May', 3212],
            ['Jun', 4014],
            ['Jul', 4431],
            ['Aug', 4955],
            ['Sep', 4758],
            ['Oct', 4998],
            ['Nov', 4694],
            ['Dec', 4528],
            ['Jan', 4528],
            ['Feb', 0],
            ['Mar', 0]
        ]
    }]
});
//-------------------
Highcharts.chart('container3', {
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
        name: 'Number of Employees to whom Incentive Paid',
        data: [
            ['Apr', 778],
            ['May', 519],
            ['Jun', 555],
            ['Jul', 580],
            ['Aug', 518],
            ['Sep', 610],
            ['Oct', 661],
            ['Nov', 532],
            ['Dec', 719],
            ['Jan', 761],
            ['Feb', 0],
            ['Mar', 0]
        ]
    }]
});