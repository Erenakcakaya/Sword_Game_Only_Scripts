using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemy;
    public Transform player;
    public float lineOfSite;
    public float attackRange = 0.5f;
    public float attackRate;
    private float attackTime = 4f;
    private int e_currentAttack = 0;
    private float e_timeSinceAttack = 0.0f;
    public Transform attackPointEnemy;
    public LayerMask playerLayer;
    public float enemySpeed = 2;
    public Animator animator;
    public HealtBar healthBar;
    public Canvas canvarBar;
    float timer = 0f;
    float countTime = 0.5f;



    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.GetComponent<HeroKnight>();
        healthBar.setHealth(currentHealth, maxHealth);


    }
    void Update()
    {
        enemyAttack();
        if (player.position.x <= enemy.transform.position.x)
        {
            StartCoroutine(turnLeft());
        }
        else
        {
            StartCoroutine(turnRight());
        }

        if (player.GetComponent<HeroKnight>().isattack == true)
        {
            timer += Time.deltaTime;
            if (timer >= countTime)
            {
                timer = 0f;
                player.GetComponent<HeroKnight>().isattack = false;
            }
        }

    }

    //Karakter döndüğü zaman scale'leri değişmemesi için 
    IEnumerator turnLeft()
    {
        yield return new WaitForSeconds(0.5f);
        enemy.transform.localScale = new Vector2(-1.5f, 1.5f);
    }
    IEnumerator turnRight()
    {
        yield return new WaitForSeconds(0.5f);
        enemy.transform.localScale = new Vector2(1.5f, 1.5f);
    }

    void enemyAttack()
    {
        //Düşman oyuncuya belli bir mesafede ise onu takip etmeye başlar.
        float distanceFromPlayer = Vector2.Distance(player.position, enemy.transform.position);
        if (distanceFromPlayer < lineOfSite && distanceFromPlayer > attackRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, enemySpeed * Time.deltaTime);
            animator.SetInteger("AnimState", 1);
        }
        //Oyuncunun yanına gidip saldırma
        else if (distanceFromPlayer <= attackRange && Time.time >= attackTime)
        {
            if (player.GetComponent<HeroKnight>().currentHealth > 0)
            {
                animator.SetInteger("AnimState", 0);
                Collider2D[] hitplayer = Physics2D.OverlapCircleAll(attackPointEnemy.position, attackRange, playerLayer);

                foreach (Collider2D player in hitplayer)
                {
                    if (player.GetComponent<HeroKnight>() != null)
                    {
                        player.GetComponent<HeroKnight>().TakeDamagePlayer(5);
                        e_currentAttack++;

                        if (e_currentAttack > 3)
                            e_currentAttack = 1;

                        if (e_timeSinceAttack > 1.0f)
                            e_currentAttack = 1;

                        animator.SetTrigger("Attack" + e_currentAttack);

                        e_timeSinceAttack = 0.0f;
                        attackTime = Time.time + 1f / attackRate;
                    }
                    else
                    {
                        animator.SetTrigger("Attack" + 1);
                    }
                }
            }
            else
            {
                StartCoroutine(waitTurn());
            }
        }
        else
        {
            animator.SetInteger("AnimState", 0);
        }

    }

    IEnumerator waitTurn()//Düşmanı döndürür.
    {
        yield return new WaitForSeconds(0.5f);
        enemy.GetComponent<SpriteRenderer>().flipX = true;
    }
    public void TakeDamage(int damage)
    {
        //Oyuncunun atağı başarılı ise; animasyon çalışır, canı azalır, can barı değişir.
        player.GetComponent<HeroKnight>().isattack = true;
        animator.SetTrigger("Hurt");
        currentHealth -= damage;
        healthBar.setHealth(currentHealth, maxHealth);



        //Düşmanı döndürür
        if (enemy.transform.localScale.x == -1.5f)
        {
            Vector2 sPos = new Vector2(enemy.transform.position.x + 0.4f, enemy.transform.position.y);
            enemy.transform.position = Vector2.Lerp(enemy.transform.position, sPos, Time.time);
        }
        else
        {
            Vector2 sPos = new Vector2(enemy.transform.position.x - 0.4f, enemy.transform.position.y);
            enemy.transform.position = Vector2.Lerp(enemy.transform.position, sPos, Time.time);
        }

        //Canı sıfırdan az ise Die fonksiyonunu çağırır.
        if (currentHealth <= 0)
        {
            Die();

        }
    }

    void Die()
    {
        canvarBar.enabled = false;
        animator.SetBool("isDead", true);
        GetComponent<CapsuleCollider2D>().enabled = false;
        this.enabled = false;

        StartCoroutine(DestroyEnemy());
    }

    IEnumerator DestroyEnemy()//5 saniye sonra ölü düşman yok olur.
    {
        yield return new WaitForSeconds(5);
        enemy.SetActive(false);
    }
    private void OnDrawGizmosSelected()
    {
        //Düşmanın attack menzili
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(enemy.transform.position, lineOfSite);

        Gizmos.DrawWireSphere(attackPointEnemy.transform.position, attackRange);

        if (attackPointEnemy == null)
        {
            return;
        }
    }

}
