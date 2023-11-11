using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crater_texture_generator : MonoBehaviour
{

        public float Min_Crater_Radius = 0.01f;
        public float Max_Crater_Radius = 0.05f;

        public float Min_Crater_Steepness = 1.0f;
        public float Max_Crater_Steepness = 6.0f;

        private Material CraterSurfaceMaterial;
        public int TextureDimension = 5000;
        public int NumberCraters = 100;
        public string SaveLocation = "texture";

        public int Noise_Tile = 2;

        private Texture2D noiseTex;
    
    // Start is called before the first frame update
    void Start()
    {
        CraterSurfaceMaterial = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material.EnableKeyword ("_Bump");
        
        //"giant normal map"
        //"big normal noise"
        noiseTex = (Texture2D)Resources.Load("map 6");

        Texture2D SurfaceTexture = InitTexture();

        //SurfaceTexture = CreateSolidTexture(SurfaceTexture);
        for(int i = 0; i < NumberCraters; i++){
           AddCrater(SurfaceTexture, GenerateRadius(), Random.Range(0f,1f),  Random.Range(0f,1f));
           //AddCrater(SurfaceTexture, 0.45f,0.5f,0.5f);  
        }
        
        SurfaceTexture.Apply();
        CraterSurfaceMaterial.SetTexture("_BumpMap", SurfaceTexture);
        System.IO.File.WriteAllBytes( "Assets/" + SaveLocation + ".png", SurfaceTexture.EncodeToPNG());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCrater(Texture2D texture, float radius, float centerX, float centerY){
        float steepness = Random.Range(Min_Crater_Steepness, Max_Crater_Steepness);

        int xMin = (int)(TextureDimension * (centerX - radius));
        int xMax = (int)(TextureDimension * (centerX + radius));

        int yMin = (int)(TextureDimension * (centerY - radius));
        int yMax = (int)(TextureDimension * (centerY + radius));

        for (int y = yMin; y < yMax; y++){
            for (int x = xMin; x < xMax; x++){
                Vector2 position = new Vector2(
                    (float)x / (float)TextureDimension - centerX, 
                    (float)y / (float)TextureDimension - centerY);
                float distance = position.magnitude;
                if (distance < radius)
                {
                    Vector3 normal = -position.normalized;
                    float slopeFactor = (1f - (Mathf.Cos(Mathf.Pow((distance / radius), steepness)* (2f * 3.1415f))))/2f;
                    //float slopeFactor = Mathf.Sin(Mathf.Pow((distance / radius), steepness)* (2f * 3.1415f));
                    normal.x *= slopeFactor;
                    normal.y *= slopeFactor;

                    Color oldColor = texture.GetPixel(x,y);
                    texture.SetPixel(x, y, new Color(((normal.x * 0.5f + 0.5f) + (oldColor.r * 2.0f))/3.0f, ((normal.y * 0.5f + 0.5f)+ (oldColor.g * 2.0f))/3.0f, 1f));
                }
            }
        }
    }

    public Texture2D InitTexture(){
        Texture2D SurfaceTexture = new Texture2D(TextureDimension, TextureDimension);
        SurfaceTexture.filterMode = FilterMode.Point;
        Color dullColor = new Color(0.5f,0.5f,0.0f);
        for (int y = 0; y < TextureDimension; y++){
            for (int x = 0; x < TextureDimension; x++){
                SurfaceTexture.SetPixel(x, y, noiseTex.GetPixel((x * Noise_Tile) % 2048, (y * Noise_Tile) % 2048));
            }
        }   
        return SurfaceTexture;
    }

    private float GenerateRadius(){
        return Random.Range(Min_Crater_Radius, Max_Crater_Radius) ;//* Mathf.Pow(Random.Range(0f,1f),99f);
    }
/*
     public void CreateGradientTexture()
    {
        Texture2D SurfaceTexture = new Texture2D(TextureDimension, TextureDimension);
        SurfaceTexture.filterMode = FilterMode.Point;

        for (int i = 0; i < TextureDimension; i++) {
            for (int j = 0; j < TextureDimension; j++) {
             SurfaceTexture.SetPixel(j, i, new Color((float)((float)j/(float)TextureDimension),(float)((float)j/(float)TextureDimension),(float)((float)j/(float)TextureDimension), 1.0f));
            }
        }
         SurfaceTexture.Apply();
        //"_MainTex"
        //"_BumpMap"
        CraterSurfaceMaterial.SetTexture("_BumpMap", SurfaceTexture);
        System.IO.File.WriteAllBytes( "Assets/random Map.png", SurfaceTexture.EncodeToPNG());
    }

     public void CreateRandomTexture()
    {
        Texture2D SurfaceTexture = new Texture2D(TextureDimension, TextureDimension);
        SurfaceTexture.filterMode = FilterMode.Point;

        for (int i = 0; i < TextureDimension; i++) {
            for (int j = 0; j < TextureDimension; j++) {
             SurfaceTexture.SetPixel(j, i, new Color(Random.Range(0f,1.0f), Random.Range(0f, 1.0f),1.0f, 1.0f));
            }
        }
         SurfaceTexture.Apply();
        CraterSurfaceMaterial.SetTexture("_BumpMap", SurfaceTexture);
         System.IO.File.WriteAllBytes( "Assets/random Map.png", SurfaceTexture.EncodeToPNG());
    }

    public Texture2D CreateSolidTexture(Texture2D SurfaceTexture)
        {
            for (int i = 0; i < TextureDimension; i++) {
                for (int j = 0; j < TextureDimension; j++) {
                SurfaceTexture.SetPixel(i, j, new Color(0f,0f,1.0f));
                }
            }
            return SurfaceTexture;
        }
        */
}
 