using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    new Transform transform; //Chaches the transform so it doesn't have to be retrieved by the system every frame.
    new Rigidbody2D rigidbody2D;
    
    public int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    Vector2 move;
    public float speed; //Adjustable Movement speed from the inspector.


    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    public GameObject projectilePrefab;
    AudioSource audioSource;
    public AudioClip throwSound; 
    public AudioClip hitSound; 


    
    void Start()
    {
        transform = base.transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 60;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource= GetComponent<AudioSource>();
    }
    
    void Update()
    {
        move.x = Input.GetAxis("Horizontal"); 
        move.y = Input.GetAxis("Vertical");
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if(Input.GetKeyDown(KeyCode.C)) Launch();

        if (Input.GetKeyDown(KeyCode.X)) Interact();

    }
    
    void FixedUpdate()
    {
        rigidbody2D.MovePosition(new Vector2(
        rigidbody2D.position.x + move.x * speed * Time.deltaTime,
        rigidbody2D.position.y + move.y * speed * Time.deltaTime
        ));
    }
    

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(hitSound);
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        
        animator.SetTrigger("Launch");
        PlaySound(throwSound);
    }
    
    void Interact(){ 

        RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }  
            }
        }
    }

    public void PlaySound(AudioClip clip) => audioSource.PlayOneShot(clip);


}