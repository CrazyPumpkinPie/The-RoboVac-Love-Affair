using UnityEngine;

public class aquaChan : MonoBehaviour
{
    [SerializeField] private float intakeSpeed = 0.5f;
    [SerializeField] private float minSizeToDestroy = 0.6f;
    [SerializeField] private float suckedWaterForce = 0.2f;
    [SerializeField] private AquaType type;
    Inventory inventory;
    private enum AquaType
    {
        water,
        oil
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            inventory = other.gameObject.GetComponent<Inventory>();
            if (type == AquaType.water)
            {
                if (inventory.Water < inventory.MaxCapacity)
                {
                    float f = intakeSpeed * Time.deltaTime;
                    transform.localScale -= new Vector3(f, f, f);
                    inventory.Water += suckedWaterForce * Time.deltaTime;
                }
                if (transform.localScale.x <= minSizeToDestroy)
                {
                    Destroy(gameObject);
                }
            }
            else if(type  == AquaType.oil)
            {
                if (inventory.Water > inventory.MinCapacity)
                {
                    float f = intakeSpeed * Time.deltaTime;
                    transform.localScale -= new Vector3(f, f, f);
                    inventory.Water -= suckedWaterForce * Time.deltaTime;
                }
                if (transform.localScale.x <= minSizeToDestroy)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
