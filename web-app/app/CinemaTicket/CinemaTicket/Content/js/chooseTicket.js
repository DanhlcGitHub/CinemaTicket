/// <reference path="bootstrap.min.js" />
var myApp = angular.module("chooseTicketModule", []);
var chooseTicketController = function ($scope, $http) {
    $scope.scheduleId;
    $scope.totalAmount = 0;

    angular.element(document).ready(function () {
        console.log("init");
        $scope.scheduleId = $("#chooseTicketId").val();
        // Load Film base on scheduleId
        $http({
            method: "POST",
            url: "/ChooseTicket/GetChooseTicketData",
            params: { scheduleId: $scope.scheduleId }
        })
        .then(function (response) {
            $scope.data = response.data;
            console.log($scope.data);
            document.getElementById("posterPicture").style.backgroundImage = "url('" + $scope.data.img + "')";
            $scope.calculateTotalAmout();
        });
    });

    $scope.clickPlusButton = function (index) {
        var sum = $scope.calculateSum();
        if (sum >= 10) {
            alert("Bạn Chỉ Được Mua 10 Vé!");
        } else {
            $scope.data.typeOfSeats[index].userChoose++;
            $scope.calculateTotalAmout();
        }
        document.getElementById("chooseSeatButtonId").disabled = false;
        console.log("index " + index + ";plus: " + $scope.data.typeOfSeats[index].userChoose);
    };

    $scope.clickMinusButton = function (index) {
        if ($scope.data.typeOfSeats[index].userChoose > 0) {
            $scope.data.typeOfSeats[index].userChoose--;
            document.getElementById("chooseSeatButtonId").disabled = false;
            var sum = $scope.calculateSum();
            if (sum <= 0) {
                console.log("go here");
                document.getElementById("chooseSeatButtonId").disabled = true;
            }
            $scope.calculateTotalAmout();
        }
    };
    $scope.calculateSum = function () {
        var sum = 0;
        for (var i = 0; i < $scope.data.typeOfSeats.length; i++) {
            sum += $scope.data.typeOfSeats[i].userChoose;
        }
        console.log(sum);
        return sum;
    };
    $scope.calculateTotalAmout = function () {
        $scope.totalAmount = 0;
        for (var i = 0; i < $scope.data.typeOfSeats.length; i++) {
            $scope.totalAmount += (parseInt($scope.data.typeOfSeats[i].userChoose)
                * parseInt($scope.data.typeOfSeats[i].price));
        }
    };
    $scope.gotoChooseSeat = function () {
        var param1 = "<input type='hidden' name='scheduleId' value='" + $scope.scheduleId + "' />";
        document.getElementById('goToChooseSeatForm').innerHTML = param1;
        document.getElementById('goToChooseSeatForm').submit();
    };
};

myApp.controller("chooseTicketController", chooseTicketController);

