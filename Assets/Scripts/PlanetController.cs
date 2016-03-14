using UnityEngine;
using System.Collections;

public class PlanetController : MonoBehaviour {

    public GameObject[] shipClustArray;
    public static bool isIncreased;

    private int increaseNo;
    private int spawnWhenNo; //turn number before ship cluster spawns
    private int turnNo;
    public int clusterNo;
    private int clusterIncreaseRate;
    private int noOfClusters; //in the array
    private int maxClustNo;
    private int noOfShips;

    private GameObject shipClust;

    // Use this for initialization
    void Start ()
    {
        turnNo = -1;
        increaseNo = 1;
        spawnWhenNo = 5;
        isIncreased = false;
        clusterNo = -1;
        clusterIncreaseRate = 1;
        noOfClusters = 9;
        maxClustNo = noOfClusters;
        shipClust = shipClustArray[0];
	}

    public void IncreaseTurnNo()
    {
        turnNo = turnNo + increaseNo;
        if (turnNo >= spawnWhenNo)
        {
            clusterNo = clusterNo + clusterIncreaseRate;
            ChangeCluster();
            SpawnCluster();
            turnNo = 0;
        }
    }

    void ChangeCluster()
    {
        if (clusterNo <= 0)
        {
            shipClust = shipClustArray[0];
        }
        else if (clusterNo >= maxClustNo)
        {
            shipClust = shipClustArray[maxClustNo];
        }
        else
        {
            shipClust = shipClustArray[clusterNo];
        }
    }

    void SpawnCluster()
    {
        Instantiate(shipClust, transform.position, transform.rotation);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (CannonController.canFire && isIncreased == false)
        {
            isIncreased = true;
            IncreaseTurnNo();
        }
	}
}
