using UnityEngine;
using System.Collections;
using Kernel.Settings;
using Lang;

public class OnGameQuit : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.quitting += OnApplicationQuit;
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.quitting -= OnApplicationQuit;
#endif
    }

    private void OnApplicationQuit()
    {
        Logger.Info("[START]\tSaving SettingManager");
        SettingManager.Save();
        Logger.Info("[END]  \tSaving SettingManager");
        Logger.Info("[START]\tLanguage Save");
        Language.Instance.Save();
        Logger.Info("[END]  \tLanguage Save");
        Logger.Info("[START]\tNetwork Disconnect");
        ServerInterface.Network.NetworkView.Disconnect();
        Logger.Info("[END]  \tNetwork Disconnect");
    }
}
