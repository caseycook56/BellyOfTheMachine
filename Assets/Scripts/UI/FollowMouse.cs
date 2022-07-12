using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class FollowMouse : MonoBehaviour
{
    public PlayerInput input;
    Vector3 mousePos;
    private void Start()
    {
        input = GameObject.FindGameObjectWithTag("main_player").GetComponent<PlayerInput>();       
    }

    void Update()
    {
        //TO DO, add fucntionality to highlight when a hookshot is in range
        if (input == null)
        {
            input = GameObject.FindGameObjectWithTag("main_player").GetComponent<PlayerInput>();
        }
        Vector2 mouse_input = input.actions["MousePosition"].ReadValue<Vector2>();

        mousePos = new Vector3(mouse_input.x, mouse_input.y, 0);
        transform.position = mousePos;
    }
}
