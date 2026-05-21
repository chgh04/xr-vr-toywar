using UnityEngine;
using UnityEngine.UI;

public class HPUIController : MonoBehaviour
{
    public Canvas hpCanvas;
    public Image hpSlider;
    public Text hpText;

    private void OnEnable()
    {
        setHP(1);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (gameObject == null) return;

        hpCanvas.transform.LookAt(Camera.main.transform.position);
        hpCanvas.transform.Rotate(0, 180, 0);
    }

    public void setHP(float  ratio)
    {
        hpSlider.fillAmount = ratio;
        hpText.text = gameObject.GetComponent<HitPoint>().HP.ToString() + "/" + gameObject.GetComponent<HitPoint>().maxHP.ToString();
    }
}
