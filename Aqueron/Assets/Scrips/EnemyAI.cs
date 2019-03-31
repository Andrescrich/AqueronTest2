using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{

    public Transform target;
    public float RayLength = 0.1f;
    public float height = 0.5f;
    private Vector3 upE;
    private Vector3 up;
    private bool encontrado = false;
    private Collider2D colider;
    public Animator animator;
    public RigidbodyType2D prb;
    public bool m_FacingRight = true;
    public float aggro = 5;
    public float velocidad;
    private bool isGrounded;
    private bool isBlocked;

    private Vector3 aggroUp;
    private Vector3 aggroDown;
    private Vector3 aggroLeft;
    private Vector3 aggroRight;

    private bool aggroUpCast;
    private bool aggroDownCast;
    private bool aggroLeftCast;
    private bool aggroRightCast;


    public float width;
    LayerMask Obstacle;
    LayerMask Player;

    //How many times each second we will update the path
    public float updateRate = 2f;

    //Caching
    private Seeker seeker;
    private Rigidbody2D rb;

    //Calculated path
    public Path path;

    //AI speed
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    //Max distance from the AI to waypoint for it to continue to the next waypoint
    public float nextWayPointDistance = 3f;

    //Waypoint we are currently moving towards
    private int currentWayPoint = 0;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(14, 12);
        //Posicion inicial del enemy
        upE = transform.position;
        //Posicion hasta la cual sube el enemy
        up = transform.position + new Vector3(0, height);

        animator = GetComponent<Animator>();
        colider = GetComponent<Collider2D>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        width = GetComponent<SpriteRenderer>().bounds.extents.x;    

        if (target == null)
        {
            Debug.LogError("No player found");
            return;
        }
        //Empieza a buscarte al cargar el nivel
        // StartCoroutine(UpdatePath());

        Obstacle = 1 << LayerMask.NameToLayer("Obstacle");
        Player = 1 << LayerMask.NameToLayer("Player");
    }
    

    IEnumerator UpdatePath ()
    {
        if (target == null)
        {
            yield return false; 
        }
       
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            yield return new WaitForSeconds(1f / updateRate);
            StartCoroutine(UpdatePath());
        
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path- Did it have an error?" + p.error);
            if (!p.error)
            {
                path = p;
                currentWayPoint = 0;
            }
        }

    private void FixedUpdate()
    {

        if (target == null)
        {
            return;
        }
        //  if (path == null) return;      


        aggroUp = transform.position + new Vector3(0f, aggro);
        aggroDown = transform.position + new Vector3(0f, -aggro);
        aggroRight = transform.position + new Vector3(aggro, 0f);
        aggroLeft = transform.position + new Vector3(-aggro, 0f);
        //Lineas de limite de aggro
        Debug.DrawLine(aggroUp + Vector3.right * aggro, aggroUp + Vector3.left * aggro, Color.green);
        Debug.DrawLine(aggroDown + Vector3.right * aggro, aggroDown + Vector3.left * aggro, Color.green);
        Debug.DrawLine(aggroRight + Vector3.down * aggro, aggroRight + Vector3.up * aggro, Color.green);
        Debug.DrawLine(aggroLeft + Vector3.down * aggro, aggroLeft + Vector3.up * aggro, Color.green);
        //Linecasts
        aggroUpCast = Physics2D.Linecast(aggroUp + Vector3.right * aggro, aggroUp + Vector3.left * aggro, Player);
        aggroDownCast = Physics2D.Linecast(aggroDown + Vector3.right * aggro, aggroDown + Vector3.left * aggro, Player);
        aggroRightCast = Physics2D.Linecast(aggroRight + Vector3.down * aggro, aggroRight + Vector3.up * aggro, Player);
        aggroLeftCast = Physics2D.Linecast(aggroLeft + Vector3.down * aggro, aggroLeft + Vector3.up * aggro, Player);


        //Empieza a buscarte cuando uno de los linecasts te detecta 
        if (!encontrado)
        {
            if (aggroDownCast || aggroUpCast || aggroLeftCast || aggroRightCast)
            {
                FindObjectOfType<AudioManager>().Play("EnemyActivated");
                StartCoroutine(UpdatePath());
                encontrado = true;
            }
            else if (!aggroDownCast && !aggroUpCast && !aggroLeftCast && !aggroRightCast)
            {
                if (gameObject.CompareTag("Rat")) NoAggroRatMovement();
                if (gameObject.CompareTag("Duck")) NoAggroDuckMovement();
            }
        }   

        if (currentWayPoint >= path.vectorPath.Count)
        {
            if (pathIsEnded) return;
            Debug.Log("End of path reached");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //siguiente punto - posicion actual = dirrecion
        Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position);
        dir *= speed * Time.fixedDeltaTime;
        Debug.Log(dir);

        if (gameObject.CompareTag("Rat"))
            animator.SetFloat("Speed", 1);
        rb.AddForce(new Vector3(dir.x, 0f), fMode);
        if (gameObject.CompareTag("Duck"))
            rb.AddForce(dir, fMode);

        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < nextWayPointDistance) {
            currentWayPoint++;
            return;
        }

        if (dir.x > 0 && m_FacingRight)
        {
            rb.velocity = Vector3.zero;
            Flip();
        }
        else if (dir.x < 0 && !m_FacingRight)
        {
            rb.velocity = Vector3.zero;
            Flip();
        }
    }

    // Switch the way the player is labelled as facing.
    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    //Cuando colisiona con una bala, desactiva colider, hace animacion y muere haciendo ruido
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            rb.velocity = Vector3.zero;
            if (collision.transform.rotation.y < 0)
            {
                rb.AddForce(new Vector2(-350f, 600f));
            }
            else
            {
                rb.AddForce(new Vector2(350f, 600f));
            }
            colider.enabled = false;
            animator.SetBool("IsDying", true);
            Invoke("Death", .5f);
            FindObjectOfType<AudioManager>().Play("HitEnemy");
        }
    }

    //Elimina enemigo
    private void Death()
    {
        Destroy(gameObject);
    }


    //Movimiento arriba y abajo de los pajaros
    void NoAggroDuckMovement()
    {
        Debug.DrawLine(upE, upE + Vector3.left * RayLength, Color.red);
        Debug.DrawLine(up, up + Vector3.left * RayLength, Color.red);
         rb.AddForce(new Vector2(0, 0.5f));
        if (gameObject.transform.position.y > up.y)
           rb.AddForce(new Vector2(0, -1.5f));    
    }

    void NoAggroRatMovement()
    {
        Vector2 vec2 = ToVector2(transform.right) * .02f;
        Vector2 lineCastPos = transform.position - transform.right * width;
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, Obstacle);
        isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - vec2, Obstacle);
        Debug.DrawLine(lineCastPos, lineCastPos - vec2);

         if (!isGrounded) Flip();
         if (isBlocked) Flip();

        Vector2 myVel = rb.velocity;
        myVel.x = -transform.right.x * velocidad;
        rb.velocity = myVel;
    }

    private Vector3 ToVector2(Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }
} 
