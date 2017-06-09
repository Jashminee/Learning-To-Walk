using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : MonoBehaviour
{
    public Camera mainCamera;

	void Update ()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
        {
            hit.transform.Translate(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        }
	}
}
