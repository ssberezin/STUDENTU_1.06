using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq;
using System.Windows;
using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.ViewModel.Filters;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Model.DBModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows;
using STUDENTU_1._06.Views.EditOrderWindows.EditOrderLine;
using STUDENTU_1._06.Views;
using System.IO;

namespace STUDENTU_1._06.ViewModel.PersoneOperations.PersoneOperations
{


    class UserOps : Helpes.ObservableObject
    {
        IDialogService dialogService;//for show messages in mvvm pattern order
        IShowWindowService showWindow;//for show messages in mvvm pattern order

        public ObservableCollection<string> AccessNameList { get; set; }
       


        //for display default image
        private string defaultPhoto;
        public string DefaultPhoto
        {
            set
            {
                if (defaultPhoto != value)
                {
                    defaultPhoto = value;
                    OnPropertyChanged(nameof(DefaultPhoto));
                }
            }
            get { return "/STUDENTU_1.06;component/Images/" + defaultPhoto; }
        }

       
     

        private PersoneContactsData usver;
        public PersoneContactsData Usver
        {
            get { return usver; }
            set
            {
                if (usver != value)
                {
                    usver = value;
                    OnPropertyChanged(nameof(Usver));
                }
                
            }
        }



        private _Contacts _contacts;
        public _Contacts _Contacts
        {
            get { return _contacts; }
            set
            {
                if (_contacts != value)
                {
                    _contacts = value;
                    OnPropertyChanged(nameof(_Contacts));
                }
            }
        }

        public UserOps()
        {            
            DefaultDataLoad();
        }


        private void DefaultDataLoad()
        {
            _Contacts = new _Contacts();
            DefaultPhoto = "default_avatar.png";
            Usver = new PersoneContactsData();
            
            AccessNameList = new ObservableCollection <string>() {"Админ", "Мастер-админ" };            
            dialogService = new DefaultDialogService();
            showWindow = new DefaultShowWindowService();
        }


        //==================================Command for add new photo to persone profile================
        private RelayCommand openFileDialogCommand;
        public RelayCommand OpenFileDialogCommand => openFileDialogCommand ?? (openFileDialogCommand = new RelayCommand(
                    (obj) =>
                    {
                        PathToFile();
                    }
                    ));
        private void PathToFile()
        {
            string path;
            path = dialogService.OpenFileDialog("C:\\");
            if (path == null)
                return;
            Usver.Persone.Photo = File.ReadAllBytes(path);
        }


        //=====================Command for call AddContactsWindow.xaml ======================================
        private RelayCommand newEditContactsCommand;
        public RelayCommand NewEditContactsCommand => newEditContactsCommand ??
            (newEditContactsCommand = new RelayCommand(
                    (obj) =>
                    {
                        //_Contacts.TmpContacts = _Contacts.Contacts;
                        _Contacts.NewEditContacts(new AddContactsWindow(obj));
                    }
                    ));


