using UnityEngine;
using System.Collections;
using Kernel.Settings;

public class SceneManagerComponant : MonoBehaviour
{
    //public static SceneManagerComponant Instance { get; set; }

    public static Setting<int> LocalPlayerId = new Setting<int>(0);
    

}
