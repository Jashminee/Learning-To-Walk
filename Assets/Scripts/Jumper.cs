using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public HingeJoint2D hingeJoint2d;
    public KeyCode open = KeyCode.Q;
    public KeyCode close = KeyCode.A;

    private void Start()
    {
        hingeJoint2d = GetComponent<HingeJoint2D>();
    }

	void Update ()
    {
		if(Input.GetKeyDown(open))
        {
            Open();
        }
        if (Input.GetKeyDown(close))
        {
            Close();
        }
    }

    public void Open()
    {
        hingeJoint2d.useMotor = true;
        float speed = 1000;
        SetMotorSpeed(hingeJoint2d.motor, speed);
        StartCoroutine(StopJumping(-speed));
    }

    public void Close()
    {
        hingeJoint2d.useMotor = true;
        float speed = -1000;
        SetMotorSpeed(hingeJoint2d.motor, speed);
        StartCoroutine(StopJumping(-speed));
    }

    void SetMotorSpeed(JointMotor2D motor, float speed)
    {
        motor.motorSpeed = speed;
        hingeJoint2d.motor = motor;
    }

    IEnumerator StopJumping(float motorValue)
    {
        yield return new WaitForFixedUpdate();
        JointMotor2D motor = hingeJoint2d.motor;
        motor.motorSpeed = motorValue;
        hingeJoint2d.motor = motor;
        hingeJoint2d.useMotor = false;
    }
}
