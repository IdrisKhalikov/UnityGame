using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;


namespace BSPTreeGeneration
{
    public class MapGenerator
    {
        private Tilemap tilemap;
        private Tilemap boundingMap;
        private Tilemap obstacleMap;
        private Tile wall;
        private Tile floor;
        private MapVisualizer groundVisualizer;
        private MapVisualizer corridorVisualizer;
        private SimpleVisualizer boundingVisualizer;
        private PrefabVisualizer prefabVisualizer;
        private RandomVisualizer obstacleVisualizer;
        public Vector3Int SpawnPoint;

        public MapGenerator(Tilemap tilemap, Tile wall, Tile floor, Tilemap boundingMap, List<GameObject> prefabs, Tilemap obstacles)
        {
            this.tilemap = tilemap;
            this.boundingMap = boundingMap;
            this.obstacleMap = obstacles;
            this.wall = wall;
            this.floor = null;
            groundVisualizer = new MapVisualizer(TileType.Ground, tilemap);
            corridorVisualizer = new MapVisualizer(TileType.Corridor, tilemap);
            boundingVisualizer = new SimpleVisualizer(boundingMap, TileType.Hole);
            prefabVisualizer = new PrefabVisualizer(prefabs, tilemap);
            obstacleVisualizer = new RandomVisualizer(obstacles, LoadTiles());
        }

        private List<TileBase> LoadTiles()
        {
            return Resources.LoadAll<TileBase>("MapTiles/Folders").ToList();
        }

        public void GenerateMap(int x, int y, int width, int height, int minWidth, int minHeight, int offset, int level)
        {
            tilemap.ClearAllTiles();
            boundingMap.ClearAllTiles();
            obstacleMap.ClearAllTiles();
            var partitions = BSPAlgorithm.GetRooms(new BoundsInt(x, y, 0, width + (10*level), height + (10*level), 0), minHeight, minWidth);
            var rooms = GenerateRoomsWithOffset(partitions, offset);
            var corridors = GenerateCorridors(rooms);
            prefabVisualizer.RenderMap(rooms, level * 10);
            var groundTiles = GetTilesFromRooms(rooms);
            obstacleVisualizer.RenderMap(rooms, 1, 10);
            SpawnPoint = CreateSpawnPosition(rooms);
            corridors.ExceptWith(groundTiles);
            var allTiles = groundTiles.Union(corridors).ToHashSet();
            groundVisualizer.RenderMap(groundTiles, allTiles);
            corridorVisualizer.RenderMap(corridors, allTiles);
            DrawHolesAroundMap(allTiles);
        }

        private HashSet<LevelTile> GenerateCorridors(List<BoundsInt> rooms)
        {
            var roomCenters = rooms.Select(room => Vector2Int.RoundToInt(room.center)).ToList();
            var corridors = new HashSet<LevelTile>();
            var currentCenter = roomCenters[Random.Range(0, roomCenters.Count)];
            while(roomCenters.Count > 0)
            {
                var closest = roomCenters.OrderBy(point => Vector2.Distance(currentCenter, point)).First();
                //corridors.AddRange(BuildPath(currentCenter, closest));
                corridors.UnionWith(BuildPath((Vector3Int)currentCenter, (Vector3Int)closest));
                roomCenters.Remove(currentCenter);
                currentCenter = closest;
            }

            return corridors;
        }

        private HashSet<LevelTile> GetTilesFromRooms(List<BoundsInt> rooms)
        {
            var tiles = new HashSet<LevelTile>();
            foreach (var room in rooms)
                for(var x = 0;x<room.size.x;x++)
                    for (int y = 0; y < room.size.y; y++)
                        tiles.Add(new LevelTile(room.min + new Vector3Int(x, y), TileType.Ground));
            return tiles;
        }

        private IEnumerable<LevelTile> BuildPath(Vector3Int start, Vector3Int end)
        {
            while (start.y != end.y)
            {
                start += start.y < end.y ? Vector3Int.up : Vector3Int.down;
                yield return new LevelTile(start + Vector3Int.left, TileType.Corridor);
                yield return new LevelTile(start + Vector3Int.right, TileType.Corridor);
                yield return new LevelTile(start, TileType.Corridor);
            }

            var corner = start;

            while (start.x != end.x)
            {
                start += start.x < end.x ? Vector3Int.right : Vector3Int.left;
                yield return new LevelTile(start + Vector3Int.up, TileType.Corridor);
                yield return new LevelTile(start + Vector3Int.down, TileType.Corridor);
                yield return new LevelTile(start, TileType.Corridor);
            }

            if (corner != start)
                for (var i = -1; i < 2; i++)
                    for (var j = -1; j < 2; j++)
                        yield return new LevelTile(corner + new Vector3Int(i, j), TileType.Corridor);
        }

        private List<BoundsInt> GenerateRoomsWithOffset(List<BoundsInt> rooms, int offset)
        {
            List<BoundsInt> positions = new List<BoundsInt>();
            foreach (var room in rooms)
            {
                //for (int x = offset; x < room.size.x - offset; x++)
                //{
                //    for (int y = offset; y < room.size.y - offset; y++)
                //    {
                //        Vector2Int position = (Vector2Int)room.min + new Vector2Int(x, y);
                //        positions.Add(position);
                //    }
                //}
                var x = room.x + offset;
                var y = room.y + offset;
                var width = room.size.x - 2 * offset;
                var height = room.size.y - 2 * offset;
                positions.Add(new BoundsInt(new Vector3Int(x, y), new Vector3Int(width, height)));
            }

            return positions;
        }

        private Vector3Int CreateSpawnPosition(List<BoundsInt> rooms)
        {
            var room = rooms[Random.Range(0, rooms.Count)];
            var spawnPoint = room.center;
            return Vector3Int.RoundToInt(tilemap.CellToWorld(Vector3Int.RoundToInt(spawnPoint)));
        }

        private void DrawHolesAroundMap(HashSet<LevelTile> tiles)
        {
            var emptyTiles = new HashSet<LevelTile>();
            foreach(var tile in tiles)
                emptyTiles.UnionWith(GetEmptyAdjacent(tile, tiles));
            boundingVisualizer.RenderMap(emptyTiles);
        }

        private IEnumerable<LevelTile> GetEmptyAdjacent(LevelTile tile, HashSet<LevelTile> tiles)
        {
            return Enumerable.Range(-1, 3)
                    .SelectMany(x => Enumerable.Range(-1, 3)
                        .Select((x, y) => new LevelTile(tile.Position + new Vector3Int(x, y), tile.Type)))
                    .Where(newTile => !tiles.Contains(newTile));
        }
    }

    
}
