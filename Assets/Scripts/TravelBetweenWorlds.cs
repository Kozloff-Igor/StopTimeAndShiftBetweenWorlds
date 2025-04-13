using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TravelBetweenWorlds : MonoBehaviour
{
    public World currentWorld;
    public World nextWorld;
    public World[] possibleNextWorlds;
    int nextWorldId;
    public bool isTransiting, noChangedMaterialsPresent, currentWorldIsCompletelyHidden;
    public List<MaterialUniqueChanger> mucCurrentWorld;
    public List<MaterialUniqueChanger> mucNewWorld;
    public List<MaterialUniqueChanger> mucWithChangedMaterials;

    [Header("Materials and stuff")]
    public Transform mainCamTr;
    public Camera camRendText;
    int layerForRendTexts = 7;

    [Header("Link to map between worlds")]
    public MapControl mapControl;

    bool isInsideFinishWorld = false;

    public GameObject winPanel;

    //hotfix jam patches

    void Start()
    {
        StartCoroutine(CheckTransformsWithChangedMaterials());
        StartCoroutine(CheckTransformsFromOldWorld());
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Q) && !isTransiting)
        {
            nextWorldId++;
            nextWorldId %= possibleNextWorlds.Length;
            nextWorld = possibleNextWorlds[nextWorldId];
            BeginTravel(nextWorld);
        }*/
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isTransiting)
            {
                mapControl.gameObject.SetActive(!mapControl.gameObject.activeInHierarchy);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (mapControl.currentMapIcon != mapControl.closestIcon && !isTransiting && !mapControl.gameObject.activeInHierarchy)
            {
                BeginTravel(possibleNextWorlds[mapControl.closestIcon.WorldType]);
                if (mapControl.closestIcon.WorldType == 5) isInsideFinishWorld = true;
                mapControl.PerformTravel();
            }

        }
    }

    void BeginTravel(World _nextWorld)
    {
        isTransiting = true;
        noChangedMaterialsPresent = false;
        currentWorldIsCompletelyHidden = false;
        nextWorld = _nextWorld;
        camRendText.Render();
        AnimateCurrentWorld(false);

        for (int q = 0; q < nextWorld.materialUniqueChangers.Count; q++)
        {
            MaterialUniqueChanger muc = nextWorld.materialUniqueChangers[q];

            if (IsInsideCameraFrustrum(muc))
            {
                Change(muc);
            }
            muc.Activate(true); //gameObject.SetActive(true);
            muc.MakeAlive(false);
        }

        nextWorld.gameObject.SetActive(true);
    }

    void AnimateCurrentWorld(bool isOn)
    {
        for (int q = 0; q < currentWorld.materialUniqueChangers.Count; q++)
        {
            MaterialUniqueChanger muc = currentWorld.materialUniqueChangers[q];
            muc.MakeAlive(isOn);
        }
    }


    bool IsInsideCameraFrustrum(MaterialUniqueChanger materialUniqueChanger) //can't use rend.isVisible, because world is hidden during transition
    {
        Transform tr = materialUniqueChanger.transform;
        float dist = (tr.position - mainCamTr.position).sqrMagnitude;
        if (dist > 11000) return false;
        float dot = Vector3.Dot((tr.position - mainCamTr.position).normalized, mainCamTr.forward);
        return dot > 0.3f;//60 degrees frust is 0.5, will be enough to not check bounds
    }

    void Change(MaterialUniqueChanger materialUniqueChanger)
    {
        mucWithChangedMaterials.Add(materialUniqueChanger);
        materialUniqueChanger.ChangeMaterials();
        Transform tr = materialUniqueChanger.transform;
        //transformsWithChangedMaterials.Add(tr);
        tr.gameObject.layer = layerForRendTexts;
        Mesh mesh = materialUniqueChanger.mesh;

        Vector3[] verts_pos = mesh.vertices;
        Vector2[] verts_uv = mesh.uv;

        for (int q = 0; q < verts_pos.Length; q++)
        {
            verts_pos[q] = tr.TransformPoint(verts_pos[q]);
            verts_uv[q] = camRendText.WorldToViewportPoint(verts_pos[q]);
        }
        mesh.uv = verts_uv;
    }

    IEnumerator CheckTransformsWithChangedMaterials()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (isTransiting && !noChangedMaterialsPresent)
            {
                for (int q = mucWithChangedMaterials.Count - 1; q >= 0; q--)
                {
                    MaterialUniqueChanger muc = mucWithChangedMaterials[q];
                    //if (!IsInsideCameraFrustrum(transformsWithChangedMaterials[q])) //isVisible is better for big stuff like houses
                    if (!muc.rend.isVisible)
                    {
                        //Change in post-jam !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!, for now MaterialUniqueChanger MUST be on object.                        
                        muc.BackToNormal();
                        muc.gameObject.layer = 0;
                        mucWithChangedMaterials.RemoveAt(q);
                    }
                }
                if (mucWithChangedMaterials.Count == 0)
                {
                    noChangedMaterialsPresent = true;
                    if (currentWorldIsCompletelyHidden) { isTransiting = false; AnimateCurrentWorld(true); CheckWin();}
                }

            }
        }
    }

    IEnumerator CheckTransformsFromOldWorld()
    {
        yield return new WaitForSeconds(0.15f);
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (isTransiting && !currentWorldIsCompletelyHidden)
            {
                for (int q = mucCurrentWorld.Count - 1; q >= 0; q--)
                {
                    MaterialUniqueChanger muc = mucCurrentWorld[q];
                    //if (!IsInsideCameraFrustrum(transformsFromOldWorld[q]))
                    if (!muc.rend.isVisible)
                    {
                        muc.Activate(false);
                        mucCurrentWorld.RemoveAt(q);
                    }
                }
                if (mucCurrentWorld.Count == 0)
                {
                    currentWorld.gameObject.SetActive(false);
                    currentWorldIsCompletelyHidden = true;
                    currentWorld = nextWorld;
                    mucCurrentWorld.AddRange(currentWorld.materialUniqueChangers);
                    if (noChangedMaterialsPresent) { isTransiting = false; AnimateCurrentWorld(true); CheckWin();}
                }
            }
        }
    }

    void CheckWin()
    {
        if (isInsideFinishWorld)
        {
            winPanel.SetActive(true);
            enabled = false;
        }
    }
}
