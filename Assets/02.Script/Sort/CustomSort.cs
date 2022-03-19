using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SortElement
{
    public SortElement(GameObject _obj, float _val)
    {
        elementObj = _obj;
        val = _val;
    }
    public void PlayAudio()
    {
        elementObj.GetComponent<AudioSource>().Play();
    }
    public GameObject elementObj;
    public float val;
}
public class CustomSort : MonoBehaviour
{
    #region Variables & Initializer
    public float SortSpeed = 0.001f;
    protected Character myCharacter;
    int arrCount = 100;
    [SerializeField] GameObject SortPrefab;
    [SerializeField] Material mat;
    protected List<SortElement> arr = new List<SortElement>();
    protected float spacing = 1;

    protected Color unitColor;

    public bool sorting { get; private set; }
    public void InitializeSort(int _count)
    {
        sorting = false;
        arrCount = _count;
        arr.Clear();
        // 랜덤한 값을 뽑아내기 위한 List 초기화
        List<float> randomArr = new List<float>();
        for (int i = 0; i < arrCount; i++)
        {
            randomArr.Add(i);
        }
        unitColor = new Color(1f / arrCount, 1f / arrCount, 1f / arrCount);

        this.transform.position = Vector3.left * arrCount / 2 * spacing;

        // randomArr에서 하나씩 랜덤하게 뺴서 실제 사용할 arr에 집어넣기
        for (int i = randomArr.Count - 1; i >= 0; i--)
        {
            int randomIndex = Random.Range(0, randomArr.Count);

            // 시각적으로 보여주기 위한 Object 생성 및 초기화
            GameObject tmp = Instantiate(SortPrefab, this.transform.position+ Vector3.right* spacing* arr.Count, Quaternion.identity);
            tmp.transform.SetParent(this.transform);
            SortElement sortElement = new SortElement(tmp, randomArr[randomIndex]);
            sortElement.elementObj.transform.localScale = new Vector3(1, sortElement.val, 1);
            sortElement.elementObj.GetComponentInChildren<MeshRenderer>().material = mat;
            sortElement.elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * sortElement.val;
            sortElement.elementObj.GetComponentInChildren<AudioSource>().pitch = (2f/arrCount) * sortElement.val;
            arr.Add(sortElement);
            randomArr.RemoveAt(randomIndex);
        }
    }
    #endregion

    #region SORT
    public virtual void Sort()
    {
        sorting = true;
    }
    protected void SortEnd()
    {
        sorting = false;
        myCharacter.SetEmotion(0);
    }
    public virtual void StopSort()
    {
        sorting = false;
        StopAllCoroutines();
    }
    public void DiscardSort()
    {
        for (int i = arr.Count - 1; i >= 0; i--)
        {
            Destroy(arr[i].elementObj);
        }
        arr.Clear();
    }
    public virtual void SetTarget(Character _character)
    {
        myCharacter = _character;
    }
    #endregion

    #region Settings
    public void SetSpacing(float _spacing)
    {
        spacing = _spacing;
        this.transform.position = Vector3.left * arrCount / 2 * spacing;
        for (int i = arr.Count - 1; i >= 0; i--)
        {
            arr[i].elementObj.transform.position  = this.transform.position + Vector3.right * spacing * i;
        }
    }
    public void Randomize()
    {
        DiscardSort();
        InitializeSort(arrCount);
    }
    public void SetCount(int _count)
    {
        arrCount = _count;
    }
    public void SetSpeed(float _val)
    {
        SortSpeed = _val;
    }
    #endregion
}
