$(document).ready(function () {

});

var managerScheduleModule = angular.module("managerScheduleModule", []);
var scheduleController = function ($scope, $http) {
    $scope.currentSelectedFilmId = null;
    $scope.currentSelectedFilmName = null;
    $scope.baseStartTime = 9;
    $scope.currentSelectedDate = null;
    $scope.currentScheduleData;
    $scope.filmData;
    $scope.today;
    $scope.cinemaId = $("#cinemaId").val();
    $("#customScheduleArea").hide();
    $("#loader").hide();
    //$("#skeduler-container").hide();

    var StatusConstant = { available: "available", choosing: "choosing", added: "added", suggested: "suggested" };

    $scope.$on('basicAddScheduleEvent', function (event) {
        console.log('here');
    });

    $scope.$on('advantageAddScheduleEvent', function (event) {
        console.log('here');
    });

    $scope.basicAddScheduleSave = function () {
        if ($("#basicAddForm").valid()) {
            if (new Date($("#basicDateSelector").val()) < new Date($scope.today)) {//compare end <=, not >=
                $("#validateModal").modal();
                $("#modalMessage").html("These day is no longer available for add chedule");
            } else {
                var filmIdStr = $("#basicFilmSelector").val();
                var timeIdStr = $("#basicTimeSelector").val();
                var roomIdStr = $("#basicRoomSelector").val();
                var scheduleDateStr = $("#basicDateSelector").val();
                $http({
                    method: "POST",
                    url: "/CinemaManager/basicAddSchedule",
                    params: {
                        filmIdStr: filmIdStr, timeIdStr: timeIdStr,
                        roomIdStr: roomIdStr, scheduleDateStr: scheduleDateStr
                    }
                })
                .then(function (response) {
                    var message = response.data.message;
                    $("#validateModal").modal();
                    $("#modalMessage").html(message);
                });
            }
        }
    };

    $('#customScheduleFilmSelector').on('change', function (e) {
        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;
        $scope.currentSelectedFilmId = valueSelected;
        var contentIdentity = "#customScheduleFilmSelector option[value='" + valueSelected + "']";
        $scope.currentSelectedFilmName = $(contentIdentity).text()
    });

    $('#customScheduleDateSelector').on('change', function (e) {
        var selectDate = this.value;
        $scope.currentSelectedDate = selectDate;
        $("#loader").show();
        $("#skeduler-container").hide();
        $scope.GetSchedule();
        $("#customScheduleArea").show();
    });

    $http({
        method: "POST",
        url: "/CinemaManager/LoadAvailableFilm",
    })
    .then(function (response) {
        $scope.filmData = response.data;
    });

    $http({
        method: "POST",
        url: "/CinemaManager/LoadAllRoomByCinemaId",
        params: { cinemaIdStr: $scope.cinemaId || $("#cinemaId").val() }
    })
    .then(function (response) {
        $scope.roomData = response.data;
    });


    $http({
        method: "POST",
        url: "/CinemaManager/LoadAllShowTime",
    })
   .then(function (response) {
       $scope.timeData = response.data;
   });

    $http({
        method: "POST",
        url: "/CinemaManager/GetCurrentDate",
    })
    .then(function (response) {
        $scope.today = response.data.today;
    });

    $scope.GetSchedule = function () {
        $http({
            method: "POST",
            url: "/CinemaManager/GetScheduleByDateFilm",
            params: {
                cinemaIdStr: $scope.cinemaId, selectedDateStr: $scope.currentSelectedDate,
            }
            //cinemaIdStr,string selectedDateStr, string filmIdStr
        })
       .then(function (response) {
           console.log("response goes here!");
           console.log(response);
           $scope.currentScheduleData = response.data;
           $scope.generateSchedule($scope.currentScheduleData);
           $("#loader").hide();
           $("#skeduler-container").show();
       });
    };

    $scope.getHeader = function (scheduleList) {
        var headers = [];
        for (var i = 0 ; i < scheduleList.length; i++) {
            var addedInfor = "(0 added)";
            var addedShowTime = $.grep(scheduleList[i].currentShowTime, function (o) {
                return (o.status == StatusConstant.added);
            });
            var totalAdded = addedShowTime.length;
            if (totalAdded != 0) {
                addedInfor = "(" + totalAdded + " added)";
            }
            var aHeader = scheduleList[i].roomName + " " + addedInfor;
            headers.push(aHeader);
        }
        return headers;
    };
    $scope.formatData = function (scheduleList) {
        var tasks = [];
        for (var i = 0 ; i < scheduleList.length; i++) {
            var timeList = scheduleList[i].currentShowTime;
            for (var j = 0; j < timeList.length; j++) {
                var aTime = timeList[j];
                var backgroundColor = $scope.getBackGroundColor(aTime.status);
                var aTask = {
                    startTime: aTime.startTimeNum - $scope.baseStartTime,
                    duration: 2,
                    column: i,
                    id: aTime.timeId,
                    filmId: aTime.filmId,
                    filmName: aTime.filmName || "",
                    title: aTime.startTime + " - " + aTime.endTime + "  " + aTime.status,
                    display: aTime.display || "block",
                    backgroundColor: backgroundColor
                }
                tasks.push(aTask);
            }
        }
        return tasks;
    }

    $scope.getBackGroundColor = function (status) {
        var backgroundColor = "#576D7C";//blue
        if (status == StatusConstant.added) {
            backgroundColor = "#dc3545";//red
        } else if (status == StatusConstant.choosing) {
            backgroundColor = "green";
        } else if (status == StatusConstant.suggested) {
            backgroundColor = "#443354"; //purple
        }
        return backgroundColor;
    }

    $scope.onTimeClick = function (e, t) {
        if (new Date($("#customScheduleDateSelector").val()) < new Date($scope.today)) {//compare end <=, not >=
            console.log("???!!");
            $("#validateModal").modal();
            $("#modalMessage").html("These day is no longer available for add chedule");
        } else {
            t.backgroundColor = "green";
            var selectedColumn = $scope.currentScheduleData[t.column].currentShowTime;
            var aTime = $scope.findTimeById(t.id, selectedColumn);
            if (aTime.status != StatusConstant.added) {
                //if available -> choosing; else if choosing -> available
                if (aTime.status == StatusConstant.suggested) {
                    aTime.status = "available";
                    aTime.filmId = "";
                    aTime.filmName = "";
                }
                else if (aTime.status == StatusConstant.available) {
                    if ($scope.currentSelectedFilmId != null) {
                        aTime.status = "choosing";
                        aTime.filmId = $scope.currentSelectedFilmId;
                        aTime.filmName = $scope.currentSelectedFilmName;
                    } else {
                        alert("Please select a film!");
                    }

                } else if (aTime.status == StatusConstant.choosing) {
                    aTime.status = "available";
                    aTime.filmId = "";
                    aTime.filmName = "";
                }
                $scope.changeVisibility(selectedColumn);
                $scope.generateSchedule($scope.currentScheduleData);
            }
        }
    }

    $scope.changeVisibility = function (timeList) {
        //all is available
        for (var i = 0 ; i < timeList.length; i++) {
            timeList[i].display = "";
        }
        //load choosing list
        var choosingList = $.grep(timeList, function (o) {
            return (o.status == StatusConstant.choosing);
        });
        // check again
        for (var k = 0 ; k < choosingList.length; k++) {
            var aTime = choosingList[k];
            for (var i = 0 ; i < timeList.length; i++) {
                var currentTime = timeList[i];
                if (aTime.startTimeNum > currentTime.startTimeNum && aTime.startTimeNum < currentTime.endTimeNum) {
                    if (currentTime.status != StatusConstant.added)
                        currentTime.display = "none";
                } else if (aTime.endTimeNum > currentTime.startTimeNum && aTime.endTimeNum < currentTime.endTimeNum) {
                    if (currentTime.status != StatusConstant.added)
                        currentTime.display = "none";
                }
            }
        }
    }

    $scope.filterSuggestList = function () {

        for (var i = 0 ; i < $scope.currentScheduleData.length; i++) {
            var timeList = $scope.currentScheduleData[i].currentShowTime;
            //load choosing list
            var choosingList = $.grep(timeList, function (o) {
                return (o.status == StatusConstant.choosing);
            });
            console.log("-------------------------------");
            console.log("before filter choosinglist");
            console.log(choosingList);
            console.log(timeList);
            // check again
            for (var k = 0 ; k < choosingList.length; k++) {
                var aTime = choosingList[k];
                for (var j = 0 ; j < timeList.length; j++) {
                    var currentTime = timeList[j];
                    if (aTime.startTimeNum > currentTime.startTimeNum && aTime.startTimeNum < currentTime.endTimeNum) {
                        if (currentTime.status == StatusConstant.suggested)
                            currentTime.status = StatusConstant.available;
                    } else if (aTime.endTimeNum > currentTime.startTimeNum && aTime.endTimeNum < currentTime.endTimeNum) {
                        if (currentTime.status == StatusConstant.suggested)
                            currentTime.status = StatusConstant.available;
                    }
                }
            }
            console.log("after filter choosinglist");
            console.log(choosingList);
            console.log(timeList);
            console.log("------------base object ------------");
            console.log($scope.currentScheduleData);
        }
    }

    $scope.findTimeById = function (id, timeList) {
        for (var i = 0 ; i < timeList.length; i++) {
            var aTime = timeList[i];
            if (aTime.timeId == id) {
                return aTime;
            }
        }
        return null;
    };

    $scope.isSaveDone = true;
    $scope.saveSchedule = function () {
        if (new Date($("#customScheduleDateSelector").val()) < new Date($scope.today)) {//compare end <=, not >=
            $("#validateModal").modal();
            $("#modalMessage").html("These day is no longer available for add chedule");
        } else {
            if ($scope.isSaveDone == true) {
                var allTimeList = [];
                $scope.filterSuggestList();
                var dataArray = [];
                for (var i = 0 ; i < $scope.currentScheduleData.length; i++) {
                    var anItem = $scope.currentScheduleData[i];
                    var roomId = anItem.roomId;
                    var timeList = anItem.currentShowTime;
                    var choosingList = $.grep(timeList, function (o) {
                        return (o.status == StatusConstant.choosing || o.status == StatusConstant.suggested);
                    });
                    var dataStr = {
                        roomId: roomId,
                        //filmId: $scope.currentSelectedFilmId,
                        selectedDate: $scope.currentSelectedDate,
                        timeList: choosingList,
                    }
                    dataArray.push(dataStr);
                }
                $scope.isSaveDone = false;
                $("#loader").show();
                $("#skeduler-container").hide();
                $scope.saveToDB(dataArray);
            }
        }
    }


    $scope.saveToDB = function (dataArray) {
        $.ajax({
            type: 'POST',
            url: "/CinemaManager/SaveCustomSchedule",
            data: { scheduleInfor: JSON.stringify({ dataArray: dataArray }) },
            success: function (response) {
                console.log(response);
                $scope.GetSchedule();
                $scope.isSaveDone = true;
                $("#loader").hide();
                $("#skeduler-container").show();
                $('#customScheduleFilmSelector').val("null");
                $scope.currentSelectedFilmId = null;
            }
        });
    }

    $scope.generateSchedule = function (scheduleList) {
        //var scheduleList = [{ roomName: "rap 1", timeList: [{ timeId: 1, startTime: 9, endTime: 11 }, { timeId: 2, startTime: 10, endTime: 12 }, { timeId: 3, startTime: 11, endTime: 13 }] }];
        $("#skeduler-container").skeduler({
            headers: $scope.getHeader(scheduleList),
            //headerContainerCssClass : "headerClass",
            tasks: $scope.formatData(scheduleList),
            cardTemplate: '<div><i class="nav-icon fa fa-calendar"></i></div><div>${filmName}</div><div>${title}</div>',
            onClick: function (e, t) { $scope.onTimeClick(e, t) }
        });
    }
}

managerScheduleModule.controller("scheduleController", scheduleController);

