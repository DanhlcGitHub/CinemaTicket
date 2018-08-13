$(document).ready(function () {
    validationManager.groupValidation();
});

var myApp = angular.module("homeModule", []);
var groupController = function ($scope, $http) {
    $scope.ListGroupCinema;
    $scope.GroupCinemaNowSelected;

    $("#logoImg").change(function () {
        //$("#logoImgText").val();
        console.log("here");
    });

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
        $("#logoImgText").val($scope.GroupCinemaNowSelected.logoImg);
        $("#divDefaultPrice").hide();
    };


    $("#form-group").submit(function (e) {
        $scope.createGroupCinema();
    });

    $scope.createGroupCinema = function () {
        var valid = $("#form-group").valid();
        if (valid == true) {
            var groupId = $("#groupId").val();
            var groupName = $("#groupName").val();
            var address = $("#address").val();
            var phone = $("#phone").val();
            var email = $("#email").val();
            var logoImg = $("#logoImg").val();
            var priceDefault = $("#priceDefault").val();
            var imgPath = "";

            if (logoImg != "") {
                var arrStr = logoImg.split('\\');
                imgPath = arrStr[arrStr.length - 1];
                //$scope.saveImage(path);
            }
            $http({
                method: "POST",
                url: "/Home/CreateGroupCinema",
                params: {
                    groupId: groupId,
                    groupName: groupName,
                    address: address,
                    phone: phone,
                    email: email,
                    logoImg: imgPath,
                    priceDefault: priceDefault
                }
            }).then(function (response) {

                $scope.clearForm();

                $('#modal-group').modal('hide');

                $scope.ListGroupCinema = response.data;

                alert("Success!");
            });
        }

    };

    $scope.clearForm = function () {
        document.getElementById("form-group").reset();
        $("#divDefaultPrice").show();
    };
    $("#loaderPicture").hide();
    $scope.uploadPicture = function () {
        var formData = new FormData();
        var files = $("#logoImg").get(0).files;

        // Add the uploaded image content to the form data collection
        if (files.length > 0) {
            formData.append("imageUpload", files[0]);
            var fileName = files[0].name;
            $("#uploadImageBtn").hide();
            $("#loaderPicture").show();

            jQuery.ajax({
                url: '/Home/SaveImage',
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
                success: function (data) {
                    $("#uploadImageBtn").show();
                    $("#loaderPicture").hide();
                    $("#validateModal").modal();
                    $("#modalMessage").html("Upload image " + data.message);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $("#uploadImageBtn").show();
                    $("#loaderPicture").hide();
                    $("#validateModal").modal();
                    $("#modalMessage").html("Some error occur, can't upload image!");
                }
            });
        } else {
            $("#validateModal").modal();
            $("#modalMessage").html("No file selected");
        }
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

