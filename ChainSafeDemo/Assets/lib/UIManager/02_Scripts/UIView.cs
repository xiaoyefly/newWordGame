using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PartySystems.UIParty
{
    public abstract class UIBase : MonoBehaviour
    {
        [SerializeField]
        private BaseUITransitionAnimation m_transitioner;

        private Action<UIBase> m_onFinishClosing;
        private Action<UIBase> m_onFinishShowing;

        public void ShowView()
        {
            gameObject.SetActive(true);
            OnStartShowing();

            if (m_transitioner != null)
            {
                m_transitioner.PlayIntroAnimation((interrupted) =>
                {
                    OnShown();
                    m_onFinishShowing?.Invoke(this);
                });
            }
            else
            {
                OnShown();
                m_onFinishShowing?.Invoke(this);
            }

        }

        public void CloseView()
        {
            OnStarClosing();

            if (m_transitioner != null)
            {
                m_transitioner.PlayCloseAnimation((interrupted) =>
                {
                    OnHidden();
                    m_onFinishClosing?.Invoke(this);
                    gameObject.SetActive(false);
                });
            }
            else
            {
                OnHidden();
                m_onFinishClosing?.Invoke(this);
                gameObject.SetActive(false);
            }
        }

        public void UpdateView()
        {
            OnUpdate();
        }

        public void RegisterOnFinishClosing(Action<UIBase> onFinishClosing)
        {
            m_onFinishClosing += onFinishClosing;
        }

        public void RegisterOnFinishShowing(Action<UIBase> onFinishShowing)
        {
            m_onFinishShowing += onFinishShowing;
        }

        public void ClearViewListeners()
        {
            m_onFinishClosing = null;
            m_onFinishShowing = null;
        }

        public virtual void OnInit(){}
        protected virtual void OnUpdate() { }
        

        protected virtual void OnStartShowing() { }

        protected virtual void OnShown() { }

        protected virtual void OnStarClosing() { }

        protected virtual void OnHidden() { }

    }

}

