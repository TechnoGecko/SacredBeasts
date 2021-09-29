
using UnityEngine;
using Animancer;
using Animancer.FSM;


namespace Characters.States {
public abstract class CharacterState : StateBehaviour, IOwnedState<CharacterState>
{
    
    public const string MenuPrefix = Character.MenuPrefix + "States/";
    
    [SerializeField] private Character _Character;

    public Character Character => _Character;

    public void SetCharacter(Character character) => _Character = character;
    

    
    
#if UNITY_EDITOR
    /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.GetComponentInParentOrChildren(ref _Character);
    }
#endif
    /************************************************************************************************************************/
    public virtual float MovementSpeedMultiplier => 0;

    public virtual bool CanTurn => MovementSpeedMultiplier != 0;
   
    /************************************************************************************************************************/
    
    public StateMachine<CharacterState> OwnerStateMachine => _Character.StateMachine;
    
    
}
}