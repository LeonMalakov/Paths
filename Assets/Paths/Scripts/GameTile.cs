using System;
using System.Collections.Generic;
using UnityEngine;

namespace Paths
{
    public class GameTile : MonoBehaviour
    {
        private GameTile[] _neighbors = new GameTile[4];

        public int X { get; private set; }
        public int Z { get; private set; }
        public GameTileType Type { get; private set; }
        public bool IsObstacle => Type == GameTileType.Obstacle;
        public GameTile Forward => _neighbors[0];
        public GameTile Right => _neighbors[1];
        public GameTile Back => _neighbors[2];
        public GameTile Left => _neighbors[3];
        public IReadOnlyList<GameTile> Neighbors => new GameTile[] { Forward, Right, Back, Left };

        public event Action<GameTileType> TypeChanged;
        public event Action<GameTile> NextChanged;

        public void Init(int x, int z)
        {
            X = x;
            Z = z;
        }

        public void SetType(GameTileType type)
        {
            Type = type;
            TypeChanged?.Invoke(Type);
        }

        public void SetNext(GameTile tile) => NextChanged?.Invoke(tile);

        public static void MakeForwardBackNeighbors(GameTile forward, GameTile back)
        {
            forward._neighbors[2] = back;
            back._neighbors[0] = forward;
        }

        public static void MakeRightLeftNeighbors(GameTile right, GameTile left)
        {
            right._neighbors[3] = left;
            left._neighbors[1] = right;
        }
    }
}
