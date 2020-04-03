using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public interface IPathFinder
    {
        IEnumerable<Vector3> FindPath(Vector3 start, Vector3 goal);
    }
}