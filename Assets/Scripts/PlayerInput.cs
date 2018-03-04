using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static int HorizontalInput = 0;
    public static int VerticalInput = 0;
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

    private int updateHorizontalInput()
    {
        int _input = 0;

        // Keyboard inputs for going right/left
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) _input++;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) _input--;

        // UI button inputs for going right/left
        if (RightButtonDown) _input++;
        if (LeftButtonDown) _input--;

        return Mathf.Clamp(_input, -1, 1);
    }

    private int updateVerticalInput()
    {
        int _input = 0;

        // Keyboard inputs for going up/down
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) _input++;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) _input--;

        // UI button inputs for going up/down
        if (UpButtonDown) _input++;
        if (DownButtonDown) _input--;

        return Mathf.Clamp(_input, -1, 1);
    }

    private bool updateShootInput()
    {
        bool _shootInput = false;

        // Keyboard inputs for shooting
        if (Input.GetKeyDown(KeyCode.LeftControl)) _shootInput = true;

        return _shootInput;
    }
}
