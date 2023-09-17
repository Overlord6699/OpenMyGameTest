using UnityEngine;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator.BFS.MovesContainer
{
    public struct UnitMovementData
    {
        public ChessUnitType Unit { get; private set; }
        public UnitMovementType MoveType { get; private set; }
        public List<Vector2Int> Steps { get; private set; }

        public void Init(ChessUnitType unitType)
        {
            Unit = unitType;
            Steps = new List<Vector2Int>();

            switch (Unit)
            {
                case ChessUnitType.Pon:
                    Steps.AddRange(new List<Vector2Int>() { Vector2Int.up, Vector2Int.down });
                    break;

                case ChessUnitType.Rook:
                    Steps.AddRange(new List<Vector2Int>() { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left });
                    break;

                case ChessUnitType.Bishop:
                    Steps.AddRange(new List<Vector2Int>() { new(1, 1), new(-1, 1), new(1, -1), new(-1, -1) });
                    break;

                case ChessUnitType.King:
                case ChessUnitType.Queen:
                    Steps.AddRange(new List<Vector2Int>() { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left });
                    Steps.AddRange(new List<Vector2Int>() { new(1, 1), new(-1, 1), new(1, -1), new(-1, -1) });
                    break;

                case ChessUnitType.Knight:
                    Steps.AddRange(new List<Vector2Int>() { new(1, 2), new(-1, 2), new(1, -2), new(-1, -2),
                        new(2, 1), new(-2, 1), new(2, -1), new(-2, -1) });
                    break;

                default: break;
            }

            switch (Unit)
            {
                case ChessUnitType.Rook:
                case ChessUnitType.Bishop:
                case ChessUnitType.Queen:
                    MoveType = UnitMovementType.Directional;
                    break;

                default:
                    MoveType = UnitMovementType.Typical;
                    break;
            }

        }

        public List<Vector2Int> GetSteps(Vector2Int from, ChessGrid grid)
        {
            if (MoveType == UnitMovementType.Typical)
            {
                return Steps;
            }
            else return AddSteps(from, grid);
        }

        private List<Vector2Int> AddSteps(Vector2Int from, ChessGrid grid)
        {
            List<Vector2Int> result = new List<Vector2Int>();

            Vector2Int delta, position;

            foreach (Vector2Int step in Steps)
            {
                delta = Vector2Int.zero;
                position = from;

                while (true)
                {
                    delta += step;
                    position += step;

                    if (!grid.IsValidPosition(position))
                        break;

                    result.Add(delta);
                }
            }

            return result;
        }
    }
}