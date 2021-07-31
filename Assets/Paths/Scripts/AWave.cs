using System.Collections.Generic;
using System.Linq;

namespace Paths
{
    public class AWave : IPathFinder
    {
        public class Node
        {
            public GameTile Tile;
            public int Cost;

            public Node(GameTile tile, int cost)
            {
                Tile = tile;
                Cost = cost;
            }
        }

        private List<Node> _marked;

        public bool FindPath(GameTile from, GameTile to, out Path path)
        {
            _marked = new List<Node>
            {
                new Node(from, 0)
            };

            Node finishNode =  Wave(to);

            if (finishNode != null)
            {
                path = BuildPath(finishNode);
                return true;
            }

            path = null;
            return false;
        }

        private Node Wave(GameTile to)
        {
            int costCounter = 0;
            Node[] currentStepNodes;
            do
            {
                currentStepNodes = GetNodesWithCost(costCounter++);

                foreach (var node in currentStepNodes)
                {
                    if (node.Tile == to)
                        return node;

                    ProccessNeighbors(node, costCounter);
                }

            } while (currentStepNodes.Count() > 0);

            return null;
        }

        private void ProccessNeighbors(Node node, int costCounter)
        {
            foreach (var next in node.Tile.Neighbors)
                ProccessNeighbor(next, costCounter);
        }

        private void ProccessNeighbor(GameTile node, int costCounter)
        {
            if (node == null) return;

            if (node.IsObstacle) return;

            if (ContainsTileNode(node) == false)
                _marked.Add(new Node(node, costCounter));
        }

        private Path BuildPath(Node finishNode)
        {
            List<GameTile> pathNodes = new List<GameTile>
            {
                finishNode.Tile
            };

            Node current = finishNode;
            while (current.Cost != 0)
            {
                current = GetNeigborNodeWithCost(current);
                pathNodes.Add(current.Tile);
            }

            pathNodes.Reverse();
            return new Path(pathNodes.ToArray());
        }

        private Node[] GetNodesWithCost(int cost) => _marked.Where(x => x.Cost == cost).ToArray();

        private Node GetNeigborNodeWithCost(Node to)
            => _marked.FirstOrDefault(x => x.Cost == to.Cost - 1 && to.Tile.Neighbors.Contains(x.Tile));

        private bool ContainsTileNode(GameTile tile) => _marked.Any(x => x.Tile == tile);
    }
}
