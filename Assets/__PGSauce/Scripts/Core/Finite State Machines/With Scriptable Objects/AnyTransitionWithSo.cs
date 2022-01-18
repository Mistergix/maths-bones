using System.Collections.Generic;
using PGSauce.Core.FSM.Base;

namespace PGSauce.Core.FSM.WithSo
{
    public sealed class AnyTransitionWithSo<T> : TransitionWithSo<T>, AnyTransitionBase<T> where T : MonoStateMachineBase<T>
    {
        public AnyTransitionWithSo(StateBase<T> to, SoDecision<T> decision, bool reverseValue, bool allowLoop, List<StateBase<T>> excludedFromStates) : base(to, decision, reverseValue, allowLoop)
        {
            ExcludedFromStates = excludedFromStates;
        }

        public List<StateBase<T>> ExcludedFromStates { get; }
    }
}