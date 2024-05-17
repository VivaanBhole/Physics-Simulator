using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class CircuitElement : Element
{
    public List<ConnectionPoint> ConnectionPoints;
    public Vector3 DesiredPos;
    public bool IsConnected;

    void Start()
    {
        Debug.Log("start: ");
        foreach (ConnectionPoint connectionPoint in ConnectionPoints)
            connectionPoint.EncompassingElement = this;
        foreach (ConnectionPoint connectionPoint in ConnectionPoints)
            Debug.Log(connectionPoint.EncompassingElement.name);
    }

    void Update()
    {
    }

    public void UpdateConnections()
    {
        int connections = 0;
        foreach(ConnectionPoint connectionPoint in ConnectionPoints)
        {
            connections += connectionPoint.Connected?1:0;
        }
        if (connections >= 2)
            IsConnected = true;
    }

    public Vector3 GetDesiredPos(Vector3 defaultPos)
    {
        if (ConnectionPoints.Count==0) return defaultPos;
        foreach(ConnectionPoint connectionPoint in ConnectionPoints)
        {
            if (connectionPoint.Connected)
            {
                Vector3 delta = transform.position - defaultPos;
                if (delta.magnitude > ConnectionPoint.radius)
                    return defaultPos;

                return transform.position + connectionPoint.snapPos - connectionPoint.transform.position;
            }
        }
        return defaultPos;
    }

    public void AddToCircuit()
    {
        Circuit circuit = null;
        
        foreach(ConnectionPoint cp in ConnectionPoints)
            foreach(ConnectionPoint other in cp.Connections)
                circuit = other.GetComponentInParent<Circuit>();

        if (!circuit)
        {
            GameObject parent = new GameObject("Circuit");
            transform.parent.parent = parent.transform;
            foreach (ConnectionPoint cp in ConnectionPoints)
                foreach (ConnectionPoint other in cp.Connections)
                {
                    other.EncompassingElement.transform.parent.parent = parent.transform;
                }
            parent.AddComponent<Rigidbody>().useGravity = false;
            parent.AddComponent<Circuit>();
        }
        else
        {
            transform.parent.parent = circuit.transform;
        }

        Destroy(GetComponentInChildren<Rigidbody>());
    } 
}