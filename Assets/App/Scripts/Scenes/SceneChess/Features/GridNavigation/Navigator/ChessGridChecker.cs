using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using UnityEngine;

public static class ChessGridChecker 
{

    public static bool IsValidPosition(this ChessGrid grid, Vector2Int pos)
    {
        return (pos.x >= 0) && (pos.y >= 0) && (pos.x < grid.Size.y) &&
            (pos.y < grid.Size.x) && (grid.Get(pos) == null);
    }
}
