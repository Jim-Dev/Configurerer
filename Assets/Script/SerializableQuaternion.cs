using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Since unity doesn't flag the Quaternion as serializable, we
/// need to create our own version. This one will automatically convert
/// between Quaternion and SerializableQuaternion
/// </summary>
[System.Serializable]
public struct SerializableQuaternion
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
    /// w component
    /// </summary>
    public float W;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    /// <param name="Z"></param>
    /// <param name="W"></param>
    public SerializableQuaternion(float X, float Y, float Z, float W)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
        this.W = W;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}, {3}]", X, Y, Z, W);
    }

    /// <summary>
    /// Automatic conversion from SerializableQuaternion to Quaternion
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Quaternion(SerializableQuaternion value)
    {
        return new Quaternion(value.X, value.Y, value.Z, value.W);
    }

    /// <summary>
    /// Automatic conversion from Quaternion to SerializableQuaternion
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static implicit operator SerializableQuaternion(Quaternion value)
    {
        return new SerializableQuaternion(value.x, value.y, value.z, value.w);
    }
}