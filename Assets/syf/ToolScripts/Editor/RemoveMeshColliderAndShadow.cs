
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

//将prefab资源打包

#if UNITY_EDITOR
public class RemoveMeshColliderAndShadow 
{
    [MenuItem("Scripts/Remove MeshCollider And Shadow")]
    static void ExecuteForWindows()
    {
        foreach (UnityEngine.GameObject obj in Selection.gameObjects)
        {
            MeshCollider[] mcs = obj.GetComponentsInChildren<MeshCollider>();
            foreach (MeshCollider mc in mcs)
            {
                UnityEngine.Object.DestroyImmediate(mc);
            }

            MeshRenderer[] mrs = obj.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr in mrs)
            {
                mr.castShadows = false;
                mr.receiveShadows = false;
            }

            ParticleRenderer[] prs = obj.GetComponentsInChildren<ParticleRenderer>();
            foreach (ParticleRenderer pr in prs)
            {
                pr.castShadows = false;
                pr.receiveShadows = false;
            }
        }
 
    }
    
}

#endif