using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Since unity doesn't flag the Vector3 as serializable, we
/// need to create our own version. This one will automatically convert
/// between Vector3 and SerializableVector3
/// </summary>
[System.Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// x component
    /// </summary>
    public float X;

    /// <summary>
    /// y component
    /// </summary>
    public float Y;

    /// <summary>
    /// z component
    /// </summary>
    public float Z;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    /// <param name="Z"></param>
    public SerializableVector3(float X, float Y, float Z)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}]", X, Y, Z);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(SerializableVector3 value)
    {
        return new Vector3(value.X, value.Y, value.Z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3(Vector3 value)
    {
        return new SerializableVector3(value.x, value.y, value.z);
    }
}