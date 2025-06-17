using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float floatHeight = 0.0001f; // Adjust this value to control the floating height
    [SerializeField] private float floatSpeed = 2f; // Adjust this value to control the floating speed
    private GameObject heldItem;
    public bool IsHoldingItem => heldItem != null;
    public GameObject HeldItem => heldItem;
    public void PickUp(GameObject item)
    {
        IPickable pickable = item.GetComponent<IPickable>();
        if (pickable != null)
        {
            Debug.Log("Picking up item: " + item.name);
            if (heldItem != null)
            {
                ShakeHeldItem();
                return;
            }

            heldItem = item;
            heldItem.transform.SetParent(holdPoint);
            heldItem.transform.localPosition = item.GetComponent<IPickable>().GetHandPositionOffset();
            heldItem.transform.localRotation = Quaternion.Euler(item.GetComponent<IPickable>().GetHandRotationOffset());
        }

    }

    public void ShakeHeldItem(float intensity = 0.005f, float duration = 0.3f)
    {
        Debug.Log("Shaking item: " + heldItem.name);
        StartCoroutine(ShakeCoroutine(intensity, duration));
    }

    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        Vector3 originalPosition = heldItem.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float xOffset = Random.Range(-intensity, intensity);
            float yOffset = Random.Range(-intensity, intensity);
            heldItem.transform.localPosition = originalPosition + new Vector3(xOffset, yOffset, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        heldItem.transform.localPosition = originalPosition; // Reset position after shaking
    }
    public void Remove()
    {
        if (heldItem != null)
        {
            Destroy(heldItem);
            heldItem.transform.SetParent(null);
            heldItem = null;
        }
    }

    public void Drop()
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            heldItem = null;
        }
    }
    public void MoveItemUpDown()
    {
        if (heldItem != null)
        {
            Vector3 newPosition = heldItem.transform.localPosition;
            newPosition.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight; // Adjust the multiplier for height
            heldItem.transform.localPosition = newPosition;
        }
    }



}
