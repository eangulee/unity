using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

//长按按钮
[AddComponentMenu("UI/LongClickButton")]
public class LongClickButton : Button
{
    [Serializable]
    public class LongClickEvent : UnityEvent { }

    [FormerlySerializedAs("onLongClick"), SerializeField]
    private LongClickEvent m_onLongClick = new LongClickEvent();
    public LongClickEvent onLongClick
    {
        get { return m_onLongClick; }
        set { m_onLongClick = value; }
    }

    private DateTime m_firstTime = default(DateTime);
    private DateTime m_secondTime = default(DateTime);

    [FormerlySerializedAs("m_singleClickable"), SerializeField]
    public bool m_singleClickable = false;

    private void Press()
    {
        if (null != onLongClick)
            onLongClick.Invoke();
        resetTime();
    }

    private void SingleClick()
    {
        if (null != onClick)
            onClick.Invoke();
        resetTime();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (m_firstTime.Equals(default(DateTime)))
            m_firstTime = DateTime.Now;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        // 在鼠标抬起的时候进行事件触发，时差大于600ms触发
        if (!m_firstTime.Equals(default(DateTime)))
            m_secondTime = DateTime.Now;
        if (!m_firstTime.Equals(default(DateTime)) && !m_secondTime.Equals(default(DateTime)))
        {
            var intervalTime = m_secondTime - m_firstTime;
            int milliSeconds = intervalTime.Seconds * 1000 + intervalTime.Milliseconds;
            if (milliSeconds > 600)
                Press();
            else
            {
                if (m_singleClickable)
                {
                    SingleClick();
                }
                else
                    resetTime();
            }
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        resetTime();
    }

    private void resetTime()
    {
        m_firstTime = default(DateTime);
        m_secondTime = default(DateTime);
    }
}