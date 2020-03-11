using Assets.Scripts.MainMenuUI.SearchGame;
using EventSystem;
using Kernel.Settings;
using Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class GameManager : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnBeforeSceneLoadRuntimeMethod()
        {
            EventView.Load();
            SettingManager.Load();
            Language.Init();
            GRoom.Init();
        }

        private void OnApplicationQuit()
        {
            SettingManager.Save();
        }
    }
}
