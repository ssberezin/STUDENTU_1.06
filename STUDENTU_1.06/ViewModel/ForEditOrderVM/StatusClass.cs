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
using STUDENTU_1._06.Views.EditOrderWindows;
using System.Collections.ObjectModel;



namespace STUDENTU_1._06.ViewModel
{
    public partial class ForEditOrder : Helpes.ObservableObject
    {
        public ObservableCollection<Status> StatusRecords { get; set; }

        private Status status;
        public Status Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        //load data array from "Statuses" data
        private void LoadStatusData()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var list = db.Statuses.ToList();
                    foreach (var item in list)
                    {
                        StatusRecords.Add(
                        new Status
                        {
                            StatusId = item.StatusId,
                            StatusName = item.StatusName
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

        //=====================Comman for call Editing window of STATUS ======================================

        private RelayCommand newEditStatusCommand;
        public RelayCommand NewEditStatusCommand => newEditStatusCommand ?? (newEditStatusCommand = new RelayCommand(
                    (obj) =>
                    {
                        EditStatusWindow editStatus = new EditStatusWindow(obj);
                        editStatus.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(editStatus);
                    }
                    ));

        //==================Commands for edit STATUSES =========================================

        private RelayCommand addStatusCommand;
        public RelayCommand AddStatusCommand => addStatusCommand ?? (addStatusCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddDirSubjWorkType("Status");
                    }
                    ));

        private RelayCommand deleteStatusCommand;
        public RelayCommand DeleteStatusCommand => deleteStatusCommand ??
            (deleteStatusCommand = new RelayCommand((selectedItem) =>
            {
                if (selectedItem == null) return;
                DeleteDirSubjWorkType("Status");
            }
                    ));

        private RelayCommand editStatusCommand;
        public RelayCommand EditStatusCommand => editStatusCommand ?? (editStatusCommand = new RelayCommand(
                    (selectedItem) =>
                    {
                        if (selectedItem == null) return;
                        EditDirSubjWorkType("Status");//can find it in ForEditOrder.cs
                    }
                    ));


    }
}
