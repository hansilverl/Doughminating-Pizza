using UnityEngine;
using TMPro;

public class OrderStatsUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI ordersText;
    [SerializeField] private TextMeshProUGUI customersText;
    
    [Header("Settings")]
    [SerializeField] private float updateInterval = 0.5f; // Update every 0.5 seconds
    
    private float lastUpdateTime;
    
    void Start()
    {
        // If not assigned in Inspector, try to find automatically
        if (ordersText == null)
        {
            GameObject ordersTextObj = GameObject.Find("OrdersText");
            if (ordersTextObj != null)
                ordersText = ordersTextObj.GetComponent<TextMeshProUGUI>();
        }
        
        if (customersText == null)
        {
            GameObject customersTextObj = GameObject.Find("CustomersText");
            if (customersTextObj != null)
                customersText = customersTextObj.GetComponent<TextMeshProUGUI>();
        }
        
        UpdateUI();
    }
    
    void Update()
    {
        // Update UI with specified interval for performance
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateUI();
            lastUpdateTime = Time.time;
        }
    }
    
    private void UpdateUI()
    {
        // Update orders text
        if (ordersText != null)
        {
            int completedOrders = CustomerManager.GetCompletedOrders();
            ordersText.text = $"Orders: {completedOrders}";
        }
        
        // Update customers text
        if (customersText != null && CustomerManager.Instance != null)
        {
            int currentLimit = CustomerManager.Instance.GetCurrentCustomerLimit();
            int currentCount = GetCurrentCustomerCount();
            customersText.text = $"Customers: {currentCount}/{currentLimit}";
        }
    }
    
    // Get current number of customers (copy of method from CustomerManager for UI)
    private int GetCurrentCustomerCount()
    {
        if (CustomerManager.Instance == null) return 0;
        
        int count = 0;
        bool[] seats = CustomerManager.Instance.availableSeatForCustomers;
        
        if (seats != null)
        {
            for (int i = 0; i < seats.Length; i++)
            {
                if (!seats[i]) // The place is taken
                {
                    count++;
                }
            }
        }
        
        return count;
    }
    
    // Public method to force UI refresh (can be called from other scripts)
    public void ForceUpdateUI()
    {
        UpdateUI();
    }
} 