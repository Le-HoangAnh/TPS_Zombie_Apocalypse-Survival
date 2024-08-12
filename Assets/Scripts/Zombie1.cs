using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie1 : MonoBehaviour
{
    [Header("Zombie Health and Damage")]
    public float zombieHealth = 100f;
    private float presentHealth;
    public float giveDamage = 5f;
    public HealthBar healthBar;

    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;
    public Transform lookPoint;
    public Camera AttackingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;

    [Header("Zombie Walking Var")]
    private float zombieSpeed = 2f;
    private float turningSpeed = 300f;
    public float stopSpeed = 1f;
    float walkingPointRadius = 2f;
    public Vector3 destination;
    public bool destinationReached;

    [Header("Zombie Attacking Var")]
    private float timeBtwAttack = 0.31f;      //time between attack
    bool previouslyAttack;

    [Header("Zombie Animation")]
    public Animator anim;
    //public float gravity = -9.81f;
    //Vector3 velocity;
    //public Transform surfaceCheck;
    //bool onSurface;
    //public float surfaceDistance = 0.4f;
    //public LayerMask surfaceMask;

    [Header("Zombie mood/states")]
    private float visionRadius = 50f;             //ban kinh tam nhin
    public float attackingRadius;          //ban kinh tan cong
    public bool playerInvisionRadius;      //ban kinh tam nhin Player cua Zombie
    public bool playerInattackingRadius;   //ban kinh tan cong Player cua Zombie

    private void Awake()
    {
        presentHealth = zombieHealth;
        healthBar.GiveFullHealth(zombieHealth);
        zombieAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

        //onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        //if (onSurface && velocity.y < 0)
        //{
        //    velocity.y = -2f;
        //}

        //velocity.y += gravity * Time.deltaTime;

        if (!playerInvisionRadius && !playerInattackingRadius) Walk();
        //Walk();
        if (playerInvisionRadius && !playerInattackingRadius) PursuePlayer();
        if (playerInvisionRadius && playerInattackingRadius) AttackPlayer();
    }
    public void Walk()
    {
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= stopSpeed)
            {
                //Turning
                destinationReached = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);

                //Move AI
                transform.Translate(Vector3.forward * zombieSpeed * Time.deltaTime);
            }
            else
            {
                destinationReached = true;
            }
        }
    }

    public void locateDestination(Vector3 destination)
    {
        this.destination = destination;
        destinationReached = false;
    }

    private void PursuePlayer()
    {
        if (zombieAgent.SetDestination(playerBody.position))
        {
            //animations
            anim.SetBool("Walking", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", false);
        }
        else
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", true);
        }
    }

    private void AttackPlayer()
    {
        zombieAgent.SetDestination(transform.position);
        transform.LookAt(lookPoint);
        if (!previouslyAttack)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                Debug.Log("Attacking" + hitInfo.transform.name);

                PlayerScript playerBody = hitInfo.transform.GetComponent<PlayerScript>();

                if (playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage);
                }

                anim.SetBool("Attacking", true);
                anim.SetBool("Walking", false);
                anim.SetBool("Running", false);
                anim.SetBool("Died", false);
            }

            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBtwAttack);
        }
    }

    private void ActiveAttacking()
    {
        previouslyAttack = false;
    }

    public void zombieHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);

        if (presentHealth <= 0)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", true);

            ZombieDie();
        }
    }

    private void ZombieDie()
    {
        zombieAgent.SetDestination(transform.position);
        zombieSpeed = 0f;
        attackingRadius = 0f;
        visionRadius = 0f;
        playerInattackingRadius = false;
        playerInvisionRadius = false;
        Object.Destroy(gameObject, 1.0f);

    }
}
