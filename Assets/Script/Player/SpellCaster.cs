using System;
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
    public GameObject spellProyectile;
    [Header("Settings")]
    public BaseSpellObject spellObject;
    public Mesh spellProyectileMesh;
    public float proyectileSpeed;
    public float rate;
    public Action<GameObject> hitAction;
    public string[] tagProyectileDetects;
    public Action<GameObject> castAtSelfAction;
    public Action detonateAction;
    //Data

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spellProyectileMesh = spellObject.spellProyectileMesh;
        proyectileSpeed = spellObject.proyectileSpeed;
        rate = spellObject.rate;
        spellObject.timeSinceLastCast = rate;
        tagProyectileDetects = spellObject.tagProyectileDetects;
        hitAction = SpellManager.ReturnSpell(spellObject.spellType).Hit;
        castAtSelfAction = SpellManager.ReturnSpell(spellObject.spellType).SelfCast;
        detonateAction = SpellManager.ReturnSpell(spellObject.spellType).Detonate;
    }


    void Update()
    {
        spellObject.timeSinceLastCast += Time.deltaTime;
        if(castLaunchInput.action.WasPressedThisFrame() && spellObject.timeSinceLastCast >= rate)
        {
            //TODO cast launch animation and particles
            spellObject.timeSinceLastCast = 0;
            CastProyectile();
        }
        if (detonateInput.action.WasPressedThisFrame())
        {
            //TODO detonate animation and particles
            detonateAction.Invoke();
        }
        if (castAtSelfInput.action.WasPressedThisFrame() && spellObject.timeSinceLastCast >= rate)
        {
            spellObject.timeSinceLastCast = 0;
            //TODO cast at self animation and particles
            castAtSelfAction.Invoke(gameObject);
        }
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
        newProyectile.GetComponent<Rigidbody>().linearVelocity = shootDir.normalized * proyectileSpeed;
        newProyectile.GetComponent<SpellProyectile>().SetProyectileSettings(hitAction, tagProyectileDetects,spellProyectileMesh);
    }


}
