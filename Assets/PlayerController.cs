using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Animator anim;
    public float speed = 0.03f;
    public Vector3 camOffset;
    float canfire = 1f;
    public float camFollowSpeed = 0.75f;

    private GameObject[] soldiersMeetingFate;

    [Range(1, 50)]
    public float damage;

    public Transform _BSP;

    public GameObject bullet;
    public float maxSpeed= 0.3f;

    private bool alive = true, deathAnimPlayed = false;

    [Range(10, 100)]
    public float health;

    [HideInInspector]
    public bool moveRight, moveLeft, shootAbove, shootDown, shootAboveRight, shootDownRight, crouch, goProne, shoot, jump = false;

    enum MyEnum
    {
        IDLE, WALKING, CROUCHING, GOING_PRONE, JUMP, SHOOT_DOWN, SHOOT_ABOVE_RIGHT, SHOOT_DOWN_RIGHT, SHOOTING_ABOVE
    }

    [HideInInspector]
    public bool[] state = new bool[(int)MyEnum.SHOOTING_ABOVE + 1];
    internal float vert;
    internal float horz; bool idleState = true;

    private void Awake()
    {
        state[0] = true;
        for (int i = 0; i < state.Length; i++)
        {
            state[i] = false;
        }

        Camera.main.transform.position = new Vector3(transform.position.x + camOffset.x, 0, transform.position.z + camOffset.z);
    }

   public void MoveRight(bool doesIt)
    {
        moveRight = doesIt;
    }

    public void Crouch()
    {

        if (!state[(int)MyEnum.CROUCHING] && !state[(int)MyEnum.GOING_PRONE] && !state[(int)MyEnum.JUMP])
        {
            anim.SetBool("Crouch", true);
            state[(int)MyEnum.CROUCHING] = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            anim.SetBool("Crouch", false);
            state[(int)MyEnum.CROUCHING] = false;
        }


    }

    public void Jump()
    {
        Debug.Log("jump");
        if (!state[(int)MyEnum.JUMP] &&  !state[(int)MyEnum.CROUCHING] && !state[(int)MyEnum.GOING_PRONE] && state[(int)MyEnum.WALKING])
        {
            Debug.Log("jump command accepted");
            anim.SetTrigger("Jump");
            state[(int)MyEnum.JUMP] = true;
        }
        StartCoroutine(UpdateJumpState());

    }

    IEnumerator UpdateJumpState()
    {
        yield return new WaitForSeconds((45f) / (60f));
        state[(int)MyEnum.JUMP] = false;
    }

    public void GoProne()
    {
        
        if (!state[(int)MyEnum.JUMP] && !state[(int)MyEnum.GOING_PRONE] && !state[(int)MyEnum.CROUCHING])
        {
            anim.SetBool("GoProne", true);
            state[(int)MyEnum.GOING_PRONE] = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            anim.SetBool("GoProne", false);
            state[(int)MyEnum.GOING_PRONE] = false;
        }
    }

    public void Shoot(bool doesIt)
    {
        shoot = doesIt;
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

        if (horz != 0)
        {
            moveRight = horz > 0;
            moveLeft = horz < 0;
            idleState = false;
        }
        if(horz == 0)
        {
            idleState = true;
        }
        shootAboveRight = vert > 0.25f && vert < 0.75f;
        shootAbove = vert > .75f;
        shootDown = vert < -0.75f;
        shootDownRight = vert < -0.25f && vert > -0.75f;

        if ((state[(int)MyEnum.WALKING] && !state[(int)MyEnum.CROUCHING] && !state[(int)MyEnum.GOING_PRONE])) {
            if (moveRight)
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
                //transform.localPosition = new Vector2(transform.position.x + speed, transform.position.y);
                GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
                //Camera.main.transform.position = new Vector3(transform.position.x + camOffset.x, 0, transform.position.z + camOffset.z);
                Vector3 newPosition = new Vector3(transform.position.x + camOffset.x, 0, transform.position.z + camOffset.z);
                Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, newPosition, camFollowSpeed * Time.deltaTime);
            }
            else if (moveLeft)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
                if (transform.position.x > Camera.main.transform.position.x - camOffset.x + 0.5f)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);
                }
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180, transform.rotation.z));
                //transform.localPosition = new Vector2(transform.position.x - speed, transform.position.y);
                //Camera.main.transform.position = new Vector3(transform.position.x + camOffset.x, 0, transform.position.z + camOffset.z);
                Vector3 newPosition = new Vector3(transform.position.x + camOffset.x, 0, transform.position.z + camOffset.z);
                //Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, newPosition, camFollowSpeed * Time.deltaTime);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
            }
        }
        if (Input.GetKeyDown(KeyCode.D)|| (!idleState && moveRight))
        {
            if (!state[(int)MyEnum.WALKING])
            {
                anim.SetBool("Walk", true);
                state[(int)MyEnum.WALKING] = true;
            }
        }else if (!idleState && moveLeft)
        {
            if (!state[(int)MyEnum.WALKING])
            {
                anim.SetBool("Walk", true);
                state[(int)MyEnum.WALKING] = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.D) || (idleState))
        {
            anim.SetBool("Walk", false);
            if (state[(int)MyEnum.WALKING])
            {
                state[(int)MyEnum.WALKING] = false;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
            }
        }


        
         if (!state[(int)MyEnum.JUMP] && shootAbove)
        {
            Debug.Log("shoot above");
            if (!state[(int)MyEnum.SHOOTING_ABOVE])
            {
                anim.SetBool("ShhotAbove", true);
                state[(int)MyEnum.SHOOTING_ABOVE] = true;

                anim.SetBool("ShootAboveRight", false);
                state[(int)MyEnum.SHOOT_ABOVE_RIGHT] = false;

                anim.SetBool("ShootDOwn", false);
                state[(int)MyEnum.SHOOT_DOWN] = false;

                anim.SetBool("ShootDownRight", false);
                state[(int)MyEnum.SHOOT_DOWN_RIGHT] = false;
            }
        }else if (!state[(int)MyEnum.JUMP] && shootAboveRight)
        {
            Debug.Log("shoot above right");
            if (!state[(int)MyEnum.SHOOT_ABOVE_RIGHT])
            {
                anim.SetBool("ShhotAbove", false);
                state[(int)MyEnum.SHOOTING_ABOVE] = false;

                anim.SetBool("ShootAboveRight", true);
                state[(int)MyEnum.SHOOT_ABOVE_RIGHT] = true;

                anim.SetBool("ShootDOwn", false);
                state[(int)MyEnum.SHOOT_DOWN] = false;

                anim.SetBool("ShootDownRight", false);
                state[(int)MyEnum.SHOOT_DOWN_RIGHT] = false;
            }
        }else if (!state[(int)MyEnum.JUMP] && shootDown)
        {
            Debug.Log("shoot down");
            if (!state[(int)MyEnum.SHOOT_DOWN])
            {
                anim.SetBool("ShhotAbove", false);
                state[(int)MyEnum.SHOOTING_ABOVE] = false;

                anim.SetBool("ShootAboveRight", false);
                state[(int)MyEnum.SHOOT_ABOVE_RIGHT] = false;

                anim.SetBool("ShootDOwn", true);
                state[(int)MyEnum.SHOOT_DOWN] = true;

                anim.SetBool("ShootDownRight", false);
                state[(int)MyEnum.SHOOT_DOWN_RIGHT] = false;
            }
        }
        else if (!state[(int)MyEnum.JUMP] && shootDownRight)
        {
            Debug.Log("shoot down right");
            if (!state[(int)MyEnum.SHOOT_DOWN_RIGHT])
            {
                anim.SetBool("ShhotAbove", false);
                state[(int)MyEnum.SHOOTING_ABOVE] = false;

                anim.SetBool("ShootAboveRight", false);
                state[(int)MyEnum.SHOOT_ABOVE_RIGHT] = false;

                anim.SetBool("ShootDOwn", false);
                state[(int)MyEnum.SHOOT_DOWN] = false;

                anim.SetBool("ShootDownRight", true);
                state[(int)MyEnum.SHOOT_DOWN_RIGHT] = true;
            }
        }
        else
        {
            anim.SetBool("ShhotAbove", false);
            state[(int)MyEnum.SHOOTING_ABOVE] = false;

            anim.SetBool("ShootAboveRight", false);
            state[(int)MyEnum.SHOOT_ABOVE_RIGHT] = false;

            anim.SetBool("ShootDOwn", false);
            state[(int)MyEnum.SHOOT_DOWN] = false;

            anim.SetBool("ShootDownRight", false);
            state[(int)MyEnum.SHOOT_DOWN_RIGHT] = false;
        }
         

        if (shoot || Input.GetKeyDown(KeyCode.Space))
        {
            //if (!state[(int)MyEnum.SHOOTING_ABOVE])
            {
                //anim.SetBool("ShhotAbove", true);
                //state[(int)MyEnum.SHOOTING_ABOVE] = true;
            }

            if (Time.time > canfire)
            {
                GameObject bulletInstance = (GameObject)Instantiate(bullet, _BSP.transform.position, Quaternion.Euler(new Vector3(0, 0, 1)));
                bulletInstance.gameObject.GetComponent<Bullet>().shooter = transform.gameObject;
                bulletInstance.gameObject.GetComponent<Bullet>().maxSpeed = maxSpeed;
                bulletInstance.gameObject.GetComponent<Bullet>().damage = damage;
                Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                bulletInstance.SetActive(true);
                canfire = Time.time + 0.25f;
            }
            
            

            
        }
    }

    public void FireBazooka()
    {
        Debug.Log("firing bazooka");
        anim.SetTrigger("FireBazooka");
        soldiersMeetingFate = GameObject.FindGameObjectsWithTag("EnemySoldier");
        Debug.Log(soldiersMeetingFate.Length + " soldiers about to meet their fate...");
        StartCoroutine("Apocalypse");
    }

    IEnumerator Apocalypse()
    {
        yield return new WaitForSeconds(2.5f);
        Debug.Log("NOW!!!!");
        foreach(GameObject soldier in soldiersMeetingFate)
        {
            soldier.GetComponent<EnemyController>().alive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (alive && other.gameObject.CompareTag("Bullet"))
        {
           // Debug.Log("i'm hit!");
            this.health -= other.gameObject.GetComponent<Bullet>().damage;
            other.gameObject.SetActive(false);
            Destroy(other);
            //Debug.Log("player hit with new health = " + health);
            if (health <= 0)
            {
                Debug.Log("COMMANDER IS DOWN!");
                alive = false;
            }
        }
    }


}
