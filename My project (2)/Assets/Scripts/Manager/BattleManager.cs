using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("Referencias")]
    public EnemyStats enemy;
    public EnemyAI enemyAI;

    [Header("Botones")]
    public Button attackButton;
    public Button attackObjectButton;

    [Header("Texto de combate")]
    public TMP_Text battleLogText;

    [Header("Narrativa combate")]
    public string[] fistAttackTexts;
    public string[] shovelAttackTexts;
    public string[] knifeAttackTexts;
    public string[] dodgeTexts;
    public string[] enemyAttackTexts;

    private bool playerTurn = true;

    // =========================
    // ATAQUE PUÑO
    // =========================
    public void AttackFist()
    {
        if (!playerTurn)
            return;

        StartCoroutine(PlayerFistTurn());
    }

    IEnumerator PlayerFistTurn()
    {
        playerTurn = false;

        ToggleButtons(false);

        yield return new WaitForSeconds(0.3f);

        // Esquive enemigo
        if (enemyAI.TryDodge())
        {
            ShowBattleLog(
                GetRandomText(dodgeTexts)
            );

            yield return new WaitForSeconds(2f);

            StartCoroutine(EnemyTurn());

            yield break;
        }

        int damage =
            PlayerStats.Instance.fistDamage;

        enemy.TakeDamage(damage);

        ShowBattleLog(
            GetRandomText(fistAttackTexts)
            + "\n-"
            + damage
            + " HP"
        );

        yield return new WaitForSeconds(2f);

        EndPlayerTurn();
    }

    // =========================
    // ATAQUE OBJETO
    // =========================
    public void AttackWithObject()
    {
        if (!playerTurn)
            return;

        StartCoroutine(PlayerObjectTurn());
    }

    IEnumerator PlayerObjectTurn()
    {
        int damage =
            PlayerStats.Instance.GetWeaponDamage();

        if (damage <= 0)
        {
            ShowBattleLog(
                "No tenés un objeto equipado"
            );

            yield break;
        }

        playerTurn = false;

        ToggleButtons(false);

        yield return new WaitForSeconds(0.3f);

        // Esquive enemigo
        if (enemyAI.TryDodge())
        {
            ShowBattleLog(
                GetRandomText(dodgeTexts)
            );

            yield return new WaitForSeconds(2f);

            StartCoroutine(EnemyTurn());

            yield break;
        }

        enemy.TakeDamage(damage);

        string equippedWeapon =
            PlayerStats.Instance.equippedWeapon;

        string attackText = "";

        switch (equippedWeapon)
        {
            case "Pala":

                attackText =
                    GetRandomText(shovelAttackTexts);

                break;

            case "Cuchillo":

                attackText =
                    GetRandomText(knifeAttackTexts);

                break;

            default:

                attackText =
                    "Atacás con el objeto";

                break;
        }

        ShowBattleLog(
            attackText
            + "\n-"
            + damage
            + " HP"
        );

        yield return new WaitForSeconds(2f);

        EndPlayerTurn();
    }

    // =========================
    // FIN TURNO PLAYER
    // =========================
    void EndPlayerTurn()
    {
        if (enemy.currentHP <= 0)
        {
            StartCoroutine(EnemyDefeated());

            return;
        }

        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyDefeated()
    {
        ToggleButtons(false);

        ShowBattleLog(
            "La rata cae al suelo sin moverse"
        );

        yield return new WaitForSeconds(2.5f);

        GameManager.Instance.returningFromBattle = true;

        GameManager.Instance.ratDefeated = true;

        FindObjectOfType<SceneFader>()
            .FadeAndLoadScene(
                GameManager.Instance.previousScene
            );
    }

    // =========================
    // TURNO ENEMIGO
    // =========================
    IEnumerator EnemyTurn()
    {
        ShowBattleLog(
            "La rata observa tus movimientos..."
        );

        yield return new WaitForSeconds(1.5f);

        EnemyAttack();

        yield return new WaitForSeconds(2f);

        playerTurn = true;

        ToggleButtons(true);

        ShowBattleLog(
            "Es tu turno"
        );
    }

    // =========================
    // ATAQUE ENEMIGO
    // =========================
    void EnemyAttack()
    {
        int damage =
            enemyAI.ChooseDamage(
                enemy.biteDamage,
                enemy.currentHP
            );

        PlayerStats.Instance.TakeDamage(damage);

        ShowBattleLog(
            GetRandomText(enemyAttackTexts)
            + "\n-"
            + damage
            + " HP"
        );
    }

    // =========================
    // ACTIVAR/DESACTIVAR BOTONES
    // =========================
    void ToggleButtons(bool enabledState)
    {
        attackButton.interactable =
            enabledState;

        attackObjectButton.interactable =
            enabledState;
    }

    // =========================
    // TEXTO RANDOM
    // =========================
    string GetRandomText(string[] texts)
    {
        int randomIndex =
            Random.Range(0, texts.Length);

        return texts[randomIndex];
    }

    // =========================
    // MOSTRAR TEXTO
    // =========================
    void ShowBattleLog(string message)
    {
        Debug.Log(message);

        battleLogText.text = message;
    }
}