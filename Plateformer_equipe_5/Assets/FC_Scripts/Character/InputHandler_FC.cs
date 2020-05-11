using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler_FC : MonoBehaviour
{
    [Header("Key Binding")]
    public KeyCode crouch = KeyCode.X;
    public KeyCode jump = KeyCode.Space;   
    public KeyCode run = KeyCode.LeftShift;
    public KeyCode run2 = KeyCode.RightShift;
    public KeyCode doPush = KeyCode.E;

    public Controler_YT controller;
    public BoxPush_FC push;

    void Update()
    {
        push.pushKey = Input.GetKey(doPush);
        push.pushKeyDown = Input.GetKeyDown(doPush);
        push.pushKeyUp = Input.GetKeyUp(doPush);

        controller.runKey = Input.GetKey(run);
        controller.runKeyDown = Input.GetKeyDown(run);
        controller.runKeyUp = Input.GetKeyUp(run);

        controller.run2Key = Input.GetKey(run2);
        controller.run2KeyDown = Input.GetKeyDown(run2);
        controller.run2KeyUp = Input.GetKeyUp(run2);

        controller.jumpKey = Input.GetKey(jump);
        controller.jumpKeyDown = Input.GetKeyDown(jump);
        controller.jumpKeyUp = Input.GetKeyUp(jump);

        controller.crouchKey = Input.GetKey(crouch);
        controller.crouchKeyDown = Input.GetKeyDown(crouch);
        controller.crouchKeyUp = Input.GetKeyUp(crouch);
    }
}
