using UnityEngine;

public class NPC : Interactable
{
    public string npcName;

    public override string GetDescription()
    {
        return "Talk to NPC";
    }
}
