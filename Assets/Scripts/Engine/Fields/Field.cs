using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public enum FieldStyle
    {
        PointSource,
        Line,
        Loops
    }

    public FieldStyle Style { get; set; }

    public float strength;

    public float extentDistance;

    /// <summary>
    /// Normal to Coils, Parallel to Line, Arbitrary for PointSource
    /// </summary>
    public Vector3 Direction { get; set; }

    public FieldSource Source { get; set; }

    public Bounds Bounds { get; set; }
    /// <summary>
    /// Generic Field. If pt charge, strength = charge
    /// </summary>
    /// <param name="style"></param>
    /// <param name="direction"></param>
    /// <param name="strength">Newtons/C, Teslas, or C</param>
    /// <param name="extentDistance"></param>
    public Field(FieldStyle style, Vector3 direction, float strength, float extentDistance = 0)
    {
        this.strength = strength;
        Style = style;
        Direction = direction.normalized;
        this.extentDistance = extentDistance;
    }
    /// <summary>
    /// Determine whether this field acts on the given element
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public bool ActsOn(Element element) 
    {
        switch (Style)
        {
            case FieldStyle.PointSource:
                return true;
            case FieldStyle.Line:
                return Bounds.Contains(element.transform.position);
            case FieldStyle.Loops:
                // TODO
                return true;
            default:
                return false;
        }
    }
    /// <summary>
    /// Virtual method for having a field affect an element
    /// </summary>
    /// <param name="element"></param>
    public virtual void Affect(Element element) { }
}
