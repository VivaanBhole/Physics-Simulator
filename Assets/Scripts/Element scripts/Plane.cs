using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Element
{
    public bool Finite;
    public float Length, Width;
    public float AreaChargeDensity;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void SetAreaChargeDensity(float d)
    {
        AreaChargeDensity = d;
        if (!Finite)
            Charge = Length * Width * AreaChargeDensity;
    }

    public override void SetFinite(float f)
    {
        Finite = f > 0;
        if (!Finite)
            Charge = Length * Width * AreaChargeDensity;
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
