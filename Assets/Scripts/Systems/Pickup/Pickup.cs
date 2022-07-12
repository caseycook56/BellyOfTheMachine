using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private AbilityHolder[] ah;

    void Start()
    {
        ah = GetComponents<AbilityHolder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            if (other.gameObject.TryGetComponent(out PickUpHolder holder))
            {
                if (holder != null)
                {
                    print("new abilities");
                    ah[0].ability = holder.grappleAbility;
                    ah[1].ability = holder.grappleSelection;
                    ah[2].ability = holder.grappleDisengage;
                    other.GetComponent<LoadTutorial>().Load();
                    Destroy(other.gameObject);
                }
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Pickup")
        //{
        //    if (collision.gameObject.TryGetComponent(out PickUpHolder holder))
        //    {
        //        if (holder != null)
        //        {
        //            print("new abilities");
        //            ah[0].ability = holder.grappleAbility;
        //            ah[1].ability = holder.grappleSelection;
        //            ah[2].ability = holder.grappleDisengage;
        //            Destroy(collision.gameObject);
        //        }
        //    }
        //}
    }
}
