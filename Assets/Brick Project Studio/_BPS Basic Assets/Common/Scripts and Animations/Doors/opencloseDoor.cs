using System.Collections;
using UnityEngine;

namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public float closeDelay = 0.5f;  // задержка перед закрытием
        
        private Coroutine closeRoutine;

        void OnTriggerEnter(Collider other)
        {
            // Если в триггер вошёл клиент
            if (other.CompareTag("Customer"))
            {
                // Открываем дверь сразу
                openandclose.Play("Opening");
                // Отменяем запланированное закрытие, если было
                if (closeRoutine != null)
                    StopCoroutine(closeRoutine);
            }
        }

        void OnTriggerExit(Collider other)
        {
            // Когда клиент вышел из области двери
            if (other.CompareTag("Customer"))
            {
                // Запускаем отложенное закрытие
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