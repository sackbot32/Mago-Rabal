using System.Collections.Generic;
using UnityEngine;

public enum SpellType : int
{
    TestA = 0,
    TestB = 1,
    FireBall = 2,
    MagicalImpulse = 3,
    Bless = 4,
    Teleport = 5

}
public static class SpellManager
{
    public static SpellBase ReturnSpell(SpellType spellType)
    {
        SpellBase sentSpell = null;

        switch (spellType)
        {
            case SpellType.TestA:
                sentSpell = new BasicSpellTestA();
                break;
            case SpellType.TestB: 
                sentSpell = new BasicSpellTestB(); 
                break;
            case (SpellType.FireBall):
                sentSpell = new Fireball();
                break; 
            case (SpellType.MagicalImpulse):
                sentSpell = new MagicalImpulse();
                break;
            case (SpellType.Bless):
                break;
            case (SpellType.Teleport):
                break;
                
        }

        return sentSpell;
    }


}
