//Jquery start//
$('.dropdown0').click(function () {
    $('.sidebar-body ul .dropdown-menu0').toggleClass("show");
});
$('.dropdown2').click(function () {
    $('.sidebar-body ul .dropdown-menu2').toggleClass("show");
});
$('.dropdown3').click(function () {
    $('.sidebar-body ul .dropdown-menu3').toggleClass("show");
});
$('.dropdown4').click(function () {
    $('.sidebar-body ul .dropdown-menu4').toggleClass("show");
});
$('.dropdown5').click(function () {
    $('.sidebar-body ul .dropdown-menu5').toggleClass("show");
});
$('.bar').click(function () {
    $('.sidebar').toggleClass("toggle");
    $('.sidebar a span').toggleClass("toggle");
    $('.sidebar a .fa-caret-down').toggleClass("toggle");
    $('.sidebar .sidebar-head img').toggleClass("toggle");
    $('.sidebar .sidebar-head p').toggleClass("toggle");
    $('.main').toggleClass("toggle");
    $('.sidebar a').toggleClass("toggle");
    if ($('.sidebar-body ul .dropdown-menu0').hasClass("show")) {
        $('.sidebar-body ul .dropdown-menu0').removeClass("show");
    }
    if ($('.sidebar-body ul .dropdown-menu2').hasClass("show")) {
        $('.sidebar-body ul .dropdown-menu2').removeClass("show");
    }
    if ($('.sidebar-body ul .dropdown-menu3').hasClass("show")) {
        $('.sidebar-body ul .dropdown-menu3').removeClass("show");
    }
    if ($('.sidebar-body ul .dropdown-menu0').hasClass("show")) {
        $('.sidebar-body ul .dropdown-menu0').removeClass("show");
    }
});
function clear() {
    localStorage.clear();
}
//Load image
function loadImg(event) {
    var output = document.getElementById('mainimgDiv');
    var primaryimg = document.createElement("img");
    primaryimg.setAttribute("id", "primaryimg");
    var output = document.getElementById('mainimgDiv');
    primaryimg.src = URL.createObjectURL(event.target.files[0]);
    localStorage.setItem('primaryimg', primaryimg);
    primaryimg.onload = function () {
        URL.revokeObjectURL(primaryimg.src) // free memory
    }
    output.appendChild(primaryimg);

    if (event.target.files.length > 1) {
        var parent = document.getElementById('galleryParent');
        for (var i = 0; i < event.target.files.length; i++) {
            var childDiv = document.createElement("div");
            var gallery = document.createElement("img");
            var gallery = document.createElement("img");
            childDiv.setAttribute("class", "col-md-2 col-3 col-lg-2 removeclass" + i);
            gallery.setAttribute("id", "galleryimg");
            gallery.setAttribute("class", "minclass" + i);

            gallery.src = URL.createObjectURL(event.target.files[i]);
            gallery.onload = function () {
                URL.revokeObjectURL(gallery.src) // free memory
            }
            childDiv.appendChild(gallery);
            parent.appendChild(childDiv);

            var StoredDiv = $('.removeclass' + i).html();
            manage_append(i, StoredDiv);
        }
        function manage_append(i, html) {

            localStorage.setItem(i, html);
            console.log("html", html);
            //  localStorage.removeItem(i);    
        }
    }
};
//Jquery end//

// crud operation start

