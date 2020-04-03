using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AI
{
    //TODO: Implement IPathFinder using Dijsktra algorithm.
    public class Dijkstra : IPathFinder
    {

        private List<Vector3> _Map;

        class Node
        {
            public Node() {}
            public Node(int steps, Vector3 position, Node _prevNode)
            {
                stepsTaken = steps;
                currentPos = position;
                previous = _prevNode;
            }

            public int stepsTaken;
            public Vector3 currentPos;
            public Node previous;


        }


        public Dijkstra(List<Vector3> _Accessables)
        {
            _Map = _Accessables;
        }

        public IEnumerable<Vector3> FindPath(Vector3 start, Vector3 goal)
        {
            Node currentNode = new Node();
            currentNode.stepsTaken = 0;
            currentNode.currentPos = start;
            List<Vector3> searched = new List<Vector3>();
            Queue<Node> searchList = new Queue<Node>();

            bool goalFound = false;

            while (!goalFound)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector3 searchNode = currentNode.currentPos + Tools.DirectionTools.Dirs[i];
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
                    else { return Enumerable.Empty<Vector3>(); }
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
