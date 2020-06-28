using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static TileLoader;

public class ListExport : MonoBehaviour
{
    public Texture2D heightmapGS;
    public Texture2D heightmapRGB;
    public Texture2D texture;

    public RawImage heightImg;
    public RawImage texImg;

    public int count = 20;
    public int index = 1;

    public string exportName = "export";

    const string basePath = @"D:\Manuel\Desktop\MASTER_THESIS\Arith";
    string outPath = Path.Combine(basePath, "Out");

    TerrainLoader terrainLoader;

    private bool export = false;

    public void Init()
    {
        terrainLoader = GetComponent<TerrainLoader>();
    }

    void Update()
    {
        if (export)
        {
            index++;
            Export();

            if (index >= count)
            {
                export = false;
            }
        }
    }

    public void ExportList()
    {
        export = true;
        index = 0;
    }

    public void Export()
    {
        TilePath tile = new TilePath(
            Path.Combine(basePath, "arith_" + index + "_gs.png"),
            Path.Combine(basePath, "arith_" + index + "_rgb.png"),
            Path.Combine(basePath, "arith_" + index + "_tex.png")
            );

        if (tile.IsValid())
        {
            ExportTile(tile, exportName);
        }
        else
        {
            Debug.Log("Unable to load tile");
            Debug.Log(tile.heightGS);
            Debug.Log(tile.heightRGB);
            Debug.Log(tile.texture);
        }
    }

    public void ExportTile(TilePath tile, string name)
    {
        heightmapGS = tile.LoadHeightGS();
        heightmapRGB = tile.LoadHeightRGB();
        texture = tile.LoadTexture();
        ExportTile(heightmapGS, heightmapRGB, texture, name);
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
        Debug.Log("Export "+index);
        ScreenCapture.CaptureScreenshot(Path.Combine(outPath, name + "_" + index.ToString("D4") + ".png"));
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
