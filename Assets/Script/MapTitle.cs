using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapTitle : MonoBehaviour
{
    TMP_Text tmp;
    [SerializeField] Transform player;

    private void Awake()
    {
        tmp = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (player.position.y < 17f)
            tmp.text = "Grassland";
        else if (player.position.y > 17f)
            tmp.text = "Space";

        if (player.position.x > 48f)
            tmp.text = "Dessert";
    }
}
