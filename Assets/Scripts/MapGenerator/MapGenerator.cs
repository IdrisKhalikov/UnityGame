using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.VisualScripting;
using System.Threading;

namespace BSPTreeGeneration
{
    public class MapGenerator
    {
        private Tilemap tilemap;
        private Tile wall;
        private Tile floor;

        public MapGenerator(Tilemap tilemap, Tile wall, Tile floor)
        {
            this.tilemap = tilemap;
            this.wall = wall;
            this.floor = null;
        }

        public void GenerateMap(int levelWidth, int levelHeight, int iterations)
        {
            tilemap.ClearAllTiles();
            tilemap.size = new Vector3Int(levelWidth, levelHeight);
            tilemap.ResizeBounds();
            tilemap.SetTile(new Vector3Int(levelWidth + 1, levelHeight + 1), floor);
            var partitions = new BSPAlgorithm(levelWidth, levelHeight).GetMap(iterations);
            foreach(var partition in partitions)
            {
                if(partition.Neighbour != null)
                    DrawCorridor(partition);
            }

            foreach(var partition in partitions)
            {
                if (partition.Iteration == iterations)
                    FillRect(GenerateRoom(partition.Bounds));
            }
        }

        private static Rectangle GenerateRoom(Rectangle rect)
        {
            var newX = rect.X + Random.Range(1, rect.Width / 3);
            var newY = rect.Y + Random.Range(1, rect.Height / 3);
            var width = rect.Width - (newX - rect.X);
            var height = rect.Height - (newY - rect.Y);
            width -= Random.Range(1, width / 3);
            height -= Random.Range(1, height / 3);
            return new Rectangle(newX, newY, width, height);
        }

        private void DrawCorridor(Partition partition)
        {
            var start = GetRectangleCenter(partition.Bounds);
            var end = GetRectangleCenter(partition.Neighbour.Bounds);
            if (start.X == end.X)
                FillRect(new Point(start.X - 2, start.Y - 2), new Point(end.X + 1, end.Y + 2));
            else
                FillRect(new Point(start.X - 2, start.Y - 2), new Point(end.X + 2, end.Y + 1));
        }

        private void DrawRect(Rectangle rect, Tile tile)
        {
            DrawRect(new Point(rect.X, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height), tile);
        }

        private void DrawRect(Point start, Point end, Tile tile)
        {
            for (var x = start.X; x <= end.X; x++)
                for (var y = start.Y; y <= end.Y; y++)
                    tilemap.SetTile(new Vector3Int(x, y), tile);
        }

        private void FillRect(Point start, Point end)
        {
            DrawRect(start, end, wall);
            DrawRect(new Point(start.X + 1, start.Y + 1), new Point(end.X - 1, end.Y - 1), floor);
        }

        private void FillRect(Rectangle rect)
        {
            DrawRect(rect, wall);
            DrawRect(new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2), floor);
        }

        private static Point GetRectangleCenter(Rectangle rect)
            => new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
    }
}
