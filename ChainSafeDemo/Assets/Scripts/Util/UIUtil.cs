using System;
using UnityEngine;

namespace Util
{
    public class UIUtil
    {
        /// <summary>
        /// 通过根节点来更新一个列表
        /// 不支持多个不同类型的节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="updateFunc"></param>
        /// <param name="Count"></param>
        public static void UpdateWithTransfrom(Transform node, Action<Transform, int> updateFunc, int Count = 0, Transform customParent = null)
        {

            if (node == null)
                return;

            Transform parent = customParent ?? node.parent;

            if (parent == null)
                return;

            int childCount = customParent?.childCount ?? parent.childCount;

            if (childCount < Count)
            {
                for (int i = 0; i < Count - childCount; i++)
                    GameObject.Instantiate(node, customParent ?? node.parent, false);
            }

            childCount = parent.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var temp = parent.GetChild(i);
                if (i < Count)
                {
                    temp.gameObject.SetActive(true);
                    if (updateFunc != null)
                    {
                        updateFunc(temp, i);
                    }
                }
                else
                    temp.gameObject.SetActive(false);
            }
        }
    }
}