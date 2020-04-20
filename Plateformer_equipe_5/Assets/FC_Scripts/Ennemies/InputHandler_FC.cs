using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler_FC : MonoBehaviour
{
    [Header("Key Binding")]
    public KeyCode crouch = KeyCode.X;
    public KeyCode jump = KeyCode.Space;
    public KeyCode run = KeyCode.LeftShift;

    public Controler_YT controller;

    void Update()
    {
        controller.runKey = Input.GetKey(run);
        controller.runKeyDown = Input.GetKeyDown(run);
        controller.runKeyUp = Input.GetKeyUp(run);

        controller.jumpKey = Input.GetKey(jump);
        controller.jumpKeyDown = Input.GetKeyDown(jump);
        controller.jumpKeyUp = Input.GetKeyUp(jump);

        controller.crouchKey = Input.GetKey(crouch);
        controller.crouchKeyDown = Input.GetKeyDown(crouch);
        controller.crouchKeyUp = Input.GetKeyUp(crouch);
    }
}
