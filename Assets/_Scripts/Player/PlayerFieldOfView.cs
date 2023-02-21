using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFieldOfView : MonoBehaviour
{
    private Mesh mesh;

    [SerializeField] private LayerMask blockVisionLayer;
    [SerializeField] [Range(0f, 180f)] private float fov;
    [SerializeField] [Range(50, 100)] private int rayCount;
    [SerializeField] [Range(10f, 100f)] private float viewDistance;

    private Transform origin;

    //Calculated
    private float angleIncrease;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        CalculateInitialValues();
        DrawMesh();
    }

    private void CalculateInitialValues()
    {
        angleIncrease = fov / rayCount;

        vertices = new Vector3[rayCount + 1 + 1];
        uv = new Vector2[vertices.Length];
        triangles = new int[rayCount * 3];
    }

    private void DrawMesh()
    {
        vertices[0] = origin.position;

        float angle = origin.eulerAngles.z + fov / 2;
        int vertexIndex = 1;
        int triangleIndex = 0;
        for(int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(origin.position, GetVectorFromAngle(angle), viewDistance, blockVisionLayer);
            if(hit.collider == null)
            {
                vertex = origin.position + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = hit.point;
            }

            vertices[vertexIndex] = vertex;
            if(i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin.position, Vector3.one * 1000f);
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
    }
}
