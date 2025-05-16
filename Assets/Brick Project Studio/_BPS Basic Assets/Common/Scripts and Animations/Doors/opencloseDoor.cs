using System.Collections;
using UnityEngine;

namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public float closeDelay = 0.5f;  // delay before closing
        
        private Coroutine closeRoutine;

        void OnTriggerEnter(Collider other)
        {
            // If a client entered the trigger
            if (other.CompareTag("Customer"))
            {
                // Open the door immediately
                openandclose.Play("Opening");
                // Cancel the scheduled closure if there was one
                if (closeRoutine != null)
                    StopCoroutine(closeRoutine);
            }
        }

        void OnTriggerExit(Collider other)
        {
            // When the client exits the door area
            if (other.CompareTag("Customer"))
            {
                // Start delayed closing
                closeRoutine = StartCoroutine(CloseAfterDelay());
            }
        }

        IEnumerator CloseAfterDelay()
        {
            yield return new WaitForSeconds(closeDelay);
            openandclose.Play("Closing");
        }
    }
}