/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameHandler_Level : MonoBehaviour {

    [SerializeField] private DoorAnims entranceDoorAnims;

    private void Start() {
        FunctionTimer.Create(() => { entranceDoorAnims.SetColor(DoorAnims.ColorName.Green); }, 3.0f);
        FunctionTimer.Create(() => { entranceDoorAnims.OpenDoor(); }, 3.5f);

        CinematicBars.Show_Static(150f, .01f);
        FunctionTimer.Create(() => { CinematicBars.Show_Static(0f, .5f); }, 3f);
    }

}
