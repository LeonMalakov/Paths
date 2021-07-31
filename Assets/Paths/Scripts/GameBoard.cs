using UnityEngine;

namespace Paths
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private GameTile _tilePrefab;

        private int _width, _height;
        private GameTile[] _tiles;
        private Path _path;
        private IPathFinder _pathFinder;

        public GameTile Start { get; private set; }
        public GameTile Finish { get; private set; }

        public void Create(int width, int height)
        {
            if (_tiles != null)
                Clear();

            _width = width;
            _height = height;
            _tiles = new GameTile[width * height];

            for (int x = 0; x < _width; x++)
                for (int z = 0; z < _height; z++)
                    CreateTile(x, z);

            SetStart(GetTile(0, 0));
            SetFinish(GetTile(_width - 1, _height - 1));
        }

        public void Clear()
        {
            foreach (var x in _tiles)
                Destroy(x.gameObject);

            _tiles = null;
            _path = null;
        }

        public GameTile GetTile(int x, int z)
        {
            if (x >= 0 && x < _width && z >= 0 && z < _height)
                return _tiles[x * _height + z];

            return null;
        }

        public void SetStart(GameTile tile)
        {
            if (tile.Type != GameTileType.Finish)
            {
                if (Start != null)
                    Start.SetType(GameTileType.Empty);

                tile.SetType(GameTileType.Start);
                Start = tile;
                CalculatePath();
            }
        }

        public void SetFinish(GameTile tile)
        {
            if (tile.Type != GameTileType.Start)
            {
                if (Finish != null)
                    Finish.SetType(GameTileType.Empty);

                tile.SetType(GameTileType.Finish);
                Finish = tile;
                CalculatePath();
            }
        }

        public void ToggleObstacle(GameTile tile)
        {
            if (tile.Type != GameTileType.Start && tile.Type != GameTileType.Finish )
            {
                tile.SetType(tile.Type == GameTileType.Empty ? GameTileType.Obstacle : GameTileType.Empty);
                CalculatePath();
            }
        }

        public void SetPathFinder(IPathFinder pathFinding)
        {
            _pathFinder = pathFinding;
            CalculatePath();
        }

        private void CreateTile(int x, int z)
        {
            var instance = _tiles[x * _height + z] = Instantiate(_tilePrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
            instance.Init(x, z);

            if (x > 0)
                GameTile.MakeRightLeftNeighbors(instance, _tiles[(x - 1) * _height + z]);

            if (z > 0)
                GameTile.MakeForwardBackNeighbors(instance, _tiles[x * _height + (z - 1)]);
        }

        private void CalculatePath()
        {
            if (Start == null || Finish == null || _pathFinder == null) return;

            bool isPathFounded = _pathFinder.FindPath(Start, Finish, out Path path);

            if (isPathFounded)
                ApplyPath(path);
            else
                ClearPath();
        }


        private void ApplyPath(Path path)
        {
            ClearPath();

            for (int i = 0; i < path.Nodes.Count - 1; i++)
                path.Nodes[i].SetNext(path.Nodes[i + 1]);

            _path = path;
        }

        private void ClearPath()
        {
            if (_path != null)
            {
                foreach (var x in _path.Nodes)
                    x.SetNext(null);

                _path = null;
            }
        }
    }
}