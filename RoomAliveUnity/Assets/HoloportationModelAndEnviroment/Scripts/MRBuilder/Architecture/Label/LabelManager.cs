using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRBuilder.Architecture.label
{
    public class LabelManager : MonoBehaviour
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        public class Factory
        {
            private const string _labelTag = "_TAG_LABELS";

            public LabelManager Create(Building building)
            {
                Transform labelsContainer = null;
                FindLabelsContainer(building.transform, ref labelsContainer);

                LabelManager labelManager = null;

                if (labelsContainer)
                {
                    labelsContainer.name = "Labels";
                    labelManager = labelsContainer.gameObject.AddComponent<LabelManager>();
                }
                else
                {
                    labelsContainer = new GameObject("Labels").transform;
                    labelManager = labelsContainer.gameObject.AddComponent<LabelManager>();
                }

                labelManager._building = building;

                labelsContainer.SetParent(building.Model.transform, true);

                foreach (Transform label in labelsContainer)
                {
                    if (label.name[0] == '_')
                    {
                        label.name = label.name.Substring(1);
                    }

                    labelManager._labels.Add(label);
                }

                return labelManager;
            }

            private void FindLabelsContainer(Transform parent, ref Transform viewPointsContainer)
            {
                if (parent.name.Contains(_labelTag))
                {
                    viewPointsContainer = parent;

                    return;
                }

                foreach (Transform child in parent)
                {
                    FindLabelsContainer(child, ref viewPointsContainer);

                    if (viewPointsContainer)
                    {
                        return;
                    }
                }
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private Building _building;

        private List<Transform> _labels = new List<Transform>();
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        public Transform GetLabelWithName(string name)
        {
            return _labels.Find((x) => { return x.name == name; });
        }
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}