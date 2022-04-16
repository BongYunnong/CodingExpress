using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAssignment : CustomGreedy
{
    public int compare(AssignmentInfo a,AssignmentInfo b)
    {
        if(a.endTime.CompareTo(b.endTime) == 0)
        {
            return a.startTime.CompareTo(b.startTime);
        }
        else
        {
            return a.endTime.CompareTo(b.endTime);
        }
    }
    [System.Serializable]
    public class AssignmentInfo
    {
        public GameObject myTimeline;
        public int startTime;
        public int endTime;
    }
    [SerializeField] GameObject greedyTimeline;
    [SerializeField] int N;
    [SerializeField] Vector2 timeRange;
    [SerializeField] float spacing=1f;
    List<AssignmentInfo> myList = new List<AssignmentInfo>();
    public override void InitializeGreedy(Character _Character)
    {
        base.InitializeGreedy(_Character);
        StopCoroutine("ProceedCoroutine");
        StopCoroutine("InitializeCoroutine");
        StartCoroutine("InitializeCoroutine");
    }

    IEnumerator InitializeCoroutine()
    {
        for (int i = 0; i < N; i++)
        {
            int randomStartTime = (int)Random.Range(timeRange.x, timeRange.y);
            int randomEndTime = (int)Random.Range(randomStartTime + 1, timeRange.y);
            AssignmentInfo tmp = new AssignmentInfo();
            tmp.startTime = randomStartTime;
            tmp.endTime = randomEndTime;
            myList.Add(tmp);

            GameObject tmpTimeline = Instantiate(greedyTimeline, this.transform.position + Vector3.back * (i * spacing) + Vector3.right * randomStartTime, Quaternion.identity);
            tmpTimeline.transform.localScale = new Vector3(randomEndTime - randomStartTime, 1, 1);
            tmp.myTimeline = tmpTimeline;
            myCharacter.transform.position = tmpTimeline.transform.position;
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine("ProceedCoroutine");
    }


    IEnumerator ProceedCoroutine()
    {
        yield return new WaitForSeconds(1f);
        myList.Sort(compare);
        for (int i = 0; i < myList.Count; i++)
        {
            myList[i].myTimeline.transform.position = new Vector3(myList[i].startTime, 0, -(i * spacing));
        }

        yield return new WaitForSeconds(1f);
        int currentTime = -1;
        int count = 0;
        for (int i = 0; i < myList.Count; i++)
        {
            if (myList[i].startTime >= currentTime)
            {
                currentTime = myList[i].endTime;
                myCharacter.transform.position = myList[i].myTimeline.transform.position;
                myList[i].myTimeline.GetComponentInChildren<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
                count++;
                yield return new WaitForSeconds(0.2f);
                myList[i].myTimeline.transform.position = new Vector3(myList[i].myTimeline.transform.position.x,0,0);
                myCharacter.transform.position = myList[i].myTimeline.transform.position;
                yield return new WaitForSeconds(0.2f);
            }
        }
        print(count);
    }

}
