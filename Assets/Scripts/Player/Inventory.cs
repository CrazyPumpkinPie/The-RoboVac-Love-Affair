using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float water;
    [SerializeField] private float maxCapacity;
    public float MaxCapacity { get { return maxCapacity; } }
    [SerializeField] private float minCapacity;
    public float MinCapacity { get { return minCapacity; } }
    public float Water { 
        get
        {
            return water; 
        } 
        set
        {
            if (value < minCapacity)
            {
                water = minCapacity;
            }
            else if(value > maxCapacity)
            {
                water = maxCapacity;
            }
            else
            {
                water = value;
            }
        } 
    }
}
