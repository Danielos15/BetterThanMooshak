site = {};

site.enrole = {
    roles: {
        teachers: [],
        assistants: [],
        students: []
    },

    getRoles: function () {
        //Get All Teachers
        site.enrole.roles.teachers = [];
        $('.teachers.users li').each(function () {
            site.enrole.roles.teachers.push($(this).attr('id'));
        });
        //Get All Assitants
        site.enrole.roles.assistants = [];
        $('.tas.users li').each(function () {
            site.enrole.roles.assistants.push($(this).attr('id'));
        });
        //Get All Students
        site.enrole.roles.students = [];
        $('.students.users li').each(function () {
            site.enrole.roles.students.push($(this).attr('id'));
        });
    },

    submit: function () {
        site.enrole.getRoles();
        var json = JSON.stringify(site.enrole.roles);

        $('form#enroleForm #roles').val(json);
    }
}
site.testcase = {
    add: function () {
       return false;
    },
    submit: function () {

    }
}
$(function () {

    $('form#enroleForm').submit(function() {
        site.enrole.submit();
    });

    $('#addTestcaseModal .save').click(function () {
        site.testcase.add();
    });

    $('#mainmenuToggle').click(function () {
        $('body').toggleClass('menuOpen');
    });

    $("input.datepicker").datetimepicker({
        format: "DD/M/YYYY HH:mm",
        calendarWeeks: true,
        sideBySide: true,
        toolbarPlacement: 'top',
        showTodayButton: true,
        showClose: true,
        showClear: true,
        widgetPositioning: {horizontal: 'left', vertical: 'bottom'}
    });
});

$(function () {

    function contains(searchInput, text)
    {
        if (searchInput.indexOf(text) != -1)
            return true;
    }

    $(".searchingFilter")
        .keyup(function() {
            var searchText = $("#userSearch").val().toLowerCase()
            $(".searching")
                .each(function() {
                    if (!contains($(this).text().toLowerCase(), searchText))
                        $(this).hide();
                    else
                        $(this).show();
                });
        });

    $(".clickable-row").click(function () {
        window.document.location = $(this).data("url");
    });
});

