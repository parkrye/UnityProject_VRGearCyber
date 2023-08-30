using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TState, TOwner> where TOwner : MonoBehaviour
{
    private TOwner owner;
    private Dictionary<TState, StateBase<TState, TOwner>> states;
    private StateBase<TState, TOwner> curState;
    public TState curStateName; 
    public StateMachine(TOwner owner)
    {
        this.owner = owner;
        this.states = new Dictionary<TState, StateBase<TState, TOwner>>();
    }

    public bool CheckState(TState state) 
    {
        if (states.ContainsKey(state)) 
        { 
            return true; 
        }
        else
        {
            return false; 
        }
    }

    public StateBase<TState, TOwner> RetrieveState(TState state)
    {
        return states[state];
    }
    
    public void AddState(TState state, StateBase<TState, TOwner> stateBase)
    {
        states.Add(state, stateBase);
    }

    public void SetUp(TState startState)
    {
        foreach (StateBase<TState, TOwner> state in states.Values)
        {
            state.Setup();
        }

        curState = states[startState];
        curStateName = startState; 
        curState.Enter();
    }

    public void Update()
    {
        curState.Update();
        curState.Transition();
    }

    public void ChangeState(TState newState)
    {
        curState.Exit();
        curState = states[newState];
        curStateName = newState; 
        curState.Enter();
    }
}
