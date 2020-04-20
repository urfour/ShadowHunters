using Assets.Scripts.MainMenuUI.SearchGame;
using Assets.Scripts.MainMenuUI.Settings;
using EventSystem;
using Kernel.Settings;
using ServerInterface.RoomEvents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenuUI.CreateRoom
{
    class GRoomSettings : MonoBehaviour
    {
        public InputField name;
        public InputField password;
        public Slider nbPlayers;
        public Text displayNbPlayers;

        public int nbMaxPlayers = 8;

        private RoomData data = new RoomData();

        private void Start()
        {
            nbPlayers.maxValue = nbMaxPlayers;
            nbPlayers.minValue = 4;

            OnNbPlayerChange();
            OnPassWordChange();
            OnNameChange();
        }

        public void OnNameChange()
        {
            data.Name = name.text;
        }

        public void OnPassWordChange()
        {
            data.Password = password.text;
            data.HasPassword = data.Password == null || data.Password.Length == 0;
        }

        public void OnNbPlayerChange()
        {
            data.MaxNbPlayer = (int)nbPlayers.value;
            displayNbPlayers.text = nbPlayers.value.ToString();
        }

        public void Create()
        {
            EventView.Manager.Emit(new CreateRoomEvent(data));
        }

        /*
        private Dictionary<string, GameObject> categories = new Dictionary<string, GameObject>();
        private List<string> access = new List<string>
        {
            "normal",
            "advanced",
            "dev"
        };

        public string categoriePrefabPath = "Prefabs/UI/Settings/categorie";
        public string[] settings;
        public RectTransform container;

        private GameObject categoriePrefab;

        int accessLevel = 2;

        private void Start()
        {
            categoriePrefab = Resources.Load<GameObject>(categoriePrefabPath);
            Generate();
        }

        public virtual void Generate()
        {
            for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
            
            RectTransform tr = this.transform as RectTransform;
            container.sizeDelta -= new Vector2(0, tr.sizeDelta.y);
            tr.sizeDelta -= new Vector2(0, tr.sizeDelta.y);
            for (int i = 0; i < settings.Length; i++)
            {
                string[] args = settings[i].Split(';');
                // split (';') : "accessibility" ; "category path" ; "setting parametre" ; "prefab path" ; "send to prefab"
                if (access.Contains(args[0]) && access.IndexOf(args[0]) <= accessLevel)
                {
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
                    PropertyInfo pi = GRoom.Instance.JoinedRoom.GetType().GetProperty(args[2]);
                    SettingItem s = null;
                    if (pi != null)
                    {
                        s = (SettingItem)pi.GetValue(GRoom.Instance.JoinedRoom);
                    }
                    setting.GetComponent<SettingPrefab>().Configurate(path[path.Length - 1], s, args[4]);
                }
            }
            foreach (KeyValuePair<string, GameObject> o in categories)
            {
                RectTransform cat = o.Value.transform as RectTransform;
                tr.sizeDelta += new Vector2(0, cat.sizeDelta.y);
            }
            container.sizeDelta += new Vector2(0, tr.sizeDelta.y);
        }
    */
    }
}