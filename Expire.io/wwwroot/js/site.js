// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function() {
    $('#ff').submit('click',
        function(e) {

            let form = $(this);
            let data = form.serialize();
            var token = $("input[name^=__RequestVerificationToken]").val();
            data += '&__RequestVerificationToken=' + token;

            $.ajax({
                type: "post",
                url: "/Admin/CreateUser",
                data: data,
                xhrFields: {
                    withCredentials: true
                },
                success: function(resp) {
                    window.alert("User was successfull added");
                    location.reload();
                },
                fail: function(err) {
                    alert(err);
                },
                error: function(jqXHR, exception) {
                    console.log(jqXHR.status + ' ' + exception);
                }
            })

            return false;
        });
})

function deleteUser(user)
{
    $.ajax({
        url: "/Admin/DeleteUser",
        data: {username : user.toString()},
        success: function (resp) {
            console.log(resp.value.resp);
            window.alert(resp.value.resp);
            selector = '#' + resp.value.id;
            var toDelete = $(selector.toString());
            toDelete.remove();
        },
        fail: function (err) {
            alert(err);
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR.status + ' ' + exception);
        }
    })
}

function makeAnAdmin(user) {
    
}