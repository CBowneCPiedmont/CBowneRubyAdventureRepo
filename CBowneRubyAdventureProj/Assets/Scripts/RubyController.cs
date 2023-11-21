using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RubyController : Singleton<RubyController>
{
    new Transform transform; //Chaches the transform so it doesn't have to be retrieved by the system every frame.
    new Rigidbody2D rigidbody2D;
    
    [SerializeField] int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    Vector2 move;
    [SerializeField] float speed; //Adjustable Movement speed from the inspector.
    

    [SerializeField] float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    [SerializeField] GameObject projectilePrefab;
    AudioSource audioSource;
    [SerializeField] AudioClip throwSound; 
    [SerializeField] AudioClip hitSound; 
    [SerializeField] bool ActiveInGame = true;

    [SerializeField] ParticleSystem DamageParticle;
    [SerializeField] ParticleSystem HealthPickupParticle;
    [SerializeField] bool requireEndToRestart;

    
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
        MovementUpdate();
        ControlUpdate();
    }
    
    void ControlUpdate()
    {
        move.x = Input.GetAxis("Horizontal"); 
        move.y = Input.GetAxis("Vertical");
    	if (Input.GetKeyDown(KeyCode.C)) Launch();
        if (Input.GetKeyDown(KeyCode.X)) Interact();
        if (Input.GetKeyDown(KeyCode.R)) UIEnding.instance.Restart(requireEndToRestart);
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
    
    void MovementUpdate()
    {
    	if(!ActiveInGame) return;
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
    }
    
    
    

    void FixedUpdate()
    {
        if(!ActiveInGame) return;
        rigidbody2D.MovePosition(new Vector2(
        rigidbody2D.position.x + move.x * speed * Time.deltaTime,
        rigidbody2D.position.y + move.y * speed * Time.deltaTime
        ));
    }
    

    public bool ChangeHealth(int amount)
    {
        if(amount == 0) return false;
        if (amount < 0)
        {
            if (isInvincible)
                return false;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(hitSound);
        }
        if(amount > 0 && currentHealth >= maxHealth) return false;
        
        Instantiate<ParticleSystem>(amount > 0? HealthPickupParticle : DamageParticle, transform.position, Quaternion.identity);
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        if(currentHealth <= 0) UIEnding.instance.GameEnd(true);
        return true;
    }
    
    void Launch()
    {
        if(!ActiveInGame) return;
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        
        animator.SetTrigger("Launch");
        PlaySound(throwSound);
    }
    
    void Interact(){ 
        
        if(!ActiveInGame) return;
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

    public void GameEnd()
    {
    	speed = 0;
        ActiveInGame = false;
    }
}