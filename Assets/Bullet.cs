using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    float angle;
    [HideInInspector]
    internal GameObject shooter;
    public float maxSpeed;
    private PlayerController pc;
    public float damage;
    public int maxHits = 1;

    private void Awake()
    {
        Debug.Log("bullet awakens");
        Debug.Log("shooter: " + shooter.gameObject.tag);
        if(shooter.gameObject.tag == "Player")
        {
            pc = shooter.GetComponent<PlayerController>();
            if (pc.state[5])
                angle = 90f;
            else if (!pc.state[2] && !pc.state[3] && pc.state[6])
                angle = pc.moveLeft ? 180 - -45f : -45f;
            else if (!pc.state[2] && !pc.state[3] && pc.state[7])
                angle = pc.moveLeft ? 180 - 35f : 35f;
            else if (pc.state[8])
                angle = -90f;
            else
                angle = pc.moveLeft ? 180 : 0f;
        }
        else
        {
            EnemyController ec = shooter.GetComponent<EnemyController>();
            angle = ec.moveLeft ? 180 : 0f;
        }
        

        Debug.Log("going with angle:" + angle);
        //Debug.Log(pc.moveLeft);
        //GetComponent<Rigidbody2D>().AddTorque(angle);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -angle);
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody2D>().velocity = shooter.transform.forward * maxSpeed;
        
        //this.GetComponent<Rigidbody2D>().AddRelativeForce(-transform.right* 100f);
        transform.position += transform.right * Time.fixedDeltaTime * maxSpeed;

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
