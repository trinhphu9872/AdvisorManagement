$(document).ready(function () {

    $.ajax({
        type: "POST",
        url: 'DashboardClass',
        data: JSON.stringify({}),
        contentType: "application/json:charset=utf-8",
        dataType: "json",
        success: function (json) {
            let valuesNameCL = json.nameCl;
            let valuesCountCL = json.valueCl;
            let JsonDataCLName = [];
            let JsonDataCLValue = [];
            

            for (let i = 0; i < valuesNameCL.length; i++) {
                let nameAd = valuesNameCL[i];
                let valueAd = valuesCountCL[i];
                JsonDataCLName.push(nameAd);
                JsonDataCLValue.push(parseInt(valueAd))
            }
    

            // Set up the chart
            Highcharts.chart('chartClass', {
                chart: {
                    type: 'cylinder',
                    options3d: {
                        enabled: true,
                        alpha: 15,
                        beta: 15,
                        depth: 50,
                        viewDistance: 25
                    }
                },
                title: {
                    text: 'Số lớp cố vấn đảm nhận'
                },
                subtitle: {
                    text: 'Source: ' +
                        '<a href="https://www.fhi.no/en/id/infectious-diseases/coronavirus/daily-reports/daily-reports-COVID19/"' +
                        'target="_blank">Capstone Team 09</a>'
                },
                xAxis: {
                    categories: JsonDataCLName,
                    title: {
                        text: 'Cố vấn'
                    }
                },
                yAxis: {
                    title: {
                        margin: 20,
                        text: 'Tổng lớp'
                    }
                },
                tooltip: {
                    headerFormat: '<b>Số lượng lớp: {point.x}</b><br>'
                },
                plotOptions: {
                    series: {
                        depth: 25,
                        colorByPoint: true
                    }
                },
                series: [{
                    data: JsonDataCLValue,
                    name: 'Số lượng lớp',
                    showInLegend: false
                }]
            });




        }
    })
});
