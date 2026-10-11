using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerHealth = player.GetComponent<PlayerHealth>();
        }

        if (player != null && other.gameObject == player)
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (player != null && other.gameObject == player)
            playerInRange = false;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerHealth = player.GetComponent<PlayerHealth>();
        }

        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth != null && enemyHealth.currentHealth > 0)
            Attack();
        if (playerHealth != null && playerHealth.currentHealth <= 0 && anim != null)
            anim.SetTrigger("PlayerDead");
    }

    void Attack()
    {
        timer = 0f;
        if (playerHealth != null && playerHealth.currentHealth > 0)
            playerHealth.TakeDamage(attackDamage);
    }
}
