// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var allTests = [];
function showProgress() {
    $("#play").hide();
    $(".running-shuttle-info").show();
}
function fetchAllTests() {
    $.getJSON("/js/fitnessrating_beeptest.json").then(function (res) {
        allTests = res;
    })
}
function totalTimeSeconds(minuteSeconds) {
    var time = minuteSeconds.split(":");
    if (time.length != 2) {
        return 0;
    }
    var minutes = parseInt(time[0]), seconds = parseInt(time[1]);

    return ((minutes * 60) + seconds);
}
function initTimer() {
    function pad10(num) {
        if (num < 10) { return "0" + num; }
        return num;
    }
    $('.circle-wrapper').circleProgress({
        value: 0,
        size: 250,
        thickness: 5,
        fill: "#e9847c"
    })
    setInterval(function () {
        var seconds = $(".next-shuttl span").text();
        var totalSeconds = $(".total-time span").text();
        var nextOrgSeconds = "";
        totalSeconds = totalTimeSeconds(totalSeconds);
        if (seconds == 0) {
            var nextShuttle = allTests.filter(x => totalTimeSeconds(x.StartTime) > totalSeconds)[0];
            var currentShuttles = allTests.filter(x => totalTimeSeconds(x.StartTime) <= totalSeconds);
            var currentShuttle = currentShuttles[currentShuttles.length - 1];
            seconds = totalTimeSeconds(nextShuttle.StartTime) - totalSeconds;
            $(".shuttle-level span").text(currentShuttle.SpeedLevel);
            $(".shuttle-no span").text(currentShuttle.ShuttleNo);
            $(".shuttle-speed span").text(currentShuttle.Speed);
            $(".total-distance span").text(currentShuttle.AccumulatedShuttleDistance);
            nextOrgSeconds = totalTimeSeconds(nextShuttle.StartTime) - totalTimeSeconds(currentShuttle.StartTime);
            $(".next-shuttl").attr("data-total-seconds", nextOrgSeconds);
        }
        seconds--;
        totalSeconds++;
        nextOrgSeconds = $(".next-shuttl").attr("data-total-seconds");
        var perc = ((nextOrgSeconds - seconds) / nextOrgSeconds).toFixed(2);
        $('.circle-wrapper').circleProgress('value', perc);
        totalSeconds = pad10(Math.floor(totalSeconds / 60)) + ":" + pad10(Math.floor(totalSeconds % 60));
        $(".total-time span").text(totalSeconds);
        $(".next-shuttl span").text(seconds);
    }, 1000)
}
function initControls() {
    function initFinalStatus() {
        function getAllShuttles(level) {
            var allShuttles = allTests.filter(x => x.SpeedLevel == level).map(x => x.ShuttleNo);
            allShuttles = allShuttles.filter((x, i) => allShuttles.indexOf(x) === i);
            return allShuttles;
        }
        $(".action-control span.final-status").click(function () {
            var control = $(this).parent();
            var fleetData = control.text();
            var splittedFleet = fleetData.replace("[", "").replace("]", "").split("-");
            var speedLevel = splittedFleet[0].trim();
            var shuttleNo = splittedFleet[1].trim();
            control.empty();
            var allLevels = allTests.map(x => x.SpeedLevel);
            allLevels = allLevels.filter((x, i) => allLevels.indexOf(x) === i);
            var allShuttles = getAllShuttles(speedLevel);
            var $select = $("<select/>");
            for (var i = 0; i < allLevels.length; i++) {
                $select.append($("<option/>").text(allLevels[i]));
            }
            control.append($select.val(speedLevel));
            $select.change(function () {
                var selectVal = $(this).val();
                var allShuttles = getAllShuttles(selectVal);
                $(this).next().empty();
                for (var i = 0; i < allShuttles.length; i++) {
                    $(this).next().append($("<option/>").text(allShuttles[i]));
                }
            });
            var $select1 = $("<select/>");
            for (var i = 0; i < allShuttles.length; i++) {
                $select1.append($("<option/>").text(allShuttles[i]));
            }
            control.append($select1.val(shuttleNo));
            var $button = $("<button/>").attr("type", "button").text("Update");
            $button.click(function () {
                var row = control.closest("tr");
                var currentFleet = {
                    SpeedLevel: parseInt($select.val()),
                    ShuttleNo: parseInt($select1.val())
                }
                var player = {
                    id: parseInt(row.find("th[scope='row']").text()),
                    statuses: [],
                    successFleet: currentFleet
                }
                savePlayer(player, function () {
                    control.closest("td").empty().append($("<span/>").addClass("final-status").append("[" + currentFleet.SpeedLevel + " - " + currentFleet.ShuttleNo + "]"));
                    initFinalStatus();
                });
            })
            control.append($button);
        })
    }
    function savePlayer(player, done) {
        $.ajax({
            contentType: 'application/json',
            data: JSON.stringify(player),
            dataType: 'json',
            success: function (data) {
                done();
            },
            error: function () {
                console.log("Request failed");
            },
            processData: false,
            type: 'POST',
            url: '/api/player/save'
        });
    }
    initFinalStatus();
    $(".action-control span.status").click(function (e) {
        var control = $(this);
        if (control.hasClass("disabled")) {
            return;
        }
        var playerRow = control.closest("tr");
        var totalSeconds = $(".total-time span").text();
        totalSeconds = totalTimeSeconds(totalSeconds);
        var prevShuttles = allTests.filter(x => totalTimeSeconds(x.StartTime) < totalSeconds);
        var prevShuttle = prevShuttles.length > 1 ? prevShuttles[prevShuttles.length - 2] : {};
        prevShuttle = {
            SpeedLevel: prevShuttle.SpeedLevel ? parseInt(prevShuttle.SpeedLevel) : 0,
            ShuttleNo: prevShuttle.ShuttleNo ? parseInt(prevShuttle.ShuttleNo) : 0
        }
        var player = {
            id: parseInt(playerRow.find("th[scope='row']").text()),
            statuses: [control.text()],
            successFleet: prevShuttle
        }
        savePlayer(player, function () {
            control.addClass("disabled");
            if (control.text() == "Stop") {
                control.closest("td").empty().append($("<span/>").addClass("final-status").append("[" + prevShuttle.SpeedLevel + " - " + prevShuttle.ShuttleNo + "]"));
                initFinalStatus();
            }
        })
    })
}
function initApp() {
    fetchAllTests();
    initTimer();
    initControls();
}
if (isStarted) {
    initApp();
}