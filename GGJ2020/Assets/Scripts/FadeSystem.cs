using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FadeSystem : MonoBehaviour
{
    [SerializeField] private Image Cover;
    private static Image m_Cover;
    public Color FadeToColor;
    public bool WillStartFromBlack = true;

    public static FadeSystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(Cover.transform.parent.gameObject);
            Destroy(this.gameObject);
        }
        m_Cover = Cover;
        FadeToColor = Color.black;

        if (WillStartFromBlack)
        {
            m_Cover.color = Color.black;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(.5f);
            seq.Append(Willfade(false));
            seq.PlayForward();
        }
    }

    public Tween Willfade(bool willfade)
    {
        if (willfade)
            return m_Cover.DOColor(FadeToColor, 1.0f);
        else
            return m_Cover.DOColor(Color.clear, 1.0f);
    }

    public Tween Willfade(bool willfade, Color toColor)
    {
        if (willfade)
            return m_Cover.DOColor(toColor, 1.0f);
        else
            return m_Cover.DOColor(Color.clear, 1.0f);
    }

    public void ChangeColor(Color color) { FadeToColor = color; }
}
