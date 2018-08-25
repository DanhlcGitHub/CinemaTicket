$(document).ready(function () {

});

var dashboardModule = angular.module("dashboardModule", []);
var dashboardController = function ($scope, $http) {
    $scope.dashboardCommonData;
    $scope.weeklyTicketData;
    $scope.filmRankingData;
    $scope.totalProfit = 0;

    
    $scope.$on('viewDashBoardEvent', function (event) {
        console.log('here');
        $scope.getDashboardCommonData();
        $scope.GetWeeklyTicketData();
        $scope.GetTop4Film();
    });

    $scope.getDashboardCommonData = function () {
        $http({
            method: "POST",
            url: "/CinemaManager/GetDashboardCommonData",
            params: { cinemaIdStr: $("#cinemaId").val() }
        })
       .then(function (response) {
           console.log("commondata");
           console.log(response);
           $scope.dashboardCommonData = response.data;
       });
    };

    $scope.GetWeeklyTicketData = function () {
        $http({
            method: "POST",
            url: "/CinemaManager/GetWeeklyTicketData",
            params: { cinemaIdStr: $("#cinemaId").val() }
        })
       .then(function (response) {
           console.log("ticketlist");
           console.log(response);
           $scope.weeklyTicketData = response.data;
           $scope.totalProfit = response.data.totalSold * response.data.price;
           $scope.drawSaleChart();
       });
    };

    $scope.GetTop4Film = function () {
        $http({
            method: "POST",
            url: "/CinemaManager/GetTop4Film",
            params: { cinemaIdStr: $("#cinemaId").val() }
        })
       .then(function (response) {
           console.log("top 4 film");
           console.log(response);
           $scope.filmRankingData = response.data;
       });
    };
    $scope.GetTop4Film();
    $scope.getDashboardCommonData();
    $scope.GetWeeklyTicketData();

    /*--------------------------Canvas part -------------------------------*/
    var ticksStyle = {
        fontColor: '#495057',
        fontStyle: 'bold'
    }

    var mode = 'index'
    var intersect = true

    $scope.drawSaleChart = function () {
        var $salesChart = $('#sales-chart')
        var salesChart = new Chart($salesChart, {
            type: 'bar',
            data: {
                labels: ['Sun','Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
                datasets: [
                  {
                      backgroundColor: '#007bff',
                      borderColor: '#007bff',
                      data: $scope.weeklyTicketData.weeklyTicketList,
                  },

                ]
            },
            options: {
                maintainAspectRatio: false,
                tooltips: {
                    mode: mode,
                    intersect: intersect
                },
                hover: {
                    mode: mode,
                    intersect: intersect
                },
                legend: {
                    display: false
                },
                scales: {
                    yAxes: [{
                        // display: false,
                        gridLines: {
                            display: true,
                            lineWidth: '2px',
                            color: 'rgba(0, 0, 0, .2)',
                            zeroLineColor: 'transparent'
                        },
                        ticks: $.extend({
                            beginAtZero: true,

                            // Include a dollar sign in the ticks
                            callback: function (value, index, values) {
                                if (value >= 1000) {
                                    value /= 1000
                                    value += 'k'
                                }
                                return '' + value
                            }
                        }, ticksStyle)
                    }],
                    xAxes: [{
                        display: true,
                        gridLines: {
                            display: false
                        },
                        ticks: ticksStyle
                    }]
                }
            }
        })
    }
}

dashboardModule.controller("dashboardController", dashboardController);

