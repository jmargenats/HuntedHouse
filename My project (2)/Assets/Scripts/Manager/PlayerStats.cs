using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Vida")]
    public int maxHP = 100;
    public int currentHP = 100;

    [Header("Arma equipada")]
    public string equippedWeapon;

    [Header("Daño")]
    public int fistDamage = 10;
    public int shovelDamage = 20;
    public int knifeDamage = 35;

    [Header("Defensa")]
    public bool defending = false;

    [Range(0f, 1f)]
    public float defendMultiplier = 0.3f;

    [Header("Sedante")]
    public bool sedativeActive = false;
    public int sedativeTurns = 0;
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
        if (defending)
        {
            damage = Mathf.CeilToInt(
                damage * defendMultiplier
            );

            defending = false;

            Debug.Log(
                "Bloqueaste parte del daño."
            );
        }
        if (sedativeActive)
        {
            damage = Mathf.CeilToInt(
                damage * 0.5f
            );

            sedativeTurns--;

            if (sedativeTurns <= 0)
            {
                sedativeTurns = 0;
                sedativeActive = false;

                Debug.Log(
                    "El efecto del sedante terminó."
                );
            }
        }
        currentHP -= damage;

        Debug.Log(
            "Player recibió "
            + damage
            + " de daño"
        );

        if (currentHP < 0)
            currentHP = 0;
        if (currentHP <= 0)
        {
            Die();
        }
    }
    public int CalculateDamageTaken(int damage)
    {
        if (defending)
        {
            damage = Mathf.CeilToInt(
                damage * defendMultiplier
            );
        }

        if (sedativeActive)
        {
            damage = Mathf.CeilToInt(
                damage * 0.5f
            );
        }

        return damage;
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
    // DEFENSA Y SEDANTE
    // =========================
    public void ActivateDefense()
    {
        defending = true;
    }

    public void ActivateSedative()
    {
        sedativeActive = true;
        sedativeTurns = 3;
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