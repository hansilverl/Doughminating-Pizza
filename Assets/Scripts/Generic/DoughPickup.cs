using UnityEngine;

public class DoughPickup : MonoBehaviour
{
    public static GameObject heldDough = null;
    public float holdHeight = 1.2f;
    private bool isHeld = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;     // Пока тесто не взято – не падает
        rb.isKinematic = true;     // Не участвует в физике до отпускания
    }

    void OnMouseDown()
    {
        if (heldDough == null)
        {
            heldDough = gameObject;
            isHeld = true;

            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (isHeld)
        {
            FollowMouse();

            if (Input.GetMouseButtonUp(0))
            {
                isHeld = false;
                heldDough = null;

                rb.isKinematic = false;
                rb.useGravity = true;

                Debug.Log("Тесто отпущено и падает.");
            }
        }
    }

    void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up * holdHeight);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            transform.position = point;
        }
    }
}