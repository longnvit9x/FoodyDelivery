var baseUrl = $("#BaseUrl").data("baseurl");
var storeID = $("#BaseUrl").data("storeid");
//chart cot

var MONTHS = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
var color = Chart.helpers.color;
var barChartData = {
    //["January", "February", "March", "April", "May", "June", "July"]
    labels: labelMonth(new Date().getMonth() + 1, new Date().getFullYear()),
    datasets: [{
        label: 'Dataset 1',
        Unit: "",
        backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
        borderColor: window.chartColors.red,
        borderWidth: 1,
        data: [
            randomScalingFactor(),
            randomScalingFactor(),
            randomScalingFactor(),
            randomScalingFactor(),
            randomScalingFactor(),
            randomScalingFactor(),
            randomScalingFactor()
        ]
    }, {
        label: 'Dataset 2',
        backgroundColor: color(window.chartColors.blue).alpha(0.5).rgbString(),
        borderColor: window.chartColors.blue,
        borderWidth: 1,
        data: [
            randomScalingFactor(),
            randomScalingFactor(),
            randomScalingFactor(),
            randomScalingFactor(),
            randomScalingFactor(),
            randomScalingFactor(),
            randomScalingFactor()
        ]
    }]

};

// Define a plugin to provide data labels
Chart.plugins.register({
    afterDatasetsDraw: function (chart, easing) {
        // To only draw at the end of animation, check for easing === 1
        var ctx = chart.ctx;

        chart.data.datasets.forEach(function (dataset, i) {
            var meta = chart.getDatasetMeta(i);
            if (!meta.hidden) {
                meta.data.forEach(function (element, index) {
                    // Draw the text in black, with the specified font
                    ctx.fillStyle = 'rgb(0, 0, 0)';

                    var fontSize = 10;
                    var fontStyle = 'normal';
                    var fontFamily = 'Times New Roman';
                    ctx.font = Chart.helpers.fontString(fontSize, fontStyle, fontFamily);

                    // Just naively convert to string for now
                    var dataString = (dataset.data[index] > 0) ? dataset.data[index].toString() + dataset.Unit : "";

                    // Make sure alignment settings are correct
                    ctx.textAlign = 'center';
                    ctx.textBaseline = 'middle';

                    var padding = 5;
                    var position = element.tooltipPosition();
                    ctx.fillText(dataString, position.x, position.y - (fontSize / 2) - padding);
                });
            }
        });
    }
});

window.onload = function () {
  
};

var colorNames = Object.keys(window.chartColors);




$(document).ready(function () {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    $('#slmonth').val(mm);
    $('#slyear').val(yyyy);
    loadThongKe();
});
var glomonths;
var gloyears;
function loadThongKe() {
    glomonths = $('#slmonth').val();
    gloyears = $('#slyear').val();

    var url = baseUrl + "store/getThongKe";
    var data = {
        StoreID: storeID,
        Month: glomonths,
        Year: gloyears
    };
    executeGetNew(url, data, loadThongKeSuccess, loadThongKeError);
}

