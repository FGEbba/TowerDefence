
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static class DirectionTools
    {
        //Sorry going to rework this to Vector 3 instead!! :D
        //Based on how I'm spawning the map, so forward is in this case the x axis and sideways is the z axis
        public static readonly Vector3[] Dirs = { new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 0) };

        public static readonly Dictionary<Direction, Vector3> DirectionToVector2Int =
        new Dictionary<Direction, Vector3>
        {
            { Direction.Up,    new Vector3(1, 0, 0)     },
            { Direction.Right, new Vector3(0, 0, -1)    },
            { Direction.Down,  new Vector3(-1, 0, 0)    },
            { Direction.Left,  new Vector3(0, 0, 1)     }
        };

        public static readonly Dictionary<Vector3, Direction> Vector2IntToDirection =
        new Dictionary<Vector3, Direction>
        {
            { new Vector3(1, 0, 0),     Direction.Up    },
            { new Vector3(0, 0, -1),    Direction.Right },
            { new Vector3(-1, 0, 0),    Direction.Down  },
            { new Vector3(0, 0, 1),     Direction.Left  }
        };

        public static bool InBounds(int topX, int topY, int x, int y)
        {
            return x >= 0 && x < topX && y >= 0 && y < topY;
        }

        public static bool InBounds(Vector3 bounds, Vector3 current)
        {
            return InBounds((int)bounds.x, (int)bounds.z, (int)current.x, (int)current.z);
        }
    }
}