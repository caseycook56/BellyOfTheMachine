using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    private GameObject player;
    private void Awake()
    {
        player = GameObject.Find("main_player");
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "main_player")
        {
            print("motlen metal hit player");
            player.GetComponent<Health>().TakeDamage(this.gameObject, 30, 5);
        }
    }
}
