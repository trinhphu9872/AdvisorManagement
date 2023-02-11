$(document).ready(function () {
  
    $.ajax({
        type: "POST",
        url: 'DashboardFileAdvisor',
        data: JSON.stringify({}),
        contentType: "application/json:charset=utf-8",
        dataType: "json",
        success: function (json) {
            let valuesNameAd = json.nameAd;
            let valuesCountAd = json.valueAd;
            let JsonDataAdName = [];
            let JsonDataAdValue = [];


            for (let i = 0; i < valuesNameAd.length; i++) {
                let nameAd = valuesNameAd[i];
                let valueAd = valuesCountAd[i];
                JsonDataAdName.push(nameAd);
                JsonDataAdValue.push(parseInt(valueAd))
            }

            // Set up the chart
            const chart = new Highcharts.Chart({
                chart: {
                    renderTo: 'FileManager',
                    type: 'column',
                    options3d: {
                        enabled: true,
                        alpha: 15,
                        beta: 15,
                        depth: 50,
                        viewDistance: 25
                    }
                },
                xAxis: {
                    categories: JsonDataAdName
                },
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                tooltip: {
                    headerFormat: '<b>{point.key}</b><br>',
                    pointFormat: 'File đã nộp: {point.y}'
                },
                title: {
                    text: 'Số file minh chứng của cố vấn học tập',
                    align: 'center'
                },
                subtitle: {
                    text: 'Source: ' +
                        '<a href="https://ofv.no/registreringsstatistikk"' +
                        'target="_blank">Capstone Team 09</a>',
                    align: 'center'
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    column: {
                        depth: 25
                    }
                },
                series: [{
                    data: JsonDataAdValue,
                    colorByPoint: true
                }]
            });


          

        }
    })
});
