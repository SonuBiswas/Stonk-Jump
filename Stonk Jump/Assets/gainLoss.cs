using TMPro;
using UnityEngine;

public class gainLoss : MonoBehaviour
{
    [SerializeField] TextMeshPro valueText;
    public bool gain;
    public float value;
    [SerializeField] Animator anim;
    void Start()
    {
        valueText.text = (gain == true) ? "+" + value.ToString()+"%" :"-" + value.ToString()+"%";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (anim != null)
            {
                anim.SetTrigger("destroy");
            }
        }
    }

}
