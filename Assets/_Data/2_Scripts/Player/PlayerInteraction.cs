using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject btnPressToTalk;
    public NPC npcInteract;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC") && collision.GetComponent<NPC>())
        {
            btnPressToTalk.SetActive(true); 
            npcInteract = collision.GetComponent<NPC>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            btnPressToTalk.SetActive(false);
            npcInteract = null;
        }
    }
}