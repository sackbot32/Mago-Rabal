using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
[Serializable]
public class SpellSlot
{
    public BaseSpellObject[] spells;
}
public class SpellSelector : MonoBehaviour
{
    public SpellCaster spellCaster;
    public SpellSlot[] spellSlots;
    public InputActionAsset selectInput;

    public int currentSpellSlot;
    public int[] weaponSelectedSlot;



    void Start()
    {
        weaponSelectedSlot = new int[4];
    }


    void Update()
    {
        if (selectInput.FindAction("1").WasPressedThisFrame())
        {
            ChangeSlot(0);
            ChangeSpell(currentSpellSlot, weaponSelectedSlot[currentSpellSlot]);
        }
        if (selectInput.FindAction("2").WasPressedThisFrame())
        {
            ChangeSlot(1);
            ChangeSpell(currentSpellSlot, weaponSelectedSlot[currentSpellSlot]);
        }
        if (selectInput.FindAction("3").WasPressedThisFrame())
        {
            ChangeSlot(2);
            ChangeSpell(currentSpellSlot, weaponSelectedSlot[currentSpellSlot]);
        }
        if (selectInput.FindAction("4").WasPressedThisFrame())
        {
            ChangeSlot(3);
            ChangeSpell(currentSpellSlot, weaponSelectedSlot[currentSpellSlot]);
        }
    }

    private void ChangeSlot(int newSpellSlot)
    {
        if (newSpellSlot < weaponSelectedSlot.Length)
        {

            if (currentSpellSlot != newSpellSlot)
            {
                currentSpellSlot = newSpellSlot;
                weaponSelectedSlot[currentSpellSlot] = 0;
            }
            else
            {
                weaponSelectedSlot[currentSpellSlot] += 1;
            }
            if(weaponSelectedSlot[currentSpellSlot] >= spellSlots[currentSpellSlot].spells.Length)
            {
                weaponSelectedSlot[currentSpellSlot] = 0;
            }
            GameManager.instance.currentSpellSlot = currentSpellSlot;
            GameManager.instance.currentSpotInSlot = weaponSelectedSlot[currentSpellSlot];

        }
    }

    private void ChangeSpell(int slot, int selectedInSlot)
    {
        if(spellCaster == null)
        {
            spellCaster = GameManager.instance.player.GetComponent<SpellCaster>();
        }
        if(spellSlots[slot].spells[selectedInSlot] != null)
        {
            spellCaster.ChangeSpell(spellSlots[slot].spells[selectedInSlot]);
        } else
        {
            print("error, no existe hechizo");
        }
    }

    public bool ContainsThisSpell(BaseSpellObject spellToCheck)
    {
        bool contains = false;
        foreach (SpellSlot slot in spellSlots)
        {
            foreach (BaseSpellObject spell in slot.spells)
            {
                if(spell == spellToCheck)
                {
                    contains = true;
                }
            }
        }

        return contains;
    }

    public void AddSpell(BaseSpellObject newSpell,int slot,int posInSlot)
    {
        BaseSpellObject oldSpellAtPoint;
        if(spellSlots[slot].spells[posInSlot] != null)
        {
            oldSpellAtPoint = spellSlots[slot].spells[posInSlot];
            spellSlots[slot].spells[posInSlot] = newSpell;
            for (int i = 0; i < spellSlots[slot].spells.Length; i++)
            {
                spellSlots[slot].spells[i] = oldSpellAtPoint;
            }
        } else
        {
            spellSlots[slot].spells[posInSlot] = newSpell;
        }
    }

}
