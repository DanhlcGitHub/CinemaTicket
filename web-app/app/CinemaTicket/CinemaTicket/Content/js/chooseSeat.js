var myApp = angular.module("seatModule", []);

var seatController = function ($scope, $http) {
    $scope.matrixData = []; // 
    $scope.alpha = ["A", "B", "C", "D", "E", "F", "J", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U"]

    angular.element(document).ready(function () {
        var data = [
		{ seatId: 1, px: 0, py: 0 }, { seatId: 1, px: 5, py: 0 }, { seatId: 1, px: 0, py: 2 }, { seatId: 1, px: 5, py: 2 }
        ];
        var matrixY = 3;
        var matrixX = 6;
        for (var i = 0 ; i < matrixY ; i++) {
            var rowObj = {
                py: i + 1,
                alphaName: $scope.alpha[i],
                seats: {},
            };
            $scope.matrixData[i] = rowObj;
            console.log("$scope.matrixData[" + i + "]: " + $scope.matrixData[i]);
        }
        //init null seat
        for (var i = 0 ; i < matrixY ; i++) {
            for (var j = 0 ; j < matrixX ; j++) {
                var seatObj = {
                    isNull: true,
                    seatId: -1,
                    px: -1,
                    py: -1
                }
                $scope.matrixData[i].seats[j] = seatObj;
            }
        }
        console.log($scope.matrixData);
        for (var i = 0; i < data.length; i++) {
            console.log("px: " + data[i].px);
            console.log("py: " + data[i].py);
            var seatObj = {
                isNull: false,
                seatId: data[i].seatId,
                px: data[i].px,
                py: data[i].py
            }

            $scope.matrixData[seatObj.py].seats[seatObj.px] = seatObj;
        }
    });
}

myApp.controller("seatController", seatController);
