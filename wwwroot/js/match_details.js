var $detailsContainer = $('#match-details-container');
var $detailsCurtain = $('#match-details-curtain');

$('.trigger-match-details').on('click', function (e) {
    e.preventDefault();
    showDetailedMatchStatistics();
});

$('#close-details').on('click', function (e) {
    e.preventDefault();
    closeDetailedMatchStatistics();
});

var fillDetailedMatchStatistics = function (homeTeamName, homeTeamLogo, homeTeamGoalsScored, awayTeamName, awayTeamLogo, awayTeamGoalsScored, date, homeGoalScorers=null, awayGoalScorers=null) {
    $('.homecomming-team.logo').css('background-image', 'url("' + homeTeamLogo + '")');

    $('.homecomming-team.name').text(homeTeamName);

    $('.homecomming-team.score').text(homeTeamGoalsScored);

    $('.away-team.logo').css('background-image', 'url("' + awayTeamLogo + '")');

    $('.away-team.name').text(awayTeamName);

    $('.away-team.score').text(awayTeamGoalsScored);

    var formattedDate = new Date(date);
    var d = formattedDate.getDate();
    var m = formattedDate.getMonth();
    m += 1;  // JavaScript months are 0-11
    var y = formattedDate.getFullYear();

    $('#date-of-match').text(d + "/" + m + "/" + y);
    //$('#time-of-match').text(time);
   
    var $awayTeamScoreEl = $('.away-team.score');
    var $homeCommingTeamScoreEl = $('.homecomming-team.score');
    var $awayTeamScore = +$awayTeamScoreEl.text();
    var $homeCommingTeamScore = +$homeCommingTeamScoreEl.text();

    if ($awayTeamScore === $homeCommingTeamScore) {
        $($awayTeamScoreEl, $homeCommingTeamScoreEl).addClass('winner');
    } else if ($awayTeamScore > $homeCommingTeamScore) {
        $awayTeamScoreEl.addClass('winner');
        $homeCommingTeamScoreEl.removeClass('winner');
    } else {
        $awayTeamScoreEl.removeClass('winner');
        $homeCommingTeamScoreEl.addClass('winner');
    };

    var homeGoalScorersNode = $("#home-scorers");
    homeGoalScorersNode.empty();
    $.each(homeGoalScorers, function (index, value) {
        homeGoalScorersNode.append(" <span>"+value+"</span><br />");
    });

    var awayGoalScorersNode = $("#away-scorers");
    awayGoalScorersNode.empty();
    console.log(awayGoalScorers);
    $.each(awayGoalScorers, function (index, value) {
        console.log("adding");
        awayGoalScorersNode.append(" <span>" + value + "</span><br />");
    });

};

var showDetailedMatchStatistics = function () {
    $detailsCurtain.css('display', 'flex').animate({ 'opacity': 1 }, 100, function () {
        $detailsContainer.animate({ 'opacity': 1 }, 300);
    });
}

var closeDetailedMatchStatistics = function () {
    $detailsContainer.animate({ 'opacity': 0 }, 300, function () {
        $detailsCurtain.animate({ 'opacity': 0 }, 100, function () {
            $(this).css('display', 'none');
        });
    });
}