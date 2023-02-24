$(document).ready(function () {

    $.ajax({
        type: "POST",
        url: 'DashboardSum',
        data: JSON.stringify({}),
        contentType: "application/json:charset=utf-8",
        dataType: "json",
        success: function (json) {
            let valuesName = json.name;
            let valuesCount = json.value;
            let JsonData = [];
            let tong = valuesCount.reduce((a, b) => parseInt(a) + parseInt(b), 0);
            for (let i = 0; i < valuesName.length; i++) {
                let name = valuesName[i];
                let value = valuesCount[i];
                let objAdd = {
                    name: `${name}`,
                    y: parseInt(value)
                };
                JsonData.push(objAdd);
            }
            Highcharts.chart('ChartStudentAndAdvisor', {
                chart: {
                    type: 'pie',
                    options3d: {
                        enabled: true,
                        alpha: 45,
                        beta: 0
                    }
                },
                title: {
                    text: 'Tổng số sinh viên và cố vấn học tập',
                    align: 'center'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        depth: 35,
                        dataLabels: {
                            enabled: true,
                            format: '{point.name}: {point.y} người'
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: 'Share',
                    data: JsonData
                }]
            });
            //// Create the chart
            //Highcharts.chart('ChartStudentAndAdvisor', {
            //    chart: {
            //        type: 'pie',
            //        options3d: {
            //            enabled: true,
            //            alpha: 45,
            //            beta: 0
            //    },
            //    title: {
            //        text: 'Tổng số sinh viên và cố vấn học tập',
            //        align: 'center'
            //    },
            //    accessibility: {
            //        announceNewData: {
            //            enabled: true
            //        },
            //        point: {
            //            valueSuffix: 'Người'
            //        }
            //    },

            //    plotOptions: {
            //        series: {
            //            dataLabels: {
            //                enabled: true,
            //                format: '{point.name}: {point.y} người'
            //            }
            //        }
            //    },

            //    tooltip: {
            //        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            //        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
            //    },

            //    series: [
            //        {
            //            name: 'Browsers',
            //            colorByPoint: true,
            //            data: JsonData
            //        }
            //    ],
            //});
        }
    })
});