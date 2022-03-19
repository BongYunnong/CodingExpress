using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeapSort : CustomSort
{
    public override void Sort()
    {
        base.Sort();
        StartCoroutine("SortCoroutine");
    }
    IEnumerator SortCoroutine()
    {
        //힙 만들기
        for(int i = 1; i < arr.Count; i++)
        {
            int child = i;
            do
            {
                int root = (child - 1) / 2;
                if (arr[root].val < arr[child].val)
                {
                    // 시각화를 위해 실제 오브젝트 위치 바꿔주기
                    Vector3 tmpPos = arr[root].elementObj.transform.position;
                    arr[root].elementObj.transform.position = arr[child].elementObj.transform.position;
                    arr[child].elementObj.transform.position = tmpPos;

                    SortElement temp = arr[root];
                    arr[root] = arr[child];
                    arr[child] = temp;
                }
                child = root;
            } while (child != 0);
        }

        // 완성된 힙 정렬
        for (int i = arr.Count-1; i >=0; i--)
        {
            // 첫번째와 마지막 바꾸기
            // 시각화를 위해 실제 오브젝트 위치 바꿔주기
            Vector3 tmpPos = arr[0].elementObj.transform.position;
            arr[0].elementObj.transform.position = arr[i].elementObj.transform.position;
            arr[i].elementObj.transform.position = tmpPos;

            SortElement temp = arr[0];
            arr[0] = arr[i];
            arr[i] = temp;


            // index 요소 Highlight
            arr[i].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
            yield return new WaitForSeconds(SortSpeed);

            int root = 0;
            int child = 1;
            do
            {
                // 자식 탐색
                child = (2 * root) + 1;
                // 왼쪽(child)와 오른쪽(child+1) 자식 중 큰 값 고르기
                // child < i-1은 정렬한 것을 제외하기 위함
                if (child < i - 1 && arr[child].val < arr[child + 1].val)
                {
                    child++;
                }
                // 자식이 Root보다 크다면 교환
                if (child < i && arr[root].val < arr[child].val)
                {
                    // 탐색중 요소 Highlight
                    arr[child].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                    arr[child].PlayAudio();
                    if (myCharacter)
                        myCharacter.transform.position = arr[child].elementObj.transform.position + Vector3.forward * -3f;

                    // 시각화를 위해 실제 오브젝트 위치 바꿔주기
                    Vector3 tmpPos2 = arr[root].elementObj.transform.position;
                    arr[root].elementObj.transform.position = arr[child].elementObj.transform.position;
                    arr[child].elementObj.transform.position = tmpPos2;

                    yield return new WaitForSeconds(SortSpeed);

                    temp = arr[root];
                    arr[root] = arr[child];
                    arr[child] = temp;


                    // 탐색중 요소 Highlight
                    arr[root].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[i].val;
                }
                root = child;
            } while (child < i);
            // index 요소 Highlight
            arr[i].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[i].val;
        }
        yield return new WaitForSeconds(SortSpeed);

        SortEnd();
    }
}
