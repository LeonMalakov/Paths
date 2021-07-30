using UnityEngine;

namespace Paths
{
    [RequireComponent(typeof(GameTile))]
    public class GameTilePresenter : MonoBehaviour
    {  
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private SpriteRenderer _arrow;

        private static readonly Quaternion ForwardRotation = Quaternion.Euler(90, 0, 0);
        private static readonly Quaternion RightRotation = Quaternion.Euler(90, 90, 0);
        private static readonly Quaternion BackRotation = Quaternion.Euler(90, 180, 0);
        private static readonly Quaternion LeftRotation = Quaternion.Euler(90, 270, 0);

        private GameTile _tile;

        private void Awake()
        {
            _tile = GetComponent<GameTile>();
            _arrow.enabled = false;
        }

        private void OnEnable()
        {
            _tile.TypeChanged += OnTypeChanged;
            _tile.NextChanged += OnNextChanged;
        }

        private void OnDisable()
        {
            _tile.TypeChanged -= OnTypeChanged;
            _tile.NextChanged -= OnNextChanged;
        }

        private void OnTypeChanged(GameTileType type)
        {
            _renderer.color = 
                type == GameTileType.Obstacle ? Color.red : 
                type == GameTileType.Start ? Color.blue :
                type == GameTileType.Finish? Color.green :
                Color.white;
        }

        private void OnNextChanged(GameTile tile)
        {
            if (tile != null)
                ApplyArrowRotation(tile);

            _arrow.enabled = tile != null;
        }

        private void ApplyArrowRotation(GameTile to)
        {
            _arrow.transform.rotation =
                to == _tile.Forward ? ForwardRotation :
                to == _tile.Right ? RightRotation :
                to == _tile.Back ? BackRotation :
                LeftRotation;
        }
    }
}
