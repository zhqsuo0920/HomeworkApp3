$(document).ready(function () {

    $.ajaxSetup({ cache: false });

    $("#openDialog").on("click", function (e) {
       
        e.preventDefault();
        var url = $(this).attr('href');
        var theDialog = $("#dialog-create").dialog({
            title: 'Create User',
            autoOpen: false,
            resizable: false,
            height: 450,
            width: 400,
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            open: function (event, ui) {
                $(this).load(url);
                $(".ui-dialog-titlebar-close").hide();
            }
        });
        theDialog.dialog("open");

        return false;
    });

    $(".openDeleteDialog").on("click", function (e) {

        e.preventDefault();
        var url = $(this).attr('href');
        var theDialog = $("#dialog-delete").dialog({
            title: 'Delete User',
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 400,
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            open: function (event, ui) {
                $(this).load(url);
                $(".ui-dialog-titlebar-close").hide();
            }
        });
        theDialog.dialog("open");

        return false;
    });

    $(".openEditDialog").on("click", function (e) {

        e.preventDefault();
        var url = $(this).attr('href');
        var theDialog = $("#dialog-edit").dialog({
            title: 'Edit User',
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 400,
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            open: function (event, ui) {
                $(this).load(url);
                $(".ui-dialog-titlebar-close").hide();
            }
        });
        theDialog.dialog("open");

        return false;
    });
});

function resetUserFields() {
    var txtUserID = document.getElementById('txt-userid');
    var txtFirstName = document.getElementById('txt-fname');
    var txtLastName = document.getElementById('txt-lname');
    var txtEmail = document.getElementById('txt-email');

    if (txtUserID) txtUserID.value = "";
    if (txtFirstName) txtFirstName.value = "";
    if (txtLastName) txtLastName.value = "";
    if (txtEmail) txtEmail.value = "";

    return false;
}
function restoreUserFields() {

    document.getElementById('txt-fname').value = document.getElementById('original-fname').value;
    document.getElementById('txt-lname').value = document.getElementById('original-lname').value;
    document.getElementById('txt-email').value = document.getElementById('original-email').value;
    
    return false;
}
