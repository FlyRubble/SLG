using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace SLG
{
    namespace UI
    {
        [CustomEditor(typeof(UIBase), true)]
        public class UIBaseInspector : Editor
        {
            /// <summary>
            /// 监视器
            /// </summary>
            public override void OnInspectorGUI()
            {
                serializedObject.Update();
                var target = serializedObject.targetObject as UIBase;
                // 脚本
                EditorGUI.BeginDisabledGroup(true);
                SerializedProperty property = serializedObject.GetIterator();
                if (property.NextVisible(true))
                {
                    EditorGUILayout.PropertyField(property, new GUIContent("Script"), true, new GUILayoutOption[0]);
                }
                EditorGUI.EndDisabledGroup();

                // 其他属性
                property = serializedObject.FindProperty("m_cache");
                EditorGUILayout.PropertyField(property);

                property = serializedObject.FindProperty("m_sibling");
                EditorGUILayout.PropertyField(property);

                if ((Sibling)property.intValue == Sibling.Custom)
                {
                    property = serializedObject.FindProperty("m_sortOrder");
                    EditorGUILayout.PropertyField(property);
                }

                if (GUILayout.Button("添加操作的UI容器"))
                {
                    target.m_list.Clear();
                    CreateContainer(target.transform, target.m_list);
                }
                property = serializedObject.FindProperty("m_list");
                Show(property);

                var list = target.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                for (int i = 0; i < list.Length; ++i)
                {
                    if (null != list[i].GetCustomAttribute(typeof(ShowInspector), true))
                    {
                        property = serializedObject.FindProperty(list[i].Name);
                        EditorGUILayout.PropertyField(property);
                    }
                }

                serializedObject.ApplyModifiedProperties();
            }

            /// <summary>
            /// 显示表对象
            /// </summary>
            /// <param name="list"></param>
            public void Show(SerializedProperty list)
            {
                EditorGUILayout.PropertyField(list);
                //EditorGUI.indentLevel += 1;
                //if (list.isExpanded)
                //{
                //    EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
                //    for (int i = 0; i < list.arraySize; i++)
                //    {
                //        EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
                //    }
                //}
                //EditorGUI.indentLevel -= 1;
            }

            /// <summary>
            /// 创建UI容器
            /// </summary>
            /// <param name="tf"></param>
            /// <param name="list"></param>
            /// <returns></returns>
            private bool CreateContainer(Transform tf, List<GameObject> list)
            {
                bool bResult = true;
                for (int i = 0; i < tf.childCount; i++)
                {
                    var child = tf.GetChild(i);
                    if (child.name.StartsWith("@"))
                    {
                        bResult = !Contain(list, child.name);
                        if (!bResult)
                        {
                            break;
                        }
                        list.Add(child.gameObject);
                    }
                    CreateContainer(child, list);
                }
                return bResult;
            }

            /// <summary>
            /// 是否包含相同对象名
            /// </summary>
            /// <param name="list"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            private bool Contain(List<GameObject> list, string name)
            {
                bool bContain = false;

                for (int i = 0; i < list.Count; ++i)
                {
                    if (list[i].name == name)
                    {
                        bContain = true;
                        break;
                    }
                }

                if (bContain)
                {
                    string tips = string.Format("UI容器里包含相同名字: '{0}'", name);
                    EditorUtility.DisplayDialog("添加操作的UI容器", tips, "知道了");
                }

                return bContain;

            }
        }
    }
}
