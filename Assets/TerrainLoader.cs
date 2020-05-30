using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoader : MonoBehaviour
{
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    Mesh mesh;
    public int mapSize = 512;
    public float heightScale = 100;
    [HideInInspector]
    public Vector3[] verts;

    public bool rgbMap = true;
    public Texture2D heightmap;
    public Texture2D texture;
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        //Init();
    }

    public void Init()
    {
        AssignMeshComponents();
        BuildMesh(new Vector2Int(mapSize, mapSize));
    }

    public void AssignMeshComponents()
    {
        // Find/create mesh holder object in children
        string meshHolderName = "Mesh Holder";
        Transform meshHolder = transform.Find(meshHolderName);
        if (meshHolder == null)
        {
            meshHolder = new GameObject(meshHolderName).transform;
            meshHolder.transform.parent = transform;
            meshHolder.transform.localPosition = Vector3.zero;
            meshHolder.transform.localRotation = Quaternion.identity;
        }

        // Ensure mesh renderer and filter components are assigned
        if (!meshHolder.gameObject.GetComponent<MeshFilter>())
        {
            meshHolder.gameObject.AddComponent<MeshFilter>();
        }
        if (!meshHolder.GetComponent<MeshRenderer>())
        {
            meshHolder.gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer = meshHolder.GetComponent<MeshRenderer>();
        meshFilter = meshHolder.GetComponent<MeshFilter>();
    }

    public void BuildMesh(Vector2Int size)
    {
        verts = new Vector3[size.x * size.y];
        Vector2[] uv = new Vector2[size.x * size.y];
        int[] triangles = new int[(size.x - 1) * (size.y - 1) * 6];
        int t = 0;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int meshMapIndex = y * size.x + x;

                Color c = heightmap.GetPixel(x, y);

                float height = rgbMap ?
                    -10000 + ((c.r * 256 * 256 + c.g * 256 + c.b) * 0.1f * 256f) :
                    c.r * 5000;

                verts[meshMapIndex] = new Vector3(x, height * heightScale, y);

                uv[meshMapIndex] = new Vector2(x / 512f, y / 512f);

                // Construct triangles
                if (x != size.x - 1 && y != size.y - 1)
                {
                    t = (y * (size.x - 1) + x) * 3 * 2;

                    triangles[t + 0] = meshMapIndex + mapSize;
                    triangles[t + 1] = meshMapIndex + mapSize + 1;
                    triangles[t + 2] = meshMapIndex;

                    triangles[t + 3] = meshMapIndex + mapSize + 1;
                    triangles[t + 4] = meshMapIndex + 1;
                    triangles[t + 5] = meshMapIndex;
                    t += 6;
                }
            }
        }

        if (mesh == null)
        {
            mesh = new Mesh();
        }
        else
        {
            mesh.Clear();
        }
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = verts;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;

        //Ist neue instanz nötig?
        Material mat = new Material(material);
        mat.name = "Instance";
        mat.SetTexture("_BaseMap", texture);
        mat.SetTextureScale("_BaseMap", new Vector2(512f / mapSize, 512f / mapSize));
        meshRenderer.material = mat;
    }

    public void UpdateMesh()
    {
        meshFilter.sharedMesh.vertices = verts;
        meshFilter.sharedMesh.RecalculateNormals();
    }
}
