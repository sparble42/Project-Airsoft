using UnityEngine;

namespace CompleteProject
{
    public class EnemyHealth : MonoBehaviour
    {
        public int startingHealth = 100;            // The amount of health the enemy starts the game with.
        public int currentHealth;                   // The current health the enemy has.
        public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
        public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
        public AudioClip deathClip;                 // The sound to play when the enemy dies.
        public GameObject pickUp;

        GameObject hitmarker;
        
        AudioSource enemyAudio;                     // Reference to the audio source.
        ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
        BoxCollider capsuleCollider;            // Reference to the capsule collider.
        float effectsDisplayTime = 0.1f;
        float timer;
        bool isDead;                                // Whether the enemy is dead.
        bool isSinking;                             // Whether the enemy has started sinking through the floor.


        void Awake ()
        {
            // Setting up the references.
            hitmarker = this.transform.Find("Hitmarker").gameObject;
            enemyAudio = GetComponent <AudioSource> ();
            hitParticles = GetComponentInChildren <ParticleSystem> ();
            capsuleCollider = GetComponent <BoxCollider> ();

            // Setting the current health when the enemy first spawns.
            currentHealth = startingHealth;
        }


        void Update ()
        {
            timer += Time.deltaTime;

            // If the enemy should be sinking...
            if (isSinking)
            {
                // ... move the enemy down by the sinkSpeed per second.
                transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
            }

            if (timer >= effectsDisplayTime)
            {
                // ... disable the effects.
                DisableEffects();
            }
        }

        public void DisableEffects()
        {
            hitmarker.SetActive(false);
        }


        public void TakeDamage (int amount)
        {
            timer = 0f;

            // If the enemy is dead...
            if (isDead)
                // ... no need to take damage so exit the function.
                return;

            hitmarker.SetActive(true);

            // Play the hurt sound effect.
            enemyAudio.Play ();

            // Reduce the current health by the amount of damage sustained.
            currentHealth -= amount;

            // If the current health is less than or equal to zero...
            if(currentHealth <= 0)
            {
                // ... the enemy is dead.
                Death ();
            }
        }


        void Death ()
        {
            // The enemy is dead.
            isDead = true;

            // Turn the collider into a trigger so shots can pass through it.
            capsuleCollider.isTrigger = true;

            // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
            enemyAudio.clip = deathClip;
            enemyAudio.Play ();

            if (Random.Range(1, 11) > 7)
            {
                Instantiate(pickUp, transform.position, transform.rotation);
            }

            StartSinking();
        }


        public void StartSinking ()
        {
            // Find and disable the Nav Mesh Agent.
            GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;

            // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
            GetComponent <Rigidbody> ().isKinematic = true;

            // The enemy should no sink.
            isSinking = true;

            // Increase the score by the enemy's score value.
            ScoreManager.score += scoreValue;

            // After 2 seconds destory the enemy.
            Destroy (gameObject, 2f);
        }
    }
}