using Kernel.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenuUI.Settings
{
    class GSettings : MonoBehaviour
    {
        private Dictionary<string, GameObject> categories = new Dictionary<string, GameObject>();
        private List<string> access = new List<string>
        {
            "normal",
            "advanced",
            "dev"
        };

        public string categoriePrefabPath = "Prefabs/UI/Settings/categorie";
        public RectTransform container;

        private GameObject categoriePrefab;

        int accessLevel = 2;

        private void Start()
        {
            categoriePrefab = Resources.Load<GameObject>(categoriePrefabPath);
            Generate();
        }

        public void Generate()
        {
            for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }

            string[] settings = SettingManager.Settings.UI_Settings.Value;
            RectTransform tr = this.transform as RectTransform;
            container.sizeDelta -= new Vector2(0, tr.sizeDelta.y);
            tr.sizeDelta -= new Vector2(0, tr.sizeDelta.y);
            for (int i = 0; i < settings.Length; i++)
            {
                string[] args = settings[i].Split(';');
                // split (';') : "accessibility" ; "category path" ; "setting parametre" ; "prefab path" ; "send to prefab"
                if (access.Contains(args[0]) && access.IndexOf(args[0]) <= accessLevel){
                    string[] path = args[1].Split('/');
                    if (!categories.ContainsKey(path[0]))
                    {
                        GameObject cat = Instantiate(categoriePrefab);
                        cat.GetComponent<Categorie>().Configurate(path[0]);
                        cat.transform.SetParent(transform);
                        categories.Add(path[0], cat);
                    }
                    GameObject c = categories[path[0]];
                    Categorie categ = c.GetComponent<Categorie>();
                    GameObject setting = Instantiate(Resources.Load<GameObject>(args[3]), categ.content);
                    RectTransform setrect = (RectTransform)setting.transform;
                    categ.content.sizeDelta += new Vector2(0, setrect.sizeDelta.y + categ.content.GetComponent<VerticalLayoutGroup>().spacing);
                    //tr.sizeDelta += new Vector2(0, setrect.sizeDelta.y + categ.content.GetComponent<VerticalLayoutGroup>().spacing);
                    //container.sizeDelta += new Vector2(0, setrect.sizeDelta.y + categ.content.GetComponent<VerticalLayoutGroup>().spacing);
                    ((RectTransform)categ.transform).sizeDelta += new Vector2(0, setrect.sizeDelta.y + categ.content.GetComponent<VerticalLayoutGroup>().spacing);
                    PropertyInfo pi = SettingManager.Settings.GetType().GetProperty(args[2]);
                    SettingItem s = null;
                    if (pi != null)
                    {
                        s = (SettingItem)pi.GetValue(SettingManager.Settings);
                    }
                    setting.GetComponent<SettingPrefab>().Configurate(path[path.Length - 1], s, args[4]);
                }
            }
            foreach (KeyValuePair<string,GameObject> o in categories)
            {
                RectTransform cat = o.Value.transform as RectTransform;
                tr.sizeDelta += new Vector2(0, cat.sizeDelta.y);
            }
            container.sizeDelta += new Vector2(0, tr.sizeDelta.y);
        }
    }
}
