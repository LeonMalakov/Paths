using UnityEngine;

namespace Paths
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private GameTile _tilePrefab;

        private int _width, _height;
        private GameTile[] _tiles;

        public void Create(int width, int height)
        {
            _width = width;
            _height = height;
            _tiles = new GameTile[width * height];

            for (int x = 0; x < _width; x++)
                for (int z = 0; z < _height; z++)
                    CreateTile(x, z);
        }

        private void CreateTile(int x, int z)
        {
            var instance = _tiles[x * _width + z] = Instantiate(_tilePrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
            instance.Init(x, z);
        }

        public GameTile GetTile(Vector3 position)
        {
            int x = Mathf.RoundToInt(position.x);
            int z = Mathf.RoundToInt(position.z);

            if (x >= 0 && x < _width && z >= 0 && z < _height)
                return _tiles[x * _width + z];

            return null;
        }
    }
}