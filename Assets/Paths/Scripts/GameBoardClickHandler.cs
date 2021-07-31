using UnityEngine;

namespace Paths
{
    [RequireComponent(typeof(GameBoard))]
    public class GameBoardClickHandler : MonoBehaviour
    {
        public delegate void ClickedEventHandler(GameTile tile, bool isAlternate);

        private GameBoard _board;

        private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

        public event ClickedEventHandler TileClicked;

        private void Awake()
        {
            _board = GetComponent<GameBoard>();
        }

        private void Update()
        {
            bool isAlternateDown = IsAlternativePointerDown();

            if (isAlternateDown || IsPointerDown())
                HandleClick(isAlternateDown);
        }

        private void HandleClick(bool isAlternate)
        {
            var tile = FindClickedTile();
            if (tile != null)
                TileClicked?.Invoke(tile, isAlternate);
        }

        private GameTile FindClickedTile()
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