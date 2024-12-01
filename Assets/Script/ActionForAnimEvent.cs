using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionForAnimEvent : MonoBehaviour
{

    public List<Action> actionList;

    private void Awake()
    {
        actionList = new List<Action>();
    }

    public void PlayActionOnIndex(int index)
    {
        actionList[index].Invoke();
    }
}
