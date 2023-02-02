using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{

    // TODO: Clamp speed

    public float speed = 10f;
    public float oomph = 100f;

    private float nextDamage = 0;

    public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction.x = 0;
        direction.z = 0;

        while (direction.x == 0) direction.x = Random.Range(-1f, 1f);
        while (direction.z == 0) direction.z = Random.Range(-1f, 1f);

        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
        
        if (nextDamage > 0) {
            oomph -= nextDamage;
            if (oomph <= 0) Destroy(this.gameObject);
            nextDamage = 0;
        }

    }

    void OnTriggerEnter(Collider other) 
    {
        // hooked up
        if (other.transform.name.StartsWith("wall_")) {
            // BOUNCE WITH ME BROTHER
            if (other.transform.name.EndsWith("t")) {
                direction.x *= -1;
            } else {
                direction.z *= -1;
            }
        } else {
            // usually true
            Bouncer otherBouncer = other.GetComponent<Bouncer>();
            if (otherBouncer != null && name != otherBouncer.name) {
                // health check - only do yourself
                nextDamage += otherBouncer.oomph;   // facilitates multi-hit collisions; could be better, could queue up events
            }
        }


    }

}
