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
        bool editMode = false;
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
                    string error;                    
                    error = UsverDataValidation();                    
                    if (error != null)
                    {
                        dialogService.ShowMessage(error);
                        return;
                    }



                   
                        //тут над проверить контакты по БД. Если есть такие, то подтянуть нужную person вместо того, чтоб создавать новую с одинковыми контактами
                        //пока считаем что проверка прошла успешно и никого такого мы не нашли
                        //добавляем в БД новую личность не глядя
                        Usver.Persone.Contacts = _Contacts.Contacts;
                        Usver.Persone.Dates.Add(Usver.Date);
                        Usver.User.Persone = Usver.Persone;
                        
                        db.Persones.Add(Usver.Persone);                        
                    
                        db.SaveChanges();

                        dialogService.ShowMessage("Данные автора сохранены");
                  

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

        //if return nul - then Ok
        private string UsverDataValidation()
        {
            PersoneOps personeOps = new PersoneOps();
            string error = null;
           
            if (personeOps.EmptyStringValidation(Usver.Persone.Name) != null)
                return "Поле имени не должно быть пустым или заполнено не корректно";
            if (personeOps.EmptyStringValidation(Usver.Persone.Surname) != null)
                return "Поле фамилии не должно быть пустым или заполнено не корректно";            
            if (personeOps.EmptyStringValidation(Usver.Persone.Patronimic) != null)
                return "Поле отчества не должно быть пустым или заполнено не корректно";
            if (!_Contacts.Contacts.ContactsValidation())
                return "Ни одно из полей контактных данных не заполнено";
            if (Usver.Date.DayBirth == ZeroDefaultDate(DateTime.Now))            
                return "Не корректно заполнено поле даты рождения";
            if (Usver.User.AccessName=="" || Usver.User.AccessName == null)
                return "Нужно указать права доспупа";
            if (Usver.Persone.Source == null || Usver.Persone.Source == "" || personeOps.EmptyStringValidation(Usver.Persone.Source)!=null)
                return "Нужно корректно заполнить поле источника данными о пользователе";
            if (Usver.Date.StartDateWork > ZeroDefaultDate(DateTime.Now))
                return "Дата начала сотрудничества не может быть больше текущей";
            if (Usver.Date.EndDateWork != new DateTime(1900, 1, 1) && Usver.Date.EndDateWork < Usver.Date.StartDateWork || Usver.Date.EndDateWork > DateTime.Now)
                return "Поле даты увольнения сотрудника заполнено не корректно.";
            if (personeOps.EmptyStringValidation(Usver.PersoneDescription.Description) != null)
                return "Поле информации о пользователе не должно быть пустым или заполнено не корректно";
            if(personeOps.EmptyStringValidation(Usver.PersoneDescription.FeedBack) != null)
                return "Поле информации о отзывов о сотруднике не должно быть пустым или заполнено не корректно";
            return error;

            
        }

        // here we get only the year, month, day, with zero other indicators
        public DateTime ZeroDefaultDate(DateTime date)
        {
            return date.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second).AddMilliseconds(-DateTime.Now.Millisecond);
        }
        //======================================================================================================

    }
}
