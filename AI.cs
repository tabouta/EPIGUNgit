using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class AI : MonoBehaviour
    {

        public int sante = 100;

        public NavMeshAgent agent;
        public GameObject player;
        public LayerMask whatIsGround, whatIsPlayer;

        //Patroling
        public Vector3 walkPoint;
        bool walkPointSet;
        public float walkPointRange;

        //Attacking
        public float timeBetweenAttacks;
        bool AlreadyAttacked;

        //States
        public float sightRange, attackRange;
        public bool playerInSightRange, playerInAttackRange;

        public bool attack = false;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            player = null;

            foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            {
                player = p;

                playerInSightRange = Vector3.Distance(player.transform.position, transform.position) < sightRange;
                playerInAttackRange = Vector3.Distance(player.transform.position, transform.position) < attackRange;
                if (playerInSightRange)
                    break;
            }

            if (player == null)
                return;

            if (!playerInSightRange && !playerInSightRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }

        private void Patroling()
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        }

        private void SearchWalkPoint()
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            walkPointSet = true;
        }
        private void ChasePlayer()
        {
            agent.SetDestination(player.transform.position);
        }
        private void AttackPlayer()
        {
            attack = true;
            agent.SetDestination(transform.position);

            transform.LookAt(player.transform);
            if (!AlreadyAttacked)
            {
                //Put attack code here

                AlreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }

        private void ResetAttack()
        {
            AlreadyAttacked = false;
        }

        private bool TakeDamages(int d)
        {
            sante -= d;
            if (sante <= 0)
            {
                GameObject.Destroy(gameObject);
                return true;
            }
            return false;
        }
    }
}