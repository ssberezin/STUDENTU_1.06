﻿using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;

namespace STUDENTU_1._06.ViewModel
{
    //Class for operations with Direction table
    public  class _Subject : Helpes.ObservableObject
    {
        public _Subject()
        {
            Subj = new Subject();
            SelectedSubj = new Subject();
            SelectedSubj2 = new Subject();
            SubjRecords = new ObservableCollection<Subject>();
            AuthorSubjects = new ObservableCollection<Subject>();
            LoadSubjectsData();
            showWindow = new DefaultShowWindowService();
            dialogService= new  DefaultDialogService();
        }

        IDialogService dialogService;
        IShowWindowService showWindow;

        public ObservableCollection<Subject> SubjRecords { get; set; }
        public ObservableCollection<Subject> AuthorSubjects { get; set; }

        //for fast delete from AuthorSubjects
        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                if (index != value)
                {
                    index = value;
                    OnPropertyChanged(nameof(Index));
                }
            }
        }

        private Subject subj;
        public Subject Subj
        {
            get { return subj; }
            set
            {
                if (subj != value)
                {
                    subj = value;
                    OnPropertyChanged(nameof(Subj));
                }
            }
        }

        private Subject selectedSubj;
        public Subject SelectedSubj
        {
            get { return selectedSubj; }
            set
            {
                if (selectedSubj != value)
                {
                    selectedSubj = value;
                    OnPropertyChanged(nameof(SelectedSubj));
                }
            }
        }

        private Subject selectedSubj2;
        public Subject SelectedSubj2
        {
            get { return selectedSubj2; }
            set
            {
                if (selectedSubj2 != value)
                {
                    selectedSubj2 = value;
                    OnPropertyChanged(nameof(SelectedSubj2));
                }
            }
        }
        //load data array from "Subjects" table
        private void LoadSubjectsData()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var list = db.Subjects.ToList<Subject>();
                    foreach (var item in list)
                    {
                        SubjRecords.Add(
                        new Subject
                        {
                            SubjectId = item.SubjectId,
                            SubName = item.SubName
                        });
                    }
                    Subj = SubjRecords[0];
                    SelectedSubj2 = Subj;
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

        private RelayCommand newEditSubject;
        public RelayCommand NewEditSubject => newEditSubject ?? (newEditSubject = new RelayCommand(
                    (obj) =>
                    {
                        EditSubject editSubject = new EditSubject(obj);                        
                        showWindow.ShowDialog(editSubject);
                    }
                    ));

        //==================Commands for edit SUBJECTS =========================================

        private RelayCommand addSubjectCommand;
        public RelayCommand AddSubjectCommand => addSubjectCommand ?? (addSubjectCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddSubj(obj as string);
                    }
                    ));

        private RelayCommand deleteSubjectCommand;
        public RelayCommand DeleteSubjectCommand => deleteSubjectCommand ??
            (deleteSubjectCommand = new RelayCommand((selectedItem) =>
            {                
                DeleteSubj();
            }
            ));

        private RelayCommand editSubjectCommand;
        public RelayCommand EditSubjectCommand => editSubjectCommand ?? (editSubjectCommand = new RelayCommand(
                    (obj) =>
                    {                        
                        EditSubj(obj as string);
                    }
                    ));


        //===================THIS METHOD IS FOR ADD RECORDS SUBJECTS TABLE==============       
        public void AddSubj(string newSubName)
        {
            Subj.SubName = newSubName;
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res2 = db.Subjects.Any(o => o.SubName == Subj.SubName);
                    if (!res2)
                    {
                        if (!string.IsNullOrEmpty(Subj.SubName) || Subj.SubName != "---")
                        {
                            Subj.SubName.Trim();
                            if (Subj.SubName[0] == ' ')
                            {
                                dialogService.ShowMessage("Нельзя добавить пустую строку");
                                return;
                            }
                            db.Subjects.Add(Subj);
                            db.SaveChanges();
                            SubjRecords.Clear();
                            LoadSubjectsData();
                            Subj = new Subject();
                            SelectedSubj2 = Subj;
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

       //===================THIS METHOD IS FOR EDIT RECORDS IN SUBJECTS TABLE==============
        public void EditSubj(string newSubName)
        {
            if (Subj.SubName == "---")
            {
                dialogService.ShowMessage("Нельзя редактировать эту запись");
                return;
            }
            Subj.SubName = newSubName;

            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res2 = db.Subjects.Find(Subj.SubjectId);
                    if (res2 != null)
                    {
                        //changing DB
                        res2.SubName = Subj.SubName;
                        db.SaveChanges();
                        SubjRecords.Clear();
                        LoadSubjectsData();
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

        public void DeleteSubj()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res = db.Orderlines;
                    if (Subj.SubjectId != 1&&!CheckRecordBeforDelete(Subj))
                    {
                        if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                        {
                            //changing DB
                            //we find all the records in which we have the desired Id and make a replacement
                            foreach (OrderLine order in res)
                            {
                                if (order.Subject.SubjectId == Subj.SubjectId)
                                    order.Subject = db.Subjects.Find(new Subject() { SubjectId = 1 }.SubjectId);
                            }
                            db.Subjects.Remove(db.Subjects.Find(Subj.SubjectId));
                            db.SaveChanges();
                            //changing collection
                            AuthorSubjects.Remove(Subj);
                            SubjRecords.Remove(Subj);

                            Subj = SubjRecords[0];
                            SelectedSubj2 = Subj;
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
        private bool CheckRecordBeforDelete(Subject subj)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //check in Orderlines table
                    var res = db.Orderlines;
                    foreach (OrderLine item in res)
                        if (item.Subject.SubjectId == subj.SubjectId ||
                           item.Subject.SubName == subj.SubName)
                            return true;
                    //if previos check  in Orderlines table wasn't true - check in Author table
                    var authorRes = db.Authors;
                    foreach (Author item in authorRes)
                        foreach (Subject i in item.Subject)
                            if (i.SubjectId == subj.SubjectId ||
                               i.SubName == subj.SubName)
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

        //===================================COMMAND FOR ADD Subj INTO AuthorSubjects ============      

        private RelayCommand addAuthorSubjectCommand;
        public RelayCommand AddAuthorSubjectCommand => addAuthorSubjectCommand ??
            (addAuthorSubjectCommand = new RelayCommand((selectedItem) =>
            {
                AddAuthorSubject();
            }
           ));

        private void AddAuthorSubject()
        {
           
            if (FindSubj(SelectedSubj2) || string.IsNullOrEmpty(SelectedSubj2.SubName) || SelectedSubj2.SubName == "---")
            {
                dialogService.ShowMessage("Уже есть такая запись в списке");
            }
            else
                AuthorSubjects.Add(SelectedSubj2);
        }

        //here we check AuthorSubjects for the added item
        private bool FindSubj(Subject subj)
        {
            
            foreach (Subject item in AuthorSubjects)
            {
                if (subj.SubjectId == item.SubjectId)                
                    return true;
            }
            return false;
        }

        //===================================COMMAND FOR DELETE DIRECTIONS FROM AuthorDirections ============      

        private RelayCommand delFromAuthorSubjectCommand;
        public RelayCommand DelFromAuthorSubjectCommand => delFromAuthorSubjectCommand ??
            (delFromAuthorSubjectCommand = new RelayCommand((selectedItem) =>
            {
                DelFromAuthorSubjects();
            }
           ));

        private void DelFromAuthorSubjects()
        {            
            AuthorSubjects.Remove(AuthorSubjects[Index]);
        }

    }
}
