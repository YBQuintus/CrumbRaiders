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
    private bool isDown = false;

    override protected void CarryOutEvent(object sender, EventArgs e)
    {
        base.CarryOutEvent(sender, e);
        isDown = true;
    }

    override protected void CarryOutEndEvent(object sender, EventArgs e)
    {
        base.CarryOutEndEvent(sender, e);
        isDown = false;
    }

    override protected void Start()
    {
        base.Start();  
        unmovedPosition = moveModel.transform.localPosition;
        movedPosition = unmovedPosition + moveTarget;
    }

    private void FixedUpdate()
    {
        if (isDown)
        {
            moveModel.transform.localPosition = Vector3.MoveTowards(moveModel.transform.localPosition, movedPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            moveModel.transform.localPosition = Vector3.MoveTowards(moveModel.transform.localPosition, unmovedPosition, moveSpeed * Time.deltaTime);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
