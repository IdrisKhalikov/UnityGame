using System.Collections.Generic;
using UnityEngine;

namespace BSPTreeGeneration
{
    public static class BSPAlgorithm
    {
        public static List<BoundsInt> GetRooms(BoundsInt dungeonLocation, int minHeight, int minWidth)
        {
            var rooms = new List<BoundsInt>();
            var stack = new Stack<BoundsInt>();
            stack.Push(dungeonLocation);
            while (stack.Count > 0)
            {
                var room = stack.Pop();
                if (room.size.x < minWidth || room.size.y < minHeight)
                    continue;
                if (Random.Range(0, 2) == 0)
                {
                    if (!SplitHorizontally(room, minHeight, stack) && !SplitVertically(room, minWidth, stack))
                        rooms.Add(room);
                }
                else
                {
                    if (!SplitVertically(room, minWidth, stack) && !SplitHorizontally(room, minHeight, stack))
                        rooms.Add(room);
                }


            }
            return rooms;
        }

        private static bool SplitHorizontally(BoundsInt room, int minHeight, Stack<BoundsInt> rooms)
        {
            if (room.size.y < minHeight * 2)
                return false;
            var ySplit = Random.Range(1, room.size.y);
            rooms.Push(new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z)));
            rooms.Push(new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
                new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z)));
            return true;
        }

        private static bool SplitVertically(BoundsInt room, int minWidth, Stack<BoundsInt> rooms)
        {
            if (room.size.x < minWidth * 2)
                return false;
            var xSplit = Random.Range(1, room.size.x);
            rooms.Push(new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z)));
            rooms.Push(new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
                new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z)));
            return true;
        }
    }
}