using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSort : CustomSort
{
    SortElement temp;
    float min;
    int index = -1;

    public override void Sort()
    {
        base.Sort();
        StartCoroutine("SortCoroutine");
    }
    IEnumerator SortCoroutine()
    {
        for (int i = 0; i < arr.Count; i++)
        {
            index = -1;
            min = Mathf.Infinity;

            // index 요소 Highlight
            if (i>0)
                arr[i-1].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
            for(int j = i; j < arr.Count; j++)
            {
                // 탐색중 요소 Highlight
                if (j > i)
                {
                    arr[j].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                    arr[j].PlayAudio();
                    arr[j - 1].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[j - 1].val;
                }

                if (myCharacter)
                    myCharacter.transform.position = arr[j].elementObj.transform.position + Vector3.forward * -3f;
                yield return new WaitForSeconds(SortSpeed);
                if (min > arr[j].val)
                {
                    min = arr[j].val;
                    index = j;
                    temp = arr[j];
                }
            }

            if (index == -1)
            {
                arr[i].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[i].val;
            }
            else
            {
                // arr에서의 위치 바꿔주기
                temp = arr[i];
                arr[i] = arr[index];
                arr[index] = temp;

                // 시각화를 위해 실제 오브젝트 위치 바꿔주기
                Vector3 tmpPos = arr[i].elementObj.transform.position;
                arr[i].elementObj.transform.position = arr[index].elementObj.transform.position;
                arr[index].elementObj.transform.position = tmpPos;


                // 정렬한 요소 Highlight
                if (myCharacter)
                    myCharacter.transform.position = arr[i].elementObj.transform.position + Vector3.forward * -3f;
                for(int k = 0; k < arr.Count; k++)
                {
                    arr[k].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[k].val;
                }
                yield return new WaitForSeconds(SortSpeed);
            }
        }
        SortEnd();
    }
}
