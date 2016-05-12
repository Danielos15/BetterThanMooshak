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

site.solution = {
    init: function () {
        if ($('#editor').length > 0) {
            site.solution.localPath = $('#localPath').data('path');
            site.solution.editor = ace.edit("editor");
            site.solution.editor.setOptions({
                minLines: 20,
                maxLines: 30,
                showPrintMargin: false,
                fontSize: 18,
                animatedScroll: true,
                theme: "ace/theme/xcode"
            });

            site.solution.editor.getSession().setMode("ace/mode/c_cpp");
            site.solution.localGet();
            site.solution.editor.getSession().on("change", site.solution.localSave);

            //set Answer editor
            site.solution.answer();
        }
    },
    answer : function() {
        if ($('#answerEditor').length > 0) {
            site.solution.answerEditor = ace.edit("answerEditor");
            site.solution.answerEditor.setOptions({
                minLines: 20,
                maxLines: 30,
                showPrintMargin: false,
                fontSize: 18,
                animatedScroll: true,
                theme: "ace/theme/xcode",
                readOnly: true
            });
            site.solution.answerEditor.getSession().setMode("ace/mode/c_cpp");
        }
    },
    localGet : function() {
        if (localStorage.getItem(site.solution.localPath)) {
            site.solution.editor.setValue(localStorage.getItem(site.solution.localPath));
        }
    },
    localSave : function() {
        localStorage.setItem(site.solution.localPath, site.solution.editor.getValue());
    },
    success : function() {
        console.log("Post was a Success");
    },
    save: function (id) {
        $.ajax({
            url: "/solution/save/"+id,
            method: "POST",
            data: {
                code: site.solution.editor.getValue()
            },
            success: site.solution.success
        });
    },
    submit: function (id) {
        $.ajax({
            url: "/solution/submit/"+id,
            method: "POST",
            data: {
                code: site.solution.editor.getValue()
            },
            success: site.solution.success
        });
        return false;
    }
}
$(function () {
    site.solution.init();



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

