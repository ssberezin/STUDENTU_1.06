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
        public ObservableCollection<WorkType> WorkTypesRecords { get; set; }

        private WorkType workType;
        public WorkType WorkType
        {
            get { return workType; }
            set
            {
                if (workType != value)
                {
                    workType = value;
                    OnPropertyChanged(nameof(WorkType));
                }
            }
        }

        //load data array from "WorkTypes" table
        private void LoadWorkTypesData()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var list = db.WorkTypes.ToList<WorkType>();
                    foreach (var item in list)
                    {
                        WorkTypesRecords.Add(
                        new WorkType
                        {
                            WorkTypeId = item.WorkTypeId,
                            TypeOfWork = item.TypeOfWork
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

        //=====================Commands for call Editing wondows ======================================

        private RelayCommand newEditWorkType;
        public RelayCommand NewEditWorkType => newEditWorkType ?? (newEditWorkType = new RelayCommand(
                    (obj) =>
                    {
                        EditWorkTypeWindow уditWorkTypeWindow = new EditWorkTypeWindow(obj);
                        уditWorkTypeWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(уditWorkTypeWindow);
                    }
                    ));

        //==================Commands for edit WORKTYPES =========================================


        private RelayCommand addWorkTypeCommand;
        public RelayCommand AddWorkTypeCommand => addWorkTypeCommand ?? (addWorkTypeCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddDirSubjWorkType("WorkType");
                    }
                    ));

        private RelayCommand deleteWorkTypeCommand;
        public RelayCommand DeleteWorkTypeCommand => deleteWorkTypeCommand ??
            (deleteWorkTypeCommand = new RelayCommand((selectedItem) =>
            {
                if (selectedItem == null) return;
                DeleteDirSubjWorkType("WorkType");
            }
            ));

        private RelayCommand editWorkTypeCommand;
        public RelayCommand EditWorkTypeCommand => editWorkTypeCommand ?? (editWorkTypeCommand = new RelayCommand(
                    (selectedItem) =>
                    {
                        if (selectedItem == null) return;
                        EditDirSubjWorkType("WorkType");
                    }
                    ));
    }
}
