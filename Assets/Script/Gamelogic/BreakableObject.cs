using UnityEngine;

public class BreakableObject : MonoBehaviour
{

    public string neededSpellName;
    private SpellProyectile proyectile;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SpellProyectile>() != null)
        {
            proyectile = other.gameObject.GetComponent<SpellProyectile>();
            if (proyectile.proyectileName == neededSpellName)
            {
                Destroy(proyectile.gameObject);
                Destroy(gameObject);

            }
        }
        print("gameObject trig name: " + other.gameObject.name);
    }
}
