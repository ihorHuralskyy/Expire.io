// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $('#ff').submit('click',
        function (e) {

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

                    myModel.on('hidden.bs.modal',
                        function () {
                            location.reload();
                        })
                },
                fail: function (err) {
                    alert(err);
                },
                error: function (jqXHR, exception) {
                    console.log(jqXHR.status + ' ' + exception);
                }
            })

            return false;
        }
    );

})



function deleteUser(user) {
    $.ajax({
        url: "/Admin/DeleteUser",
        data: { username: user.toString() },
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
            for (i = 0; i < b.length; ++i) {
                b[i].style.display = "none";
            }


            $(selector.toString()).find(".dropdown")[0].style.display = "none";
            $(selector.toString()).find(".giveToManager")[0].style.display = "none";

        },
        fail: function (err) {
            alert(err);
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR.status + ' ' + exception);
        }
    })
}
function makeAnManager(user) {
    $.ajax({
        url: "/Admin/MakeUserAnManager",
        data: { username: user.toString() },
        success: function (resp) {

            let myModel = $("#myModal");
            myModel.find(".modal-title")[0].textContent = "Add role";
            myModel.find(".modal-body").find("p")[0].textContent = resp.value.resp;

            $("#myModal").modal('show');


            selector = '#' + resp.value.id;
            var a = $(selector.toString()).find("p");
            a[1].textContent = "Manager";
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
function filterElement(element) {

    let button1 = $("[doc_type='license']");
    let button2 = $("[doc_type='insurance']");
    let button3 = $("[doc_type='sertificate']");

    if (element.className == "btn btn-outline-secondary") {
        element.className = "btn btn-secondary";
        element.blur();
        if (element.getAttribute('doc_type') == 'license') {
            button2[0].className = "btn btn-outline-secondary";
            button3[0].className = "btn btn-outline-secondary";
            shuffleInstance.filter(function (el) {
                return el.getAttribute('data-group') == 'license';
            });
        }
        else if (element.getAttribute('doc_type') == 'insurance') {
            button1[0].className = "btn btn-outline-secondary";
            button3[0].className = "btn btn-outline-secondary";
            shuffleInstance.filter(function (el) {
                return el.getAttribute('data-group') == 'insurance';
            });
        }
        else if (element.getAttribute('doc_type') == 'sertificate') {
            button2[0].className = "btn btn-outline-secondary";
            button1[0].className = "btn btn-outline-secondary";
            shuffleInstance.filter(function (el) {
                return el.getAttribute('data-group') == 'sertificate';
            });
        }
    } else {
        element.className = "btn btn-outline-secondary";
        shuffleInstance.filter();
        element.blur();
    }


}

function getModalForCreateDocument() {
    $.ajax({
        type: "get",
        url: "/Document/CreateDocument",
        success: function (resp) {
            $('#dialogContent').html(resp);
            $('#modDialog').modal('show');
        },
        fail: function (err) {
            alert(err);
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR.status + ' ' + exception);
        }
    })
}
function getModalForGiveToManager(userId) {
    $.ajax({
        type: "get",
        url: "/Admin/GetManagerList",
        success: function (resp) {
            $('#dialogContetntForGiveToManager').html(resp);
            $('#modalForGiveToManager').modal('show');
        },
        fail: function (err) {
            alert(err);
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR.status + ' ' + exception);
        }
    })

    window.localStorage.setItem('userId', userId);
}
function giveToManager(managerUserName) {
    let userId = window.localStorage.getItem('userId');
    clearFromStorage();
    $.ajax({
        type: "get",
        url: "/Admin/GiveToManager",
        data: {
            managerName: managerUserName,
            userId:userId
        },
        success: function (resp) {
            $('#modalForGiveToManager').modal('hide');
            let myModel = $("#myModal");
            myModel.find(".modal-title")[0].textContent = "Add to manager";
            myModel.find(".modal-body").find("p")[0].textContent = resp.resp;
            $("#myModal").modal('show');
        },
        fail: function (err) {
            alert(err);
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR.status + ' ' + exception);
        }
    })

}

function clearFromStorage() {
    window.localStorage.removeItem("userId");
}


function createForm() {
    $("#docForm").submit('click', function() {
        let photo = $('#file')[0].files[0];
        let data = new FormData(this);
        //let data = $('#docForm').serialize();
        var token = $("input[name^=__RequestVerificationToken]").val();
        data.append('&__RequestVerificationToken=' + token);


        $.ajax({
            type: "post",
            url: "/Document/CreateDocument",
            data: data,
            success: function (resp) {
                console.log(resp);
                $("#docCreationResult h3")[0].innerText = resp.data;
                $("#docCreationResult h3")[0].style.color = resp.color;
                $("#docCreationResult").show();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(XMLHttpRequest);
            }
            //statusCode: {
            //    400: function (resp) {
            //        console.log(resp);
            //        $("#docCreationResult h3")[0].innerText = resp.value.data;
            //        $("#docCreationResult h3")[0].style.color = resp.value.color;
            //        $("#docCreationResult").show();
            //    }
            //}

        })
        return false;

    })
}