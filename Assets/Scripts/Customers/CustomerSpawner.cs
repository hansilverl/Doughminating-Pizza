using UnityEngine;
using UnityEngine.AI;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;         // Префаб покупателя
    public Transform spawnPoint;              // Где появится покупатель
    public Transform orderPoint;              // Куда он пойдёт

    void Start()
    {
        SpawnCustomer();
    }

    void SpawnCustomer()
    {
        GameObject customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.SetDestination(orderPoint.position);
        }
    }
}