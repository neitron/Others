using UnityEngine;



public class Coin : Interactable
{


    public int currencyCost = 5;



    private void Start()
    {
        transform.Rotate(0.0f, Random.Range(0, 361), 0.0f); 
    }


    private void Update()
    {
        transform.Rotate(0.0f, 180.0f * Time.deltaTime, 0.0f);    
    }


    public override void Interact(PlayerController actor)
    {
        actor.EarnCoin(currencyCost);
        GameObject.Destroy(this.gameObject);
    }


}
