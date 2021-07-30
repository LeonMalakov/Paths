using System.Collections.Generic;

namespace Paths
{
    public class Path
    {
        private GameTile[] _nodes;

        public IReadOnlyList<GameTile> Nodes => _nodes;

        public Path(GameTile[] nodes) => _nodes = nodes;
    }
}
