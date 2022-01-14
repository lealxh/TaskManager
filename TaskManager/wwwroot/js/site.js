// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets

StartSingle = (id) => {

    $.ajax({
        type: 'GET',
        url: '/Tasks/StartSingle/'+id,
        success: function (res) {
            console.log(res);

            setInterval(function () {
                LoadHTML("/Tasks/TasksPartial", ".simulacion-inner");
            }, 1000)
        }
    })

}

StartAll = () => {

    $.ajax({
        type: 'GET',
        url: '/Tasks/StartAll',
        success: function (res) {
            setInterval(function () {
                LoadHTML("/Tasks/TasksPartial", ".simulacion-inner");
            }, 1000)
        }
    })

}

Cancel = (id) => {

    $.ajax({
        type: 'GET',
        url: '/Tasks/CancelThread/'+id,
        success: function (res) {
            console.log(res)
            LoadHTML("/Tasks/TasksPartial", ".simulacion-inner");
            
        }
        
    })

}
LoadHTML = (url,div) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $(div).html(res);
        }
    })
}

showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
            // to make popup draggable
           /* $('.modal-dialog').draggable({
                handle: ".modal-header"
            });*/
        }
    })
}


ajaxPostback = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    LoadHTML("/Tasks/TasksPartial",".simulacion-inner")
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
// Write your JavaScript code.
