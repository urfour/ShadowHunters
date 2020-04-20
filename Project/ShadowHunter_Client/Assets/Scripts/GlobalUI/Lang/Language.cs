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
                Notifier.Notify();
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
            return Instance.GetText(label);
        }

        public static void AddListener(OnNotification listener)
        {
            Notifier.AddListener(listener);
        }

        public static void RemoveListener(OnNotification listener)
        {
            Notifier.AddListener(listener);
        }

        public static (Language,string)[] GetAllLanguages(string path = "Lang")
        {
            string[] files = IOSystem.GetAllFileNames(path);
            (Language, string)[] languages = new (Language, string)[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                languages[i] = (new Language(files[i]), files[i]);
            }
            return languages;
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
        
        public string GetText(string label)
        {
            string[] args = label.Split('&');
            if (args.Length > 1)
            {
                label = args[0] + "&" + (args.Length - 1);
                if (!Translations.ContainsKey(label))
                {
                    Translations.Add(label, label);
                    Save();
                    return Translations[label];
                }
                else
                {
                    string tlabel = Translations[label];
                    string[] targs = tlabel.Split('&');
                    string tmp = targs[0];
                    for (int i = 1; i < targs.Length; i++)
                    {
                        int index = int.Parse(""+ targs[i][0]);
                        if (index > 0 && index < targs.Length)
                        {
                            tmp += args[index] + targs[i].Substring(1);
                        }
                        else
                        {
                            tmp += "[INALID ARG INDEX : " + index + "]";
                        }
                    }
                    return tmp;
                }
            }
            else
            {
                if (!Translations.ContainsKey(args[0]))
                {
                    Translations.Add(label, label);
                    Save();
                }
                return Translations[label];
            }
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
