using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health System Configuration")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;

    [Header("Feedback Configuration")]
    [SerializeField] Color damageColor = Color.red;
    [SerializeField] float flashDuration = 0.1f;
    [SerializeField] GameObject deathEffect;

    MeshRenderer[] enemyRends;
    Color[] originalColors;

    private void Awake()
    {
        enemyRends = GetComponentsInChildren<MeshRenderer>();

        // Guardar el color original de cada renderer
        originalColors = new Color[enemyRends.Length];
        for (int i = 0; i < enemyRends.Length; i++)
        {
            if (enemyRends[i].material.HasProperty("_Color"))
                originalColors[i] = enemyRends[i].material.color;
            else
                originalColors[i] = Color.white; // Valor por defecto
        }

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameObject.SetActive(false);
            // Aquí puedes instanciar efectos de muerte si quieres
        }
    }

    public void TakeDamage(int damage)
    {
        AudioManager.Instance.PlaySFX(2);
        currentHealth -= damage;
        StartCoroutine(FlashDamageColor());
    }

    IEnumerator FlashDamageColor()
    {
        // Cambiar a color de daño
        for (int i = 0; i < enemyRends.Length; i++)
        {
            if (enemyRends[i].material.HasProperty("_Color"))
                enemyRends[i].material.color = damageColor;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restaurar colores originales
        for (int i = 0; i < enemyRends.Length; i++)
        {
            if (enemyRends[i].material.HasProperty("_Color"))
                enemyRends[i].material.color = originalColors[i];
        }
    }
}