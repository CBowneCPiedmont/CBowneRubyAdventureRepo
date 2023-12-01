using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushroomDamage : MonoBehaviour
{
    
      public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        
        if (controller != null)
        {
            {
            controller.ChangeHealth(-1);
            controller.ChangeSpeed(4);
        }
            Destroy(gameObject);
            controller.PlaySound(collectedClip);
        }
    }
}
