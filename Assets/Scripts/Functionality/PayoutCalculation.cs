using System.Collections.Generic;
using UnityEngine;

public class PayoutCalculation : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> Pay_Line_References = new List<GameObject>();

    List<int> tempIds = new List<int>();



    //use static pay lines provided
    internal void ActivatePayLine(int LineId, bool m_hovered)
    {
        if (LineId >= 0)
        {
            int m_actual_line_id = LineId;
            // for (int i = 0; i < Pay_Line_References.Count; i++)
            // {
            //     if (m_actual_line_id == i)
            //     {
                    Debug.Log(string.Concat("<color=yellow><b>", m_hovered.ToString(), m_actual_line_id.ToString(),"</b></color>"));
                    if (!m_hovered)
                    {
                        tempIds.Add(m_actual_line_id);
                    }
                    foreach(var _ in tempIds) { Debug.Log(string.Concat("<color=yellow><b>", tempIds, "</b></color>"));}
                    Pay_Line_References[m_actual_line_id].SetActive(true);
            //     }
            // }
        }
    }

    internal void ResetAllPayLines() { foreach (var i in Pay_Line_References) { i.SetActive(false); } tempIds.Clear(); }

    internal void TurnOffHoverPayline()
    {
        for(int i = 0; i < Pay_Line_References.Count; i++)
        {
            if(!tempIds.Contains(i))
            {
                Pay_Line_References[i].SetActive(false);
            }
        }
    }

}
