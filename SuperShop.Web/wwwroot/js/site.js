// Please see documentation at
// https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.


// Dark Mode Switching
function darkModeSwitching() {
    const switchInput = document.getElementById("darkModeSwitch");
    const htmlElement = document.documentElement;

    switchInput.checked = htmlElement.getAttribute("data-bs-theme") === "dark";

    switchInput.addEventListener("change", function () {
        if (this.checked) {
            htmlElement.setAttribute("data-bs-theme", "dark");
        } else {
            htmlElement.setAttribute("data-bs-theme", "light");
        }
    });
}


// Preview Single Image
function previewImage(event) {
    const reader = new FileReader();
    const preview = document.getElementById('preview');

    reader.onload = function () {
        preview.src = reader.result;
    };

    reader.readAsDataURL(event.target.files[0]);
}


// Preview Multiple Images
function previewImages(event) {
    const reader = new FileReader();
    const previews = document.getElementsByClassName('preview');

    reader.onload = function () {
        for (let i = 0; i < previews.length; i++) {
            previews[i].src = reader.result;
        }
    };

    reader.readAsDataURL(event.target.files[0]);
}


// Initialize Datepicker
function initializeDatepicker() {
    const userLang = navigator.language || navigator.languages[0];

    $('.date').datepicker({
        format: "dd/mm/yyyy", language: userLang, autoclose: true, todayHighlight: true
    });

    $('#datePicker').datepicker({
        format: "dd/mm/yyyy", language: userLang, autoclose: true, todayHighlight: true
    });
}


// Handle Delete Confirmation
// Handle Database Update Exception Error
function handleDbUpdateExceptionError() {

    let showErrorModal = '@(TempData["showErrorModal"] ?? false)'.toLowerCase();

    if (showErrorModal === "true") {
        $('#DbUpdateExceptionStaticBackdrop').modal('show');
    }


    let saveChangesError = $("input[name='saveChangesError']").val();

    if (saveChangesError === "True") {
        $('#DbUpdateExceptionStaticBackdrop').modal('show');

    }

    // let dbUpdateException = $("input[name='dbUpdateException']").val();

    // if (dbUpdateException === "True") {
    //     $('#DbUpdateExceptionStaticBackdrop').modal('show');
    // }
}


// Handle Document Ready Events
// function handleDocumentReady() {
//     // let id = -1;
//     // debugger;
//     darkModeSwitching();
// }


// Call the functions on document ready
$(document).ready(function () {
    initializeDatepicker();
    handleDbUpdateExceptionError();
    // handleDocumentReady();
    darkModeSwitching();
});