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
using STUDENTU_1._06.Views.EditOrderWindows.ContactsWindows;
using System.Data.Entity;

namespace STUDENTU_1._06.ViewModel.PersoneOperations.PersoneOperations
{


    class UserOps : Helpes.ObservableObject
    {
        IDialogService dialogService;//for show messages in mvvm pattern order
        IShowWindowService showWindow;//for show messages in mvvm pattern order
        bool editMode = false,
            cancelSaveUserData = false;
        
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
            string error = CheckExistUser();
            if (error != null)
            {
                dialogService.ShowMessage(error);
                return;
            }
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
                    //initial data validation
                    string error;                    
                    error = UsverDataValidation();                    
                    if (error != null)
                    {
                        dialogService.ShowMessage(error);
                        return;
                    }

                    // тут мы проверяем контакты по БД.Если есть такие, то подтянуть 
                    //нужную person вместо того, чтоб создавать новую с одинковыми контактами
                    

                    Persone tmpPerson = CheckExistPerson(_Contacts.Contacts);
                    if (tmpPerson == null)
                    {
                        dialogService.ShowMessage("Проблемы со связью с базой данных\n на стадии проверки существования контактов " +
                            "\n при добавлении нового пользователя");
                        return;
                    }
                   
                    if (tmpPerson.PersoneId != 0)
                    {
                        Persone persone = new Persone();
                        Contacts OldContacts = new Contacts();
                        //тут мы находим Person , которая уже имеется в БД с такими же контактами для дальнейшей работы с ней
                        //в текущем контексте
                        Usver.Persone = db.Persones.Where(e => e.PersoneId == tmpPerson.PersoneId).FirstOrDefault();                        OldContacts = db.Contacts.Where(c => c.ContactsId == Usver.Persone.Contacts.ContactsId).FirstOrDefault();
                        OldContacts = db.Contacts.Where(c => c.ContactsId == Usver.Persone.Contacts.ContactsId).FirstOrDefault();
                        //тут мы подготавливаем данные для вызова окна сравнения текущих данных личности пользователя
                        //и предыдущих его данных 
                        _Contacts.OldPersoneCompare = (Persone)persone.CloneExceptVirtual();
                        _Contacts.CurPersoneCompare = (Persone)this.Usver.Persone.CloneExceptVirtual();
                        _Contacts.TmpContacts = (Contacts)OldContacts.CloneExceptVirtual();
                        _Contacts.OldTmpContactsCompare = (Contacts)OldContacts.CloneExceptVirtual();
                        _Contacts.TmpContactsCompare = (Contacts)this._Contacts.Contacts.CloneExceptVirtual();
                        //вызывем окно сравнения
                        CompareContatctsWindow compareContatctsWindow = new CompareContatctsWindow(this);
                        showWindow.ShowDialog(compareContatctsWindow);

                        //если в результате сравнения не был принят ни один из вариантов
                        if (!_Contacts.saveCompareResults)
                        {
                            //тут лучше придумать диалоговое окно с радиокнопками , для выбора вариантов действия
                            // - отменить прием заказа и отправить пользователя закрыть окно приема заказа
                            //т.к. не понятно как реализовать закрытие окна из вьюмодел не вмешиваяся в сраный мввм
                            //но в идеале закрыть окно приема заказа. Думаю, что это потянет за собой перепил по всему проекту
                            //процедуры закрытия окна.
                            // - 
                            if (dialogService.YesNoDialog("Не сохранен ни один из вариантов...\n" +
                                    "Отменить процедуру оформления нового пользователя?"))
                            {
                                dialogService.ShowMessage("Ок. Тогда просто закройте окно оформления нового пользователя");
                                _Contacts.Contacts = OldContacts;
                                cancelSaveUserData = true;
                                return;
                            }
                            else
                            {
                                dialogService.ShowMessage("В базе данных не могут дублироваться контакты\n" +
                                      "Задайте другие контактные данные пользователя в окне приема заказа.");
                                _Contacts.Contacts = OldContacts;
                                cancelSaveUserData = true;
                                return;
                            };
                        }

                       bool  personeCompare = persone.ComparePersons(persone, _Contacts.Persone);
                       bool contactsCompare = _Contacts.CompareContacts(persone.Contacts, _Contacts.Contacts);
                        
                        if (!personeCompare)
                        {
                            db.Entry(Usver.Persone).State = EntityState.Modified;
                            Usver.Persone.CopyExeptVirtualIdPhoto(Usver.Persone, _Contacts.Persone);
                        }
                        if (!contactsCompare)
                        {
                            db.Entry(OldContacts).State = EntityState.Modified;
                            _Contacts.Contacts.CopyExceptVirtualAndId(OldContacts, _Contacts.Contacts);
                            Usver.Persone.Contacts = _Contacts.Contacts;
                        }
                        //db.SaveChanges();
                    }
                    else                    
                        Usver.Persone.Contacts = _Contacts.Contacts;

                        Usver.Persone.Dates.Add(Usver.Date);
                        Usver.User.Persone = Usver.Persone;                        
                        db.Users.Add(Usver.User);
                        db.SaveChanges();
                        dialogService.ShowMessage("Данные сохранены");
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
            if (ZeroDefaultDate(Usver.Date.DayBirth) == ZeroDefaultDate(DateTime.Now))            
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

        //if returned null then db.Users has no entries
        private string CheckExistUser()
        {
            string result = "По технической причине в текущий момент \n регистрация нового пользователя не возможна";
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    if (db.Users.Count() > 0)
                        result = "Регистрировать новых пользователей может только администратор";
                    else
                        return null;
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
            return result;
        }

        //if returnd null - DB connetion problems
        //if Persone.Person_Id ==0 - not found any exist person
        //if Persone.Person_Id !=0 - here we fount exist person
        private Persone CheckExistPerson(Contacts contacts)
        {
            
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {                    
                    // here we find  Id of former Contscts
                    int contactId = 0;
                    contactId = _Contacts.Contacts.CheckContacts(contacts);
                    if (contactId == 0)                    
                        return new Persone { PersoneId = 0 };                    
                    else                    
                        return db.Persones.Where(o => o.Contacts.ContactsId == contactId).FirstOrDefault();
                    
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

            return null;
        }

        // here we get only the year, month, day, with zero other indicators
        public DateTime ZeroDefaultDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }


        //======================================================================================================



    }
}
