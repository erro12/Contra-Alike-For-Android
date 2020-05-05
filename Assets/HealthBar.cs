using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update

    private EnemyController EC;
    Vector3 localScale;
    float modifier;

    void Start()
    {
        EC = transform.parent.GetComponent<EnemyController>();
        modifier = transform.localScale.x / EC.health;
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        localScale.x = EC.health*modifier;
        transform.localScale = localScale;
    }
}
