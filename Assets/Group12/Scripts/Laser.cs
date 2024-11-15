using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public KeyCode keycode;
    [SerializeField] private ParticleSystem particle;
    private List <ParticleSystem> particleSystems = new List<ParticleSystem>();


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = true;
        particleSystems.AddRange(particle.GetComponentsInChildren<ParticleSystem>(true));
        
        var emission = particle.emission;
        emission.enabled = false;
        //Debug.Log(particleSystems.Count);
        foreach (var system in particleSystems)
        {
            emission = system.emission;
            emission.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            Debug.DrawLine(transform.position, hit.point);
            
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);
            if(Input.GetKey(keycode))
            {
                lineRenderer.enabled = true;
                var emission = particle.emission;
                emission.enabled = true;
                foreach (var system in particleSystems)
                {
                    emission = system.emission;
                    emission.enabled = true;
                }
                
            }
            else
            {
                lineRenderer.enabled = false;
                var emission = particle.emission;
                emission.enabled = false;
                foreach (var system in particleSystems)
                {
                    emission = system.emission;
                    emission.enabled = false;
                }
            }
        }
    }
}
