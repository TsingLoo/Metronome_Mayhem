using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public KeyCode keycode;
    [SerializeField] private ParticleSystem particle;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = true;

        particle.gameObject.SetActive(false);
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
                particle.gameObject.SetActive(true);
                
            }
            else
            {
                lineRenderer.enabled = false;
                particle.gameObject.SetActive(false);
            }
        }
    }
}
