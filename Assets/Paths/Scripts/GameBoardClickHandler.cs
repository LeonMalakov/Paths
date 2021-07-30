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

        private void Awake()
        {
            _board = GetComponent<GameBoard>();
        }

        private void Update()
        {
            if (IsPointerDown())
                HandleClick();
        }

        private void HandleClick()
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(TouchRay, out var distance))
            {
                var position = TouchRay.GetPoint(distance);
                var tile = _board.GetTile(position);
                if (tile != null)
                    TileClicked?.Invoke(tile);
            }
        }

        private static bool IsPointerDown() => Input.GetMouseButtonDown(0);
    }
}