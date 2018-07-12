$(document).ready(function () {
    validationManager.employeeValidation();
    validationManager.cinemaValidation();
    validationManager.addRoomValidation();

    $("#cinemaChooseFile").change(function () {
        readURL(this);
    });
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#cinemaProfilePicture').attr('src', e.target.result)
                .width(150)
                .height(200);

        }
        reader.readAsDataURL(input.files[0]);
    }
}

var validationManager = {
    employeeValidation: function () {
        $("#empForm").validate({
            rules: {
                inputEmpUsername: {
                    required: true,
                    minlength: 5
                },
                inputEmpPassword: {
                    required: true,
                    minlength: 5
                },
                inputEmpConfirmPassword: {
                    required: true,
                    minlength: 5,
                    equalTo: "#inputEmpPassword"
                },
                inputEmpName: {
                    required: true,
                    minlength: 5,
                },
                inputEmpPhone: {
                    required: true,
                    minlength: 9,
                },
                inputEmpSelectCinema: {
                    required: true,
                },
                inputEmpEmail: {
                    required: true,
                    email: true
                },
            },
            messages: {
                inputEmpUsername: {
                    required: 'Please enter employee username',
                    minlength: 'please enter at least 5 character!'
                },
                inputEmpPassword: {
                    required: 'Please enter employee password',
                    minlength: 'please enter at least 5 character!'
                },
                inputEmpConfirmPassword: {
                    required: 'Please enter confirm password',
                    minlength: 'please enter at least 5 character!',
                    equalTo: 'confirm password not match'
                },
                inputEmpName: {
                    required: "Please enter employee name",
                    minlength: "please enter at least 5 character!"
                },
                inputEmpPhone: {
                    required: "Please enter phone number",
                    minlength: "phone must be 9-11 digits number"
                }
                ,
                inputEmpEmail: {
                    required: "Please provide a email",
                },
                inputEmpSelectCinema: {
                    required: "Please select an cinema",
                },
            }
        });
    },

    cinemaValidation: function () {
        $("#cinemaForm").validate({
            rules: {
                inputCinemaName: {
                    required: true,
                    minlength: 5
                },
                inputCinemaAddress: {
                    required: true,
                    minlength: 5
                },
                inputCinemaPhone: {
                    required: true,
                    minlength: 9,
                },
                inputCinemaEmail: {
                    required: true,
                    email: true
                },
            },
            messages: {
                inputCinemaName: {
                    required: 'Please enter cinema name',
                    minlength: 'please enter at least 5 character!'
                },
                inputCinemaAddress: {
                    required: 'Please enter cinema address',
                    minlength: 'please enter at least 5 character!'
                },
                inputCinemaPhone: {
                    required: "Please enter phone number",
                    minlength: "phone must be 9-11 digits number"
                },
                inputCinemaEmail: {
                    required: "Please provide a email",
                },
            }
        });
    },

    addRoomValidation: function () {
        $("#seatForm").validate({
            rules: {
                inputCapacity: {
                    required: true,
                    max: 324,
                    min: 64
                },
                inputRoomName: {
                    required: true,
                    minlength: 4
                },
            },
            messages: {
                inputCapacity: {
                    required: 'Please enter capacity',
                    max: 'max capacity is 324',
                    min : 'min capacity is 64'
                },
                inputRoomName: {
                    required: "Please enter room name",
                    minlength: "please enter at least 4 character!"
                },
            }
        });
    }

}