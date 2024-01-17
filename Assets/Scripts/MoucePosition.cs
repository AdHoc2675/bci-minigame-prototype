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

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        judgeObject.SetActive(false);
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

}
