using UnityEngine;

public abstract class BaseState
{
    // Instance of Enemy
    public Enemy enemy;
    // Instance of StateMachine class
    public StateMachine stateMachine;

    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();

}