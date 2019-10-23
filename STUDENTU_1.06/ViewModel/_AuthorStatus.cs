using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.DBModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STUDENTU_1._06.ViewModel
{
    public class _AuthorStatus : Helpes.ObservableObject
    {

        public _AuthorStatus()
        {
            AuthorStatus = new AuthorStatus();
            AuthorStatusRecords = new ObservableCollection<AuthorStatus>();
            LoadData();
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
        }

        IDialogService dialogService;
        IShowWindowService showWindow;

        public ObservableCollection<AuthorStatus> AuthorStatusRecords { get; set; }

        private AuthorStatus authorStatus;
        public AuthorStatus AuthorStatus
        {
            get { return authorStatus; }
            set
            {
                if (authorStatus != value)
                {
                    authorStatus = value;
                    OnPropertyChanged(nameof(AuthorStatus));
                }
            }
        }

        //load data array from "Statuses" data
        private void LoadData()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var list = db.AuthorStatuses.ToList();
                    foreach (var item in list)
                    {
                        AuthorStatusRecords.Add(
                        new AuthorStatus
                        {
                            AuthorStatusId = item.AuthorStatusId,
                            AuthorStatusName = item.AuthorStatusName
                        });
                    }
                    AuthorStatus = AuthorStatusRecords[0];
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

        private RelayCommand newEditAuthorStatusCommand;
        public RelayCommand NewEditAuthorStatusCommand => newEditAuthorStatusCommand ?? (newEditAuthorStatusCommand = new RelayCommand(
                    (obj) =>
                    {
                        EditAuthorStatusWindow editStatus = new EditAuthorStatusWindow(obj);
                        editStatus.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(editStatus);
                    }
                    ));

        //==================Commands for edit STATUSES =========================================

        private RelayCommand addStatusCommand;
        public RelayCommand AddStatusCommand => addStatusCommand ?? (addStatusCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddAuthorStatus();
                    }
                    ));

        private RelayCommand deleteStatusCommand;
        public RelayCommand DeleteStatusCommand => deleteStatusCommand ??
            (deleteStatusCommand = new RelayCommand((selectedItem) =>
            {
                if (selectedItem == null) return;
                DeleteAuthorStatus();
            }
            ));

        private RelayCommand editStatusCommand;
        public RelayCommand EditStatusCommand => editStatusCommand ?? (editStatusCommand = new RelayCommand(
                    (selectedItem) =>
                    {
                        if (selectedItem == null) return;
                        EditAuthorStatus();//can find it in ForEditOrder.cs
                    }
                    ));

        //===================THIS METHOD IS FOR DELETE RECORDS FROM STATUS TABLES==============
        public void DeleteAuthorStatus()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //var res = db.Orderlines;
                    //if (Status.StatusId != 1)
                    //{
                    //    if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                    //    {
                    //        //changing DB
                    //        //we find all the records in which we have the desired Id and make a replacement
                    //        foreach (OrderLine order in res)
                    //        {
                    //            if (order.Status.StatusId == Status.StatusId)
                    //                order.Status = db.Statuses.Find(new Status() { StatusId = 1 }.StatusId);
                    //        }
                    //        db.Statuses.Remove(db.Statuses.Find(Status.StatusId));
                    //        db.SaveChanges();
                    //        //changing collection
                    //        StatusRecords.Remove(Status);
                    //    }
                    //}
                    //else
                    //    dialogService.ShowMessage("Нельзя удалить эту запись");

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

        //===================THIS METHOD IS FOR ADD RECORDS IN STATUS TABLES==============
        public void AddAuthorStatus()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //var res4 = db.Statuses.Any(o => o.StatusName == Status.StatusName);
                    //if (!res4)
                    //{
                    //    if (!string.IsNullOrEmpty(Status.StatusName))
                    //    {
                    //        Status.StatusName = Status.StatusName.ToLower();
                    //        db.Statuses.Add(Status);
                    //        db.SaveChanges();
                    //        StatusRecords.Clear();
                    //        LoadStatusData();
                    //        Status = new Status();

                    //    }
                    //    else
                    //        return;
                    //}
                    //else
                    //    dialogService.ShowMessage("Уже есть такое название в базе данных");
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

        //===================THIS METHOD IS FOR EDIT RECORDS IN STATUS TABLES==============
        public void EditAuthorStatus()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //var res4 = db.Statuses.Find(Status.StatusId);
                    //if (res4 != null)
                    //{
                    //    //changing DB
                    //    res4.StatusName = Status.StatusName.ToLower();
                    //    db.SaveChanges();
                    //}
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


    }
}