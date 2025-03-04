using UnityEngine;

public class DialogActorPopupEffect : MonoBehaviour
{
    public ActorStats actorStats;
    Animator animator;

    void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowActorPopup()
    {
        animator.SetBool("IsOpen", true);
    }

    public void HideActorPopup()
    {
        animator.SetBool("IsOpen", false);
    }
}
