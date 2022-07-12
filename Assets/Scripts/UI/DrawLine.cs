using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawLine : MonoBehaviour
{
    PhysicsBody body;
    InputController _InputController;
    LineRenderer _LineRenderer;

    private void Start()
    {
        body = GameObject.Find("Player").GetComponent<PhysicsBody>();
        _InputController = GameObject.Find("Player").GetComponent<InputController>();
        _LineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    public void FixedUpdate()
    {
        GenerateLines();
    }

    private void GenerateLines()
    {
        if (body == null)
        {
            body = GameObject.Find("Player").GetComponent<PhysicsBody>();
            _InputController = GameObject.Find("Player").GetComponent<InputController>();
        }
        _LineRenderer.positionCount = 2;
        _LineRenderer.SetPosition(0, body.GetPosition());
        _LineRenderer.SetPosition(1, _InputController.GetMouseWorldPosition());
    }

}
