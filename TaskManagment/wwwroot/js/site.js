$(document).ready(function () {
    const url = "http://localhost:50704/";
    const inviteMemberUrl = url + "Project/InviteMember";

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

    $('.invite-member-btn').click(function () {
        const email = $('#invite-member-input').val();
        const projectId = $('#invite-projectId').val();
        const projectName = $('#invite-projectName').val();
        $.post(inviteMemberUrl, { email: email, projectId: projectId, projectName: projectName })
            .done(data => console.log(data),
                error => console.log(error));
        $('#invite-member-input').val("");
    });
});