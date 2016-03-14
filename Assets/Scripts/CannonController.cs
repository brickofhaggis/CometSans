using UnityEngine;
using System.Collections;

public class CannonController : MonoBehaviour {

    public GameObject[] cometType; //comet firing options
    public GameObject clusterFrag; 
    public Transform firePoint;
    public static bool canFire;
    public Vector2 cometPosition;

    private float cannonRotation;
    private int frameCount = 0;
    private float rotationSpeed = 1f;

    private float minRotation = 300f; // angles between 360 and 300, down
    private float maxRotation = 60f; // angles between 0 and 60, up
    private int lastDirection = 0;
    private float fireDelay;

    private const int maxClusterComets = 1; //max no of cluster comets on screen at any time

    private GameObject comet; //selected comet

    // Use this for initialization
    void Start()
    {
        cometController.clusterCometNo = 0; //initialise number of cluster comets to zero
        comet = cometType[0]; //default comet set
        canFire = true;
        fireDelay = 3.5f;
    }

    void FireComet()
    {
        if (!(comet.tag == "Cluster Comet" && cometController.clusterCometNo >= maxClusterComets)) //can't fire if too many cluster comets at once
        {
            Instantiate(comet, firePoint.position, firePoint.rotation);
            canFire = false;
            Invoke("ShootDelay", fireDelay);
        }
    }

    public IEnumerator SpawnFragments(Transform parentPos, GameObject parentComet)
    {
        int fragCount = 0;
        GameObject temp;
        //Vector2 spawnPosition;
        float parentRadius;
        float parentAngle;
        float fragAngle;
        float[] angs = new float[6];
        Transform[] positions = new Transform[6];
        float randAng;
        //float maxAng = 15f;
        Quaternion fragRotation;

        temp = parentComet;

        if (parentComet.tag == "Cluster Comet" || parentComet.tag == "Comet")
        {
            comet = clusterFrag; //cluster frag
            if (parentComet.tag == "Cluster Comet")
            {
                fragCount = 6;
            }
            else
            {
                fragCount = 2;
            }
        }
        else if (parentComet.tag == "Big Comet")
        {
            comet = cometType[0]; //default comet
            fragCount = 2;
        }

        parentRadius = parentComet.transform.lossyScale.x / 2;
        parentAngle = Quaternion.Angle(Quaternion.identity, parentPos.rotation);

        angs[0] = 1.25f;
        angs[1] = -1.25f;
        angs[2] = 5f;
        angs[3] = -5f;
        angs[4] = 7.5f;
        angs[5] = -7.5f;

        /*positions[0] = new Vector2(parentPos.position.x + (parentRadius / 2), parentPos.position.y + (parentRadius / 4));
        positions[1] = new Vector2(parentPos.position.x + (parentRadius / 2), parentPos.position.y - (parentRadius / 4));
        positions[2] = new Vector2(parentPos.position.x, parentPos.position.y + parentRadius);
        positions[3] = new Vector2(parentPos.position.x, parentPos.position.y - parentRadius);
        positions[4] = new Vector2(parentPos.position.x - (parentRadius / 2), parentPos.position.y + (parentRadius * 3 / 2));
        positions[5] = new Vector2(parentPos.position.x - (parentRadius / 2), parentPos.position.y - (parentRadius * 3 / 2));*/


        for (int i = 0; i < fragCount; i++) //spawns a number of fragments at slightly varied positions
        {
            positions[i] = parentComet.GetComponent<cometController>().GetShatterPos(i);
            //randAng = (float)Random.Range(-maxAng, maxAng);
            randAng = angs[i];
            fragAngle = parentAngle + randAng;
            fragRotation = Quaternion.Euler(0, 0, fragAngle);


            //spawnPosition = new Vector2((float)Random.Range(-parentRadius, parentRadius) + parentPos.position.x, (float)Random.Range(-parentRadius, parentRadius) + parentPos.position.y);
            //spawnPosition = positions[i];
            //Instantiate(comet, spawnPosition, fragRotation);
            //Instantiate(comet, parentPos.position, fragRotation);


            Instantiate(comet, positions[i].position, positions[i].rotation);
        }

        comet = temp; //returns comet to original so that it's still selected for firing

        yield return null;
    }

    private void ShootDelay()
    {
        canFire = true;
    }

    bool TestRotation(int direction) //tests if cannon is rotated at max bounds
    {
        bool canRotate;

        cannonRotation = transform.rotation.eulerAngles.z;

        if (cannonRotation >= minRotation || cannonRotation <= maxRotation) //if within correct bounds
        {
            canRotate = true;
        }
        else
        {
            canRotate = false;
        }

        if (canRotate == false && lastDirection != direction)
        {
            canRotate = true;
        }

        lastDirection = direction;

        return canRotate;
    }

    void RotateCannon()
    {
        int direction = 0; //initialise to 0

        frameCount++;

        if (Input.GetKey(KeyCode.UpArrow)) //keyboard controls
        {
            direction = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = -1;
        }
        if (TestRotation(direction) && frameCount >= 5) //only adjust aim every 5 frames so player can tap keys for precise aiming
        {
            transform.Rotate(new Vector3(0, 0, direction * rotationSpeed));
            if (direction != 0)
            {
                frameCount = 0;
            }
        }

        /*Vector3 mousePos; //mouse controls
        Vector3 cannonPos;
        float angle;

        mousePos = Input.mousePosition;
        cannonPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - cannonPos.x;
        mousePos.y = mousePos.y - cannonPos.y;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);*/
    }

    void ChangeComet() //use number keys to select comet type
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            comet = cometType[0]; //comet
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            comet = cometType[1]; //big comet
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            comet = cometType[2]; //small comet
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            comet = cometType[3]; //drill comet
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            comet = cometType[4]; //cluster comet
        }
    }

    // Calls the fire method when holding down ctrl or mouse
    void Update()
    {
        RotateCannon();
        if (Input.GetButtonDown("Fire1") && canFire == true)
        {
            FireComet();
        }
        ChangeComet();
    }
}
