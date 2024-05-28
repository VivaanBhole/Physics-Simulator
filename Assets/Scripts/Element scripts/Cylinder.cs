using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : Element
{

    [SerializeField] public float Radius, Length;
    [SerializeField] public bool Hollow;
    [SerializeField] public bool Finite;
    [SerializeField] public float VolumeChargeDensity, LinearChargeDensity;
    [SerializeField] public GameObject hollowCylinder, SolidCylinder;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void SetRadius(float r)
    {
        Radius = r;
        transform.localScale = new Vector3(Radius, Radius, transform.localScale.z);
    }

    public override void SetLength(float l)
    {
        Length = l;
        transform.localScale += Vector3.right * (l - transform.localScale.x);
    }

    public override void SetHollow(float h)
    {
        Hollow = h > 0;
        hollowCylinder.SetActive(Hollow);
        SolidCylinder.SetActive(!Hollow);

    }

    public override void SetLinearChargeDensity(float d)
    {
        LinearChargeDensity = d;
        VolumeChargeDensity = d * Mathf.PI * Mathf.Pow(Radius, 2);
        if (!Finite)
            Charge = LinearChargeDensity * Length;
    }

    public override void SetVolumeChargeDensity(float d)
    {
        VolumeChargeDensity = d;
        LinearChargeDensity = d / (Mathf.PI * Mathf.Pow(Radius, 2));
        if (!Finite)
            Charge = LinearChargeDensity * Length;
    }
    public override void SetFinite(float f)
    {
        Finite = f > 0;
        if (!Finite)
            Charge = LinearChargeDensity * Length;
    }

}
