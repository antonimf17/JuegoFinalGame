using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health System Configuration")]
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;

    [Header("Feedback Configuration")]
    [SerializeField] Material baseMat;
    [SerializeField] Material damagedMat;
    [SerializeField] GameObject deathEffect;

    MeshRenderer enemyRend;

    private void Awake()
    {
        enemyRend = GetComponent<MeshRenderer>();
        baseMat = enemyRend.material;
        currentHealth = maxHealth;
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameObject.SetActive(false);

        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        enemyRend.material = damagedMat;
        Invoke(nameof(ResetdamageMaterial), 0.1f);
    }
    void ResetdamageMaterial()
    {
        enemyRend.material = baseMat;
    }
}
