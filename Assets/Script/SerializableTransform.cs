using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    [Serializable]
    public struct SerializableTransform
    {
        public SerializableVector3 Position;
        public SerializableQuaternion Rotation;
        public SerializableVector3 Scale;

public static SerializableTransform Empty()
        {
            SerializableTransform t = new SerializableTransform();
            t.Position = Vector3.zero;
            t.Rotation = Quaternion.identity;
            t.Scale = Vector3.one;
            return t;
        }

        public SerializableTransform(SerializableVector3 position, SerializableQuaternion rotation, SerializableVector3 scale)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
        }

        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}]", Position, Rotation, Scale);
        }

        /// <summary>
        /// Automatic conversion from SerializableVector3 to Vector3
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static implicit operator Transform(SerializableTransform transform)
        {
            GameObject gameObject = new GameObject("EmptyTransformDummy");
            gameObject.transform.position = transform.Position;
            gameObject.transform.rotation = transform.Rotation;
            gameObject.transform.localScale = transform.Scale;
            return gameObject.transform;
        }

        /// <summary>
        /// Automatic conversion from Vector3 to SerializableVector3
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator SerializableTransform(Transform transform)
        {
            return new SerializableTransform(
                transform.position,
                transform.rotation,
                transform.localScale);
        }

        public static SerializableTransform Lerp(SerializableTransform A,SerializableTransform B,float Alpha)
        {
            SerializableTransform output = new SerializableTransform();

            output.Position= Vector3.Lerp(A.Position, B.Position, Alpha);
            output.Rotation = Quaternion.Lerp(A.Rotation, B.Rotation, Alpha);
            output.Scale = Vector3.Lerp(A.Scale, B.Scale, Alpha);

            return output;
        }

        public SerializableTransform Mirror()
        {
            SerializableTransform normalTransform = this;
            Quaternion normalRotation = ((Quaternion)normalTransform.Rotation);
            //normalRotation.
            SerializableVector3 mirroredPosition = new SerializableVector3(
                normalTransform.Position.X * -1,
                normalTransform.Position.Y,
                normalTransform.Position.Z);
            //SerializableQuaternion mirroredRotation = normalTransform.Rotation

            //SerializableTransform mirroredTransform = new SerializableTransform();

            return new SerializableTransform(mirroredPosition, normalTransform.Rotation, normalTransform.Scale);
        }
    }
}
