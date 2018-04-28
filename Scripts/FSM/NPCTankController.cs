using UnityEngine;
using System.Collections;

public class NPCTankController : AdvancedFSM 
{
    public GameObject Bullet;
    public int health;

    float currentSpeed;
    float currentRotSpeed;

    public bool rageMode;
    public const int OBSTACLE_MASK = 8;

    public int Health
    {
        get
        {
            return health;
        }

      
    }

    //private Transform turret;
    public int maxHealth;
    //Initialize the Finite state machine for the NPC tank
    protected override void Initialize()
    {
        health = 100;

        elapsedTime = 0.0f;
        shootRate = 2.0f;
        maxHealth = health;

        //Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        if (!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");

        //Get the turret of the tank
        tankTurret = gameObject.transform.GetChild(0).transform;
        bulletSpawnPoint = tankTurret.GetChild(0).transform;

        //Start Doing the Finite State Machine
        ConstructFSM();
       
    }

    //Update each frame
    protected override void FSMUpdate()
    {
        //Check for health
        elapsedTime += Time.deltaTime;

		//Make the AI reason and act based on its current state
		CurrentState.Reason(playerTransform, transform);
		CurrentState.Act(playerTransform, transform);
    }

    public void SetTransition(Transition t) 
    { 
        PerformTransition(t); 
    }

    private void ConstructFSM()
    {
        //Get the list of points
        pointList = GameObject.FindGameObjectsWithTag("WanderPoint");

        Transform[] waypoints = new Transform[pointList.Length];
        int i = 0;
        foreach(GameObject obj in pointList)
        {
            waypoints[i] = obj.transform;
            i++;
        }
		
		//Add Transition and State Pairs
        PatrolState patrol = new PatrolState(waypoints, this);
		//If the tank sees the player while patrolling, move to chasing state
        patrol.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
		//If the tank loses health while patrolling, move to dead state
        patrol.AddTransition(Transition.NoHealth, FSMStateID.Dead);
        patrol.AddTransition(Transition.HalfHealth, FSMStateID.Rage);

        ChaseState chase = new ChaseState(waypoints, this);
		//If the tank loses the player while chasing, move to patrol state
        chase.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
		//If the tank reaches the player while attacking, move to attack state
        chase.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
		//If the tank loses health while patrolling, move to dead state
        chase.AddTransition(Transition.NoHealth, FSMStateID.Dead);
        chase.AddTransition(Transition.HalfHealth, FSMStateID.Rage);

        AttackState attack = new AttackState(waypoints, this);
		//If the tank loses the player while attacking, move to patrol state
        attack.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
		//If the player is within sight while attacking, move to chase state
        attack.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
		//If the tank loses health while attacking, move to dead state
        attack.AddTransition(Transition.NoHealth, FSMStateID.Dead);
        attack.AddTransition(Transition.HalfHealth, FSMStateID.Rage);

        DeadState dead = new DeadState(this);
		//When there is no health, go to dead state
        dead.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        RageState rage = new RageState(this);
        rage.AddTransition(Transition.NoHealth, FSMStateID.Dead);
        rage.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);

		//Add states to the state list
        AddFSMState(patrol);
        AddFSMState(chase);
        AddFSMState(attack);
        AddFSMState(dead);
        AddFSMState(rage);
    }

    /// <summary>
    /// Check the collision with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Reduce health
        if (collision.gameObject.tag == "Bullet")
        {
            health -= this.Bullet.GetComponent<Bullet>().damage;

            if (health <= 0)
            {
                Debug.Log("Switch to Dead State");
                SetTransition(Transition.NoHealth);
                Explode();
            }
        }
       
    }

  


    protected void Explode()
    {
        float rndX = Random.Range(10.0f, 30.0f);
        float rndZ = Random.Range(10.0f, 30.0f);
        for (int i = 0; i < 3; i++)
        {
            GetComponent<Rigidbody>().AddExplosionForce(10000.0f, transform.position - new Vector3(rndX, 10.0f, rndZ), 40.0f, 10.0f);
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 20.0f, rndZ));
        }

        Destroy(gameObject, 1.5f);
    }

    /// <summary>
    /// Shoot the bullet from the turret
    /// </summary>
    public void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            elapsedTime = 0.0f;
        }
    }

    public void PointTurret()
    {
        //tankTurret.LookAt(playerTransform);
        tankTurret.rotation = Quaternion.Slerp(tankTurret.rotation, Quaternion.LookRotation((playerTransform.position - tankTurret.position).normalized), Time.deltaTime * 50f);
    }

    public void MoveToTarget(Vector3 target, float speed, float rotSpeed)
    {
        Vector3 direction = target - transform.position;
        direction.Normalize();

        currentSpeed = speed;
        currentRotSpeed = rotSpeed;
        //transform.position += direction / 10f;
        //transform.position += direction * 10f * Time.deltaTime;
        //transform.rotation = Quaternion.LookRotation(direction);

        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
       

        tankTurret.rotation = transform.rotation;

        //AvoidObstacles(direction);


        transform.position += transform.forward * currentSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), currentRotSpeed * Time.deltaTime);

    }

    //public void AvoidObstacles(Vector3 dir)
    //{
    //    var leftR = transform.position;
    //    var rightR = transform.position;
    //    var midR = transform.position;
    //    midR.y += 5;
    //    leftR.x -= 20;

    //    rightR.x += 20;

    //    Create Rays
    //    Ray midRay = new Ray(midR, transform.forward * 700);
    //    Ray leftRay = new Ray(leftR, transform.forward * 700);
    //    Ray rightRay = new Ray(rightR, transform.forward * 700);

    //    RaycastHit hit;
    //    if (Physics.Raycast(midRay, out hit))
    //    {

    //        if (hit.transform.CompareTag("obstacle"))
    //        {
    //            dir += hit.normal * 400;
    //            Debug.Log("Hit");
    //        }



    //    }
    //    if (Physics.Raycast(leftRay, out hit))
    //    {

    //        if (hit.transform.CompareTag("obstacle"))
    //        {
    //            Debug.Log("Hit");
    //            dir += hit.normal * 400;
    //        }


    //    }
    //    if (Physics.Raycast(rightRay, out hit))
    //    {

    //        if (hit.transform.CompareTag("obstacle"))
    //        {
    //            Debug.Log("Hit");
    //            dir += hit.normal * 400;
    //        }


    //    }




    //    Debug.DrawRay(transform.position, transform.forward * 200, Color.red);
    //    Debug.DrawRay(leftR, transform.forward * 200, Color.red);
    //    Debug.DrawRay(rightR, transform.forward * 200, Color.red);

    //}




    public void RageEffect(float nShootRate, float nMoveSpeed)
    {
        this.GetComponent<MeshRenderer>().material.color = Color.blue;
        shootRate = nShootRate;
        currentSpeed = nMoveSpeed;
        rageMode = true;

    }

}
