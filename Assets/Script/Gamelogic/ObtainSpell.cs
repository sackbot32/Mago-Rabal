using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObtainSpell : MonoBehaviour
{
    public InteractZone zone;
    public BaseSpellObject spellObject;
    public Image spellImage;
    public int slot;
    public int posInslot;
    private Transform player;
    private Vector3 dirToPlayer;
    void Start()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.spellSelector.ContainsThisSpell(spellObject))
            {
                Destroy(gameObject);
            }
        }

        spellImage.sprite = spellObject.spellSprite;

        zone.interactAction = GiveSpellToPlayer;

        player = zone.player;
    }

    private void Update()
    {
        if(player == null)
        {
            player = zone.player;
        }
        dirToPlayer = (player.position - transform.position).normalized;
        spellImage.transform.parent.transform.right = -dirToPlayer;
        spellImage.transform.parent.transform.rotation = Quaternion.Euler(0, spellImage.transform.parent.transform.rotation.eulerAngles.y, 0);
    }

    public void GiveSpellToPlayer()
    {
        GameManager.instance.spellSelector.AddSpell(spellObject, slot,posInslot);
        Destroy(gameObject);
    }
}
