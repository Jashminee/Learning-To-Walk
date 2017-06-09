using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CammeraFollow : MonoBehaviour
{
    public Transform target;

    float destinationX;
    float currentX;

    float timer;

    private void Update()
    {
        if (transform.position.x == target.position.x)
            return;

        float factor = Mathf.Abs(transform.position.x - target.position.x) / 100.0f;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, transform.position.z), factor);

        //ApplyX();
    }

    void ApplyX ()
    {
        Vector3 position = transform.position;
        position.x = currentX;
        transform.position = position;
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(transform.position.x, -Screen.height), new Vector3(transform.position.x, +Screen.height));
    }
}
