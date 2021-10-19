using Animancer;
using UnityEngine;

namespace Characters.States
{
    [DefaultExecutionOrder(DefaultExecutionOrder)]
    public class MultiState : CharacterState
    {
        public const int DefaultExecutionOrder = -1000;

        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("While in one of the States, should it try to enter them again in order every " + nameof(FixedUpdate) + "?")]
        private bool _AutoInternalTransitions;
        public bool AutoInternalTransitions => _AutoInternalTransitions;

        [SerializeField]
        [Tooltip("The other states that this one will try to enter in order")]
        private CharacterState[] _States;
        public CharacterState[] States => _States;

        private CharacterState _CurrentState;

        /************************************************************************************************************************/

        public override bool CanEnterState => Character.StateMachine.CanSetState(_States);

        public override bool CanExitState => true;

        /************************************************************************************************************************/

        public override void OnEnterState()
        {
            if (Character.StateMachine.TrySetState(_States))
            {
                if (_AutoInternalTransitions)
                {
                    _CurrentState = Character.StateMachine.CurrentState;
                    enabled = true;
                }
            }
            else
            {
                var text = ObjectPool.AcquireStringBuilder()
                    .AppendLine($"{nameof(MultiState)} failed to enter any of its {nameof(States)}:");

                for (int i = 0; i < _States.Length; i++)
                {
                    text.Append("    [")
                        .Append(i)
                        .Append("] ")
                        .AppendLine(_States[i].ToString());
                }

                Debug.LogError(text.ReleaseToString(), this);
            }
        }

        public override void OnExitState() { }

        /************************************************************************************************************************/

        protected virtual void FixedUpdate()
        {
            if (_CurrentState != Character.StateMachine.CurrentState)
            {
                enabled = false;
                return;
            }

            var newState = Character.StateMachine.CanSetState(_States);
            if (_CurrentState != newState && newState != null)
            {
                _CurrentState = newState;
                Character.StateMachine.ForceSetState(newState);
            }
        }
    }
}
