﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameUI
{
    class ResourceLoader
    {
        private static string card_path = "cardicons";
        private static string character_path = "charactericons";

        public static Dictionary<string,Sprite> CardSprites;
        public static Dictionary<string, Sprite> CharacterSprites;
        //public static Sprite CharacterUnknownSprite;

        public static void Load()
        {
            CardSprites = new Dictionary<string, Sprite>();
            CharacterSprites = new Dictionary<string, Sprite>();

            IconResource[] cards = Resources.LoadAll<IconResource>(card_path);
            foreach (IconResource i in cards)
            {
                if (!CardSprites.ContainsKey(i.label))
                {
                    CardSprites.Add(i.label, i.sprite);
                }
                else
                {
                    Debug.LogWarning("Card label already exists : " + i.label);
                }
            }
            
            IconResource[] characters = Resources.LoadAll<IconResource>(character_path);
            foreach (IconResource i in characters)
            {
                if (!CharacterSprites.ContainsKey(i.label))
                {
                    CharacterSprites.Add(i.label, i.sprite);
                }
                else
                {
                    Debug.LogWarning("Card label already exists : " + i.label);
                }
            }
        }
    }
}
