using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class WorldDataHelper
{
    public static Vector3Int ChunkPositionFromBlockCoords(World world, Vector3Int pos)
    {
        return new Vector3Int
        {
            x = Mathf.FloorToInt(pos.x / (float)world.chunkSize) * world.chunkSize,
            y = Mathf.FloorToInt(pos.y / (float)world.chunkHeight) * world.chunkHeight,
            z = Mathf.FloorToInt(pos.z / (float)world.chunkSize) * world.chunkSize
        };
    }

    internal static List<Vector3Int> GetChunkPositionsNearUpdatePoint(World world, Vector3Int updatePoint)
    {
        int startX = updatePoint.x - (world.chunkDrawingRange) * world.chunkSize;
        int startZ = updatePoint.z - (world.chunkDrawingRange) * world.chunkSize;
        int endX = updatePoint.x + (world.chunkDrawingRange) * world.chunkSize;
        int endZ = updatePoint.z + (world.chunkDrawingRange) * world.chunkSize;

        List<Vector3Int> chunkPositionsToCreate = new List<Vector3Int>();
        for (int x = startX; x <= endX; x += world.chunkSize)
        {
            for (int z = startZ; z <= endZ; z += world.chunkSize)
            {
                Vector3Int chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, 0, z));
                chunkPositionsToCreate.Add(chunkPos);
                //if (x >= updatePoint.x - world.chunkSize &&
                //    x <= updatePoint.x - world.chunkSize &&
                //    z >= updatePoint.z - world.chunkSize &&
                //    z <= updatePoint.z - world.chunkSize)
                //{
                //    for (int y = -world.chunkHeight; y >= updatePoint.y - world.chunkHeight * 2; y -= world.chunkHeight)
                //    {
                //        chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, y, z));
                //        chunkPositionsToCreate.Add(chunkPos);
                //    }
                //} //Code for vertical chunks
            }
        }
        return chunkPositionsToCreate;
    }

    internal static void RemoveChunkData(World world, Vector3Int pos)
    {
        world.worldData.chunkDataDictionary.Remove(pos);
    }

    internal static void RemoveChunk(World world, Vector3Int pos)
    {
        ChunkRenderer chunk = null;
        if (world.worldData.chunkDictionary.TryGetValue(pos, out chunk))
        {
            world.RemoveChunk(chunk);
            world.worldData.chunkDictionary.Remove(pos);
        }
    }

    internal static List<Vector3Int> GetChunkDataPositionsNearUpdatePoint(World world, Vector3Int updatePoint)
    {
        int startX = updatePoint.x - (world.chunkDrawingRange + world.chunkDrawingBuffer) * world.chunkSize;
        int startZ = updatePoint.z - (world.chunkDrawingRange + world.chunkDrawingBuffer) * world.chunkSize;
        int endX = updatePoint.x + (world.chunkDrawingRange + world.chunkDrawingBuffer) * world.chunkSize;
        int endZ = updatePoint.z + (world.chunkDrawingRange + world.chunkDrawingBuffer) * world.chunkSize;

        List<Vector3Int> chunkDataPositionsToCreate = new List<Vector3Int>();
        for (int x = startX; x <= endX; x += world.chunkSize)
        {
            for (int z = startZ; z <= endZ; z += world.chunkSize)
            {
                Vector3Int chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, 0, z));
                chunkDataPositionsToCreate.Add(chunkPos);
                //if (x >= updatePoint.x - world.chunkSize &&
                //    x <= updatePoint.x - world.chunkSize &&
                //    z >= updatePoint.z - world.chunkSize &&
                //    z <= updatePoint.z - world.chunkSize)
                //{
                //    for (int y = -world.chunkHeight; y >= updatePoint.y - world.chunkHeight * 2; y -= world.chunkHeight)
                //    {
                //        chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, y, z));
                //        chunkDataPositionsToCreate.Add(chunkPos);
                //    }
                //} //Code for vertical chunks
            }
        }
        return chunkDataPositionsToCreate;
    }

    internal static List<Vector3Int> GetUnneededData(World.WorldData worldData, List<Vector3Int> chunkDataPositionsNearUpdatePoint)
    {
        return worldData.chunkDataDictionary.Keys
            .Where(pos => chunkDataPositionsNearUpdatePoint.Contains(pos) == false && worldData.chunkDataDictionary[pos].modifiedByThePlayer == false)
            .ToList();
    }

    internal static List<Vector3Int> GetUnneededChunks(World.WorldData worldData, List<Vector3Int> chunkPositionsNearUpdatePoint)
    {
        List<Vector3Int> chunkPositionsToRemove = new List<Vector3Int>();
        foreach (var pos in worldData.chunkDictionary.Keys
            .Where(pos => chunkPositionsNearUpdatePoint.Contains(pos) == false))
        {
            chunkPositionsToRemove.Add(pos);
        }
        return chunkPositionsToRemove;
    }

    internal static List<Vector3Int> SelectPositionsToCreate(World.WorldData worldData, List<Vector3Int> chunkPositionsNearUpdatePoint, Vector3Int updatePoint)
    {
            return chunkPositionsNearUpdatePoint
                .Where(pos => worldData.chunkDictionary.ContainsKey(pos) == false)
                //.OrderBy(pos => Vector3.Distance(updatePoint, pos))
                .OrderBy(pos => Vector3.SqrMagnitude(updatePoint + pos))
                .ToList();
    }

    internal static List<Vector3Int> SelectDataPositionsToCreate(World.WorldData worldData, List<Vector3Int> chunkDataPositionsNearUpdatePoint, Vector3Int updatePoint)
    {
            return chunkDataPositionsNearUpdatePoint
                    .Where(pos => worldData.chunkDataDictionary.ContainsKey(pos) == false)
                    //.OrderBy(pos => Vector3.Distance(updatePoint, pos))
                    .OrderBy(pos => Vector3.SqrMagnitude(updatePoint + pos))
                    .ToList();
    }
}