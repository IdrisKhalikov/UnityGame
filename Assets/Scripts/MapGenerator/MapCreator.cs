using BSPTreeGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class MapCreator : MonoBehaviour
{
    public Tilemap Tilemap;
    public Tile Wall;
    public Tile Floor;
    public int MapWidth;
    public int MapHeight;
    public int SplitIterations;

    [ExecuteInEditMode]
    public void CreateMap()
    {
        var generator = new MapGenerator(Tilemap, Wall, Floor);
        generator.GenerateMap(MapWidth, MapHeight, SplitIterations);
    }
}

[CustomEditor(typeof(MapCreator)), CanEditMultipleObjects]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //Reference to our script
        var creator = (MapCreator)target;

        if (GUILayout.Button("Generate"))
        {
            creator.CreateMap();
        }
    }
}
