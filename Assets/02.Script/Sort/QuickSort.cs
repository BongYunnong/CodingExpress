using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSort : CustomSort
{
    public override void Sort()
    {
        base.Sort();
        StartCoroutine("SortCoroutine");
    }

    IEnumerator SortCoroutine()
    {
        yield return StartCoroutine(QuickSortCoroutine(0,arr.Count-1));
        SortEnd();
    }

    IEnumerator QuickSortCoroutine(int start, int end)
    {
        if (start >= end)
        {
            yield return new WaitForSeconds(SortSpeed);
        }
        else
        {
            int key = start;
            int i = start + 1;
            int j = end;
            SortElement temp;
            while (i <= j)
            {
                // 왼쪽에서부터 오른쪽으로 key보다 큰 값 찾기
                while (i <= end && arr[i].val <= arr[key].val)
                {
                    // 탐색중 요소 Highlight
                    if (myCharacter)
                        myCharacter.transform.position = arr[i].elementObj.transform.position + Vector3.forward * -3f;
                    arr[i].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                    arr[i].PlayAudio();
                    if (i - 1 >=0)
                        arr[i - 1].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[i - 1].val;
                    yield return new WaitForSeconds(SortSpeed);

                    i++;
                }
                // 오른쪽에서부터 왼쪽으로 key보다 작은 값 찾기
                while (j>start&&arr[j].val >= arr[key].val)
                {
                    // 탐색중 요소 Highlight
                    if (myCharacter)
                        myCharacter.transform.position = arr[j].elementObj.transform.position + Vector3.forward * -3f;
                    arr[j].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                    arr[j].PlayAudio();
                    if (j+1 <arr.Count)
                        arr[j+1].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[j+1].val;
                    yield return new WaitForSeconds(SortSpeed);

                    j--;
                }

                // index 요소 Highlight
                if (myCharacter)
                    myCharacter.transform.position = arr[Mathf.Min(i, arr.Count - 1)].elementObj.transform.position + Vector3.forward * -3f;
                arr[Mathf.Min(i, arr.Count - 1)].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.green;

                // index 요소 Highlight
                if (myCharacter)
                    myCharacter.transform.position = arr[Mathf.Max(j, 0)].elementObj.transform.position + Vector3.forward * -3f;
                arr[Mathf.Max(j, 0)].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.green;

                yield return new WaitForSeconds(SortSpeed);
                if (i > j)
                {
                    // 만약 엇갈리면

                    // 시각화를 위해 실제 오브젝트 위치 바꿔주기
                    Vector3 tmpPos = arr[j].elementObj.transform.position;
                    arr[j].elementObj.transform.position = arr[key].elementObj.transform.position;
                    arr[key].elementObj.transform.position = tmpPos;

                    // key와 j(오른쪽에서부터 찾은 작은 값) 위치 바꾸기
                    temp = arr[j];
                    arr[j] = arr[key];
                    arr[key] = temp;
                }
                else
                {
                    // 만약 엇갈리지 않았다면

                    // 시각화를 위해 실제 오브젝트 위치 바꿔주기
                    Vector3 tmpPos = arr[i].elementObj.transform.position;
                    arr[i].elementObj.transform.position = arr[j].elementObj.transform.position;
                    arr[j].elementObj.transform.position = tmpPos;

                    // i와 j 위치 바꾸기
                    temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }


                // 정렬한 요소 Highlight
                if (myCharacter)
                    myCharacter.transform.position = arr[key].elementObj.transform.position + Vector3.forward * -3f;
                arr[Mathf.Min(i, arr.Count - 1)].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[Mathf.Min(i, arr.Count - 1)].val;
                arr[Mathf.Max(j, 0)].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[Mathf.Max(j, 0)].val;
                arr[key].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[key].val;

                yield return new WaitForSeconds(SortSpeed);
            }
            // j는 이미 정렬되어있기에 start부터 j-1까지 정렬
            yield return StartCoroutine(QuickSortCoroutine(start, j - 1));
            // j는 이미 정렬되어있기에 j+1부터 end까지 정렬
            yield return StartCoroutine(QuickSortCoroutine(j+1,end));
        }
    }
}
