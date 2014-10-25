$(document).ready(function () {
    $('[wantPlayReplay=true]').click(function () {
        var replayName = $(this).text();
        window.location = "/Home?name=" + replayName;
    });
});

function onload() {
    var replayName = location.search.split('name=')[1];
    if (replayName) {
        var div = document.getElementById('scene');
        showReplay(div, "Home/GetReplay?name=" + replayName);
    }
}