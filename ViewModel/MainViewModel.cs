using GalaSoft.MvvmLight;
using Model;
using Model.Elements;
using ViewModel.UndoAndRedo;
using ViewModel.UndoAndRedo.Implementation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TrainsModel;
using System;
using Utility;
using System.Windows.Forms;
using GalaSoft.MvvmLight.CommandWpf;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        IModel iModel;
        String fileName;
        BaseElementViewModel selectedElement;
        public ObservableCollection<BaseElementViewModel> Elements { get; } = new ObservableCollection<BaseElementViewModel>();
        public ICommand addNode => new RelayCommand(AddNode);
        public ICommand addStation => new RelayCommand(AddStation);
        public ICommand AddConnectionCommand => new RelayCommand(AddConnection);

        private void AddConnection()
        {
            var station1 = Elements[0].Element as StationImpl;
            var station2 = Elements[1].Element as StationImpl;
            iModel.ConnectNodes(station1,station2);
            RefreshElements();
        }

        public ICommand SaveCommand => new RelayCommand(SaveModel);
        public ICommand SaveAsCommand => new RelayCommand(SaveModelAs);
        public ICommand LoadCommand => new RelayCommand(LoadModel);
        public ICommand AddConnectionPoint => new RelayCommand<string>(v =>
        {
            StationViewModel station = selectedElement as StationViewModel;

            if (station != null)
                station.AddConnectionPoint(v);
            else
                throw new NotImplementedException();
        });
        private UndoAndRedoImpl undoAndRedoInstance => UndoAndRedoImpl.GetUndoAndredoInstance;

        public ICommand UndoOperation => new RelayCommand(Undo, CanUndoFunction);

        public ICommand RedoOperation => new RelayCommand(Redo, CanRedoFunction);

        private bool _canUndo  = false;
        private bool _canRedo = false;

        private bool CanUndoFunction() => CanUndo;
        private bool CanRedoFunction() => CanRedo;

        public bool CanUndo { get { return _canUndo;} set { _canUndo = value; RaisePropertyChanged(); } }
        public bool CanRedo { get { return _canRedo; } set { _canRedo = value; RaisePropertyChanged(); } }

        public MainViewModel()
        {
            iModel = new ModelImpl();
            foreach (var Element in iModel.GetElements())
            {
                BaseElementViewModel viewModel = Util.CreateViewModel(Element);
                viewModel.HasBeenSelected += OnHasBeenSelected;
                Elements.Add(viewModel);
                
            
            }
        }


        public void ShowSaveDialog()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            fileName = sfd.FileName;
        }
        public void ShowLoadDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            fileName = ofd.FileName;
        }

        public void SaveModel()
        {
            if (string.IsNullOrEmpty(fileName))
                ShowSaveDialog();
            if (!string.IsNullOrEmpty(fileName))
                FileIOUtils.SaveObject(iModel,fileName);
        }
        public void SaveModelAs()
        {
            ShowSaveDialog();
            if (!string.IsNullOrEmpty(fileName))
                FileIOUtils.SaveObject(iModel, fileName);
        }
        public void LoadModel()
        {
            ShowLoadDialog();
            if (!string.IsNullOrEmpty(fileName))
            iModel = FileIOUtils.LoadObject<ModelImpl>(fileName);
            RefreshElements();
        }
        public string inputText { get; private set; }

        
        private void OnHasBeenSelected(object sender, EventArgs e)
        {
            BaseElementViewModel element = sender as BaseElementViewModel;
            if (element != null)
            {
                //This statement "cleans" the selection-marking of the prior selection, here a Station
                if (selectedElement is StationViewModel)
                {
                    ((StationImpl)selectedElement.Element).Opacity = 1;
                }

                
                selectedElement = element;

                if (element is StationViewModel)
                {
                    //Console.WriteLine(((StationImpl)element.Element).Name + ((StationImpl)element.Element).Color);
                    ((StationImpl) element.Element).Opacity = 0.5;
                    //Console.WriteLine(((StationImpl)element.Element).Name + ((StationImpl)element.Element).Color);
                }
                RefreshElements();
                // Should show the StationData usercontrol



            }
            else
                throw new NotImplementedException();
        }

        private void AddNode()
        {
            iModel.AddNode(10,10);
            RefreshElements();
        }

        private void AddStation()
        {
            String name = Microsoft.VisualBasic.Interaction.InputBox("Please enter the name of the station", "Add station", "Default", -1, -1);
            undoAndRedoInstance.AddToListAndExecute(new AddStationCommand(iModel, name, 20, 20));
            CanUndo = undoAndRedoInstance.IsUndoListPopulated;
            UndoOperation.CanExecute(CanUndo);
            ((RelayCommand) UndoOperation).RaiseCanExecuteChanged();
            RefreshElements();
        }
        private void RefreshElements()
        {
            Elements.Clear();
            foreach (var Element in iModel.GetElements())
            {
                var elementViewModel = Util.CreateViewModel(Element);
                elementViewModel.HasBeenSelected += OnHasBeenSelected;
                Elements.Add(elementViewModel);
            }
        }


        private void Undo()
        {
            CanUndo = undoAndRedoInstance.IsUndoListPopulated;
            UndoOperation.CanExecute(CanUndo);
            undoAndRedoInstance.UndoOperation();
           // CommandManager.InvalidateRequerySuggested();
            ((RelayCommand)UndoOperation).RaiseCanExecuteChanged();
            ((RelayCommand)RedoOperation).RaiseCanExecuteChanged();
            RefreshElements();


        }

        private void Redo()
        {
            CanRedo = undoAndRedoInstance.IsRedoListPopulated;
            UndoOperation.CanExecute(CanRedo);
            undoAndRedoInstance.RedoOperation();
            //CommandManager.InvalidateRequerySuggested();
            ((RelayCommand) UndoOperation).RaiseCanExecuteChanged();
            ((RelayCommand) RedoOperation).RaiseCanExecuteChanged();
            RefreshElements();
        }
    }
}