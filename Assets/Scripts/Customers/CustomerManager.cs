using System.Collections;
using UnityEngine;

public class CustomerManager : MonoBehaviour   // class and file with the same name
{
    [Header("Doors")]
    public Transform entryDoor;    // people come here
    public Transform exitDoor;     // leave from here
    
    [Header("Seats & Spawning")]
    public Transform[] seatTransforms;         // "chairs" points
    public GameObject customerPrefab;          // prefab with CustomerController
    public float spawnXOffset = -10f;
    public float spawnInterval = 1f;

    [HideInInspector]
    public bool[] availableSeatForCustomers;

    void Awake()
    {
        availableSeatForCustomers = new bool[seatTransforms.Length];
        for (int i = 0; i < availableSeatForCustomers.Length; i++)
            availableSeatForCustomers[i] = true;
    }

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // find the _first_ free space
            for (int i = 0; i < seatTransforms.Length; i++)
            {
                if (availableSeatForCustomers[i])
                {
                    SpawnCustomerAtSeat(i);
                    // after spawning we wait exactly as long as specified in the inspector
                    yield return new WaitForSeconds(spawnInterval);
                    // break out of the loop to spawn only one at a time
                    break;
                }
            }
            // immediately on the next iteration we look for free space again
            yield return null;
        }
    }

    void SpawnCustomerAtSeat(int seatIndex)
    {
        Vector3 spawnPos = entryDoor.position;

        GameObject go = Instantiate(customerPrefab, spawnPos, Quaternion.identity);
        var ctrl = go.GetComponent<CustomerController>();
        ctrl.mySeat      = seatIndex;
        ctrl.destination = seatTransforms[seatIndex].position;
        // we pass the doors
        ctrl.entryPoint  = entryDoor;
        ctrl.exitPoint   = exitDoor;

        availableSeatForCustomers[seatIndex] = false;
    }
}