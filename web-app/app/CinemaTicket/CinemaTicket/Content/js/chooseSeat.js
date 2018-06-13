var myApp = angular.module("seatModule", []);
var seatController = function ($scope, $http) {
    $scope.matrix = []; // 
    $scope.matrixX = 0;
    $scope.matrixY = 0;
    $scope.alpha = ["A", "B", "C", "D", "E", "F", "J", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U"];

    $http({
        method: "POST",
        url: "/Seat/FindAllSeatByScheduleId",
        params: { scheduleId: $("#scheduleId").val() }
    })
    .then(function (response) {
        $scope.data = response.data.seats;
        console.log("data receive");
        console.log(response);
        $scope.matrixX = response.data.matrixX;
        $scope.matrixY = response.data.matrixY;
        $scope.displayData($scope.matrixX, $scope.matrixY);
       
    });

    /* var data = [
     { seatId: 1, px: 0, py: 0 }, { seatId: 1, px: 5, py: 0 }, { seatId: 1, px: 0, py: 2 }, { seatId: 1, px: 5, py: 2 }
     ];*/
    $scope.displayData = function (matrixX, matrixY) {
        for (var i = 0 ; i < matrixY ; i++) {
            var rowObj = {
                py: i + 1,
                alphaName: $scope.alpha[i],
                seats: {},
            };
            $scope.matrix[i] = rowObj;
        }
        //init null seat
        for (var i = 0 ; i < matrixY ; i++) {
            for (var j = 0 ; j < matrixX ; j++) {
                var seatObj = {
                    isNull: true,
                    className: 'btn-seat-hidden',
                    seatId: -1,
                    px: -1,
                    py: -1
                }
                $scope.matrix[i].seats[j] = seatObj;
            }
        }
        console.log($scope.matrix);
        for (var i = 0; i < $scope.data.length; i++) {
            console.log("px: " + $scope.data[i].px);
            console.log("py: " + $scope.data[i].py);
            var seatObj = {
                isNull: false,
                className: 'btn-seat',
                seatId: $scope.data[i].seatId,
                px: $scope.data[i].px,
                py: $scope.data[i].py
            }

            $scope.matrix[seatObj.py].seats[seatObj.px] = seatObj;
        }
    };


}

myApp.controller("seatController", seatController);
