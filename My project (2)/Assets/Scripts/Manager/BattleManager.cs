using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    public Button defendButton;
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
    public Sprite octavioSprite;
    private bool playerTurn = true;

    [Header("Recompensa")]
    public Inventario inventario;
    public Sprite rewardIcon;
    public string rewardItemType;

    [Header("Glitches")]
    public VideoPlayer glitchVideo;
    public RawImage glitchImage;

    [Header("Final si Octavio derrota al jugador")]
    public Image deathBlackFade;
    public Image deathNewspaper;
    [Min(0f)] public float deathBlackFadeDuration = 1.5f;
    [Min(0f)] public float deathNewspaperFadeDuration = 1.5f;
    [Min(0f)] public float deathNewspaperDisplayDuration = 4f;

    [Header("Tutorial")]
    public bool showTutorial = true;
    private int tutorialStep = 0;
    private bool tutorialActive = false;
    private bool octavioDeathSequenceStarted = false;

    [Header("Ayuda de Tomas contra Octavio")]
    [TextArea(2, 5)]
    public string tomasOpeningText =
        "Ves al niño del cuarto aparecer detras tuyo y abalanzarse sobre su padre.";
    [Min(0)] public int tomasOpeningDamage = 50;
    [Min(1)] public int tomasStunTurns = 3;
    [Min(0f)] public float tomasOpeningMessageDelay = 3f;

    private bool tomasOpeningPlayed = false;

    Queue<string> battleMessages =
    new Queue<string>();

    public int maxMessages = 3;
    private bool useOrangeLine = false;
    bool cameFromTutorial = GameManager.Instance.previousScene == "Tutorial";

    void Start()
    {
        if (deathBlackFade != null)
        {
            Color blackFadeColor = deathBlackFade.color;
            blackFadeColor.a = 0f;
            deathBlackFade.color = blackFadeColor;
            deathBlackFade.raycastTarget = false;
        }

        if (deathNewspaper != null)
        {
            Color newspaperColor = deathNewspaper.color;
            newspaperColor.a = 0f;
            deathNewspaper.color = newspaperColor;
            deathNewspaper.raycastTarget = false;
        }

        bool shouldShowTutorial = showTutorial && !GameManager.Instance.battleTutorialCompleted && GameManager.Instance.previousScene == "Tutorial";
        LoadEnemyData();

        if (ShouldPlayTomasOpening())
        {
            StartCoroutine(PlayTomasOpening());
            return;
        }

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
    IEnumerator GlitchScreen(
    float duration = 0.2f,
    float alpha = 0.5f)
    {
        if (glitchVideo == null)
            yield break;

        Color c = glitchImage.color;

        c.a = alpha;
        glitchImage.color = c;

        glitchVideo.Stop();
        glitchVideo.Play();

        yield return new WaitForSeconds(duration);

        c.a = 0f;
        glitchImage.color = c;

        glitchVideo.Stop();
    }
    IEnumerator FlashEnemy(
    Color flashColor,
    int flashes = 1,
    float flashDuration = 0.08f)
    {
        if (enemyPortrait == null)
            yield break;

        Color originalColor = enemyPortrait.color;

        for (int i = 0; i < flashes; i++)
        {
            enemyPortrait.color = flashColor;

            yield return new WaitForSeconds(flashDuration);

            enemyPortrait.color = originalColor;

            yield return new WaitForSeconds(flashDuration);
        }
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
            case "octavio":

                enemy.enemyID = "octavio";
                enemy.enemyName = "Octavio";

                enemy.maxHP = 200;
                enemy.attackDamage = 35;
                enemy.currentHP = enemy.maxHP;

                enemyAI.dodgeChance = 20;
                enemyPortrait.sprite = octavioSprite;
                enemyPortrait.rectTransform.localScale =
                new Vector3(2f, 2f, 1f);
                enemyPortrait.rectTransform.anchoredPosition =
                new Vector2(0f, -50f);

                break;

            case "rat_01":

                enemy.enemyID = "rat_01";
                enemy.enemyName = "Rata mutante";

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
                enemy.attackDamage = 16;

                enemy.currentHP =
                    enemy.maxHP;

                enemyAI.dodgeChance = 20;
                enemyPortrait.sprite = subjectSprite;

                break;

            case "rat_pack":

                enemy.enemyID = "rat_pack";
                enemy.enemyName = "Jauría de Ratas";

                enemy.maxHP = 80;
                enemy.attackDamage = 50;

                enemy.currentHP = enemy.maxHP;

                enemyAI.dodgeChance = 40;

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
        if (enemyAI.TryDodge("Puño"))
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

        damage = Mathf.RoundToInt(
            damage *
            enemy.fistMultiplier);
        enemy.TakeDamage(damage);
        StartCoroutine(FlashEnemy(Color.red));
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

        string equipped = PlayerStats.Instance.equippedWeapon;

        if (equipped == "Sedante")
        {
            StartCoroutine(PlayerSedativeTurn());

            return;
        }

        StartCoroutine(PlayerObjectTurn());
    }
    IEnumerator PlayerSedativeTurn()
    {
        playerTurn = false;

        ToggleButtons(false);

        PlayerStats.Instance.ActivateSedative();

        // Consumir el objeto
        GameManager.Instance.collectedItems.Remove("Sedante");

        // Desequiparlo
        PlayerStats.Instance.equippedWeapon = "";

        // Actualizar la UI
        battleInventory.RefreshInventory();

        ShowBattleLog(
            "Inyectás el sedante.\nLa criatura parece debilitarse."
        );

        yield return new WaitForSeconds(2f);
        Inventario inventario =
    FindFirstObjectByType<Inventario>();

        if (inventario != null)
        {
            inventario.RemoveItem("Sedante");
        }
        StartCoroutine(EnemyTurn());
    }
    public void Defend()
    {
        if (!playerTurn)
            return;

        StartCoroutine(PlayerDefendTurn());
    }
    IEnumerator PlayerDefendTurn()
    {
        playerTurn = false;

        ToggleButtons(false);

        PlayerStats.Instance.ActivateDefense();

        ShowBattleLog(
            "Adoptás una postura defensiva."
        );

        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemyTurn());
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
        if (enemyAI.TryDodge(PlayerStats.Instance.equippedWeapon))
        {
            ShowBattleLog(
                GetRandomText(dodgeTexts)
            );

            yield return new WaitForSeconds(2f);

            StartCoroutine(EnemyTurn());

            yield break;
        }
        switch (PlayerStats.Instance.equippedWeapon)
        {
            case "Pala":
                damage = Mathf.RoundToInt(
    damage * enemy.shovelMultiplier
);
                if (Random.Range(0, 100) < 15)
                {
                    enemy.stunned = true;
                    enemy.stunnedTurns = Mathf.Max(
                        enemy.stunnedTurns,
                        1
                    );

                    ShowBattleLog(
                        "El enemigo queda aturdido, es tu turno");
                }

                break;

            case "Cuchillo":
                damage = Mathf.RoundToInt(
    damage * enemy.knifeMultiplier
);
                if (Random.Range(0, 100) < 30)
                {
                    enemy.bleeding = true;

                    enemy.bleedingTurns = 3;

                    ShowBattleLog(
                        "La herida comienza a sangrar");
                }

                break;
        }
        enemy.TakeDamage(damage);
        StartCoroutine(FlashEnemy(Color.red));
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

        if (
            enemy.enemyID == "rat_01" ||
            enemy.enemyID == "octavio"
        )
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
        if (enemy.stunned)
        {
            enemy.stunnedTurns = Mathf.Max(
                enemy.stunnedTurns,
                1
            );

            enemy.stunnedTurns--;

            if (enemy.stunnedTurns <= 0)
            {
                enemy.stunned = false;
            }

            ShowBattleLog(
                enemy.enemyName +
                " está inmovilizado, es tu turno");
            StartCoroutine(FlashEnemy(Color.yellow, 4));
            yield return new WaitForSeconds(2);

            playerTurn = true;

            ToggleButtons(true);

            yield break;
        }
        if (enemy.bleeding)
        {
            StartCoroutine(FlashEnemy(Color.red, 4));
            enemy.TakeDamage(4);

            enemy.bleedingTurns--;

            ShowBattleLog(
                enemy.enemyName +
                " pierde sangre.");

            if (enemy.bleedingTurns <= 0)
                enemy.bleeding = false;
        }
        ShowBattleLog(enemy.enemyName + " observa tus movimientos...");

        yield return new WaitForSeconds(1.5f);

        EnemyAttack();

        if (octavioDeathSequenceStarted)
            yield break;

        if (enemy.currentHP <= 0)
        {
            StartCoroutine(EnemyDefeated());

            yield break;
        }
        yield return new WaitForSeconds(2f);

        playerTurn = true;

        ToggleButtons(true);

        ShowBattleLog(
            "Es tu turno"
        );
    }

    bool ShouldPlayTomasOpening()
    {
        return
            !tomasOpeningPlayed &&
            enemy != null &&
            enemy.enemyID == "octavio" &&
            GameManager.Instance != null &&
            GameManager.Instance.helpedTomasEscape;
    }

    IEnumerator PlayTomasOpening()
    {
        tomasOpeningPlayed = true;
        playerTurn = false;
        ToggleButtons(false);

        int maximumOpeningDamage = Mathf.Max(
            0,
            enemy.currentHP - 1
        );

        int appliedDamage = Mathf.Clamp(
            tomasOpeningDamage,
            0,
            maximumOpeningDamage
        );

        ShowBattleLog(tomasOpeningText);

        yield return new WaitForSeconds(
            tomasOpeningMessageDelay
        );

        enemy.TakeDamage(appliedDamage);
        StartCoroutine(FlashEnemy(Color.yellow, 4));
        StartCoroutine(ShakeEnemy());

        ShowBattleLog(
            "Tom\u00E1s golpea a Octavio.\n-" +
            appliedDamage +
            " HP"
        );

        yield return new WaitForSeconds(
            tomasOpeningMessageDelay
        );

        enemy.stunned = true;
        enemy.stunnedTurns = Mathf.Max(
            1,
            tomasStunTurns
        );

        ShowBattleLog(
            "Octavio queda inmovilizado durante " +
            enemy.stunnedTurns +
            " turnos."
        );

        yield return new WaitForSeconds(
            tomasOpeningMessageDelay
        );

        playerTurn = true;
        ToggleButtons(true);
        ShowBattleLog("Es tu turno");
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
        bool defended = PlayerStats.Instance.defending;
        int displayedDamage =
            PlayerStats.Instance.CalculateDamageTaken(damage);

        PlayerStats.Instance.TakeDamage(damage);

        ShowBattleLog(
            GetRandomText(enemyAttackTexts)
            + "\n-"
            + displayedDamage
            + " HP"
        );

        if (
            PlayerStats.Instance.currentHP <= 0 &&
            enemy.enemyID == "octavio" &&
            deathNewspaper != null
        )
        {
            octavioDeathSequenceStarted = true;
            PlayerStats.Instance.StopAllCoroutines();
            StartCoroutine(ShowOctavioDeathNewspaper());
            return;
        }

        StartCoroutine(GlitchScreen());

        if (defended)
        {
            StartCoroutine(CounterAttackRoutine());
        }
        Debug.Log("HP luego del ataque: " + PlayerStats.Instance.currentHP);
    }

    IEnumerator ShowOctavioDeathNewspaper()
    {
        playerTurn = false;
        ToggleButtons(false);

        if (deathBlackFade != null)
        {
            deathBlackFade.gameObject.SetActive(true);
            deathBlackFade.transform.SetAsLastSibling();
        }

        deathNewspaper.gameObject.SetActive(true);
        deathNewspaper.transform.SetAsLastSibling();

        if (deathBlackFade != null)
        {
            yield return StartCoroutine(
                FadeImage(
                    deathBlackFade,
                    0f,
                    1f,
                    deathBlackFadeDuration
                )
            );
        }

        yield return StartCoroutine(
            FadeImage(
                deathNewspaper,
                0f,
                1f,
                deathNewspaperFadeDuration
            )
        );

        yield return new WaitForSeconds(
            deathNewspaperDisplayDuration
        );
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerDied = true;
        }
        SceneManager.LoadScene("Ending");
    }

    IEnumerator FadeImage(
        Image image,
        float startAlpha,
        float endAlpha,
        float duration
    )
    {
        Color imageColor = image.color;
        imageColor.a = startAlpha;
        image.color = imageColor;

        if (duration <= 0f)
        {
            imageColor.a = endAlpha;
            image.color = imageColor;
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            imageColor.a = Mathf.Lerp(
                startAlpha,
                endAlpha,
                elapsedTime / duration
            );
            image.color = imageColor;
            yield return null;
        }

        imageColor.a = endAlpha;
        image.color = imageColor;
    }
    IEnumerator CounterAttackRoutine()
    {
        yield return new WaitForSeconds(1f);
        CounterAttack();
        yield return new WaitForSeconds(1f);
    }
    void CounterAttack()
    {
        int damage;

        switch (PlayerStats.Instance.equippedWeapon)
        {
            case "Pala":

                damage =
                    PlayerStats.Instance.shovelDamage / 2;

                damage = Mathf.RoundToInt(
                    damage *
                    enemy.shovelMultiplier);

                break;

            case "Cuchillo":

                damage =
                    PlayerStats.Instance.knifeDamage / 2;

                damage = Mathf.RoundToInt(
                    damage *
                    enemy.knifeMultiplier);

                break;

            default:

                damage =
                    PlayerStats.Instance.fistDamage / 2;

                damage = Mathf.RoundToInt(
                    damage *
                    enemy.fistMultiplier);

                break;
        }

        enemy.TakeDamage(damage);

        ShowBattleLog(
            "Bloqueás el golpe y contraatacás.\n-"
            + damage
            + " HP"
        );
    }
    // =========================
    // ACTIVAR/DESACTIVAR BOTONES
    // =========================
    void ToggleButtons(bool enabledState)
    {
        defendButton.interactable = enabledState;
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
        Debug.Log("========== BATTLE LOG ==========");
        Debug.Log("Max Messages: " + maxMessages);
        Debug.Log("Antes: " + battleMessages.Count);

        string color =
            useOrangeLine
            ? "#E6B56A"
            : "#FFFFFF";

        useOrangeLine = !useOrangeLine;

        string formattedMessage =
            "<color=" + color + ">- "
            + message
            + "</color>";

        battleMessages.Enqueue(formattedMessage);

        Debug.Log("Después de agregar: " + battleMessages.Count);

        while (battleMessages.Count > maxMessages)
        {
            Debug.Log(
                "Eliminando: "
                + battleMessages.Peek()
            );

            battleMessages.Dequeue();
        }

        Debug.Log("Final: " + battleMessages.Count);

        battleLogText.text =
            string.Join(
                "\n",
                battleMessages
            );

        Debug.Log("===============================");
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