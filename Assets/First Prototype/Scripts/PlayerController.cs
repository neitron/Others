using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    const int RMB = 1;

    public LayerMask groundMask;
    public LayerMask interactMast;

    public float attackRange;
    public float interactRange;
    public Vector2 damage;
    public int coinAmount;
    Camera cam;
    PlayerMotor motor;

	// Use this for initialization
	void Start ()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                OnRMB();
            }
        }
        else if(Input.GetMouseButtonDown(RMB))
        {
            OnRMB();
        }
	}

    private void OnRMB()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f, groundMask))
        {
            motor.MoveToPoint(hit.point);
        }

        if (Physics.Raycast(ray, out hit, 100.0f, interactMast))
        {
            var distance = Vector3.Distance(hit.collider.gameObject.transform.position, transform.position);
            if (hit.collider.gameObject.tag == "Enemy" && attackRange >= distance)
            {
                var currentDamage = UnityEngine.Random.Range((int)damage.x, (int)damage.y);
                hit.collider.gameObject.GetComponent<EnemyAI>().HitEnemy(currentDamage);
            }
            else if (hit.collider.gameObject.tag == "Coin" && interactRange >= distance)
            {
                coinAmount += 5;
                Destroy(hit.collider.gameObject);
            }
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
