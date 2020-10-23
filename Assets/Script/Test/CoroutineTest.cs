using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CtTest(1,2));
        StartCoroutine("CtTest3",1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            StopCoroutine("CtTest3");            
    }

    private IEnumerator CtTest(int a,int b)
    {
        yield return new WaitForSeconds(1);
        Debug.Log(a+b);
    }
    private IEnumerator CtTest3(int a)
    {
        yield return new WaitForSeconds(3);
        Debug.Log(a);
    }

    

}
