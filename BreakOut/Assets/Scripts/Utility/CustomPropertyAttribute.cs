using UnityEngine;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BreakoutProject
{
    namespace Utility
    {
        /// <Summary>
        /// �g���v���p�e�B
        /// </Summary>
        public class ReadOnlyAttribute : PropertyAttribute
        {

        }

        /// <Summary>
        /// �g���v���p�e�BUI�Ή�
        /// </Summary>
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
        public class ReadOnlyDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(_position, _property, _label);
                EditorGUI.EndDisabledGroup();
            }
        }
#endif
    }
}
