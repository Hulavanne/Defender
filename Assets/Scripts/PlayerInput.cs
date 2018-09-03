using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static float HorizontalInput = 0.0f;
    public static float VerticalInput = 0.0f;
    public static bool ShootInput = false;

    public static bool RightButtonDown = false;
    public static bool LeftButtonDown = false;
    public static bool UpButtonDown = false;
    public static bool DownButtonDown = false;

    void LateUpdate()
    {
        HorizontalInput = updateHorizontalInput();
        VerticalInput = updateVerticalInput();
        ShootInput = updateShootInput();
    }

    private float updateHorizontalInput()
    {
        float _input = 0;

        // Keyboard inputs for going right/left
        _input = Input.GetAxisRaw("Horizontal");

        // UI button inputs for going right/left
        if (RightButtonDown) _input++;
        if (LeftButtonDown) _input--;

        return Mathf.Clamp(_input, -1, 1);
    }

    private float updateVerticalInput()
    {
        float _input = 0;

        // Keyboard inputs for going up/down
        _input = Input.GetAxisRaw("Vertical");

        // UI button inputs for going up/down
        if (UpButtonDown) _input++;
        if (DownButtonDown) _input--;

        return Mathf.Clamp(_input, -1, 1);
    }

    private bool updateShootInput()
    {
        // Keyboard input for shooting (UI button input for shooting found in UserInterface.cs)
        return Input.GetButtonDown("Fire1");
    }
}
