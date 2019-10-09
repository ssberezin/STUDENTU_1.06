using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using STUDENTU_1._06.Model.HelpModelClasses;
using System.Collections.ObjectModel;

namespace STUDENTU_1._06.ViewModel
{
    public partial class ForEditOrder : Helpes.ObservableObject
    {
        public ObservableCollection<Direction> DirRecords { get; set; }

        private Direction dir;
        public Direction Dir
        {
            get { return dir; }
            set
            {
                if (dir != value)
                {
                    dir = value;
                    OnPropertyChanged(nameof(Dir));
                }
            }
        }

        //load data array from "Directions" table
        private void LoadDirectionsData()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var list = db.Directions.ToList<Direction>();
                    foreach (var item in list)
                    {
                        DirRecords.Add(
                        new Direction
                        {
                            DirectionId = item.DirectionId,
                            DirectionName = item.DirectionName
                        });
                    }
                }
                catch (ArgumentNullException ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
                catch (OverflowException ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
                catch (System.Data.Entity.Core.EntityException ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
            }
        }

        //=====================Command for call Editing wondows ======================================
        private RelayCommand newEditDirection;
        public RelayCommand NewEditDirection => newEditDirection ?? (newEditDirection = new RelayCommand(
                    (obj) =>
                    {
                        EditDirection editDirection = new EditDirection(obj);
                        editDirection.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(editDirection);
                    }
                    ));

        //==================Commands for edit DIRECTIONS =========================================

        private RelayCommand addDirectionCommand;
        public RelayCommand AddDirectionCommand => addDirectionCommand ?? (addDirectionCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddDirSubjWorkType("Direction");
                    }
                    ));

        private RelayCommand deleteDirectionCommand;
        public RelayCommand DeleteDirectionCommand => deleteDirectionCommand ??
            (deleteDirectionCommand = new RelayCommand((selectedItem) =>
            {
                if (selectedItem == null) return;
                DeleteDirSubjWorkType("Direction");
            }
                    ));

        private RelayCommand editDirectionCommand;
        public RelayCommand EditDirectionCommand => editDirectionCommand ?? (editDirectionCommand = new RelayCommand(
                    (selectedItem) =>
                    {
                        if (selectedItem == null) return;
                        EditDirSubjWorkType("Direction");
                    }
                    ));

    }
}
