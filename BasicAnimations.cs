using Enums;
using UnityEngine;

public static class BasicAnimations
{
    public static void MoveLocalTransform(
        Transform what, Vector3 deltaTo,
        float time, BasicAnimConditions direction)
    {
        switch (direction)
        {
            case BasicAnimConditions.Negative:
                deltaTo = deltaTo * -1;
                break;
        }

        what.LeanMoveLocal(
                what.transform.localPosition + deltaTo, time)
            .setEaseOutSine();
    }
    
    public static void MoveTransform(
        Transform what, Vector3 deltaTo,
        float time, BasicAnimConditions direction)
    {
        switch (direction)
        {
            case BasicAnimConditions.Negative:
                deltaTo = deltaTo * -1;
                break;
        }

        what.LeanMove(
                what.transform.position + deltaTo, time)
            .setEaseOutSine();
    }
    
}