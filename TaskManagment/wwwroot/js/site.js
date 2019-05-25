$(document).ready(function () {
    M.updateTextFields();

    $(".dropdown-trigger").dropdown({
        coverTrigger: false,
        inDuration: 300,
        outDuration: 225,
        hover: true, 
        alignment: "left",
        constrainWidth: true
    });
});