//For small popup start
ShowInSmallPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#small-modal .modal-body').html(res);
            $('#small-modal .modal-title').html(title);
            $('#small-modal').modal('show');

            $("#Designation").change(function () {
                var d = $("#Designation option:selected").val();
                $.ajax({
                    type: 'GET',
                    url: '/Employees/GetDesignationSalary/?id=' + d,
                    dataType: 'JSON',
                    success: function (data) {
                        console.log("salary", data)
                        var salary = document.getElementById("Salary");
                        salary.value = data;
                    },
                    error: function (err) {
                        console.log(err)
                    }
                })
            });
            $("#Designation").trigger("change");
        }
    });

}
ShowInLargePopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {

            $('#large-modal .modal-body').html(res);
            $('#large-modal .modal-title').html(title);
            $('#large-modal').modal('show');

            //$(".subcategory").v.empty();
            //$(".brand").empty();
            $(".category").click(function (e) {
                var selectedCategoryId = $(".category option:selected").val();

                $(".subcategory").empty();
                $(".brand").empty();
                $.ajax({
                    url: '/Products/GetAllSubCategory/?id=' + selectedCategoryId,
                    type: 'GET',
                    dataType: 'JSON',
                    success: function (data) {
                        $.each(data, function (i, obj) {
                            var s = '<option value="' + obj.id + '">' + obj.name + '</option>';
                            console.log("ggg", obj);
                            $(".subcategory").append(s);
                        });
                    },
                    error: function (res) {
                        console.log(res);
                    }
                })


            })
            $(".subcategory").click(function (e) {
                $(".brand").empty();
                var selectedSubCategoryId = $(".subcategory option:selected").val();
                $.ajax({
                    url: '/Products/GetAllBrand/?id=' + selectedSubCategoryId,
                    type: 'GET',
                    dataType: 'JSON',
                    success: function (data) {
                        $.each(data, function (i, obj) {
                            var s = '<option value="' + obj.id + '">' + obj.name + '</option>';
                            console.log("ggg", obj);
                            $(".brand").append(s);
                        });
                    },
                    error: function (res) {
                        console.log(res);
                    }
                })
            })

            //just for this portion i add 2 popup function
        }
    });

}
ShowInLargePopupPurchaQuotation = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {

            $('#large-modal .modal-body').html(res);
            $('#large-modal .modal-title').html(title);
            $('#large-modal').modal('show');
            //just for this portion i add 2 popup function
            var $tableBodyClear = $('.datatable').find("tbody");

            $trfirstclear = $tableBodyClear.find("tr:first");
            $trfirstclear.find("td").find(':input').val('');

            //  dynamically add or remove feild start
            $("#addAnother").click(function (e) {

                var $tableBody = $('.datatable').find("tbody");
                $trfirst = $tableBody.find("tr:last");
                $trNew = $trfirst.clone();
                $trNew.find("td").find(':input').val('');
                //this part add 
                var suffix = $trNew.find(':input:first').attr('name').match(/\d+/);
                console.log(suffix);
                $.each($trNew.find(':input'), function (i, val) {
                    $('#qt').keyup(function () {
                        var res = $('#qt').val() * $('#sum').val();
                        if (res == Number.POSITIVE_INFINITY || res == Number.NEGATIVE_INFINITY || isNaN(res))
                            res = "N/A"; // OR 0
                        $('#result').val(res);
                    });

                    // Replaced Name
                    var oldN = $(this).attr('name');
                    var newN = oldN.replace('[' + suffix + ']', '[' + (parseInt(suffix) + 1) + ']');

                    $(this).attr('name', newN);
                    //Replaced value
                    var type = $(this).attr('type');

                    // If you have another Type then replace with default value
                    $(this).removeClass("input-validation-error");

                });
                var img = $trNew.find('#productImg');
                img.attr("src", "https://localhost:44377/images/noimg.png");
                $trfirst.after($trNew);
                $trNew.find("td").find('#removeFeildWithValue').addClass('removeTR');
                $trfirst.find("td").find('#removeFeildWithValue').removeClass('refreshTR');
                $(this).removeClass("input-validation-error");
            });

            $('.datatable').on('click', '.removeTR', function () {
                $(this).parents('tr').remove();
            });
            $('.datatable').on('click', '.refreshTR', function () {
                $closestclear = $('.refreshTR').closest('tr');
                $closestclear.find("td").find(':input').val('');
                var img1 = $closestclear.find('#productImg');
                img1.attr("src", "https://localhost:44377/images/noimg.png");
            });

            //dynamically add or remove feild end
            //image change start
            $('.datatable').on('change', '#product', function () {
                var curentrow = $(this).closest("tr");
                var colVal = curentrow.find('#product').val();

                console.log("fddfsdfd", colVal);

                $.ajax({
                    url: '/Quotations/GetProductImg/?id=' + colVal,
                    type: 'GET',
                    dataType: 'JSON',
                    success: function (data) {
                        //  console.log(data);
                        var img = curentrow.find('#productImg');
                        var storeImgPath = curentrow.find('#imgPath');
                        var path = data.imagePath;
                        img.attr("src", "https://localhost:44377/" + path);
                        storeImgPath.val(data.imagePath);

                    },
                    error: function (res) {
                        console.log(res);
                    }
                });

            });
            //image change end 
            //tax Rate get
            $('.datatable').on('change', '#tax', function () {
                var curentrow = $(this).closest("tr");
                var colVal = curentrow.find('#tax').val();

                //console.log("fddfsdfd", colVal);

                $.ajax({
                    url: '/Quotations/GetTaxRate/?id=' + colVal,
                    type: 'GET',
                    dataType: 'JSON',
                    success: function (data) {
                        console.log("rate", data);
                        var taxRate1 = curentrow.find('#rate')
                        taxRate1.val(data.rate)
                        console.log("rate", taxRate1.val());

                        var price = curentrow.find('#price').val();
                        var discount = curentrow.find('#discount').val();
                        var discountAmount = (discount / 100) * price;
                        var taxRate = curentrow.find('#rate').val();

                        console.log("TAX RATE", taxRate);
                        var quantity = curentrow.find('#quantity').val();
                        
                        var totalCost = curentrow.find('#totalCost');
                        var perProductCost = curentrow.find('#perProductCost');
                        var taxAmount = (taxRate / 100) * price;
                        var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
                        var perProductCost1 = ((price - discountAmount) + taxAmount);
                        console.log("amount", totalAmount)
                        totalCost.val(totalAmount);
                        perProductCost.val(perProductCost1);
                        // final subtotal
                        var finalAmount = 0;
                        var productAmount = 0;
                        $(".datatable .define").each(function (row, tr) {
                            var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                            if (isNaN(amount)) {
                                amount = 0;
                            }
                            console.log("amount", amount);
                            productAmount = amount + productAmount;

                        });
                        console.log("productamount", productAmount);
                        var shippingCost = parseFloat($('.shippingcost').val());
                        var discount1 = parseFloat($('#discountOntotalOrder').val());
                        if (isNaN(discount)) {
                            discount = 0;
                        }
                        if (isNaN(shippingCost)) {
                            shippingCost = 0;
                        }
                        if (isNaN(productAmount)) {
                            productAmount = 0
                        }
                        finalAmount = productAmount + shippingCost;
                        var discountAmount = (discount1 / 100) * finalAmount;
                        finalAmount = finalAmount - discountAmount;
                        $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
                    },
                    error: function (res) {
                        console.log(res);
                    }
                });


            });


            //sub total count
            $('.datatable').on('keyup', '#tax', function () {
                var curentrow = $(this).closest("tr");
                var price = curentrow.find('#price').val();
                var discount = curentrow.find('#discount').val();
                var discountAmount = (discount / 100) * price;
                var taxRate = curentrow.find('#rate').val();
                var quantity = curentrow.find('#quantity').val();
                var totalCost = curentrow.find('#totalCost');
                var perProductCost = curentrow.find('#perProductCost');
                var taxAmount = (taxRate / 100) * price;
                var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
                var perProductCost1 = ((price - discountAmount) + taxAmount);
                console.log("amount", totalAmount)
                totalCost.val(totalAmount);
                perProductCost.val(perProductCost1);
                // final subtotal
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount1 = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;
                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount1 / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            });
            $('.datatable').on('keyup', '#price', function () {
                var curentrow = $(this).closest("tr");
                var price = curentrow.find('#price').val();
                var discount = curentrow.find('#discount').val();
                var discountAmount = (discount / 100) * price;
                var taxRate = curentrow.find('#rate').val();
                var quantity = curentrow.find('#quantity').val();
                var totalCost = curentrow.find('#totalCost');
                var perProductCost = curentrow.find('#perProductCost');
                var taxAmount = (taxRate / 100) * price;
                var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
                var perProductCost1 = ((price - discountAmount) + taxAmount);
                console.log("amount", totalAmount)
                totalCost.val(totalAmount);
                perProductCost.val(perProductCost1);
                // final subtotal
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount1 = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;
                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount1 / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            });
            $('.datatable').on('keyup', '#discount', function () {
                var curentrow = $(this).closest("tr");
                var price = curentrow.find('#price').val();
                var discount = curentrow.find('#discount').val();
                var discountAmount = (discount / 100) * price;
                var taxRate = curentrow.find('#rate').val();
                var quantity = curentrow.find('#quantity').val();
                var totalCost = curentrow.find('#totalCost');
                var perProductCost = curentrow.find('#perProductCost');
                var taxAmount = (taxRate / 100) * price;
                var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
                var perProductCost1 = ((price - discountAmount) + taxAmount);
                console.log("amount", totalAmount)
                totalCost.val(totalAmount);
                perProductCost.val(perProductCost1);
                // final subtotal
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount1 = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;
                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount1 / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            });
            $('.datatable').on('change', '#tax', function () {
                var curentrow = $(this).closest("tr");
                var price = curentrow.find('#price').val();
                var discount = curentrow.find('#discount').val();
                var discountAmount = (discount / 100) * price;
                var taxRate = curentrow.find('#rate').val();
                var quantity = curentrow.find('#quantity').val();
                var totalCost = curentrow.find('#totalCost');
                var perProductCost = curentrow.find('#perProductCost');
                var taxAmount = (taxRate / 100) * price;
                var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
                var perProductCost1 = ((price - discountAmount) + taxAmount);
                console.log("amount", totalAmount)
                totalCost.val(totalAmount);
                perProductCost.val(perProductCost1);
                // final subtotal
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount1 = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;
                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount1 / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            });

            $('.datatable').on('keyup', '#quantity', function () {
                var curentrow = $(this).closest("tr");
                var price = curentrow.find('#price').val();
                var discount = curentrow.find('#discount').val();
                var discountAmount = (discount / 100) * price;
                var taxRate = curentrow.find('#rate').val();
                var quantity = curentrow.find('#quantity').val();
                var totalCost = curentrow.find('#totalCost');
                var perProductCost = curentrow.find('#perProductCost');
                var taxAmount = (taxRate / 100) * price;
                var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
                var perProductCost1 = ((price - discountAmount) + taxAmount);
                console.log("amount", totalAmount)
                totalCost.val(totalAmount);
                perProductCost.val(perProductCost1);

                // final subtotal
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount1 = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;
                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount1 / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            });
            $('.datatable').on('change', '#quantity', function () {
                var curentrow = $(this).closest("tr");
                var price = curentrow.find('#price').val();
                var discount = curentrow.find('#discount').val();
                var discountAmount = (discount / 100) * price;
                var taxRate = curentrow.find('#rate').val();
                var quantity = curentrow.find('#quantity').val();
                var totalCost = curentrow.find('#totalCost');
                var perProductCost = curentrow.find('#perProductCost');
                var taxAmount = (taxRate / 100) * price;
                var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
                var perProductCost1 = ((price - discountAmount) + taxAmount);
                console.log("amount hhhhh", totalAmount, taxAmount)
                totalCost.val(totalAmount);
                perProductCost.val(perProductCost1);

                // final subtotal
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount1 = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;
                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount1 / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            });
            $('.shippingcost').keyup(function () {
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;
                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));

                console.log("shippingCost", shippingCost);
                console.log("discount", discount);
                console.log("finalAmount", finalAmount);
            });
            $('#discountOntotalOrder').keyup(function () {
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;
                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount / 100) * finalAmount;
                console.log("final +dis", (discount / 100) * finalAmount, finalAmount, discount, " hhh", discountAmount);
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));

            });

        }
    });

}
ShowInLargePopupOnEdit = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {

            $('#large-modal .modal-body').html(res);
            $('#large-modal .modal-title').html(title);
            $('#large-modal').modal('show');
            //dynamically add or remove feild start
            $("#addAnother").click(function (e) {

                var $tableBody = $('.datatable').find("tbody");
                $trfirst = $tableBody.find("tr:last");
                $trNew = $trfirst.clone();
                $trNew.find("td").find(':input').val('');
                //this part add 
                var suffix = $trNew.find(':input:first').attr('name').match(/\d+/);
                console.log(suffix);
                $.each($trNew.find(':input'), function (i, val) {
                    $('#qt').keyup(function () {
                        var res = $('#qt').val() * $('#sum').val();
                        if (res == Number.POSITIVE_INFINITY || res == Number.NEGATIVE_INFINITY || isNaN(res))
                            res = "N/A"; // OR 0
                        $('#result').val(res);
                    });

                    // Replaced Name
                    var oldN = $(this).attr('name');
                    var newN = oldN.replace('[' + suffix + ']', '[' + (parseInt(suffix) + 1) + ']');

                    $(this).attr('name', newN);
                    //Replaced value
                    var type = $(this).attr('type');

                    // If you have another Type then replace with default value
                    $(this).removeClass("input-validation-error");

                });
                var img = $trNew.find('#productImg');
                img.attr("src", "https://localhost:44377/images/noimg.png");
                $trfirst.after($trNew);
                $trNew.find("td").find('#removeFeildWithValue').removeClass('refreshTR');
                $trNew.find("td").find('#removeFeildWithValue').addClass('removeTR');
                $(this).removeClass("input-validation-error");
            });

            $('.datatable').on('click', '.removeTR', function () {
                $(this).parents('tr').remove();
            });
            $('.datatable').on('click', '.refreshTR', function () {
                $closestclear = $('.refreshTR').closest('tr');
                $closestclear.find("td").find(':input').val('');
                var img1 = $closestclear.find('#productImg');
                img1.attr("src", "https://localhost:44377/images/noimg.png");
            });

            //dynamically add or remove feild end
            //image change start
            $('.datatable').on('change', '#product', function () {
                var curentrow = $(this).closest("tr");
                var colVal = curentrow.find('#product').val();

                console.log("fddfsdfd", colVal);

                $.ajax({
                    url: '/Quotations/GetProductImg/?id=' + colVal,
                    type: 'GET',
                    dataType: 'JSON',
                    success: function (data) {
                        //  console.log(data);
                        var img = curentrow.find('#productImg');
                        var storeImgPath = curentrow.find('#imgPath');
                        var path = data.imagePath;
                        img.attr("src", "https://localhost:44377/" + path);
                        storeImgPath.val(data.imagePath);

                    },
                    error: function (res) {
                        console.log(res);
                    }
                });

            });
            //image change end 
            //tax Rate get
            $('.datatable').on('change', '#tax', function () {
                var curentrow = $(this).closest("tr");
                var price = curentrow.find('#price').val();
                var discount = curentrow.find('#discount').val();
                var discountAmount = (discount / 100) * price;
                var taxRate = curentrow.find('#rate').val();
                var quantity = curentrow.find('#quantity').val();
                if (quantity == null) {
                    quantity = 0;
                }
                var totalCost = curentrow.find('#totalCost');
                var perProductCost = curentrow.find('#perProductCost');
                var taxAmount = (taxRate / 100) * price;
                var totalAmount = ((price - discountAmount) + taxAmount) * quantity;

                var perProductCost1 = ((price - discountAmount) + taxAmount);
                console.log("amount datatable", totalAmount, discountAmount, taxAmount, price)
                totalCost.val(totalAmount);
                perProductCost.val(perProductCost1);
                // final subtotal
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                   // console.log("amount ffff", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount1 = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;
                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount1 / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            });

            //sub total count
            //$('.datatable').on('keyup', '#tax', function () {
            //    var curentrow = $(this).closest("tr");
            //    var price = curentrow.find('#price').val();
            //    var discount = curentrow.find('#discount').val();
            //    var discountAmount = (discount / 100) * price;
            //    var taxRate = curentrow.find('#rate').val();
            //    var quantity = curentrow.find('#quantity').val();
            //    var totalCost = curentrow.find('#totalCost');
            //    var perProductCost = curentrow.find('#perProductCost');
            //    var taxAmount = (taxRate / 100) * price;
            //    var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
            //    var perProductCost1 = ((price - discountAmount) + taxAmount);
            //    console.log("amount", totalAmount)
            //    totalCost.val(totalAmount);
            //    perProductCost.val(perProductCost1);
            //    // final subtotal
            //    var finalAmount = 0;
            //    var productAmount = 0;
            //    $(".datatable .define").each(function (row, tr) {
            //        var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
            //        if (isNaN(amount)) {
            //            amount = 0;
            //        }
            //        console.log("amount", amount);
            //        productAmount = amount + productAmount;

            //    });
            //    console.log("productamount", productAmount);
            //    var shippingCost = parseFloat($('.shippingcost').val());
            //    var discount1 = parseFloat($('#discountOntotalOrder').val());
            //    if (isNaN(discount)) {
            //        discount = 0;


            //    }
            //    if (isNaN(shippingCost)) {
            //        shippingCost = 0;
            //    }
            //    if (isNaN(productAmount)) {
            //        productAmount = 0
            //    }
            //    finalAmount = productAmount + shippingCost;
            //    var discountAmount = (discount1 / 100) * finalAmount;
            //    finalAmount = finalAmount - discountAmount;
            //    $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            //});
            $('.datatable').on('keyup', '#price', function () {
                var curentrow = $(this).closest("tr");
                var price = curentrow.find('#price').val();
                var discount = curentrow.find('#discount').val();
                var discountAmount = (discount / 100) * price;
                var taxRate = curentrow.find('#rate').val();
                var quantity = curentrow.find('#quantity').val();
                var totalCost = curentrow.find('#totalCost');
                var perProductCost = curentrow.find('#perProductCost');
                var taxAmount = (taxRate / 100) * price;
                var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
                var perProductCost1 = ((price - discountAmount) + taxAmount);
                console.log("amount", totalAmount)
                totalCost.val(totalAmount);
                perProductCost.val(perProductCost1);
                // final subtotal
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    else {
                        console.log("amount", amount);
                        productAmount = amount + productAmount;

                    }
                    
                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount1 = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;


                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                if (isNaN(quantity)) {
                    quantity = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount1 / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            });
            $('.datatable').on('keyup', '#discount', function () {
                var curentrow = $(this).closest("tr");
                var price = curentrow.find('#price').val();
                var discount = curentrow.find('#discount').val();
                var discountAmount = (discount / 100) * price;
                var taxRate = curentrow.find('#rate').val();
                var quantity = curentrow.find('#quantity').val();
                var totalCost = curentrow.find('#totalCost');
                var perProductCost = curentrow.find('#perProductCost');
                var taxAmount = (taxRate / 100) * price;
                var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
                var perProductCost1 = ((price - discountAmount) + taxAmount);
                console.log("amount", totalAmount)
                totalCost.val(totalAmount);
                perProductCost.val(perProductCost1);
                // final subtotal
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount1 = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;


                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount1 / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            });
            //$('.datatable').on('change', '#tax', function () {
            //    var curentrow = $(this).closest("tr");
            //    var price = curentrow.find('#price').val();
            //    var discount = curentrow.find('#discount').val();
            //    var discountAmount = (discount / 100) * price;
            //    var taxRate = curentrow.find('#rate').val();
            //    var quantity = curentrow.find('#quantity').val();
            //    var totalCost = curentrow.find('#totalCost');
            //    var perProductCost = curentrow.find('#perProductCost');
            //    var taxAmount = (taxRate / 100) * price;
            //    var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
            //    var perProductCost1 = ((price - discountAmount) + taxAmount);
            //    console.log("amount", totalAmount)
            //    totalCost.val(totalAmount);
            //    perProductCost.val(perProductCost1);
            //    // final subtotal
            //    var finalAmount = 0;
            //    var productAmount = 0;
            //    $(".datatable .define").each(function (row, tr) {
            //        var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
            //        if (isNaN(amount)) {
            //            amount = 0;
            //        }
            //        console.log("amount", amount);
            //        productAmount = amount + productAmount;

            //    });
            //    console.log("productamount", productAmount);
            //    var shippingCost = parseFloat($('.shippingcost').val());
            //    var discount1 = parseFloat($('#discountOntotalOrder').val());
            //    if (isNaN(discount)) {
            //        discount = 0;


            //    }
            //    if (isNaN(shippingCost)) {
            //        shippingCost = 0;
            //    }
            //    if (isNaN(productAmount)) {
            //        productAmount = 0
            //    }
            //    finalAmount = productAmount + shippingCost;
            //    var discountAmount = (discount1 / 100) * finalAmount;
            //    finalAmount = finalAmount - discountAmount;
            //    $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            //});

            $('.datatable').on('keyup', '#quantity', function () {
                var curentrow = $(this).closest("tr");
                var price = curentrow.find('#price').val();
                var discount = curentrow.find('#discount').val();
                var discountAmount = (discount / 100) * price;
                var taxRate = curentrow.find('#rate').val();
                var quantity = curentrow.find('#quantity').val();
                var totalCost = curentrow.find('#totalCost');
                var perProductCost = curentrow.find('#perProductCost');
                var taxAmount = (taxRate / 100) * price;
                var totalAmount = ((price - discountAmount) + taxAmount) * quantity;
                var perProductCost1 = ((price - discountAmount) + taxAmount);
                console.log("amount", totalAmount)
                totalCost.val(totalAmount);
                perProductCost.val(perProductCost1);

                // final subtotal
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount1 = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;


                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount1 / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));
            });
            $('.shippingcost').keyup(function () {
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;


                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount / 100) * finalAmount;
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));

                console.log("shippingCost", shippingCost);
                console.log("discount", discount);
                console.log("finalAmount", finalAmount);
            });
            $('#discountOntotalOrder').keyup(function () {
                var finalAmount = 0;
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#totalCost').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;

                });
                console.log("productamount", productAmount);
                var shippingCost = parseFloat($('.shippingcost').val());
                var discount = parseFloat($('#discountOntotalOrder').val());
                if (isNaN(discount)) {
                    discount = 0;
                }
                if (isNaN(shippingCost)) {
                    shippingCost = 0;
                }
                if (isNaN(productAmount)) {
                    productAmount = 0
                }
                finalAmount = productAmount + shippingCost;
                var discountAmount = (discount / 100) * finalAmount;
                console.log("final +dis", finalAmount, " hhh", discountAmount);
                finalAmount = finalAmount - discountAmount;
                $('#subtotalOfThisOrder').val(finalAmount.toFixed(2));

            });

        }
    });

}
jQueryAjaxPostVendorsBillsModal = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                console.log("res", res)
                if (res.isValid) {

                    $('#view-all').html(res.html)
                    $('#large-modal .modal-body').html('');
                    $('#large-modal .modal-title').html('');
                    $('#large-modal').modal('hide');
                    toastr.success("Successfully", "Bills Post");
                }
                else
                    $('#large-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log("res", err)
                toastr.success(" ", "Faild");


            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
jQueryAjaxPostInvoiceModal = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                console.log("res", res)
                if (res.isValid) {

                    $('#view-all').html(res.html)
                    $('#large-modal .modal-body').html('');
                    $('#large-modal .modal-title').html('');
                    $('#large-modal').modal('hide');
                    toastr.success("Successfully", "Invoice Post");
                }
                else
                    $('#large-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log("res", err)
                toastr.success(" ", "Faild");


            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                console.log("res", res)
                if (res.isValid) {

                    $('#view-all').html(res.html)
                    $('#small-modal .modal-body').html('');
                    $('#small-modal .modal-title').html('');
                    $('#small-modal').modal('hide');
                    toastr.success("Successfully", "Save");
                }
                else
                    $('#small-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log("res", err)
                toastr.success(" ", "Faild");


            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
//Large Modal
jQueryAjaxPostLargeModal = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                console.log(res)
                if (res.isValid) {

                    $('#view-all').html(res.html)
                    $('#large-modal .modal-body').html('');
                    $('#large-modal .modal-title').html('');
                    $('#large-modal').modal('hide');
                    toastr.success("Successfully", "Save");
                }
                else
                    $('#large-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err, "errrrrrrrrrr")
                toastr.success(" ", "Faild");


            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
//For small popup end
ShowEmployeeSalaryInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {

            $('#large-modal .modal-body').html(res);
            $('#large-modal .modal-title').html(title);
            $('#large-modal').modal('show');
            var emp = $('.datatable').find("#Employee");
            emp.val('');
            $("#addAnother").click(function (e) {

                var $tableBody = $('.datatable').find("tbody");
                $trfirst = $tableBody.find("tr:last");
                $trNew = $trfirst.clone();
                $trNew.find("td").find(':input').val('');
                //this part add 
                var suffix = $trNew.find(':input:first').attr('name').match(/\d+/);
                console.log(suffix);
                $.each($trNew.find(':input'), function (i, val) {
                    $('#qt').keyup(function () {
                        var res = $('#qt').val() * $('#sum').val();
                        if (res == Number.POSITIVE_INFINITY || res == Number.NEGATIVE_INFINITY || isNaN(res))
                            res = "N/A"; // OR 0
                        $('#result').val(res);
                    });

                    // Replaced Name
                    var oldN = $(this).attr('name');
                    var newN = oldN.replace('[' + suffix + ']', '[' + (parseInt(suffix) + 1) + ']');

                    $(this).attr('name', newN);
                    //Replaced value
                    var type = $(this).attr('type');

                    // If you have another Type then replace with default value
                    $(this).removeClass("input-validation-error");

                });

                $trfirst.after($trNew);
                $trNew.find("td").find('#removeFeildWithValue').removeClass('refreshTR');
                $trNew.find("td").find('#removeFeildWithValue').addClass('removeTR');
                $(this).removeClass("input-validation-error");
            });

            $('.datatable').on('click', '.removeTR', function () {
                var amountTotal = parseFloat($('#subtotalOfThisSalaryBill').val());
                var amountSub = parseFloat($(this).closest('tr').find('#Salarytoal').val());
                console.log("amountSub", amountSub)
                if (!isNaN(amountTotal) && !isNaN(amountSub)) {
                    amountTotal = amountTotal - amountSub;   
                }
                var amountTotal = $('#subtotalOfThisSalaryBill').val(amountTotal);
                $(this).parents('tr').remove();
            });
            $('.datatable').on('click', '.refreshTR', function () {
                $closestclear = $('.refreshTR').closest('tr');
                var amountTotal = parseFloat($('#subtotalOfThisSalaryBill').val());
                var amountSub = parseFloat($(this).closest('tr').find('#Salarytoal').val());
                console.log("amountSub", amountSub)
                if (!isNaN(amountTotal) && !isNaN(amountSub)) {
                    amountTotal = amountTotal - amountSub;
                }
                var amountTotal = $('#subtotalOfThisSalaryBill').val(amountTotal);
                $closestclear.find("td").find(':input').val('');

            });


            $('.datatable').on('change', '#Employee', function () {
                var curentrow = $(this).closest("tr");
                var selectedEmployeeId = curentrow.find("#Employee option:selected").val();
                $.ajax({
                    url: '/EmployeeSalary/GetEmployeeSalary/?id=' + selectedEmployeeId,
                    type: 'GET',
                    dataType: 'JSON',
                    success: function (data) {

                        var colSalaryAmountVal = curentrow.find('#SalaryAmount');
                        console.log("colval", colSalaryAmountVal, "", data)
                        colSalaryAmountVal.val(data);
                        
                        var salary = parseFloat(curentrow.find('#SalaryAmount').val());
                        if (isNaN(salary)) {
                            salary = 0.0;
                        }
                        console.log("amountSalary", salary);
                        //var colBonusAmountValue = 0.0;
                        var bonus = parseFloat(curentrow.find('#Bonus').val());
                        if (isNaN(bonus)) {
                            bonus = 0.0;
                        }
                        var totalSalarybill = salary + bonus;
                        console.log("bonus", bonus);
                        curentrow.find('#Salarytoal').val(totalSalarybill.toFixed(2));
                        console.log("$('#Salarytoal').val", $('#Salarytoal').val());
                        var productAmount = 0;
                        $(".datatable .define").each(function (row, tr) {
                            var amount = parseFloat($(this).closest('tr').find('#Salarytoal').val());
                            if (isNaN(amount)) {
                                amount = 0;
                            }
                            console.log("amount", amount);
                            productAmount = amount + productAmount;
                            console.log("prod total", productAmount);
                            $('#subtotalOfThisSalaryBill').val(productAmount.toFixed(2));
                        });

                       

                    },
                    error: function (res) {
                        console.log(res);
                    }
                });
               

            })
            $('.datatable').on('keyup', '#Bonus', function () {
                var curentrow = $(this).closest("tr");

                var salary = parseFloat(curentrow.find('#SalaryAmount').val());
                if (isNaN(salary)) {
                    salary = 0.0;
                }
                console.log("amountSalary", salary);
                //var colBonusAmountValue = 0.0;
                var bonus = parseFloat(curentrow.find('#Bonus').val());
                if (isNaN(bonus)) {
                    bonus = 0.0;
                }
                var totalSalarybill = salary + bonus;
                console.log("bonus", bonus);
                curentrow.find('#Salarytoal').val(totalSalarybill.toFixed(2));
                console.log("$('#Salarytoal').val", $('#Salarytoal').val());
                var productAmount = 0;
                $(".datatable .define").each(function (row, tr) {
                    var amount = parseFloat($(this).closest('tr').find('#Salarytoal').val());
                    if (isNaN(amount)) {
                        amount = 0;
                    }
                    console.log("amount", amount);
                    productAmount = amount + productAmount;
                    console.log("prod total", productAmount);
                    $('#subtotalOfThisSalaryBill').val(productAmount.toFixed(2));
                });


            })



            //just for this portion i add 2 popup function
        }
    });

}
// crud operation end


jQueryAjaxPostSalaryModal = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                console.log(res)
                if (res.isValid) {

                    $('#view-all').html(res.html)
                    $('#large-modal .modal-body').html('');
                    $('#large-modal .modal-title').html('');
                    $('#large-modal').modal('hide');
                    toastr.success("Successfully", "Save");
                }
                else
                    $('#large-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err, "errrrrrrrrrr")
                toastr.success(" ", "Faild");


            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}


ShowInSalaryBillLargePopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {

            $('#large-modal .modal-body').html(res);
            $('#large-modal .modal-title').html(title);
            $('#large-modal').modal('show');

        }
    });

}