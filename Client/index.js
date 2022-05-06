
// $(document).ready(function () {
    // var times = [];
    // var maxRxLevels = [];
    // var maxTxLevels = [];
    // var rsL_DEVIATIONs = [];

    // var timesD = [];
    // var maxRxLevelsD = [];
    // var maxTxLevelD = [];
    // var rsL_DEVIATIONsD = [];


    function getValue(radio) {
        var chart = $("#chart").data("kendoChart");
        // chart.refresh();
        var fetcheddata = [];
        var times = [];
        var maxRxLevels = [];
        var maxTxLevels = [];
        var rsL_DEVIATIONs = [];
        $.getJSON("https://localhost:44338/api/Fetch/" + radio.value, function (data) {
            console.log(radio.value);
            for (var i in data) {
                fetcheddata.push(data[i]);
                maxRxLevels.push(data[i].maxRxLevel)
                maxTxLevels.push(data[i].maxTxLevel)
                rsL_DEVIATIONs.push(data[i].rsL_DEVIATION)
                times.push(data[i].time)
            }
            console.log(data)

            var data_Source = new kendo.data.DataSource({

                transport: {
                    read: {
                        url: "https://localhost:44338/api/Fetch/" + radio.value,
                        dataType: "JSON"
                    }
                },
        
                schema: {
                    model: {
                        fields: {
                            time: { type: "string" },
                            link: { type: "string" },
                            maxRxLevel: { type: "number" },
                            maxTxLevel: { type: "number" },
                            rsL_DEVIATION: { type: "number" }
                        }
                    }
                },
        
                sort: {
                    field: "Time",
                    dir: "desc"
                },
            });
        

            $("#grid").kendoGrid({
                sortable: true,
                filterable: true,
                columns: [
                    {
                        field: "time",
                        title: "Time"
                    },
                    {
                        field: "link",
                        title: "Link"
                    },
                    {
                        field: "maxRxLevel",
                        title: "MaxRxLevel"
                    },
                    {
                        field: "maxTxLevel",
                        title: "MaxTxLevel"
                    },
                    {
                        field: "rsL_DEVIATION",
                        title: "RSL_DEVIATION"
                    }
                ],
                dataSource: fetcheddata
            });


            data_Source.fetch(function () {
                $("#chart").kendoChart({
                title: {
                    text: "KPIS DISPLAYS"
                },
                legend: {
                    position: "bottom"
                },
                seriesDefaults: {
                    type: "line"
                },
                series: [{
                    name: "MaxRxLevel",
                    data: maxRxLevels
                },
                {
                    name: "MaxTxLevel",
                    data: maxTxLevels
                },
                {
                    name: "RSL_DEVIATION",
                    data: rsL_DEVIATIONs
                }
                ],
                valueAxis: {
                    labels: {
                        format: "{0:n3}"

                    }
                },
                categoryAxis: {
                    categories: times,
                    type: "date",
                    labels: {
                        dateFormats: {
                            days: "M-d-y hh:mm:ss"
                        }
                    }
                },
            });
            });
        });
    }

    // $("#start").datepicker({
    //     onSelect: function() { 
    //         var dateObject = $(this).datepicker('getDate');
    //         console.log(dateObject); 
    //     }
    // });