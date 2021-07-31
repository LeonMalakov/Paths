using UnityEngine;

namespace Paths
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private GameBoard _board;
        [SerializeField] private Transform _camera;
        [SerializeField] private GameBoardClickHandler _gameBoardClickHandler;

        private void Start()
        {
            _board.Create(10, 10);
            //_board.SetPathFinder(new AStar());
            _board.SetPathFinder(new AWave());
            _camera.position = new Vector3(5, 10, 5);
            _camera.rotation = Quaternion.Euler(90, 0, 0);
        }

        private void OnEnable()
        {
            _gameBoardClickHandler.TileClicked += OnTileClicked;
            _gameBoardClickHandler.TileAlternateClicked += OnTileAlternateClicked;
        }

        private void OnDisable()
        {
            _gameBoardClickHandler.TileClicked -= OnTileClicked;
            _gameBoardClickHandler.TileAlternateClicked -= OnTileAlternateClicked;
        }

        private void OnTileClicked(GameTile tile)
        {
            _board.ToggleObstacle(tile);
        }

        private void OnTileAlternateClicked(GameTile tile)
        {
            if (IsStartSelectPressed())
                _board.SetStart(tile);
            else
                _board.SetFinish(tile);
        }

        private bool IsStartSelectPressed() => Input.GetKey(KeyCode.LeftShift);
    }
}