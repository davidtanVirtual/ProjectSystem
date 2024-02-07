using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingEffect : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float range;

    private Vector3 objectOriginPos;
    private Vector3 objectMovePos;

    // Start is called before the first frame update
    void Start()
    {
        objectOriginPos = transform.position;

        StartCoroutine(Bobbing());
    }

    public IEnumerator Bobbing()
    {
        float ticks = 0;

        while (true)
        {
            float index = Mathf.PingPong(ticks, 5);

            transform.position = objectOriginPos + Vector3.right * range * index;

            yield return new WaitForEndOfFrame();
            Debug.Log(index);
            ticks += speed;
        }
    }
}
