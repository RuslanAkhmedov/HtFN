using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MovementController movementController;
    public float stamine;
    public bool canUseStamine;

    private float restoreStamineTime = 3f;
    private bool isRestoringStamine = false;
    private float restoreStartTime;

    void Start()
    {
        stamine = 100f;
        canUseStamine = true;
    }

    void FixedUpdate()
    {
        bool isUsingStamine = movementController.useStamine; 

        if (isUsingStamine && canUseStamine)
        {
            stamine -= Time.fixedDeltaTime * 5;
            stamine = Mathf.Max(stamine, 0);

            if (stamine <= 0)
            {
                canUseStamine = false; 
            }

            isRestoringStamine = false;
            restoreStartTime = Time.time + restoreStamineTime;
        }

        // ≈сли не используетс€ стамина и не идет восстановление
        if (!isUsingStamine && !isRestoringStamine && stamine < 100f)
        {
            // ∆дем 3 секунды перед восстановлением
            if (Time.time >= restoreStartTime)
            {
                isRestoringStamine = true;
            }
        }

        if (isRestoringStamine)
        {
            stamine += Time.fixedDeltaTime * 30;
            if (stamine >= 100f)
            {
                stamine = 100f;
                canUseStamine = true;
                isRestoringStamine = false;
            }
        }
    }

}
