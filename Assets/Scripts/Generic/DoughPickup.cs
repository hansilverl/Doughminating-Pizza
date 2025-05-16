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

        rb.useGravity = false;     // Until the dough is taken, it does not fall
        rb.isKinematic = true;     // Does not participate in physics until released
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

                Debug.Log("The dough is released and falls.");
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