/*var myModule = angular.module("myModule", []);
var myController = function ($scope, $http) {
    $scope.myData = "myData";


    $scope.addView = function () {
        $http({
            method: "POST",
            url: "/home/View2",
        })
        .then(function (response) {
            console.log(response);
            $("#mainDiv").html(response.data);
        });
    }

    $scope.addView3 = function () {
        $http({
            method: "POST",
            url: "/home/View3",
        })
        .then(function (response) {
            console.log(response);
            $("#mainDiv").html(response.data);
        });
    }
}

myModule.controller("myController", myController);

angular.module("CombineModule", ["myModule", "myModule1"]);*/

$(document).ready(function () {
    console.log("document loaded");
});

var addView = function () {
    /*$.ajax({
        type: "POST",
        url: "/home/View2",
        success: function(data, textStatus) {
            $("#mainDiv").html(data);
        },
        error: function() {
            alert('Not OKay');
        }
    })*/
    $('#mainDiv').load("home/View2")
    //$('#mainDiv').load('@Url.Action("view2","/Home")');
}

var addView3 = function () {
    $.ajax({
        type: "POST",
        url: "/home/View3",
        success: function(data, textStatus) {
            $("#mainDiv").html(data);    
        },
        error: function() {
            alert('Not OKay');
        }
    })
}
