using UnityEngine;

public interface IPawnMovement
{
    void StartMoving();
    void StopMoving();
    bool ReachedEndOfLane();
}

public interface IPawnSkill
{
    bool CanUseSkill();
    void UseSkill();
    bool IsSkillTargetInRange(out GameObject skillTarget);
}