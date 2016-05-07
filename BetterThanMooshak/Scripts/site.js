$(function () {
    $('#mainmenuToggle').click(function () {
        $('body').toggleClass('menuOpen');
    });

    $("input.datepicker").datetimepicker({
        format: "DD/M/YYYY HH:mm",
        calendarWeeks: true,
        sideBySide: true
    });
})