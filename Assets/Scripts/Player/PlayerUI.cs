using UnityEngine;
using TMPro;
// process UI displayed in player screen
public class PlayerUI : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI promtText;
    public TextMeshProUGUI pointText;
    private PlayerInventory playerInventory;
    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }
    void Update()
    {
        pointText.text = playerInventory.showMoney().ToString() + "$";
    }
    public void UpdateText(string promtMessage)
    {
        promtText.text = promtMessage;
    }
}
