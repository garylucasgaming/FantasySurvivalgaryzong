using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionExtensions
{
    public static Vector3Int GetVector(this Direction direction)
    {
         switch(direction)
        {
            case Direction.up: return Vector3Int.up;
            case Direction.down: return Vector3Int.down;
            case Direction.right:  return Vector3Int.right;
            case Direction.left:  return Vector3Int.left;
            case Direction.forward: return Vector3Int.forward;
            case Direction.backwards: return Vector3Int.back;
            default: throw new Exception("invalid input direction");
        }
    }
}