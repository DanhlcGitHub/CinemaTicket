$(document).ready(function () {

});

var partnerDashboardModule = angular.module("partnerDashboardModule", []);
var partnerDashboardController = function ($scope, $http) {
    $scope.dashboardCommonData;
    $scope.pieChartData;
    $scope.currentCinemaData;
    $scope.price;
    $scope.Colors = ["#dc3545", "#28a745", "#ffc107", "#17a2b8", "#007bff", "#6c757d", "#fd2d69", "#f86900", "#009688", "#633a00", ];
    $scope.$on('manageDashboardEvent', function (event) {
        console.log('here');
        $scope.getDashboardCommonData();
        $scope.getPieChartData();
        $scope.getAllListCinema();
    });

    $scope.getAllListCinema = function () {
        $http({
            method: "POST",
            url: "/Partner/GetListCinema",
            params: { groupIdStr: $("#groupId").val(), }
        })
       .then(function (response) {
           $scope.pieChartData = $scope.initColorForCinema(response.data);
       });
    };

    $scope.getDashboardCommonData = function () {
        $http({
            method: "POST",
            url: "/Partner/GetDashboardCommonData",
            params: { groupIdStr: $("#groupId").val(), }
        })
       .then(function (response) {
           console.log("commondata");
           console.log(response);
           $scope.dashboardCommonData = response.data;
           $scope.price = response.data.price;
       });
    };

    $scope.getPieChartData = function () {
        $http({
            method: "POST",
            url: "/Partner/GetPieChartData",
            params: { groupIdStr: $("#groupId").val(), }
        })
       .then(function (response) {
           console.log("pie chart");
           console.log(response);
           $scope.pieChartData = $scope.formatPieCharData(response.data);
           $scope.drawPieChart();
       });
    };

    $scope.formatPieCharData = function (data) {
        var pieData = [];
        for (var i = 0 ; i < data.length; i++) {
            var obj = data[i];
            var aData = {
                value: obj.ticketSold,
                color: $scope.Colors[i],
                highlight: $scope.Colors[i],
                label: obj.cinemaName,
                revenue: obj.revenue
            };
            pieData.push(aData);
        }
        return pieData;
    };

    $scope.initColorForCinema = function (data) {
        var returnData = [];
        for (var i = 0 ; i < data.length; i++) {
            var obj = data[i];
            var aData = {
                label: obj.cinemaName,
                color: $scope.Colors[i],
            };
            returnData.push(aData);
        }
        return returnData;
    };

    $scope.getDashboardCommonData();
    $scope.getPieChartData();
    $scope.getAllListCinema();
    /*--------------------------Canvas part -------------------------------*/
    var ticksStyle = {
        fontColor: '#495057',
        fontStyle: 'bold'
    }

    var mode = 'index'
    var intersect = true

    $scope.drawPieChart = function () {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.
        var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
        var pieChart = new Chart(pieChartCanvas)
        var PieData = $scope.pieChartData;
        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: '#fff',
            //Number - The width of each segment stroke
            segmentStrokeWidth: 1,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: 'easeOutBounce',
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: false,
            //String - A legend template
            legendTemplate: '<ul class="<%=name.toLowerCase()%>-legend"><% for (var i=0; i<segments.length; i++){%><li><span style="background-color:<%=segments[i].fillColor%>"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>',
            //String - A tooltip template
            tooltipTemplate: '(<%=value %>) <%=label%>'
        }
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions)
        //-----------------
        //- END PIE CHART -
        //-----------------
    }
}

partnerDashboardModule.controller("partnerDashboardController", partnerDashboardController);

