using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridBuilder : MonoBehaviour
{
    [SerializeField] int xSize, ySize;

    [SerializeField] bool showVertices = false;

    Vector3[] vertices;

    Mesh mesh;

    void Awake() {
        Generate();
    }

    void Generate()
    {
        //x and y size are +1 because need one more vertex than tiles 
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        //reposition camera to ensure dots appear central
        Camera.main.transform.Translate(new Vector3(xSize / 2, ySize / 2, 0));

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        //Position each of the vertices as required
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        //xSize * ySize gives number of square tiles in mesh grid
        //* this by 6 because each square requires 2 triangles, each of which use 3 ints from triangle array
        int[] triangles = new int[xSize * ySize * 6]; //triangles must be built clockwise
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) //ti and vi = triangle and vertex indices
        {
            //creates 2 triangles (i.e. 1 square) per iteration
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi; //bottom left
                triangles[ti + 3] = triangles[ti + 2] = vi + 1; //bottom right vertex
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1; //top left vertex 
                triangles[ti + 5] = vi + xSize + 2; //top right vertex
            }
        }
        mesh.triangles = triangles;        
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos() {
        //don't draw in edit mode (when there are no vertices) as this would throw error
        if (!showVertices || vertices == null)
        {
            return;    
        }

        Gizmos.color = Color.magenta;
        for (int i = 0; i < vertices.Length; i++)
        {
            //draw a small sphere at every vertex point in the vertices array
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
