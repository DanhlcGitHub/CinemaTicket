var partnerReportModule = angular.module("partnerReportModule", []);
var reportController = function ($scope, $http) {
    $scope.report = "report";
}

partnerReportModule.controller("reportController", reportController);