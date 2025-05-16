using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VectorGraphics;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    //***************************************************************************//
    // This class manages all things related to a customer, including
    // wishlist, ingredients, patience and animations.
    //***************************************************************************//
    
    public Transform entryPoint;    // из чего спавнимся
    public Transform exitPoint;     // куда уходим

    // Modifiable variables (Only Through Inspector - Do not hardcode!)
    public float customerPatience = 30.0f;              // seconds (default = 30)
    public float customerMoveSpeed = 3f;
    public bool showProductIngredientHelpers = true;    // whether to show customer wish's ingredients as a helper or not

    // Audio Clips
    public AudioClip orderIsOkSfx;
    public AudioClip orderIsNotOkSfx;
    public AudioClip receivedSomethingGoodSfx;

    // Customer Moods
    /* Currently we have 4 moods: 
     [0] Default
     [1] Bored 
     [2] Satisfied
     [3] Angry
     we change to appropriate material whenever needed. 
    */
    public Material[] customerMoods;
    private int moodIndex;

    // UI Elements
    public GameObject patienceBarFG;
    public GameObject patienceBarBG;
    
    private float barInitialScaleX;
    private Vector3 barInitialPos;
    
    public GameObject requestBubble;
    public GameObject money3dText;
    
    public int mySeat;
    public Vector3 destination;
    private CustomerManager manager;


    
    [SerializeField] private Transform iconRowTransform;

    // Private customer variables
    private string customerName;                // random name
    private float currentCustomerPatience;      // current patience of the customer
    private bool isOnSeat;                      // is customer on his seat?
    private bool isLeaving;                     // is customer leaving?
    private float creationTime;                 // when was this customer created
    private Vector3 startingPosition;           // reference position
    private List<System.Type> wantedIngredients = new();  // what ingredients does this customer want?
    private GameObject[] ingredientIcons;       // visual representation of wanted ingredients
    private bool patienceBarSliderFlag;         // flag to control patience bar animation

    // Available ingredient types
    private static readonly System.Type[] AvailableIngredients = new System.Type[]
    {
        typeof(Sauce),
        typeof(Cheese),
        typeof(Bacon),
        typeof(Pineapple),
        typeof(Pepperoni)
    };

    void Awake()
    {
        manager = FindObjectOfType<CustomerManager>();
        
        startingPosition = entryPoint != null 
            ? entryPoint.position 
            : transform.position;
        transform.position = startingPosition;

        // Make sure the customer has the correct tag for door triggers
        gameObject.tag = "Customer";

        requestBubble.SetActive(false);
        patienceBarFG.SetActive(false);
        patienceBarBG.SetActive(false);
        
        
        if (isOnSeat)
        {
            patienceBarFG.SetActive(true);
            patienceBarBG.SetActive(true);
            patienceBarSliderFlag = true;
        }
        
        isLeaving = false;
        
        currentCustomerPatience = customerPatience;
        creationTime = Time.time;
        moodIndex = 0;
        
        barInitialScaleX = patienceBarBG.transform.localScale.x;
        barInitialPos = patienceBarBG.transform.localPosition;


        Init();
        StartCoroutine(goToSeat());
    }

    //***************************************************************************//
    // Initialize all customer related variables
    //***************************************************************************//
    void Init()
    {
        // Give this customer a random name
        customerName = "Customer_" + Random.Range(100, 10000);
        gameObject.name = customerName;

        // Randomly choose ingredients for this customer
        // Always include sauce and cheese as base ingredients
        wantedIngredients.Add(typeof(Sauce));
        wantedIngredients.Add(typeof(Cheese));

        // Add 0-2 random toppings
        int numToppings = Random.Range(0, 3); // 0, 1, or 2 toppings
        List<System.Type> availableToppings = new List<System.Type>
        {
            typeof(Bacon),
            typeof(Pineapple),
            typeof(Pepperoni)
        };

        for (int i = 0; i < numToppings; i++)
        {
            if (availableToppings.Count > 0)
            {
                int randomIndex = Random.Range(0, availableToppings.Count);
                wantedIngredients.Add(availableToppings[randomIndex]);
                availableToppings.RemoveAt(randomIndex); // Remove to avoid duplicates
            }
        }

        // Create visual representation of wanted ingredients
        ingredientIcons = new GameObject[wantedIngredients.Count];
        for (int i = 0; i < wantedIngredients.Count; i++)
        {
            if (iconRowTransform != null)
            {
                string iconName = wantedIngredients[i].Name.ToLower();
                Sprite iconSprite = Resources.Load<Sprite>($"UI/Icons/{iconName}");

                if (iconSprite != null)
                {
                    GameObject iconGO = new GameObject("Icon_" + iconName);
                    iconGO.transform.SetParent(iconRowTransform, false);

                    Image img = iconGO.AddComponent<Image>();
                    img.sprite = iconSprite;
                    img.rectTransform.sizeDelta = new Vector2(1, 1); 

                    ingredientIcons[i] = iconGO;

                    if (!showProductIngredientHelpers)
                        img.enabled = false;
                }
                else
                {
                    Debug.LogWarning($"Icon sprite not found: {iconName}");
                }
            }
        }
    }


    //***************************************************************************//
    // After this customer has been instantiated, it starts somewhere outside game scene
    // and then goes to its position (seat) with a nice animation.
    //***************************************************************************//
    private float timeVariance;
    
    IEnumerator goToSeat()
    {
        // Пока не подошли к «стулу»
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                customerMoveSpeed * Time.deltaTime
            );
            yield return null;
        }
        // Как только сели — запускаем UI и выходим
        isOnSeat              = true;
        patienceBarSliderFlag = true;
        requestBubble.SetActive(true);
        patienceBarFG.SetActive(true);
        patienceBarBG.SetActive(true);
    }

    void Update()
    {
        if (patienceBarSliderFlag)
            StartCoroutine(patienceBar());

        // Manage customer's mood by changing its material
        // updateCustomerMood();
    }

    //***************************************************************************//
    // Make the customer react to events by changing its material (and texture)
    //***************************************************************************//
    void updateCustomerMood()
    {
        if (!isLeaving)
        {
            if (currentCustomerPatience <= customerPatience / 2)
                moodIndex = 1;  // Bored
            else
                moodIndex = 0;  // Default
        }

        GetComponent<Renderer>().material = customerMoods[moodIndex];
    }

    //***************************************************************************//
    // Show and animate progress bar based on customer's patience
    //***************************************************************************//
    IEnumerator patienceBar()
    {
        patienceBarSliderFlag = false;

        while (currentCustomerPatience > 0)
        {
            currentCustomerPatience -= Time.deltaTime;
            float ratio = Mathf.Clamp01(currentCustomerPatience / customerPatience);

            // Вычисляем новый scale X
            float newScaleX = barInitialScaleX * ratio;

            // Применяем scale к BG (шторке)
            patienceBarBG.transform.localScale = new Vector3(
                newScaleX,
                patienceBarBG.transform.localScale.y,
                patienceBarBG.transform.localScale.z
            );

            // Сдвигаем шторку вправо, чтобы правый край оставался на месте
            float delta = (newScaleX - barInitialScaleX) * 0.5f;
            patienceBarBG.transform.localPosition = new Vector3(
                barInitialPos.x - delta,
                barInitialPos.y,
                barInitialPos.z
            );

            yield return null;
        }

        patienceBarBG.SetActive(false);
        StartCoroutine(leave());
    }




    //***************************************************************************//
    // Fill customer's patience bar and make it full again
    //***************************************************************************//
    void fillCustomerPatience()
    {
        currentCustomerPatience = customerPatience;
        patienceBarFG.SetActive(true);
        // сброс масштаба в полный размер
        patienceBarFG.transform.localScale = new Vector3(1f,
            patienceBarFG.transform.localScale.y,
            patienceBarFG.transform.localScale.z);
    }

    //***************************************************************************//
    // Check if the delivered pizza matches what the customer wants
    //***************************************************************************//
    public void TryGivePizza(GameObject obj)
    {
        if (isLeaving) return;

        Pizza pizza = obj.GetComponent<Pizza>();
        if (pizza == null)
        {
            OrderIsIncorrect();
            return;
        }

        // Check if pizza has all required ingredients
        bool hasAll = true;
        foreach (System.Type ingType in wantedIngredients)
        {
            if (ingType == typeof(Sauce) && !pizza.HasSauce) hasAll = false;
            if (ingType == typeof(Cheese) && !pizza.HasCheese) hasAll = false;
            if (ingType == typeof(Bacon) && !pizza.HasBacon) hasAll = false;
            if (ingType == typeof(Pineapple) && !pizza.HasPineapple) hasAll = false;
            if (ingType == typeof(Pepperoni) && !pizza.HasPepperoni) hasAll = false;
        }

        // Also check that pizza doesn't have any unwanted ingredients
        if (hasAll)
        {
            if (!wantedIngredients.Contains(typeof(Bacon)) && pizza.HasBacon) hasAll = false;
            if (!wantedIngredients.Contains(typeof(Pineapple)) && pizza.HasPineapple) hasAll = false;
            if (!wantedIngredients.Contains(typeof(Pepperoni)) && pizza.HasPepperoni) hasAll = false;
        }

        if (hasAll)
            orderIsCorrect();
        else
            OrderIsIncorrect();
    }

    //***************************************************************************//
    // If order is delivered correctly
    //***************************************************************************//
    void orderIsCorrect()
    {
        Debug.Log($"{customerName}: Order is correct!");
        moodIndex = 2;  // Satisfied
        GetComponent<Renderer>().material = customerMoods[moodIndex];
        playSfx(orderIsOkSfx);
        InstantiateMoney();
        StartCoroutine(leave());
    }

    //***************************************************************************//
    // If order is NOT delivered correctly
    //***************************************************************************//
    void OrderIsIncorrect()
    {
        Debug.Log($"{customerName}: Order is not correct!");
        moodIndex = 3;  // Angry
        GetComponent<Renderer>().material = customerMoods[moodIndex];
        playSfx(orderIsNotOkSfx);
        StartCoroutine(leave());
    }

    //***************************************************************************//
    // Show money earned
    //***************************************************************************//
    void InstantiateMoney()
    {
        float leaveTime = Time.time;
        int remainedPatienceBonus = (int)Mathf.Round(customerPatience - (leaveTime - creationTime));
        int finalMoney = 5 + remainedPatienceBonus; // Base price + patience bonus

        GameObject money = Instantiate(money3dText, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        money.GetComponent<TextMesh>().text = $"${finalMoney}";
    }

    //***************************************************************************//
    // Leave routine with animations
    //***************************************************************************//
    IEnumerator leave()
    {
        manager.availableSeatForCustomers[mySeat] = true;

        if (isLeaving) yield break;
        isLeaving = true;

        // анимации закрытия UI…
        StartCoroutine(animate(Time.time, patienceBarBG, 0.7f, 0.8f));
        yield return new WaitForSeconds(0.3f);  
        StartCoroutine(animate(Time.time, requestBubble, 0.75f, 0.95f));
        yield return new WaitForSeconds(0.4f);

        // First move to the door position to ensure we trigger the door collider
        Vector3 doorPosition = exitPoint != null ? exitPoint.position : transform.position + Vector3.right * 10f;
        
        // Move to the door
        while (Vector3.Distance(transform.position, doorPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                doorPosition,
                customerMoveSpeed * Time.deltaTime
            );
            yield return null;
        }
        
        // Small pause at the door to ensure trigger detection
        yield return new WaitForSeconds(0.2f);

        // Then move fully outside (off-screen)
        Vector3 offScreenPosition = doorPosition + ((doorPosition - transform.position).normalized * 10f);
        
        while (Vector3.Distance(transform.position, offScreenPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                offScreenPosition,
                customerMoveSpeed * Time.deltaTime
            );
            yield return null;
        }

        Destroy(gameObject);
    }


    //***************************************************************************//
    // Animate customer UI elements
    //***************************************************************************//
    IEnumerator animate(float _time, GameObject _go, float _in, float _out)
    {
        float t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime * 10;
            _go.transform.localScale = new Vector3(
                Mathf.SmoothStep(_in, _out, t),
                _go.transform.localScale.y,
                _go.transform.localScale.z
            );
            yield return 0;
        }
        float r = 0.0f;
        if (_go.transform.localScale.x >= _out)
        {
            while (r <= 1.0f)
            {
                r += Time.deltaTime * 2;
                _go.transform.localScale = new Vector3(
                    Mathf.SmoothStep(_out, 0.01f, r),
                    _go.transform.localScale.y,
                    _go.transform.localScale.z
                );
                if (_go.transform.localScale.x <= 0.01f)
                    _go.SetActive(false);
                yield return 0;
            }
        }
    }

    //***************************************************************************//
    // Play AudioClips
    //***************************************************************************//
    void playSfx(AudioClip _sfx)
    {
        if (_sfx == null) return;
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
        {
            audio.clip = _sfx;
            audio.Play();
        }
    }
}