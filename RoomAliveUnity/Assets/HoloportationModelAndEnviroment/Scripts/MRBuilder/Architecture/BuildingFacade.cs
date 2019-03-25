using HoloGroup.UserInteraction.Input.Gaze;
using HoloGroup.Patterns;
using MRBuilder.Architecture.Layouts;

using MRBuilder.UserInteraction.Input.Gesture;
using HoloGroup.Windows;

using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

using Cursor = HoloGroup.UserInteraction.Input.Gaze.Cursor;


namespace MRBuilder.Architecture
{
    public class BuildingFacade : MonoSingleton<BuildingFacade>
    {
        #region Enums
        private enum PlacingType
        {
            New,
            Replace
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        public struct TransformInformation
        {
            public Vector3 Position { get; set; }

            public Vector3 LocalPosition { get; set; }

            public Quaternion Rotation { get; set; }

            public Quaternion LocalRotation { get; set; }

            public Vector3 LossyScale { get; set; }

            public Vector3 LocalScale { get; set; }

            public TransformInformation(Transform transform)
            {
                Position = transform.position;
                LocalPosition = transform.localPosition;
                Rotation = transform.rotation;
                LocalRotation = transform.localRotation;
                LossyScale = transform.lossyScale;
                LocalScale = transform.localScale;
            }
        }
        #endregion

        #region Classes
        #endregion

        #region Fields
        [SerializeField]
       // private ModelPlacer _modelPlacer;

        private PlacingType _placingType;

        private string _pathToFile;

        private LayerMask _oldLayerMask;

        private UnityEngine.XR.WSA.Input.GestureSettings _handOldGestureSettings;

        private UnityEngine.XR.WSA.Input.GestureSettings _clickerOldGestureSettings;

        private TransformInformation _transformBeforeInside;

        [SerializeField]
        [Range(0f, 4f)]
        private float _transitionDuration = 2f;

        [SerializeField]
        [Range(0f, 4f)]
        private float _defaultGrowth = 1.8f;

        [SerializeField]
        [Range(0f, 4f)]
        private float _maxHeightRaycastDistance = 4f;
        #endregion

        #region Events
        public UnityEvent BuildingPlacing = new UnityEvent();

        public UnityEvent BuildingPlaced = new UnityEvent();

        public UnityEvent FurniturePlacing = new UnityEvent();

        public UnityEvent FurniturePlaced = new UnityEvent();
        #endregion

        #region Properties
        public string PathToActiveModel
        {
            get { return _pathToFile; }
        }

        public TransformInformation TransformBeforeInside
        {
            get { return _transformBeforeInside; }
        }
        #endregion

        #region Methods
        private Model CreateModel(string includedBaseFolder)
        {
            string includedTag = @"INCLUDED:\";

            Transform modelTransform = null;

            Model.Factory modelFactory = new Model.Factory();
            Model model = null;

                string modelName = _pathToFile.Substring(includedTag.Length);

                Transform modelPrefab = Resources.Load<Transform>(string.Format("{0}/{1}/{1}", includedBaseFolder, modelName));
                modelTransform = Instantiate(modelPrefab);
                modelTransform.name = modelName;

                model = modelFactory.MakeModel(modelTransform, modelName, true);


           // modelTransform.position = _modelPlacer.TargetLastPosition;

            return model;
        }

        #region Placing
        public void StartPlacingNewBuilding(string pathToBuilding)
        {
            //if (_modelPlacer.IsPlacing)
            //{
            //    return;
            //}

            //_placingType = PlacingType.New;

            //_pathToFile = pathToBuilding;

            //BuildingManager.Instance.DestroyCurrentBuilding();

            //_modelPlacer.ShowHint = true;
            //StartPlacing(EndPlacingNewBuilding, LayerMask.GetMask("SpatialMesh"));

            //BuildingPlacing.Invoke();
        }

        private IEnumerator WaitOneFrameAndStartPlacingNewBuildingRoutine(string pathToBuilding)
        {
            yield return null;

            StartPlacingNewBuilding(pathToBuilding);
        }

