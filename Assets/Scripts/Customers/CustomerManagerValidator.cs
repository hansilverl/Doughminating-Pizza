using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(CustomerManager))]
public class CustomerManagerValidator : Editor
{
    public override void OnInspectorGUI()
    {
        CustomerManager manager = (CustomerManager)target;
        
        EditorGUILayout.LabelField("Validation Status", EditorStyles.boldLabel);
        
        // Check for null references
        if (manager.entryDoor == null)
        {
            EditorGUILayout.HelpBox("Entry Door is not assigned!", MessageType.Error);
        }
        
        if (manager.exitDoor == null)
        {
            EditorGUILayout.HelpBox("Exit Door is not assigned!", MessageType.Error);
        }
        
        if (manager.customerPrefab == null)
        {
            EditorGUILayout.HelpBox("Customer Prefab is not assigned!", MessageType.Error);
        }
        
        // Check seat transforms
        SerializedProperty seatTransforms = serializedObject.FindProperty("seatTransforms");
        if (seatTransforms != null && seatTransforms.arraySize == 0)
        {
            EditorGUILayout.HelpBox("No seat transforms assigned. They will be auto-found at runtime.", MessageType.Warning);
        }
        
        EditorGUILayout.Space();
        
        // Draw default inspector
        DrawDefaultInspector();
        
        EditorGUILayout.Space();
        
        // Runtime info
        if (Application.isPlaying)
        {
            EditorGUILayout.LabelField("Runtime Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Completed Orders: {CustomerManager.GetCompletedOrders()}");
            EditorGUILayout.LabelField($"Current Customer Limit: {manager.GetCurrentCustomerLimit()}");
            
            if (manager.availableSeatForCustomers != null)
            {
                EditorGUILayout.LabelField($"Available Seats: {manager.availableSeatForCustomers.Length}");
                for (int i = 0; i < manager.availableSeatForCustomers.Length; i++)
                {
                    string status = manager.availableSeatForCustomers[i] ? "Free" : "Occupied";
                    EditorGUILayout.LabelField($"  Seat {i}: {status}");
                }
            }
        }
    }
}
#endif 