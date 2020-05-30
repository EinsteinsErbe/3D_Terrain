using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileLoader : MonoBehaviour
{
    public int zoom = 12;
    public int x = 2138;
    public int y = 1452;

    public bool scale1024 = false;

    public const string DATA_DIR = @"D:\Manuel\Desktop\MASTER_THESIS\DATA\TILES";
    public const string FILE_HEIGHT_RGB = "height_rgb.png";
    public const string FILE_HEIGHT_RGB_1024 = "height_rgb_1024.png";
    public const string FILE_HEIGHT_GS = "height_gs.png";
    public const string FILE_HEIGHT_GS_1024 = "height_gs_1024.png";
    public const string FILE_SAT_MAPTILER_512 = "sat_3_512.jpg";
    public const string FILE_SAT_MAPTILER_1024 = "sat_3_1024.jpg";

    TerrainLoader terrainLoader;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        terrainLoader = GetComponent<TerrainLoader>();
    }

    public void LoadTile()
    {
        LoadTile(x, y, zoom);
    }

    public void LoadTile(int x, int y, int z)
    {
        LoadTile(x.ToString(), y.ToString(), z.ToString());
    }

    public void LoadTile(string x, string y, string z)
    {
        string path = Path.Combine(DATA_DIR, z, x, y);
        if (Directory.Exists(path))
        {
            string rgb = Path.Combine(path, scale1024 ? FILE_HEIGHT_RGB_1024 : FILE_HEIGHT_RGB);
            string sat = Path.Combine(path, scale1024 ? FILE_SAT_MAPTILER_1024 : FILE_SAT_MAPTILER_512);

            LoadTile(new TilePath(rgb, rgb, sat));
        }
    }

    private void LoadTile(TilePath tile)
    {
        if (tile.IsValid())
        {
            terrainLoader.heightmap = tile.LoadHeightRGB();
            terrainLoader.texture = tile.LoadTexture();
            terrainLoader.rgbMap = true;
            terrainLoader.Init();
        }
        else
        {
            Debug.Log("Unable to load tile");
            Debug.Log(tile.heightGS);
            Debug.Log(tile.heightRGB);
            Debug.Log(tile.texture);
        }
    }

    public void LoadRandomTile()
    {
        TilePath tile = GetRandomTile(zoom, scale1024);

        if (tile.IsValid())
        {
            LoadTile(tile);
        }
        else
        {
            Debug.Log("Unable to load tile");
        }
    }

    public static TilePath GetRandomTile(int zoom, bool scale1024)
    {
        string path = Path.Combine(DATA_DIR, zoom.ToString());
        if (Directory.Exists(path))
        {
            string[] xs = Directory.GetDirectories(path);

            string x = Path.Combine(path, xs[Random.Range(0, xs.Length)]);
            if (Directory.Exists(x))
            {
                string[] ys = Directory.GetDirectories(x);
                string y = Path.Combine(x, ys[Random.Range(0, ys.Length)]);
                if (Directory.Exists(y))
                {
                    string gs = Path.Combine(y, scale1024 ? FILE_HEIGHT_GS_1024 : FILE_HEIGHT_GS);
                    string rgb = Path.Combine(y, scale1024 ? FILE_HEIGHT_RGB_1024 : FILE_HEIGHT_RGB);
                    string sat = Path.Combine(y, scale1024 ? FILE_SAT_MAPTILER_1024 : FILE_SAT_MAPTILER_512);

                    return new TilePath(gs, rgb, sat);
                }
            }
        }
        return new TilePath();
    }

    public class TilePath
    {
        public string heightGS;
        public string heightRGB;
        public string texture;

        public TilePath() { }

        public TilePath(string heightGS, string heightRGB, string texture)
        {
            this.heightGS = heightGS;
            this.heightRGB = heightRGB;
            this.texture = texture;
        }

        public bool IsValid()
        {
            return File.Exists(heightGS) && File.Exists(heightRGB) && File.Exists(texture);
        }

        private Texture2D LoadPath(string path)
        {
            Texture2D tex = new Texture2D(1, 1);    
            tex.filterMode = FilterMode.Point;
            tex.LoadImage(File.ReadAllBytes(path), false);
            return tex;
        }

        public Texture2D LoadHeightGS()
        {
            return LoadPath(heightGS);
        }

        public Texture2D LoadHeightRGB()
        {
            return LoadPath(heightRGB);
        }

        public Texture2D LoadTexture()
        {
            return LoadPath(texture);
        }
    }
}
