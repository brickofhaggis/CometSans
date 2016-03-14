using UnityEngine;
using System.Collections;

public class ShipClusterController : MonoBehaviour {

    public GameObject cannonPlanet;
    public static float speed = 7.5F;

    private float distance;
    private float maxDistance = 15f;
    private int noOfShips = 1; //initialise to one

    // Use this for initialization
    void Start ()
    {
        speed = 7.5f;
    }

    /*public void InitialiseNoOfShips(int number)
    {
        noOfShips = number;
    }
    
    public void ReduceNoOfShips()
    {
        noOfShips = noOfShips - 1;
        if (noOfShips <= 0)
        {
            Destroy(this.gameObject);
        }
    }*/

    public GameObject GetCannonPlanet()
    {
        return cannonPlanet;
    }
	
	// Update is called once per frame
	void Update ()
    {
        distance = Vector2.Distance(transform.position, cannonPlanet.transform.position);
        if (distance > maxDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, cannonPlanet.transform.position, Time.deltaTime * speed);
        }
    }
}
