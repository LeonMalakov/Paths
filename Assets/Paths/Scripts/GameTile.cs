using UnityEngine;

namespace Paths
{
    public class GameTile : MonoBehaviour
    {
        public int X { get; private set; }
        public int Z { get; private set; }

        public void Init(int x, int z)
        {
            X = x;
            Z = z;
        }
    }
}
