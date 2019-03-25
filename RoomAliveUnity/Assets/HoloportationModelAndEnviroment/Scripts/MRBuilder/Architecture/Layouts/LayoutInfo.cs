using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRBuilder.Architecture.Layouts
{
    public struct LayoutInfo
    {
        #region
        private string _name;

        private bool _visibility;

        private bool _transparency;

        private bool _highlighted;
        #endregion

        #region Properties
        public string Name { get { return _name; } }

        public bool Visibility { get { return _visibility; } }

        public bool Transparency { get { return _transparency; } }

        public bool Highlighted { get { return _highlighted; } }
        #endregion

        #region Methods
        public LayoutInfo(string name, bool visibility, bool transparency, bool highlighted)
        {
            _name = name;

            _visibility = visibility;
            _transparency = transparency;
            _highlighted = highlighted;
        }
        #endregion
    }
}