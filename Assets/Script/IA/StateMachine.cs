using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    Stack<State> states;
    void Awake()
    {
        states = new Stack<State>();
    }


    private void Update()
    {
        if (GetCurrentState() != null)
        {
            GetCurrentState().Execute();
        }
    }

    public void PushState(System.Action active, System.Action onEnter, System.Action onExit)
    {
        if (GetCurrentState() != null)
        {
            GetCurrentState().OnExit();
        }

        State state = new State(active, onEnter, onExit);
        states.Push(state);
        GetCurrentState().OnEnter();

    }

    public void PopState()
    {
        GetCurrentState().OnExit();
        GetCurrentState().ActiveAction = null;
        states.Pop();
        GetCurrentState().OnEnter();
    }

    private State GetCurrentState()
    {
        return states.Count > 0 ? states.Peek() : null;
    }
}
