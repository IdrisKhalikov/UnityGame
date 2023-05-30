using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class MapSaver : MonoBehaviour
{
    [SerializeField]
    private Tilemap Tilemap;
    [SerializeField]
    private int LevelId;

    //[ExecuteInEditMode]
    public void SaveMap()
    {
        var level = ScriptableObject.CreateInstance<ScriptableLevel>();
        Tilemap.CompressBounds();
        level.LevelId = LevelId;
        level.Tiles = GetAllTiles(Tilemap).ToList();

        //ScriptableObjectUtility.SaveLevelToFile(level);
    }

    //[ExecuteInEditMode]
    public void LoadMap()
    {
        var level = Resources.Load<ScriptableLevel>($"Levels/{LevelId}");
        if (level == null)
        {
            Debug.Log("Level does not exit!");
            return;
        }

        ClearMap();

        foreach(var savedTile in level.Tiles)
        {
            switch(savedTile.Tile.Type)
            {
                case TileType.Default:
                case TileType.Ground:
                    Tilemap.SetTile(savedTile.Position, savedTile.Tile);
                    break;
                default:
                    throw new ArgumentException("No such tile!");
            }
        }
    }

    public void ClearMap()
    {
        var tilemaps = FindObjectsOfType<Tilemap>();

        foreach (var tilemap in tilemaps)
            tilemap.ClearAllTiles();
    }

    private IEnumerable<SavedTile> GetAllTiles(Tilemap tilemap)
    {
        foreach(var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(position))
                continue;
            var tile = tilemap.GetTile<MapTile>(position);
            yield return new SavedTile() { Position = position, Tile = tile };
        }
    }
}

//#if UNITY_EDITOR
//public static class ScriptableObjectUtility
//{
//    public static void SaveLevelToFile(ScriptableLevel level)
//    {
//        AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/{level.LevelId}.asset");
//        AssetDatabase.SaveAssets();
//        AssetDatabase.Refresh();
//    }
//}

//#endif


//[CustomEditor(typeof(MapSaver)), CanEditMultipleObjects]
//public class MapSaverEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        var saver = (MapSaver)target;

//        if (GUILayout.Button("Save"))
//            saver.SaveMap();

//        if (GUILayout.Button("Load"))
//            saver.LoadMap();

//        if (GUILayout.Button("ClearMap"))
//            saver.ClearMap();
//    }
//}
