using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private List<HotbarItem> HotbarItems;
    [SerializeField] private GameObject SelectionIndicator;

    public delegate void UpdateSelection(ElementData elementData);
    public UpdateSelection updateSelection;
    void Start()
    {
        SetSelection(1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            SetSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSelection(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSelection(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetSelection(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetSelection(5);
        }
    }

    public void SetSelection(int index)
    {
        SelectionIndicator.transform.position = new Vector3(
            HotbarItems[index - 1].transform.position.x,
            SelectionIndicator.transform.position.y,
            SelectionIndicator.transform.position.z
            );

        updateSelection(HotbarItems[index-1].Item);
    }
}