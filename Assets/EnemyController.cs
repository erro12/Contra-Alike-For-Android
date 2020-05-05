using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Range(1, 3)]
    public float speed;

    [Range(10, 100)]
    public float health;

    [Range(1, 10)]
    public float damage;

    internal bool alive = true, deathAnimPlayed = false;

    public float fireRate = 0f;
    public float maxSpeed = 0f;
    public GameObject bullet;
    [HideInInspector]
    public bool moveLeft = true;
    public GameObject _BSP;

    private GameObject Target;
    [Range(15, 20)]
    private float minDistance = 16f;
    public Animator anim;
    private bool isWalking = false, attack = false;

    private void Awake()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Target = GameObject.FindGameObjectWithTag("Player");
        //transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x+5, transform.localPosition.y, transform.localPosition.z);
        minDistance = Random.Range(15f, 20f);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        if (!alive)
        {
            if (!deathAnimPlayed)
            {
                anim.SetTrigger("Die");
                StartCoroutine("Die");
            }
            return;
        }

        if(Vector3.Distance(Target.transform.position, transform.position) <= minDistance)
        {
            anim.SetBool("Walk", false);
            isWalking = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
            attack = true;
        }
        else
        {
            if (!isWalking)
            {
                anim.SetBool("Walk", true);
                isWalking = true;
            }

            GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);
        }


        if (attack)
        {
            if (Time.time > fireRate)
            {
                GameObject bulletInstance = (GameObject)Instantiate(bullet, _BSP.transform.position, Quaternion.Euler(new Vector3(0, 0, 1)));
                bulletInstance.gameObject.GetComponent<Bullet>().shooter = transform.gameObject;
                bulletInstance.gameObject.GetComponent<Bullet>().maxSpeed = maxSpeed;
                bulletInstance.gameObject.GetComponent<Bullet>().damage = damage;
                Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                bulletInstance.SetActive(true);
                fireRate = Time.time + fireRate;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (alive && other.gameObject.CompareTag("BulletP") && other.gameObject.GetComponent<Bullet>().maxHits > 0)
        {
            other.gameObject.GetComponent<Bullet>().maxHits--;
            Debug.Log("wait.... Something's odd.... why am i bleeding");
            this.health -= other.gameObject.GetComponent<Bullet>().damage;
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, -20);
            other.gameObject.SetActive(false);
            Destroy(other);
            Debug.Log("hit with new health = " + health);
            if (health <= 0)
            {
                Debug.Log("guess i'll die");
                alive = false;
            }
        }
    }
}
