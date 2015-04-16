$(function(){
	$('header nav a,.suprise .btn').click(function () {
        $(window).scrollTo($(this).attr("href"), { offset: { top: 0 }, duration: 300 }, { easing: 'elasout' });
        return false;
    });
});