// validations
function validateUserID() {
    var txtUserID = document.getElementById('txt-userid');
    if (txtUserID && txtUserID.value == "") {
        return 'UserID field is required';
    }
    return "";
}

function validateFirstName() {
    if (document.getElementById('txt-fname').value == "") {
        return 'First Name field is required';
    }
    return "";
}

function validateLastName() {
    if (document.getElementById('txt-lname').value == "") {
        return 'Last Name field is required';
    }
    return "";
}

function validateEmail(emailAddress) {
    var expression = /^[\w\-\.\+]+\@[a-zA-Z0-9\.\-]+\.[a-zA-z0-9]{2,4}$/

    return expression.test(emailAddress);
}

function validateEmailAddress() {
    var emailAddress = document.getElementById('txt-email').value;

    if (emailAddress == "") {
        return 'Email field is required';
    }

    if (!validateEmail(emailAddress)) {
        return 'Email Address is invalid';
    }
    return "";
}


function validateUserExistence(action) {
    var userID = document.getElementById("txt-userid").value;
    var returnMessage = "";

    $.ajax({
        url: 'http://localhost:56441/user/UserExists?userID=' + userID,
        type: 'GET',
        contentType: "application/json;charset=utf-8",
        async: false,
        success: function (response) {
            if (response == "True") {
                returnMessage = action == "create" ? "This UserID exists." : "";
            }
        },
        error: function (response) {
            returnMessage = response;
        }
    });

    return returnMessage;
}

function isValidUser(action) {

    var userIdInvalidMessage = validateUserID();
    var firstNameInvalidMessage = validateFirstName();
    var lastNameInvalidMessage = validateLastName();
    var emailInvalidMessage = validateEmailAddress();
    var finalErrorMessage = "Errors:";

    if (userIdInvalidMessage != "")
        finalErrorMessage += "\n" + userIdInvalidMessage;
    if (firstNameInvalidMessage != "")
        finalErrorMessage += "\n" + firstNameInvalidMessage;
    if (lastNameInvalidMessage != "")
        finalErrorMessage += "\n" + lastNameInvalidMessage;
    if (emailInvalidMessage != "")
        finalErrorMessage += "\n" + emailInvalidMessage;

    if (finalErrorMessage != "Errors:") {
        alert(finalErrorMessage);
        return false;
    } else {
        userIdInvalidMessage = validateUserExistence(action);
        if (userIdInvalidMessage != "") {
            finalErrorMessage += "\n" + userIdInvalidMessage;
            alert(finalErrorMessage);
            return false;
        } else {
            return true;
        }
    }
}