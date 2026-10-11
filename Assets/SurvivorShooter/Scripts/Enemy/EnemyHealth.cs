using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;

    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        currentHealth = startingHealth;
    }

    void Update()
    {
        if (isSinking)
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead) return;

        if (enemyAudio != null)
            enemyAudio.Play();

        currentHealth -= amount;

        if (hitParticles != null)
        {
            hitParticles.transform.position = hitPoint;
            hitParticles.Play();
        }

        if (currentHealth <= 0)
            Death();
    }

    void Death()
    {
        isDead = true;
        if (capsuleCollider != null)
            capsuleCollider.isTrigger = true;
        if (anim != null)
            anim.SetTrigger("Dead");
        if (enemyAudio != null && deathClip != null)
        {
            enemyAudio.clip = deathClip;
            enemyAudio.Play();
        }
        ScoreManager.score += scoreValue;

        // Ensure enemy sinks and is cleaned up even if animation event is not present
        Invoke("StartSinking", 1.5f);
    }

    public void StartSinking()
    {
        if (isSinking) return;
        var nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (nav != null) nav.enabled = false;
        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        isSinking = true;
        Destroy(gameObject, 2f);
    }
}
