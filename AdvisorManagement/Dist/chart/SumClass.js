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
         


 
     
            const chart = Highcharts.chart('container', {
                title: {
                    text: 'Phân công lớp CVHT',
                    align: 'left'
                },
                colors: [
                    '#4caefe',
                    '#3fbdf3',
                    '#35c3e8',
                    '#2bc9dc',
                    '#20cfe1',
                    '#16d4e6',
                    '#0dd9db',
                    '#03dfd0',
                    '#00e4c5',
                    '#00e9ba',
                    '#00eeaf',
                    '#23e274'
                ],
                xAxis: {
                    categories: JsonDataCLName,
                    title: {
                        text : ""
                    }
                },
                yAxis: {
                 
                    title: {
                        text: "Lớp học"
                    }
                },
                series: [{
                    type: 'column',
                    name: 'Lớp đảm nhận',
                    borderRadius: 5,
                    colorByPoint: true,
                    data: JsonDataCLValue,
                    showInLegend: false
                }]
            });



        }
    })
});