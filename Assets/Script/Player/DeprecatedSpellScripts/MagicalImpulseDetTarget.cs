using UnityEngine;
using System.Collections.Generic;

public class MagicalImpulseDetTarget : MonoBehaviour
{
    public static MagicalImpulseDetTarget instance;
    [SerializeField]
    private List<MagicalImpulseEffect> targets;
    void Awake()
    {
        instance = this;
        targets = new List<MagicalImpulseEffect>();
    }

    private void Start()
    {
        targets.Clear();
    }

    public void Add(MagicalImpulseEffect newTarget)
    {
        if (!targets.Contains(newTarget))
        {
            targets.Add(newTarget);
        }
    }

    public void Remove(MagicalImpulseEffect newTarget)
    {
        if (targets.Contains(newTarget))
        {
            targets.Remove(newTarget);
        }
    }

    public MagicalImpulseEffect[] ReturnList()
    {
        return targets.ToArray();
    }
}
