using System.Collections.Generic;

namespace PGSauce.Core.FSM.WithSo
{
    public interface AnyTransitionBase : TransitionBase
    {
        public List<SoStateBase> ExcludedStates();
    }
}