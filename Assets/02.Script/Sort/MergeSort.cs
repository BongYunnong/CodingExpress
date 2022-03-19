using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSort : CustomSort
{
    List<SortElement> dummyArr = new List<SortElement>();


    public override void Sort()
    {
        base.Sort();
        dummyArr.Clear();
        for (int i = 0; i < arr.Count; i++)
        {
            dummyArr.Add(null);
        }
        StartCoroutine("SortCoroutine");
    }

    IEnumerator SortCoroutine()
    {
        yield return StartCoroutine(MergeSortFunc(0,arr.Count-1));
        SortEnd();
    }

    IEnumerator MergeSortFunc(int m, int n)
    {
        if (m < n)
        {
            int middle = (m + n) / 2;
            // middle 기준으로 양쪽으로 나누어서 재귀호출
            yield return StartCoroutine(MergeSortFunc(m, middle));
            yield return StartCoroutine(MergeSortFunc(middle + 1, n));
            // 양 쪽이 알맞게 정렬되면 쪼개진 것 하나로 합치기
            yield return StartCoroutine(Merge(m, middle, n));
        }
    }

    IEnumerator Merge(int m, int middle, int n)
    {
        int i = m;
        int j = middle + 1;
        int k = m;
        while (i <= middle && j <= n)
        {
            if (arr[i].val <= arr[j].val)
            {
                dummyArr[k] = arr[i];
                i++;
            }
            else
            {
                dummyArr[k] = arr[j];
                j++;
            }

            // 탐색중 요소 Highlight
            if (myCharacter)
                myCharacter.transform.position = dummyArr[k].elementObj.transform.position + Vector3.forward * -3f;
            dummyArr[k].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            dummyArr[k].PlayAudio();
            yield return new WaitForSeconds(SortSpeed);

            k++;
        }
        if (i > middle)
        {
            for (int x = j; x <= n; x++)
            {
                dummyArr[k] = arr[x];

                // index 요소 Highlight
                if (myCharacter)
                    myCharacter.transform.position = dummyArr[k].elementObj.transform.position + Vector3.forward * -3f;
                dummyArr[k].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                yield return new WaitForSeconds(SortSpeed);

                k++;
            }
        }
        else
        {
            for (int x = i; x <= middle; x++)
            {
                dummyArr[k] = arr[x];

                // index 요소 Highlight
                if (myCharacter)
                    myCharacter.transform.position = dummyArr[k].elementObj.transform.position + Vector3.forward * -3f;
                dummyArr[k].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                yield return new WaitForSeconds(SortSpeed);

                k++;
            }
        }
        for (int x = m; x <= n; x++)
        {
            // 정렬한 요소 Highlight
            if (myCharacter)
                myCharacter.transform.position = dummyArr[x].elementObj.transform.position + Vector3.forward * -3f;
            dummyArr[x].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * dummyArr[x].val;

            // arr에서의 위치 바꿔주기
            arr[x] = dummyArr[x];
            arr[x].elementObj.transform.position = this.transform.position + Vector3.right * x * spacing;

            yield return new WaitForSeconds(SortSpeed);
        }
    }

}
