using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEngine.UI;

public class UIWrapTool
{

    #region 树节点
    //每个节点的node
    class UINodeTree
    {
        public Transform parent
        {
            get
            {
                return root.parent;
            }
        }
        public string ClassName
        {
            get
            {
                string className = root.name + "Wrap";
                UINodeTree pt = ParentTree;
                while (pt != null)
                {
                    className = pt.ClassName + "_" + className;
                    pt = pt.ParentTree;
                }
                return className;
            }
        }

        public Transform root;

        /// <summary>
        /// 子节点
        /// </summary>
        public List<KeyValuePair<Type, Transform>> childs = new List<KeyValuePair<Type, Transform>>();

        /// <summary>
        /// 子node
        /// </summary>
        public List<UINodeTree> childNodeTree = new List<UINodeTree>();

        /// <summary>
        /// 父树
        /// </summary>
        public UINodeTree ParentTree = null;

        public UINodeTree(Transform trans, UINodeTree parentTree)
        {
            root = trans;
            ParentTree = parentTree;
            Exec(root);
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        public void Exec(Transform trans)
        {
            if (trans.childCount == 0)
            {
                return;
            }
            for (int i = 0; i < trans.childCount; i++)
            {
                Transform child = trans.GetChild(i);
                if (child.name.StartsWith(specialNodeTag))
                {
                    //生成子节点
                    childNodeTree.Add(new UINodeTree(child, this));
                }
                else
                {
                    foreach (var kv in UIComponentTypeToName)
                    {
                        foreach (string comName in kv.Value)
                        {
                            if (child.name.ToLower().StartsWith(comName) && (comName == "go_" || child.GetComponent(kv.Key) != null))
                            {
                                childs.Add(new KeyValuePair<Type, Transform>(kv.Key, child));
                                goto next;
                            }
                        }
                    }
                next:;
                    if (child.childCount > 0)
                        Exec(child);
                }
            }
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        public void GeneratorCode()
        {
            //生成代码
            string tempCode = File.ReadAllText(Application.dataPath + "/Main/Util/UIWrap/UIWrapTemplate.txt");

            string defineCode = "";

            string assginCode = "";

            foreach (var node in childs)
            {

                defineCode += string.Format("public {0}  {1};\n", node.Key, node.Value.name);

                string getCode = node.Key.ToString().Contains("GameObject") ? ";\n" : string.Format("GetComponent<{0}>();\n", node.Key.ToString());

                Transform trans = node.Value.transform;

                while (true)
                {
                    getCode = string.Format("GetChild({0}).", trans.GetSiblingIndex()) + getCode;
                    trans = trans.parent;
                    if (trans.parent == null || trans == root)
                    {
                        getCode = string.Format("{0} = transform.{1}", node.Value.name, getCode);
                        break;
                    }
                }

                assginCode += getCode + "\n";

            }

            foreach (var node in childNodeTree)
            {
                defineCode += string.Format("public {0}  {1};\n", node.ClassName, node.root.name);
                string getCode = "";
                Transform trans = node.root.transform;

                while (true)
                {
                    getCode = string.Format("GetChild({0}).", trans.GetSiblingIndex()) + getCode;
                    trans = trans.parent;
                    if (trans.parent == null || trans == root)
                    {
                        getCode = getCode.Substring(0, getCode.Length - 1); //remove .
                        getCode = string.Format("{0} = transform.{1}.GetOrAddComponent<{2}>();", node.root.name, getCode, node.ClassName);
                        break;
                    }
                }
                assginCode += getCode + "\n";
            }

            //类名
            tempCode = tempCode.Replace("{CLASSNAME}", ClassName);
            //定义
            tempCode = tempCode.Replace("{DEFINE}", defineCode);
            //赋值
            tempCode = tempCode.Replace("{ASSGIN}", assginCode);

            //代码路径
            string codePath = string.Format("{0}/Main/UI/UIWrap/{1}.cs", Application.dataPath, ClassName);

            if (File.Exists(codePath))
                File.Delete(codePath);

            //写入
            using (FileStream fileStream = new FileStream(codePath, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                {
                    sw.Write(tempCode);
                    sw.Flush();
                    Debug.Log("Gen code Success UIName" + ClassName + "  UIWrap Path:" + codePath);
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// 子节点处理
    /// </summary>
    static string specialNodeTag = "node_";

    //类型对应组件开始名
    //带下划的前缀是为了防止一些不需要的组件 只是因为名称前缀重复 不需要导出
    //TODO: 后续要提供嵌套功能 暂时只支持单节点
    static Dictionary<Type, List<string>> UIComponentTypeToName = new Dictionary<Type, List<string>>()
        {
            { typeof(RectTransform),new List<string>(){"rt_","rect_" } },
            { typeof(Transform),new List<string>(){"tr_"} },
            { typeof(Button),new List<string>(){"btn_"} },
            { typeof(Image),new List<string>(){"img_"} },
            { typeof(Text),new List<string>(){"txt_"} },
            { typeof(ScrollRect),new List<string>(){"sc_"} },
            { typeof(Dropdown),new List<string>(){"dw_"} },
            { typeof(Toggle),new List<string>(){"tog_","to_"} },
            // { typeof(UnityEngine.UI.ExtensionsToggle),new List<string>(){"tog_","to_"} },
            { typeof(Slider),new List<string>(){"sl_"} },
            { typeof(Scrollbar),new List<string>(){"sb_"} },
            { typeof(RawImage),new List<string>(){"raw_"} },
            // { typeof(RawImage),new List<string>(){"spraw_"} },
            { typeof(ToggleGroup),new List<string>(){"tbg_"} },
            // {typeof(Toggle) ,new List<string>{"tab_" } },

            // { typeof(EventTriggerListener),new List<string>(){"et_", "btn_" } },
            { typeof(InputField),new List<string>(){"ip_"} },

            // { typeof(LoopListView),new List<string>(){"llv_"} },
            // { typeof(LoopGridView),new List<string>(){"lgv_"} },
        };


    [MenuItem("GameObject/使用当前节点生成UIWrap文件", false, 1)]
    static void UseTargetObjExportUIWrap()
    {
        foreach (var obj in Selection.objects)
        {
            Transform tar = (obj as GameObject).transform;

            UINodeTree rootNode = new UINodeTree(tar, null);

            GenWrapFile(rootNode);
        }
    }

    [MenuItem("GameObject/生成UIWrap文件", false, 1)]
    static void ExportUIWrap()
    {

        List<Transform> rootList = new List<Transform>();

        foreach (var obj in Selection.objects)
        {
            if (obj is GameObject)
            {
                Transform tar = (obj as GameObject).transform;
                Transform thisRootNode = tar;
                while (true)
                {
                    //挂在UI编辑场景的节点时会报错的问题
                    if (thisRootNode.parent == null || thisRootNode.parent.name == "Canvas (Environment)")
                        break;
                    thisRootNode = thisRootNode.parent;
                }
                if (!rootList.Contains(thisRootNode))
                    rootList.Add(thisRootNode);
            }
        }

        foreach (var trans in rootList)
        {

            UINodeTree rootNode = new UINodeTree(trans, null);

            GenWrapFile(rootNode);

        }
    }


    static void GenWrapFile(UINodeTree rootTree)
    {
        if (rootTree.childNodeTree.Count > 0)
        {
            for (int i = 0; i < rootTree.childNodeTree.Count; i++)
                GenWrapFile(rootTree.childNodeTree[i]);
        }
        rootTree.GeneratorCode();
        AssetDatabase.Refresh();
    }

}