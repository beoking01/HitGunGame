using UnityEngine;
using System.Collections.Generic;

public class PannelFieldList : MonoBehaviour
{
    [Header("Danh sách tất cả các Panels")]
    public List<GameObject> allPanels;


    public void OpenPanel(int panelIndex)
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            // Nếu i trùng với index truyền vào thì hiện, ngược lại thì ẩn
            allPanels[i].SetActive(i == panelIndex);
        }
    }
}