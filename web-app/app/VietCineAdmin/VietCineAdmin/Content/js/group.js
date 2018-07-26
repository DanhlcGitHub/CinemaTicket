﻿$(document).ready(function () {
    validationManager.groupValidation();
});

var myApp = angular.module("homeModule", []);
var groupController = function ($scope, $http) {
    $scope.ListGroupCinema;
    $scope.GroupCinemaNowSelected;

    $http({
        method: "GET",
        url: "/Home/GetAllGroupCinema"
    }).then(function (response) {
        console.log(response.data);
        $scope.ListGroupCinema = response.data;
    });

    $scope.clickDetail = function (index) {
        $scope.GroupCinemaNowSelected = $scope.ListGroupCinema[index];
        $("#groupId").val($scope.GroupCinemaNowSelected.GroupId);
        $("#groupName").val($scope.GroupCinemaNowSelected.name);
        $("#address").val($scope.GroupCinemaNowSelected.address);
        $("#phone").val($scope.GroupCinemaNowSelected.phone);
        $("#email").val($scope.GroupCinemaNowSelected.email);
        $("#logoImg").val($scope.GroupCinemaNowSelected.logoImg);

    };

    $scope.createGroupCinema = function () {
        $("#form-group").submit(function (e) {
            e.preventDefault(e);
            var valid = $("#form-group").valid();
            if (valid == true) {
                var groupId = $("#groupId").val();
                var groupName = $("#groupName").val();
                var address = $("#address").val();
                var phone = $("#phone").val();
                var email = $("#email").val();
                var logoImg = $("#logoImg").val();
                var priceDefault = $("#priceDefault").val();
                

                $http({
                    method: "POST",
                    url: "/Home/CreateGroupCinema",
                    params: {
                        groupId: groupId,
                        groupName: groupName,
                        address: address,
                        phone: phone,
                        email: email,
                        logoImg: logoImg,
                        priceDefault: priceDefault
                    }
                }).then(function (response) {

                    $scope.clearForm();

                    $scope.ListGroupCinema = response.data;

                    alert("Success!");

                });
            }
        });
        
    };

    $scope.clearForm = function () {
        document.getElementById("form-group").reset()
    };

};

myApp.controller("groupController", groupController);

$.validator.addMethod('customphone', function (value, element) {
    return this.optional(element) || /^\d{9,11}$/.test(value);
}, "Please enter a valid phone number");

var validationManager = {
    groupValidation: function () {
        $("#form-group").validate({
            rules: {
                groupName: {
                    required: true
                },
                address: {
                    required: true
                },
                phone: 'customphone',
                email: {
                    required: true,
                    email: true
                },
                logoImg: {
                    required: true
                },
                defaultPrice: {
                    required: true,

                }

            },
            messages: {
                groupName: {
                    required: 'Please enter group name!',
                },
                address: {
                    required: 'Please enter address!',
                    min: 'Film length must be greater than 0!'
                },
                phone: {
                    required: 'Please enter phone number!'
                },
                email: {
                    required: 'Please enter email!',
                    email: 'Email address is not correct!'
                },
                logoImg: {
                    required: 'Please enter logo imgage!',
                },
            }
        });
    }
}

