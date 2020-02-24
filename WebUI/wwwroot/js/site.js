// signalR hub connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/tictactoe")
    .configureLogging(signalR.LogLevel.Error)
    .build();

// restart hub on close
connection.onclose(function () {
    start();
});

// method that is called on any new connection to show active players list
connection.on("PlayerConnectHandle", function (players) {
    // add row for each active player to the players table
    var playerRowTemplate = getTemplate("player-row");
    for (var i = 0; i < players.length; ++i) {
        var player = players[i];
        var items = [{
            playerName: player.name,
            playerId: player.id,
            groupName: player.groupName
        }];

        appendPlayerRow(items, playerRowTemplate);
        if (player.groupName) {
            hidePlayer(player.id);
        }
    }
    
    checkIfAnyPlayers();
});

// method that is called on the first player's connection being established to inform other players
connection.on("PlayerFirstTimeConnectHandle", function (playerId, playerName) {
    // at least one player is active now
    $(".no-players-message").hide();
    var playerRowTemplate = getTemplate("player-row");
    var items = [{
        playerName: playerName,
        playerId: playerId
    }];
    
    appendPlayerRow(items, playerRowTemplate);
});

// method that is called on the last player's connection being disconected to inform other players
connection.on("PlayerDisconnectHandle", function (playerId) {
    removePlayer(playerId);
    checkIfAnyPlayers();
});

// method that is called when player opens game history for the first time in order to sinc other connections of the same user
connection.on("PlayerGamesFirstTimeOpenHandle", function (playerId) {
    var playerRowElement = $("tr[player-id='" + playerId + "']");
    var gamesRowElement = playerRowElement.next();
    var gamesTableElement = gamesRowElement.find(".games-table");

    // get games between two players
    $.get("/api/players/" + playerId
        ).done(function (data) {
            var games = data["games"];
            if (games.length) {
                // there are at least one game
                gamesRowElement.find(".no-games-message").hide();
                var gameRowTemplate = getTemplate("game-row");
                for (var i = 0; i < games.length; ++i) {
                    var game = games[i];
                    var badgeClass = "";
                    var actionHtml = "&nbsp;";
                    // set different colors and actions depending on game results
                    switch (game.result) {
                        case "Active":
                            badgeClass = "badge-info";
                            actionHtml = "<button class='btn btn-outline-success continue-game-btn' type='button'>Continue</button>";
                            break;
                        case "Loss":
                            badgeClass = "badge-danger";
                            actionHtml = "&nbsp;";
                            break;
                        case "Win":
                            badgeClass = "badge-success";
                            actionHtml = "&nbsp;";
                            break;
                        case "Draw":
                            badgeClass = "badge-warning";
                            actionHtml = "&nbsp;";
                            break;
                    }

                    var items = [{
                        id: i + 1,
                        startDate: game.startDate,
                        badgeClass: badgeClass,
                        result: game.result,
                        gameId: game.id,
                        actionHtml: actionHtml
                    }];
                    var appendElement = gamesRowElement.find(".games-table > tbody:last-child");
                    processTemplate(items, gameRowTemplate, appendElement);
                    gamesTableElement.show();
                }
            }

            gamesTableElement.addClass("initialized");
            gamesRowElement.find("div").collapse("show");
        }).fail(function () {
            alert("Your request has failed. Please contact support.");
        });
});

// method that is called after player opens already loaded game history in order to sinc other connections of the same user
connection.on("PlayerGamesOpenHandle", function (playerId) {
    $("tr[player-id='" + playerId + "']").next().find("div").collapse("show");
});

// method that is called after player closes already loaded game history in order to sinc other connections of the same user
connection.on("PlayerGamesCloseHandle", function (playerId) {
    $("tr[player-id='" + playerId + "']").next().find("div").collapse("hide");
});

