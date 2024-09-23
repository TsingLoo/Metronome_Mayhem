using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour, INote
{
    private bool beenHit;
    private float speed, lifetime;

    public void init(float speed, float lifetime)
    {
        this.speed = speed;
        this.lifetime = lifetime;
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        transform.position += speed * Time.fixedDeltaTime * transform.forward;
    }

    public float hit(bool holding) {
        kill();
        return holding ? 0.3f : lifetime;
    }
    public bool hasBeenHit() {
        return beenHit;
    }
    private void kill() {
        Destroy(gameObject);
    }
}

interface INote 
{
    float hit(bool holding);
    bool hasBeenHit();
}