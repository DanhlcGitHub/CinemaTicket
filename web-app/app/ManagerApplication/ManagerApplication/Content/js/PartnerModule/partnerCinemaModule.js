
var partnerCinemaModule = angular.module("partnerCinemaModule", []);
var partnerCinemaController = function ($scope, $http) {
    $scope.allCinemaData;
    $scope.currentCinemaList;
    $scope.currentPageCinemaData;
    $scope.cinemaMaxItem = 5;
    $scope.cinemaPageIndex = 0;
    $scope.currentCinema;
    $scope.isAddnewCinema = true;
    $scope.currentSelectedCinemaIndex = 0;
    $scope.cinemaTotalPage = 0;
    $scope.isAddRoom = false;

    $("#cinemaForm").submit(function (e) {
        e.preventDefault(e);
        if ($scope.isAddnewCinema == true) {
            $scope.addNewCinema();
        } else {//update
            $scope.updateCinema();
        }
    });
    /*====================== receive event area ====================*/
    $scope.$on('reloadCinemaEvent', function (event) {
        $scope.getAllCinema();
        console.log("reload");
    });
    /**************************************************Cinema Function*************************************************/
    $scope.getAllCinema = function () {
        $http({
            method: "POST",
            url: "/Partner/GetAllCinemaInGroup",
            params: { groupIdStr: $("#groupId").val(), }
        })
        .then(function (response) {
            $scope.cinemaPageIndex = 0;
            $scope.isAddnewCinema = true;
            $scope.currentCinema = {};
            $scope.allCinemaData = response.data;
            $scope.currentCinemaList = response.data;
            $scope.currentPageCinemaData = $scope.currentCinemaList.slice($scope.cinemaPageIndex * $scope.cinemaMaxItem, (($scope.cinemaPageIndex + 1) * $scope.cinemaMaxItem));
            $scope.cinemaTotalPage = Math.ceil($scope.currentCinemaList.length / $scope.cinemaMaxItem);
            $scope.cinemaPagingArray = new Array($scope.cinemaTotalPage);
            $scope.updateCinemaTrackingArray();
        });
    };
    $scope.cinemaPreviousClick = function () {
        $scope.cinemaPageIndex--;
        if ($scope.cinemaPageIndex < 0) $scope.cinemaPageIndex = Math.ceil($scope.currentCinemaList.length / $scope.cinemaMaxItem) - 1;
        $scope.currentPageCinemaData = $scope.currentCinemaList.slice($scope.cinemaPageIndex * $scope.cinemaMaxItem, (($scope.cinemaPageIndex + 1) * $scope.cinemaMaxItem));
        $scope.updateCinemaTrackingArray();
    };

    $scope.cinemaNextClick = function () {
        $scope.cinemaPageIndex++;
        if ($scope.cinemaPageIndex >= (Math.ceil($scope.currentCinemaList.length / $scope.cinemaMaxItem))) $scope.cinemaPageIndex = 0;
        $scope.currentPageCinemaData = $scope.currentCinemaList.slice($scope.cinemaPageIndex * $scope.cinemaMaxItem, (($scope.cinemaPageIndex + 1) * $scope.cinemaMaxItem));
        $scope.updateCinemaTrackingArray();
    };

    $scope.cinemaPageClick = function (index) {
        $scope.cinemaPageIndex = index;
        $scope.currentPageCinemaData = $scope.currentCinemaList.slice($scope.cinemaPageIndex * $scope.cinemaMaxItem, (($scope.cinemaPageIndex + 1) * $scope.cinemaMaxItem));
        $scope.updateCinemaTrackingArray();
    };

    $("#loaderPicture").hide();
    $scope.uploadPicture = function () {
        var formData = new FormData();
        var files = $("#cinemaChooseFile").get(0).files;
        
        // Add the uploaded image content to the form data collection
        if (files.length > 0) {
            formData.append("imageUpload", files[0]);
            var fileName = files[0].name;
            $("#uploadImageBtn").hide();
            $("#loaderPicture").show();
            
            jQuery.ajax({
                url: '/Partner/SaveImage',
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

    $scope.getAllCinema();

    $scope.cinemaDetail = function (index) {
        $scope.currentSelectedCinemaIndex = index;
        var validator = $("#cinemaForm").validate();
        validator.resetForm();
        $scope.isAddnewCinema = false;
        $scope.currentCinema = $scope.currentPageCinemaData[index];

    };

    $scope.showAddCinemaForm = function (index) {
        var validator = $("#cinemaForm").validate();
        validator.resetForm();
        $scope.isAddnewCinema = true;
        $scope.currentCinema = {};
        //set default picture
        $scope.currentCinema.profilePicture = "https://www.shofu.de/wp-content/themes/aaika/assets/images/default.jpg";
    };

    $scope.readFile = function () {
        var input = $("#cinemaChooseFile");
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#cinemaProfilePicture')
                    .attr('src', e.target.result)
                    .width(150)
                    .height(150);
            };
            reader.readAsDataURL(input.files[0]);
        }
    };

    $scope.updateCinema = function () {
        if ($("#cinemaForm").valid()) {
            var cinemaName = $("#inputCinemaName").val();
            var cinemaAddress = $("#inputCinemaAddress").val();
            var cinemaPhone = $("#inputCinemaPhone").val();
            var cinemaEmail = $("#inputCinemaEmail").val();
            var openTime = $("#inputCinemaOpenTime").val();
            var introduction = $("#inputCinemaIntroduction").val();
            var imagePath = $('#cinemaChooseFile').val();
            var path = "";
            if (imagePath != "") {
                var arrStr = imagePath.split('\\');
                path = arrStr[arrStr.length - 1];
                //$scope.saveImage(path);
            }

            var cinemaObj = {};
            cinemaObj.id = $scope.currentCinema.cinemaId;
            cinemaObj.cinemaName = cinemaName;
            cinemaObj.cinemaAddress = cinemaAddress;
            cinemaObj.phone = cinemaPhone;
            cinemaObj.email = cinemaEmail;
            cinemaObj.openTime = openTime;
            cinemaObj.introduction = introduction;
            cinemaObj.imagePath = path;
            // show current file
            if (path != "") {
                var f = document.getElementById('cinemaChooseFile').files[0];
                r = new FileReader();
                r.onloadend = function (e) {
                    var data = e.target.result;
                    //send your binary data via $http or $resource or do anything else with it
                    //$scope.uploadImage(data);
                }
                r.readAsBinaryString(f);
            }


            //update cinema
            $http({
                method: "POST",
                url: "/Partner/UpdateCinema",
                params: { cinemaObj: cinemaObj, }
            })
             .then(function (response) {
                 //load list cinema again
                 $('#cinemaForm')[0].reset();

                 $http({
                     method: "POST",
                     url: "/Partner/GetAllCinemaInGroup",
                     params: { groupIdStr: $("#groupId").val() }
                 })
                .then(function (response) {
                    $scope.allCinemaData = response.data;

                    $scope.currentCinemaList = response.data;
                    $scope.currentPageCinemaData = $scope.currentCinemaList.slice($scope.cinemaPageIndex * $scope.cinemaMaxItem, (($scope.cinemaPageIndex + 1) * $scope.cinemaMaxItem));
                    $scope.cinemaTotalPage = Math.ceil($scope.currentCinemaList.length / $scope.cinemaMaxItem);
                    $scope.cinemaPagingArray = new Array($scope.cinemaTotalPage);
                    $scope.updateCinemaTrackingArray();
                    $scope.currentCinema = $scope.currentPageCinemaData[$scope.currentSelectedCinemaIndex];
                });
                 $("#validateModal").modal();
                 $("#modalMessage").html("Update success!");
             });
        }
    };

    $scope.addNewCinema = function () {
        if ($("#cinemaForm").valid()) {
            var cinemaName = $("#inputCinemaName").val();
            var cinemaAddress = $("#inputCinemaAddress").val();
            var cinemaPhone = $("#inputCinemaPhone").val();
            var cinemaEmail = $("#inputCinemaEmail").val();
            var openTime = $("#inputCinemaOpenTime").val();
            var introduction = $("#inputCinemaIntroduction").val();
            var imagePath = $('#cinemaChooseFile').val();
            var path = "";
            if (imagePath != "") {
                var arrStr = imagePath.split('\\');
                path = arrStr[arrStr.length - 1];
            }

            var cinemaObj = {};
            cinemaObj.id = $scope.currentCinema.cinemaId;
            cinemaObj.cinemaName = cinemaName;
            cinemaObj.cinemaAddress = cinemaAddress;
            cinemaObj.phone = cinemaPhone;
            cinemaObj.email = cinemaEmail;
            cinemaObj.openTime = openTime;
            cinemaObj.introduction = introduction;
            cinemaObj.imagePath = path;
            cinemaObj.groupId = $("#groupId").val();
            // upload file
            if (path != "") {
                var f = document.getElementById('cinemaChooseFile').files[0];
                r = new FileReader();
                r.onloadend = function (e) {
                    var data = e.target.result;
                    //send your binary data via $http or $resource or do anything else with it
                    //$scope.uploadImage(data);
                }
                r.readAsBinaryString(f);
            }
            //update cinema
            $http({
                method: "POST",
                url: "/Partner/CreateCinema",
                params: { cinemaObj: cinemaObj, }
            })
             .then(function (response) {
                 //load list cinema again
                 $('#cinemaForm')[0].reset();
                 $("#cinemaProfilePicture").attr("src", "https://www.shofu.de/wp-content/themes/aaika/assets/images/default.jpg");
                 $scope.getAllCinema();
                 $("#validateModal").modal();
                 $("#modalMessage").html("Create success!");
             });
        }
    }

    $scope.uploadImage = function (data) {

        var fd = new FormData();
        fd.append('file', data);
        $http.post("/Partner/UploadImage", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
        .success(function () {
        })
        .error(function () {
        });
    };

    $scope.updateCinemaTrackingArray = function () {
        for (var i = 0 ; i < $scope.cinemaPagingArray.length; i++) {
            $scope.cinemaPagingArray[i] = "page-item";
        }
        $scope.cinemaPagingArray[$scope.cinemaPageIndex] = "page-item active";
    };

    $scope.viewRoom = function (index) {
        var infor = {
            currentCinema: $scope.currentCinema,
            currentRoom: $scope.currentCinema.rooms[index],
        }
        $scope.$parent.$broadcast('viewRoomEvent', infor);
    };

    $scope.showAddRoom = function () {
        $scope.isAddRoom = true;
        var infor = {
            currentCinema: $scope.currentCinema,
        }
        $scope.$parent.$broadcast('addRoomEvent', infor);
        $("#seatAreaId").hide();
        $('#viewSeatModal').modal();
    };
}

partnerCinemaModule.controller("partnerCinemaController", partnerCinemaController);





