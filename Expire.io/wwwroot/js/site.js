// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function() {
    $('#ff').submit('click',
        function(e) {

            let from = $(this);

            $.ajax({
                type: "post",
                url: "/Admin/CreateUser",
                data: from.serialize(),
                success: function() {
                    window.alert("User was successfull added");
                },
                fail: function(err) {
                    alert(err);
                }
            })
        });
})