        //==================================Command for save user data to DB=================================
        private RelayCommand saveUserDataCommand;
        public RelayCommand SaveUserDataCommand => saveUserDataCommand ?? (saveUserDataCommand = new RelayCommand(
                    (obj) =>
                    {
                        SaveUserData();
                    }
                    ));
        private void SaveUserData()
        {

            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    PersoneOps personeOps = new PersoneOps();//create for us personeOps.ValidPersoneDataCheck methode
                    string error;
                    error = personeOps.ValidPersoneDataCheck(Usver.Persone.Name, Usver.Persone.Surname, Usver.Persone.Patronimic,
                        1, _Contacts.Contacts.ContactsValidation());
                    if (error != null)
                    {
                        dialogService.ShowMessage(error);
                        return;
                    }
                    else
                    {
                        //тут, возможно, придется пилить ветку редактирования 
                        //возможно, что так
                        //if we  need to modified entrie
                        //    if (PersoneContactsData.Author.AuthorId != 0)
                        //    {
                        //        db.Entry(PersoneContactsData.Persone).State = EntityState.Modified;
                        //        db.Entry(PersoneContactsData.Date).State = EntityState.Modified;
                        //        db.Entry(PersoneContactsData.Author).State = EntityState.Modified;
                        //        db.Entry(_Dir.Dir).State = EntityState.Modified;
                        //        db.Entry(_Subj.Subj).State = EntityState.Modified;
                        //        db.Entry(PersoneContactsData.PersoneDescription).State = EntityState.Modified;
                        //    }
                    };
                    
                    Usver.Contacts = _Contacts.Contacts;
                    //    PersoneContactsData.Persone.Contacts = _Contacts.Contacts;
                    //    if (PersoneContactsData.Author.AuthorId != 0)
                    //        PersoneContactsData.Persone.Dates[0] = PersoneContactsData.Date;
                    //    else
                    //        PersoneContactsData.Persone.Dates.Add(PersoneContactsData.Date);

                    //    PersoneContactsData.Persone.PersoneDescription = PersoneContactsData.PersoneDescription;
                    //    PersoneContactsData.Author.Persone = PersoneContactsData.Persone;
                    //    PersoneContactsData.Author.AuthorStatus = db.AuthorStatuses.Find(_AuthorStatus.AuthorStatus.AuthorStatusId);
                    //    if (PersoneContactsData.Author.AuthorId == 0)
                    //        db.Authors.Add(PersoneContactsData.Author);
                    //    db.SaveChanges();

                    //    //удаляем из списка направлений упоминания об авторе, если в списке направлений автора нет более того или  иного направления после правки
                    //    // remove the author’s mention from the list of directions if the author’s list does not have more than one direction or another after editing
                    //    var res = db.Directions.ToList();
                    //    foreach (var i in res)
                    //    {
                    //        if (i.Author.Contains(PersoneContactsData.Author) && !_Dir.AuthorDirections.Contains(i))
                    //            i.Author.Remove(PersoneContactsData.Author);
                    //        continue;
                    //    }


                    //    foreach (Direction item in _Dir.AuthorDirections)
                    //    {
                    //        var res1 = db.Directions.Find(item.DirectionId);
                    //        if (res1 != null && !res1.Author.Contains(PersoneContactsData.Author))
                    //        {
                    //            //changing DB
                    //            if (PersoneContactsData.Author.AuthorId != 0)
                    //            {

                    //                res1.Author.Add(PersoneContactsData.Author);
                    //                continue;
                    //            }
                    //            else
                    //                res1.Author.Add(PersoneContactsData.Author);
                    //            continue;
                    //        }
                    //    }
                    //    db.SaveChanges();

                    //    //удаляем из списка предметов упоминания об авторе, если в списке предметов автора нет более того или  иного направления после правки
                    //    // remove the author’s mention from the list of subjects if the author’s list does not have more than one subject or another after editing
                    //    var res2 = db.Subjects.ToList();
                    //    foreach (var i in res2)
                    //    {
                    //        if (i.Authors.Contains(PersoneContactsData.Author) && !_Subj.AuthorSubjects.Contains(i))
                    //            i.Authors.Remove(PersoneContactsData.Author);
                    //        continue;
                    //    }
                    //    //here we add author in subjects
                    //    foreach (Subject item in _Subj.AuthorSubjects)
                    //    {
                    //        var res1 = db.Subjects.Find(item.SubjectId);
                    //        if (res1 != null)
                    //        {
                    //            //changing DB
                    //            res1.Authors.Add(PersoneContactsData.Author);
                    //            continue;
                    //        }
                    //        else
                    //            res1.Authors.Add(PersoneContactsData.Author);
                    //        continue;

                    //    }
                    //    db.SaveChanges();

                    //    dialogService.ShowMessage("Данные автора сохранены");
                    //    //обнуляем поля окна
                    //    //clear window fields
                    //    if (PersoneContactsData.Author.AuthorId == 0)
                    //    {
                    //        _Contacts = new _Contacts();
                    //        PersoneContactsData.Persone = new Persone();
                    //        _AuthorStatus = new _AuthorStatus();
                    //        PersoneContactsData.Author = new Author();
                    //        PersoneContactsData.Date = new Dates();
                    //        _Subj = new _Subject();
                    //        _Dir = new _Direction();
                    //        PersoneContactsData.PersoneDescription = new PersoneDescription();
                    //    }
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
        
        //======================================================================================================

    }
}
