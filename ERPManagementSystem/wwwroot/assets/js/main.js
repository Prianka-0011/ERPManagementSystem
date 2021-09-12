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
    //popup
    

 //cart quantity
    var stockQuantity = $('#stockQuantity').val();
    $('#cartQuantity').on('change', function () {
        console.log(" this.defaultValue", this.defaultValue)


        //var direction = this.defaultValue < this.value
        //this.defaultValue = this.value;
        //if (direction) alert("increase!");
        //else alert("decrease!");

        var $tableBody = $('.cartTable').find("tbody");
        var $closest = $tableBody.find(".cartTr");

        var cartQuentity = $closest.find("td").find('#cartQuantity').val();
        if (cartQuentity >=1) {
            if (cartQuentity < stockQuantity) {



                console.log("cartTolat", stockQuantity)
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
            else {
                console.log("$stockQuantity", stockQuantity)

                $closest.find("td").find('#cartQuantity').val(stockQuantity)
                var cartQuentity = $closest.find("td").find('#cartQuantity').val();
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
        }
        else {
            var lowQnt = 1;
            $closest.find("td").find('#cartQuantity').val(lowQnt)
        }
    });
    $('#cartQuantity').on('keyup', function () {
        var $tableBody = $('.cartTable').find("tbody");
        var $closest = $tableBody.find(".cartTr");

        var cartQuentity = $closest.find("td").find('#cartQuantity').val();
        if (cartQuentity >= 1) {
            if (cartQuentity < stockQuantity) {
                console.log("cartTolat", stockQuantity)
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
            else {
                console.log("$stockQuantity", stockQuantity)

                $closest.find("td").find('#cartQuantity').val(stockQuantity)
                var cartQuentity = $closest.find("td").find('#cartQuantity').val();
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
        }
        else {
            var lowQnt = null;
            $closest.find("td").find('#cartQuantity').val(lowQnt)
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


ShowInQuickCard = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#productModal .modal-body').html(res);
            $('#productModal').modal('show');
            $('#quickCardQuantity').on('keyup', function () {

                var cardQuantity = $('#quickCardQuantity').val();

                var quickCardStockQuantity = $('#quickStockQuantity').val();
                if (cardQuantity >= 1) {
                    if (cardQuantity < quickCardStockQuantity) {
                        console.log("cart Quantity", cardQuantity, quickCardStockQuantity);
                    }
                    else {
                        $('#quickCardQuantity').val(quickCardStockQuantity);
                    }
                } else {
                    var lowQunty = null;
                    $('#quickCardQuantity').val(lowQunty);
                }
            });
            $('#quickCardQuantity').on('change', function () {

                var cardQuantity = $('#quickCardQuantity').val();

                var quickCardStockQuantity = $('#quickStockQuantity').val();
                if (cardQuantity >= 1) {
                    if (cardQuantity < quickCardStockQuantity) {
                        console.log("cart Quantity", cardQuantity, quickCardStockQuantity);
                    }
                    else {
                        $('#quickCardQuantity').val(quickCardStockQuantity);
                    }
                } else {
                    var lowQunty = 1;
                    $('#quickCardQuantity').val(lowQunty);
                } 
            });
        }
    });

}

$(window).scroll(function () {
    if ($(window).scrollTop() ==
        $(document).height() - $(window).height()) {
        GetData();
    }
});

function GetData() {
    $.ajax({
        type: 'GET',
        url: '/Home/Index',
        
        dataType: 'json',
        success: function (data) {
            if (data != null) {
                for (var i = 0; i < data.length; i++) {
                    $("#container").append("<h2>" +
                        data[i].CompanyName + "</h2>");
                }
                pageIndex++;
            }
        },
        beforeSend: function () {
            $("#progress").show();
        },
        complete: function () {
            $("#progress").hide();
        },
        error: function () {
            alert("Error while retrieving data!");
        }
    });
}