using UnityEngine;

public enum EFaction
{
    Player,
    Enemy
}

public class Entity_Base : MonoBehaviour
{
    [SerializeField] EFaction _Faction;

    public EFaction Faction => _Faction;
}
