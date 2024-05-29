using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricField : Field
{
    public static readonly float k = (float)8.9875517923 * Mathf.Pow(10,9);

    public ElectricField(FieldStyle style, Vector3 direction, float strength, float extentDistance = 0) : base(style, direction, strength, extentDistance)
    {
    }

    public override void Affect(Element element)
    {
       switch (Style)
        {
            // Parallel plates
            case FieldStyle.Line:
            {
                // qE
                float eForce = strength * element.Charge;
                element.rb.AddForce(eForce * Direction, ForceMode.Impulse);
                break;
            }
            case FieldStyle.PointSource: 
            {
                // kq1q2/r^2
                float eForce = k * strength * element.Charge / (element.transform.position - Source.transform.position).sqrMagnitude;
                element.rb.AddForce(eForce * Direction, ForceMode.Impulse);
                break;
            }
        }
    }
}
