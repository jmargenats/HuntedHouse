using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Vida")]
    public int maxHP = 100;
    public int currentHP = 100;

    [Header("Daño")]
    public int fistDamage = 5;

    public int shovelDamage = 20;
    public int knifeDamage = 12;

    [Header("Arma equipada")]
    public string equippedWeapon;

    private void Awake()
    {
        Instance = this;
    }

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

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log("Player recibió daño");
    }
}