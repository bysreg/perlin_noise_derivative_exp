using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour {

    [Range(2, 512)]
    public int resolution = 256;

    public float frequency = 1f;

    [Range(1, 8)]
    public int octaves = 1;

    [Range(1f, 4f)]
    public float lacunarity = 2f;

    [Range(0f, 1f)]
    public float persistence = 0.5f;

    [Range(2, 3)]
    public int dimensions = 3;

    public int repeatable_tiles = 1;

    public NoiseMethodType type;

    public Gradient coloring;
}
