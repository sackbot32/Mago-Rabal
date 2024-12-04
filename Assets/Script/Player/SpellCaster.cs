using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField]
    private Animator armAnim;
    [SerializeField]
    private Image spellImage;
    private SpellSelector spellSelector;
    //These will be filled by a serializedobject later
    [Header("Spell Settings")]
    public BaseSpellObject currentSpellObject;
    public Sprite currentImage;
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
        DOTween.Init();
        if(GameManager.instance.player == null)
        {
            GameManager.instance.player = gameObject;
        }
        spellSelector = GameManager.instance.spellSelector;
        spellSelector.spellCaster = this;
        if(GameManager.instance.currentSpellSlot == 0 && GameManager.instance.currentSpotInSlot == 0)
        {
            ChangeSpell(spellSelector.spellSlots[0].spells[0]);
        } else
        {
            ChangeSpell(spellSelector.spellSlots[GameManager.instance.currentSpellSlot].spells[GameManager.instance.currentSpotInSlot]);
        }
    }


    void Update()
    {
        if(Time.timeScale > 0)
        {
            currentSpellObject.timeSinceLastCast += Time.deltaTime;
        
            if (isAutomatic)
            {
                AutomaticCasting();
            } else
            {
                SemiAutomaticCasting();
            }
        }
        
    }

    private void SemiAutomaticCasting()
    {
        if (castLaunchInput.action.WasPressedThisFrame() && currentSpellObject.timeSinceLastCast >= rate)
        {
            //TODO cast launch animation and particles
            currentSpellObject.timeSinceLastCast = 0;
            armAnim.Play("lanzar", -1, 0);
            armAnim.SetBool("AutomaticShooting", false);
            if (castParticle != null)
            {
                GameObject casticle = Instantiate(castParticle, castSourcePoint.position + (castSourcePoint.forward).normalized * 0.2f, Quaternion.identity);
                casticle.transform.forward = castSourcePoint.forward;
                casticle.transform.parent = castSourcePoint;
            }
            CastProyectile();
        }
        if (detonateInput.action.WasPressedThisFrame())
        {
            //TODO detonate animation and particles
            armAnim.Play("cerrar");
            detonateAction.Invoke(currentAtributes);
        }
        if (castAtSelfInput.action.WasPressedThisFrame() && currentSpellObject.timeSinceLastCast >= rate)
        {
            armAnim.Play("amimismo", -1, 0);
            currentSpellObject.timeSinceLastCast = 0;
            //TODO cast at self animation and particles
            castAtSelfAction.Invoke(gameObject, currentAtributes);
        }
    }

    private void AutomaticCasting()
    {
        if (castLaunchInput.action.IsPressed() && currentSpellObject.timeSinceLastCast >= rate)
        {
            //TODO cast launch animation and particles
            if(castParticle != null)
            {
                GameObject casticle = Instantiate(castParticle, castSourcePoint.position + (castSourcePoint.forward).normalized * 0f, Quaternion.identity);
                casticle.transform.forward = castSourcePoint.forward;
                casticle.transform.parent = castSourcePoint;
            }
            currentSpellObject.timeSinceLastCast = 0;
            armAnim.SetBool("AutomaticShooting", true);
            CastProyectile();
        }
        armAnim.SetBool("AutomaticShooting", castLaunchInput.action.IsPressed());

        if (detonateInput.action.WasPressedThisFrame())
        {
            //TODO detonate animation and particles
            armAnim.Play("cerrar");
            detonateAction.Invoke(currentAtributes);
        }
        if (castAtSelfInput.action.WasPressedThisFrame() && currentSpellObject.timeSinceLastCast >= rate)
        {
            armAnim.Play("amimismo", -1, 0);
            currentSpellObject.timeSinceLastCast = 0;
            //TODO cast at self animation and particles
            castAtSelfAction.Invoke(gameObject, currentAtributes);
        }
    }

    public void ChangeSpell(BaseSpellObject newSpellObject)
    {
        //Functional
        currentSpellObject = newSpellObject;
        spellProyectile = newSpellObject.spellProyectile;
        spellProyectileName = newSpellObject.spellProyectileName;
        proyectileSpeed = newSpellObject.proyectileSpeed;
        rate = newSpellObject.rate;
        castParticle = newSpellObject.castParticle;
        proyectileHitParticle = newSpellObject.proyectileHitParticle;
        isAutomatic = newSpellObject.isAutomatic;
        tagProyectileDetects = newSpellObject.tagProyectileDetects;
        currentAtributes = newSpellObject.atributes;
        currentImage = newSpellObject.spellSprite;
        StartCoroutine(ChangeImage(currentImage));
        onProyectileAction = SpellManager.ReturnSpell(newSpellObject.spellType).ApplyToProyectile;
        hitAction = SpellManager.ReturnSpell(newSpellObject.spellType).Hit;
        castAtSelfAction = SpellManager.ReturnSpell(newSpellObject.spellType).SelfCast;
        detonateAction = SpellManager.ReturnSpell(newSpellObject.spellType).Detonate;
    }

    private IEnumerator ChangeImage(Sprite newImage)
    {
        DOTween.To(() => spellImage.color, x=> spellImage.color = x, new Color(1,1,1,0),0.25f);
        while(spellImage.color.a > 0)
        {
            yield return null;
        }
        spellImage.sprite = newImage;
        while(spellImage.sprite != currentImage)
        {
            spellImage.sprite = currentImage;
        }
        DOTween.To(() => spellImage.color, x => spellImage.color = x, new Color(1, 1, 1, 1), 0.15f);
        yield return null;
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
