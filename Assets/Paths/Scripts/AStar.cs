using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Paths
{
    public class AStar : IPathFinder
    {
        private class Node
        {
            public GameTile Tile;
            public Node Parent;
            public float DistanceFromStart;
            public float DistanceToEnd;

            public float Cost => DistanceFromStart + DistanceToEnd;

            public Node(GameTile tile, Node parent, float distanceFromStart, float distanceToEnd)
            {
                Tile = tile;
                Parent = parent;
                DistanceFromStart = distanceFromStart;
                DistanceToEnd = distanceToEnd;
            }
        }

        public bool FindPath(GameTile from, GameTile to, out Path path)
        {
            var closed = new List<GameTile>();
            var open = new List<Node>
            {
                new Node(from, null, 0, CalculateSqrDistance(from, to))
            };

            while (open.Count > 0)
            {
                var node = GetNodeWithMinCost(open);
                open.Remove(node);
                closed.Add(node.Tile);

                foreach (var next in node.Tile.Neighbors)
                {
                    if (next == null) continue;

                    if (closed.Contains(next)) continue;

                    if (next.IsObstacle)
                    {
                        closed.Add(next);
                        continue;
                    }

                    if (next == to)
                    {
                        path = BuildPath(node, to);
                        return true;
                    }

                    var distanceFromStart = node.DistanceFromStart + 1;
                    var nextNode = FindTileNode(next, open);

                    if (nextNode != null)
                        UpdateNode(nextNode, node, distanceFromStart);
                    else
                        open.Add(new Node(next, node, distanceFromStart, CalculateSqrDistance(from, to)));
                }
            }

            path = null;
            return false;
        }

        private Node FindTileNode(GameTile tile, List<Node> list) => list.FirstOrDefault(x => x.Tile == tile);

        private void UpdateNode(Node node, Node parent, float distanceFromStart)
        {
            if (distanceFromStart < node.DistanceFromStart)
            {
                node.DistanceFromStart = distanceFromStart;
                node.Parent = parent;
            }
        }

        private Path BuildPath(Node endNode, GameTile finish)
        {
            List<GameTile> pathNodes = new List<GameTile> { finish, endNode.Tile };
            Node current = endNode;
            while (current.Parent != null)
            {
                pathNodes.Add(current.Parent.Tile);
                current = current.Parent;
            }
            pathNodes.Reverse();

            return new Path(pathNodes.ToArray());
        }

        private Node GetNodeWithMinCost(List<Node> list)
        {
            Node minCostNode = list[0];

            for (int i = 1; i < list.Count; i++)
                if (list[i].Cost < minCostNode.Cost)
                    minCostNode = list[i];

            return minCostNode;
        }

        private float CalculateSqrDistance(GameTile from, GameTile to) => Mathf.Pow(to.X - from.X, 2) + Mathf.Pow(to.Z - from.Z, 2);
    }
}