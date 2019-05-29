$(document).ready(function () {
    M.updateTextFields();

    $(".dropdown-trigger").dropdown({
        coverTrigger: false,
        inDuration: 300,
        outDuration: 225,
        alignment: "left",
        constrainWidth: true
    });

    $('.modal').modal();

    $('.hamburger').click(function () {
        $(this).toggleClass('is-active');
    });
});