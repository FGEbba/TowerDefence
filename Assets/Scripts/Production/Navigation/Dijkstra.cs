using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AI
{
    //TODO: Implement IPathFinder using Dijsktra algorithm.
    public class Dijkstra : IPathFinder
    {

        private List<Vector2Int> _Map;

        class Node
        {
            public Node() {}
            public Node(int steps, Vector2Int position, Node _prevNode)
            {
                stepsTaken = steps;
                currentPos = position;
                previous = _prevNode;
            }

            public int stepsTaken;
            public Vector2Int currentPos;
            public Node previous;


        }


        public Dijkstra(List<Vector2Int> _Accessables)
        {
            _Map = _Accessables;
        }

        public IEnumerable<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
        {
            Node currentNode = new Node();
            currentNode.stepsTaken = 0;
            currentNode.currentPos = start;
            List<Vector2Int> searched = new List<Vector2Int>();
            Queue<Node> searchList = new Queue<Node>();

            bool goalFound = false;

            while (!goalFound)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2Int searchNode = currentNode.currentPos + Tools.DirectionTools.Dirs[i];
                    if (searchNode == goal) { goalFound = true; }
                    if ((_Map.Contains(searchNode) || goalFound) && !searched.Contains(searchNode))
                    {
                        searched.Add(searchNode);
                        Node node = new Node(currentNode.stepsTaken + 1, searchNode, currentNode);
                        searchList.Enqueue(node);
                        if (goalFound) { currentNode = node; }
                    }
                    if (goalFound) { break; }
                }
                if (!goalFound)
                {
                    if (searchList.Count > 0) { currentNode = searchList.Dequeue(); }
                    else { return Enumerable.Empty<Vector2Int>(); }
                }

            }
            Node backwardsWalk = currentNode;
            searched.Clear();

            while (backwardsWalk.currentPos != start)
            {
                searched.Add(backwardsWalk.currentPos);
                if (backwardsWalk.previous != null) { backwardsWalk = backwardsWalk.previous; }
            }

            searched.Add(start);
            searched.Reverse();

            return searched;
        }
    }
}
