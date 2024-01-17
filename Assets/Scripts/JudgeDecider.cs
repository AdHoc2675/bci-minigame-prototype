using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeDecider : MonoBehaviour
{
    public Collider2D MouceArea;
    public Collider2D GreatArea;
    public Collider2D GoodArea;
    public Collider2D PoorArea;

    // Start is called before the first frame update
    void Start()
    {
        GreatArea = transform.GetChild(0).GetComponent<Collider2D>();
        GoodArea = transform.GetChild(1).GetComponent<Collider2D>();
        PoorArea = transform.GetChild(2).GetComponent<Collider2D>();

        StartCoroutine(JudgementChecker());
     }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator JudgementChecker()
    {
        while (true)
        {
            if (MouceArea.bounds.Intersects(GreatArea.bounds))
            {
                Debug.Log("Great");
            }

            else if (MouceArea.bounds.Intersects(GoodArea.bounds))
            {
                Debug.Log("Good");
            }

            else if (MouceArea.bounds.Intersects(PoorArea.bounds))
            {
                Debug.Log("Poor");
            }
            else
            {
                Debug.Log("Miss");
            }

            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }
}
