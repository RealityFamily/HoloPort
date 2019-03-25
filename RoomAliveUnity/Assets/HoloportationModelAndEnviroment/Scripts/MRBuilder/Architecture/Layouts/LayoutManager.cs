using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using HoloGroup.Patterns;

namespace MRBuilder.Architecture.Layouts
{
    [Serializable]
    public class LayoutManager : MonoSingleton<LayoutManager>
    {
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        public class Factory
        {
            private const string _layoutTag = "_LAYOUT_";

            public LayoutManager Create(Building building)
            {
                Dictionary<string, List<Transform>> layoutedObjects = new Dictionary<string, List<Transform>>();

                FindLayouts(building.Model.transform, layoutedObjects);

                Transform defaultLayout = new GameObject("DEFAULT").transform;
                defaultLayout.position = building.Model.transform.position;

                Transform[] modelChilds = new Transform[building.Model.transform.childCount];
                for (int i = 0; i < building.Model.transform.childCount; i++)
                {
                    modelChilds[i] = building.Model.transform.GetChild(i);
                }

                foreach (Transform modelChild in modelChilds)
                {
                    modelChild.SetParent(defaultLayout, false);
                }

                defaultLayout.SetParent(building.Model.transform, false);
                defaultLayout.localPosition = Vector3.zero;

                Transform furnitureLayout = new GameObject("FURNITURES").transform;
                furnitureLayout.SetParent(building.Model.transform, false);

                foreach (KeyValuePair<string, List<Transform>> layout in layoutedObjects)
                {
                    Transform newLayout = null;

                    if (layout.Key == "DEFAULT")
                    {
                        newLayout = defaultLayout;
                    }
                    else if (layout.Key == "FURNITURES")
                    {
                        newLayout = furnitureLayout;
                    }
                    else
                    {
                        newLayout = new GameObject(layout.Key).transform;
                        newLayout.SetParent(building.Model.transform, false);
                    }

                    foreach (Transform layoutedObject in layout.Value)
                    {
                        layoutedObject.SetParent(newLayout, true);
                    }
                }

                LayoutManager layoutManager = new GameObject("Layouts").AddComponent<LayoutManager>();

                layoutManager._building = building;

                Layout.Factory layoutFactory = new Layout.Factory();

                modelChilds = new Transform[building.Model.transform.childCount];
                for (int i = 0; i < building.Model.transform.childCount; i++)
                {
                    modelChilds[i] = building.Model.transform.GetChild(i);
                }

                foreach (Transform modelChild in modelChilds)
                {
                    Layout layout = layoutFactory.Create(modelChild.gameObject,
                        new LayoutInfo(modelChild.name, true, false, false));

                    layout.Changed.AddListener(layoutManager.Layout_Changed);

                    layout.transform.SetParent(layoutManager.transform, false);

                    layoutManager._layouts.Add(layout);
                }

                layoutManager.transform.SetParent(building.Model.transform, false);
                layoutManager.transform.localPosition = Vector3.zero;

                return layoutManager;
            }

            private void FindLayouts(Transform parent, Dictionary<string, List<Transform>> layoutedObjects)
            {
                if (parent.name.Contains(_layoutTag) && !parent.name.Contains("_MeshPart"))
                {
                    int layoutNameStartIndex = parent.name.IndexOf(_layoutTag) + _layoutTag.Length;
                    string layoutName = parent.name.Substring(layoutNameStartIndex);

                    if (!layoutedObjects.ContainsKey(layoutName))
                    {
                        layoutedObjects.Add(layoutName, new List<Transform>());
                    }

                    layoutedObjects[layoutName].Add(parent);
                }

                foreach (Transform child in parent)
                {
                    FindLayouts(child, layoutedObjects);
                }
            }
        }
        #endregion

        #region Fields
        private Building _building;

        private List<Layout> _layouts = new List<Layout>();
        #endregion

        #region Events
        public event Action<LayoutManager, string, LayoutKind, bool> LayoutChanged;
        #endregion

        #region Properties
        public int LayoutsCount { get { return _layouts.Count; } }

        public string[] LayoutsNames { get { return _layouts.Select((x) => { return x.Name; }).ToArray(); } }
        #endregion

        #region Methods
        public LayoutInfo[] GetLayoutsInfo()
        {
            return _layouts.Select((x) => { return (LayoutInfo)x; }).ToArray();
        }

