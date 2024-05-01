using UnityEngine;
using UnityEngine.Events;

public class ConfigChangedHandlers : MonoBehaviour
{
    public static void registerHandlers()
    {
        addHandler("Fields Visible", delegate
        {
            //TODO 
            Debug.Log("Fields Visible Changed");
        });

        addHandler("Gravity", (ConfigValue configValue) =>
        {
            Debug.Log(configValue.Value);
        });


    }

    private static void addHandler(string key, UnityAction<ConfigValue> handler)
    {
        Config.Instance.ConfigDict[key].ConfigChanged.AddListener(handler);
    }
}
