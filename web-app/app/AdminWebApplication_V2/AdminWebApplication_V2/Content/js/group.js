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
        var groupId = $("#groupId").val();
        var groupName = $("#groupName").val();
        var address = $("#address").val();
        var phone = $("#phone").val();
        var email = $("#email").val();
        var logoImg = $("#logoImg").val();

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
        $("form-group")[0].reset();
    };

};

myApp.controller("groupController", groupController);