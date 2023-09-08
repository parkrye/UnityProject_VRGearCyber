using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID; 

namespace PID
{
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
        /// <summary>
        /// Checks within Dict if State with such enum is registered in the StateMachine. 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
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

        //Physics or LateUpdate Function should and could be made. 
        public void ChangeState(TState newState)
        {
            curState.Exit();
            curState = states[newState];
            curStateName = newState;
            curState.Enter();
        }
    }
}