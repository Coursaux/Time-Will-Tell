using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    private TextMeshProUGUI health;
    private TextMeshProUGUI time;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        health = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        time = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        health.text = "Health: " + Mathf.Round(Mathf.Clamp(player.GetComponent<HealthManager>().currentHealth, 0, 100));
        time.text = "Time: " + Mathf.Clamp(player.GetComponent<PlayerController>().timeAmount, 0, float.MaxValue).ToString("0.0");
    }
}
