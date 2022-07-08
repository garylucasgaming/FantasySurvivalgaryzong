using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ChunkRenderer : MonoBehaviour
{

    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Mesh mesh;
    public bool showGizmo;

    public ChunkData ChunkData
    {
        get; private set;
    }

    public bool ModifiedByThePlayer
    {
        get
        {
            return ChunkData.modifiedByThePlayer;
        }
        set
        {
            ChunkData.modifiedByThePlayer = value;
        }
    }


    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
    }

    public void InitializeChunk(ChunkData data)
    {
        this.ChunkData = data;
    }


    private void RenderMesh(MeshData meshdata)
    {

        mesh.Clear();

        mesh.subMeshCount = 2;
        mesh.vertices = meshdata.vertices.Concat(meshdata.waterMesh.vertices).ToArray();

        mesh.SetTriangles(meshdata.triangles.ToArray(), 0);
        mesh.SetTriangles(meshdata.waterMesh.triangles.Select(val => val + meshdata.vertices.Count).ToArray(), 1);

        mesh.uv = meshdata.uv.Concat(meshdata.waterMesh.uv).ToArray();
        //mesh.uv = meshData.uv.ToArray();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = null;
        Mesh collisionMesh = new Mesh();
        collisionMesh.vertices = meshdata.colliderVertices.ToArray();
        collisionMesh.triangles = meshdata.colliderTriangles.ToArray();
        collisionMesh.RecalculateNormals();

        meshCollider.sharedMesh = collisionMesh;
    }


    public void UpdateChunk()
    {
        RenderMesh(Chunk.GetChunkMeshData(ChunkData));
    }

    public void UpdateChunk(MeshData data)
    {
        RenderMesh(data);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(showGizmo)
        {
            if(Application.isPlaying && ChunkData != null)
            {
                if(Selection.activeObject == gameObject)
                {
                    Gizmos.color = new Color(0, 1, 0, 0.4f);
                    
                }
                else
                {
                    Gizmos.color = new Color(1,0,1,0.4f);
                }

                Gizmos.DrawCube(transform.position + new Vector3(ChunkData.chunkSize / 2f, ChunkData.chunkHeight / 2f,
                    ChunkData.chunkSize / 2f), new Vector3(ChunkData.chunkSize, ChunkData.chunkHeight, ChunkData.chunkSize));
            }
        }
    }
#endif

}
