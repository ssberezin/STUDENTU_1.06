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
using STUDENTU_1._06.Model.DBModelClasses;

namespace STUDENTU_1._06.ViewModel
{
   public class _Universities: Helpes.ObservableObject
    {
        public _Universities()
        {
            University = new University();
            UniversityRecords = new ObservableCollection<University>();
            LoadUniversityData();
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
        }

        IDialogService dialogService;
        IShowWindowService showWindow;

        public ObservableCollection<University> UniversityRecords { get; set; }

        private University university;
        public University University
        {
            get { return university; }
            set
            {
                if (university != value)
                {
                    university = value;
                    OnPropertyChanged(nameof(University));
                }
            }
        }

        //load data array from "Statuses" data
        private void LoadUniversityData()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var list = db.Universities.ToList();
                    foreach (var item in list)
                    {
                        UniversityRecords.Add(
                        new University
                        {
                            UniversityId = item.UniversityId,
                            UniversityName = item.UniversityName,
                            City=item.City
                        });
                    }
                    University = UniversityRecords[0];
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

        private RelayCommand newEditUniversityCommand;
        public RelayCommand NewEditUniversityCommand => newEditUniversityCommand ?? (newEditUniversityCommand = new RelayCommand(
                    (obj) =>
                    {
                        EditUniversity editUniversity = new EditUniversity(obj);
                        showWindow.ShowDialog(editUniversity);
                    }
                    ));

        //==================Commands for edit STATUSES =========================================

        private RelayCommand addUniversityCommand;
        public RelayCommand AddUniversityCommand => addUniversityCommand ?? (addUniversityCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddUniversity(obj as string);
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
                    if (Status.StatusId != 1 && !CheckRecordBeforDelete(Status))
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
        public void AddUniversity(string newName)
        {
            University.UniversityName = newName;
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res4 = db.Universities.Any(o => o.UniversityName == University.UniversityName);
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
