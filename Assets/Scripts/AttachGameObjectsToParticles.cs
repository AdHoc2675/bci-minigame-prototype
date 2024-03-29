﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject m_Prefab;
    private GameObject tempObj;
    public MoucePosition moucePosition;

    private ParticleSystem m_ParticleSystem;
    private List<GameObject> m_Instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_Particles;

    // Start is called before the first frame update
    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count) { 

            tempObj = Instantiate(m_Prefab, m_ParticleSystem.transform);

            float temp = ((moucePosition.currentEnergy / (moucePosition.maxEnergy * 1.0f)) * 4.0f);

            if (temp <= 1.0f)
            {
                temp = 1.0f;
            }

            tempObj.GetComponent<Light2D>().pointLightOuterRadius = tempObj.GetComponent<Light2D>().pointLightOuterRadius * temp;

            //tempObj.Light = tempObj.transform.localScale * temp;
            m_Instances.Add(Instantiate(m_Prefab, m_ParticleSystem.transform));
        }
        bool worldSpace = (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_Instances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace)
                    m_Instances[i].transform.position = m_Particles[i].position;
                else
                    m_Instances[i].transform.localPosition = m_Particles[i].position;
                m_Instances[i].SetActive(true);
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
        }
    }
}
