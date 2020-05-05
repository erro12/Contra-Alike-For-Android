using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private GameObject Boss1;
    public float Boss1SpawnTime;
    public GameObject EnemySoldier, Powerup;

    public GameObject BazookaBtn;

    private bool bazookaLocked = true;

    private int scount = 0;
    
    [Range(2, 50)]
    public float MaxSoldierSpawnTime, MinSoldierSpawnTime, PowerUpSpawnInterval;
    private bool boss1Spawned = false;

    private float SoldierSpawnTime;
    public int maxPowerups = 4;
    private Vector2 screenBounds;
    private float startTime, soldierSpawnStartTime = 0f, powerUpSpawnStartTime;
    // Start is called before the first frame update
    private void Awake()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Boss1 = GameObject.FindGameObjectWithTag("Boss1");
        Boss1.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x+5, Boss1.transform.position.y, Boss1.transform.position.z);
        Boss1.SetActive(false);

        startTime = Time.time;
        soldierSpawnStartTime = Time.time;
        powerUpSpawnStartTime = Time.time;
        SoldierSpawnTime = Random.Range(MinSoldierSpawnTime, MaxSoldierSpawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime >= Boss1SpawnTime && !boss1Spawned)
        {
            Debug.Log("boss music intensifies!!!");
            //spawn Boss 1...
            Boss1.SetActive(true);
            Boss1.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + 5, Boss1.transform.position.y, Boss1.transform.position.z);
            boss1Spawned = true;
        }
        else if(Time.time - soldierSpawnStartTime > SoldierSpawnTime &&!boss1Spawned)
        {
            soldierSpawnStartTime = Time.time+ SoldierSpawnTime;
            //spawn soldier prefab...
            GameObject soldier = Instantiate(EnemySoldier);
            soldier.name = "Soldier"+scount;
            soldier.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width+5, 0, 0)).x, EnemySoldier.transform.localPosition.y, EnemySoldier.transform.localPosition.z);
            soldier.SetActive(true);
        }

        if(Time.time - powerUpSpawnStartTime > PowerUpSpawnInterval && maxPowerups > 0)
        {
            powerUpSpawnStartTime = Time.time+PowerUpSpawnInterval;
            //spawn powerup
            Vector3 offPos = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 15, 0, 0)).x, Powerup.transform.localPosition.y, Powerup.transform.localPosition.z);

            GameObject powerup = Instantiate(Powerup, offPos, Quaternion.Euler(0, 0, 0));
            Powerup.name = "Powerup" + maxPowerups;
            maxPowerups--;
            //Powerup.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width+15, 0, 0)).x, Powerup.transform.localPosition.y, Powerup.transform.localPosition.z);
            Powerup.SetActive(true);
        }
    }

    public void UnlockBazooka()
    {
        bazookaLocked = false;
        BazookaBtn.SetActive(true);
        //BazookaBtn.GetComponent<Animator>().SetTrigger("Activate");
    }

    public void FireBazooka()
    {
        Debug.Log("loading bazooka");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().FireBazooka();
        BazookaBtn.SetActive(false);
    }
}
