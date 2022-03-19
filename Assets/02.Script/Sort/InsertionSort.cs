using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSort : CustomSort
{
    SortElement temp;
    int index = -1;

    public override void Sort()
    {
        base.Sort();
        StartCoroutine("SortCoroutine");
    }
    IEnumerator SortCoroutine()
    {
        for (int i = 0; i < arr.Count-1; i++)
        {
            index = i;

            // Index 요소 Highlight
            arr[index].elementObj.GetComponentInChildren<MeshRenderer>().material.color =Color.green;
            yield return new WaitForSeconds(SortSpeed);

            while (index >= 0 && arr[index].val > arr[index+1].val)
            {
                // arr에서의 위치 바꿔주기
                temp = arr[index];
                arr[index] = arr[index+1];
                arr[index+1] = temp;

                // 시각화를 위해 실제 오브젝트 위치 바꿔주기
                Vector3 tmpPos = arr[index].elementObj.transform.position;
                arr[index].elementObj.transform.position = arr[index+1].elementObj.transform.position;
                arr[index+1].elementObj.transform.position = tmpPos;

                // 탐색중 요소 Highlight
                arr[index].elementObj.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                arr[index+1].PlayAudio();

                if (myCharacter)
                    myCharacter.transform.position = arr[index].elementObj.transform.position + Vector3.forward * -3f;

                index--;

                yield return new WaitForSeconds(SortSpeed);
            }

            // 정렬한 요소 Highlight
            if (myCharacter)
                myCharacter.transform.position = arr[i].elementObj.transform.position + Vector3.forward * -3f;
            for (int j = 0; j <= i; j++)
            {
                arr[j].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[j].val;
            }
            yield return new WaitForSeconds(SortSpeed);
        }


        // 정렬한 요소 Highlight
        for (int j = arr.Count - 2; j < arr.Count; j++)
            arr[j].elementObj.GetComponentInChildren<MeshRenderer>().material.color = unitColor * arr[j].val;
        SortEnd();
    }
}
