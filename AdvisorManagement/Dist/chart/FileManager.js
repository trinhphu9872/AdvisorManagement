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
            let date = new Date();
            let year = date.getFullYear();


            for (let i = 0; i < valuesNameAd.length; i++) {
                let nameAd = valuesNameAd[i];
                let valueAd = valuesCountAd[i];
                JsonDataAdName.push(nameAd);
                JsonDataAdValue.push(parseInt(valueAd))
            }

            const chart = Highcharts.chart('FileManager', {
                title: {
                    text: 'Tổng quan báo cáo',
                    align: 'center'
                },
                colors: ['#fe8401', '#fd7d01', '#fb7601', '#fa6f01', '#f96801', '#f86101', '#f65a01', '#f55301', '#f44c01', '#f34601', '#f13f01', '#f03801', '#ef3101', '#ee2a01', '#ec2301', '#eb1c01', '#ea1501', '#e90e01', '#e70701', '#e60001'],
                xAxis: {
                    categories: JsonDataAdName,
                    title: {
                        text: "Tổng quan báo cáo năm " + year 
                    }
                },
                yAxis: {

                    title: {
                        text: "Báo cáo"
                    }
                },
                series: [{
                    type: 'column',
                    name: 'Báo cáo',
                    borderRadius: 5,
                    colorByPoint: true,
                    data: JsonDataAdValue,
                    showInLegend: false
                }]
            });




        }
    })
});