using UnityEngine;

public class ObtainSpell : MonoBehaviour
{
    public InteractZone zone;
    public BaseSpellObject spellObject;
    public int slot;
    public int posInslot;
    void Start()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.spellSelector.ContainsThisSpell(spellObject))
            {
                Destroy(gameObject);
            }
        }

        zone.interactAction = GiveSpellToPlayer;
    }

    public void GiveSpellToPlayer()
    {
        GameManager.instance.spellSelector.AddSpell(spellObject, slot,posInslot);
        Destroy(gameObject);
    }
}
