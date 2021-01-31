using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : Element2DController
{
    [SerializeField] protected int lifePoint;
    [SerializeField] protected int damage;
    private CharacterController characterController;
    [SerializeField] protected float distanceToShoot;
    [SerializeField] protected float distanceToChase;
    [SerializeField] private float time2disappeare = 4.0f;
    private bool alive = true;
    [SerializeField] protected float shootSpeed;
    [SerializeField] protected float startShootTime;
    private bool shooting = false;
    [SerializeField]  protected float speed;
    protected Transform cible;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundRadius;
    [SerializeField] private float gravity = -9.81f;
    private Transform groundCheck;
    private Vector3 actualDownwardVelocity;
    private bool stunned = false;
    protected float maxTimeStunned;
    private float timeStunned;
    [SerializeField] protected GameObject droppedhand;

    protected AudioSource mainAudioSource;
    [SerializeField] protected AudioClip attackClip;
    [SerializeField] protected AudioClip hurtClip;
    [SerializeField] protected AudioClip deathClip;

    [Header("WalkAnimation")]
    [SerializeField] protected float walkTimeBetweenFrame = 0.1f;
    [SerializeField] protected List<Material> walkFrames;
    protected AnimationWithMaterial walkAnimation;

    [Header("ShootAnimation")]
    [SerializeField] protected float shootTimeBetweenFrame = 0.1f;
    [SerializeField] protected List<Material> shootFrames;
    protected AnimationWithMaterial shootAnimation;

    [Header("DeathAnimation")]
    [SerializeField] protected float deathTimeBetweenFrame = 0.1f;
    [SerializeField] protected List<Material> deathFrames;
    protected AnimationWithMaterial deathAnimation;


    [Header("HitAnimation")]
    [SerializeField] protected float hitTimeBetweenFrame = 0.1f;
    [SerializeField] protected List<Material> hitFrames;
    protected AnimationWithMaterial hitAnimation;



    protected override void Start()
    {
        base.Start();

        cible = GameObject.FindGameObjectWithTag("Player").transform;

        characterController = GetComponent<CharacterController>();
        groundCheck = GameObject.Find("GroundCheck").transform;
        mainAudioSource = GetComponent<AudioSource>();
        timeStunned = maxTimeStunned;

        walkAnimation = new AnimationWithMaterial();
        walkAnimation.frames = walkFrames;
        walkAnimation.timeBetweenFrame = walkTimeBetweenFrame;

        shootAnimation = new AnimationWithMaterial();
        shootAnimation.frames = shootFrames;
        shootAnimation.timeBetweenFrame = shootTimeBetweenFrame;

        deathAnimation = new AnimationWithMaterial();
        deathAnimation.frames = deathFrames;
        deathAnimation.timeBetweenFrame = deathTimeBetweenFrame;

        hitAnimation = new AnimationWithMaterial();
        hitAnimation.frames = hitFrames;
        hitAnimation.timeBetweenFrame = hitTimeBetweenFrame;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (alive && !stunned)
        {
            Movement();
            Attack();
        }
        ManageDeath();
        ManageStunne();
    }

    void Movement()
    {
        bool inChaseRange = distanceToChase >= Vector3.Distance(transform.position, cible.position);
        if (!shooting && inChaseRange)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, cible.transform.position, speed * Time.deltaTime);
            characterController.Move((newPosition - transform.position) + Gravity());
            LaunchAnimation(walkAnimation);
        }
    }

    
    void Die()
    {
        //TODO animations
        LaunchAnimation(deathAnimation);
        PlaySound(deathClip);
       if (stunned)
        {
            Vector3 position = transform.position;
            Collectible hand = droppedhand.GetComponent<DroppedHand>();
            Instantiate(hand, position, Quaternion.identity);
        }

        alive = false;
        CancelInvoke();
        shooting = false;
    }

    private void ManageDeath()
    {
        if (!alive)
        {
            time2disappeare -= Time.deltaTime;
            if (time2disappeare < 0)
            {

                Destroy(gameObject);
            }
        }
    }

    public void LooseLife(int quantityToLoose)
    {
        if (alive)
        {
            PlaySound(hurtClip);
            LaunchAnimation(hitAnimation);
            lifePoint -= quantityToLoose;
            if (lifePoint <= 0)
            {
                Die();
            }
        }
    }
    void Attack()
    {
        bool inShootingRange = distanceToShoot >= Vector3.Distance(transform.position, cible.position);
        if (shooting)
        {
            if (!inShootingRange)
            {
                CancelInvoke();
                shooting = false;
            }
        } else
        {
            if (inShootingRange)
            {
                InvokeRepeating("Shoot", startShootTime, shootSpeed);
                shooting = true;
            }
        }
        
    }

    //Check if the Player is on the ground or not
    private bool CheckGround()
    {
        bool isOnGround;
        isOnGround = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
        return isOnGround;
    }


    //Calculate the effect of Gravity (No friction)
    private Vector3 Gravity()
    {
        actualDownwardVelocity.y += gravity * Time.deltaTime;


        if (CheckGround() && actualDownwardVelocity.y < 0)
        {
            actualDownwardVelocity.y = -2f;
        }


        return actualDownwardVelocity * Time.deltaTime;

    }

    protected abstract void Shoot();

    public bool IsAlive()
    {
        return alive;
    }

    public virtual void Stunne()
    {   
        stunned = true;
        CancelInvoke();
        shooting = false;
    }

    private void ManageStunne()
    {
        if (stunned)
        {
            timeStunned -= Time.deltaTime;
            if (timeStunned < 0 && alive)
            {
                stunned = false;
                timeStunned = maxTimeStunned;
                Vector3 position = transform.position + new Vector3(1,0,1);
                Collectible hand = droppedhand.GetComponent<DroppedHand>();
                Instantiate(hand, position, Quaternion.identity);
            }
        }
    }

    public int GetLife()
    {
        return lifePoint;
    }
    public void PlaySound(AudioClip audioClip)
    {
        mainAudioSource.clip = audioClip;
        mainAudioSource.time = 0;
        mainAudioSource.Play();
    }
}
