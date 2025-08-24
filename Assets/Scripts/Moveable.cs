using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Moveable : Listener
{
    [SerializeField] private Vector3 moveTarget;
    [SerializeField] private GameObject moveModel;
    [SerializeField] private float moveSpeed;
    private Vector3 unmovedPosition;
    private Vector3 movedPosition;

    override protected void CarryOutEvent(object sender, EventArgs e)
    {
        base.CarryOutEvent(sender, e);
        StartCoroutine(MoveToTarget());
    }

    override protected void CarryOutEndEvent(object sender, EventArgs e)
    {
        base.CarryOutEndEvent(sender, e);
        StartCoroutine(MoveFromTarget());
    }

    IEnumerator MoveToTarget()
    {
        while (moveModel.transform.localPosition != movedPosition)
        {
            moveModel.transform.localPosition = Vector3.MoveTowards(moveModel.transform.localPosition, movedPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoveFromTarget()
    {
        while (moveModel.transform.localPosition != unmovedPosition)
        {
            moveModel.transform.localPosition = Vector3.MoveTowards(moveModel.transform.localPosition, unmovedPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    override protected void Start()
    {
        base.Start();  
        unmovedPosition = moveModel.transform.localPosition;
        movedPosition = unmovedPosition + moveTarget;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
