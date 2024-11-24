using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCaster : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private InputActionReference castLaunchInput;
    [SerializeField]
    private InputActionReference castAtSelfInput;
    [SerializeField]
    private InputActionReference detonateInput;
    [SerializeField]
    private Transform castSourcePoint;
    //These will be filled by a serializedobject later
    [Header("Spell Settings")]
    public BaseSpellObject spellObject;
    public GameObject spellProyectile;
    public string spellProyectileName;
    public float proyectileSpeed;
    public float rate;
    [Tooltip("If true while pushing the spell button it will do the spell as soon as possible")]
    public bool isAutomatic;
    public Action<GameObject, List<SpellAtribute>> onProyectileAction;
    public Action<GameObject, List<SpellAtribute>> hitAction;
    public string[] tagProyectileDetects;
    public Action<GameObject, List<SpellAtribute>> castAtSelfAction;
    public Action<List<SpellAtribute>> detonateAction;
    public List<SpellAtribute> currentAtributes;
    [Header("Visual spell settings")]
    public GameObject castParticle;
    public GameObject proyectileHitParticle;

    //Data
    

    void Start()
    {
        ChangeSpell(spellObject);
    }


    void Update()
    {
        spellObject.timeSinceLastCast += Time.deltaTime;
        if (isAutomatic)
        {
            AutomaticCasting();
        } else
        {
            SemiAutomaticCasting();
        }
        
    }

    private void SemiAutomaticCasting()
    {
        if (castLaunchInput.action.WasPressedThisFrame() && spellObject.timeSinceLastCast >= rate)
        {
            //TODO cast launch animation and particles
            spellObject.timeSinceLastCast = 0;
            CastProyectile();
        }
        if (detonateInput.action.WasPressedThisFrame())
        {
            //TODO detonate animation and particles
            detonateAction.Invoke(currentAtributes);
        }
        if (castAtSelfInput.action.WasPressedThisFrame() && spellObject.timeSinceLastCast >= rate)
        {
            spellObject.timeSinceLastCast = 0;
            //TODO cast at self animation and particles
            castAtSelfAction.Invoke(gameObject, currentAtributes);
        }
    }

    private void AutomaticCasting()
    {
        if (castLaunchInput.action.IsPressed() && spellObject.timeSinceLastCast >= rate)
        {
            //TODO cast launch animation and particles
            spellObject.timeSinceLastCast = 0;
            CastProyectile();
        }
        if (detonateInput.action.WasPressedThisFrame())
        {
            //TODO detonate animation and particles
            detonateAction.Invoke(currentAtributes);
        }
        if (castAtSelfInput.action.IsPressed() && spellObject.timeSinceLastCast >= rate)
        {
            spellObject.timeSinceLastCast = 0;
            //TODO cast at self animation and particles
            castAtSelfAction.Invoke(gameObject, currentAtributes);
        }
    }

    private void ChangeSpell(BaseSpellObject newSpellObject)
    {
        //Functional
        spellProyectile = newSpellObject.spellProyectile;
        spellProyectileName = newSpellObject.spellProyectileName;
        proyectileSpeed = newSpellObject.proyectileSpeed;
        rate = newSpellObject.rate;
        isAutomatic = newSpellObject.isAutomatic;
        tagProyectileDetects = newSpellObject.tagProyectileDetects;
        currentAtributes = newSpellObject.atributes;
        onProyectileAction = SpellManager.ReturnSpell(newSpellObject.spellType).ApplyToProyectile;
        hitAction = SpellManager.ReturnSpell(newSpellObject.spellType).Hit;
        castAtSelfAction = SpellManager.ReturnSpell(newSpellObject.spellType).SelfCast;
        detonateAction = SpellManager.ReturnSpell(newSpellObject.spellType).Detonate;
    }

    private void CastProyectile()
    {
        //TODO create pool for proyectiles
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //RaycastHit hit;
        Vector3 targetPoint;
        targetPoint = ray.GetPoint(5);
        Vector3 shootDir = targetPoint - castSourcePoint.position;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    targetPoint = hit.point;
        //}
        //else
        //{
        //    targetPoint = ray.GetPoint(5);
        //}
        GameObject newProyectile = Instantiate(spellProyectile, castSourcePoint.position, castSourcePoint.rotation);
        onProyectileAction.Invoke(newProyectile, currentAtributes);
        newProyectile.GetComponent<Rigidbody>().linearVelocity = shootDir.normalized * proyectileSpeed;
        newProyectile.GetComponent<SpellProyectile>().SetProyectileSettings(hitAction,currentAtributes, tagProyectileDetects,spellProyectileName,proyectileHitParticle);
    }


}
