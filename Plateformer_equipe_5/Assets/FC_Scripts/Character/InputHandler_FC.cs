using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler_FC : MonoBehaviour
{
    public Controler_YT controller;
    public BoxPush_FC push;

    void Update()
    {
        push.pushKey = Input.GetButton("Fire2");
        push.pushKeyDown = Input.GetButtonDown("Fire2");
        push.pushKeyUp = Input.GetButtonUp("Fire2");

        controller.runKey = Input.GetButton("Fire1");
        controller.runKeyDown = Input.GetButtonDown("Fire1");
        controller.runKeyUp = Input.GetButtonUp("Fire1");

        controller.jumpKey = Input.GetButton("Jump");
        controller.jumpKeyDown = Input.GetButtonDown("Jump");
        controller.jumpKeyUp = Input.GetButtonUp("Jump");
    }
}
