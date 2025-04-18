using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private float buttonAnimSpeed = 9;
    private bool tap = true;
    public AudioClip tapAudio;

    private RaycastHit hitInfo;
    private Ray ray;
    
    // public GameObject playerRecord; Личный рекорд
    
    void Awake()
    {
        // TODO: Добавть личный рекород игрока (Нужен префаб)
    }
    
    void Update()
    {
        if (tap)
        {
            StartCoroutine(tapManager());
        }
    }

    IEnumerator tapManager()
    {
        // Mouse 
        if (Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        else
        {
            yield break;
        }

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject objectHit = hitInfo.transform.gameObject;
            switch (objectHit.name)
            {
                case "ButtonPlay-01":
                    playAudio(tapAudio);
                    StartCoroutine(animateButton(objectHit));
                    // PlayerRecord() 
                    yield return new WaitForSeconds(1.0f);
                    SceneManager.LoadScene("Game");
                    break;
            }
        }
        
        IEnumerator animateButton(GameObject _btn)
        {
            tap = false;
            Vector3 startingScale = _btn.transform.localScale;  
            Vector3 destinationScale = startingScale * 0.85f;                           

            //Scale up
            float t = 0.0f;
            while (t <= 1.0f)
            {
                t += Time.deltaTime * buttonAnimSpeed;
                _btn.transform.localScale = new Vector3(Mathf.SmoothStep(startingScale.x, destinationScale.x, t),
                    Mathf.SmoothStep(startingScale.y, destinationScale.y, t),
                    _btn.transform.localScale.z);
                yield return 0;
            }
            
            float r = 0.0f;
            if (_btn.transform.localScale.x >= destinationScale.x)
            {
                while (r <= 1.0f)
                {
                    r += Time.deltaTime * buttonAnimSpeed;
                    _btn.transform.localScale = new Vector3(Mathf.SmoothStep(destinationScale.x, startingScale.x, r),
                        Mathf.SmoothStep(destinationScale.y, startingScale.y, r),
                        _btn.transform.localScale.z);
                    yield return 0;
                }
            }

            if (r >= 1)
                tap = true;
        }

        void playAudio(AudioClip _clip)
        {
            GetComponent<AudioSource>().clip = _clip;

            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
