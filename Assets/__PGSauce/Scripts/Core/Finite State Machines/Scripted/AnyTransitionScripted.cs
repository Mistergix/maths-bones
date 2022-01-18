using System.Collections.Generic;
using PGSauce.Core.FSM.Base;

namespace PGSauce.Core.FSM.Scripted
{
    public sealed class AnyTransitionScripted<T> : TransitionScripted<T>, AnyTransitionBase<T> where T : MonoStateMachineBase<T>
    {
        public AnyTransitionScripted(StateScripted<T> to, Decision decision, bool allowLoop, List<StateBase<T>> excludedFromStates) : base(to,decision,allowLoop)
        {
            ExcludedFromStates = excludedFromStates;
        }

        public List<StateBase<T>> ExcludedFromStates { get; }
    }
}