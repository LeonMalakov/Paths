using System.Collections.Generic;
using System.Linq;

namespace Paths
{
    public class AStar : IPathFinder
    {
        private class Node
        {
            public GameTile Tile;
            public Node Parent;
            public int DistanceFromStart;
            public int DistanceToEnd;

            public int Cost => DistanceFromStart + DistanceToEnd;

            public Node(GameTile tile, Node parent, int distanceFromStart, int distanceToEnd)
            {
                Tile = tile;
                Parent = parent;
                DistanceFromStart = distanceFromStart;
                DistanceToEnd = distanceToEnd;
            }
        }

        private List<GameTile> _closed;
        private List<Node> _open;

        public bool FindPath(GameTile from, GameTile to, out Path path)
        {
            _closed = new List<GameTile>();
            _open = new List<Node>
            {
                new Node(from, null, 0, CalculateSqrDistance(from, to))
            };

            var finishNode = Trace(to);

            if (finishNode != null)
            {
                path = BuildPath(finishNode);
                return true;
            }

            path = null;
            return false;
        }

        private Node Trace(GameTile to)
        {
            while (_open.Count > 0)
            {
                var node = ExtractNodeWithMinCost();
                _closed.Add(node.Tile);

                if (node.Tile == to)
                    return node;

                ProccessNeighbors(to, node);
            }

            return null;
        }

        private void ProccessNeighbors(GameTile to, Node parent)
        {
            foreach (var next in parent.Tile.Neighbors)
                ProccessNeighbor(next, parent, to);
        }

        private void ProccessNeighbor(GameTile node, Node parent, GameTile to)
        {
            if (node == null) return;

            if (_closed.Contains(node)) return;

            if (node.IsObstacle)
            {
                _closed.Add(node);
                return;
            }

            var distanceFromStart = parent.DistanceFromStart + 1;
            var nextNode = FindTileNode(node);

            if (nextNode != null)
                UpdateNode(nextNode, parent, distanceFromStart);
            else
                _open.Add(new Node(node, parent, distanceFromStart, CalculateSqrDistance(node, to)));
        }

        private void UpdateNode(Node node, Node parent, int distanceFromStart)
        {
            if (distanceFromStart < node.DistanceFromStart)
            {
                node.DistanceFromStart = distanceFromStart;
                node.Parent = parent;
            }
        }

        private Path BuildPath(Node endNode)
        {
            List<GameTile> pathNodes = new List<GameTile> { endNode.Tile };
            Node current = endNode;
            while (current.Parent != null)
            {
                pathNodes.Add(current.Parent.Tile);
                current = current.Parent;
            }
            pathNodes.Reverse();

            return new Path(pathNodes.ToArray());
        }

        private Node ExtractNodeWithMinCost()
        {
            Node minCostNode = _open[0];

            for (int i = 1; i < _open.Count; i++)
                if (_open[i].Cost < minCostNode.Cost)
                    minCostNode = _open[i];

            _open.Remove(minCostNode);
            return minCostNode;
        }

        private Node FindTileNode(GameTile tile) => _open.FirstOrDefault(x => x.Tile == tile);

        private int CalculateSqrDistance(GameTile from, GameTile to)
        {
            int x = to.X - from.X;
            int z = to.Z - from.Z;
            return x * x + z * z;
        }
    }
}