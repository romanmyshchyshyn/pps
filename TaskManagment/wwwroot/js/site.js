$(document).ready(function () {
    const url = "http://localhost:50704/";
    const inviteMemberUrl = url + "Project/InviteMember";
    const addTaskUrl = url + "CustomTask/Create";
    const updateTaskStatusUrl = url + "CustomTask/UpdateStatus";

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
        const inviteMemberInput = $('#invite-member-input');
        if (inviteMemberInput.hasClass('invalid') || inviteMemberInput.val() === "") {
            return;
        }

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
            .fail((error) => {
                console.log(error);

                span.text(error.responseText.substring(1, error.responseText.length - 1));
                span.removeClass();
                span.addClass('field-validation-error');
            }, "json");

        inviteMemberInput.val("");
    });

    $('#invite-member-input').keypress(function () {
        $('#invite-user-error-span').text("");
    });

    const tasksTableWrapper = document.getElementsByClassName('project-tasks-tables-wrapper')[0];
    if (tasksTableWrapper != null) {
        new Sortable(tasksTableWrapper, {
            animation: 150,
            handle: '.project-tasks-tables-wrapper-handle'
        });
    }
    

    const tasksTablesContent = document.getElementsByClassName('project-tasks-table-content');
    if (tasksTablesContent != null) {
        [].forEach.call(tasksTablesContent, function (elem) {
            new Sortable(elem, {
                group: 'tasks',
                animation: 150,
                onAdd: function (evt) {
                    const item = $(evt.item);
                    const id = $(item).attr("taskId");
                    const status = item.closest('.project-tasks-table').find('.project-tasks-table-header-text-status').text().trim();

                    $.post(updateTaskStatusUrl, { id, status })
                        .done(() => { })
                        .fail(error => console.log(error));
                }
            });
        });
    }    

    $('.project-tasks-table-header-add-btn').click(function () {        
        $(this).closest('.project-tasks-table').find('.project-tasks-table-add-task').slideToggle(500);
    });

    $('.project-tasks-table-add-task-cancel-btn').click(function () {
        $(this).closest('.project-tasks-table-add-task').slideUp(500);
    });

    $('.project-tasks-table-add-task-save-btn').click(function () {
        const addTaskInput = $(this).closest('.project-tasks-table-add-task').find('.project-tasks-table-add-task-input');
        const taskName = addTaskInput.val();
        if (taskName === "" || taskName == null) {
            return;
        }

        const taskStatusSpan = $(this).closest('.project-tasks-table').find('.project-tasks-table-header-text-status');
        const taskStatus = taskStatusSpan.text().trim();
        const projectId = $('#invite-projectId').val();

        $.post(addTaskUrl, { name: taskName, status: taskStatus, projectId: projectId })
            .done(task => {
                const template = `
                <div class="project-tasks-table-item" taskId=${task.id}>
                    <div class="user-image project-tasks-table-item-user-image" style="background-image:url('${task.userAssigneeImagePath}')">
                       
                    </div>
                    <h4 class="project-tasks-table-item-title">
                        ${task.name}
                    </h4>
                </div>`;

                const tableContent = $(this).closest('.project-tasks-table').find('.project-tasks-table-content');
                const templateElem = $(template);
                templateElem.css({ display: 'none' });
                tableContent.prepend(templateElem);
                templateElem.slideDown(500);
            })
            .fail(error => console.log(error));

        addTaskInput.val("");
        $(this).closest('.project-tasks-table-add-task').slideUp(500);
    });

});