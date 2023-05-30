using BSPTreeGeneration;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreator : MonoBehaviour
{
    public Tilemap Tilemap;
    public Tilemap Holes;
    public Tilemap Folders;
    public Tile Wall;
    public Tile Floor;
    [SerializeField]
    public List<GameObject> Prefabs;
    public int X;
    public int Y;
    public int MapWidth;
    public int MapHeight;
    public int MinWidth;
    public int MinHeight;
    public int Offset;

    //[ExecuteAlways]
    public Vector3Int CreateMap(int level)
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            DestroyImmediate(enemy);
        }

        var generator = new MapGenerator(Tilemap, Wall, Floor, Holes, Prefabs, Folders);
        generator.GenerateMap(X, Y, MapWidth, MapHeight, MinWidth, MinHeight, Offset, level);
        return generator.SpawnPoint;
    }
}

//[CustomEditor(typeof(MapCreator)), CanEditMultipleObjects]
//public class LevelGeneratorEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        var creator = (MapCreator)target;

//        if (GUILayout.Button("Generate"))
//        {
//            creator.CreateMap(0);
//        }
//    }
//}
