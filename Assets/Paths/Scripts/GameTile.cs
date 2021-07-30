using UnityEngine;

namespace Paths
{
    public class GameTile : MonoBehaviour
    {
        private GameTile _forward, _back, _right, _left;

        public int X { get; private set; }
        public int Z { get; private set; }

        public void Init(int x, int z)
        {
            X = x;
            Z = z;
        }

        public static void MakeForwardBackNeighbors(GameTile forward, GameTile back)
        {
            forward._back = back;
            back._forward = forward;
        }

        public static void MakeRightLeftNeighbors(GameTile right, GameTile left)
        {
            right._left = left;
            left._right = right;
        }
    }
}
