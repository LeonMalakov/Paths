using System;
using UnityEngine;

namespace Paths
{
    [RequireComponent(typeof(GameBoard))]
    public class GameBoardClickHandler : MonoBehaviour
    {
        private GameBoard _board;

        private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

        public event Action<GameTile> TileClicked;
        public event Action<GameTile> TileAlternateClicked;

        private void Awake()
        {
            _board = GetComponent<GameBoard>();
        }

        private void Update()
        {
            if (IsPointerDown())
                HandleClick();
            else if(IsAlternativePointerDown())
                HandleAlternateClick();
        }

        private void HandleClick()
        {
            var tile = FindTile();
            if (tile != null)
                TileClicked?.Invoke(tile);
        }

        private void HandleAlternateClick()
        {
            var tile = FindTile();
            if (tile != null)
                TileAlternateClicked?.Invoke(tile);
        }

        private GameTile FindTile()
        {
            GameTile tile = null;

            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(TouchRay, out var distance))
            {
                var position = TouchRay.GetPoint(distance);
                tile = GetTile(position);
            }

            return tile;
        }

        private GameTile GetTile(Vector3 position)
        {
            int x = Mathf.RoundToInt(position.x);
            int z = Mathf.RoundToInt(position.z);
            var tile = _board.GetTile(x, z);
            return tile;
        }

        private static bool IsPointerDown() => Input.GetMouseButtonDown(0);
        private static bool IsAlternativePointerDown() => Input.GetMouseButtonDown(1);
    }
}