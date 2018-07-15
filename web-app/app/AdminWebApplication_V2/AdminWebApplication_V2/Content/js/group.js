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
        document.getElementById("groupId").value = $scope.GroupCinemaNowSelected.GroupId;
        document.getElementById("groupName").value = $scope.GroupCinemaNowSelected.name;
        document.getElementById("address").value = $scope.GroupCinemaNowSelected.address;
        document.getElementById("phone").value = $scope.GroupCinemaNowSelected.phone;
        document.getElementById("email").value = $scope.GroupCinemaNowSelected.email;
        document.getElementById("logoImg").value = $scope.GroupCinemaNowSelected.logoImg;
    };

    $scope.createGroupCinema = function () {
        var groupId = document.getElementById("groupId").value;
        var groupName = document.getElementById("groupName").value;
        var address = document.getElementById("address").value;
        var phone = document.getElementById("phone").value;
        var email = document.getElementById("email").value;
        var logoImg = document.getElementById("logoImg").value;

        $http({
            method: "POST",
            url: "/Home/CreateGroupCinema",
            params: {
                groupId: groupId,
                groupName: groupName,
                address: address,
                phone: phone,
                email: email,
                logoImg: logoImg
            }
        }).then(function (response) {

            $scope.clearForm();

            $scope.ListGroupCinema = response.data;

            $('#alert').show();

        });
    };

    $scope.clearForm = function () {
        document.getElementById("groupName").value = "";
        document.getElementById("address").value = "";
        document.getElementById("phone").value = "";
        document.getElementById("email").value = "";
        document.getElementById("logoImg").value = "";
    };

};

myApp.controller("groupController", groupController);