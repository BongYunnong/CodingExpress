using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreedyManager : MonoBehaviour
{
    #region Variables & Initializer
    [SerializeField] Character character;

    [SerializeField] Text greedyModeTxt;

    [SerializeField] List<CustomGreedy> GreedyLists;
    [SerializeField] int greedyIndex = -1;

    bool camMode = true;
    [SerializeField] GameObject canvas;
    [SerializeField] Button startBtn;

    #endregion

    public void ClickLeftGreedyBtn()
    {
        AddGreedyIndex(GreedyLists.Count - 1);
    }
    public void ClickRightGreedyBtn()
    {
        AddGreedyIndex(1);
    }
    public void ClickStartBtn()
    {
        GreedyLists[greedyIndex].StartGreedy();
    }
    public void ClickResetBtn()
    {
        GreedyLists[greedyIndex].ResetGreedy();
        GreedyLists[greedyIndex].InitializeGreedy(character);
    }

    private void AddGreedyIndex(int _add)
    {
        int pastIndex = greedyIndex;
        greedyIndex = greedyIndex + _add;
        greedyIndex = greedyIndex % GreedyLists.Count;
        greedyIndex = Mathf.Clamp(greedyIndex, 0, GreedyLists.Count);
        greedyModeTxt.text = GreedyLists[greedyIndex].name;

        if (pastIndex != greedyIndex)
        {
            if (pastIndex != -1)
            {
                GreedyLists[pastIndex].ResetGreedy();
            }
            GreedyLists[greedyIndex].InitializeGreedy(character);
        }
    }

    private void Update()
    {
        if (greedyIndex >= 0)
        {
            bool canStart = GreedyLists[greedyIndex].proceeding == false && GreedyLists[greedyIndex].initialized;
            startBtn.interactable = canStart;
        }
        else
        {
            startBtn.interactable = false
                ;
        }
    }
}
