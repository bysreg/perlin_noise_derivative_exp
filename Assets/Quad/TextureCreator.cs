using UnityEngine;

public class TextureCreator : MonoBehaviour {

	[Range(2, 512)]
	public int resolution = 256;

	public float frequency = 1f;

	[Range(1, 8)]
	public int octaves = 1;

	[Range(1f, 4f)]
	public float lacunarity = 2f;

	[Range(0f, 1f)]
	public float persistence = 0.5f;

	[Range(1, 3)]
	public int dimensions = 3;

	public NoiseMethodType type;

	public Gradient coloring;

	private Texture2D texture;
	
	private void OnEnable () {
		if (texture == null) {
			texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);
			texture.name = "Procedural Texture";
			//texture.wrapMode = TextureWrapMode.Clamp;
			texture.wrapMode = TextureWrapMode.Clamp;
            //texture.filterMode = FilterMode.Trilinear;
            texture.filterMode = FilterMode.Point;
            //texture.anisoLevel = 9;
            GetComponent<MeshRenderer>().material.mainTexture = texture;
		}
		FillTexture();
	}

	private void Update () {
		if (transform.hasChanged) {
			transform.hasChanged = false;
			FillTexture();
		}
	}
	
    private float ConvertToTileableValue(Vector3 point, float w, float h)
    {
        NoiseMethod method = Noise.methods[(int)type][dimensions - 1];
        float f_x_y     = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence).value;
        float f_x_w_y   = Noise.Sum(method, point + new Vector3(-w, 0, 0), frequency, octaves, lacunarity, persistence).value;
        float f_x_w_y_h = Noise.Sum(method, point + new Vector3(-w, -h, 0), frequency, octaves, lacunarity, persistence).value;
        float f_x_y_h   = Noise.Sum(method, point + new Vector3(0, -h, 0), frequency, octaves, lacunarity, persistence).value;

        float x = point.x;
        float y = point.y;

        return (
            f_x_y * (w - x) * (h - y) +
            f_x_w_y * (x) * (h - y) +
            f_x_w_y_h * (x) * (y) +
            f_x_y_h * (w - x) * (y)
        ) / (w * h);
    }

	public void FillTexture () {
		if (texture.width != resolution) {
			texture.Resize(resolution, resolution);
		}
		
		Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f,-0.5f));
		Vector3 point10 = transform.TransformPoint(new Vector3( 0.5f,-0.5f));
		Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
		Vector3 point11 = transform.TransformPoint(new Vector3( 0.5f, 0.5f));

		NoiseMethod method = Noise.methods[(int)type][dimensions - 1];
		float stepSize = 1f / resolution;
		for (int y = 0; y < resolution; y++) {
			Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
			for (int x = 0; x < resolution; x++) {
				Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence).value;

                //float sample = ConvertToTileableValue(point, 1, 1);


                if (type != NoiseMethodType.Value)
                {
                    sample = sample * 0.5f + 0.5f;
                }
                texture.SetPixel(x, y, coloring.Evaluate(sample));
            }
		}
		texture.Apply();

        //Debug.Log(Noise.Sum(method, new Vector3(0.5f, 0.99f, 0), frequency, octaves, lacunarity, persistence).value);
	}
}