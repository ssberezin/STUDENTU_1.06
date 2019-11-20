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

      

        //==================Commands for edit STATUSES =========================================

        private RelayCommand addAuthorStatusCommand;
        public RelayCommand AddAuthorStatusCommand => addAuthorStatusCommand ?? (addAuthorStatusCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddAuthorStatus();
                    }
                    ));

        private RelayCommand deleteAuthorStatusCommand;
        public RelayCommand DeleteAuthorStatusCommand => deleteAuthorStatusCommand ??
            (deleteAuthorStatusCommand = new RelayCommand((selectedItem) =>
            {
                if (selectedItem == null) return;
                DeleteAuthorStatus();
            }
            ));

        private RelayCommand editAuthorStatusCommand;
        public RelayCommand EditAuthorStatusCommand => editAuthorStatusCommand ?? (editAuthorStatusCommand = new RelayCommand(
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
                    var res = db.Authors;
                    if (AuthorStatus.AuthorStatusId != 1&& !CheckRecordBeforDelete(AuthorStatus))
                    {
                        if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                        {
                            //changing DB
                            //we find all the records in which we have the desired Id and make a replacement
                            foreach (Author item in res)
                            {
                                if (item.AuthorStatus.AuthorStatusId == AuthorStatus.AuthorStatusId)
                                    item.AuthorStatus = db.AuthorStatuses.Find(new AuthorStatus() { AuthorStatusId = 1 }.AuthorStatusId);
                            }
                            db.AuthorStatuses.Remove(db.AuthorStatuses.Find(AuthorStatus.AuthorStatusId));
                            db.SaveChanges();
                            //changing collection
                            AuthorStatusRecords.Remove(AuthorStatus);
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
        private bool CheckRecordBeforDelete(AuthorStatus authorStatus)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    
                    var res = db.Authors.ToList();
                    foreach (Author item in res)
                        if(item.AuthorStatus.AuthorStatusId==authorStatus.AuthorStatusId||
                           item.AuthorStatus.AuthorStatusName == authorStatus.AuthorStatusName )                        
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
        public void AddAuthorStatus()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res4 = db.AuthorStatuses.Any(o => o.AuthorStatusName == AuthorStatus.AuthorStatusName);
                    if (!res4)
                    {
                        if (!string.IsNullOrEmpty(AuthorStatus.AuthorStatusName))
                        {
                            AuthorStatus.AuthorStatusName = AuthorStatus.AuthorStatusName.ToLower();
                            AuthorStatus.AuthorStatusName.Trim();
                            if (AuthorStatus.AuthorStatusName[0] == ' ')
                            {
                                dialogService.ShowMessage("Нельзя добавить пустую строку");
                                return;
                            }
                            db.AuthorStatuses.Add(AuthorStatus);
                            db.SaveChanges();
                            AuthorStatusRecords.Clear();
                            LoadData();
                            AuthorStatus = new AuthorStatus();

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
        public void EditAuthorStatus()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res4 = db.AuthorStatuses.Find(AuthorStatus.AuthorStatusId);
                    if (res4 != null)
                    {
                        //changing DB
                        res4.AuthorStatusName = AuthorStatus.AuthorStatusName.ToLower();
                        db.SaveChanges();
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