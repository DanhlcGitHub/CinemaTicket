$(document).ready(function () {
    validationManager.employeeValidation();
    validationManager.cinemaValidation();
    validationManager.addRoomValidation();
    //seat_open();
    //partner_open();
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
                .height(150);

        }
        reader.readAsDataURL(input.files[0]);
    }
}



$.validator.addMethod('customphone', function (value, element) {
    return this.optional(element) || /^\d{9,11}$/.test(value);
}, "Please enter 9 - 11 digit number!");

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
                inputEmpPhone: 'customphone'
                ,
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
                    min: 'min capacity is 64'
                },
                inputRoomName: {
                    required: "Please enter room name",
                    minlength: "please enter at least 4 character!"
                },
            }
        });
    }

}



function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

function partner_open() {
    document.getElementById("partner_main").style.marginLeft = "15%";
    document.getElementById("partner_sidebar").style.width = "15%";
    document.getElementById("partner_sidebar").style.display = "block";
    document.getElementById("partner_openNav").style.display = 'none';
    document.getElementById("partner_closeNav").style.display = 'inline-block';
    document.getElementById("partner_menubar").style.display = 'inline-block';
}
function partner_close() {
    document.getElementById("partner_main").style.marginLeft = "5%";
    document.getElementById("partner_rightarrowdiv").style.paddingRight = "0px";
    document.getElementById("partner_rightarrowdiv").style.display = "inline-block";
    document.getElementById("partner_sidebar").style.width = "5%";
    document.getElementById("partner_openNav").style.display = "inline-block";
    document.getElementById("partner_closeNav").style.display = 'none';
    document.getElementById("partner_menubar").style.display = 'none';
}