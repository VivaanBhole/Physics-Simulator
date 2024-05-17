using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    public static float radius { get; private set; }
    public bool Connected, beingPlaced;
    public float Potential;
    public HashSet<ConnectionPoint> Connections = new HashSet<ConnectionPoint>();
    public CircuitElement EncompassingElement;
    public Vector3 snapPos;

    public static bool isVisible = false;
    private bool _visibilityUpated = true;
    private Renderer _renderer;
    void Start()
    {
        if(radius == 0)
            radius = GetComponent<SphereCollider>().radius;

        _renderer = GetComponent<Renderer>();
        beingPlaced = true;
        Connected = false;
    }


    void Update()
    {
        if (_visibilityUpated != isVisible)
        {
            _visibilityUpated = isVisible;
            _renderer.enabled = isVisible;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!beingPlaced) return;
        ConnectionPoint otherConnectionPoint = other.GetComponent<ConnectionPoint>();
        if (otherConnectionPoint)
        {
            Connect(otherConnectionPoint);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!beingPlaced) return;
        ConnectionPoint otherConnectionPoint = other.GetComponent<ConnectionPoint>();
        if (otherConnectionPoint)
        {
            otherConnectionPoint.Connections.Remove(this);
            Connections.Remove(otherConnectionPoint);
            otherConnectionPoint.updateConnections();
            updateConnections();            
        }
    }

    private void updateConnections()
    {
        if(EncompassingElement)
        if(Connections.Count > 0 != Connected)
            EncompassingElement.UpdateConnections();
        Connected = Connections.Count > 0;
    }

    private void Connect(ConnectionPoint other)
    {
        other.Connections.Add(this);
        Connections.Add(other);
        other.updateConnections();
        updateConnections();

        if (Connected)
        {
            snapPos = other.transform.position;
        }
    }
}
