using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

[System.Serializable]
public class BonusController : MonoBehaviour
{
    [Header("Tap Bonus Buttons")]
    [SerializeField]
    private List<BonusChest> Chest_References;
    [SerializeField]
    private TMP_Text[] Bonus_Text;
    [SerializeField]
    private TMP_Text Total_Bonus;
    [SerializeField]
    private GameObject Bonus_Object;
    [SerializeField]
    private SlotBehaviour slotManager;
    [SerializeField]
    private AudioController _audioManager;
    //[SerializeField]
    //private GameObject PopupPanel;
    [SerializeField]
    private Transform Win_Transform;
    [SerializeField]
    private Transform Loose_Transform;

    // [Header("For Testing Purpose Only...")]

    private int[] m_BonusChestIndices; //Testing Bonus Data To Entered In The Unity Editor
    private int m_Chest_Index_Count = 0;
    private double m_total_bonus = 0;
    private bool IsOpening;
    private double multiplier;
    [SerializeField] private SlotBehaviour slotBehaviour;
    private void Start()
    {
        Chest_References[0].m_Chest_Button.onClick.RemoveAllListeners();
        Chest_References[0].m_Chest_Button.onClick.AddListener(delegate { OnClickOpenBonus(0); });

        Chest_References[1].m_Chest_Button.onClick.RemoveAllListeners();
        Chest_References[1].m_Chest_Button.onClick.AddListener(delegate { OnClickOpenBonus(1); });

        Chest_References[2].m_Chest_Button.onClick.RemoveAllListeners();
        Chest_References[2].m_Chest_Button.onClick.AddListener(delegate { OnClickOpenBonus(2); });

        Chest_References[3].m_Chest_Button.onClick.RemoveAllListeners();
        Chest_References[3].m_Chest_Button.onClick.AddListener(delegate { OnClickOpenBonus(3); });

        Chest_References[4].m_Chest_Button.onClick.RemoveAllListeners();
        Chest_References[4].m_Chest_Button.onClick.AddListener(delegate { OnClickOpenBonus(4); });
    }

    internal void StartBonus(List<int> bonusResult, double mult)
    {
        //if (PopupPanel) PopupPanel.SetActive(false);
        if (Win_Transform) Win_Transform.gameObject.SetActive(false);
        if (Loose_Transform) Loose_Transform.gameObject.SetActive(false);
        // if (_audioManager) _audioManager.SwitchBGSound(true);
        if(_audioManager) _audioManager.playBgAudio("bonus");
        if(_audioManager) _audioManager.StopWLAaudio();
        if (Bonus_Object) Bonus_Object.SetActive(true);
        m_BonusChestIndices = bonusResult.ToArray();
        multiplier = mult;

    }

    //Get Bonus Data From Backend Through bonusData of type List<string>
    // internal void PopulateWheel(List<string> bonusdata)
    // {
    //     for (int i = 0; i < bonusdata.Count; i++)
    //     {
    //         if (bonusdata[i] == "-1") 
    //         {
    //             if (Bonus_Text[i]) Bonus_Text[i].text = "NO \nBONUS";
    //         }
    //         else
    //         {
    //             if (Bonus_Text[i]) Bonus_Text[i].text = bonusdata[i];
    //         }   
    //     }
    // }

    private void OnClickOpenBonus(int indexOfChest)
    {
        if (IsOpening) return;

        // Debug.Log(string.Concat("<color=red>", "Click On Chest Detected... ", indexOfChest, "</color>"));
        StartCoroutine(DisablePassedIndexChest(indexOfChest));
    }

    private IEnumerator DisablePassedIndexChest(int indexOfChest)
    {
        bool value_Config = false;
        IsOpening = true;
        double bonusAmount = multiplier * m_BonusChestIndices[m_Chest_Index_Count];
        // TMP_Text bonusText=Win_Transform.GetChild(0).GetComponent<TMP_Text>();
        //Loop Through The References To Find The Element Of The Index
        //Checking if in the array of the possibilities it comes zero then game will get over.

        // Debug.Log(string.Concat("<color=green><b>", "Nice Bro...", "</b></color>"));
        //Using ImageAnimation custom script to animate the chest frame by frame.
        Chest_References[indexOfChest].m_Chest_Button.GetComponent<ImageAnimation>().StartAnimation();
        //Using DOTween to animate the score on click on the chest
        DoAnimationOnChestClick(indexOfChest, bonusAmount);
        Chest_References[indexOfChest].m_Chest_Button.interactable = false;
        //Playing the sound
        // PlayWinLooseSound(true);
        // Win_Transform.GetChild(0).GetComponent<TMP_Text>().text = bonusAmount.ToString();

        m_total_bonus += bonusAmount;
        Total_Bonus.text = m_total_bonus.ToString();

        yield return new WaitForSeconds(0.5f);
        Chest_References[indexOfChest].m_Chest_Button.GetComponent<ImageAnimation>().StopAnimation();

        if (m_BonusChestIndices[m_Chest_Index_Count] != 0)
        {
            value_Config = true;

        }
        // StopCoroutine(DisablePassedIndexChest(indexOfChest));
        // StopCoroutine(ActivateWinLooseImage(value_Config));
        m_Chest_Index_Count++;
        yield return new WaitForSeconds(0.5f);
        if (!value_Config)
        {
            yield return new WaitForSeconds(1.5f);
            ResetChestBonusButtons();
        }
        IsOpening = false;


    }

