using UnityEngine;
using System.Collections;

public class cometController : MonoBehaviour {

    public CannonController cannonScript; 
    public static int clusterCometNo; //number of cluster comets on screen
    public static bool turnOver;
    public Transform[] shatterPositions;

    private bool drilled; //has the drill comet drilled a planet?

    private int damage = 0;

    private float speed; 
    private int turnNo; //local number of turns for the comet

    private Rigidbody2D rb2D;

    private int noOfShipHits;
    private int maxNoOfShipHits = 3;

    // initialization
    void Awake ()
    {
        drilled = true; //initialise to true
        rb2D = GetComponent<Rigidbody2D>();
        InitialiseStats();
        rb2D.AddForce(transform.right * speed, ForceMode2D.Impulse); //moves in direction of cannon
        transform.rotation = Quaternion.identity;
    }

    public Transform GetShatterPos(int number)
    {
        return shatterPositions[number];
    }

    public void IncreaseNoOfHits()
    {
        noOfShipHits++;
        if (noOfShipHits >= maxNoOfShipHits)
        {
            Destroy(this.gameObject);
        }
    }

    public bool GetDrilled() //return drilled value
    {
        return drilled;
    }

    public void ChangeDrilled() //set drilled to true
    {
        drilled = true;
    }

    public int GetDamage()
    {
        return damage;
    }

    void InitialiseStats() //initialises damage, speed and mass
    {
        turnNo = 0;
        if (this.gameObject.tag == "Comet")
        {
            damage = 10;
            speed = 10;
            rb2D.mass = 0.75f;
        }
        else if (this.gameObject.tag == "Big Comet")
        {
            damage = 20;
            speed = 12.5f;
            rb2D.mass = 1.5f;
        }
        else if (this.gameObject.tag == "Small Comet")
        {
            damage = 5;
            speed = 8.5f;
            rb2D.mass = 0.4f;
        }
        else if (this.gameObject.tag == "Drill Comet")
        {
            damage = 10;
            speed = 12.5f;
            rb2D.mass = 0.75f;
            drilled = false;
        }
        else if (this.gameObject.tag == "Cluster Comet")
        {
            damage = 20;
            speed = 12.5f;
            rb2D.mass = 1.5f;
            clusterCometNo++;
        }
        else if (this.gameObject.tag == "Cluster Fragment")
        {
            damage = 1;
            speed = 4.25f;
            rb2D.mass = 0.3f;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (drilled == true && other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == "Planet" && other.gameObject.tag != "Bounce Planet")
        { //destroy comet if it comes in contact with a planet except for bounce planets
            if (this.tag == "Cluster Comet")
            {
                clusterCometNo--;
            }
            Destroy(this.gameObject);
        }
    }

    void TestDistance()
    {
        int maxTurn = 10;
        turnNo++;

        if (turnNo > maxTurn) //if turn length exceeds max turn, destroy comet
        {
            if (this.tag == "Cluster Comet")
            {
                clusterCometNo--;
            }
            Destroy(this.gameObject);
        }
    }

    public void BreakUpComet() //destroys comet and starts procedure to spawn fragments
    {
        StartCoroutine(cannonScript.SpawnFragments(this.transform, this.gameObject));
        if (this.tag == "Cluster Comet")
        {
            clusterCometNo--;
        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
        if (CannonController.canFire == true)
        {
            Time.timeScale = 0.0001f;
            if (turnOver == true)
            {
                PlanetController.isIncreased = false;
                TestDistance();
            }
            turnOver = false;
        }
        else
        {
            Time.timeScale = 1f;
            turnOver = true;
        }
        if (Input.GetKeyDown(KeyCode.D) && this.gameObject.tag == "Cluster Comet")
        {
            BreakUpComet();
        }
        transform.right = rb2D.velocity;
	}
}
