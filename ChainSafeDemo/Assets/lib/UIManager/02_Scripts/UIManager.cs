using System;
using System.Collections;
using System.Collections.Generic;
using PartySystems.Utils;
using UnityEngine;

namespace PartySystems.UIParty
{
    [RequireComponent(typeof(Canvas))]
    public class UIManager : MonoSingleton<UIManager>
    {
        public enum EViewPriority
        {
            LowRenderPriority = 0,
            MediumRenderPriority = 1,
            HighRenderPriority = 2,
            UltraRenderPriority = 3
        }

        [SerializeField]
        private List<UIViewLibrary> m_initialViews;        

        [SerializeField]
        private Transform m_poolingObjectsParent;

        private Canvas m_uiCanvas;

        public Canvas UICanvas
        {
            get
            {
                if (m_uiCanvas == null)
                {
                    m_uiCanvas = GetComponentInParent<Canvas>();
                }
                return m_uiCanvas;
            }
        }

        private GenericPool<UIBase> m_viewPool;

        private List<UIViewLibrary> m_activeViewLibraries = new List<UIViewLibrary>();

        private Dictionary<Type, UIBase> m_viewsMap = new Dictionary<Type, UIBase>();

        private List<NullableReference<UIBase>> m_activeViews = new List<NullableReference<UIBase>>();

        private Dictionary<EViewPriority, Transform> m_priorityParentDict = new Dictionary<EViewPriority, Transform>();

        private List<NullableReference<UIBase>> m_closingViews = new List<NullableReference<UIBase>>();

        public void InitUIManager()
        {
            m_uiCanvas = GetComponent<Canvas>();

            InitPriorityContainers();

            for (int i = 0, count = m_initialViews.Count; i < count; i++)
            {
                AddViewLibrary(m_initialViews[i]);
            }

            if(m_poolingObjectsParent == null)
            {
                GameObject poolingGO = new GameObject();
                poolingGO.transform.SetParent(transform);
                m_poolingObjectsParent = poolingGO.transform;
            }

            m_viewPool = new GenericPool<UIBase>(m_poolingObjectsParent);
        }

        public override void Init()
        {
            base.Init();
            InitUIManager();
        }

        private void InitPriorityContainers()
        {
            if(m_priorityParentDict.Count > 0)
            {
                return;
            }

            foreach( EViewPriority priority in (EViewPriority[]) Enum.GetValues(typeof(EViewPriority)))
            {
                // GameObject newPriorityObject = new GameObject(priority.ToString());
                // newPriorityObject.transform.SetParent(transform, false);
                // newPriorityObject.transform.SetSiblingIndex((int)priority);

                
                GameObject newPriorityObject = new GameObject(priority.ToString());
                newPriorityObject.transform.SetParent(transform, false);
                RectTransform rectTransform = newPriorityObject.AddComponent<RectTransform>();
                // rectTransform.sizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
                // rectTransform.anchorMin = Vector2.zero;
                // rectTransform.anchorMax = Vector2.one;
                // rectTransform.offsetMin = Vector2.zero;
                // rectTransform.offsetMax = Vector2.zero;
                RectTransform parentRect = transform.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(1920, 1080);
                RectTransform childRect = rectTransform;

                float scaleX = parentRect.sizeDelta.x / 1920f;
                float scaleY = parentRect.sizeDelta.y / 1080f;

                childRect.localScale = new Vector3(scaleX, scaleY, 1f);
                newPriorityObject.transform.SetSiblingIndex((int)priority);
                
                m_priorityParentDict.Add(priority, newPriorityObject.transform);
            }
        }

        private void AddViewLibrary(UIViewLibrary views)
        {
            m_activeViewLibraries.Add(views);
            foreach (UIBase view in views.Views)
            {
                if (!m_viewsMap.ContainsKey(view.GetType()))
                {
                    m_viewsMap.Add(view.GetType(), view);
                }
            }
        }

