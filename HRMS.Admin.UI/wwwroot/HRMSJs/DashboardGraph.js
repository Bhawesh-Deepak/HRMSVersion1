
var blue = "#348fe2",
    blueLight = "#5da5e8",
    blueDark = "#1993E4",
    aqua = "#49b6d6",
    aquaLight = "#6dc5de",
    aquaDark = "#3a92ab",
    green = "#00acac",
    greenLight = "#33bdbd",
    greenDark = "#008a8a",
    orange = "#f59c1a",
    orangeLight = "#f7b048",
    orangeDark = "#c47d15",
    dark = "#2d353c",
    grey = "#b6c2c9",
    purple = "#727cb6",
    purpleLight = "#8e96c5",
    purpleDark = "#5b6392",
    red = "#ff5b57";
var white = "rgba(255,255,255,1.0)",
    fillBlack = "rgba(45, 53, 60, 0.6)",
    fillBlackLight = "rgba(45, 53, 60, 0.2)",
    strokeBlack = "rgba(45, 53, 60, 0.8)",
    highlightFillBlack = "rgba(45, 53, 60, 0.8)",
    highlightStrokeBlack = "rgba(45, 53, 60, 1)",
    fillBlue = "rgba(52, 143, 226, 0.6)",
    fillBlueLight = "rgba(52, 143, 226, 0.2)",
    strokeBlue = "rgba(52, 143, 226, 0.8)",
    highlightFillBlue = "rgba(52, 143, 226, 0.8)",
    highlightStrokeBlue = "rgba(52, 143, 226, 1)",
    fillGrey = "rgba(182, 194, 201, 0.6)",
    fillGreyLight = "rgba(182, 194, 201, 0.2)",
    strokeGrey = "rgba(182, 194, 201, 0.8)",
    highlightFillGrey = "rgba(182, 194, 201, 0.8)",
    highlightStrokeGrey = "rgba(182, 194, 201, 1)",
    fillGreen = "rgba(0, 172, 172, 0.6)",
    fillGreenLight = "rgba(0, 172, 172, 0.2)",
    strokeGreen = "rgba(0, 172, 172, 0.8)",
    highlightFillGreen = "rgba(0, 172, 172, 0.8)",
    highlightStrokeGreen = "rgba(0, 172, 172, 1)",
    fillPurple = "rgba(114, 124, 182, 0.6)",
    fillPurpleLight = "rgba(114, 124, 182, 0.2)",
    strokePurple = "rgba(114, 124, 182, 0.8)",
    highlightFillPurple = "rgba(114, 124, 182, 0.8)",
    highlightStrokePurple = "rgba(114, 124, 182, 1)",
    randomScalingFactor = function () { return Math.round(100 * Math.random()) };
$(document).ready(function () {
    BindMonthlyAttendanceGraph();
    MonthlyGrossSalaryIncentive();
    NoofEmployeeSalaryPaid();
    NoofEmployeeIncentivePaid();
});
$("#AssesmentYear1").change(function () {
    BindMonthlyAttendanceGraph($("#AssesmentYear1").val());
});
$("#AssesmentYear2").change(function () {
    MonthlyGrossSalaryIncentive($("#AssesmentYear2").val());
});
$("#AssesmentYear3").change(function () {
    NoofEmployeeSalaryPaid($("#AssesmentYear3").val());
});
$("#AssesmentYear4").change(function () {
    NoofEmployeeIncentivePaid($("#AssesmentYear4").val());
});
function BindMonthlyAttendanceGraph(event) {
    debugger;
    var ctx = document.getElementById("myChart-4").getContext("2d");
    var data = {
        labels: ["Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar"],
        datasets: [{
            label: "Total Present",
            backgroundColor: fillBlue,
            data: [85966, 84572, 94347, 113794, 128530, 126383, 139812, 120122, 120791, 0, 0, 0]
        }, {
            label: "Total Absent",
            backgroundColor: strokePurple,
            data: [17814, 13415, 8847, 8391, 3980, 7587, 16314, 8122, 12295, 0, 0, 0]
        }, {
            label: "Total Employees",
            backgroundColor: orangeLight,
            data: [3721, 3242, 4015, 4435, 4956, 4759, 6251, 4649, 4529, 0, 0, 0]
        }]
    };
    var myBarChart = new Chart(ctx, {
        type: 'bar',
        data: data,
        options: {
            barValueSpacing: 10,
            scales: {
                yAxes: [{
                    ticks: {
                        min: 0,
                    }
                }]
            }
        }
    });
}
function MonthlyGrossSalaryIncentive(event) {
    var ctx = document.getElementById("myChart-5").getContext("2d");
    var data = {
        labels: ["Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar"],
        datasets: [{
            label: "Gross Salary",
            backgroundColor: '#33bdbd',
            data: [116500018, 112429557, 110892602, 126680702, 132873168, 143819752, 155044084, 149415537, 162666193, 0, 0, 0]
        }, {
            label: "Performance Incentives",
            backgroundColor: "#8e96c5",
            data: [18905792, 16779774, 10015738, 13161981, 8895766, 9628087, 11027584, 14080574, 13698153, 0, 0, 0]
        }]
    };

    var myBarChart = new Chart(ctx, {
        type: 'bar',
        data: data,
        options: {
            barValueSpacing: 10,
            scales: {
                yAxes: [{
                    ticks: {
                        min: 0,
                    }
                }]
            }
        }
    });
}
function NoofEmployeeSalaryPaid(event) {
    var xValues = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var yValues = [0, 0, 0, 3720, 3212, 4014, 4431, 4955, 4758, 4998, 4694, 4528];
    var barColors = [
        "#60FBA9",
        "#FBE860",
        "#FBC360",
        "#0BB26E",
        "#0BB2AD",
        "#F8C471",
        "#FADBD8",
        "#21618C",
        "#BCCF05",
        "#204D5F",
        "#EEABAD",
        "#B9D1E3"
    ];
    new Chart("myChart-2", {
        type: "doughnut",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues
            }]
        },
        options: {
            title: {
                display: false,
                text: "World Wide Wine Production 2018"
            }
        }
    });
}
function NoofEmployeeIncentivePaid(event) {
    var xValues = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var yValues = [0, 0, 0, 778, 519, 555, 580, 518, 610, 661, 532, 719];
    var barColors = [
        "#60FBA9",
        "#FBE860",
        "#FBC360",
        "#0BB26E",
        "#0BB2AD",
        "#F8C471",
        "#FADBD8",
        "#21618C",
        "#BCCF05",
        "#204D5F",
        "#EEABAD",
        "#B9D1E3"
    ];
    new Chart("myChart-3", {
        type: "doughnut",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues
            }]
        },
        options: {
            title: {
                display: false,
                text: "World Wide Wine Production 2018"
            }
        }
    });
}