    private void ResetChestBonusButtons()
    {
        Bonus_Object.SetActive(false);
        if(_audioManager) _audioManager.playBgAudio();

        m_Chest_Index_Count = 0;
        m_total_bonus = 0;
        multiplier = 0;
        Total_Bonus.text = string.Concat("Bonus Score", "\n\n", m_total_bonus.ToString());
        foreach (var item in Chest_References)
        {
            item.m_Chest_Button.GetComponent<ImageAnimation>().StopAnimation();
            item.m_Chest_Button.interactable = true;
        }
        ResetToDefaultAnimationAfterChestClick();

    }

    private IEnumerator ActivateWinLooseImage(bool winLose)
    {
        //PopupPanel.SetActive(true);
        if (winLose)
        {
            Win_Transform.gameObject.SetActive(true);
            Loose_Transform.gameObject.SetActive(false);
        }
        else
        {
            Win_Transform.gameObject.SetActive(false);
            Loose_Transform.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1.2f);
        //PopupPanel.SetActive(false);
        Win_Transform.gameObject.SetActive(false);
        Loose_Transform.gameObject.SetActive(false);
    }

    internal void PlayWinLooseSound(bool isWin)
    {
        // if (isWin)
        // {
        //     _audioManager.PlayBonusAudio("win");
        // }
        // else
        // {
        //     _audioManager.PlayBonusAudio("lose");
        // }
    }

    #region DOTween Animations and Reset Animations
    private void DoAnimationOnChestClick(int m_index, double m_score)
    {
        if (_audioManager) _audioManager.StopWLAaudio();

        BonusChest m_Temp_Chest = Chest_References[m_index];

        if (m_score > 0)
        {
            m_Temp_Chest.m_Score.text = "+" + m_score.ToString();
            if (_audioManager) _audioManager.PlayWLAudio("bonuswin");

        }
        else
        {

            m_Temp_Chest.m_Score.text = "Game Over";
            if (_audioManager) _audioManager.PlayWLAudio("bonuslose");

        }

        m_Temp_Chest.m_ScoreHolder.SetActive(true);
        DOTweenScale(m_Temp_Chest.m_ScoreHolder.transform, m_Temp_Chest.m_ScoreHolder.transform, 1f);
    }

    private void DOTweenScale(Transform m_rect_transform, Transform m_obj_transform, float m_time)
    {
        m_rect_transform.DOScale
            (
                m_obj_transform.localScale + (Vector3.one * 1.2f),
                m_time
            );
        m_rect_transform.DOLocalMoveY
            (
                m_obj_transform.position.y + 220,
                m_time
            ).OnComplete(() =>
            {

                m_obj_transform.gameObject.SetActive(false);
            });
        //m_obj_transform.localScale = Vector3.one * 1.5f;

        //m_obj_transform.position = m_obj_transform.position + (Vector3.up * 2);
    }

    //This method is used to reset the bonus chest to default and ready for next bonus
    private void ResetToDefaultAnimationAfterChestClick()
    {
        foreach (var i in Chest_References)
        {
            i.m_ScoreHolder.transform.localScale = Vector3.zero;
            i.m_ScoreHolder.transform.localPosition = new Vector3(0.1f, 0.1f, 0.1f);
            i.m_ScoreHolder.SetActive(false);
        }
        slotBehaviour.CheckPopups = false;
    }
    #endregion

    #region Structures Used
    [System.Serializable]
    public struct BonusChest
    {
        public TMP_Text m_Score;
        public Button m_Chest_Button;
        public GameObject m_ScoreHolder;
    }
    #endregion
}
