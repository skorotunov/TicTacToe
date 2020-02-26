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
            playerId: player.id
        }];

        appendPlayerRow(items, playerRowTemplate);
    }
    
    checkIfAnyPlayers();
});

// method that is called on any new connection to connect to the game
connection.on("PlayerInGameConnectHandle", function (opponent) {
    connectToExistingGame(opponent);
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

// method that is called on the last player's connection being disconected from the game with another player
connection.on("PlayerInGameDisconnectHandle", function () {
    gameFinish();
    var alertElement = $("#failure-alert");
    alertElement.html("We are sorry. Your opponent has just left the game. You can continue it later.");
    alertElement.fadeTo(2000, 500).slideUp(500, function () {
        alertElement.slideUp(500);
    });
});

// method that is called when player opens game history for the first time in order to sinc other connections of the same user
connection.on("PlayerGamesFirstTimeOpenHandle", function (playerId) {
    var playerRowElement = $("tr[player-id='" + playerId + "']");
    var gamesRowElement = playerRowElement.next();
    var gamesTableElement = gamesRowElement.find(".games-table");

    // get games between two players
    $.get("/api/players/" + playerId).done(function (data) {
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

            // check if one gfme should be marked as pending
            var pendingGameId = gamesTableElement.attr("pending-game-id");
            if (pendingGameId) {
                var buttonElement = gamesRowElement.find(".game-continue[game-id='" + pendingGameId + "']").find(".continue-game-btn");
                buttonElement.html("<span class='spinner-border spinner-border-sm' role='status' aria-hidden='true'></span> Waiting for player...");
                gamesTableElement.attr("pending-game-id", "");
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
connection.on("NewGameStartCallerHandle", function (playerId, gameId) {
    if (gameId) {
        var buttonElement = $("tr[player-id='" + playerId + "']").next().find(".game-continue[game-id='" + gameId + "']").find(".continue-game-btn");
        if (buttonElement.length) {
            // regular case, game table is loaded
            buttonElement.html("<span class='spinner-border spinner-border-sm' role='status' aria-hidden='true'></span> Waiting for player...");
        }
        else {
            // new connection is opened when player is waiting for response
            $("tr[player-id='" + playerId + "']").next().find(".games-table").attr("pending-game-id", gameId);
        }
    }
    else {
        var buttonElement = $("tr[player-id='" + playerId + "']").find(".player-play-btn");
        buttonElement.html("<span class='spinner-border spinner-border-sm' role='status' aria-hidden='true'></span> Waiting for player...");
    }
    
    $(".players-table").addClass("disabled");
    
});

// method that is called if result of the starting new game is unsuccessful
connection.on("NewGameFailureHandle", function (playerId, message) {
    enableGameButtons(playerId);
    if (typeof message !== "undefined") {
        $("#failure-alert").html(message);
        $("#failure-alert").fadeTo(2000, 500).slideUp(500, function () {
            $("#failure-alert").slideUp(500);
        });
    }
});

// method that is called when one player has requested to play with another
connection.on("NewGameStartReceiverHandle", function (playerId, playerName, gameId, gameDate) {
    // show modal to player in order to ask if he wants to play
    if (!$("#gameRequestModal_" + playerId).length) {
        var gameRequestModalTemplate = getTemplate("gameRequestModal");
        var items = [{
            playerId: playerId,
            gameId: gameId
        }];
        processTemplate(items, gameRequestModalTemplate, $("#modals"));
    }

    // show different messages for new game and existing one
    if (gameId) {
        $("#gameRequestModal_" + playerId).find(".modal-footer").attr("game-id", gameId);
        $("#gameRequestModal_" + playerId).find(".modal-body").html("Player " + playerName + " has just challenged you to continue previously started on " + gameDate + " Tic-Tac-Toe game. Will you accept this challenge?");
    }
    else {
        $("#gameRequestModal_" + playerId).find(".modal-footer").attr("game-id", "");
        $("#gameRequestModal_" + playerId).find(".modal-body").html("Player " + playerName + " has just challenged you to play Tic-Tac-Toe game. Will you accept this challenge?");
    }
    
    $("#gameRequestModal_" + playerId).modal("show")
});

// method that is called on decline game request to close modals
connection.on("NewGameDeclineHandle", function (playerId) {
    $("#gameRequestModal_" + playerId).modal("hide");
});

// method that is called to handle UI changes for caller after new game is accepted
connection.on("NewGameAcceptCallerHandle", function (opponent) {
    $(".modal").each(function (index) {
        $(this).modal("hide");
    });
    // check if we want to continue existing game or create new
    if (opponent.gameId) {
        connectToExistingGame(opponent);
    }
    else {
        processTurnInfo(opponent);
    }

    prepareGameElements(opponent);
});

// method that is called to handle UI changes for receiver after new game is accepted
connection.on("NewGameAcceptReceiverHandle", function (opponent) {
    // check if we want to continue existing game or create new
    if (opponent.gameId) {
        connectToExistingGame(opponent);
    }
    else {
        processTurnInfo(opponent);
    }

    prepareGameElements(opponent);
});

// method that is called when player starts the game to inform other players
connection.on("NewGameAcceptOthersHandle", function (playerId) {
    removePlayer(playerId);

    // no active players left
    checkIfAnyPlayers();
});

// method that is called when new game is created and it's id is sent to other connections in the group
connection.on("NewGameCreateHandle", function (gameId) {
    $(".boardContainer").attr("game-id", gameId);
});

// method that is called for receiver after one player made his turn
connection.on("TurnCompleteReceiverHandle", function (x, y) {
    var cellElement = $("#pos-" + x + "-" + y);
    var isOpponentCrossPlayer = $.parseJSON($(".boardContainer").attr("is-opponent-cross-player").toLowerCase());
    fillCell(cellElement, !isOpponentCrossPlayer);
    $(".your-turn-message").removeClass("display-none");
    $(".wait-turn-message").each(function (index) {
        $(this).addClass("display-none");
    });
    $(".board-table").removeClass("disabled");
});

// method that is called for caller after one player made his turn
connection.on("TurnCompleteCallerHandle", function (x, y) {
    var cellElement = $("#pos-" + x + "-" + y);
    var isOpponentCrossPlayer = $.parseJSON($(".boardContainer").attr("is-opponent-cross-player").toLowerCase());
    fillCell(cellElement, isOpponentCrossPlayer);

    $(".your-turn-message").addClass("display-none");
    $(".board-table").addClass("disabled");
    $(".wait-turn-message").each(function (index) {
        $(this).removeClass("display-none");
    });
});

// method that is called after the game is finished
connection.on("GameEndHandle", function (result) {
    var alertElement;
    var isOpponentCrossPlayer = $.parseJSON($(".boardContainer").attr("is-opponent-cross-player").toLowerCase());
    if ((result === "Win" && !isOpponentCrossPlayer) || (result === "Loss" && isOpponentCrossPlayer)) {
        alertElement = $("#success-alert");
        alertElement.html("Congratulations! You won the game.");
    }
    else if ((result === "Loss" && !isOpponentCrossPlayer) || (result === "Win" && isOpponentCrossPlayer)) {
        alertElement = $("#failure-alert");
        alertElement.html("Sorry! You lose the game. Try again later.");
    }
    else {
        alertElement = $("#warning-alert");
        alertElement.html("Game has ended in a draw. Try again later.");
    }

    gameFinish();
    alertElement.fadeTo(2000, 500).slideUp(500, function () {
        alertElement.slideUp(500);
    });
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
    connection.invoke("OnNewGameStartCaller", playerId, "").catch(err => {
        alert("Your request has failed. Please contact support.");
    });
    connection.invoke("OnNewGameStartReceiver", playerId, "", "").catch(err => {
        alert("Your request has failed. Please contact support.");
    });
});

// event handler on continue non-finished game button
$(document).on("click", ".continue-game-btn", function () {
    var playerId = $(this).closest(".player-games").closest("tr").prev().attr("player-id");
    var gameId = $(this).closest("td").attr("game-id");
    var gameDate = $(this).closest("tr").find(".game-date").text();
    connection.invoke("OnNewGameStartCaller", playerId, gameId).catch(err => {
        alert("Your request has failed. Please contact support.");
    });
    connection.invoke("OnNewGameStartReceiver", playerId, gameId, gameDate).catch(err => {
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
    var gameId = parseInt($(this).closest("div").attr("game-id"), 10);
    connection.invoke("OnNewGameAccept", playerId, gameId).catch(err => {
        alert("Your request has failed. Please contact support.");
    });
});

// event handler on board cell
$(document).on("click", ".board-cell:not(.filled)", function () {
    var cellElement = $(this);
    var boardElement = $(".boardContainer");
    var partsArray = $(this).attr("id").split('-');
    var gameId = boardElement.attr("game-id");
    var xCell = parseInt(partsArray[1], 10);
    var yCell = parseInt(partsArray[2], 10);
    
    var isOpponentCrossPlayer = $.parseJSON(boardElement.attr("is-opponent-cross-player").toLowerCase());
    var opponentId = boardElement.attr("opponent-id");
    var url;
    if (isOpponentCrossPlayer) {
        url = "/api/noughtplayergametiles";
    }
    else {
        url = "/api/crossplayergametiles";
    }

    // game needs to be created
    if (!gameId) {
        // post method to create new game in the database
        $.ajax({
            type: "POST",
            url: "/api/games",
            data: JSON.stringify({
                opponentId: opponentId,
                isOpponentCrossPlayer: isOpponentCrossPlayer
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (data) {
            boardElement.attr("game-id", data);
            connection.invoke("OnNewGameCreate", data, opponentId).catch(err => {
                alert("Your request has failed. Please contact support.");
            });

            insertMove(cellElement, data, xCell, yCell, isOpponentCrossPlayer, opponentId, url);
        }).fail(function () {
            alert("Your request has failed. Please contact support.");
        });
    }
    else {
        insertMove(cellElement, parseInt(gameId, 10), xCell, yCell, isOpponentCrossPlayer, opponentId, url);
    }
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
function enableGameButtons(playerId) {
    // id playerId is not provided - set default value for all buttons
    if (typeof playerId === "undefined") {
        $(".player-play-btn").each(function (index) {
            $(this).html("Play");
        });
    }
    else {
        $("tr[player-id='" + playerId + "']").find(".player-play-btn").html("Play");
    }

    $(".continue-game-btn").each(function (index) {
        $(this).html("Continue");
    });

    $(".players-table").removeClass("disabled");
}

// remove player and his games rows from players table
function removePlayer(playerId) {
    var gamesRowElement = $("#player_" + playerId).closest("tr");
    var playerRowElement = gamesRowElement.prev();
    gamesRowElement.remove();
    playerRowElement.remove();
}

// update turn info for players
function processTurnInfo(opponent) {
    var infoMessage;
    var boardElement = $(".boardContainer");
    boardElement.attr("opponent-id", opponent.id);
    boardElement.attr("is-opponent-cross-player", opponent.isCrossPlayer);
    if (opponent.isCrossPlayer) {
        // it is opponent turn
        infoMessage = "Your are playing as <strong><i>O</i></strong> against <strong><i>" + opponent.name + "</i></strong>.";
        $(".wait-turn-message").each(function (index) {
            $(this).removeClass("display-none");
        });
    }
    else {
        // your turn
        infoMessage = "Your are playing as <strong><i>X</i></strong> against <strong><i>" + opponent.name + "</i></strong>.";
        $(".your-turn-message").removeClass("display-none");
        $(".board-table").removeClass("disabled");
    }

    $(".game-info-message").html(infoMessage).removeClass("display-none");
}

// check if there are still lefat any players. If no - show message
function checkIfAnyPlayers() {
    // no active players left
    if (!$(".players-table tr").length) {
        $(".no-players-message").show();
    }
}

// fill cell with X or O
function fillCell(cellElement, isOpponentCrossPlayer) {
    cellElement.addClass("filled");
    if (isOpponentCrossPlayer) {
        cellElement.text("O");
    }
    else {
        cellElement.text("X");
    }
}

// insert player's move to the databse
function insertMove(cellElement, gameId, xCell, yCell, isOpponentCrossPlayer, opponentId, url) {
    // post method to create new move in the database
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify({
            gameId: gameId,
            x: xCell,
            y: yCell
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        // turn has ended
        fillCell(cellElement, isOpponentCrossPlayer);
        $(".your-turn-message").addClass("display-none");
        $(".board-table").addClass("disabled");
        $(".wait-turn-message").each(function (index) {
            $(this).removeClass("display-none");
        });

        connection.invoke("OnTurnComplete", gameId, opponentId, xCell, yCell).catch(err => {
            alert("Your request has failed. Please contact support.");
        });
    }).fail(function () {
        alert("Your request has failed. Please contact support.");
    });
}

// clean up fields after game was finished
function gameFinish() {
    var boardElement = $(".boardContainer");
    boardElement.attr("game-id", "");
    boardElement.attr("is-opponent-cross-player", "");
    boardElement.attr("opponent-id", "");

    $(".game-info-message").addClass("display-none");
    $(".your-turn-message").addClass("display-none");
    $(".wait-turn-message").each(function (index) {
        $(this).addClass("display-none");
    });
    $(".board-cell").each(function (index) {
        $(this).html("");
        $(this).removeClass("filled");
    });
    $(".board-table").addClass("disabled");
    $(".players-table").removeClass("disabled");
}

// prepare UI to display game data
function prepareGameElements(opponent) {
    removePlayer(opponent.id);
    checkIfAnyPlayers();
    $(".player-games").each(function (index) {
        $(this).collapse("hide");
    });
    $(".players-table").addClass("disabled");
}

// connect to existing game (for continue game and addinf new connection while others in the game)
function connectToExistingGame(opponent) {
    // set necessary opponent data
    var boardElement = $(".boardContainer");
    if (opponent.gameId !== 0) {
        boardElement.attr("game-id", opponent.gameId);
    }

    boardElement.attr("opponent-id", opponent.id);

    // get game moves
    $.get("/api/games/" + opponent.gameId)
        .done(function (data) {
            var isOpponentCrossPlayer = data.isOpponentCrossPlayer;
            boardElement.attr("is-opponent-cross-player", isOpponentCrossPlayer);
            for (var i = 0; i < data.crossPlayerTiles.length; ++i) {
                var cellElement = $("#pos-" + data.crossPlayerTiles[i].tile.x + "-" + data.crossPlayerTiles[i].tile.y);
                cellElement.addClass("filled");
                cellElement.text("X");
            }

            for (var j = 0; j < data.noughtPlayerTiles.length; ++j) {
                var cellElement = $("#pos-" + data.noughtPlayerTiles[j].tile.x + "-" + data.noughtPlayerTiles[j].tile.y);
                cellElement.addClass("filled");
                cellElement.text("O");
            }

            if ((i === j && isOpponentCrossPlayer) || (i !== j && !isOpponentCrossPlayer)) {
                // it's opponent turn
                $(".wait-turn-message").each(function (index) {
                    $(this).removeClass("display-none");
                });
            }
            else {
                // it's your turn

                $(".your-turn-message").removeClass("display-none");
                $(".board-table").removeClass("disabled");
            }

            var infoMessage;
            if (isOpponentCrossPlayer) {
                infoMessage = "Your are playing as <strong><i>O</i></strong> against <strong><i>" + opponent.name + "</i></strong>.";

            }
            else {
                infoMessage = "Your are playing as <strong><i>X</i></strong> against <strong><i>" + opponent.name + "</i></strong>.";
            }

            $(".players-table").addClass("disabled");
            $(".game-info-message").html(infoMessage).removeClass("display-none");
        }).fail(function () {
            alert("Your request has failed. Please contact support.");
        });
}