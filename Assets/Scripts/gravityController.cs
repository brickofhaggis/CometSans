using UnityEngine;
using System.Collections;

public class gravityController : MonoBehaviour {

    public int health;
    public cometController cometScript;

    private float gravityScale;
    private float gravityStrength;
    private float sqrMag;
    private float gravityRangeScale;

    private Rigidbody2D planet;
    private GameObject comet;
    private float distance;
    private float planetRadius;

    private bool drilling;
    private Collider2D planetCol;

    private bool gravMethod;
    private bool hasBeenDrilled;

	// Use this for initialization
	void Start ()
    {
        hasBeenDrilled = false;
        gravMethod = true;
        planetCol = GetComponent<Collider2D>();
        drilling = false;
        gravityScale = 10f;
        planet = GetComponent<Rigidbody2D>();
        planet.mass = planet.transform.lossyScale.x; //mass becomes proportional to scale //put in planet class later // override for some types
        gravityStrength = planet.mass * gravityScale;
        planetRadius = planet.transform.lossyScale.x / 2;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void FixedUpdate()
    {

    }

    Vector2 CalculateDirection(Collider2D other)
    {
        Vector2 direction;
        if (planet.tag == "Magnetic Planet")
        {
            direction = other.transform.position - transform.position;
        }
        else
        {
            direction = -(other.transform.position - transform.position);
        }
        sqrMag = direction.sqrMagnitude;

        return direction;
    }

    float CalculateMagnitude()
    {
        float magnitude;

        if (gravMethod == true)
        {
            magnitude = (planet.mass * gravityStrength) / sqrMag;
        }
        else
        {
            magnitude = planet.mass * 2;
        }

        return magnitude;
    }

    IEnumerator OnTriggerStay2D(Collider2D other)
    {
        if (planet.tag != "Bounce Planet" && other.gameObject.tag != "Moon")
        {
            comet = other.gameObject;
            cometScript = comet.GetComponent<cometController>();
            StartCoroutine(UpdateDistance(comet));
            if (drilling == true) //drilling only true when drill comet selected and hasn't drilled
            {
                planetCol.isTrigger = true;
                gravMethod = false;
            }
            else
            {
                planetCol.isTrigger = false;
                gravMethod = true;
            }
        }

        other.attachedRigidbody.AddForce(CalculateDirection(other).normalized * CalculateMagnitude()); //gravity

        if (planet.tag == "Comet Breaker Planet")
        {
            if (other.tag == "Comet" || other.tag == "Big Comet" || other.tag == "Cluster Comet")
            {
                comet = other.gameObject;
                cometScript = comet.GetComponent<cometController>();
                cometScript.BreakUpComet();
            }
        }
        yield return new WaitForFixedUpdate();
    }

    IEnumerator UpdateDistance(GameObject comet)
    {
        distance = Mathf.Abs(Vector3.Distance(comet.transform.position, planet.transform.position));
        if (comet.gameObject.tag == "Drill Comet" && cometScript.GetDrilled() == false)
        {
            if (distance <= (planetRadius + 0.75))
            {
                drilling = true;
                hasBeenDrilled = true; //used to prove the planet's been drilled
            }
            else if (distance > (planetRadius + 0.75))
            {
                drilling = false;
            }
        }
        yield return null;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<SpriteRenderer>().sortingLayerName == "Comet")
        {
            cometScript = other.GetComponent<cometController>();
            if (cometScript.GetDrilled() == false && hasBeenDrilled == true)
            {
                cometScript.ChangeDrilled();
                health = health - cometScript.GetDamage(); //to damage the first planet on the way out
                if (health <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
            hasBeenDrilled = false; //without this it would assume being in the grav radius counts as being drilled
            planetCol.isTrigger = false; //seems redundant at first glance but eliminates bugs
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == "Planet")
        {
            health = 0;
            Destroy(other.gameObject);
        }
        else
        {
            cometScript = other.gameObject.GetComponent<cometController>();
            if (cometScript.GetDrilled() == true)
            {
                health = health - cometScript.GetDamage();
            }
        }

        if (health <= 0)
        {
            if (planet.tag == "Moon")
            {
                GetComponent<Rigidbody2D>().isKinematic = false;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
