(function($) {
    "use strict";

    /*-------------------------
      main-menu active
    --------------------------*/
    $('.main-menu nav').meanmenu({
        meanScreenWidth: "991",
        meanMenuContainer: '.mobile-menu'
    });

    /*-------------------------
      search active
    --------------------------*/
    $(".icon-search").on("click", function() {
        $(this).parent().find('.toogle-content').slideToggle('medium');
    })


    /*-------------------------
      slider active
    --------------------------*/
    $('.slider-active').owlCarousel({
        loop: true,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        items: 1,
        dots: false,
        nav: true,
        navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    })

    /*-------------------------
      product thumb img slider
    --------------------------*/
    $('.pro-thumb-img-slider').owlCarousel({
        loop: true,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        items: 4,
        dots: false,
        margin: 25,
        nav: true,
        navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
        responsive: {
            0: {
                items: 3
            },
            600: {
                items: 3
            },
            1000: {
                items: 4
            }
        }
    })

    /*-------------------------
      testimonial-active
    --------------------------*/

    $('.testimonial-active').owlCarousel({
        loop: true,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        items: 1,
        dots: false,
        nav: false,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    })

    /*--- showlogin toggle function ----*/
    
    $( '#showlogin' ).on('click', function() {
        $( '#checkout-login' ).slideToggle(900);
    }); 

    /*--- showlogin toggle function ----*/
    $( '#showcoupon' ).on('click', function() {
        $( '#checkout_coupon' ).slideToggle(900);
    });
    
    /*--- showlogin toggle function ----*/
    $('#ship-box').on('click', function() {
        $('#ship-box-info').slideToggle(1000);
    });


    /*----------------------------
        youtube video
    ------------------------------ */
    $('.youtube-bg').YTPlayer({
        containment: '.youtube-bg',
        autoPlay: true,
        loop: true,
    });




    /* isotop active */
    
    $('.grid').imagesLoaded( function() {

        // init Isotope
        var $grid = $('.grid').isotope({
            itemSelector: '.grid-item',
            percentPosition: true,
            masonry: {
                // use outer width of grid-sizer for columnWidth
                columnWidth: '.grid-item',
            }
        });
    });
    
    
    
    
    
    
    
    
    
    
    
    /* counterUp */
    $('.count').counterUp({
        delay: 10,
        time: 1000
    });

    /*-------------------------------------------
        03. scrollUp jquery active
    --------------------------------------------- */
    $.scrollUp({
        scrollText: '<i class="fa fa-angle-up"></i>',
        easingType: 'linear',
        scrollSpeed: 900,
        animation: 'fade'
    });


    /*--
     Menu Sticky
    -----------------------------------*/
    var windows = $(window);
    var stickey = $(".style-6");

    windows.on('scroll', function() {
        var scroll = windows.scrollTop();
        if (scroll < 1) {
            stickey.removeClass("stick");
        } else {
            stickey.addClass("stick");
        }
    });



 //cart quantity
    var stockQuantity = $('.stockQuantity').val();
    $('#cartQuantity').on('change', function () {
        var $tableBody = $('.cartTable').find("tbody");
        var $closest = $tableBody.find(".cartTr");
        var $stockProduct = $tableBody.find("#stockQuantity").val();

        var cartQuentity = $closest.find("td").find('#cartQuantity').val();
        if (cartQuentity < $stockProduct) {
            console.log("cartTolat", $stockProduct)
            var salePrice = $closest.find("td").find('#salePrice').val();
            var updateProdutAmount = cartQuentity * salePrice;
            $closest.find("td").find('#productTotalSpan').text(updateProdutAmount);
            $closest.find("td").find('#productTotal').val(updateProdutAmount);
            var productAmount = 0;
            $(".cartTable .cartTr").each(function (row, tr) {
                var amount = parseFloat($(this).closest('tr').find('#productTotal').val());
                if (isNaN(amount)) {
                    amount = 0;
                }
                console.log("amount", amount);
                productAmount = amount + productAmount;

            });
            $('.cartSubtotal').text(productAmount);
        } else {
            console.log("cartTolat", stockProduct)
            cartQuentity = $stockProduct;
            var salePrice = $closest.find("td").find('#salePrice').val();
            var updateProdutAmount = cartQuentity * salePrice;
            $closest.find("td").find('#productTotalSpan').text(updateProdutAmount);
            $closest.find("td").find('#productTotal').val(updateProdutAmount);
            var productAmount = 0;
            $(".cartTable .cartTr").each(function (row, tr) {
                var amount = parseFloat($(this).closest('tr').find('#productTotal').val());
                if (isNaN(amount)) {
                    amount = 0;
                }
                console.log("amount", amount);
                productAmount = amount + productAmount;
            });
            $('.cartSubtotal').text(productAmount);
        }

    });
    $('#cartQuantity').on('keyup', function () {
        var $tableBody = $('.cartTable').find("tbody");
        var $closest = $tableBody.find(".cartTr");
        var $stockProduct = $tableBody.find("#stockQuantity").val();
        console.log("$stockProduct", stockProduct);
        var cartQuentity = $closest.find("td").find('#cartQuantity').val();
        if (cartQuentity < $stockProduct) {
            var salePrice = $closest.find("td").find('#salePrice').val();
            var updateProdutAmount = cartQuentity * salePrice;
            $closest.find("td").find('#productTotalSpan').text(updateProdutAmount);
            $closest.find("td").find('#productTotal').val(updateProdutAmount);
            var productAmount = 0;
            $(".cartTable .cartTr").each(function (row, tr) {
                var amount = parseFloat($(this).closest('tr').find('#productTotal').val());
                if (isNaN(amount)) {
                    amount = 0;
                }
                console.log("amount", amount);
                productAmount = amount + productAmount;

            });
            $('.cartSubtotal').text(productAmount);
        } else {
            cartQuentity = stockProduct;
            var salePrice = $closest.find("td").find('#salePrice').val();
            var updateProdutAmount = cartQuentity * salePrice;
            $closest.find("td").find('#productTotalSpan').text(updateProdutAmount);
            $closest.find("td").find('#productTotal').val(updateProdutAmount);
            var productAmount = 0;
            $(".cartTable .cartTr").each(function (row, tr) {
                var amount = parseFloat($(this).closest('tr').find('#productTotal').val());
                if (isNaN(amount)) {
                    amount = 0;
                }
                console.log("amount", amount);
                productAmount = amount + productAmount;
            });
            $('.cartSubtotal').text(productAmount);
        }
        
    });
/*-- Product Quantity --*/

    $('.product-quantity2').append('<span class="dec qtybtn"><i class="fa fa-angle-left"></i></span><span class="inc qtybtn"><i class="fa fa-angle-right"></i></span>');
    var quantity = $('.quatityDDD').val();
    $('.qtybtn').on('click', function () {
        var $button = $(this);
        var oldValue = $button.parent().find('input').val();
        
        if ($button.hasClass('inc')) {
            console.log("oldValue", oldValue, "  ", quantity)
            if (oldValue < quantity) {
               
                var newVal = parseFloat(oldValue) + 1;
            } else {

                newVal = quantity;
            }
            
           
        } else {
            // Don't allow decrementing below zero
            console.log("olsddfd", oldValue)
            if (oldValue > 1) {
                var newVal = parseFloat(oldValue) - 1;

            } else {
                newVal = 1;
            }
            oldValue = newVal;
        }
        $button.parent().find('input').val(newVal);
    });




})(jQuery);