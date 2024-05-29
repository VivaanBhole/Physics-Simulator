using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSource : MonoBehaviour
{
    public enum FieldSourceType
    {
        Point,
        Plane
    }

    public FieldSourceType type;

    public Field field;

    /// <summary>
    /// Call with the appropriate type and field in order to allow the field to start working properly baseed on source
    /// </summary>
    /// <param name="type"></param>
    /// <param name="field"></param>
    public void InitalizeFieldSource(FieldSourceType type, Field field)
    {
        FieldUpdater.AddFieldSource(this);
        Bounds bounds = GetComponent<Renderer>().bounds;
        this.field = field;
        this.type = type;

        if (field.Style == Field.FieldStyle.Line)
        {
            // Grow the field bounding box to encapsulate how large it's supposed to be
            field.Bounds = bounds;
            field.Bounds.Encapsulate(bounds.center + field.Direction * field.extentDistance);
        }
    }
}
