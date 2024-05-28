using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarItem : MonoBehaviour
{
    public ElementData Item;
    public Image Image;
    public TextMeshProUGUI Text;

    void Start()
    {
        if (Item)
        {
            Image.sprite = Item.Icon;
            Text.text = Item.Name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
