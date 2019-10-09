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
        public ObservableCollection<Subject> SubjRecords { get; set; }

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
                        AddDirSubjWorkType("Subject");
                    }
                    ));

        private RelayCommand deleteSubjectCommand;
        public RelayCommand DeleteSubjectCommand => deleteSubjectCommand ??
            (deleteSubjectCommand = new RelayCommand((selectedItem) =>
            {
                if (selectedItem == null) return;
                DeleteDirSubjWorkType("Subject");
            }
            ));

        private RelayCommand editSubjectCommand;
        public RelayCommand EditSubjectCommand => editSubjectCommand ?? (editSubjectCommand = new RelayCommand(
                    (selectedItem) =>
                    {
                        if (selectedItem == null) return;
                        EditDirSubjWorkType("Subject");
                    }
                    ));

    }
}
