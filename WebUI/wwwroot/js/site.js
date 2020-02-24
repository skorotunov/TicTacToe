
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
    if (!players.length) {
        // there are no active plaers
        $(".no-players-message").show();
    }
    else {
        // add row for each active player to the players table
        var playerRowTemplate = getTemplate("player-row");
        for (var i = 0; i < players.length; ++i) {
            var player = players[i];
            var items = [{
                playerName: player.name,
                playerId: player.id
            }];

            appendPlayerRow(items, playerRowTemplate);
        }
    }
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
    var gamesRowElement = $("#player_" + playerId).closest("tr");
    var playerRowElement = gamesRowElement.prev();
    gamesRowElement.remove();
    playerRowElement.remove();

    // no active players left
    if (!$(".players-table tr").length) {
        $(".no-players-message").show();
    }
});

// method that is called when player opens game history for the first time in order to sinc other connections of the same user
connection.on("PatientGamesFirstTimeOpenHandle", function (playerId) {
    var playerRowElement = $("tr[player-id='" + playerId + "']");
    var gamesRowElement = playerRowElement.next();
    var gamesTableElement = gamesRowElement.find(".games-table");

    // get games between two players
    $.get("/api/players/" + playerId)
        .done(function (data) {
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
        })
        .fail(function () {
            alert("Your request has failed. Please contact support.");
        });
});

// method that is called after player opens already loaded game history in order to sinc other connections of the same user
connection.on("PatientGamesOpenHandle", function (playerId) {
    $("tr[player-id='" + playerId + "']").next().find("div").collapse("show");
});

// method that is called after player closes already loaded game history in order to sinc other connections of the same user
connection.on("PatientGamesCloseHandle", function (playerId) {
    $("tr[player-id='" + playerId + "']").next().find("div").collapse("hide");
});

// method that is called when player tries to start new game in order to sinc other connections of the same user
connection.on("NewGameStartCallerHandle", function (playerId) {
    var startButtonElement = $("tr[player-id='" + playerId + "']").find(".player-play-btn");
    $(".players-table").addClass("disabled");
    startButtonElement.html("<span class='spinner-border spinner-border-sm' role='status' aria-hidden='true'></span> Waiting for player...");
});

// method that is called if result of the starting new game is unsuccessful
connection.on("NewGameFailureCallerHandle", function (playerId, message) {
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
    if (typeof message !== "undefined") {
        $("#failure-alert").html(message);
        $("#failure-alert").fadeTo(2000, 500).slideUp(500, function () {
            $("#failure-alert").slideUp(500);
        });
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










// event handler on show player games history button
$(document).on("click", ".player-history-btn", function () {
    var playerRowElement = $(this).closest("tr");
    var gamesRowElement = playerRowElement.next();
    var playerId = playerRowElement.attr("player-id");
    var gamesTableElement = gamesRowElement.find(".games-table");
    // check if games list was already loaded. If so - no need to load, because games list can be altered only after game complition event
    if (gamesTableElement.hasClass("initialized")) {
        if (gamesRowElement.find("div").hasClass("show")) {
            connection.invoke("OnPatientGamesClose", playerId).catch(err => {
                alert("Your request has failed. Please contact support.");
            });
        }
        else {
            connection.invoke("OnPatientGamesOpen", playerId).catch(err => {
                alert("Your request has failed. Please contact support.");
            });
        }
    }
    else {
        connection.invoke("OnPatientGamesFirstTimeOpen", playerId).catch(err => {
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
    alert("game with" + $(this).closest("div").attr("player-id") + " accepted");
    // TODO signalr
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

//function to fill splitted template
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

function appendPlayerRow(items, playerRowTemplate) {
    var elementToAppend = $(".players-table > tbody:last-child");
    processTemplate(items, playerRowTemplate, elementToAppend);
}