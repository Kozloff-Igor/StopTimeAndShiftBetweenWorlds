using UnityEngine;


public class MyUVmagic : MonoBehaviour
{
    public Transform uvProj;
    public Mesh mesh;
    Camera cam;
    public Camera rendTextCam;


    public Mesh meshFrom, meshTo;
    //  public bool generateUV2;
    void Start()
    {
        MeshFilter meshFilter = uvProj.GetComponent<MeshFilter>();
        if (meshFilter) { mesh = meshFilter.mesh; }
        else
        {
            mesh = uvProj.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }
        cam = Camera.main;

        /*if (generateUV2 && mesh.uv2.Length > 0)
        {
            Vector2[] verts_uv = mesh.uv;
            Vector2[] verts_uv2 = mesh.uv2;
            for (int q = 0; q < verts_uv2.Length; q++)
            {
                verts_uv2[q] = verts_uv[q];
            }
            mesh.uv2 = verts_uv2;
        }*/


    }

    [ContextMenu("GenerateUV2")]
    void GenerateUV2()
    {
        if (mesh.uv2.Length > 0)
        {
            Vector2[] verts_uv = mesh.uv;
            Vector2[] verts_uv2 = mesh.uv2;
            for (int q = 0; q < verts_uv2.Length; q++)
            {
                verts_uv2[q] = verts_uv[q];
            }
            mesh.uv2 = verts_uv2;
        }
    }
    [ContextMenu("Translate UVs")]
    void TranslateUV()
    {
        Vector2[] verts_uv = meshFrom.uv;
        Vector2[] vertsTo_uv = meshTo.uv;
        
        for (int q = 0; q < verts_uv.Length; q++)
        {
            vertsTo_uv[q] = verts_uv[q];
        }
        meshTo.uv = vertsTo_uv;
        //meshTo.uv = meshFrom.uv;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rendTextCam.Render();
            Vector3[] verts_pos = mesh.vertices;
            Vector2[] verts_uv = mesh.uv;

            for (int q = 0; q < verts_pos.Length; q++)
            {
                verts_pos[q] = uvProj.TransformPoint(verts_pos[q]);
                //verts_uv[q] = cam.WorldToScreenPoint(verts_pos[q]);
                verts_uv[q] = cam.WorldToViewportPoint(verts_pos[q]);

                //Debug.Log(verts_uv[q]);
            }
            mesh.uv = verts_uv;
            uvProj.gameObject.SetActive(uvProj.gameObject.activeSelf);
        }

    }
}