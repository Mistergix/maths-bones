using System.Collections.Generic;
using PGSauce.Core.FSM.Base;

namespace PGSauce.Core.FSM.Scripted
{
    public abstract class StateScripted<T> : StateBase<T> where T : MonoStateMachineBase<T>
    {
        protected StateScripted()
        {
            Transitions = new List<TransitionBase<T>>();
        }
        
        public override string Name()
        {
            return GetType().ToString();
        }

        public override bool IsNullState => false;
    }
}
