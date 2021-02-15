using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ColliderShower : MonoBehaviour
{
    public Material Material;

    [ReadOnlyInInspector]
    public Mesh Cube;
    [ReadOnlyInInspector]
    public BoxCollider BoxCollider;
    void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        Cube = PrimitiveMesh.GetUnityPrimitiveMesh(PrimitiveType.Cube);
    }

    void Update()
    {
        var mesh = Cube;
        var mat = Material;
        if (BoxCollider && mesh)
        {
            var matri = Matrix4x4.identity;
            matri.SetTRS(BoxCollider.transform.position, BoxCollider.transform.rotation, BoxCollider.size);
            Graphics.DrawMesh(Cube, matri, mat, 0);
        }

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        UnityEditor.Handles.Label(transform.position, "Åö×²ºÐ");
    }
#endif

}
