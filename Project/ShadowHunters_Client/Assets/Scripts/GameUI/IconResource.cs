using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameUI
{
    [CreateAssetMenu(fileName ="new_icon", menuName ="GameResource/Icon")]
    class IconResource : ScriptableObject
    {
        public string label;
        public Sprite sprite;
    }
}
