using UnityEngine;
using System.Collections;

public class LazerController : MonoBehaviour {

    public GameObject cannonPlanet;

    private CPController cpScript;

    private int damage;

    private float lasSpeed;

    private float distance;

	// Use this for initialization
	void Start ()
    {
        if (this.gameObject.tag == "Ship Laser")
        {
            damage = 10;
        }
        else if (this.gameObject.tag == "Large Ship Laser")
        {
            damage = 50;
        }
        lasSpeed = ShipClusterController.speed * 2;
	}

    public int GetDamage()
    {
        return damage;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector3.MoveTowards(transform.position, cannonPlanet.transform.position, Time.deltaTime * lasSpeed);
        distance = Mathf.Abs(Vector2.Distance(transform.position, cannonPlanet.transform.position));
        if (distance < 0.1)
        {
            GetComponent<Collider2D>().isTrigger = false;
            Destroy(this.gameObject);
        }
    }
}
