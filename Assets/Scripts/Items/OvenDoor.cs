using UnityEngine;

public class OvenDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private float openYAngle = -120f;  // how far the door opens (negative = left swing)
    [SerializeField] private float animationSpeed = 5f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(0f, openYAngle, 0f) * closedRotation;
    }

    public void Interact()
    {
        if (!isAnimating)
        {
            isOpen = !isOpen;
            StartCoroutine(AnimateDoor(isOpen ? openRotation : closedRotation));
        }
    }

    public string getInteractionText()
    {
        return isOpen ? "Press 'E' to close oven door" : "Press 'E' to open oven door";
    }

    private System.Collections.IEnumerator AnimateDoor(Quaternion targetRotation)
    {
        isAnimating = true;

        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * animationSpeed
            );
            yield return null;
        }

        transform.localRotation = targetRotation;
        isAnimating = false;
    }
}