        private void UnloadViewLibrary(UIViewLibrary views)
        {
            for(int i=0, count=views.Views.Count; i<count; i++)
            {
                List<NullableReference<UIBase>> foundRefs = m_activeViews.FindAll((nr) => nr.Reference.GetType() == views.Views[i].GetType());
                if(foundRefs.Count > 0)
                {
                    for(int m=0, count2=foundRefs.Count; m<count2; m++)
                    {
                        foundRefs[i].Reference.CloseView();
                    }
                }

                if(m_viewsMap.ContainsKey(views.Views[i].GetType()))
                {
                    m_viewsMap.Remove(views.Views[i].GetType());
                }
            }

            m_activeViewLibraries.Remove(views);
            
        }

        private float _scaleX = 1;
        private float _scaleY = 1;
        private void Update()
        {
            if (transform != null)
            {
                RectTransform parentRect = transform.GetComponent<RectTransform>();
                float scaleX = parentRect.sizeDelta.x / 1920f;
                float scaleY = parentRect.sizeDelta.y / 1080f;

                if (scaleX != _scaleX || scaleY != _scaleY)
                {
                    _scaleX = scaleX;
                    _scaleY = scaleY;
                    foreach (var VARIABLE in m_priorityParentDict)
                    {
                        RectTransform childRect = VARIABLE.Value.GetComponent<RectTransform>();
                        childRect.localScale = new Vector3(_scaleX, _scaleY, 1f);
                    }
                }
            }
           
            for (int i = m_activeViews.Count - 1; i >= 0; i--)
            {
                m_activeViews[i].Reference.UpdateView();
                
            }

            for(int i = m_closingViews.Count - 1; i >= 0; i--)
            {
                m_closingViews[i].Reference.ClearViewListeners();
                m_viewPool.ReturnObject(m_closingViews[i].Reference);
                m_closingViews[i].NullifyRef();
                m_closingViews.RemoveAt(i);
            }
        }

        /// <summary>
        /// Get an Active UIView of type T. If it doesn't exist, it will create the view with the given priority (MediumRender if priority is null)
        /// If the View exist and a priority was given, the method will update the View priority.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public SharedNullableReference<T> GetHUD<T>(EViewPriority? newPriority = null) where T : UIBase
        {
            Type viewType = typeof(T);
            NullableReference<T> requestedView = m_activeViews.Find((x) => x.Reference.GetType() == viewType) as NullableReference<T>;
            if(requestedView != null)
            {
                if(newPriority != null)
                {
                    requestedView.Reference.transform.SetParent(m_priorityParentDict[newPriority.Value], false);
                }
                return requestedView.MakeSharedRef();
            }else
            {
                return RequestView<T>(newPriority != null ? newPriority.Value : EViewPriority.MediumRenderPriority);
            }
        }

        /// <summary>
        /// Will instantiate (from a pool) a UIView of type T (if exist) and it will return a SharedRef of the View.
        /// When the UIView is closed, the SharedRef will be automaticaly set to null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="priority">Render order for the View</param>
        /// <returns></returns>
        public SharedNullableReference<T> RequestView<T>(EViewPriority priority = EViewPriority.MediumRenderPriority) where T : UIBase
        {
            if (m_viewsMap.ContainsKey(typeof(T)))
            {
                T requestedView = m_viewPool.GetObject<T>(m_viewsMap[typeof(T)] as T);
                requestedView.transform.SetParent(m_priorityParentDict[priority], false);
                requestedView.gameObject.SetActive(false);                
                NullableReference<UIBase> newNullableRef = new NullableReference<UIBase>(requestedView);
                m_activeViews.Add(newNullableRef);
                requestedView.OnInit();
                requestedView.RegisterOnFinishClosing(OnViewClosed);
                return newNullableRef.MakeSharedRefAs<T>();
            }

            return null;
        }

        private void OnViewClosed(UIBase @base)
        {
            NullableReference<UIBase> nullableView = m_activeViews.Find((nv) => nv.Reference == @base);
            if (nullableView != null)
            {
                m_activeViews.Remove(nullableView);
                m_closingViews.Add(nullableView);
            }
        }


    }

}

