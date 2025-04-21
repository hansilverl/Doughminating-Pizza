using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class CustomerController : MonoBehaviour
{
    public GameObject orderBubble;                 // UI-пузырёк
    public TextMeshProUGUI orderText;              // Текст с названием пиццы

    private NavMeshAgent agent;
    private bool hasShownOrder = false;

    private string[] pizzaNames = { "Margherita", "Mushroom", "Pepperoni", "Four Cheese" };
    private int customerPizzaID;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        orderBubble.SetActive(false); // Пузырёк скрыт в начале
    }

    void Update()
    {
        // Проверяем: покупатель дошёл до точки назначения
        if (!hasShownOrder && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            ShowOrder();
            hasShownOrder = true;
        }
    }

    void ShowOrder()
    {
        customerPizzaID = Random.Range(0, pizzaNames.Length);
        orderText.text = "I want: " + pizzaNames[customerPizzaID];
        orderBubble.SetActive(true);
    }
}