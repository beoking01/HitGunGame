using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer; // how long enemy search player
    private float searchTimeMover; // how long player set up destination for find
    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnowPos);
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
        if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
            searchTimeMover += Time.deltaTime;
            searchTimer += Time.deltaTime;
            if (searchTimeMover > 4)
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                searchTimeMover = 0;
            }
            if (searchTimer > 10)
            {
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public override void Exit()
    {
        
    }
}
