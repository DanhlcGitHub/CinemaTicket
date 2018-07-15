$(document).ready(function () {
    validationManager.loginValidation();
});

var loginModule = angular.module("loginModule", []);
var loginController = function ($scope, $http) {
    $("#loginForm").submit(function (e) {
       
    });

    $scope.checkLogin = function () {
        var username = $("#txtUsername").val();
        var password = $("#txtPassword").val();

        $http({
            method: "POST",
            url: "/home/CheckLogin",
            params: { username: username, password: password }
        })
        .then(function (response) {

         });
}

loginModule.controller("partnerController", loginController);

var validationManager = {
    loginValidation: function () {
        $("#loginForm").validate({
            rules: {
                inputEmpUsername: {
                    required: true,
                    minlength: 5
                },
                inputEmpPassword: {
                    required: true,
                    minlength: 5
                },
            },
            messages: {
                inputEmpUsername: {
                    required: 'Please enter username',
                    minlength: 'please enter at least 5 character!'
                },
                inputEmpPassword: {
                    required: 'Please enter password',
                    minlength: 'please enter at least 5 character!'
                },
            }
        });
    },



}
