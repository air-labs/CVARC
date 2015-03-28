$(document).ready(function () {
    attachOnUploadFile();
    attachOnFilter();
    attachOnLogin();
    attachOnCloseInfo();
});

function attachOnUploadFile() {
    $("[uploadClient]").each(function (i, element) {
        var jElement = $(element);
        var level = jElement.attr("uploadClient");
        jElement.uploadFile({
            url: "/Replay/UploadFile?level=" + level,
            fileName: "file",
            acceptFiles: ".zip, .rar",
            dragDrop: false,
            onSuccess: function (html) {
                $("body").html(html);
            },
            onError: function () {
                window.location = "/Replay/UploadFailed";
            }
        });
    });
};

function attachOnCloseInfo() {
    $(".close").click(function() {
        $("#loginInfo").hide();
    });
}

function attachOnLogin() {
    var loginFunction = function () {
        var login = $("#login").val();
        var pass = $("#pass").val();
        $.ajax({ url: "/Login/Login?login=" + login + "&password=" + pass }).done(function () {
            window.location = "/Home";
        }).fail(function () {
            $("#incorrectPassMessage").show();
        });
    };

    $("#loginButton").click(loginFunction);
    $("body").keyup(function(event) {
        if (event.keyCode == 13) {
            loginFunction();
        }
    });

    $("#logoutButton").click(function () {
        $.ajax({ url: "/Login/Logout" }).done(function () {
            window.location = "/Home";
        });
    });
}

function attachOnFilter() {
    $("input[filterBy]").each(function (i, element) {
        var jElement = $(element);
        var level = jElement.attr("filterBy");
        var table = $("table[filterBy=" + level + "]");
        //                    var tableWidth = table.width();
        var trs = table.find("tr").not('thead tr');
        jElement.keyup(function () {
            var value = jElement.val();
            trs.each(function (index, elem) {
                var teamName = $(elem).find("[teamName]").text();
                if (teamName.indexOf(value) >= 0)
                    $(elem).show();
                else
                    $(elem).hide();
            });
            //                        table.width(tableWidth);
        });
    });
};