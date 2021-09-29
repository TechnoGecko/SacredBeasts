namespace Characters.States.Attack
{
    public abstract class AttackState : CharacterState
    {
        public override bool CanTurn => false;
    
        public override bool CanExitState => false;


        public override void OnExitState()
        {
            base.OnExitState();
            Character.Animancer.EndHitSequence();
        }
    
    }
}



