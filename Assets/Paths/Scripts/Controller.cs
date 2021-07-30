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
            _camera.position = new Vector3(5, 10, 5);
            _camera.rotation = Quaternion.Euler(90, 0, 0);
        }

        private void OnEnable()
        {
            _gameBoardClickHandler.TileClicked += OnTileClicked;
        }

        private void OnDisable()
        {
            _gameBoardClickHandler.TileClicked -= OnTileClicked;
        }

        private void OnTileClicked(GameTile tile)
        {
            Debug.Log($"Click: {tile.X}, {tile.Z}");
        }
    }
}