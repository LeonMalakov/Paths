using UnityEngine;

namespace Paths
{
    public class Controller : MonoBehaviour
    {
        private const int StartBoardWidth = 10;
        private const int StartBoardHeight = 10;

        [SerializeField] private GameBoard _board;
        [SerializeField] private CameraPlacer _camera;
        [SerializeField] private GameBoardClickHandler _gameBoardClickHandler;
        [SerializeField] private AlgorithmToggle _algorithmToggle;
        [SerializeField] private BoardPanel _boardPanel;

        private void Awake()
        {
            _algorithmToggle.Init(isAStar: true);
            _boardPanel.Init(StartBoardWidth, StartBoardHeight);

            _board.Create(StartBoardWidth, StartBoardHeight);
            _camera.Place(StartBoardWidth, StartBoardHeight);
        }

        private void OnEnable()
        {
            OnAlgorithmChanged(_algorithmToggle.IsAStar);
            _gameBoardClickHandler.TileClicked += OnTileClicked;
            _gameBoardClickHandler.TileAlternateClicked += OnTileAlternateClicked;
            _algorithmToggle.StateChanged += OnAlgorithmChanged;
            _boardPanel.ValuesChanged += OnBoardValuesChanged;
        }

        private void OnDisable()
        {
            _gameBoardClickHandler.TileClicked -= OnTileClicked;
            _gameBoardClickHandler.TileAlternateClicked -= OnTileAlternateClicked;
            _algorithmToggle.StateChanged -= OnAlgorithmChanged;
            _boardPanel.ValuesChanged -= OnBoardValuesChanged;
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

        private void OnAlgorithmChanged(bool isAStar)
        {
            IPathFinder finder;
            if (isAStar) finder = new AStar();
            else finder = new AWave();
            _board.SetPathFinder(finder);
        }

        private void OnBoardValuesChanged(int width, int height)
        {
            _board.Create(width, height);
            _camera.Place(width, height);
        }

        private bool IsStartSelectPressed() => Input.GetKey(KeyCode.LeftShift);
    }
}