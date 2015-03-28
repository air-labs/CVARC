$(document).ready(function () {
    attachOnUploadFile();
    attachOnFilter();
    attachOnLogin();
    attachOnCloseInfo();
    setMinSize();
});

function setMinSize() {
    var teamNameCol = $(".teamName");
    teamNameCol.css("min-width", teamNameCol.width());
    var pointsCol = $(".points");
    pointsCol.css("min-width", pointsCol.width());
};

function attachOnUploadFile() {
    $("input").change(function () {
        $(this).parent().submit();
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
        var trs = table.find("tr").not('thead tr');
        jElement.keyup(function () {
            var value = jElement.val();
            trs.each(function (index, elem) {
                var teamName = $(elem).find("[teamName]").text();
                if (teamName.toLowerCase().indexOf(value.toLowerCase()) >= 0)
                    $(elem).show();
                else
                    $(elem).hide();
            });
        });
    });
};