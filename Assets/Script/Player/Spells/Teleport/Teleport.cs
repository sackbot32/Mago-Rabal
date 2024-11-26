using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Teleport : SpellBase
{

    private string targetGameObjectKey = "TeleportTarget";
    private string teleportParticleGameObjectKey = "TeleportParticle";
    public void ApplyToProyectile(GameObject proyectile, List<SpellAtribute> atributes)
    {

    }

    public void Detonate(List<SpellAtribute> atributes)
    {
        GameObject targetGM;
        GameObject particle;
        SpellDictionary.instance.spellGameObjectDict.TryGetValue(targetGameObjectKey, out targetGM);
        SpellDictionary.instance.spellGameObjectDict.TryGetValue(teleportParticleGameObjectKey, out particle);
        if (targetGM != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 newPlayerPos = targetGM.transform.position + new Vector3(0,1,0);
            if(targetGM.tag == "Enemy")
            {
                Vector3 newEnemPos = player.transform.position + new Vector3(0,1,0);
                targetGM.transform.position = newEnemPos;
                GameObject.Instantiate(particle, newEnemPos - new Vector3(0, 1, 0), Quaternion.identity);

            } else
            {
                GameObject.Destroy(targetGM);
            }
            SpellDictionary.instance.spellGameObjectDict.Remove(targetGameObjectKey);
            player.transform.position = newPlayerPos;
            GameObject.Instantiate(particle, newPlayerPos - new Vector3(0, 1, 0), Quaternion.identity);
        }
    }

    public void Hit(GameObject hitObj, List<SpellAtribute> atributes)
    {
        GameObject oldTargetGM;
        if(SpellDictionary.instance.spellGameObjectDict.TryGetValue(targetGameObjectKey, out oldTargetGM))
        {
            if(oldTargetGM != null)
            {
                if(oldTargetGM.tag != "Enemy")
                {
                    GameObject.Destroy(oldTargetGM);
                }
            }
        }
        GameObject newTargetGM = null;
        if(hitObj.tag == "Enemy")
        {
            newTargetGM = hitObj;
        } else if(hitObj.tag == "Floor")
        {

            Vector3 hitPoint = new Vector3(atributes.Find(x => x.name == "HitPosX").value, atributes.Find(y => y.name == "HitPosY").value, atributes.Find(z => z.name == "HitPosZ").value);
            Debug.Log(hitPoint);
            newTargetGM = new GameObject();
            newTargetGM.transform.position = hitPoint;
        }
        SpellDictionary.instance.spellGameObjectDict[targetGameObjectKey] = newTargetGM;
        
    }

    public void SelfCast(GameObject player, List<SpellAtribute> atributes)
    {
        GameObject particle;
        SpellDictionary.instance.spellGameObjectDict.TryGetValue(teleportParticleGameObjectKey, out particle);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject chosenEnemy = enemies[Random.Range(0, enemies.Length)];
        bool rightEnemy = false;
        int failsafe = 0;
        RaycastHit hit;
        while (!rightEnemy && failsafe < 10)
        {
            failsafe += 1;
            if (Physics.Raycast(player.transform.position,chosenEnemy.transform.position - player.transform.position,out hit))
            {
                if(hit.transform.tag == "Enemy")
                {
                    rightEnemy = true;
                }
                else
                {
                    chosenEnemy = enemies[Random.Range(0, enemies.Length)];
                }
            }
        }
        if (rightEnemy)
        {
            Vector3 newEnemPos = player.transform.position + new Vector3(0, 1, 0);
            Vector3 newPlayerPos = chosenEnemy.transform.position + new Vector3(0, 1, 0);
            chosenEnemy.transform.position = newEnemPos;
            player.transform.position = newPlayerPos;
            GameObject.Instantiate(particle, newPlayerPos - new Vector3(0, 1, 0), Quaternion.identity);
            GameObject.Instantiate(particle, newEnemPos - new Vector3(0, 1, 0), Quaternion.identity);
        }
    }



}
