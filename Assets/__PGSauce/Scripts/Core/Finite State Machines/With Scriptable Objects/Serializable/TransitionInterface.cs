namespace PGSauce.Core.FSM.WithSo
{
    public class TransitionInterface : TransitionBase
    {
        private readonly SoStateBase _target;
        private readonly SoDecisionBase _decision;
        private readonly bool _reverse;

        public TransitionInterface(SoStateBase target, SoDecisionBase decision, bool reverse)
        {
            _target = target;
            _decision = decision;
            _reverse = reverse;
        }
        
        public SoDecisionBase GetDecision()
        {
            return _decision;
        }

        public SoStateBase GetTargetState()
        {
            return _target;
        }

        public bool ReverseValue()
        {
            return _reverse;
        }
    }
}