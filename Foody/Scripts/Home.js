
$(document).ready(function ($) {
    var nav = $('#menu-header');
    $(window).scroll(function () {
        if ($(this).scrollTop() > 80) {
            nav.addClass("navbar-fixed-top");
        } else {
            nav.removeClass("navbar-fixed-top");
        }
        if ($('div#menu-left').length) {
            if ((($(this).scrollTop()) + $('#menu-header').height()) > $('div#menu-left').parent().position().top) {
                console.log("menu search", $(this).scrollTop());
                $("div#menu-seach")
                    .css('top', nav.height())
                    .css('width', $("div#menu-seach").parent().width())
                    .css('position', 'fixed');
                $("div#menu-left")
                    .css('top', nav.height())
                    .css('width', $("div#menu-left").width())
                .css('position', 'fixed');
            } else{
                $("div#menu-seach").css('top', '0').css('position', 'relative');
                $("div#menu-left").css('top', '0').css('position', 'relative');
            }
            if (($(this).scrollTop() + $('#menu-header').height() + $('#menu-left').height()) > $('div#footer').position().top) {
                $("div#menu-seach").css('top', '0').css('position', 'relative');
                $("div#menu-left").css('top', '0').css('position', 'relative');
            }
        }

    });
});
