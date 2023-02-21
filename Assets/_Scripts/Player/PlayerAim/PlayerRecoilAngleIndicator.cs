using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecoilAngleIndicator : MonoBehaviour
{
    [Header("Indicator Stuff")]
    [SerializeField] private Transform leftIndicator;
    [SerializeField] private Transform rightIndicator;
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    [SerializeField] private Transform virtualMouse;

    [SerializeField] private PlayerWeaponsController weaponsController;

    private void FixedUpdate()
    {
        IndicateRecoil(weaponsController.GetTotalRecoilAngle());
    }

    public void IndicateRecoil(float recoilAngle)
    {
        //Recoil Angle
        leftPivot.eulerAngles = new Vector3(leftPivot.eulerAngles.x, leftPivot.eulerAngles.y, recoilAngle / 2);
        rightPivot.eulerAngles = new Vector3(rightPivot.eulerAngles.x, rightPivot.eulerAngles.y, -recoilAngle / 2);

        //Indicator Bars Position
        leftIndicator.eulerAngles = new Vector3(leftIndicator.eulerAngles.x, leftIndicator.eulerAngles.y, transform.parent.eulerAngles.z);
        rightIndicator.eulerAngles = new Vector3(rightIndicator.eulerAngles.x, rightIndicator.eulerAngles.y, transform.parent.eulerAngles.z);

        float distance = Vector3.Distance(transform.position, virtualMouse.position);

        leftIndicator.localPosition = leftIndicator.right * distance;
        rightIndicator.localPosition = rightIndicator.right * distance;
    }
}
