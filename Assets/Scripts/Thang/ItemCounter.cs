using UnityEngine;

public class ItemCounter : MonoBehaviour
{
    private int carotCount = 0;
    private int dualeoCount = 0;
    private int trungCount = 0;
    private int otCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("carot"))
        {
            carotCount++;
            Debug.Log("Số lượng carot đã chạm: " + carotCount);
        }
        else if (other.CompareTag("dualeo"))
        {
            dualeoCount++;
            Debug.Log("Số lượng dualeo đã chạm: " + dualeoCount);
        }
        else if (other.CompareTag("trung"))
        {
            trungCount++;
            Debug.Log("Số lượng trung đã chạm: " + trungCount);
        }
        else if (other.CompareTag("ot"))
        {
            otCount++;
            Debug.Log("Số lượng ot đã chạm: " + otCount);
        }
    }
}