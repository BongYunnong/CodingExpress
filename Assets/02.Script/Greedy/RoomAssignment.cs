using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAssignment : CustomGreedy
{
    public int compare(AssignmentInfo a,AssignmentInfo b)
    {
        if(a.endTime==b.endTime)
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
    [SerializeField] Transform startTR;
    [SerializeField] Transform resultTR;
    [SerializeField] GameObject Fence;

    [SerializeField] GameObject greedyTimeline;
    [SerializeField] int N;
    [SerializeField] Vector2 timeRange;
    [SerializeField] float spacing=1f;
    List<AssignmentInfo> myList = new List<AssignmentInfo>();
    int answer = 0;

    List<GameObject> fences=new List<GameObject>();
    public override void InitializeGreedy(Character _Character)
    {
        base.InitializeGreedy(_Character);

        for (int i = 0; i < 24; i++)
        {
            GameObject fence = Instantiate(Fence, resultTR.position + Vector3.right * i, Quaternion.identity);
            fence.transform.localScale = new Vector3(0.1f, 2f, 0.1f);
            fence.gameObject.SetActive(true);
            fences.Add(fence);
        }

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

            GameObject tmpTimeline = Instantiate(greedyTimeline, startTR.position + Vector3.back * (i * spacing) + Vector3.right * randomStartTime, Quaternion.identity);
            tmpTimeline.transform.localScale = new Vector3(randomEndTime - randomStartTime, 1, 1);
            tmpTimeline.transform.localPosition += Vector3.right * tmpTimeline.transform.localScale.x / 2f;
            tmpTimeline.GetComponentInChildren<MeshRenderer>().material.color = Color.white * ((float)i / N);
            tmpTimeline.name = randomStartTime + "_" + randomEndTime;
            tmp.myTimeline = tmpTimeline;
            myCharacter.transform.position = tmpTimeline.transform.position;
            yield return new WaitForSeconds(0.1f);
        }

        initialized = true;
    }

    public override void StartGreedy()
    {
        base.StartGreedy();
        StartCoroutine("ProceedCoroutine");
    }


    IEnumerator ProceedCoroutine()
    {
        yield return new WaitForSeconds(1f);
        myList.Sort(compare);
        for (int i = 0; i < myList.Count; i++)
        {
            myList[i].myTimeline.transform.position = startTR.position+new Vector3(myList[i].startTime, 0, -(i * spacing));
            myList[i].myTimeline.transform.position += Vector3.right * myList[i].myTimeline.transform.localScale.x / 2f;
        }

        yield return new WaitForSeconds(1f);
        int currentTime = -1;
        answer = 0;
        for (int i = 0; i < myList.Count; i++)
        {
            if (myList[i].startTime >= currentTime)
            {
                currentTime = myList[i].endTime;
                myCharacter.transform.position = myList[i].myTimeline.transform.position;
                myList[i].myTimeline.GetComponentInChildren<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
                answer++;
                yield return new WaitForSeconds(0.2f);
                myList[i].myTimeline.transform.position = resultTR.position + new Vector3(myList[i].myTimeline.transform.position.x-startTR.position.x,0,0);
                myCharacter.transform.position = myList[i].myTimeline.transform.position;
                yield return new WaitForSeconds(0.2f);
            }
        }
        print(answer);
        proceeding = false;
    }

    public override void ResetGreedy()
    {
        base.ResetGreedy();
        StopCoroutine("ProceedCoroutine");
        StopCoroutine("InitializeCoroutine");
        for (int i = 0; i < myList.Count; i++)
        {
            Destroy(myList[i].myTimeline);
        }
        for(int i=0;i< fences.Count; i++)
        {
            Destroy(fences[i]);
        }
        fences.Clear();
        myList.Clear();
        answer = 0;
    }
}
