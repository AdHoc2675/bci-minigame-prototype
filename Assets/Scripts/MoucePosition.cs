using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LSL;
using LSL4Unity.Utils;

public class MoucePosition : MonoBehaviour
{
    private Camera camera;
    private Vector2 mousePosition;

    public bool mouseHold;
    public Vector2 mouseClickPoint;
    public GameObject judgeObject;

    public RaycastHit2D[] hitList;
    public RaycastHit2D hit;

    public int judgeChecker = 0;

    public int Combo = 0;
    public int Score = 0;
    public Text ComboText;
    public Text ScoreText;

    public int maxEnergy = 100;
    public int currentEnergy;

    public RectTransform energyBar;
    public Text energyText;

    public ParticleSystem lineParticle;
    public ParticleSystem sparkParticle;
    public ParticleSystem.MainModule lineParticleMain;
    public ParticleSystem.MainModule sparkParticleMain;

    public GameObject GreatJudgeText;
    public GameObject GoodJudgeText;
    public GameObject PoorJudgeText;
    public GameObject MissJudgeText;

    #region LSL4Unity_inlet
    public string StreamName; // must be same with the OpenViBE streamname
    ContinuousResolver resolver;
    double max_chunk_duration = 2.0; // epoch interval 2.0sec
    private StreamInlet inlet;

    private float[,] data_buffer;
    private double[] timestamp_buffer;
    bool isSatisfied = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        judgeObject.SetActive(false);

        currentEnergy = (maxEnergy / 2);

        lineParticleMain = lineParticle.main;
        sparkParticleMain = sparkParticle.main;

        StartCoroutine(JudgementChecker());
    }

    private void Awake()
    {
        if (!StreamName.Equals(""))
            resolver = new ContinuousResolver("name", StreamName);
        else
        {
            Debug.LogError("Object must specify a name for resolver to lookup a stream.");
            this.enabled = false;
            return;
        }
        StartCoroutine(ResolveExpectedStream());
    }

    // Update is called once per frame
    void Update()
    {
        #region LSL_inlet_update
        if (inlet != null)
        {
            int samples_returned = inlet.pull_chunk(data_buffer, timestamp_buffer);
            if (samples_returned > 0)
            {
                float x = data_buffer[samples_returned - 1, 0] * 10;
                currentEnergy = (int)x;
            }
        }
        #endregion

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

        if (currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;
        }

        energyBar.localScale = new Vector3((currentEnergy / (maxEnergy * 1.0f)), 1, 1);
        energyText.text = "X " + currentEnergy.ToString() + "%";

        float temp = ((currentEnergy / (maxEnergy * 1.0f)) * 4.0f);

        if (temp <= 1.0f)
        {
            temp = 1.0f;
        }
        lineParticleMain.startSize = new ParticleSystem.MinMaxCurve(0.25f * temp, 0.25f * temp);
        sparkParticleMain.startSize = new ParticleSystem.MinMaxCurve(0.25f * temp, 0.25f * temp);
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
                    Combo++;
                    Score = (int)(Score + (1.0f * currentEnergy));
                    Instantiate(GreatJudgeText, transform);
                }
                else if (judgeChecker == 2)
                {
                    Debug.Log("Good");
                    Combo++;
                    Score = (int)(Score + (0.7f * currentEnergy));
                    Instantiate(GoodJudgeText, transform);
                }
                else if (judgeChecker == 1)
                {
                    Debug.Log("Poor");
                    Combo++;
                    Score = (int)(Score + (0.3f * currentEnergy));
                    Instantiate(PoorJudgeText, transform);

                }
                else if (judgeChecker == 0)
                {
                    Debug.Log("Miss");
                    Combo = 0;
                    Instantiate(MissJudgeText, transform);
                }
            }


            ComboText.text = "Combo: " + Combo;
            ScoreText.text = "Score: " + Score;
            judgeChecker = 0;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator ResolveExpectedStream()
    {
        var results = resolver.results();
        while (results.Length == 0)
        {
            yield return new WaitForSeconds(.1f);
            results = resolver.results();
        }
        inlet = new StreamInlet(results[0]);
        int buf_samples = (int)Mathf.Ceil((float)(inlet.info().nominal_srate() * max_chunk_duration));
        int n_channels = inlet.info().channel_count();
        data_buffer = new float[buf_samples, n_channels];
        timestamp_buffer = new double[buf_samples];
    }

}