// method that is called when player tries to start new game in order to sinc other connections of the same user
connection.on("NewGameStartCallerHandle", function (playerId) {
    var startButtonElement = $("tr[player-id='" + playerId + "']").find(".player-play-btn");
    $(".players-table").addClass("disabled");
    startButtonElement.html("<span class='spinner-border spinner-border-sm' role='status' aria-hidden='true'></span> Waiting for player...");
});

// method that is called if result of the starting new game is unsuccessful
connection.on("NewGameFailureHandle", function (playerId, message) {
    if (typeof message !== "undefined") {
        $("#failure-alert").html(message);
        $("#failure-alert").fadeTo(2000, 500).slideUp(500, function () {
            $("#failure-alert").slideUp(500);
            enableNewGameButton(playerId);
        });
    }
    else {
        enableNewGameButton(playerId);
    }
});

// method that is called when one player has requested to play with another
connection.on("NewGameStartReceiverHandle", function (playerId, playerName) {
    // show modal to player in order to ask if he wants to play
    if (!$("#gameRequestModal_" + playerId).length) {
        var gameRequestModalTemplate = getTemplate("gameRequestModal");
        var items = [{
            playerName: playerName,
            playerId: playerId
        }];
        processTemplate(items, gameRequestModalTemplate, $("#modals"));
    }
    
    $("#gameRequestModal_" + playerId).modal("show")
});

// method that is called on decline game request to close modals
connection.on("NewGameDeclineHandle", function (playerId) {
    $("#gameRequestModal_" + playerId).modal("hide");
});

// method that is called to handle UI changes for caller after new game is accepted
connection.on("NewGameAcceptCallerHandle", function (player) {
    $(".modal").each(function (index) {
        $(this).modal("hide");
    });
    hidePlayer(player.id);
    checkIfAnyPlayers();
    $(".players-table").addClass("disabled");
    processTurnInfo(player);
});

// method that is called to handle UI changes for receiver after new game is accepted
connection.on("NewGameAcceptReceiverHandle", function (player) {
    $("tr[player-id='" + player.id + "']").find(".player-play-btn").html("Play");
    hidePlayer(player.id);
    checkIfAnyPlayers();
    $(".players-table").addClass("disabled");
    processTurnInfo(player);
});

// method that is called when player starts the game to inform other players
connection.on("NewGameAcceptOthersHandle", function (playerId) {
    hidePlayer(playerId);

    // no active players left
    checkIfAnyPlayers();
});

// event handler on show player games history button
$(document).on("click", ".player-history-btn", function () {
    var playerRowElement = $(this).closest("tr");
    var gamesRowElement = playerRowElement.next();
    var playerId = playerRowElement.attr("player-id");
    var gamesTableElement = gamesRowElement.find(".games-table");
    // check if games list was already loaded. If so - no need to load, because games list can be altered only after game complition event
    if (gamesTableElement.hasClass("initialized")) {
        if (gamesRowElement.find("div").hasClass("show")) {
            connection.invoke("OnPlayerGamesClose", playerId).catch(err => {
                alert("Your request has failed. Please contact support.");
            });
        }
        else {
            connection.invoke("OnPlayerGamesOpen", playerId).catch(err => {
                alert("Your request has failed. Please contact support.");
            });
        }
    }
    else {
        connection.invoke("OnPlayerGamesFirstTimeOpen", playerId).catch(err => {
            alert("Your request has failed. Please contact support.");
        });
    }
});

// event handler on play new game button
$(document).on("click", ".player-play-btn", function () {
    var playerId = $(this).closest("tr").attr("player-id");
    connection.invoke("OnNewGameStartCaller", playerId).catch(err => {
        alert("Your request has failed. Please contact support.");
    });
    connection.invoke("OnNewGameStartReceiver", playerId).catch(err => {
        alert("Your request has failed. Please contact support.");
    });
});

