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
});
//Jquery end//

// crud operation start

//For small popup start
ShowInSmallPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            console.log("res",res)
            $('#small-modal .modal-body').html(res);
            $('#small-modal .modal-title').html(title);
            $('#small-modal').modal('show');
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
//For small popup end

// crud operation end
