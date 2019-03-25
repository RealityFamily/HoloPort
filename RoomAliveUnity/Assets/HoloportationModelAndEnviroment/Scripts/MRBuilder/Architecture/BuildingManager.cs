using HoloGroup.Patterns;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace MRBuilder.Architecture
{
    public class BuildingManager : MonoSingleton<BuildingManager>
    {
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        [Serializable]
        private struct BuildingLimitations
        {
            [SerializeField]
            private float _minSize;

            [SerializeField]
            private float _maxSize;

            public float MinSize { get { return _minSize; } }

            public float MaxSize { get { return _maxSize; } }
        }
        #endregion

        #region Classes
        [Serializable]
        public class BuildingCreatedEvent : UnityEvent<Building> { }

        [Serializable]
        public class BuildingDestroyedEvent : UnityEvent<Building> { }
        #endregion

        #region Fields
        private Building _building;

        [SerializeField]
        private BuildingLimitations _buildingLimitations;
        #endregion

        #region Events

        public BuildingCreatedEvent BuildingCreated = new BuildingCreatedEvent();


        public BuildingDestroyedEvent BuildingDestroyed = new BuildingDestroyedEvent();
        #endregion

        #region Properties
        public Building Building { get { return _building; } }
        #endregion

        #region Methods

        public void MakeBuilding(Model model)
        {
            Building.Factory buildingFactory = new Building.Factory();

            _building = buildingFactory.MakeBuilding(model, _buildingLimitations.MinSize, _buildingLimitations.MaxSize);

            _building.transform.SetParent(transform);

            BuildingCreated.Invoke(_building);
        }

        public void DestroyCurrentBuilding()
        {
            if (_building)
            {
                BuildingDestroyed.Invoke(_building);

                Destroy(_building.gameObject);

                Resources.UnloadUnusedAssets();
            }
        }
        #endregion

        #region Event handlers
        #endregion
    }
}