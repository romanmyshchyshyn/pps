$(document).ready(function () {
    const url = "http://localhost:50704/";
    const inviteMemberUrl = url + "Project/InviteMember";
    const addTaskUrl = url + "CustomTask/Create";
    const editTaskDescriptionUrl = url + "CustomTask/EditDescription";
    const editTaskDeadlineUrl = url + "CustomTask/EditDeadline";
    const getTaskUrl = url + "CustomTask/Get";
    const updateTaskStatusUrl = url + "CustomTask/UpdateStatus";

    function formatDate(d) {
        var d_names = new Array("Sunday", "Monday", "Tuesday",
            "Wednesday", "Thursday", "Friday", "Saturday");

        var m_names = new Array("January", "February", "March",
            "April", "May", "June", "July", "August", "September",
            "October", "November", "December");
        
        var curr_day = d.getDay();
        var curr_date = d.getDate();
        var sup = "";
        if (curr_date === 1 || curr_date === 21 || curr_date === 31) {
            sup = "st";
        }
        else if (curr_date === 2 || curr_date === 22) {
            sup = "nd";
        }
        else if (curr_date === 3 || curr_date === 23) {
            sup = "rd";
        }
        else {
            sup = "th";
        }
        var curr_month = d.getMonth();
        var curr_year = d.getFullYear();

        return (d_names[curr_day] + " " + curr_date + "<SUP>"
            + sup + "</SUP> " + m_names[curr_month] + " " + curr_year);
    }

    function formatDateForDataPicker(d) {

        var m_names = new Array("Jan", "Feb", "Mar",
            "Apr", "May", "Jun", "Jul", "Aug", "Sep",
            "Oct", "Nov", "Dec");

        var curr_month = d.getMonth();
        var curr_date = d.getDate();
        var curr_year = d.getFullYear();

        return m_names[curr_month] + " " + curr_date + ", " + curr_year;
    }


    M.updateTextFields();
    $('.datepicker').datepicker({
        container: document.body,
        minDate: new Date(),
        onSelect: function (date) {

            const taskId = $('#task-edit-modal-id-input').val();
            $.post(editTaskDeadlineUrl, { id: taskId, deadline: date })
                .done(data => data)
                .fail(error => console.log(error));
        }        
    });

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
                        
                    </h4>
                    <a class="waves-effect waves-light modal-trigger task-edit-modal-trigger" href="#task-edit-modal"></a>
                </div>`;

                const tableContent = $(this).closest('.project-tasks-table').find('.project-tasks-table-content');
                const templateElem = $(template);
                templateElem.find('.project-tasks-table-item-title').text(task.name);
                templateElem.css({ display: 'none' });
                tableContent.prepend(templateElem);
                templateElem.slideDown(500);
            })
            .fail(error => console.log(error));

        addTaskInput.val("");
        $(this).closest('.project-tasks-table-add-task').slideUp(500);
    });

    $('.task-edit-modal-trigger').click(function () {
        const taskId = $(this).closest('.project-tasks-table-item').attr("taskId");
        const taskNameField = $('.task-edit-modal-name');
        const taskStatusElem = $('.task-edit-modal-info-status b');
        const taskCreatorElem = $('.task-edit-modal-info-creator b');
        const taskCreationDateElem = $('.task-edit-modal-info-creation-date');
        const taskIdInput = $('#task-edit-modal-id-input');
        const taskDescriptionTextarea = $('#task-edit-modal-description-textarea');
        const taskDeadlineInput = $('.task-edit-modal-deadline-input');
        
        console.log(getTaskUrl + `?id=${taskId}`);

        $.get(getTaskUrl + `?id=${taskId}`, function (task) {
            taskIdInput.val(task.id);
            taskNameField.text(task.name);
            taskStatusElem.text(task.status);
            taskCreatorElem.text(task.userCreatorFullName);
            taskCreationDateElem.html(formatDate(new Date(task.creationDate)));
            taskDescriptionTextarea.val(task.description);
            taskDeadlineInput.val(formatDateForDataPicker(new Date(task.deadline)));

            M.updateTextFields();
        })
        .fail(error => console.log(error));
    });

    $('.task-edit-modal-description-clean-btn').click(function () {
        const textareaElem = $(this).closest('.task-edit-modal-description').find('#task-edit-modal-description-textarea');
        textareaElem.val("");
    });

    $('.task-edit-modal-description-save-btn').click(function () {
        const textareaElem = $(this).closest('.task-edit-modal-description').find('#task-edit-modal-description-textarea');
        const description = textareaElem.val();
        const taskId = $('#task-edit-modal-id-input').val();

        $.post(editTaskDescriptionUrl, { id: taskId, description: description })
            .done(data => data)
            .fail(error => console.log(error));
    });
});