using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroKnight : MonoBehaviour
{

    [SerializeField] public float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;

    [SerializeField] GameObject m_slideDust;
    [SerializeField] private GameObject player;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int currentHealth;
    [SerializeField] private GameObject cam;

    [SerializeField] private GameObject fadeToBlack;
    public GameObject cameraObject;

    [SerializeField] private GameObject infoText;



    public Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    [SerializeField] private bool m_rolling = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;
    public HealtBar healthBar;
    public Canvas canvarBar;
    public int combo;
    public float timer = 0f;
    float countTime = 5f;
    public bool isattack;
    public bool newPumpkin;


    void Start()
    {
        infoText.SetActive(false);
        combo = 0;
        currentHealth = maxHealth;
        healthBar.setHealth(currentHealth, maxHealth);
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update()
    {
        if (newPumpkin)//Balkabağı taşıyorsa sprite'ı açılır ve oyuncunun hızı düşer.
        {
            this.gameObject.transform.GetChild(8).gameObject.SetActive(true);
            m_speed = 2;
        }
        else
        {
            //Balkabağı taşımıyorsa:
            m_speed = 4;
            this.gameObject.transform.GetChild(8).gameObject.SetActive(false);
        }
        if (player.transform.position.y < -20)//Oyuncu düşerse ölme fonksiyonu çağırılır.
        {
            Die();
        }
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
        {
            m_rolling = false;
            m_rollCurrentTime = 0;
        }

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1);
            m_facingDirection = 1;
        }

        else if (inputX < 0)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1);
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);


        if (combo > 0 && !isattack)//Kombo sayacı
        {
            //5 saniye içinde attack yapıp düşmana vuramazsa combo sıfırlanır.
            timer += Time.deltaTime;
            if (timer >= countTime)//Timer 5 olursa combo sıfırlanır.
            {
                timer = 0f;
                combo = 0;
            }
        }
        else if (isattack)
        {
            timer = 0f;//Attack başarılı olursa timer 0 olur.
        }


        //Attack
        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling && !newPumpkin)//Balkabağı taşırken saldırı yapamaz.
        {
            Attack();//Saldırı Fonksiyonu çağırılır.
            m_currentAttack++;


            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.5f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;

        }


        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding && !newPumpkin)//Balkabağı taşırken takla atamaz.
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);

        }


        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling && !newPumpkin)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

    }


    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    public void TakeDamagePlayer(int damage)
    {
        m_animator.SetTrigger("Hurt");//Hasar alma animasyonunu çalıştırır.
        currentHealth -= damage;//Hasarı azaltır.
        healthBar.setHealth(currentHealth, maxHealth);//Can barını cana göre düzenleyen fonksiyonu çağırır.

        if (currentHealth <= 0)
        {
            Die();//ölür.
        }
        else
        {
            cam.GetComponent<CameraShake>().Shake(0.04f, 0.2f);//Hasar alırsa kamera sallanmasını sağlayan fonksiyonu çağırır.
        }

    }

    void Die()
    {
        canvarBar.enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;//Ölen düşman veya oyuncunun colliderini kapatır.
        m_animator.SetBool("isDead", true);//Ölüm animasyonunu çalıştırır.
        this.enabled = false;//Script'i kapatır.

        cam.GetComponent<CameraShake>().Shake(0.1f, 0.2f);//Kamera sallanmasını sağlayan fonksiyonu çağırır.

        //Kamerayı belli bir hızda yukarı kaldırır.
        cam.GetComponentInParent<CameraFollow>().yPos = 5.6f;
        cam.GetComponentInParent<CameraFollow>().speed = 1f;

        //Kamera kararması
        Color newcolor = fadeToBlack.transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
        newcolor.a = 0.8f;
        fadeToBlack.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = newcolor;

    }

    void Attack()
    {
        //Saldırı mesafesi içindeki düşmanları diziye alır.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Enemy>() != null)
            {
                enemy.GetComponent<Enemy>().TakeDamage(20);//Hasar veren fonksiyonu çağırır.
                combo++;//Kombo sayısını arttırır.
                cam.GetComponent<CameraShake>().Shake(0.01f, 0.1f);//Kamera sallanmasını sağlayan fonksiyonu çağırır.

                if (m_currentAttack == 4)
                {
                    //Düşmanın ne tarafta olduğuna göre çalışır ve düşmanı geriye iter.
                    if (enemy.transform.localScale.x == -1.5f)
                    {
                        Vector2 sPos = new Vector2(enemy.transform.position.x + 0.9f, enemy.transform.position.y);
                        enemy.transform.position = Vector2.Lerp(enemy.transform.position, sPos, Time.time);
                    }
                    else
                    {
                        Vector2 sPos = new Vector2(enemy.transform.position.x - 0.9f, enemy.transform.position.y);
                        enemy.transform.position = Vector2.Lerp(enemy.transform.position, sPos, Time.time);
                    }
                }

            }
            else
            {
                m_animator.SetTrigger("Attack" + m_currentAttack);//Animator'deki animasyonları uygular.
            }
            if (enemy.GetComponent<Enemy>().currentHealth <= 0)
            {
                isattack = false;
            }
        }


    }


    private void OnTriggerStay2D(Collider2D other)
    {
        //OnTriggerStay2D yerine OnTriggerEnter2D'ye bir bool atayıp içine girdiğinde true çıktığında false yapsaydım ve Update içinde aşağıdaki kodları kullansaydım arada çıkan takılma bug'ları düzelecekti. 
        //Zaten bu şekilde sağlıklı sonuç vermiyor. O an düşünememişim.
        if (other.gameObject.tag == "Speaker")
        {
            //Karakterin üzerinde tuşa basabileceğini hatırlatan bir bilgi yazısı çıkıyor.
            infoText.SetActive(true);
            //Konuşmacı olarak tanımladığım objenin altından DialogueBox'u buluyor. (Tüm Speaker nesnelerinde DialogueBox aynı yerde)
            Transform child = other.transform.GetChild(0).GetChild(0).Find("DialogueBox");
            if (child.GetComponentInChildren<Dialogue>().isOpen == false)//Konuşma başlamamışsa
            {
                if (Input.GetKey(KeyCode.F))
                {
                    child.gameObject.SetActive(true);//DialogueBox aktif hale gelir.
                    child.GetComponentInChildren<Dialogue>().isOpen = true;//Konuşma başlamıştır bitene kadar true kalır.
                    child.GetComponentInChildren<Dialogue>().textMeshProUGUI.text = string.Empty;//Text'i en başta boşaltır.
                    child.GetComponentInChildren<Dialogue>().StartDialogue();
                    //Inspector'dan konulan sprite'ı DialogueBox'takine eşitler.
                    child.GetComponentInChildren<Dialogue>().dialogueImage.GetComponent<Image>().sprite = other.transform.GetChild(0).GetChild(0).GetComponentInChildren<Dialogue>().sprite;

                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        infoText.SetActive(false);//Karakterin üzerinde tuşa basabileceğini hatırlatan bir bilgi yazısını kapatır.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Pumpkin")
        {
            //Balkabağı alındığında collider ve sprite'ı kapatılır.
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            newPumpkin = true;//Alındığı için true değer alır.
        }
        if (other.gameObject.tag == "WalkingSpeaker" && newPumpkin)//Balkabağı varken olacak olan konuşmayı başlatır.
        {
            other.GetComponent<WalkDialogue_House>().textMeshProUGUI.text = string.Empty;
            other.GetComponent<WalkDialogue_House>().StartDialogue();
            other.GetComponent<WalkDialogue_House>().dialogueImage.GetComponent<Image>().sprite = other.GetComponent<WalkDialogue_House>().sprite;


        }
    }

    void OnDrawGizmosSelected()
    {
        //Saldırı mesafesini ayarlar.
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
