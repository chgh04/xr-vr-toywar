using Unity.VisualScripting;
using UnityEngine;

public class TurretDisabled : MonoBehaviour
{
    HitPoint turretHP;
    public Canvas turretBreakUICanvas;

    int originAutoHealPoint;
    float originAutoHealTime;
    bool turretBreak;

    public bool TurretBreak
    {
        get { return turretBreak; }
    }

    public void OnEnable()
    {
        turretBreak = false;
        turretBreakUICanvas.gameObject.SetActive(false);
        if (turretHP == null) turretHP = GetComponent<HitPoint>();
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        if (gameObject == null) return;

        turretBreakUICanvas.transform.LookAt(Camera.main.transform.position);
        turretBreakUICanvas.transform.Rotate(0, 180, 0);
    }

    public void TurretBreakdown()
    {
        turretBreak = true;
        turretBreakUICanvas.gameObject.SetActive(true);

        originAutoHealPoint = turretHP.autoHealingPoint;
        originAutoHealTime = turretHP.autoHealingTime;

        turretHP.autoHealingPoint *= 3;
        turretHP.autoHealingTime /= 2;
    }

    public void TurretFix()
    {
        turretBreak = false;
        turretBreakUICanvas.gameObject.SetActive(false);

        turretHP.autoHealingPoint = originAutoHealPoint;
        turretHP.autoHealingTime = originAutoHealTime;
    }
}
