using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    //public Rigidbody rb;

    PlayerController pc;
    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    public void FixedUpdate()
    {
        //Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        pc.vert = variableJoystick.Vertical;
        pc.horz = variableJoystick.Horizontal;
        //Debug.Log(pc.horz + ":" + pc.vert);
        //rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}