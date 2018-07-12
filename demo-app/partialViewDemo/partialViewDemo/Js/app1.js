var myModule1 = angular.module("myModule1", []);
var myController1 = function ($scope, $http) {
    $scope.myData1 = "myData1";
    
    
    $scope.$on('someEvent', function(event, args) {
        console.log("hello from begilium");
        console.log("data: " + args);
    });
}

myModule1.controller("myController1", myController1);