using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public System.Action<Collider2D> OnStartCollideWithGround = delegate { };
    public System.Action<Collider2D> OnEndCollideWithGround = delegate { };

    void OnTriggerEnter2D(Collider2D collider)
    {
        OnStartCollideWithGround.Invoke(collider);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        OnEndCollideWithGround.Invoke(collider);
    }
}
