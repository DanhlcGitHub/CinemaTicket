var myApp = angular.module("homeModule", []);
var partnerController = function ($scope, $http) {
    $scope.ListPartnerAccount;
    $scope.ListGroupCinema;

    $http({
        method: "GET",
        url: "/Home/GetAllPartner"
    }).then(function (response) {
        $scope.ListPartnerAccount = response.data;
        });

    $http({
        method: "GET",
        url: "/Home/GetAllGroupCinema"
    }).then(function (response) {
        $scope.ListGroupCinema = response.data;
    });

    $scope.clickDetail = function (index) {

        $scope.PartnerNowSelected = $scope.ListPartnerAccount[index];
        $("#partnerIdUpdate").val($scope.PartnerNowSelected.partnerId);
        $("#partnerPasswordUpdate").val($scope.PartnerNowSelected.partnerPassword);
        $("#groupCinemaIdUpdate").val($scope.PartnerNowSelected.groupOfCinemaId); 
        $("#partnerName").val($scope.PartnerNowSelected.partnerName);
        $("#phoneUpdate").val($scope.PartnerNowSelected.phone);
        $("#emailUpdate").val($scope.PartnerNowSelected.email);
        
    };

    $scope.clickDelete = function (index) {
        $scope.PartnerNowSelected = $scope.ListPartnerAccount[index];
        var partnerId = $scope.PartnerNowSelected.partnerId;

        if (confirm("Are you sure to delete this account!")) {
            $http({
                method: "POST",
                url: "/Home/DeletePartner",
                params: {
                    partnerId: partnerId
                }
            }).then(function (response) {
                $scope.ListPartnerAccount = response.data;
            });
        }
    };

    $scope.createPartner = function () {
        var partnerId = $("#partnerId").val();
        var partnerPassword = $("#partnerPassword").val();
        var groupCinemaId = $("#groupCinemaId option:selected").val();
        var partnerName = $("#partnerName").val();
        var phone = $("#phone").val();
        var email = $("#email").val();

        $http({
            method: "POST",
            url: "/Home/CreatePartnerAccount",
            params: {
                    partnerId: partnerId,
                    partnerPassword: partnerPassword,
                groupCinemaId: groupCinemaId,
                partnerName: partnerName,
                    phone: phone,
                    email: email,
            }
        }).then(function (response) {
            $scope.ListPartnerAccount = response.data;

            $scope.clearForm();
            $('#modal-account').modal('hide');

            alert("Successful!");
        });
    };

    $scope.updatePartner = function () {
        var partnerId = $("#partnerIdUpdate").val();
        var partnerPassword = $("#partnerPasswordUpdate").val();
        var partnerName = $("#partnerNameUpdate").val();
        var phone = $("#phoneUpdate").val();
        var email = $("#emailUpdate").val();

        $http({
            method: "POST",
            url: "/Home/UpdatePartnerAccount",
            params: {
                partnerId: partnerId,
                partnerPassword: partnerPassword,
                partnerName: partnerName,
                phone: phone,
                email: email,
            }
        }).then(function (response) {
            $scope.ListPartnerAccount = response.data;

            $scope.clearFormUpdate();
            $('#modal-account-update').modal('hide');

            alert("Successful!");
        });

    };

    $scope.clearForm = function () {
        $("#form-account")[0].reset();
    };

    $scope.clearFormUpdate = function () {
        $("#form-account-update")[0].reset();
    };
}

myApp.controller("partnerController", partnerController);