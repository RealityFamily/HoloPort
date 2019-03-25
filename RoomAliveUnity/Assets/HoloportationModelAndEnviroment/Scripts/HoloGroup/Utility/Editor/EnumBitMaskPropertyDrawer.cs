using HoloGroup.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HoloGroup.Utility.Editor
{
    [CustomPropertyDrawer(typeof(BitMaskAttribute))]
    public class EnumBitMaskPropertyDrawer : PropertyDrawer
    {
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #endregion

        #region Fields
        #endregion

        #region Events
        #endregion

        #region Properties
        #endregion

        #region Methods
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            BitMaskAttribute bitMaskAttribyte = attribute as BitMaskAttribute;
            label.text = string.Format("{0} ({1})", label.text, property.intValue);

            string[] enumNames = Enum.GetNames(bitMaskAttribyte.PropertyType);
            int[] enumValues = (int[])Enum.GetValues(bitMaskAttribyte.PropertyType);

            int value = property.intValue;
            int maskValue = 0;

            for (int i = 0; i < enumValues.Length; i++)
            {
                if (enumValues[i] != 0)
                {
                    if ((value & enumValues[i]) == enumValues[i])
                        maskValue |= 1 << i;
                }
                else if (value == 0)
                    maskValue |= 1 << i;
            }

            int newMaskValue = EditorGUI.MaskField(position, label, maskValue, enumNames);
            int changes = maskValue ^ newMaskValue;

            for (int i = 0; i < enumValues.Length; i++)
            {
                if ((changes & (1 << i)) != 0)            // has this list item changed?
                {
                    if ((newMaskValue & (1 << i)) != 0)     // has it been set?
                    {
                        if (enumValues[i] == 0)           // special case: if "0" is set, just set the val to 0
                        {
                            value = 0;
                            break;
                        }
                        else
                            value |= enumValues[i];
                    }
                    else                                  // it has been reset
                    {
                        value &= ~enumValues[i];
                    }
                }
            }

            property.intValue = value;
        }
        #endregion

        #region Event handlers
        #endregion
    }
}