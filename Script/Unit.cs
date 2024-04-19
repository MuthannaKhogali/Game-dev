using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // variables for the unit
    public string unitName;
    public int damage;
    public int maxHP;
    public int currentHP;

    public bool canRaiseAttack = false;
    public bool canSleep = false;
    public bool canHeal = false;
    public bool canRaiseDefense = false;

    // variables for the game mechanics
    int raiseAttackTurns = 0;
    int raiseDefenseTurns = 0;
    bool sleeping = false;
    int sleepTurnsRemaining = 0;
    bool defending;


    public bool TakeDamage(int dmg) 
    {
        // if raise defense is active still the divide damage by 2 and take one turn off the counter
        if (raiseDefenseTurns > 0)
        {
            dmg /= 2;
            raiseDefenseTurns--;
        }

        // if a player is defending divide the damage by 3
        if (defending)
        {
            dmg /= 3;
            defending = false;
        }
        currentHP -= dmg;

        // checks if dead or not
        if (currentHP <= 0)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    // sets defending to true
    public void Defend()
    {
        defending = true;
    }

    // sets defending to false
    public void StopDefend()
    {
        defending = false;
    }

    // raises attack for 3 turns
    public void RaiseAttack()
    {
        damage += 20;
        raiseAttackTurns = 3;
    }

    // decreases raise attack counter
    public void UpdateRaiseAttack()
    {
        // reduces your attack back to what it was
        if (raiseAttackTurns > 0)
        {
            raiseAttackTurns--;
            if (raiseAttackTurns == 0)
            {
                damage -= 20;
            }
        }
        // make sure it doesnt go below 0
        if (raiseAttackTurns < 0)
        {
            raiseAttackTurns = 0;
        }

        // make sure that you only get 3 turns off raised attack
        if (raiseAttackTurns > 3)
        {
            raiseAttackTurns = 3;
        }
    }

    // increases raise defense by 3
    public void RaiseDefense()
    {
        raiseDefenseTurns = 3;
        // make sure that you only get 3 turns off raised defense
        if (raiseDefenseTurns > 3)
        {
            raiseDefenseTurns = 3;
        }
    }

    // sets sleep to true and makes enemy sleep for 1 turn
    public void Sleep()
    {
        sleeping = true;
        sleepTurnsRemaining = 1;
    }

    // for checking if enemy sleeping
    public bool IsSleeping()
    {
        return sleeping;
    }

    // reduce counter by 1
    public void PassTurn()
    {
        if (sleeping)
        {
            sleepTurnsRemaining--;

            if (sleepTurnsRemaining <= 0)
            {
                sleeping = false;
            }
        }
    }

    // heals the hp by the heal amount
    public void Heal()
    {
        int healAmount = 20;
        currentHP += healAmount;
        if (currentHP > maxHP) 
        {
            currentHP = maxHP;
        }
    }
}
