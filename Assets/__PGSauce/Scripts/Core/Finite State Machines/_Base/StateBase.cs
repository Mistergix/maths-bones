using System.Collections.Generic;
using System.Linq;

namespace PGSauce.Core.FSM.Base
{
    /// <summary>
    /// The State the fsm can be in
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class StateBase<T> where T : MonoStateMachineBase<T>
    {
        public abstract string Name();
        public abstract bool IsNullState { get; }
        public List<TransitionBase<T>> Transitions { get; protected set; }

        public override string ToString()
        {
            return Name();
        }

        public int debugIndex;
        
        /// <summary>
        /// If a transition is valid, change the state
        /// </summary>
        /// <param name="controller"></param>
        /// <returns>true of the state has been changed</returns>
        public bool CheckTransitions(T controller)
        {
            return CheckTransitions(controller, Transitions);
        }

        /// <summary>
        /// If a transition is valid, change the state
        /// </summary>
        /// <param name="controller"></param>
        /// <returns>true of the state has been changed</returns>
        public bool CheckAnyTransitions(T controller, List<AnyTransitionBase<T>> any)
        {
            var from = controller.Fsm.CurrentState;
            var transitions = any.Where(t => !t.ExcludedFromStates.Contains(from)).Select(t => t as TransitionBase<T>).ToList();
            return CheckTransitions(controller, transitions);
        }

        public virtual void Enter(T controller)
        {
        }

        public virtual void Exit(T controller)
        {
        }

        public virtual void PhysicsUpdate(T controller)
        {
        }

        public virtual void LogicUpdate(T controller)
        {
        }
        
        private bool CheckTransitions(T controller, List<TransitionBase<T>> transitions)
        {
            var from = controller.Fsm.CurrentState;
            foreach (var t in transitions)
            {
                var to = t.To;
                if (from.Equals(to))
                {
                    var allowLoop = t.AllowLoop;
                    if(!allowLoop){continue;}
                }
                
                var decisionSucceeded = t.Decide(controller);

                if (!decisionSucceeded) continue;
                controller.Fsm.ChangeState(to);
                return true;
            }

            return false;
        }
    }
}