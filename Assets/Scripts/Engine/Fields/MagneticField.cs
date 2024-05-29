using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : Field
{
    public MagneticField(FieldStyle style, Vector3 direction, float strength, float extentDistance = 0) : base(style, direction, strength, extentDistance)
    {
    }

    public override void Affect(Element element)
    {
        switch (Style)
        {
            case FieldStyle.Line:
            {
                // q vxB
                var bForce = element.Charge * Vector3.Cross(element.rb.velocity, strength * Direction);
                element.rb.AddForce(bForce, ForceMode.Impulse);
                break;
            }

            case FieldStyle.Loops:
            {
                    // TODO depending on leaving this in?
                    break;
            }
            // no monopoles
        }
    }
}
