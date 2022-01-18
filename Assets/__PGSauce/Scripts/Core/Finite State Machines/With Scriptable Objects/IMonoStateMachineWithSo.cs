using System.Collections.Generic;

namespace PGSauce.Core.FSM.WithSo
{
    public interface IMonoStateMachineWithSo
    {
        SoStateBase InitialState();
        List<AnyTransitionBase> GetAny();
        SoStateBase GetCurrentState();
    }
}