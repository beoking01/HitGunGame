using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;
    // property for patrol state
    public void Initialise()
    {
        // setup defaut state.
        ChangeState(new PatrolState());
    }
    void Update()
    {
        if (activeState != null)
        {
            activeState.Perform();
        }
    }
    public void ChangeState(BaseState newState)
    {
        if (activeState != null)
        {
            // run clear up on activeStated.
            activeState.Exit();
        }
        // change to a new
        activeState = newState;
        // fail-safe null check to make sure the new state wasn't null
        if (activeState != null)
        {
            // setup new state
            activeState.stateMachine = this;
            // assgin state enemy class
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }
}
