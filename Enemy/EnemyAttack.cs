using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyAttack : MonoBehaviour
    {
        public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
        public int attackDamage = 10;               // The amount of health taken away per attack.
        
        GameObject player;                          // Reference to the player GameObject.
        GameObject bullet;
        PlayerHealth playerHealth;                  // Reference to the player's health.
        EnemyHealth enemyHealth;                    // Reference to this enemy's health.
        PlayerShoot playerShoot;
        HitDetection hitDetection;
        bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
        bool enemyHit;
        float timer;                                // Timer for counting up to the next attack.
        int dmg;


        void Awake ()
        {
            // Setting up the references.
            player = GameObject.FindGameObjectWithTag ("Player");
            bullet = GameObject.FindGameObjectWithTag("Bullet");
            playerHealth = player.GetComponent <PlayerHealth> ();
            playerShoot = player.GetComponent<PlayerShoot>();
            enemyHealth = GetComponent<EnemyHealth>();
            dmg = playerShoot.damagePerShot;
        }


        void OnTriggerEnter (Collider other)
        {
            // If the entering collider is the player...
            if(other.gameObject == player)
            {
                // ... the player is in range.
                playerInRange = true;
            }
        }


        void OnTriggerExit (Collider other)
        {
            // If the exiting collider is the player...
            if(other.gameObject == player)
            {
                // ... the player is no longer in range.
                playerInRange = false;
            }
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.name == "Bullet(Clone)")
            {
                enemyHit = true;
                Destroy(col.gameObject);
            }
        }

        void Update ()
        {
            // Add the time since Update was last called to the timer.
            timer += Time.deltaTime;

            // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
            if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
            {
                // ... attack.
                Attack ();
            }

            if (enemyHit && enemyHealth.currentHealth > 0)
            {
                Hit();
            }
        }

        void Hit()
        {
            if (enemyHealth.currentHealth > 0)
            {
                enemyHealth.TakeDamage(dmg);
                enemyHit = false;
            }
        }

        void Attack ()
        {
            // Reset the timer.
            timer = 0f;

            // If the player has health to lose...
            if(playerHealth.currentHealth > 0)
            {
                // ... damage the player.
                playerHealth.TakeDamage (attackDamage);
            }
        }
    }
}