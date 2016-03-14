using UnityEngine;
using System.Collections;

public class CPController : MonoBehaviour {

    public int health;

	// Use this for initialization
	void Awake ()
    {
        health = 200;
	}

    public void ReduceHealth(int damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ship Laser" || other.tag == "Large Ship Laser")
        {
            LazerController laserScript = other.gameObject.GetComponent<LazerController>();
            ReduceHealth(laserScript.GetDamage());
        }
    }

	// Update is called once per frame
	void Update ()
    {
	    
	}
}
