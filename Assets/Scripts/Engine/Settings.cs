using UnityEngine;

public class Settings : MonoBehaviour
{
    //Singleton
    public static Settings Instance { get; set; } = null;

    //Declaration

    public enum TextSize
    {
        Small = 0,
        Medium = 1, 
        Large = 2
    }

    //Properties
    public byte Volume { get; set; } = 100;
    public TextSize Size { get; set; } = TextSize.Medium;
    public float Sensitivity {get; set; } = 2f;

    private void Awake()
    {

        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            Instance = this;
        }
    }

    public static int CorrespondingFontSize()
    {
        switch (Instance.Size)
        {
            case TextSize.Small:
                return 22;
            case TextSize.Medium:
                return 27;
            case TextSize.Large:
                return 32;
            default:
                Debug.Log("An error occured loading text size into text");
                return 32;
        }
    }
}