        public void SwitchLayoutKind(LayoutKind layoutKind, string layoutName)
        {
            if (SharingManager.Instance.IsMaster)
                SharingManager.Instance.ServerSession.SendLayoutChanging(layoutKind, layoutName);

            Layout layout = _layouts.Find((x) => { return x.Name == layoutName; });

            if (layout == null)
            {
                Debug.LogErrorFormat("Invalid layout name \"{0}\".", layoutName);

                return;
            }

            switch (layoutKind)
            {
                case LayoutKind.Visibility:
                    layout.Visibility = !layout.Visibility;
                    break;
                case LayoutKind.Transparency:
                    layout.Transparency = !layout.Transparency;
                    break;
                case LayoutKind.Highlighting:
                    layout.Highlighted = !layout.Highlighted;
                    break;
            }
        }

        public void SetLayoutKind(LayoutKind layoutKind, bool state, string layoutName)
        {
            Layout layout = _layouts.Find((x) => { return x.Name == layoutName; });

            if (layout == null)
            {
                Debug.LogErrorFormat("Invalid layout name \"{0}\".", layoutName);

                return;
            }

            switch (layoutKind)
            {
                case LayoutKind.Visibility:
                    layout.Visibility = state;
                    break;
                case LayoutKind.Transparency:
                    layout.Transparency = state;
                    break;
                case LayoutKind.Highlighting:
                    layout.Highlighted = state;
                    break;
            }
        }

        public void SetAllLayoutsKind(LayoutKind layoutKind, bool state)
        {
            foreach (Layout layout in _layouts)
            {
                switch (layoutKind)
                {
                    case LayoutKind.Visibility:
                        layout.Visibility = state;
                        break;
                    case LayoutKind.Transparency:
                        layout.Transparency = state;
                        break;
                    case LayoutKind.Highlighting:
                        layout.Highlighted = state;
                        break;
                }
            }
        }

        public void SetAllLayoutsKind(LayoutInfo[] layoutsInfo)
        {
            foreach (LayoutInfo layoutInfo in layoutsInfo)
            {
                Layout layout = _layouts.Find((x) => { return x.Name == layoutInfo.Name; });

                if (layout == null)
                {
                    Debug.LogErrorFormat("Invalid layout name \"{0}\".", layoutInfo.Name);

                    return;
                }

                layout.Visibility = layoutInfo.Visibility;
                layout.Transparency = layoutInfo.Transparency;
                layout.Highlighted = layoutInfo.Highlighted;
            }
        }

        public void ResetLayoutKinds(string layoutName)
        {
            Layout layout = _layouts.Find((x) => { return x.Name == layoutName; });

            if (layout == null)
            {
                Debug.LogErrorFormat("Invalid layout name \"{0}\".", layoutName);

                return;
            }

            layout.ResetLayoutKinds();
        }

        public void ResetAllLayoutsKinds()
        {
            foreach (Layout layout in _layouts)
            {
                layout.ResetLayoutKinds();
            }
        }

        public void SwitchAllLayoutsKind(LayoutKind layoutKind)
        {
            if (SharingManager.Instance.IsMaster)
                SharingManager.Instance.ServerSession.SendAllLayoutChanging(layoutKind);

            bool? kindState = GetKindStateForAllLayouts(layoutKind);
            bool nextKindState = GetNextKindState(kindState);

            for (int i = 1; i < _layouts.Count; i++)
            {
                _layouts[i].SetKindState(layoutKind, nextKindState);
            }
        }

        private bool? GetKindStateForAllLayouts(LayoutKind kind)
        {
            bool[] kindStates = null;

            switch (kind)
            {
                case LayoutKind.Visibility:
                    kindStates = _layouts.Select((x) => { return x.Visibility; }).Skip(1).ToArray();
                    break;
                case LayoutKind.Transparency:
                    kindStates = _layouts.Select((x) => { return x.Transparency; }).Skip(1).ToArray();
                    break;
                case LayoutKind.Highlighting:
                    kindStates = _layouts.Select((x) => { return x.Highlighted; }).Skip(1).ToArray();
                    break;
            }

            bool layoutsKindHasTrue = false;
            bool layoutsKindHasFalse = false;

            foreach (bool kindState in kindStates)
            {
                if (kindState)
                {
                    layoutsKindHasTrue = true;
                }
                else
                {
                    layoutsKindHasFalse = true;
                }
            }

            if (layoutsKindHasTrue && layoutsKindHasFalse)
            {
                return null;
            }
            else if (layoutsKindHasTrue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool GetNextKindState(bool? state)
        {
            if (state == null)
            {
                return true;
            }

            return !state.Value;
        }

        public Layout GetLayout(string layoutName)
        {
            return _layouts.Find((x) => { return x.Name == layoutName; });
        }
        #endregion

        #region Event handlers
        public void Layout_Changed(Layout layout, LayoutKind layoutKind, bool newState)
        {
            if (LayoutChanged != null)
            {
                LayoutChanged.Invoke(this, layout.Name, layoutKind, newState);
            }
        }
        #endregion
    }
}