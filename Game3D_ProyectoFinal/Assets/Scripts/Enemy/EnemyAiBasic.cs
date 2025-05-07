using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Librería para usar clases de NavMesh

public class EnemyAiBasic : MonoBehaviour
{
    [Header("Ai Configuration")]
    [SerializeField] NavMeshAgent agent; //Ref al componente que permite que el objeto tenga ia
    [SerializeField] Transform target; //Ref al transform del objeto a perseguir
    [SerializeField] LayerMask targetLayer; //Determina cual es la capa de detección del target
    [SerializeField] LayerMask groundLayer; //Determina cual es la capa de detección del suelo

    [Header("Patroling Stats")]
    public Vector3 walkPoint; //Dirección a la que se moverá la IA si no detecta al target
    [SerializeField] float walkPointRange; //Rango ´máximo de dirección a generar
    bool walkPointset;//Determina si la IA a llegado al objetivo y entoces genera un nuevo objetivo

    [Header("Attack configuration")]
    public float timeBetweenAttacks; //Tiempo de espera entre ataque y ataque (Se suele igualar a la duracción de ataque)
    bool alreadyAttacked; //Determina si ya se a atacado (Evita atques infinitos seguidos)
                          //Variables que se usan si el ataque es a distancia
    [SerializeField] GameObject Projectile;
    [SerializeField] Transform shootPoint; //Ref a la posición desde donde se genera la bala
    [SerializeField] float shootSpeedZ; //Velocidad de la bala hacia delante
    [SerializeField] float shootSpeedY; //Velocidad de la bala hacia arriba (Solo si es catapulta con gravedad)

    [Header("States & Detection")]
    [SerializeField] float sightRange; //Distancia de detección del target de la IA
    [SerializeField] float attackRange; //Distancia a partir de la cual la IA ataca
    [SerializeField] bool targetInSightRange; //Determina si el target está a distancia de detección
    [SerializeField] bool targetInAttackRange; //Determina si el target está a distancia de ataque

    private void Awake()
    {
        target = GameObject.Find("Player").transform; //Al inicio del juego le dice que el target es el player
        agent = GetComponent<NavMeshAgent>(); //Al inicio del juego se referencia el componente del agente
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EnemyStateUpdater();
    }

    void EnemyStateUpdater()
    {
        //Chequear si el target está en los rangos de detección y/o ataque
        targetInSightRange = Physics.CheckSphere(transform.position, sightRange, targetLayer);
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, targetLayer);

        //Cambios dinámicos de estado de la IA 
        //Si no detecta al target ni está en rango de ataque : Patrulla
        if (!targetInSightRange && !targetInAttackRange) Patroling();
        //Si detecta al target pero no está en rango de ataque: Persigue
        if (targetInSightRange && !targetInAttackRange) ChaseTarget();
        //Si detecta al target y esta en rang ode ataque: Ataca 
        if (targetInSightRange && targetInAttackRange) AttackTarget();
    }

    void Patroling()
    {
        //Sistema de patrullaje

        if (!walkPointset)
        {
            //Si no hay punto al que dirijirse se genera 
            SearchWalkPoint();
        }
        else
        {
            //Si el punto generado es caminable, el agente lo perseguirá 
            agent.SetDestination(walkPoint);
        }

        //Sistema para que el agente busque un nuevo destino en caso de ya haya llegado al punto actual 
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1) walkPointset = false;

    }

    void SearchWalkPoint()
    {
        //Este método es un sistema de  generación de puntos a perseguir por el agente

        //sistema de generación de puntos a patrullar random
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //Determinamos el nuevo punto random a perseguir
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Detección si no hay suelo debajo, para evitar bucles infinitos
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointset = true; //Confirmamos que el punto es caminable, por lo que empezará el movimiento
        }


    }

    void ChaseTarget()
    {
        //Una vez detecta al target, el agente lo persigue 
        agent.SetDestination(target.position);

    }

    void AttackTarget()
    {
        //Cuando comienza a atacar, el agente se queda quieto (se persigue a si mismo)
        agent.SetDestination(transform.position);
        //El agente siempre observa directamente al target
        transform.LookAt(target);

        if (!alreadyAttacked)
        {
            //Si no estamos atacando, se comienza a atacar
            //Aquí iria el código de ataque a personalizar 

            //En este ejemplo, vamos a generar una bala, referenciar su rigidboy y empujarla por fuerzas
            Rigidbody rb = Instantiate(Projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shootSpeedZ, ForceMode.Impulse);
            //Si es modo catapulta, se añade la siguiente linea
            //rb.AddForce(transform.up * shootSpeedY, ForceMode.Impulse);

            //Se termina el ataque, empieza el cooldown de intervalo de ataque
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); //Vuelve a atacar en el intervalo de tiempo indicado, se suele timear con la animación de ataque

        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    //Función para que los Gizmos de detección (Perseguir/Ataque) se dibujen en escena al selecionar el objeto

    private void OnDrawGizmosSelected()
    {
        //Dibuja una esfera de color rojo que define el rango de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //Dibuja una esfera de color amarillo que define el rango de persecución
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }


}
