using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSort : CustomSort
{
    SortElement temp;
    public override void Sort()
    {
        base.Sort();
        StartCoroutine("SortCoroutine");
    }
    IEnumerator SortCoroutine()
    {
        for (int i = 0; i < arr.Count; i++)
        {
            for (int j = 0; j < arr.Count-1-i; j++)
            {
                if (arr[j].val > arr[j + 1].val)
                {
                    // Index, 탐색중 요소 Highlight
                    if (myCharacter)
                        myCharacter.transform.position = arr[j + 1].elementObj.transform.position + Vector3.forward * -3f;
                    arr[j + 1].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                    arr[j+1].PlayAudio();
                    arr[j].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                    yield return new WaitForSeconds(SortSpeed);

                    // arr에서의 위치 바꿔주기
                    temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;

                    // 시각화를 위해 실제 오브젝트 위치 바꿔주기
                    Vector3 tmpPos = arr[j].elementObj.transform.position;
                    arr[j].elementObj.transform.position = arr[j + 1].elementObj.transform.position;
                    arr[j + 1].elementObj.transform.position = tmpPos;


                    // Highlight 되돌리기
                    arr[j + 1].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[j+1].val;
                    arr[j].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[j].val;
                }

            }

            // 정렬한 요소 Highlight
            if (myCharacter)
                myCharacter.transform.position = arr[arr.Count - 1 - i].elementObj.transform.position + Vector3.forward * -3f;
            yield return new WaitForSeconds(SortSpeed);
        }
        SortEnd();
    }
}
