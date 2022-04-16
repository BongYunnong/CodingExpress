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
    void Start()
    {
        AddGreedyIndex(1 + Random.Range(0, GreedyLists.Count));
    }
    #endregion

    public void AddGreedyIndex(int _add)
    {
        int pastIndex = greedyIndex;
        greedyIndex = greedyIndex + _add;
        greedyIndex = Mathf.Clamp(greedyIndex, 0, GreedyLists.Count - 1);
        greedyModeTxt.text = GreedyLists[greedyIndex].name;

        if (pastIndex != greedyIndex)
        {
            if (pastIndex != -1)
            {
                //GreedyLists[pastIndex].StopSort();
                //GreedyLists[pastIndex].DiscardSort();
            }
            GreedyLists[greedyIndex].InitializeGreedy(character);
        }
    }
    public void StartGreedy()
    {
        if (GreedyLists[greedyIndex].proceeding == false)
        {
            GreedyLists[greedyIndex].InitializeGreedy(character);
        }
    }
}