function loadThongKeSuccess(data) {
    if (data != null) {
        if (myBar == null || myLine == null) {
            defaulf();
        }
        barChartData.labels = labelMonth(glomonths, gloyears);
        barChartData.datasets = [];
        lineChartData.labels = labelMonth(glomonths, gloyears);
        lineChartData.datasets = [];
        var colorName = colorNames[barChartData.datasets.length % colorNames.length];;
        var dsColor = window.chartColors[colorName];
        var newDSBaiViet = {
            label: 'Lượt đánh giá (đơn vị: lượt(L))',
            Unit: "L",
            backgroundColor: color(window.chartColors.green).alpha(0.5).rgbString(),
            borderColor: window.chartColors.green,
            borderWidth: 1,
            data: []
        };
        var pt = data.Posts;
        var posts = $.map(pt, function (value, index) {
            return [value];
        });
        for (var i = 0; i < 31; i++) {
            for (var index = 0; index < posts.length; ++index) {
                if (posts[index].PostTime == i + 1) {
                    newDSBaiViet.data.push(posts[index].NumberPost);
                    break;
                }
                else {
                    if (index == posts.length - 1) {
                        newDSBaiViet.data.push(0);
                    }
                }

            }
        }

        barChartData.datasets.push(newDSBaiViet);

        var newDSDonHang = {
            label: 'Đơn đặt hàng (đơn vị: đơn(Đ)',
            Unit: "Đ",
            backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
            borderColor: window.chartColors.red,
            borderWidth: 1,
            data: []
        };
        var iv = data.Invoices;
        var invoices = $.map(iv, function (value, index) {
            return [value];
        });
        for (var i = 0; i < 31; i++) {
            for (var index = 0; index < invoices.length; ++index) {

                if (invoices[index].OrderDate == i + 1) {
                    newDSDonHang.data.push(invoices[index].NumberInvoice);
                    break;
                }
                else {
                    if (index == invoices.length - 1) {
                        newDSDonHang.data.push(0);
                    }
                }

            }
        }

        barChartData.datasets.push(newDSDonHang);


        // doanh so
        //var newDSDoanhSo = {
        //    label: 'Doanh số (đơn vị: nghìn đồng(Đ)',
        //    Unit: "Đ",
        //    backgroundColor: color(window.chartColors.green).alpha(0.5).rgbString(),
        //    borderColor: window.chartColors.green,
        //    data: [],
        //    type: 'line',
        //    pointRadius: 0,
        //    fill: false,
        //    //borderWidth: 1,
        //    lineTension: 0,
        //};
        var newDSDoanhSo = {
            type:"line",
            label: 'Doanh số (đơn vị: nghìn đồng(Đ)',
            Unit: "Đ",
            borderColor: window.chartColors.red,
            backgroundColor: window.chartColors.red,
            fill: false,
            data: [],
            yAxisID: "y-axis-1",
        }

        var ds = data.Revenues;
        var dss = $.map(ds, function (value, index) {
            return [value];
        });
        for (var i = 0; i < 31; i++) {
            for (var index = 0; index < dss.length; ++index) {

                if (dss[index].Day == i + 1) {
                    newDSDoanhSo.data.push(dss[index].Money);
                    break;
                }
                else {
                    if (index == dss.length - 1) {
                        newDSDoanhSo.data.push(0);
                    }
                }

            }
        }

        lineChartData.datasets.push(newDSDoanhSo);
        //var newDonHang = {
        //    label: 'Đơn hàng (đơn vị: đơn(Đh)',
        //    Unit: "Đh",
        //    backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
        //    borderColor: window.chartColors.red,
        //    data: [],
        //    type: 'bar',
        //    pointRadius: 0,
        //    fill: false,
        //    //borderWidth: 1,
        //    lineTension: 0,
        //};
        var newDonHang = {
            label: 'Đơn hàng (đơn vị: đơn(Đh)',
            Unit: "Đh",
            type: 'line',
            lineTension: 0,
            borderColor: window.chartColors.green,
            backgroundColor: window.chartColors.green,
            fill: false,
            data: [],
            yAxisID: "y-axis-2",
        }
        for (var i = 0; i < 31; i++) {
            for (var index = 0; index < dss.length; ++index) {

                if (dss[index].Day == i + 1) {
                    newDonHang.data.push(dss[index].NumberInvoice);
                    break;
                }
                else {
                    if (index == dss.length - 1) {
                        newDonHang.data.push(0);
                    }
                }

            }
        }

        lineChartData.datasets.push(newDonHang);
        myBar.update();
        myLine.update();
    } else {
        defaulf();
    }
}

