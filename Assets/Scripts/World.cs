using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public List<MaterialUniqueChanger> materialUniqueChangers = new List<MaterialUniqueChanger>();

    [ContextMenu("FillMaterialUniqueChangers")]
    void FillMaterialUniqueChangers()
    {
        MaterialUniqueChanger[] mucs = GetComponentsInChildren<MaterialUniqueChanger>();
        Debug.Log(mucs.Length);
        materialUniqueChangers.AddRange(mucs);
        //materialUniqueChangers.Add(mucs[0]);
        foreach (MaterialUniqueChanger muc in mucs)
        {
            muc.InitialSetup();
        }
    }
}