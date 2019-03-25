using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions 
{
    #region Entities
    #region Enums
    #endregion

    #region Delegates
    #endregion

    #region Structures
    #endregion

    #region Classes
    #endregion

    #region Interfaces
    #endregion
    #endregion

    #region Fields
    #endregion

    #region Events
    #endregion

    #region Behaviour
    #region Properties
    #endregion

    #region Methods
    public static Vector3 Devide(this Vector3 first, Vector3 devider)
    {
        first.x /= devider.x;
        first.y /= devider.y;
        first.z /= devider.z;

        return first;
    }

    public static float Max(this Vector3 vector)
    {
        float maxValue = vector.x;

        if (vector.y > maxValue)
        {
            maxValue = vector.y;
        }

        if (vector.z > maxValue)
        {
            maxValue = vector.z;
        }

        return maxValue;
    }

    public static float Min(this Vector3 vector)
    {
        float minValue = vector.x;

        if (vector.y < minValue)
        {
            minValue = vector.y;
        }

        if (vector.z < minValue)
        {
            minValue = vector.z;
        }

        return minValue;
    }
    #endregion
    #endregion
}