var myLine;
var myBar;
function defaulf() {
    var ctx = document.getElementById("canvas").getContext("2d");
    myBar = new Chart(ctx, {
        type: 'bar',
        data: barChartData,
        options: {
            responsive: true,
            legend: {
                position: 'bottom',
            },
            title: {
                display: true,
            },
            scales: {
                xAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: "Ngày trong tháng"
                    }
                }],
                yAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: "Đơn vị"
                    }
                }]
            }
        }
    });

    var ctx = document.getElementById("myline").getContext("2d");
    myLine = Chart.Line(ctx, {
        data: lineChartData,
        options: {
            responsive: true,
            hoverMode: 'index',
            stacked: false,
            legend: {
                position: 'bottom',
            },
            title: {
                display: true,
            },
            scales: {
                xAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: "Ngày trong tháng"
                    }
                }],
                yAxes: [{
                    // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                    display: true,
                    position: "left",
                    id: "y-axis-1",
                    labelString: "Doanh số (Đ)"
                }, {
                    // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                    display: true,
                    position: "right",
                    id: "y-axis-2",
                    labelString: "Đơn hàng (Đh)",
                    // grid line settings
                    gridLines: {
                        drawOnChartArea: false, // only want the grid lines for one axis to show up
                    },
                }],
            }
        }
    });
}

function loadThongKeError(data) {
    display("Đã xảy ra lỗi", "Không thể thống kê");
}

function daysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}
function labelMonth(months, years) {
    var lbl = [];
    for (i = 0; i < daysInMonth(months, years) ; i++) {
        lbl.push(i + 1);
    }
    return lbl;
}



//function randomNumber(min, max) {
//    return Math.random() * (max - min) + min;
//}

//function randomBar(date, lastClose) {
//    var open = randomNumber(lastClose * .95, lastClose * 1.05);
//    var close = randomNumber(open * .95, open * 1.05);
//    var high = randomNumber(Math.max(open, close), Math.max(open, close) * 1.1);
//    var low = randomNumber(Math.min(open, close) * .9, Math.min(open, close));
//    return {
//        t: date.valueOf(),
//        y: close
//    };
//}


//var dateFormat = 'MMMM DD YYYY';
//var date = moment('April 01 2017', dateFormat);
//var data = [randomBar(date, 30)];
//var labels = [date];
//while (data.length < 31) {
//    date = date.clone().add(1, 'd');
//    if (date.isoWeekday() <= 5) {
//        data.push(randomBar(date, data[data.length - 1].y));
//        labels.push(date);
//    }
//}

//var lineChartData = {
//    type: 'bar',
//    data: {
//        labels: labelMonth(new Date().getMonth() + 1, new Date().getFullYear()),
//        datasets: [{
//            label: 'Dataset 1',
//            backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
//            borderColor: window.chartColors.red,
//            data: data,
//            type: 'line',
//            //pointRadius: 0,
//            //fill: false,
//            //lineTension: 0,
//            borderWidth: 2
//        }]
//    },
//    options: {
//        responsive: true,
//        title: {
//            display: true,
//        },
//        legend: {
//            position: 'bottom',
//        },
//        scales: {
//            xAxes: [{
//                scaleLabel: {
//                    display: true,
//                    labelString: 'Ngày trong tháng'
//                }
//            }],
//            yAxes: [{
//                scaleLabel: {
//                    display: true,
//                    labelString: 'Đơn vị',
//                    position: "right",
//                }
//            }]
//        }
//    }
//};

//document.getElementById('update').addEventListener('click', function () {
//    var type = document.getElementById('type').value;
//    lineChartData.data.datasets[0].type = type;
//    chart.update();
//});
//var chart;

//var ctx = document.getElementById("chart1").getContext("2d");
//chart = new Chart(ctx, lineChartData);


var lineChartData = {
    labels: labelMonth(new Date().getMonth() + 1, new Date().getFullYear()),
    datasets: [{
        label: "My First dataset",
        Unit:"đ",
        borderColor: window.chartColors.red,
        backgroundColor: window.chartColors.red,
        fill: false,
        data: [],
        yAxisID: "y-axis-1",
    }, {
        label: "My Second dataset",
        borderColor: window.chartColors.blue,
        backgroundColor: window.chartColors.blue,
        fill: false,
        data: [],
        yAxisID: "y-axis-2"
    }]
};
