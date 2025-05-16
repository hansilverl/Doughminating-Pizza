using System.Collections;
using UnityEngine;

public class CustomerManager : MonoBehaviour   // класс и файл с одинаковым именем
{
    [Header("Doors")]
    public Transform entryDoor;    // сюда заходят
    public Transform exitDoor;     // от сюда уходят
    
    [Header("Seats & Spawning")]
    public Transform[] seatTransforms;         // 4 точки «стульев»
    public GameObject customerPrefab;          // префаб с CustomerController
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
            // найдём _первое_ свободное место
            for (int i = 0; i < seatTransforms.Length; i++)
            {
                if (availableSeatForCustomers[i])
                {
                    SpawnCustomerAtSeat(i);
                    // после спауна ждём ровно столько, сколько указано в инспекторе
                    yield return new WaitForSeconds(spawnInterval);
                    // выходим из цикла, чтобы за раз спаунить только одного
                    break;
                }
            }
            // сразу же на следующей итерации снова ищем свободное место
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
        // передаём двери
        ctrl.entryPoint  = entryDoor;
        ctrl.exitPoint   = exitDoor;

        availableSeatForCustomers[seatIndex] = false;
    }
}