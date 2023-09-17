using UnityEngine;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator.BFS.MovesContainer;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator.BFS
{
    public class BFSNavigator
    {
        private Vector2Int[][] _prevMatrix;

        private Vector2Int RESET_VALUE = -Vector2Int.one;

        public BFSNavigator(Vector2Int size)
        {
            InitMatrix(size);
        }

        private void InitMatrix(Vector2Int size)
        {
            _prevMatrix = new Vector2Int[size.x][];

            for (int i = 0; i < size.x; i++) _prevMatrix[i] = new Vector2Int[size.y];

            for (int i = 0; i < size.x; i++)
                for (int j = 0; j < size.y; j++)
                    _prevMatrix[i][j] = RESET_VALUE;
        }

        public List<Vector2Int> ScanPath(Vector2Int from, Vector2Int to, ChessGrid grid, UnitMovementData unit)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>(10);

            SetPreviousPos(from, from);

            queue.Enqueue(from);

            while (queue.Count > 0)
            {
                from = queue.Dequeue();

                List<Vector2Int> steps = unit.GetSteps(from, grid);

                foreach (Vector2Int step in steps)
                {
                    Vector2Int position = from + step;

                    if (!(grid.IsValidPosition(position) && IsNotVisited(position))) continue;

                    SetPreviousPos(position, from);

                    if (position.Equals(to))
                    {
                        return CreatePath(position);
                    }
                    else 
                        queue.Enqueue(position);
                }
            }

            return null;
        }

        private List<Vector2Int> CreatePath(Vector2Int from)
        {
            List<Vector2Int> path = new List<Vector2Int>(10);

            do
            {
                path.Add(from);
                from = GetPreviousPos(from);
            }
            while (GetPreviousPos(from) != from);

            path.Reverse();

            return path;
        }

        private Vector2Int GetPreviousPos(Vector2Int pos)
        {
            return _prevMatrix[pos.y][pos.x];
        }

        private void SetPreviousPos(Vector2Int pos, Vector2Int prev)
        {
            _prevMatrix[pos.y][pos.x] = prev;
        }

        private bool IsNotVisited(Vector2Int pos)
        {
            return GetPreviousPos(pos) == RESET_VALUE;
        }
    }
}