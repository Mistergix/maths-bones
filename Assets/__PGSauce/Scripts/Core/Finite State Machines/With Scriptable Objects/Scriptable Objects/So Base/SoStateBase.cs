using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using MonKey.Extensions;
using Sirenix.OdinInspector;

namespace PGSauce.Core.FSM.WithSo
{
    public abstract class SoStateBase : ScriptableObject
    {
        [SerializeField] private bool isNullState;
        [SerializeField] [HideIf("IsNullState")]
        private string stateName;

        public int debugIndex;

        public string StateName => IsNullState ? "NULL" : (stateName.IsNullOrEmpty() ? name : stateName);
        public bool IsNullState => isNullState;

        public abstract List<TransitionBase> GetTransitions();

        public List<TransitionBase> TemporaryTransitions { get; private set; } = new List<TransitionBase>();

        public void AddTemporaryTransition(AnyTransitionBase any)
        {
            TemporaryTransitions ??= new List<TransitionBase>();

            TemporaryTransitions.Add(new TransitionInterface(any.GetTargetState(), any.GetDecision(), any.ReverseValue()));
        }

        public void CleanTemporaryTransitions()
        {
            TemporaryTransitions?.Clear();
        }

        public List<TransitionBase> GetTransitionsIncludingTemporary()
        {
            var list = new List<TransitionBase>(GetTransitions());
            list.AddRange(TemporaryTransitions);
            return list;
        }
    }
}
