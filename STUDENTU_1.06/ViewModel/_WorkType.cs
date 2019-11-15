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
   public class _WorkType : Helpes.ObservableObject
    {
        public _WorkType()
        {
            WorkType = new WorkType();
            WorkTypesRecords = new ObservableCollection<WorkType>();
            LoadWorkTypesData();
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
        }

        IDialogService dialogService;
        IShowWindowService showWindow;

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
                    WorkType = WorkTypesRecords[0];
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
                        AddWorkType();
                                            }
                    ));

        private RelayCommand deleteWorkTypeCommand;
        public RelayCommand DeleteWorkTypeCommand => deleteWorkTypeCommand ??
            (deleteWorkTypeCommand = new RelayCommand((selectedItem) =>
            {
                if (selectedItem == null) return;
                DeleteWorkType();
            }
            ));

        private RelayCommand editWorkTypeCommand;
        public RelayCommand EditWorkTypeCommand => editWorkTypeCommand ?? (editWorkTypeCommand = new RelayCommand(
                    (selectedItem) =>
                    {
                        if (selectedItem == null) return;
                        EditWorkType();
                    }
                    ));

        //===================THIS METHOD IS FOR DELETE RECORDS FROM WORKTYPES TABLES==============
        public void DeleteWorkType()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res = db.Orderlines;
                    if (WorkType.WorkTypeId != 1&&!CheckRecordBeforDelete(WorkType))
                    {
                        if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                        {
                            //changing DB
                            //we find all the records in which we have the desired Id and make a replacement
                            foreach (OrderLine order in res)
                            {
                                if (order.WorkType.WorkTypeId == WorkType.WorkTypeId)
                                    order.WorkType = db.WorkTypes.Find(new WorkType() { WorkTypeId = 1 }.WorkTypeId);
                            }
                            db.WorkTypes.Remove(db.WorkTypes.Find(WorkType.WorkTypeId));
                            db.SaveChanges();
                            //changing collection
                            WorkTypesRecords.Remove(WorkType);
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
        private bool CheckRecordBeforDelete(WorkType wt)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //check in Orderlines table
                    var res = db.Orderlines;
                    foreach (OrderLine item in res)
                        if (item.WorkType.WorkTypeId == wt.WorkTypeId ||
                           item.WorkType.TypeOfWork == wt.TypeOfWork)
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

        //===================THIS METHOD IS FOR ADD RECORDS IN WORKTYPE TABLES==============
        public void AddWorkType()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res = db.WorkTypes.Any(o => o.TypeOfWork == WorkType.TypeOfWork);
                    if (!res)
                    {
                        if (!string.IsNullOrEmpty(WorkType.TypeOfWork)||WorkType.TypeOfWork!="---")
                        {
                            WorkType.TypeOfWork = WorkType.TypeOfWork.ToLower();
                            WorkType.TypeOfWork.Trim();
                            if (WorkType.TypeOfWork[0] == ' ')
                            {
                                dialogService.ShowMessage("Нельзя добавить пустую строку");
                                return;
                            }
                            db.WorkTypes.Add(WorkType);
                            db.SaveChanges();
                            WorkTypesRecords.Clear();
                            LoadWorkTypesData();
                            WorkType = new WorkType();
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

        //===================THIS METHOD IS FOR EDIT RECORDS IN  WORKTYPES TABLES==============
        public void EditWorkType()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res = db.WorkTypes.Find(WorkType.WorkTypeId);
                    if (res != null)
                    {
                        //changing DB
                        res.TypeOfWork = WorkType.TypeOfWork.ToLower();
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
