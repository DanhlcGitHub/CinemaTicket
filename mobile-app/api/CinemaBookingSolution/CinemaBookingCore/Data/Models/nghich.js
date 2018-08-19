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


    $("#form-account").submit(function (e) {
        $scope.createPartner();
    });

    $scope.createPartner = function () {
        var valid = $("#form-account").valid();
        if (valid == true) {
            var partnerId = $("#partnerId").val();
            var partnerPassword = $("#partnerPassword").val();
            var groupCinemaId = $("#groupCinemaId option:selected").val();
            var partnerName = $("#partnerName").val();
            var phone = $("#phone").val();
            var email = $("#email").val();

            $http({
                method: "POST",
                url: "/Home/isPartnerUsernameExist",
                params: {
                    partnerId: partnerId,
                }
            }).then(function (response) {
                if (response.data.isExist == "true") {
                    alert("This username already exist!");
                    $("#partnerId").focus();
                } else {
                    $http({
                        method: "POST",
                        url: "/Home/SendMailForPartner",
                        params: {
                            partnerId: partnerId,
                            partnerPassword: partnerPassword,
                            email: email,
                        }
                    }).then(function (response) {
                        if (response.data.isSuccess == "true") {
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

                                alert("Success!");
                            });
                        } else {
                            alert("Some error occur, please check your connection!");
                        }
                    });
                }
            });
        }
    };

    $("#form-account-update").submit(function (e) {
        $scope.updatePartner();
    });

    $scope.updatePartner = function () {
        var valid = $("#form-account-update").valid();
        if (valid == true) {
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
        }
    };

    $scope.clearForm = function () {
        $("#form-account")[0].reset();
    };

    $scope.clearFormUpdate = function () {
        $("#form-account-update")[0].reset();
    };
}

myApp.controller("partnerController", partnerController);

$(document).ready(function () {
    validationManager.partnerValidation();
    validationManager.partnerUpdateValidation();
});


$.validator.addMethod('customphone', function (value, element) {
    return this.optional(element) || /^\d{9,11}$/.test(value);
}, "Please enter a valid phone number!");

var validationManager = {
    partnerValidation: function () {
        $("#form-account").validate({
            rules: {
                partnerId: {
                    required: true,
                    minlength: 6
                },
                partnerPassword: {
                    required: true,
                    minlength: 8
                },
                partnerName: {
                    required: true
                },
                phone: 'customphone',
                email: {
                    required: true,
                    email: true
                },
            },
            messages: {
                partnerId: {
                    required: 'Please enter film name!',
                    minlength: 'PartnerId must be greater than 6 characters!'
                },
                partnerPassword: {
                    required: 'Please enter partnerPassword!',
                    minlength: 'Password must be greater than 8 characters!'
                },
                partnerName: {
                    required: 'Please enter partner name!',
                },
                email: {
                    required: 'Please enter email!',
                    email: 'Email address is not correct!'
                },
            }
        });
    },
    partnerUpdateValidation: function () {
        $("#form-account-update").validate({
            rules: {
                partnerIdUpdate: {
                    required: true,
                    minlength: 6
                },
                partnerPasswordUpdate: {
                    required: true,
                    minlength: 8
                },
                partnerNameUpdate: {
                    required: true
                },
                phoneUpdate: {
                    required: true,
                    phone: true
                },
                emailUpdate: {
                    required: true,
                    email: true
                },
            },
            messages: {
                partnerIdUpdate: {
                    required: 'Please enter film name!',
                    minlength: 'PartnerId must be greater than 6 characters!'
                },
                partnerPasswordUpdate: {
                    required: 'Please enter partnerPassword!',
                    minlength: 'Password must be greater than 8 characters!'
                },
                partnerNameUpdate: {
                    required: 'Please enter partner name!',
                },
                phoneUpdate: {
                    required: 'Please enter phone number!',
                    phone: 'Phone number is not correct!'
                },
                emailUpdate: {
                    required: 'Please enter email!',
                    email: 'Email address is not correct!'
                },
            }
        });
    }
}

