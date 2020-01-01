using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    Player player;
    Text healthText;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        healthText = gameObject.GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        healthText.text = player.GetHealth().ToString();
    }
}
