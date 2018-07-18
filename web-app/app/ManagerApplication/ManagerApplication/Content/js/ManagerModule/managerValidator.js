$(document).ready(function () {
    validationManager.employeeValidation();
    validationManager.cinemaValidation();
    validationManager.addRoomValidation();
    //seat_open();
    //partner_open();

});

var validationManager = {
    customFilterValidation: function () {
        $("#customFilterForm").validate({
            rules: {
                customScheduleDateSelector: {
                    required: true,
                },
                customScheduleFilmSelector: {
                    required: true,
                },
            },
            messages: {
                customScheduleDateSelector: {
                    required: 'Please enter capacity',
                },
                customScheduleFilmSelector: {
                    required: "Please enter room name",
                },
            }
        });
    }

}
