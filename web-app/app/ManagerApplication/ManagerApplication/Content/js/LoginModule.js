﻿$(document).ready(function () {
    validationManager.loginValidation();
});

var loginModule = angular.module("loginModule", []);
var loginController = function ($scope, $http) {
    $scope.roleList = [{ key: "partner", value: "partner" }, { key: "cinemaManager", value: "cinemaManager" }];

    $("#loginForm").submit(function (e) {
        e.preventDefault(e);
        var valid = $("#loginForm").valid();
        if (valid == true) {
            $scope.checkLogin();
        }
    });

    $scope.checkLogin = function () {
        var username = $("#txtUsername").val();
        var password = $("#txtPassword").val();
        var role = $('#inputRole').val();

        $http({
            method: "POST",
            url: "/Utility/CheckLogin",
            params: { username: username, password: password,role : role }
        })
        .then(function (response) {
            var data = response.data;
            if (data == "") {
                console.log('hể');
                $('#validateModal').modal();
                $("#modalMessage").html("Sai tên đăng nhập hoặc mật khẩu!");
            } else if (data.valid == "true") {
                //refresh page
                location.reload();
            }
        });
    }

    loginModule.controller("loginController", loginController);
}

var validationManager = {
    loginValidation: function () {
        $("#loginForm").validate({
            rules: {
                txtUsername: {
                    required: true,
                    minlength: 5
                },
                txtPassword: {
                    required: true,
                    minlength: 5
                },
                inputRole: {
                    required: true,
                }
            },
            messages: {
                txtUsername: {
                    required: 'Please enter username',
                    minlength: 'please enter at least 5 character!'
                },
                txtPassword: {
                    required: 'Please enter password',
                    minlength: 'please enter at least 5 character!'
                },
            }
        });
    }
}
