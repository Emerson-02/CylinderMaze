using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class InvertMesh : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        int[] triangles = mesh.triangles;
        Vector3[] normals = mesh.normals;

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int temp = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = temp;
        }

        mesh.normals = normals;
        mesh.triangles = triangles;
    }
}
