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
                success: function (resp) {
                    let myModel = $("#myModal");
                    myModel.find(".modal-title")[0].textContent = "Create user";
                    myModel.find(".modal-body").find("p")[0].textContent = "User was created succsesfully";

                    $("#myModal").modal('show');

                    myModel.on('hidden.bs.modal', function() {
                        location.reload();
                    })
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
            
            let myModel = $("#myModal");
            myModel.find(".modal-title")[0].textContent = "Delere user";
            myModel.find(".modal-body").find("p")[0].textContent = resp.value.resp;

            $("#myModal").modal('show');

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
function aaa() {
    let myModel = $("#myModal");
    myModel.find(".modal-title")[0].textContent = "Add role";
    myModel.find(".modal-body").find("p")[0].textContent = resp.value.resp;

    $("#myModal").modal('show');
}
function makeAnAdmin(user) {
    $.ajax({
        url: "/Admin/MakeUserAnAdmin",
        data: { username: user.toString() },
        success: function (resp) {

            let myModel = $("#myModal");
            myModel.find(".modal-title")[0].textContent = "Add role";
            myModel.find(".modal-body").find("p")[0].textContent = resp.value.resp;

            $("#myModal").modal('show');


            selector = '#' + resp.value.id;
            var a = $(selector.toString()).find("p");
            a[1].textContent = "Admin";
            var b = $(selector.toString()).find(".adminButton");
            b[0].style.display = "none";
        },
        fail: function (err) {
            alert(err);
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR.status + ' ' + exception);
        }
    })
}