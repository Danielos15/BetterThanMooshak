$(function () {
    $('#mainmenuToggle').click(function () {
        $('body').toggleClass('menuOpen');
    });

    $("input.datepicker").datepicker();
})