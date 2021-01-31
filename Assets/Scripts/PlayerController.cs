using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float maxSpeed; //Player max move speed
    [SerializeField] private float accelerationSpeed; //Player acceleration speed
    [SerializeField] private float decelerationSpeed; //Player deceleration speed
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundRadius;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject meleePrefab;
    [SerializeField] private int handDamage;
    [SerializeField] private int meleeDamage;
    [SerializeField] private float meleeAttackDuration;
    [SerializeField] private float meleeCooldownDuration;
    [SerializeField] private HealthBar healthBar;
    private int maxHealth = 20;
    private int lifePoint = 20;
    private float turnSmoothVelocity = 0;
    private float turnSmoothTime = 0;
    private Transform groundCheck;
    private Vector3 actualDownwardVelocity; //Actual speed at which the Player is going down
    private float actualSpeed; //Actual speed of the player movement
    private Transform cameraTransform; //The Transform component of the Player Camera
    private CharacterController characterController;
    private Vector3 lastMoveDirection;
    private Transform shootLocation;
    private bool canShoot = true;
    private bool canMelee = true;
    private AudioSource walkAudioSource;
    private AudioSource mainAudioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip meleeClip;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private float timeBetweenFrame = 0.1f;
    private UIManager uiManager;

    private void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        characterController = GetComponent<CharacterController>();
        groundCheck = GameObject.Find("GroundCheck").transform;
        shootLocation = GameObject.Find("ShootLocation").transform;
        healthBar.SetMaxHealth(maxHealth);
        mainAudioSource = GetComponent<AudioSource>();
        walkAudioSource = GameObject.Find("WalkSoundSource").GetComponent<AudioSource>();
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
    }

    private void Update()
    {
        Actions();
    }

    //All Player Actions
    private void Actions()
    {
        if (Time.timeScale != 0)
        {
            transform.rotation = cameraTransform.rotation;

            if (Input.GetButtonDown("ShootHand"))
            {
                if (canShoot)
                {
                    ShootHand();
                }

                //else if we want to animate it
            }

            if (Input.GetButtonDown("MeleeAttack") && canMelee)
            {

                AttackInMelee();
                canMelee = false;
                StartCoroutine(CooldownMelee());
            }

            if (Input.GetButtonDown("Pause") )
            {
                uiManager.Pause();
            }

            Move();
            PlayWalkingSound();
        }
    }

    //Player Movement
    private void Move()
    {
        characterController.Move(MovementCalculation() + Gravity());
    }

    private void PlayWalkingSound()
    {
        if(actualSpeed != 0 && !walkAudioSource.isPlaying)
        {

            walkAudioSource.clip = walkClip;
            walkAudioSource.time = 0;
            walkAudioSource.Play();

        }
    }

    private Vector3 MovementCalculation()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");

        //Direction of the player movement without the camera
        Vector3 directionWOCam = new Vector3(horizontalAxis, 0f, verticalAxis).normalized;


        //If the Player moves
        if (directionWOCam.magnitude >= 0.1f)
        {
            if (actualSpeed < maxSpeed)
            {
                actualSpeed += accelerationSpeed * Time.deltaTime;
            }


            float targetAngle = Mathf.Atan2(directionWOCam.x, directionWOCam.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            lastMoveDirection = moveDirection;
            return moveDirection.normalized * actualSpeed * Time.deltaTime;

        }
        else
        {
            if (actualSpeed > 0)
            {
                actualSpeed -= decelerationSpeed * Time.deltaTime;
            }
            if (actualSpeed < 0)
            {
                actualSpeed = 0;
            }
            return lastMoveDirection.normalized * actualSpeed * Time.deltaTime;
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

    void ShootHand()
    {
        canShoot = false;
        HandProjectile handProjectile = projectile.GetComponent<HandProjectile>();
        Vector3 posToShootFrom = shootLocation.transform.position;
        Vector3 shootDirection = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * Vector3.forward;

        handProjectile.projectileDirection = shootDirection;
        handProjectile.damageDealt = handDamage;
        Instantiate(projectile, posToShootFrom, Quaternion.identity);
        uiManager.DisappearHand();
        PlaySound(shootClip);
    }
    
    void AttackInMelee()
    {
        MeleeAttack meleeAttack = meleePrefab.GetComponent<MeleeAttack>();

        meleeAttack.damageDealt = meleeDamage;
        meleeAttack.hitboxDuration = meleeAttackDuration;
        Instantiate(meleePrefab, shootLocation.transform);
        PlaySound(meleeClip);
        StartCoroutine(uiManager.LeftAttackAnimation(timeBetweenFrame));

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            if(other.name == "DroppedHand")
            {
                other.gameObject.GetComponent<DroppedHand>().Collect();
                canShoot = true;
                uiManager.AppearHand();
            }
        }

        if (other.CompareTag("VictoryTrigger"))
        {
            uiManager.Victory();
        }
    }

    IEnumerator CooldownMelee()
    {
        yield return new WaitForSecondsRealtime(meleeAttackDuration + meleeCooldownDuration);
        canMelee = true;
    }

    public void LooseLife(int quantityToLoose)
    {
        PlaySound(hurtClip);
        lifePoint -= quantityToLoose;
        healthBar.SetHealth(lifePoint);
        if (lifePoint <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        PlaySound(deathClip);
        uiManager.Lost();
    }

    private void PlaySound(AudioClip audioClip)
    {
        mainAudioSource.clip = audioClip;
        mainAudioSource.time = 0;
        mainAudioSource.Play();
    }
}
