angular.module('manager')
    .controller('ChartsController', [
        '$scope', '$http', '$modal', 'settingsService', 'Str', function ($scope, $http, $modal, $db, str) {

            $scope.range = "3";
            $scope.chartsData = [];
            var api = $db.getChartsApi();

            $scope.changeRange = function (v) {
                api.getTableData(v).success(function (data) {
                    $scope.chartsData = data.chart;
                    initGrid(data.titles, data.grid);

                    var options = {
                        chart: {
                            renderTo: 'charts',
                            type: 'spline'
                        },
                        title: {
                            text: str.msg_MainStatisticCharts
                        },
                        subtitle: {
                            text: str.msg_ToThinkAbout
                        },
                        xAxis: { type: 'datetime' },
                        yAxis: {
                            title: {
                                text: str.msg_SpentMoneyPerMonth
                            },
                            min: 0,
                            minorGridLineWidth: 0,
                            gridLineWidth: 0,
                            alternateGridColor: null,
                        },
                        tooltip: {
                            valueSuffix: ' UAH'
                        },
                        plotOptions: {
                            spline: {
                                lineWidth: 4,
                                states: {
                                    hover: {
                                        lineWidth: 5
                                    }
                                },
                                marker: {
                                    enabled: false
                                },
                                pointInterval: 3600000, // one hour
                                pointStart: Date.UTC(2009, 9, 6, 0, 0, 0)
                            }
                        },
                        navigation: {
                            menuItemStyle: {
                                fontSize: '10px'
                            }
                        },
                        series: data.chart
                    };
                    var chart = new Highcharts.Chart(options);
                });
            }

            function initGrid(columns, data) {
                $scope.chartsGridData = data;
                $scope.chartsGrid = {
                    data: 'chartsGridData',
                    multiSelect: false,
                    selectedItems: $scope.selectedcharts,
                    enableColumnResize: true,
                    enableCellSelection: true,
                    enableRowSelection: true,
                    enableCellEdit: false,
                    enableRowEdit: false,
                    columnDefs: columns
                };

                $scope.chartsGridEnabled = true;
            }

            $scope.changeRange($scope.range);
        }
    ]);