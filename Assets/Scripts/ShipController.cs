using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

    public GameObject laser;

    private GameObject cannonPlanet;

    public int health;
    private float rechargeRate;

    private bool canFire;
    private float fireRange;
    private float distance;
    private float randNo;

	// Use this for initialization
	void Start ()
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        Collider2D col = GetComponent<Collider2D>();
        rb2d.isKinematic = true;
        col.isTrigger = true;
        Physics2D.IgnoreLayerCollision(8, 9, true);
        InitialiseStats();
        fireRange = 45f;
        cannonPlanet = GetComponentInParent<ShipClusterController>().GetCannonPlanet();
        canFire = true;
	}

    void FireLaser()
    {
        Instantiate(laser, transform.position, transform.rotation);
        Invoke("ShootDelay", rechargeRate);
    }

    private void ShootDelay()
    {
        canFire = true;
    }

    void InitialiseStats()
    {
        if (this.gameObject.tag == "Ship")
        {
            health = 1;
            rechargeRate = 2f;
        }
        else if (this.gameObject.tag == "Large Ship")
        {
            health = 4;
            rechargeRate = 5f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SpriteRenderer>().sortingLayerName == "Comet")
        {
            health = health - 1;
            other.GetComponent<cometController>().IncreaseNoOfHits();
            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        }     
    }
	
	// Update is called once per frame
	void Update ()
    {
        distance = Mathf.Abs(Vector2.Distance(transform.position, cannonPlanet.transform.position));
        if (distance <= fireRange && canFire == true)
        {
            randNo = Random.Range(0f, 1f);
            canFire = false;
            Invoke("FireLaser", randNo);
        }
	}
}
