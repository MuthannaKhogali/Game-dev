using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// different states of battle
public enum BattleState { START , PLAYERTURN1, PLAYERTURN2, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    // variables for the the different types of units
    public BattleState state;
    public GameObject Player1Prefab;
    public GameObject Player2Prefab;
    public GameObject EnemyPrefab;

    public Transform player1battlestation;
    public Transform player2battlestation;
    public Transform enemybattlestation;

    Unit Player1Unit;
    Unit Player2Unit;
    Unit EnemyUnit;

    // variables for the UI
    public BattleHUD player1HUD;
    public BattleHUD player2HUD;
    public BattleHUD enemyHUD;

    public TextMeshProUGUI battleText;
    public TextMeshProUGUI skill1Text;
    public TextMeshProUGUI skill2Text;
    public TextMeshProUGUI who1;
    public TextMeshProUGUI who2;
    public GameObject skillPanel;
    public GameObject whoPanel;

    // variables for the different type of states during the game
    bool raisingAttack = false;
    bool raisingDefense = false;
    bool healing = false;
    bool attacked = false;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUp());
    }

    // this is for setting up the battle
    IEnumerator SetUp() 
    {
        // makes the players and the enemy go to where they suppose to be
        GameObject player1GO = Instantiate(Player1Prefab, player1battlestation);
        Player1Unit = player1GO.GetComponent<Unit>();


        GameObject player2GO = Instantiate(Player2Prefab, player2battlestation);
        Player2Unit = player2GO.GetComponent<Unit>();


        GameObject enemyGO = Instantiate(EnemyPrefab, enemybattlestation);
        EnemyUnit = enemyGO.GetComponent<Unit>();

        // set their HUDs
        player1HUD.setHUD(Player1Unit);
        player2HUD.setHUD(Player2Unit);
        enemyHUD.setHUD(EnemyUnit);

        // displays starting message and starts player 1s turn
        battleText.text = EnemyUnit.unitName + " looks at you menacingly.";
        yield return new WaitForSeconds(2f);
        battleText.text = Player1Unit.unitName + "'s Turn";
        state = BattleState.PLAYERTURN1;
        UpdateSkillButtons();

    }

    // updates everything for the skill buttons
    void UpdateSkillButtons() 
    {
        // changes the text for who you want to use your skill on
        who1.text = Player1Unit.unitName;
        who2.text = Player2Unit.unitName;

        // updates all skill button text depending on if a character can raise attack/sleep and heal/raise defense
        if (state == BattleState.PLAYERTURN1)
        {

            if (Player1Unit.canRaiseAttack)
            {
                skill1Text.text = "Raise Attack";
            }
            else
            {
                skill1Text.text = "Sleep";
            }

            if (Player1Unit.canHeal)
            {
                skill2Text.text = "Heal";
            }
            else
            {
                skill2Text.text = "Raise Defense";
            }
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            if (Player2Unit.canRaiseAttack)
            {
                skill1Text.text = "Raise Attack";
            }
            else
            {
                skill1Text.text = "Sleep";
            }

            if (Player2Unit.canHeal)
            {
                skill2Text.text = "Heal";
            }
            else
            {
                skill2Text.text = "Raise Defense";
            }
        }
    }

    // for when the player 1 attacks 
    IEnumerator Player1Attack() 
    {
        // sets to has attacked so player cant attack again
        attacked = true;
        // damages enemy and changes text
        bool dead = EnemyUnit.TakeDamage(Player1Unit.damage);
        enemyHUD.SetHP(EnemyUnit.currentHP);
        // updates the raise attack counter
        Player1Unit.UpdateRaiseAttack();
        battleText.text = Player1Unit.unitName + " attacks the enemy!";
        yield return new WaitForSeconds(1f);
        // if the enemy is dead then you win if not then next players turn
        if (dead)
        {
            yield return new WaitForSeconds(1f);
            state = BattleState.WON;
            EndBattle();
        }
        else 
        {
            yield return new WaitForSeconds(1f);
            state = BattleState.PLAYERTURN2;
            battleText.text = Player2Unit.unitName + "'s Turn";

        }
        // sets attacked back to false for player 2 then updates the skill buttons
        yield return new WaitForSeconds(1f);
        attacked = false;
        UpdateSkillButtons();
    }

    // for when the player 2 attacks 
    IEnumerator Player2Attack()
    {
        // sets to has attacked so player cant attack again
        attacked = true;
        // damages enemy and changes text
        bool dead = EnemyUnit.TakeDamage(Player2Unit.damage);
        enemyHUD.SetHP(EnemyUnit.currentHP);
        // updates the raise attack counter
        Player2Unit.UpdateRaiseAttack();
        battleText.text = Player2Unit.unitName + " attacks the enemy!";
        yield return new WaitForSeconds(1f);
        // if the enemy is dead then you win if not then next players turn
        if (dead)
        {
            yield return new WaitForSeconds(1f);
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
            battleText.text = EnemyUnit.unitName + " 's Turn";
            yield return new WaitForSeconds(1f);
        }
        // sets attacked back to false for player 2 then updates the skill buttons
        yield return new WaitForSeconds(1f);
        attacked = false;
        UpdateSkillButtons();

    }

    // enemy AI
    IEnumerator EnemyTurn()
    {
        // if the enemy is sleeping it will skip the enemies turn and update the text correspondingly
        if (EnemyUnit.IsSleeping())
        {
            battleText.text = EnemyUnit.unitName + " is sleeping!";
            yield return new WaitForSeconds(2f);
            EnemyUnit.PassTurn();
            state = BattleState.PLAYERTURN1;
            battleText.text = Player1Unit.unitName + "'s Turn";
            UpdateSkillButtons();
        }
        else
        {
            // he jus attacks for right now
            battleText.text = EnemyUnit.unitName + " attacks!";
            yield return new WaitForSeconds(1f);
            bool dead = Player1Unit.TakeDamage(EnemyUnit.damage);
            player1HUD.SetHP(Player1Unit.currentHP);
            yield return new WaitForSeconds(1f);
            // if he kills player 1 then you lose other wise back to player 1
            if (dead)
            {
                yield return new WaitForSeconds(1f);
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                yield return new WaitForSeconds(1f);
                state = BattleState.PLAYERTURN1;
                battleText.text = Player1Unit.unitName + "'s Turn";
            }
            UpdateSkillButtons();
        }
    }

    // Decides what happens for when you lose or win the battle
    void EndBattle() 
    {
        if (state == BattleState.WON)
        {
            battleText.text = "You won the battle";
        }
        else if (state == BattleState.LOST) 
        {
            battleText.text = "You lost the battle";

        }
    }

    // for when you press the attack button
    public void OnAttack() 
    {
        // if not anyones turn then dont let the user attack
        if(state != BattleState.PLAYERTURN1 && state != BattleState.PLAYERTURN2)
        {
            return;
        }
        // SO the user cant just spam the attack button and win
        if (attacked)
        {
            return;
        }

        // if player 1 turn then run player 1s attack and same for player 2
        if (state == BattleState.PLAYERTURN1)
        {
            StartCoroutine(Player1Attack());
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            StartCoroutine(Player2Attack());
        }
    }

    // if the user presses the defend button
    public void OnDefend()
    {
        // if not the player turn dont let them defend
        if (state != BattleState.PLAYERTURN1 && state != BattleState.PLAYERTURN2)
        {
            return;
        }

        // if player 1 turn run defend and same for player 2 then go the next persons turn
        if (state == BattleState.PLAYERTURN1)
        {
            Player1Unit.Defend();
            state = BattleState.PLAYERTURN2;
            battleText.text = Player2Unit.unitName + "'s Turn";
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            Player2Unit.Defend();
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
            battleText.text = EnemyUnit.unitName + "'s Turn";
        }
        UpdateSkillButtons();
    }

    // for the players first skill
    public void OnSkillOne()
    {
        // if its player 1s turn take it to his skill set and same for player 2
        if (state == BattleState.PLAYERTURN1)
        {
            StartCoroutine(ActivateSkillOnePlayer1());
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            StartCoroutine(ActivateSkillOnePlayer2());
        }
    }

    // player 1s first skill
    IEnumerator ActivateSkillOnePlayer1()
    {
        // checks if the player can raise attack 
        if (Player1Unit.canRaiseAttack)
        {
            // sets raising attack skill to true and opens the who you want to buff panel 
            raisingAttack = true;
            skillPanel.SetActive(false);
            whoPanel.SetActive(true);
        }
        // checks if the player can sleep
        else if (Player1Unit.canSleep)
        {
            // makes sure not to open the who panel cause they are sleeping the enemy
            skillPanel.SetActive(false);
            whoPanel.SetActive(false);
            // sleeps enemy for 1 turn
            EnemyUnit.Sleep();

            yield return new WaitForSeconds(1f);
            // changes turn
            if (state == BattleState.PLAYERTURN1)
            {
                state = BattleState.PLAYERTURN2;
                battleText.text = Player2Unit.unitName + "'s Turn";
            }
            else if (state == BattleState.PLAYERTURN2)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }

            UpdateSkillButtons();
        }
    }

    // player 2s first skill
    IEnumerator ActivateSkillOnePlayer2() 
    {
        // checks if the player can raise attack 
        if (Player2Unit.canRaiseAttack)
        {
            // sets raising attack skill to true and opens the who you want to buff panel
            raisingAttack = true;
            skillPanel.SetActive(false);
            whoPanel.SetActive(true);
        }
        // checks if the player can sleep
        else if (Player2Unit.canSleep)
        {
            // makes sure not to open the who panel cause they are sleeping the enemy
            skillPanel.SetActive(false);
            whoPanel.SetActive(false);
            // sleeps enemy for 1 turn
            EnemyUnit.Sleep();

            yield return new WaitForSeconds(1f);
            // changes the turn
            if (state == BattleState.PLAYERTURN1)
            {
                state = BattleState.PLAYERTURN2;
                battleText.text = Player2Unit.unitName + "'s Turn";
            }
            else if (state == BattleState.PLAYERTURN2)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }

            UpdateSkillButtons();
        }
    }

    // for the second skill
    public void OnSkillTwo() 
    {
        // if player 1 turn go to player 1 skill and same for player 2
        if (state == BattleState.PLAYERTURN1)
        {
            StartCoroutine(ActivateSkillTwoPlayer1());
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            StartCoroutine(ActivateSkillTwoPlayer2());
        }
    }

    // second skill for player 1
    IEnumerator ActivateSkillTwoPlayer1() 
    {
        // if they can raise defense
        if (Player1Unit.canRaiseDefense)
        {
            // set to true and open the who panel
            raisingDefense = true;
            skillPanel.SetActive(false);
            whoPanel.SetActive(true);
        }
        // if they can heal
        else if (Player1Unit.canHeal)
        {
            // set to true and open the who panel
            healing = true;
            skillPanel.SetActive(false);
            whoPanel.SetActive(true);
        }
        yield return null;
    }

    // for player 2 second skill
    IEnumerator ActivateSkillTwoPlayer2()
    {
        // if they can raise defense
        if (Player2Unit.canRaiseDefense)
        {
            // set to true and open the who panel
            raisingDefense = true;
            skillPanel.SetActive(false);
            whoPanel.SetActive(true);
        }
        // if they can heal
        else if (Player2Unit.canHeal) 
        {
            // set to true and open the who panel
            healing = true;
            skillPanel.SetActive(false);
            whoPanel.SetActive(true);
        }
        yield return null;
    }

    // if you select target one aka if you select player 1
    public void OnSelectTargetOne() 
    {
        // raises the players attack and goes to next turn
        if (raisingAttack)
        {
            Player1Unit.RaiseAttack();
            raisingAttack = false;
            whoPanel.SetActive(false);

            if (state == BattleState.PLAYERTURN1)
            {
                state = BattleState.PLAYERTURN2;
                battleText.text = Player2Unit.unitName + "'s Turn";
            }
            else if (state == BattleState.PLAYERTURN2)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
                battleText.text = EnemyUnit.unitName + "'s Turn";
            }
            UpdateSkillButtons();
        }

        // raises the players defense and goes to next turn
        if (raisingDefense)
        {
            Player1Unit.RaiseDefense();
            raisingDefense = false;
            whoPanel.SetActive(false);

            if (state == BattleState.PLAYERTURN1)
            {
                state = BattleState.PLAYERTURN2;
                battleText.text = Player2Unit.unitName + "'s Turn";
            }
            else if (state == BattleState.PLAYERTURN2)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
                battleText.text = EnemyUnit.unitName + "'s Turn";
            }
            UpdateSkillButtons();
        }

        // heals player and goes to next turn
        if (healing) 
        {
            Player1Unit.Heal();
            player1HUD.SetHP(Player1Unit.currentHP);
            healing = false;
            whoPanel.SetActive(false);

            if (state == BattleState.PLAYERTURN1)
            {
                state = BattleState.PLAYERTURN2;
                battleText.text = Player2Unit.unitName + "'s Turn";
            }
            else if (state == BattleState.PLAYERTURN2)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
                battleText.text = EnemyUnit.unitName + "'s Turn";
            }
            UpdateSkillButtons();
        }
    }

    // if you select target 2 aka player 2
    public void OnSelectTargetTwo()
    {
        // raises attack and goes to next turn
        if (raisingAttack)
        {
            Player2Unit.RaiseAttack();
            raisingAttack = false;
            whoPanel.SetActive(false);

            if (state == BattleState.PLAYERTURN1)
            {
                state = BattleState.PLAYERTURN2;
                battleText.text = Player2Unit.unitName + "'s Turn";
            }
            else if (state == BattleState.PLAYERTURN2)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
                battleText.text = EnemyUnit.unitName + "'s Turn";
            }
            UpdateSkillButtons();
        }

        // raises defense and goes to next turn
        if (raisingDefense)
        {
            Player2Unit.RaiseDefense();
            raisingDefense = false;
            whoPanel.SetActive(false);

            if (state == BattleState.PLAYERTURN1)
            {
                state = BattleState.PLAYERTURN2;
                battleText.text = Player2Unit.unitName + "'s Turn";
            }
            else if (state == BattleState.PLAYERTURN2)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
                battleText.text = EnemyUnit.unitName + "'s Turn";
            }
            UpdateSkillButtons();
        }

        // heals and goes to next turn
        if (healing)
        {
            Player2Unit.Heal();
            player2HUD.SetHP(Player2Unit.currentHP);
            healing = false;
            whoPanel.SetActive(false);

            if (state == BattleState.PLAYERTURN1)
            {
                state = BattleState.PLAYERTURN2;
                battleText.text = Player2Unit.unitName + "'s Turn";
            }
            else if (state == BattleState.PLAYERTURN2)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
                battleText.text = EnemyUnit.unitName + "'s Turn";
            }
            UpdateSkillButtons();
        }
    }

    // if you press the skill button
    public void OnSkill() 
    {
        // checks if its who turn it is and if they can open the skill panel
        if (state == BattleState.PLAYERTURN1 && !attacked)
        {
            skillPanel.SetActive(true);
        }
        else if (state == BattleState.PLAYERTURN2 && !attacked)
        {
            skillPanel.SetActive(true);
        }
    }

    // if the user wants to go back to the main menu panel
    public void onBack() 
    {
        // sets everything back to false
        raisingAttack = false;
        raisingDefense = false;
        healing = false;
        whoPanel.SetActive(false);
        skillPanel.SetActive(false);
    }
}
