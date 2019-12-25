using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views.EditOrderWindows;

namespace STUDENTU_1._06.ViewModel
{
    public  class _Status : Helpes.ObservableObject
    {

        public _Status()
        {
            Status = new Status();
            StatusRecords = new ObservableCollection<Status>();
            LoadStatusData();
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
        }

        IDialogService dialogService;
        IShowWindowService showWindow;

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
                    Status = StatusRecords[0];
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
                        showWindow.ShowDialog(editStatus);
                    }
                    ));

        //==================Commands for edit STATUSES =========================================

        private RelayCommand addStatusCommand;
        public RelayCommand AddStatusCommand => addStatusCommand ?? (addStatusCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddStatus(obj as string);
                    }
                    ));

        private RelayCommand deleteStatusCommand;
        public RelayCommand DeleteStatusCommand => deleteStatusCommand ??
            (deleteStatusCommand = new RelayCommand((obj) =>
            {               
                DeleteStatus();
            }
            ));

        private RelayCommand editStatusCommand;
        public RelayCommand EditStatusCommand => editStatusCommand ?? (editStatusCommand = new RelayCommand(
                    (obj) =>
                    {                        
                        EditStatus(obj as string);
                    }
                    ));

        //===================THIS METHOD IS FOR DELETE RECORDS FROM STATUS TABLES==============
        public void DeleteStatus()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res = db.Orderlines;                   
                            if (Status.StatusId != 1&&!CheckRecordBeforDelete(Status))
                            {
                                if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                                {
                                    //changing DB
                                    //we find all the records in which we have the desired Id and make a replacement
                                    foreach (OrderLine order in res)
                                    {
                                        if (order.Status.StatusId == Status.StatusId)
                                            order.Status = db.Statuses.Find(new Status() { StatusId = 1 }.StatusId);
                                    }
                                    db.Statuses.Remove(db.Statuses.Find(Status.StatusId));
                                    db.SaveChanges();
                                    //changing collection
                                   StatusRecords.Remove(Status);                                   
                                }
                            }
                            else
                                dialogService.ShowMessage("Нельзя удалить эту запись");                            

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

        //check if the record has links with other tables before deleting
        private bool CheckRecordBeforDelete(Status status)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //check in Orderlines table
                    var res = db.Orderlines;
                    foreach (OrderLine item in res)
                        if (item.Status.StatusId == status.StatusId ||
                           item.Status.StatusName == status.StatusName)
                            return true;                    
                    return false;

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
            //есть подозрение, что такой подход не очень то правомерен, но пока лень с этим заморачиваться
            return false;
        }

        //===================THIS METHOD IS FOR ADD RECORDS IN STATUS TABLES==============
        public void AddStatus(string newName)
        {
            Status.StatusName = newName;
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res4 = db.Statuses.Any(o => o.StatusName == Status.StatusName);
                    if (!res4)
                    {
                        if (!string.IsNullOrEmpty(Status.StatusName))
                        {
                            Status.StatusName = Status.StatusName.ToLower();
                            Status.StatusName.Trim();
                            if (Status.StatusName[0] == ' ')
                            {
                                dialogService.ShowMessage("Нельзя добавить пустую строку");
                                return;
                            }
                            db.Statuses.Add(Status);
                            db.SaveChanges();
                            StatusRecords.Clear();
                            LoadStatusData();
                            Status = new Status();

                        }
                        else
                            return;
                    }
                    else
                        dialogService.ShowMessage("Уже есть такое название в базе данных");
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
        public void EditStatus(string newName)
        {
            if (Status.StatusName == "---")
            {
                dialogService.ShowMessage("Нельзя редактировать эту запись");
                return;
            }
            Status.StatusName = newName;
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res4 = db.Statuses.Find(Status.StatusId);
                    if (res4 != null)
                    {
                        //changing DB
                        res4.StatusName = Status.StatusName.ToLower();
                        db.SaveChanges();
                        StatusRecords.Clear();
                        LoadStatusData();
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


    }
}