// event handler on game decline button
$(document).on("click", ".player-declineGame-btn", function () {
    var playerId = $(this).closest("div").attr("player-id");
    connection.invoke("OnNewGameDecline", playerId).catch(err => {
        alert("Your request has failed. Please contact support.");
    });
});

// event handler on game accepted button
$(document).on("click", ".player-acceptGame-btn", function () {
    var playerId = $(this).closest("div").attr("player-id");
    connection.invoke("OnNewGameAccept", playerId).catch(err => {
        alert("Your request has failed. Please contact support.");
    });
});

// event handler on board cell
$(document).on("click", ".board-cell:not(.filled)", function () {
    var tableElement = $(this).closest("table");
    var partsArray = $(this).attr("id").split('-');
    var xCell = partsArray[1];
    var yCell = partsArray[2];
    if (!tableElement.attr("game-id")) {
        // post method to create new game in the database
        $.ajax({
            type: "POST",
            url: "/api/games",
            data: JSON.stringify({ PlayerId: "Test", IsCrossPlayer: true }),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (data) {
            tableElement.attr("game-id", data);
        }).fail(function () {
            alert("Your request has failed. Please contact support.");
        });
    }

    var gameId = tableElement.attr("game-id");
});

// function to start connection. Restart connection after 5 seconds
function start() {
    connection.start().catch(function (err) {
        setTimeout(function () {
            start();
        }, 5000);
    });
}

// function to get splitted template by name
function getTemplate(name) {
    return $("script[data-template='" + name + "']").text().split(/\$\{(.+?)\}/g);
}

// function to fill splitted template
function processTemplate(items, templateItems, elementToAppend) {
    elementToAppend.append(items.map(function (item) {
        return templateItems.map(render(item)).join("");
    }));
}

// function to render template
function render(props) {
    return function (tok, i) {
        return (i % 2) ? props[tok] : tok;
    };
}

// function to appened processed row
function appendPlayerRow(items, playerRowTemplate) {
    var elementToAppend = $(".players-table > tbody:last-child");
    processTemplate(items, playerRowTemplate, elementToAppend);
}

// re-enable new game button after callback
function enableNewGameButton(playerId) {
    // id playerId is not provided - set default value for all buttons
    if (typeof playerId === "undefined") {
        $(".player-play-btn").each(function (index) {
            $(this).html("Play");
        });
    }
    else {
        $("tr[player-id='" + playerId + "']").find(".player-play-btn").html("Play");
    }

    $(".players-table").removeClass("disabled");
}

// hide (but not remove) player and his games rows from players table
function hidePlayer(playerId) {
    var gamesRowElement = $("#player_" + playerId).closest("tr");
    var playerRowElement = gamesRowElement.prev();
    gamesRowElement.addClass("display-none");
    playerRowElement.addClass("display-none");
}

// remove player and his games rows from players table
function removePlayer(playerId) {
    var gamesRowElement = $("#player_" + playerId).closest("tr");
    var playerRowElement = gamesRowElement.prev();
    gamesRowElement.remove();
    playerRowElement.remove();
}

// update turn info for players
function processTurnInfo(player) {
    var infoMessage;
    if (player.isCrossPlayer) {
        // it is opponent turn
        infoMessage = "Your are playing as <strong><i>O</i></strong> against <strong><i>" + player.name + "</i></strong>.";
        $(".wait-turn-message").each(function (index) {
            $(this).removeClass("display-none");
        });
    }
    else {
        // your turn
        infoMessage = "Your are playing as <strong><i>X</i></strong> against <strong><i>" + player.name + "</i></strong>.";
        $(".your-turn-message").removeClass("display-none");
        $(".board-table").removeClass("disabled");
    }

    $(".game-info-message").html(infoMessage).removeClass("display-none");
}

// check if there are still lefat any players. If no - show message
function checkIfAnyPlayers() {
    // no active players left
    if (!$(".players-table tr:not(.display-none)").length) {
        $(".no-players-message").show();
    }
}