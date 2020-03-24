using EventSystem;
using IO;
using Kernel.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lang
{
    class Language
    {


        #region SINGLOTON
        private static Language _instance;
        public static Language Instance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                Notifier.TryNotify();
            }
        }
        
        public static void Init()
        {
            Notifier = new ListenableObject();
            //if (SettingManager.Settings == null) SettingManager.Load();
            Instance = new Language(SettingManager.Settings.UI_Lang.Value);

            SettingManager.Settings.UI_Lang.AddListener((sender) => { Instance = new Language(SettingManager.Settings.UI_Lang.Value); });
        }
        
        private static ListenableObject Notifier { get; set; } = new ListenableObject();

        public static string Translate(string label, params string[] args)
        {
            if (Instance == null) return label;
            if (!Instance.Translations.ContainsKey(label))
            {
                Instance.Translations.Add(label, label);
                Instance.Save();
            }
            return Instance.Translations[label];
        }

        public static void AddListener(OnNotification listener)
        {
            Notifier.AddListener(listener);
        }

        public static void RemoveListener(OnNotification listener)
        {
            Notifier.AddListener(listener);
        }
        #endregion




        #region INSTANCE
        private Dictionary<string, string> Translations { get; set; } = new Dictionary<string, string>();

        public string Lang { get; private set; }

        public Language(string lang)
        {
            this.Lang = lang;
            this.Load();
        }

        public void Load()
        {
            string[] lines = IOSystem.GetAllLines("Lang/" + this.Lang + ".txt");
            if (lines != null)
            {
                foreach (string l in lines)
                {
                    string[] args = l.Split(';');
                    if (args.Length != 2) Debug.LogWarning("Lang error : " + l);
                    else Translations.Add(args[0], args[1]);
                }
            }
            else
            {
                IOSystem.CreateFile("Lang/" + this.Lang + ".txt", false);
            }
        }

        public void Save()
        {
            string[] lines = new string[Translations.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> l in Translations)
            {
                lines[i] = l.Key + ";" + l.Value;
                i++;
            }
            IOSystem.CreateFile("Lang/" + this.Lang + ".txt", allLines: lines, deletePrevious: true);
        }
        #endregion
    }
}
