using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    void Update()
    {
        UpdateInput();
        UpdateMouse();
    }

    // 키 입력
    void UpdateInput()
    {
        Vector3 moveDirection = Vector3.zero;
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection.y = 1;
        }
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection.y = -1;
        }
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x = -1;
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x = 1;
        }
        
        SystemManager.Instance.Player.ProcessInput(moveDirection);
    }

    //  마우스 입력
    void UpdateMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SystemManager.Instance.Player.Fire();
        }
    }
}