        private void EndPlacingNewBuilding(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int count, Ray headRay)
        {
            //if (!_modelPlacer.IsPlacing || _placingType != PlacingType.New)
            //{
            //    return;
            //}

            EndPlacingWithRestoreLayerMask(EndPlacingNewBuilding);

            CreateBuilding();
        }

        private void CreateBuilding()
        {
            //Model model = CreateModel(MRBuilderSettings.PathToIncludedBuildings);
            //BuildingManager.Instance.MakeBuilding(model);

            BuildingPlaced.Invoke();
        }
        #endregion

        #region Replacing
        public void StartReplacingBuilding()
        {
            Building building = BuildingManager.Instance.Building;

            //if (_modelPlacer.IsPlacing || !BuildingManager.Instance.Building)
            //{
            //    return;
            //}

            _placingType = PlacingType.Replace;

            building.gameObject.SetActive(false);

            //_modelPlacer.ShowHint = false;
            StartPlacing(EndReplacingBuilding, LayerMask.GetMask("SpatialMesh"));

            BuildingPlacing.Invoke();
        }

        public void WaitOneFrameAndStartReplacingBuilding()
        {
            StartCoroutine(WaitOneFrameAndStartReplacingBuildingRoutine());
        }

        private IEnumerator WaitOneFrameAndStartReplacingBuildingRoutine()
        {
            yield return null;

            StartReplacingBuilding();
        }

        private void EndReplacingBuilding(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int count, Ray headRay)
        {
            //if (!_modelPlacer.IsPlacing || _placingType != PlacingType.Replace)
            //{
            //    return;
            //}

            Building building = BuildingManager.Instance.Building;
            building.ResetState();

            EndPlacingWithRestoreLayerMask(EndReplacingBuilding);

            //building.transform.position = _modelPlacer.TargetLastPosition;

            BuildingManager.Instance.Building.gameObject.SetActive(true);

            BuildingPlaced.Invoke();
        }
        #endregion

        #region Placing Furniture
        private IEnumerator WaitOneFrameAndStartPlacingNewFurniture(string pathToFurniture)
        {
            yield return null;

            StartPlacingNewFurniture(pathToFurniture);
        }

        public void StartPlacingNewFurniture(string pathToFile)
        {
            _placingType = PlacingType.New;

            _pathToFile = pathToFile;

            //_modelPlacer.ShowHint = false;
            StartPlacing(EndPlacingNewFurniture);

            FurniturePlacing.Invoke();
        }

        private void EndPlacingNewFurniture(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int count, Ray headRay)
        {
            //if (!_modelPlacer.IsPlacing || _placingType != PlacingType.New)
            //{
            //    return;
            //}

            EndPlacing(EndPlacingNewFurniture);

            CreateFurniture();
        }

        private void CreateFurniture()
        {
            //string furnitureName = Path.GetFileNameWithoutExtension(_pathToFile);

            //bool isIncluded = _pathToFile.StartsWith(@"INCLUDED:\");

            //if (BuildingManager.Instance.Building.FurnitureManager.Find((x) => { return x.name == furnitureName && x.GetComponent<Model>().IsIncluded == isIncluded; }))
            //{
            //    Furniture copiedFurniture = BuildingManager.Instance.Building.FurnitureManager.CopyFurnitureWithName(furnitureName);
            //    copiedFurniture.transform.position = _modelPlacer.TargetLastPosition;
            //}
            //else
            //{
            //    Model model = CreateModel(MRBuilderSettings.PathToIncludedFurnitures);
            //    BuildingManager.Instance.Building.FurnitureManager.MakeFurniture(model, true, true);
            //}

            //FurniturePlaced.Invoke();

            //WindowsManager.Instance.OpenFurnitureFolderViewerWindow();
        }
        #endregion

        #region Common placing and end-placing methods
        private void StartPlacing(UnityAction<UnityEngine.XR.WSA.Input.InteractionSourceKind, int, Ray> tapHandler)
        {
            Cursor.Instance.IsShown = false;

            _handOldGestureSettings = GestureManager.Instance.HandGestureRecognizer.GetRecognizableGestures();
            GestureManager.Instance.HandGestureRecognizer.SetRecognizableGestures(UnityEngine.XR.WSA.Input.GestureSettings.Tap);

            //Timer.Instance.DelayInvoke(1f, () => { GestureManager.Instance.HandGestureRecognizer.Tapped.AddListener(tapHandler); });

            //_modelPlacer.StartPlacing();
        }

