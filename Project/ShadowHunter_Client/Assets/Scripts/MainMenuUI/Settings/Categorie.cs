using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MainMenuUI.Settings
{
    class Categorie : MonoBehaviour
    {
        public LabelTranslate label;
        public RectTransform content;

        public void Configurate(string label, string data = null)
        {
            this.label.label = label;
            this.label.Refresh();
        }
        
    }
}
