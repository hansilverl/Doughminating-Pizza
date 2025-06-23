using UnityEngine;
using TMPro;
using System.Collections;

public class RestaurantGameManager : MonoBehaviour
{
    [Header("Day System")]
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private float dayDuration = 60f; // Seconds per day (60 seconds = 1 minute)
    
    [Header("Difficulty Levels")]
    [SerializeField] private int daysPerLevel = 7;  // Days before difficulty increases
    [SerializeField] private int maxLevel = 5;      // Maximum difficulty level
    
    // Game state
    private static int currentDay = 0;
    private static int currentLevel = 1;
    private float dayTimer = 0f;
    private bool gameStarted = false;
    
    // Singleton pattern
    public static RestaurantGameManager Instance { get; private set; }
    
    // Events for other systems to subscribe to
    public static System.Action<int> OnDayChanged;
    public static System.Action<int, int> OnLevelChanged; // (oldLevel, newLevel)
    
    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Auto-find day text if not assigned
        if (dayText == null)
        {
            FindDayText();
        }
    }
    
    void Start()
    {
        StartGame();
    }
    
    void Update()
    {
        if (!gameStarted) return;
        
        // Update day timer
        dayTimer += Time.deltaTime;
        
        // Check if a day has passed
        if (dayTimer >= dayDuration)
        {
            dayTimer = 0f;
            AdvanceDay();
        }
    }
    
    /// <summary>
    /// Start the restaurant game
    /// </summary>
    public void StartGame()
    {
        if (gameStarted) return;
        
        gameStarted = true;
        currentDay = 0;  // Start from day 0
        currentLevel = 1;
        dayTimer = 0f;
        
        UpdateDayDisplay();
        
        Debug.Log("Restaurant Game Started! Day 0, Level 1");
    }
    
    /// <summary>
    /// Advance to the next day
    /// </summary>
    private void AdvanceDay()
    {
        currentDay++;
        
        // Check if we need to increase difficulty level
        int newLevel = Mathf.Min(((currentDay - 1) / daysPerLevel) + 1, maxLevel);
        
        if (newLevel != currentLevel)
        {
            int oldLevel = currentLevel;
            currentLevel = newLevel;
            OnLevelChanged?.Invoke(oldLevel, currentLevel);
            Debug.Log($"Level increased from {oldLevel} to {currentLevel}!");
        }
        
        // Update UI
        UpdateDayDisplay();
        
        // Notify other systems
        OnDayChanged?.Invoke(currentDay);
        
        Debug.Log($"Day {currentDay} started (Level {currentLevel})");
    }
    
    /// <summary>
    /// Update the day display in UI
    /// </summary>
    private void UpdateDayDisplay()
    {
        if (dayText != null)
        {
            dayText.text = currentDay.ToString();
        }
    }
    
    /// <summary>
    /// Auto-find the day text component in the scene
    /// </summary>
    private void FindDayText()
    {
        // Look for GameObject named "Day" with a child "Text (TMP)"
        GameObject dayObject = GameObject.Find("Day");
        if (dayObject != null)
        {
            // Look for Text (TMP) component in children
            TextMeshProUGUI[] textComponents = dayObject.GetComponentsInChildren<TextMeshProUGUI>();
            if (textComponents.Length > 0)
            {
                dayText = textComponents[0];
                Debug.Log("Auto-found Day Text component");
            }
            else
            {
                Debug.LogWarning("Found Day object but no TextMeshProUGUI component in children");
            }
        }
        else
        {
            Debug.LogWarning("Could not find Day object in scene. Please assign dayText manually.");
        }
    }
    
    /// <summary>
    /// Get current day (0-based)
    /// </summary>
    public static int GetCurrentDay() => currentDay;
    
    /// <summary>
    /// Get current difficulty level (1-based)
    /// </summary>
    public static int GetCurrentLevel() => currentLevel;
    
    /// <summary>
    /// Get progress through current day (0.0 to 1.0)
    /// </summary>
    public float GetDayProgress()
    {
        return gameStarted ? (dayTimer / dayDuration) : 0f;
    }
    
    /// <summary>
    /// Check if game has started
    /// </summary>
    public static bool IsGameStarted() => Instance != null && Instance.gameStarted;
    
    /// <summary>
    /// Reset the game state (for debugging or restart)
    /// </summary>
    public void ResetGame()
    {
        gameStarted = false;
        currentDay = 0;
        currentLevel = 1;
        dayTimer = 0f;
        UpdateDayDisplay();
        Debug.Log("Restaurant Game Reset");
    }
    
    /// <summary>
    /// Get difficulty multiplier based on current level
    /// </summary>
    public static float GetDifficultyMultiplier()
    {
        return 1f + (currentLevel - 1) * 0.2f; // Each level increases difficulty by 20%
    }
    
    /// <summary>
    /// Get level name/description
    /// </summary>
    public static string GetLevelName()
    {
        return currentLevel switch
        {
            1 => "Beginner",
            2 => "Easy",
            3 => "Normal",
            4 => "Hard",
            5 => "Expert",
            _ => "Unknown"
        };
    }
    
    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
} 