        private void StartPlacing(UnityAction<UnityEngine.XR.WSA.Input.InteractionSourceKind, int, Ray> tapHandler, LayerMask mask)
        {
            _oldLayerMask = GazeManager.Instance.collideMask;
            GazeManager.Instance.collideMask = mask;

            StartPlacing(tapHandler);
        }

        private void EndPlacingWithRestoreLayerMask(UnityAction<UnityEngine.XR.WSA.Input.InteractionSourceKind, int, Ray> tapHandler)
        {
            GazeManager.Instance.collideMask = _oldLayerMask;

            EndPlacing(tapHandler);
        }

        private void EndPlacing(UnityAction<UnityEngine.XR.WSA.Input.InteractionSourceKind, int, Ray> tapHandler)
        {
            //_modelPlacer.EndPlacing();

            GestureManager.Instance.HandGestureRecognizer.Tapped.RemoveListener(tapHandler);
            //GestureManager.Instance.ClickerGestureRecognizer.Tapped.RemoveListener(tapHandler);

            GestureManager.Instance.HandGestureRecognizer.SetRecognizableGestures(_handOldGestureSettings);
            //GestureManager.Instance.ClickerGestureRecognizer.SetRecognizableGestures(_clickerOldGestureSettings);

            Cursor.Instance.IsShown = true;
        }
        #endregion

        public void GoInside()
        {
            Building building = BuildingManager.Instance.Building;

            if (building.IsBusy ||
                building.CurrentVisualMode == Building.VisualMode.Inside)
            {
                return;
            }

            _transformBeforeInside = new TransformInformation(building.transform)
            {
                LossyScale = building.transform.lossyScale,
                LocalScale = building.transform.localScale
            };

            #region Calculate growth
            LayerMask spatialMeshLayer = LayerMask.GetMask("SpatialMesh");
            RaycastHit floorHitInfo;

            Ray rayToFloor = new Ray(Camera.main.transform.position, -Vector3.up);

            float growth = _defaultGrowth;

            if (Physics.Raycast(rayToFloor, out floorHitInfo, _maxHeightRaycastDistance, spatialMeshLayer))
            {
                growth = floorHitInfo.distance;
            }

            growth /= 1f / building.transform.localScale.x;

            Vector3 growthVector = new Vector3(0f, growth, 0f);
            #endregion

            LayerMask buildingLayer = LayerMask.GetMask("Building");

            RaycastHit[] hitInfos = GazeManager.Instance.RaycastAll(buildingLayer);

            bool hasHit = false;
            RaycastHit gazeHitInfo = new RaycastHit();

            foreach (RaycastHit hitInfo in hitInfos)
            {
                BuildingElement buildingElement = hitInfo.transform.GetComponent<BuildingElement>();

                if (buildingElement == null)
                {
                    continue;
                }

                if (buildingElement.Type == BuildingElement.ElementType.Floor)
                {
                    hasHit = true;
                    gazeHitInfo = hitInfo;
                    break;
                }
            }

            if (hasHit)
            {
                Vector3 insidePoint = gazeHitInfo.point + growthVector;
                building.GoInInsideMode(insidePoint, _transitionDuration);
            }
            else
            {
                if (building.DefaultInsidePoint != null)
                {
                    Vector3 insidePoint = building.DefaultInsidePoint.position + growthVector;
                    building.GoInInsideMode(insidePoint, _transitionDuration);
                }
            }
        }

        public void GoInsideToViewPoint(string viewPointName)
        {
            Building building = BuildingManager.Instance.Building;

            if (building.IsBusy || building.CurrentVisualMode == Building.VisualMode.Inside)
            {
                return;
            }

            _transformBeforeInside = new TransformInformation(building.transform);
            _transformBeforeInside.LossyScale = building.transform.lossyScale;
            _transformBeforeInside.LocalScale = building.transform.localScale;

            BuildingManager.Instance.Building.JumpToViewPoint(viewPointName, 1f);
        }

