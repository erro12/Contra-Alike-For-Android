using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    Animator anim;
    bool flying = true;
    public float speed=2f;
    GameManager _GM;



    Vector3 dropStartPos;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        _GM = GameObject.Find("_GM").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (flying)
        {
            //Debug.Log("tf why still fying");
            GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            if(transform.localPosition.y > -4.75f)
            {
                //Debug.Log(dropStartPos.y " is obviously greater than -5f")
                //Debug.Log("tf why still falling");
                transform.localPosition = new Vector3(transform.localPosition.x - 0.15f, transform.localPosition.y - 0.25f, 0);
            }
        }
    }

    /*
     void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
         */

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("break this shit!");
        if (flying && other.gameObject.CompareTag("BulletP"))
        {
            flying = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
            anim.SetTrigger("Drop");
            //dropStartPos = transform.localPosition;

            //Debug.Log("going for: "+ -5+" from: "+dropStartPos.y);
        }
        else
        {
            if(!flying && other.gameObject.CompareTag("Player"))
            {
                Debug.Log("bazooka UNLOCKED!!!!!!");
                _GM.UnlockBazooka();
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
