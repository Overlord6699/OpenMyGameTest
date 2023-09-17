using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator.BFS;
using App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator.BFS.MovesContainer;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            BFSNavigator BFS = new(grid.Size);

            UnitMovementData unitMoves = new();
            unitMoves.Init(unit);

            return BFS.ScanPath(from, to, grid, unitMoves);
        }

    }
}