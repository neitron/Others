using UnityEngine;



public abstract class Interactable : MonoBehaviour
{


    public float radius = 3.0f;


    public abstract void Interact(PlayerController actor);


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }


}
