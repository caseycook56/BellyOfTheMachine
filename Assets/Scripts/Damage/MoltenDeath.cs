using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDeath : MonoBehaviour
{
    private GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "main_player")
        {
            print("motlen metal hit player");
            player.GetComponent<Health>().TakeDamage(this.gameObject, 100, 1);
        }
    }

}
