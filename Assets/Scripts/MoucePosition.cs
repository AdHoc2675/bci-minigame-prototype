using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoucePosition : MonoBehaviour
{
    private Camera camera;
    private Vector2 mousePosition;

    public bool mouseHold;
    public Vector2 mouseClickPoint;
    public GameObject judgeObject;

    public RaycastHit2D[] hitList;
    public RaycastHit2D hit;

    public bool greatChecker = false;
    public bool goodChecker = false;
    public bool poorChecker = false;
    public bool missChecker = false;
    public int judgeChecker = 0;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        judgeObject.SetActive(false);

        StartCoroutine(JudgementChecker());
    }

    // Update is called once per frame
    void Update()
    {
        mouseHold = Input.GetMouseButton(0);

        if (mouseHold == true)
        {
            judgeObject.SetActive(true);
        }
        else
        {
            judgeObject.SetActive(false);
        }
        mousePosition = Input.mousePosition;
        mousePosition = camera.ScreenToWorldPoint(mousePosition);

        transform.position = mousePosition;
    }

    IEnumerator JudgementChecker()
    {
        while (true)
        {
            /*
            try {
                hit = Physics2D.Raycast(mousePosition, transform.forward);

            }
            catch {
                Debug.Log("Miss");
            }
            if (hit.collider.gameObject.CompareTag("GreatArea") == true)
            {
                Debug.Log("Great");
            }
            else if (hit.collider.gameObject.CompareTag("GoodArea") == true)
            {
                Debug.Log("Good");
            }
            else if (hit.collider.gameObject.CompareTag("PoorArea") == true)
            {
                Debug.Log("Poor");
            }
            */

            
            hitList = Physics2D.RaycastAll(mousePosition, transform.forward);

            
            for (int i = 0; i < hitList.Length; i++)
            {
                if (hitList[i].collider.gameObject.CompareTag("Target") == true)
                {
                    judgeChecker++;
                } 
            }
            
            if (mouseHold == true)
            {
                Debug.Log(judgeChecker);
                if (judgeChecker == 3)
                {
                    Debug.Log("Great");
                }
                else if (judgeChecker == 2)
                {
                    Debug.Log("Good");
                }
                else if (judgeChecker == 1)
                {
                    Debug.Log("Poor");

                }
                else if (judgeChecker == 0)
                {
                    Debug.Log("Miss");
                }
            }

            

            judgeChecker = 0;
            yield return new WaitForSeconds(0.3f);
        }
        yield return null;
    }
}
