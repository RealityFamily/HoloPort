﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace HoloGroup.Editor
{
	public class Utils 
	{
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        [MenuItem("HoloGroup/Utils/Open Persistent Folder")]
        private static void OpenPersistentFolder()
        {
            Process.Start("explorer.exe", string.Format("/n,{0}", Application.persistentDataPath.Replace('/', '\\')));
        }
		#endregion
		
		#region Indexers
		#endregion
			
		#region Events handlers
		#endregion
		#endregion
	}
}
