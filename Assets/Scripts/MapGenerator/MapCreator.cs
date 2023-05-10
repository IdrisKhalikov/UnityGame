using BSPTreeGeneration;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreator : MonoBehaviour
{
    public Tilemap Tilemap;
    public Tile Wall;
    public Tile Floor;
    [SerializeField]
    public MapVisualizer visualizer;
    public int X;
    public int Y;
    public int MapWidth;
    public int MapHeight;
    public int MinWidth;
    public int MinHeight;
    public int Offset;

    [ExecuteInEditMode]
    public void CreateMap()
    {
        var generator = new MapGenerator(Tilemap, Wall, Floor, visualizer);
        generator.GenerateMap(X, Y, MapWidth, MapHeight, MinWidth, MinHeight, Offset);
    }
}

[CustomEditor(typeof(MapCreator)), CanEditMultipleObjects]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var creator = (MapCreator)target;

        if (GUILayout.Button("Generate"))
        {
            creator.CreateMap();
        }
    }
}
