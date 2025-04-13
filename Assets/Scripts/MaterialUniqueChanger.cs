using UnityEngine;
using UnityEngine.AI;

public class MaterialUniqueChanger : MonoBehaviour
{
    public Material[] materialsWithRendText;
    public Material[] materialsRegular;
    public Renderer rend;
    public Mesh mesh;



    public GameObject originalPrefab;
    public GameObject parent;
    public Mesh originalMesh;

    public Rigidbody rb;
    public Animator animator;

    public void InitialSetup()
    {
        if (!rend) rend = GetComponent<Renderer>();
        if (materialsRegular.Length == 0) materialsRegular = rend.sharedMaterials;

        if (materialsWithRendText.Length != materialsRegular.Length) Debug.LogError("Materials length wrong");
    }

    void GrabMesh()
    {
        Transform tr = transform;
        if (tr.TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
        {
            mesh = meshFilter.mesh;
        }
        else
        {
            if (tr.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinnedMeshRenderer))
            {
                mesh = skinnedMeshRenderer.sharedMesh;
            }
            else { Debug.LogError("No mesh filter"); }
        }
    }

    public void ChangeMaterials()
    {
        if (!mesh) GrabMesh();
        rend.sharedMaterials = materialsWithRendText;
    }

    public void BackToNormal()
    {
        rend.sharedMaterials = materialsRegular;
        RestoreOriginalUV();
    }

    void RestoreOriginalUV()
    {
        if (!mesh) GrabMesh();
        if (originalMesh) { mesh.uv = originalMesh.uv; }
        else
        {
            Mesh originalPrefabMesh = originalPrefab.GetComponent<MeshFilter>().sharedMesh;
            mesh.uv = originalPrefabMesh.uv;
        }
        //GetComponent<MeshFilter>().mesh = originalPrefabMesh;
    }

    public void Activate(bool isOn)
    {
        if (parent) parent.SetActive(isOn); else gameObject.SetActive(isOn);
    }

    public void MakeAlive(bool isOn)
    {
        if (rb) rb.isKinematic = !isOn;
        if (animator)
        {
            animator.enabled = isOn;
            animator.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = isOn;
        }
    }
}
