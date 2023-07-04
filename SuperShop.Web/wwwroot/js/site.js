// Please see documentation at
// https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.


// ------------------------------------------------------------------------------------------------------------------ //
//
// Dark Mode
// https://getbootstrap.com/docs/5.0/customize/color/#dark-mode
//
// ------------------------------------------------------------------------------------------------------------------ //

$(document).ready(function () {
    let switchInput = document.getElementById("darkModeSwitch");
    let htmlElement = document.documentElement;

    // Set initial state based on data-bs-theme attribute
    switchInput.checked = htmlElement.getAttribute("data-bs-theme") === "dark";

    // Toggle theme when switch is clicked
    switchInput.addEventListener("change", function () {
        if (this.checked) {
            htmlElement.setAttribute("data-bs-theme", "dark");
        } else {
            htmlElement.setAttribute("data-bs-theme", "light");
        }
    });
});


// ------------------------------------------------------------------------------------------------------------------ //
//
// Preview Image and preview Images
//
// ------------------------------------------------------------------------------------------------------------------ //

function previewImage(event) {
    let reader = new FileReader();

    reader.onload = function () {
        let preview = document.getElementById('preview');
        preview.src = reader.result;
    }

    reader.readAsDataURL(event.target.files[0]);
}


function previewImages(event) {
    let reader = new FileReader();

    reader.onload = function () {
        let previews = document.getElementsByClassName('preview');

        for (let i = 0; i < previews.length; i++) {
            previews[i].src = reader.result;
        }
    }

    reader.readAsDataURL(event.target.files[0]);
}


// ------------------------------------------------------------------------------------------------------------------ //
//
// Product Orders
//
// ------------------------------------------------------------------------------------------------------------------ //


$(document).ready(function () {

    let id = 0;

    $("#btnConfirm").click(function () {
        $("#confirmDialog").modal("show");
        return false;
    });

    $("#btnNoConfirm").click(function () {
        $("#confirmDialog").modal("hide");
        return false;
    });

    $("#btnYesConfirm").click(function () {
        window.location.href = "/Orders/ConfirmOrder";
    });

    $("a[id*=btnDeleteItem]").click(function () {
        id = $(this).parent()[0].id;
        $("#deleteDialog").modal("show");
        return false;
    });

    $("#btnNoDelete").click(function () {
        $("#deleteDialog").modal("hide");
        return false;
    });

    $("#btnYesDelete").click(function () {
        window.location.href = "/Orders/DeleteItem/" + id;
    });

    $("#closingXConfirmDialog").click(function () {
        $("#confirmDialog").modal("hide");
        return false;
    });

    $("#closingXDeleteDialog").click(function () {
        $("#deleteDialog").modal("hide");
        return false;
    });
});