        public void GoOutside()
        {
            if (BuildingManager.Instance.Building.IsBusy || BuildingManager.Instance.Building.CurrentVisualMode == Building.VisualMode.Outside)
            {
                return;
            }

            BuildingManager.Instance.Building.GoInOutsideMode(
                _transformBeforeInside.Position,
                _transformBeforeInside.Rotation,
                _transformBeforeInside.LocalScale.x,
                _transitionDuration);

           // WindowsManager.Instance.CloseViewPointWindow();
        }

        public void PreviewViewPoints()
        {
            //BuildingManager.Instance.Building.ViewPointManager.PreviewViewPoints();
        }

        private void CancelPreviewViewPoint()
        {
            //BuildingManager.Instance.Building.ViewPointManager.CancelPreviewViewPoint();
        }

        public void JumpToViewPoint(string viewPointName)
        {
            if (BuildingManager.Instance.Building.CurrentVisualMode == Building.VisualMode.Outside)
            {
                GoInsideToViewPoint(viewPointName);
            }
            else
            {
                BuildingManager.Instance.Building.JumpToViewPoint(viewPointName, 1f);
            }
        }

        private void ClearFurnitures()
        {
            //BuildingManager.Instance.Building.FurnitureManager.RemoveAllFurnitures();
        }
        #endregion

        #region Event Handlers
        //public void BuildingFolderViewerWindow_Selected(FolderViewerWindow buildingFolderViewerWindow, string pathToSelected)
        //{
        //    StartCoroutine(WaitOneFrameAndStartPlacingNewBuildingRoutine(pathToSelected));
        //}

        //#region Layouts
        //public void LayoutWindow_LayoutKindClicked(LayoutWindow layoutWindow, string layoutName, LayoutKind layoutKind)
        //{
        //    BuildingManager.Instance.Building.LayoutManager.SwitchLayoutKind(layoutKind, layoutName);
        //}

        //public void LayoutWindow_AllLayoutsKindClicked(LayoutWindow layoutWindow, LayoutKind layoutKind)
        //{
        //    BuildingManager.Instance.Building.LayoutManager.SwitchAllLayoutsKind(layoutKind);
        //}
        #endregion

        #region View Points
        public void ViewPointWindow_Opened(Window viewPointWindow)
        {
            if (BuildingManager.Instance.Building.CurrentVisualMode == Building.VisualMode.Outside)
            {
                PreviewViewPoints();
            }
        }

        public void ViewPointWindow_Closing(Window viewPointWindow)
        {
            CancelPreviewViewPoint();
        }

        //public void ViewPointWindow_ViewPointJumpClicked(ViewPointWindow viewPointWindow, ViewPointPanel viewPointPanel)
        //{
        //    if (BuildingManager.Instance.Building.CurrentVisualMode == Building.VisualMode.Outside)
        //    {
        //        CancelPreviewViewPoint();
        //    }

        //    JumpToViewPoint(viewPointPanel.ViewPointName);
        //}
        #endregion

        //public void FurnitureFolderViewerWindow_Selected(FolderViewerWindow furnitureFolderViewerWindow, string pathToSelected)
        //{
        //    StartCoroutine(WaitOneFrameAndStartPlacingNewFurniture(pathToSelected));
        //}

        //public void FurniturePresetsFolderViewerWindow_Selected(FolderViewerWindow furnitureFolderViewerWindow, string pathToSelected)
        //{
        //    ClearFurnitures();

        //    PresetSerializer presetSerializer = new PresetSerializer();
        //    presetSerializer.DeserializeAndMakeFurnitures(Path.GetFileName(pathToSelected));
        //}

        //public void FurnitureFolderViewerWindow_ClearBuildingFurnituresPressed(FurnitureFolderViewerWindow furnitureFolderViewerWindow)
        //{
        //    ManipulatorFacade.Instance.StopManipulateFurniture();
        //    ClearFurnitures();
        //}
        //#endregion
    }
}