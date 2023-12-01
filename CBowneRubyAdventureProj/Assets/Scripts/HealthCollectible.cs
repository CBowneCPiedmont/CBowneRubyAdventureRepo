using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    
      public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        
        if (controller != null)
        {
            if(controller.ChangeHealth(1) == false) return;
            controller.ChangeSpeed(7); //Fabiana Code Change
            Destroy(gameObject);
            controller.PlaySound(collectedClip);
        }
    }
}
