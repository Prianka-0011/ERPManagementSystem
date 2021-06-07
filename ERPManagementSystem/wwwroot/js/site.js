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
    document.getElementById('displayonEditPri').style.display = "none";
    document.getElementById('displayonEdit').style.display = "none";
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
            childDiv.setAttribute("class", "col-md-2 col-3 col-lg-2 removeclass"+i);
            gallery.setAttribute("id", "galleryimg");
            gallery.setAttribute("class", "minclass"+i);
            
            gallery.src = URL.createObjectURL(event.target.files[i]);
            gallery.onload = function () {
                URL.revokeObjectURL(gallery.src) // free memory
            }
            childDiv.appendChild(gallery);
            parent.appendChild(childDiv);

            var StoredDiv = $('.removeclass'+i).html();
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
//document.getElementById('removeFeildWithValue').style.display = "none";
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
        }
    });

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

// crud operation end
//cascading input

function subcategoryList() {
    var selectedCategoryId = $(".category option:selected").val();
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
}
