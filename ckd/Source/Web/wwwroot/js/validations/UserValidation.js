$(document).ready(function () {
    $("#UserSignup").validate({
        rules: {
            Name: {
                required: true,
                minlength: 5,
                maxlength: 10
            },
            Username: {
                required: true,
                minlength: 5,
                maxlength: 10
            },
            Email: "required",

            PhoneNumber: {
                required: true,
                minlength: 10,
                maxlength: 10
            },
            Password: {
                required: true,
                minlength: 8,
                maxlength: 10
            },
            Department: "required",
            Business: "required",
            Country: "required",
        },
        messages: {
            Name: {
                required: "Please enter name",
                minlength: "Username must consist of at least 5 characters",
                maxlength: "Maximum characters allowed is 10"
            },
            Username: {
                required: "Please enter name",
                minlength: "Username must consist of at least 5 characters",
                maxlength: "Maximum characters allowed is 10"
            },
            Email: "Please enter a valid email address",
            PhoneNumber: {
                required: "Please enter Phone number",
                minlength: "PhoneNumber must consist of at least 10 characters",
                maxlength: "Maximum characters allowed is 10"
            },
            Password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 8 characters long",
                maxlength: "Maximum characters allowed is 12",
            },
            Department: "Please select any dropdown value",
            Business: "Please select any dropdown value",
            Country: "Please select any dropdown value"
        }
    });
});