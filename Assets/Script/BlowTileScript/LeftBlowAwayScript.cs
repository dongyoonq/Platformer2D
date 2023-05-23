using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftBlowAwayScript : MonoBehaviour
{
    AreaEffector2D effector;

    private void Awake()
    {
        effector = transform.GetComponentInParent<AreaEffector2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            effector.forceAngle = 180;
        }
    }
}
