using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MRBuilder.Architecture.Layouts
{
    public class Layout : MonoBehaviour
    {
        #region Classes
        public class Factory
        {
            public Layout Create(GameObject layoutObject, LayoutInfo layoutInfo)
            {
                Layout layout = layoutObject.AddComponent<Layout>();

                layout._name = layoutInfo.Name;
                layout._visibility = layoutInfo.Visibility;
                layout._transparency = layoutInfo.Transparency;
                layout._highlighted = layoutInfo.Highlighted;

                layout.FindAndSetShader();
                layout.RegisterObject(layoutObject);

                return layout;
            }
        }

        [Serializable]
        public class ChangedEvent : UnityEvent<Layout, LayoutKind, bool> { }
        #endregion

        #region Fields
        private string _name;

        private bool _visibility;

        private bool _transparency;

        private bool _highlighted;

        private Shader _transparentShader;

        private List<Renderer> _renderers = new List<Renderer>();

        private Dictionary<Renderer, Material[]> _defaultMaterials = new Dictionary<Renderer, Material[]>();

        private Dictionary<Renderer, Material[]> _transparentMaterials = new Dictionary<Renderer, Material[]>();

        private Coroutine _highlightingCoroutine;
        #endregion

        #region Events
        public ChangedEvent Changed = new ChangedEvent();
        #endregion

        #region Properties
        public string Name { get { return _name; } }

        public bool Visibility
        {
            get { return _visibility; }
            set
            {
                if (value == _visibility)
                {
                    return;
                }

                _visibility = value;

                gameObject.SetActive(_visibility);

                if (Changed != null)
                {
                    Changed.Invoke(this, LayoutKind.Visibility, _visibility);
                }
            }
        }

        public bool Transparency
        {
            get { return _transparency; }
            set
            {
                if (value == _transparency)
                {
                    return;
                }

                ForceSetTransparency(value);
            }
        }

        public bool Highlighted
        {
            get { return _highlighted; }
            set
            {
                if (value == _highlighted)
                {
                    return;
                }

                ForceSetHighlighting(value);
            }
        }
        #endregion

        #region Methods
        private void FindAndSetShader()
        {
            _transparentShader = Shader.Find("Gemeleon/Unlit/Emulation/Building/Transparent");
            //_transparentShader = Shader.Find("Unlit/Transparent");
        }

        public void SetKindState(LayoutKind kind, bool state)
        {
            switch (kind)
            {
                case LayoutKind.Visibility:
                    Visibility = state;
                    break;
                case LayoutKind.Transparency:
                    Transparency = state;
                    break;
                case LayoutKind.Highlighting:
                    Highlighted = state;
                    break;
            }
        }

        public void ResetLayoutKinds(bool resetVisibility = false)
        {
            Transparency = false;
            Highlighted = false;

            if (resetVisibility)
            {
                Visibility = true;
            }
        }

        private IEnumerator StartHighlightingRoutine()
        {
            while (_highlighted)
            {
                yield return HighlightOnceRoutine();
            }

            if (!_transparency)
            {
                foreach (Renderer renderer in _renderers)
                {
                    renderer.sharedMaterials = _defaultMaterials[renderer];
                }
            }

            ResetHighlightValueForAllMaterials();

            _highlightingCoroutine = null;
        }

        private IEnumerator HighlightOnceRoutine()
        {
            yield return null;

            float timePassed = 0f;

            while (timePassed < 1f)
            {
                timePassed += Time.deltaTime;

                SetHighlightValue(timePassed);

                yield return null;
            }

            while (timePassed > 0f)
            {
                timePassed -= Time.deltaTime;

                SetHighlightValue(timePassed);

                yield return null;
            }

            SetHighlightValue(0f);
        }

        private void SetHighlightValue(float value)
        {
            foreach (Renderer renderer in _renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    Color highlightColor = material.GetColor("_HighlightColor");
                    highlightColor.a = value;

                    material.SetColor("_HighlightColor", highlightColor);
                }
            }
        }

        private void ResetHighlightValueForAllMaterials()
        {
            foreach (Renderer renderer in _renderers)
            {
                ResetHighlightValueForMaterials(_defaultMaterials[renderer]);
                ResetHighlightValueForMaterials(_transparentMaterials[renderer]);
            }
        }

        private void ResetHighlightValueForMaterials(Material[] materials)
        {
            foreach (Material material in materials)
            {
                Color color = material.GetColor("_HighlightColor");
                color.a = 0f;

                material.SetColor("_HighlightColor", color);
            }
        }

        private void OnEnable()
        {
            if (_highlighted)
            {
                _highlightingCoroutine = StartCoroutine(StartHighlightingRoutine());
            }
        }

        private void OnDisable()
        {
            ResetHighlightValueForAllMaterials();

            _highlightingCoroutine = null;
        }

        private void OnDestroy()
        {
            ResetHighlightValueForAllMaterials();
        }

        public static explicit operator LayoutInfo(Layout layout)
        {
            return new LayoutInfo(layout.Name, layout.Visibility, layout.Transparency, layout.Highlighted);
        }

        public void RegisterObject(GameObject targetObject)
        {
            List<Renderer> renderers = new List<Renderer>(targetObject.GetComponentsInChildren<Renderer>());

            foreach (Renderer renderer in renderers)
            {
                _renderers.Add(renderer);
                _defaultMaterials.Add(renderer, renderer.sharedMaterials);

                Material[] transparentMaterials = new Material[renderer.sharedMaterials.Length];
                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    transparentMaterials[i] = new Material(_transparentShader);
                    transparentMaterials[i].CopyPropertiesFromMaterial(renderer.sharedMaterials[i]);

                    Color color = transparentMaterials[i].color;
                    color.a = 0.25f;

                    transparentMaterials[i].color = color;
                }

                _transparentMaterials.Add(renderer, transparentMaterials);

                ForceSetTransparency(_transparency);
                ForceSetHighlighting(_highlighted);
            }
        }

        public void UnregisterObject(GameObject targetObject)
        {
            List<Renderer> renderers = new List<Renderer>(targetObject.GetComponentsInChildren<Renderer>());

            foreach (Renderer renderer in renderers)
            {
                _renderers.Remove(renderer);
                _defaultMaterials.Remove(renderer);
                _transparentMaterials.Remove(renderer);
            }
        }

        private void ForceSetTransparency(bool state)
        {
            _transparency = state;

            if (_transparency)
            {
                Highlighted = false;
            }

            foreach (Renderer renderer in _renderers)
            {
                renderer.sharedMaterials = _transparency ?
                    _transparentMaterials[renderer] :
                    _defaultMaterials[renderer];
            }

            if (Changed != null)
            {
                Changed.Invoke(this, LayoutKind.Transparency, _transparency);
            }
        }

        private void ForceSetHighlighting(bool state)
        {
            _highlighted = state;

            if (_highlighted)
            {
                Transparency = false;
            }

            if (gameObject.activeInHierarchy && _highlighted && _highlightingCoroutine == null)
            {
                _highlightingCoroutine = StartCoroutine(StartHighlightingRoutine());
            }

            if (Changed != null)
            {
                Changed.Invoke(this, LayoutKind.Highlighting, _highlighted);
            }
        }

        public void SetOriginMaterialsTo(GameObject targetObject)
        {
            Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.sharedMaterials = _defaultMaterials[renderer];
            }
        }

        public void SetTransparentMaterialsTo(GameObject targetObject)
        {
            Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.sharedMaterials = _transparentMaterials[renderer];
            }
        }
        #endregion
    }
}