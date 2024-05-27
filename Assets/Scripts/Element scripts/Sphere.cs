using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : Element
{
    // Start is called before the first frame update
    [SerializeField] public float Radius;
    [SerializeField] public bool Hollow;
    [SerializeField] public float VolumeChargeDensity;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetRadius(float r)
    {
        Radius = r;
        transform.localScale = Vector3.one * r;
    }

    public override void SetHollow(float h)
    {
        Hollow = h > 0;
    }

    public override void SetVolumeChargeDensity(float d)
    {
        VolumeChargeDensity = d;
        Charge = (4f / 3f) * Mathf.PI * Mathf.Pow(Radius, 3);
    }
}
