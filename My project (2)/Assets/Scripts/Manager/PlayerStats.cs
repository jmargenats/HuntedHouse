using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Vida")]
    public int maxHP = 100;

    public int currentHP = 100;

    [Header("Daño")]
    public int baseFistDamage = 5;

    public int shovelDamage = 20;

    public int knifeDamage = 30;

    [Header("Atributos")]
    public int strength = 0;

    public int fistHits = 0;

    [Header("Arma equipada")]
    public string equippedWeapon;

    private bool isDead = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void RegisterFistHit()
    {
        fistHits++;

        if (fistHits >= 5)
        {
            fistHits = 0;

            strength++;

            Debug.Log(
                "Fuerza aumentó a "
                + strength
            );
        }
    }
    public int GetFistDamage()
    {
        return baseFistDamage + strength;
    }
    // =========================
    // ARMAS
    // =========================

    public void EquipWeapon(string weaponName)
    {
        equippedWeapon = weaponName;
    }

    public int GetWeaponDamage()
    {
        switch (equippedWeapon)
        {
            case "Pala":
                return shovelDamage;

            case "Cuchillo":
                return knifeDamage;

            default:
                return 0;
        }
    }

    // =========================
    // DAÑO
    // =========================

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHP -= damage;

        Debug.Log(
            "Player recibió "
            + damage
            + " de daño"
        );

        if (currentHP <= 0)
        {
            currentHP = 0;

            Die();
        }
    }

    // =========================
    // CURACIÓN
    // =========================

    public void Heal(int amount)
    {
        currentHP += amount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        Debug.Log(
            "Player recuperó "
            + amount
            + " HP"
        );
    }

    // =========================
    // MUERTE
    // =========================

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        StartCoroutine(
            DeathSequence()
        );
    }

    IEnumerator DeathSequence()
    {
        Debug.Log(
            "La oscuridad te consume..."
        );

        yield return new WaitForSeconds(2f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }

        ResetStats();

        SceneManager.LoadScene(
            "MainMenu"
        );
    }

    // =========================
    // RESET
    // =========================

    public void ResetStats()
    {
        currentHP = maxHP;

        equippedWeapon = "";

        isDead = false;
    }
}