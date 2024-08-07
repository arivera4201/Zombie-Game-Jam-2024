using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    //Set player's resolution to 640x480 for retro style
    void Start()
    {
        Screen.SetResolution(640, 480, true);
    }

}
