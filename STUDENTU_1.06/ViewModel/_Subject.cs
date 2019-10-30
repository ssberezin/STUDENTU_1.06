using STUDENTU_1._06.Helpes;
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
                        editSubject.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(editSubject);
                    }
                    ));

        //==================Commands for edit SUBJECTS =========================================

        private RelayCommand addSubjectCommand;
        public RelayCommand AddSubjectCommand => addSubjectCommand ?? (addSubjectCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddSubj();
                    }
                    ));

        private RelayCommand deleteSubjectCommand;
        public RelayCommand DeleteSubjectCommand => deleteSubjectCommand ??
            (deleteSubjectCommand = new RelayCommand((selectedItem) =>
            {
                if (selectedItem == null) return;
                DeleteSubj();
            }
            ));

        private RelayCommand editSubjectCommand;
        public RelayCommand EditSubjectCommand => editSubjectCommand ?? (editSubjectCommand = new RelayCommand(
                    (selectedItem) =>
                    {
                        if (selectedItem == null) return;
                        EditSubj();
                    }
                    ));


        //===================THIS METHOD IS FOR ADD RECORDS SUBJECTS TABLE==============       
        public void AddSubj()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res2 = db.Subjects.Any(o => o.SubName == Subj.SubName);
                    if (!res2)
                    {
                        if (!string.IsNullOrEmpty(Subj.SubName))
                        {
                            db.Subjects.Add(Subj);
                            db.SaveChanges();
                            SubjRecords.Clear();
                            LoadSubjectsData();
                            Subj = new Subject();
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
        public void EditSubj()
        {
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
                    if (Subj.SubjectId != 1)
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
                            SubjRecords.Remove(Subj);
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
            if (FindSubj())
            {
                dialogService.ShowMessage("Уже есть такое название в в списке");
            }
            else
                AuthorSubjects.Add(Subj);
        }

        //here we check AuthorSubjects for the added item
        private bool FindSubj()
        {
            bool flag = false;
            foreach (Subject item in AuthorSubjects)
            {
                flag = Subj.SubjectId == item.SubjectId ? true : false;
                break;

                if (Subj.SubjectId == item.SubjectId ||Subj.SubName == "---")
                {
                    flag = true;
                    break;
                }
            }
            return flag;
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
            AuthorSubjects.Remove(Subj);
        }

    }
}
