﻿using System.Collections.Generic;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    public class GameTilesVM
    {
        public bool IsOpponentCrossPlayer { get; set; }

        public IList<CrossPlayerTileDTO> CrossPlayerTiles { get; set; }

        public IList<NoughtPlayerTileDTO> NoughtPlayerTiles { get; set; }
    }
}