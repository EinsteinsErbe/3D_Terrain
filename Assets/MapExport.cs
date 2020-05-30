using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static TileLoader;

public class MapExport : MonoBehaviour
{
    public Texture2D heightmapGS;
    public Texture2D heightmapRGB;
    public Texture2D texture;

    public RawImage heightImg;
    public RawImage texImg;

    public int count = 20;
    public int index = 1;

    public bool maps = true;

    public string exportName = "export";

    const string basePath = @"D:\Manuel\Desktop\MASTER_THESIS\Survey";
    string generatedPath = Path.Combine(basePath, "Generated");
    string tilePath = Path.Combine(basePath, "Tile");
    string mapPath = Path.Combine(basePath, "Map");

    TerrainLoader terrainLoader;

    public void Init()
    {
        terrainLoader = GetComponent<TerrainLoader>();
    }

    public void ExportReal()
    {
        TilePath tile = maps ? GetRandomTile(10, true) : GetRandomTile(12, false);

        Debug.Log(tile.texture);

        if (tile.IsValid())
        {
            ExportTile(tile, "real");
        }
        else
        {
            Debug.Log("Unable to load tile");
        }
    }

    public void ExportGenerated()
    {
        string prefix = maps ? "map" : "tile";

        TilePath tile = new TilePath(
            Path.Combine(generatedPath, prefix + "_generated_" + index + "_gs.png"),
            Path.Combine(generatedPath, prefix + "_generated_" + index + "_rgb.png"),
            Path.Combine(generatedPath, prefix + "_generated_" + index + "_tex.png")
            );

        if (tile.IsValid())
        {
            ExportTile(tile, "generated");
        }
        else
        {
            Debug.Log("Unable to load tile");
            Debug.Log(tile.heightGS);
            Debug.Log(tile.heightRGB);
            Debug.Log(tile.texture);
        }
    }

    public void ExportTile()
    {
        ExportTile(exportName);
    }

    public void ExportTile(string name)
    {
        ExportTile(heightmapGS, heightmapRGB, texture, name);
    }

    public void ExportTile(TilePath tile, string name)
    {
        heightmapGS = tile.LoadHeightGS();
        heightmapRGB = tile.LoadHeightRGB();
        texture = tile.LoadTexture();
        ExportTile(name);
    }

    public void ExportTile(Texture2D gs, Texture2D rgb, Texture2D sat, string name)
    {
        terrainLoader.heightmap = rgb;
        terrainLoader.texture = sat;
        terrainLoader.rgbMap = true;

        terrainLoader.Init();

        heightImg.texture = gs;
        texImg.texture = sat;

        //RenderTexture tex = new RenderTexture(1024, 1024, 24);
        //Camera.main.targetTexture = tex;
        Camera.main.Render();

        ScreenCapture.CaptureScreenshot(Path.Combine(maps ? mapPath : tilePath, name + "_" + index + ".png"));
        /*
        RenderTexture.active = tex;
        Texture2D virtualPhoto = new Texture2D(1024, 1024, TextureFormat.RGB24, false);
        
        virtualPhoto.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);

        RenderTexture.active = null;
        Camera.main.targetTexture = null;
        // consider ... Destroy(tempRT);

        byte[] bytes;
        bytes = virtualPhoto.EncodeToPNG();

        System.IO.File.WriteAllBytes(@"D:\Manuel\Desktop\MASTER_THESIS\DATA\export.png", bytes);
        // virtualCam.SetActive(false); ... no great need for this.
        */
    }
}
