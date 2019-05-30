$(document).ready(function () {
    const url = "http://localhost:50704/";
    const inviteMemberUrl = url + "Project/InviteMember";

    M.updateTextFields();

    $(".dropdown-trigger").dropdown({
        coverTrigger: false,
        inDuration: 10,
        outDuration: 10,
        alignment: "left",
        constrainWidth: true
    });

    $('.modal').modal();

    $('.hamburger').click(function () {
        $(this).toggleClass('is-active');
    });

    $('.invite-member-btn').click(function () {
        const email = $('#invite-member-input').val();
        const projectId = $('#invite-projectId').val();
        const projectName = $('#invite-projectName').val();
        const span = $('#invite-user-error-span');
        $.post(inviteMemberUrl, { email: email, projectId: projectId, projectName: projectName })
            .done(data => {
                console.log(data);
                span.text("Email was sent.");
                span.removeClass();
                span.addClass('field-validation-success');
            }, "json")
            .fail(() => {                
                span.text("There is no user with such email.");
                span.removeClass();
                span.addClass('field-validation-error');
            }, "json");
        $('#invite-member-input').val("");
    });

    $('#invite-member-input').keypress(function () {
        $('#invite-user-error-span').text("");
    });
});