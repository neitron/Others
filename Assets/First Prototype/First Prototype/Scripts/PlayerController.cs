using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{


    public float interactRange;
    public float attackRange;


    PlayerMotor motor;
    InputHandler inputHandler;



    void Start ()
    {
        motor = GetComponent<PlayerMotor>();
        inputHandler = new InputHandler(GameObject.Find("Joy Pad").GetComponent<JoyPad>());
    }

    
    void Update ()
    {
        ICommand command = inputHandler.HandleInput();
        command.Execute(this);
	}

    
    public void Run(Vector3 newPoint)
    {
        motor.MoveToPoint(newPoint);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
