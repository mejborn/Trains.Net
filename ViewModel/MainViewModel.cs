using GalaSoft.MvvmLight;
using Model;
using Model.Elements.Interface;
using Model.Elements.Implementation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TrainsModel;
using System;
using System.Linq;
using System.Windows;
using Utility;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IModel _model;
        private string _fileName;
        private BaseElementViewModel _selectedElement;

        public ObservableCollection<BaseElementViewModel> Elements { get; } =
            new ObservableCollection<BaseElementViewModel>();

        public ICommand AddNodeCommand => new RelayCommand(AddNode);
        public ICommand AddStationCommand => new RelayCommand(AddStation);
        public ICommand AddConnectionCommand => new RelayCommand(AddConnection);
        public ICommand DeleteElementCommand => new RelayCommand(DeleteElement);
        public ICommand SaveCommand => new RelayCommand(SaveModel);
        public ICommand SaveAsCommand => new RelayCommand(SaveModelAs);
        public ICommand LoadCommand => new RelayCommand(LoadModel);

        public ICommand AddConnectionPointCommand => new RelayCommand<string>(v =>
        {
            var station = _selectedElement as StationViewModel;

            try
            {
                station?.AddConnectionPoint(v);
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a station first", "An error has occured", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        });

        private static UndoAndRedoImpl UndoAndRedoInstance => UndoAndRedoImpl.UndoAndRedoInstance;

        public RelayCommand UndoOperation => new RelayCommand(Undo);
        public RelayCommand RedoOperation => new RelayCommand(Redo);

        private bool _canUndo;
        private bool _canRedo;

        public bool CanUndo
        {
            get { return _canUndo; }
            set
            {
                _canUndo = value;
                RaisePropertyChanged();
            }
        }

        public bool CanRedo
        {
            get { return _canRedo; }
            set
            {
                _canRedo = value;
                RaisePropertyChanged();
            }
        }

        private void AddConnection()
        {
            try
            {
                var station1 = Elements[0].Element as StationImpl;
                var station2 = Elements[1].Element as StationImpl;
                _model.ConnectNodes(station1, station2);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteElement()
        {
            var elementViewModel = _selectedElement;
            var station = elementViewModel.Element as StationImpl;
            if (station?.Connections.Any() == true)
                if (MessageBox.Show(
                        "This station has connections. Are you sure?",
                        "Warning",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            _model.DeleteObject(elementViewModel.Element);
            Elements.Remove(elementViewModel);
            UndoAndRedoInstance.AddUndoItem(elementViewModel, UndoAndRedoImpl.Actions.Remove, "");
        }

        public void ShowSaveDialog()
        {
            var sfd = new SaveFileDialog();
            sfd.ShowDialog();
            _fileName = sfd.FileName;
        }

        public void ShowLoadDialog()
        {
            var ofd = new OpenFileDialog();
            ofd.ShowDialog();
            _fileName = ofd.FileName;
        }

        public void SaveModel()
        {
            if (string.IsNullOrEmpty(_fileName))
                ShowSaveDialog();
            if (!string.IsNullOrEmpty(_fileName))
                FileIOUtils.SaveObject(_model, _fileName);
        }

        public void SaveModelAs()
        {
            ShowSaveDialog();
            if (!string.IsNullOrEmpty(_fileName))
                FileIOUtils.SaveObject(_model, _fileName);
        }

        public void LoadModel()
        {
            ShowLoadDialog();
            if (!string.IsNullOrEmpty(_fileName))
                _model = FileIOUtils.LoadObject<ModelImpl>(_fileName);
            RefreshElements();
        }

        public string InputText { get; private set; }

        public MainViewModel()
        {
            _model = new ModelImpl();
        }

        private void OnHasBeenSelected(object sender, EventArgs e)
        {
            var element = sender as BaseElementViewModel;
            if (_selectedElement != null)
                _selectedElement.Opacity = 1;

            if (element != null)
                element.Opacity = 0.5;

            _selectedElement = element;
        }

        private void OnHasBeenReleased(object sender, EventArgs e)
        {
            var element = sender as BaseElementViewModel;
            if (element != null &&
                Math.Abs(element.Top - element.PrevPos.Y) > 0.0001 &&
                Math.Abs(element.Left - element.PrevPos.X) > 0.0001)
            {
                UndoAndRedoInstance.AddUndoItem(element, UndoAndRedoImpl.Actions.Move, element.PrevPos);
            }
        }


        private void AddNode()
        {
            try
            {
                var node = _model.AddNode(10, 10);
                var vm = Util.CreateViewModel(node);
                vm.HasBeenReleased += OnHasBeenReleased;
                vm.HasBeenSelected += OnHasBeenSelected;
                Elements.Add(vm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void AddStation()
        {

            try
            {
                var name = Microsoft.VisualBasic.Interaction.InputBox("Please enter the name of the station",
                    "Add station", "Default", -1, -1);
                var element = _model.AddStation(name, 20, 20);
                var vm = Util.CreateViewModel(element);
                vm.HasBeenReleased += OnHasBeenReleased;
                vm.HasBeenSelected += OnHasBeenSelected;
                UndoAndRedoInstance.AddUndoItem<string>(vm, UndoAndRedoImpl.Actions.Insert, null);
                Elements.Add(vm);
                RefreshCanRedoUndo();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshCanRedoUndo()
        {
            CanUndo = UndoAndRedoInstance.CanUndo;
            CanRedo = UndoAndRedoInstance.CanRedo;
        }

        private void RefreshElements()
        {
            Elements.Clear();
            foreach (var element in _model.GetElements())
            {
                var elementViewModel = Util.CreateViewModel(element);
                elementViewModel.HasBeenSelected += OnHasBeenSelected;
                elementViewModel.HasBeenReleased += OnHasBeenReleased;
                Elements.Add(elementViewModel);
            }
            CanUndo = UndoAndRedoInstance.CanUndo;
            CanRedo = UndoAndRedoInstance.CanRedo;
        }



        private void UpdateElementPosition(BaseElementViewModel e, Point p)
        {
            var _prevPos = new Point(e.Left, e.Top);
            e.Top = p.Y;
            e.Left = p.X;
            e.PrevPos = _prevPos;
        }

        private void Undo()
        {
            var element = UndoAndRedoInstance.Undo() as IUndoRedoObject;
            var vm = element?.O as BaseElementViewModel;
            switch (element?.A)
            {
                case UndoAndRedoImpl.Actions.Insert:
                    _model.RemoveElement(vm?.Element as BaseElementImpl);
                    Elements.Remove(vm);
                    break;
                case UndoAndRedoImpl.Actions.Remove:
                    _model.AddElement(vm?.Element as BaseElementImpl);
                    Elements.Add(vm);
                    break;
                case UndoAndRedoImpl.Actions.Move:
                    var moveObject = element as UndoRedoObject<Point>;
                    if (moveObject != null)
                        foreach (var viewModel in Elements)
                        {
                            if (viewModel == vm)
                                UpdateElementPosition(viewModel, vm.PrevPos);
                        }
                    break;
                case UndoAndRedoImpl.Actions.Update:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            RefreshCanRedoUndo();
        }

        private void Redo()
        {
            var element = UndoAndRedoInstance.Redo() as IUndoRedoObject;
            var vm = element?.O as BaseElementViewModel;
            switch (element?.A)
            {
                case UndoAndRedoImpl.Actions.Insert:
                    _model.AddElement(vm?.Element as BaseElementImpl);
                    Elements.Add(vm);
                    break;
                case UndoAndRedoImpl.Actions.Remove:
                    _model.RemoveElement(vm?.Element as BaseElementImpl);
                    Elements.Remove(vm);
                    break;
                case UndoAndRedoImpl.Actions.Move:
                    var moveObject = element as UndoRedoObject<Point>;
                    if (moveObject != null)
                        foreach (var viewModel in Elements)
                        {
                            if (viewModel == vm)
                                UpdateElementPosition(viewModel, vm.PrevPos);
                        }
                    break;
                case UndoAndRedoImpl.Actions.Update:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            RefreshCanRedoUndo();
        }
    }
}