using UnityEngine;
using System.Collections;
using System;

public class Bandit : MonoBehaviour
{

    [SerializeField] float m_speed;
    [SerializeField] float m_jumpForce = 7.5f;
    public int health;
    private bool check_hero;
    private bool checkRun;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;
    private static float m_timeSinceAttack = 0;
    private static float m_timeStartGame = 0;
    public int damage;
    //private readonly HeroKnight hero;
    public Transform atackPos;
    public LayerMask hero;
    public float atackRange;
    private static int heroHealth;
    private static float a;
    private static Transform player;

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        check_hero = false;
        checkRun = true;
        heroHealth = 10;
        a = 0;
        player = GameObject.FindGameObjectWithTag("Hero").GetComponent<Transform>();
    }
    public void TakeDamage(int damage)
    {
        m_animator.SetTrigger("Hurt");
        health -= damage;
        if (health == 0)
        {
            m_animator.SetTrigger("Death");
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            check_hero = false;
            checkRun = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (health > 0)
        {
            if (collision.CompareTag("Hero"))
            {
                check_hero = false;
                checkRun = true;
            }
        }
        else
        {
            check_hero = false;
            checkRun = false;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (health > 0)
        {
            if (collision.CompareTag("Hero"))
            {
                check_hero = true;
                checkRun = false;
            }
        }
        else
        {
            check_hero = false;
            checkRun = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(atackPos.position, atackRange);
    }

    void Update()
    {

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }
        m_timeSinceAttack += Time.deltaTime;
        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
        m_timeStartGame += Time.deltaTime;
        if (check_hero)
        {
            if (Math.Round(m_timeSinceAttack) > 2 && heroHealth > 0)
            {
                m_animator.SetTrigger("Attack");
                m_timeSinceAttack = 0;
            }
        }

        if (heroHealth <= 0)
        {
            check_hero = false;
            checkRun = false;
        }

        //Debug.Log(check);

        // -- Handle input and movement --
        //float inputX = Input.GetAxis("Horizontal");

        ////Swap direction of sprite depending on walk direction
        //if (inputX > 0)
        //    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        //else if (inputX < 0)
        //    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //// Move
        ///*m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y)*/
        //;

        ////Set AirSpeed in animator
        ////m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        //// -- Handle Animations --
        ////Death
        //if (Input.GetKeyDown("e")) {
        //    if(!m_isDead)
        //        m_animator.SetTrigger("Death");
        //    else
        //        m_animator.SetTrigger("Recover");

        //    m_isDead = !m_isDead;
        //}

        //Hurt
        if (Math.Round(m_timeStartGame) > 5 && checkRun)
        {
            m_animator.SetInteger("AnimState", 2);
            transform.position = Vector2.MoveTowards(transform.position, player.position, m_speed * Time.deltaTime * 0.5f);
            if (transform.position.x < player.position.x)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded)
        {
            //m_animator.SetTrigger("Jump");
            //m_grounded = false;
            //m_animator.SetBool("Grounded", m_grounded);
            //m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            //m_groundSensor.Disable(0.2f);
        }

        //Run
        //else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        //    m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    public void Attack()
    {
        Collider2D[] heroes = Physics2D.OverlapCircleAll(atackPos.position, atackRange, hero);
        for (int i = 0; i < heroes.Length; i++)
        {
            heroes[i].GetComponent<HeroKnight>().TakeDamage(damage);
            heroHealth = heroes[i].GetComponent<HeroKnight>().health;
        }
    }
}
