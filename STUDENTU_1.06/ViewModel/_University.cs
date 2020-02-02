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
   public class _University: Helpes.ObservableObject
    {
        public _University()
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

        private RelayCommand deleteUniversityCommand;
        public RelayCommand DeleteUniversityCommand => deleteUniversityCommand ??
            (deleteUniversityCommand = new RelayCommand((obj) =>
            {
                DeleteUniversity();
            }
            ));

        private RelayCommand editUniversityCommand;
        public RelayCommand EditUniversityCommand => editUniversityCommand ?? (editUniversityCommand = new RelayCommand(
                    (obj) =>
                    {
                        EditUniversity(obj as string);
                    }
                    ));

        //===================THIS METHOD IS FOR DELETE RECORDS FROM STATUS TABLES==============
        public void DeleteUniversity()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res = db.Universities.ToList();
                    int len = res.Count();
                    if (University.UniversityId != 1 && !CheckRecordBeforDelete(University))
                    {
                        if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                        {
                            //changing DB
                            //we find all the records in which we have the desired Id and make a replacement
                            for (int i = 0; i < len; i++)
                                if (res[i].UniversityId == University.UniversityId)
                                    res[i] = db.Universities.Find(1);
                            db.Universities.Remove(db.Universities.Find(University.UniversityId));
                            db.SaveChanges();
                            //changing collection
                            UniversityRecords.Remove(University);
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
        private bool CheckRecordBeforDelete(University university)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res1 = db.Clients;
                    foreach (var item in res1)
                        foreach(var i in item.Universities)
                            if(i.UniversityId== university.UniversityId||
                                i.UniversityName==university.UniversityName)
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
                    bool flag = false;
                    var res4 = db.Universities.ToList();
                    foreach (var item in res4)
                        if (item.City == University.City && item.UniversityName == University.UniversityName)
                        {
                            flag = true;
                            break;
                        }
                    if (!flag)
                    {
                        if (!string.IsNullOrEmpty(University.UniversityName))
                        {                            
                            University.UniversityName.Trim();
                            University.City.Trim();
                            if (University.UniversityName[0] == ' '||University.City[0]==' ')
                            {
                                dialogService.ShowMessage("Нельзя добавить пустую строку");
                                return;
                            }
                            db.Universities.Add(University);
                            db.SaveChanges();
                            UniversityRecords.Clear();
                            LoadUniversityData();
                            University = new University();

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
        public void EditUniversity(string newName)
        {
            if (University.UniversityName == "---")
            {
                dialogService.ShowMessage("Нельзя редактировать эту запись");
                return;
            }
            University.UniversityName = newName;
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res4 = db.Universities.Find(University.UniversityId);
                    if (res4 != null)
                    {
                        //changing DB
                        University.UniversityName.Trim();
                        University.City.Trim();
                        if (University.UniversityName[0] == ' ' || University.City[0] == ' ')
                        {
                            dialogService.ShowMessage("Нельзя добавить пустую строку");
                            return;
                        }
                        res4.UniversityName = University.UniversityName;
                        res4.City = University.City;
                        db.SaveChanges();
                        UniversityRecords.Clear();
                        LoadUniversityData();
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
