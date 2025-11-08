using UnityEngine;

public class PatrolState : BaseState
{
    // track which waypoint we are currently targeting
    public int wayPointIndex = 0;
    private float waitTimer;
    public override void Enter()
    {

    }
    public override void Perform()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }
    public override void Exit()
    {

    }
    public void PatrolCycle()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer > 3)
        {
            // Implement our logic patrol
            if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance + 0.2f)
            {
                if (wayPointIndex < enemy.path.waypoints.Count - 1)
                {
                    wayPointIndex++;
                }
                else
                {
                    wayPointIndex = 0;
                }
                enemy.Agent.SetDestination(enemy.path.waypoints[wayPointIndex].position);
            }
            waitTimer = 0f;
        }
    }
}
