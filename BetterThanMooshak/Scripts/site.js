$scrollTo = function(selector) {
    $('html, body').animate({
        scrollTop: ($(selector).offset().top) - 80
    }, 1500);
}

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
            site.solution.answer(true);
        } else {
            site.solution.answer(false);
        }
    },
    answer : function(isReadOnly) {
        if ($('#answerEditor').length > 0) {
            site.solution.answerEditor = ace.edit("answerEditor");
            site.solution.answerEditor.setOptions({
                minLines: 20,
                maxLines: 30,
                showPrintMargin: false,
                fontSize: 18,
                animatedScroll: true,
                theme: "ace/theme/xcode",
                readOnly: isReadOnly
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
    submitSuccess: function (data) {
        console.log('SuccessFunction Started');
        var response = data;
        console.log(response);
        $('#compilerRespond').empty();
        if (response.maxAttemptsReach) {
            $compileError =
                $('<div class="alert alert-warning alert-dismissible" role="alert">'
                    + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>'
                    + '<strong>Warning!</strong> Max Attempts reach Error!'
                    + '<hr />'
                    + '<span style="white-space:pre-line;">'
                        + response.errorMessage
                    + '</span>'
                + '</div>');
            $('#compilerRespond').append($compileError);
        }
        else if (response.hasCompileError) {
            $compileError =
                $('<div class="alert alert-danger alert-dismissible" role="alert">'
                    + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>'
                    + '<strong>Warning!</strong> Compile Error'
                    + '<hr />'
                    + '<span style="white-space:pre-line;">'
                        + response.errorMessage
                    + '</span>'
                + '</div>');
            $('#compilerRespond').append($compileError);
        } else {
            for (i = 0; i < response.tests.length; i++) {
                $type = "warning";
                $text = "<strong>Wrong Output</strong> Something is not going as planes";
                if (response.tests[i].isCorrect > 0) {
                    $type = "success";
                    $text = "<strong>Correct Output</strong> Looks like you did it! Congratulations;";
                }
                $output =
                $('<div class="alert alert-' + $type + ' alert-dismissible" role="alert">'
                    + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>'
                    + $text
                    + '<hr />'
                    + '<div class="panel panel-info">'
                        + '<div class="panel-heading">Input</div>'
                        + '<div class="panel-body">'
                            + '<span style="white-space:pre-line;">'
                                + ((response.tests[i].input == "null") ? "" : response.tests[i].input)
                            + '</span>'
                        + '</div>'
                    + '</div>'
                    + '<div class="panel panel-info">'
                        + '<div class="panel-heading">Expected Output</div>'
                        + '<div class="panel-body">'
                            + '<span style="white-space:pre-line;">'
                                + response.tests[i].expectedOutput
                            + '</span>'
                        + '</div>'
                    + '</div>'
                    + '<div class="panel panel-info">'
                        + '<div class="panel-heading">Optained Output</div>'
                        + '<div class="panel-body">'
                            + '<span style="white-space:pre-line;">'
                                + response.tests[i].output
                            + '</span>'
                        + '</div>'
                    + '</div>'
                + '</div>');

                $('#compilerRespond').append($output);
            }
        }
        $scrollTo('#compilerRespond');
        console.log('SuccessFunction Ended');
    },
    timeout: function () {
        $('#compilerRespond').empty();
        $compileError =
               $('<div class="alert alert-danger alert-dismissible" role="alert">'
                   + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>'
                   + '<strong>Warning!</strong> Runtime Error'
                   + '<hr />'
                   + '<span style="white-space:pre-line;">'
                       + 'The Process took to long and had to be terminated by the operating system!'
                   + '</span>'
               + '</div>');
        $scrollTo('#compilerRespond');
        $('#compilerRespond').append($compileError);
    },
    save: function (id) {
        $.ajax({
            url: "/solution/save/"+id,
            timeout: 60000,
            method: "POST",
            data: {
                code: site.solution.editor.getValue()
            },
            success: site.solution.success
        })
        .done(function (data) {

        })
        .fail(function (jqXHR, textStatus) {

        });
    },
    submit: function (id) {
        site.solution.debug =  $.ajax({
            url: "/solution/submit/"+id,
            method: "POST",
            timeout: 60000,
            data: {
                code: site.solution.editor.getValue()
            }
        })
            .done(function (data) {
                site.solution.submitSuccess(data)
            })
            .fail(function (jqXHR, textStatus) {
                console.log('fail function called');
                if (textStatus === 'timeout') {
                    site.solution.timeout();
                }
                console.log('fail function ended');
            });
    },
    load: function (id) {
        $.ajax({
            url: "/solution/load/" + id,
            method: "POST",
            timeout: 60000,
        })
            .done(function (data) {
                site.solution.editor.setValue(data);
            })
            .fail(function (jqXHR, textStatus) {
                console.log('fail function called for load');
            });
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

