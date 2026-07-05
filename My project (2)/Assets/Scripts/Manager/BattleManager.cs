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
    public Button fleeButton;
    public Button healButton;
    public BattleInventoryUI battleInventory;

    [Header("Texto de combate")]
    public TMP_Text battleLogText;

    [Header("Narrativa combate")]
    public string[] fistAttackTexts;
    public string[] shovelAttackTexts;
    public string[] knifeAttackTexts;
    public string[] dodgeTexts;
    public string[] enemyAttackTexts;

    [Header("Imagenes de combate")]
    public Image enemyPortrait;
    public Sprite ratSprite;
    public Sprite subjectSprite;
    public Sprite nurseSprite;
    public Sprite childSprite;
    public Sprite ratPackSprite;
    private bool playerTurn = true;

    [Header("Recompensa")]
    public Inventario inventario;
    public Sprite rewardIcon;
    public string rewardItemType;

    [Header("Tutorial")]
    public bool showTutorial = true;
    private int tutorialStep = 0;
    private bool tutorialActive = false;
    bool cameFromTutorial = GameManager.Instance.previousScene == "Tutorial";

    void Start()
    {

        bool shouldShowTutorial = showTutorial &&  !GameManager.Instance.battleTutorialCompleted && GameManager.Instance.previousScene == "Tutorial";
        LoadEnemyData();

        if (shouldShowTutorial)
        {
            StartTutorial();
        }
        else
        {
            ShowBattleLog("Es tu turno");
            ToggleButtons(true);
        }
    }

    void Update()
    {
        if (!tutorialActive) return;

        if (tutorialStep == 0 &&
            PlayerStats.Instance.equippedWeapon == "Pala")
        {
            tutorialStep = 1;

            ShowBattleLog(
                "Bien. Ahora usá Atacar con objeto para golpear con la pala."
            );

            attackObjectButton.interactable = true;
        }
    }
    IEnumerator FlashEnemy()
    {
        if (enemyPortrait == null)
            yield break;

        enemyPortrait.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        enemyPortrait.color = Color.white;
    }
    IEnumerator ShakeEnemy()
    {
        Vector3 originalPos =
            enemyPortrait.rectTransform.localPosition;

        for (int i = 0; i < 6; i++)
        {
            enemyPortrait.rectTransform.localPosition =
                originalPos +
                (Vector3)Random.insideUnitCircle * 8f;

            yield return new WaitForSeconds(
                0.02f
            );
        }

        enemyPortrait.rectTransform.localPosition =
            originalPos;
    }
    // =========================
    // ATAQUE PUÑO
    // =========================
    public void AttackFist()
    {
        if (!playerTurn)
            return;
        if (tutorialActive && tutorialStep != 2)
            return;

        if (tutorialActive && tutorialStep == 2)
        {
            tutorialStep = 3;

            ShowBattleLog(
                "Atacar con puño hace menos daño, pero no necesita objeto equipado. También podés huir si necesitás salir del combate."
            );

            attackButton.interactable = false;
            attackObjectButton.interactable = false;
            fleeButton.interactable = true;

            

            return;
        }

        StartCoroutine(PlayerFistTurn());
    }
    void LoadEnemyData()
    {
        string enemyID =
            GameManager.Instance.currentEnemyID;
        enemyPortrait.preserveAspect = true;
        switch (enemyID)
        {
            case "rat_01":

                enemy.enemyID = "rat_01";
                enemy.enemyName = "Rata Mutada";

                enemy.maxHP = 30;
                enemy.attackDamage = 5;

                enemy.currentHP =
                    enemy.maxHP;

                enemyAI.dodgeChance = 40;
                enemyPortrait.sprite = ratSprite;

                break;

            case "subject_01":

                enemy.enemyID = "subject_01";
                enemy.enemyName =
                    "Sujeto Experimental 01";

                enemy.maxHP = 100;
                enemy.attackDamage = 12;

                enemy.currentHP =
                    enemy.maxHP;

                enemyAI.dodgeChance = 50;
                enemyPortrait.sprite = subjectSprite;

                break;

            case "rat_pack":

                enemy.enemyID = "rat_pack";
                enemy.enemyName = "Jauría de Ratas";

                enemy.maxHP = 80;
                enemy.attackDamage = 10;

                enemy.currentHP = enemy.maxHP;

                enemyAI.dodgeChance = 25;

                enemyPortrait.sprite = ratPackSprite;

                break;

            default:

                enemy.enemyID = "unknown";
                enemy.enemyName = "Enemigo";

                enemy.maxHP = 30;
                enemy.attackDamage = 5;

                enemy.currentHP =
                    enemy.maxHP;

                enemyAI.dodgeChance = 20;

                break;
        }

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

        int damage = PlayerStats.Instance.GetFistDamage();

        enemy.TakeDamage(damage);

        PlayerStats.Instance.RegisterFistHit();
        StartCoroutine(FlashEnemy());
        StartCoroutine(ShakeEnemy());
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

        if (tutorialActive && tutorialStep != 1)
            return;

        if (tutorialActive && tutorialStep == 1)
        {
            tutorialStep = 2;

            ShowBattleLog(
                "Buen golpe. También podés atacar con los puños usando el botón Atacar."
            );

            attackObjectButton.interactable = false;
            attackButton.interactable = true;

            return;
        }

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
        StartCoroutine(FlashEnemy());
        StartCoroutine(ShakeEnemy());
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
    void UpdateHealButton()
    {
        if (
            healButton == null ||
            battleInventory == null
        )
            return;

        string item =
            battleInventory.GetSelectedItem();

        healButton.interactable =
            playerTurn &&
            (
                item == "Pastillas"
                ||
                item == "Botiquin"
            );
    }
    public void Heal()
    {
        Debug.Log("Botón curar apretado");
        if (!playerTurn)
            return;

        StartCoroutine(
            PlayerHealTurn()
        );
    }
    IEnumerator PlayerHealTurn()
    {
        Debug.Log("Entré a PlayerHealTurn");
        string selectedItem =
    battleInventory.GetSelectedItem();

        int healAmount = 0;

        switch (selectedItem)
        {
            case "Pastillas":

                healAmount = 20;

                break;

            case "Botiquin":

                healAmount = 40;

                break;

            default:

                ShowBattleLog(
                    "No tenés un objeto curativo seleccionado."
                );

                yield break;
        }

        playerTurn = false;

        ToggleButtons(false);

        PlayerStats.Instance.Heal(
            healAmount
        );

        GameManager.Instance
    .collectedItems
    .Remove(selectedItem);

        ShowBattleLog(
            "Recuperás "
            + healAmount
            + " HP."
        );
        battleInventory.RefreshInventory();
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
            enemy.enemyName +
            " cae al suelo sin moverse."
        );

        Debug.Log(
            "Enemy ID al morir: "
            + enemy.enemyID
        );

        GameManager.Instance.DefeatEnemy(
            enemy.enemyID
        );

        Debug.Log(
            "Lista derrotados: "
            + string.Join(
                ",",
                GameManager.Instance.defeatedEnemies
            )
        );

        GameManager.Instance.DefeatEnemy(enemy.enemyID);

        if (enemy.enemyID == "rat_01")
        {
            GameManager.Instance.AddItem("Llave");
        }

        yield return new WaitForSeconds(2.5f);

        GameManager.Instance.returningFromBattle = true;

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
        ShowBattleLog(enemy.enemyName + " observa tus movimientos...");

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
                enemy.attackDamage,
                enemy.currentHP
            );

        PlayerStats.Instance.TakeDamage(damage);

        ShowBattleLog(
            GetRandomText(enemyAttackTexts)
            + "\n-"
            + damage
            + " HP"
        );
        Debug.Log(
    "HP luego del ataque: " +
    PlayerStats.Instance.currentHP
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

        fleeButton.interactable = 
            enabledState;

        if (healButton != null)
        {
            healButton.interactable =
                enabledState;
        }
    }




    // =========================
    // BOTON HUIR
    // =========================
    public void OnFleeButtonPressed()
    {
        if (tutorialActive && tutorialStep == -1)
        {
            RunAway();
            return;
        }

        if (tutorialActive && tutorialStep == 3)
        {
            tutorialActive = false;
            tutorialStep = 4;

            GameManager.Instance.battleTutorialCompleted = true;

            ToggleButtons(true);

            StartCoroutine(FinishTutorialAfterDelay());

            return;
        }

        RunAway();
    }
    void RunAway()
    {
        GameManager.Instance.returningFromBattle = true;
        GameManager.Instance.ratDefeated = false;

        FindObjectOfType<SceneFader>()
            .FadeAndLoadScene(
                GameManager.Instance.previousScene
            );
    }

    // =========================
    // TEXTO RANDOM
    // =========================
    string GetRandomText(string[] texts)
    {
        if (texts == null || texts.Length == 0)
            return "";

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

    // =========================
    // TUTORIAL
    // =========================

    void StartTutorial()
    {
        tutorialActive = true;
        tutorialStep = 0;
        playerTurn = true;

        bool hasShovel =
            GameManager.Instance.HasItem("Pala");

        if (!hasShovel)
        {
            ShowBattleLog(
                "Todavía no tenés un arma, busca una antes de enfrentarte a esta criatura.\n Usá Huir para regresar."
            );

            attackButton.interactable = false;
            attackObjectButton.interactable = false;
            fleeButton.interactable = true;

            tutorialStep = -1;

            return;
        }

        attackButton.interactable = false;
        attackObjectButton.interactable = false;
        fleeButton.interactable = false;

        ShowBattleLog(
            "Tutorial: seleccioná la pala desde la sección Objetos para equiparla."
        );
    }

    IEnumerator FinishTutorialAfterDelay()
    {
        ShowBattleLog(
            "Perfecto. Ya conocés las acciones básicas del combate."
        );

        yield return new WaitForSeconds(2f);

        ShowBattleLog("Es tu turno.");
    }
}