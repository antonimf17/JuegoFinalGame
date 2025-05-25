using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Libreria para usar clases de NavMesh.

public class Prueba : MonoBehaviour
{
    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent; // Componente que permite al objeto tener IA.
    [SerializeField] Transform target; // Transform del objeto a perseguir.
    [SerializeField] LayerMask targetLayer; // Capa de detección del target.
    [SerializeField] LayerMask groundLayer; // Capa de detección del suelo.

    [Header("Patroling Stats")]
    public Vector3 walkPoint; // Dirección a la que se movera la IA si no se detecta al target.
    [SerializeField] float walkPointRange; // Distancia máxima de dirección a generar.
    [SerializeField] bool walkPointSet; // Determina si la IA ha llegado al objetivo

    [Header("Attack Configuration")]
    public float timeBetweenAttacks; // Tiempo de espera entre ataques.
    private bool alredyAttacked; // Determina si ya ha atacado.

    // Variables para ataques a distancia:
    [SerializeField] GameObject projectile; // Referencia de la bala física.
    [SerializeField] Transform shootPoint; // Punto desde donde se genera la bala.
    [SerializeField] float shootSpeedZ; // velocidad frontal de la bala.
    [SerializeField] float shootSpeedY; // Velocidad vertical de la bala (solo si le afecta la gravedad).

    [Header("States & Detection")]
    [SerializeField] float sightRange; // Distancia de detección del target de la IA.
    [SerializeField] float attackRange; // Distancia de ataque.
    [SerializeField] bool targetInSightRange; // Determina si el target esta a distancia de detección.
    [SerializeField] bool targetInAttacktRange; // Determina si el target esta a distancia de ataque.

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        EnemyStateUpdater();
        

        if (!targetInAttacktRange)
        {
            Vector3 velocity = agent.velocity;

            if (velocity.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }

    void EnemyStateUpdater()
    {
        // Revisar si el target esta en los rangos de detección y/o ataque:
        targetInSightRange = Physics.CheckSphere(transform.position, sightRange, targetLayer);
        targetInAttacktRange = Physics.CheckSphere(transform.position, attackRange, targetLayer);

        // Cambios dinámicos de estado de la IA:
        // Orden de prioridades: ataque > persecución > patrulla.
        if (!targetInSightRange && !targetInAttacktRange)
        {
            Patroling();
        }
        if (targetInSightRange && !targetInAttacktRange)
        {
            ChaseTarget();
        }
        if (targetInSightRange && targetInAttacktRange)
        {
            AttackTarget();
        }
    }

    void Patroling()
    {
        if (!walkPointSet)
        {
            // Genera un punto de caminado nuevo:
            SearchWalkPoint();
        }
        else
        {
            // Mueve el agente al nuevo punto de caminado:
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1)
        {
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        // Generación de nuevo punto de caminado:
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        // Fijación nuevo punto de caminado:
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Comprobación de si el nuevo punto de caminado es válido:
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }

    void ChaseTarget()
    {
        agent.SetDestination(target.position);
    }

    void AttackTarget()
    {
        // Antes de atacar...:
        agent.SetDestination(transform.position); // Evita que se mueva.
        transform.LookAt(target);

        if (!alredyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shootSpeedZ, ForceMode.Impulse);
            //rb.AddForce(transform.up * shootSpeedY, ForceMode.Impulse); // Solo si le afecta la gravedad.

            // Añade un intervalo entre ataques.
            alredyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alredyAttacked = false;
    }

    // Función para que los Gizmos de detección (perseguir/ataque) se dibujen en la escena al seleccionar el objeto.
    private void OnDrawGizmosSelected()
    {
        // Dibuja el rango de ataque:
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Dibuja el rango de persecución:
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
