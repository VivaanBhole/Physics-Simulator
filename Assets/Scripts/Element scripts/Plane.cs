using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Element
{
    public float Length, Width;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void SetLength(float l)
    {
        Length = l;
        transform.localScale += Vector3.right * (l - transform.localScale.x);
    }

    public override void SetWidth(float w)
    {
        Width = w;
        transform.localScale += Vector3.forward * (w - transform.localScale.z);

    }
}
