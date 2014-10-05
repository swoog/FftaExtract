$(document).ready(function () {
    $('a[href^="#"].year').click(function () {
        location.href = $(this).attr("href");
        return true;
    });

    var url = document.location.href, idx = url.indexOf("#");
    var the_id = idx != -1 ? url.substring(idx) : $('a[href^="#"].year').attr("href");

    $('a[href^="' + the_id + '"].year').tab('show');
    $(the_id).addClass("fade").addClass("in");
});
