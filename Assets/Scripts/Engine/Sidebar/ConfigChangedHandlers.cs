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
            Physics.gravity = new(Physics.gravity.x, configValue.Value, Physics.gravity.z);
        });

        addHandler("Time Scale", (ConfigValue configValue) =>
        {
            Debug.Log(configValue.Value);
            Time.timeScale = configValue.Value;
        });


    }

    private static void addHandler(string key, UnityAction<ConfigValue> handler)
    {
        Config.Instance.ConfigDict[key].ConfigChanged.AddListener(handler);
    }
}
