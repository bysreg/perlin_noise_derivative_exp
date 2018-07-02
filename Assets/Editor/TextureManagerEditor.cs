using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TextureManager))]
public class TextureManagerEditor : Editor
{
    private void PropagateChanges(TextureManager tm)
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < tm.transform.GetChildCount(); i++)
            {
                Transform folder = tm.transform.GetChild(i);
                for(int j=0;j<folder.GetChildCount();++j)
                {
                    TextureCreator go = (TextureCreator)folder.GetChild(j).gameObject.GetComponent<TextureCreator>();

                    go.resolution = tm.resolution;
                    go.frequency = tm.frequency;
                    go.lacunarity = tm.lacunarity;
                    go.persistence = tm.persistence;
                    go.dimensions = tm.dimensions;
                    go.type = tm.type;
                    go.coloring = tm.coloring;
                    go.repeatable_tiles = tm.repeatable_tiles;
                    go.FillTexture();
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();

        TextureManager myScript = (TextureManager)target;

        if (EditorGUI.EndChangeCheck())
        {
            PropagateChanges(myScript);
        }
    }
}