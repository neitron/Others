using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{


    public float interactRange;
    public float attackRange;


    PlayerMotor motor;
    InputHandler inputHandler;

    public int moneyAmount { get; protected set; }



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


    internal void EarnCoin(int currencyCost)
    {
        Debug.Log("Just war erned: " + currencyCost);
        moneyAmount += currencyCost;

        // TODO: temp code. Remove it
        GameObject.Find("Money Amount").GetComponent<Text>().text = moneyAmount.ToString();
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        Interactable interactable = collision.collider.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.Interact(this.gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
