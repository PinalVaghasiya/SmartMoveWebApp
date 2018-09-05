/*global jQuery */
(function ($) {
    "use strict";

    jQuery(document).ready(function ($) {
        /*---------------------------------
         All Window Scroll Function Start
        --------------------------------- */
        $(window).scroll(function () {
            // Header Fix Js Here
            if ($(window).scrollTop() >= 200) {
                $('#header-area').addClass('fixTotop');
            } else {
                $('#header-area').removeClass('fixTotop');
            }

            // Scroll top Js Here
            if ($(window).scrollTop() >= 400) {
                $('.scroll-top').slideDown(400);
            } else {
                $('.scroll-top').slideUp(400);
            }
        });
        /*--------------------------------
         All Window Scroll Function End
        --------------------------------- */

        // Click to Scroll TOP
        $(".scroll-top").click(function () {
            $('html, body').animate({
                scrollTop: 0
            }, 1500);
        }); //Scroll TOP End

        // SlickNav or Mobile Menu
        $(".mainmenu").slicknav({
            'label': '',
            'prependTo': '#header-bottom .container .row'
        }); // SlickNav End


        // Home Page Two Slideshow
        $("#slideslow-bg").vegas({
            overlay: true,
            transition: 'fade',
            transitionDuration: 2000,
            delay: 4000,
            color: '#000',
            animation: 'random',
            animationDuration: 20000,
            slides: [
                {
                    src: 'assets/img/slider-img/slider-img-1.jpg'
                },
                {
                    src: 'assets/img/slider-img/slider-img-2.jpg'
                },
                {
                    src: 'assets/img/slider-img/slider-img-3.jpg'
                },
                {
                    src: 'assets/img/slider-img/slider-img-4.jpg'
                }
            ]
        }); //Home Page Two Slideshow
    }); //Ready Function End

    jQuery(window).load(function () {
        jQuery('.preloader').fadeOut();
        jQuery('.preloader-spinner').delay(350).fadeOut('slow');
        jQuery('body').removeClass('loader-active');
    }); //window load End


}(jQuery));