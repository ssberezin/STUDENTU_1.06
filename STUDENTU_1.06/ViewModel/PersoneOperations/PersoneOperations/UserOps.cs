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
using STUDENTU_1._06.Views.PersoneOperations.UserOperations;

namespace STUDENTU_1._06.ViewModel.PersoneOperations.PersoneOperations
{
    public class UserRecord
    {
        public int UserId { get; set; }
        public string NickName { get; set; }   
        public string FIO { get; set; }
        public string Accessname { get; set; }
        public DateTime StartDateWork { get; set; }
        public string Firedate { get; set; }
        
    }

    class UserOps : Helpes.ObservableObject
    {
        IDialogService dialogService;//for show messages in mvvm pattern order
        IShowWindowService showWindow;//for show messages in mvvm pattern order

        bool editMode = false,
            anyUsersInDB = false,
            newUserSave = false,//us with anyUsersInDB. 
            cancelSaveUserData = false;//us during compare date in save usere data methode

        int UserId=0;

        Object closeRegWindow;

        public ObservableCollection<string> AccessNameList { get; set; }

        //for keep Users List
        public ObservableCollection<UserRecord> Records { get; set; }


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

        private User littleUser;
        public User LittleUser//set that parametr in CheckUserAccessRights method
        {
            get { return littleUser; }
            set
            {
                if (littleUser != value)
                {
                    littleUser = value;
                    OnPropertyChanged(nameof(LittleUser));
                }

            }
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

        private UserRecord selectedRecord;
        public UserRecord SelectedRecord
        {
            get { return selectedRecord; }
            set
            {
                if (selectedRecord != value)
                {
                    selectedRecord = value;
                    OnPropertyChanged(nameof(SelectedRecord));
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

        private _Filters _filters;
        public _Filters _Filters
        {
            get { return _filters; }
            set
            {
                if (_filters != value)
                {
                    _filters = value;
                    OnPropertyChanged(nameof(Filters));
                }
            }
        }

        //bool notTheOne = false;//flag for 

       

        public UserOps(object obj)
        {
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
            closeRegWindow = obj;
    
            string error = CheckExistUser();
            if (error != null)
            {
                dialogService.ShowMessage(error);              
                return;
            }
            DefaultDataLoad();

        }

        //for UserInformationWindow
        public UserOps(int UserId, int tmp)
        {
            //notTheOne = true;
            anyUsersInDB = true;//here we show, that we have some user(s) in DB
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
            if (tmp != 0)
            {
                this.UserId = tmp;
                editMode = true;
                DefaultDataLoadForEditUser(tmp);
            }
            else
            {
                this.UserId = UserId;
                DefaultDataLoad();
            }
            
        }
        public UserOps(int UserId)
        {
            //notTheOne = true;
            anyUsersInDB = true;//here we show, that we have some user(s) in DB
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
            Records = new ObservableCollection<UserRecord>();
            this.UserId = UserId;
            LoadDataForUsersList();
        }


        private void DefaultDataLoad()
        {
            _Contacts = new _Contacts();
            DefaultPhoto = "default_avatar.png";
            Usver = new PersoneContactsData();
            
            AccessNameList = new ObservableCollection <string>() {"Админ", "Мастер-админ" };

            //first user have to be only "master-admin"
            Usver.User.AccessName = "Мастер-админ";
            //dialogService = new DefaultDialogService();
            //showWindow = new DefaultShowWindowService();
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

        //=====================Command for close user registration window ======================================
        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand => cancelCommand ??
            (cancelCommand = new RelayCommand(
                    (obj) =>
                    {
                        Window win = obj as Window;
                        win.Close();
                    }
                    ));


        //=====================Command for call AddContactsWindow.xaml ======================================
        private RelayCommand newEditContactsCommand;
        public RelayCommand NewEditContactsCommand => newEditContactsCommand ??
            (newEditContactsCommand = new RelayCommand(
                    (obj) =>
                    {
                        _Contacts.TmpContacts = _Contacts.Contacts;
                        _Contacts.NewEditContacts(new AddContactsWindow(obj));
                    }
                    ));


        //==================================Command for save user data to DB=================================
        private RelayCommand saveUserDataCommand;
        public RelayCommand SaveUserDataCommand => saveUserDataCommand ?? (saveUserDataCommand = new RelayCommand(
                    (obj) =>
                    {
                        if (!editMode)
                        {
                           
                            SaveUserData();
                            //here we close current registration window right after first user registration
                            if (!anyUsersInDB && newUserSave)
                            {
                                //we can close reg window only if new users data has been save
                                Window win = closeRegWindow as Window;
                                win.Close();
                            }
                        }
                        else
                        {                            
                            if (SelectedRecord == null)
                                SaveEditUserData(UserId);
                            else
                                SaveEditUserData(SelectedRecord.UserId);
                        }    
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
                    

                    Persone tmpPerson = CheckExistPerson(_Contacts.Contacts,0);
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
                        Usver.User.Persone.PersoneDescription = Usver.PersoneDescription;                    
                        db.Users.Add(Usver.User);
                        db.SaveChanges();
                    newUserSave = true;
                    if (!anyUsersInDB)
                        dialogService.ShowMessage("Данные сохранены. Теперь нужно авторизироваться...");
                    else
                        dialogService.ShowMessage("Данные сохранены.");
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
            if (!anyUsersInDB && Usver.User.AccessName != "Мастер-админ")
                return "Так как регистрируется первый пользователь,\n" +
                    " то его уровень доступа должен быть Мастер-админ, не менее.";
            if (Usver.User.AccessName=="" || Usver.User.AccessName == null)
                return "Нужно указать права доспупа";
            if (Usver.PersoneDescription.Source == null || Usver.Persone.Source == "" || personeOps.EmptyStringValidation(Usver.Persone.Source)!=null)
                return "Нужно корректно заполнить поле источника данными о пользователе";
            if (Usver.Date.StartDateWork > ZeroDefaultDate(DateTime.Now))
                return "Дата начала сотрудничества не может быть больше текущей";
            if (Usver.Date.EndDateWork != new DateTime(1900, 1, 1) && Usver.Date.EndDateWork < Usver.Date.StartDateWork || Usver.Date.EndDateWork > DateTime.Now)
                return "Поле даты увольнения сотрудника заполнено не корректно.";
            if (personeOps.EmptyStringValidation(Usver.PersoneDescription.Description) != null)
                return "Поле информации о пользователе не должно быть пустым или заполнено не корректно";
            if(personeOps.EmptyStringValidation(Usver.PersoneDescription.FeedBack) != null)
                return "Поле информации о отзывов о сотруднике не должно быть пустым или заполнено не корректно";
            string er = null;
            if (!editMode)
            {
                er = CheckExistLogin(Usver.User.UserNickName);
                if (er != null)
                    return er;
            }
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
                  
                    if (UserId==0 && db.Users.Count() > 0)
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


        private string CheckExistLogin(string newUserLogin)
        {
            string result = "По технической причине в текущий момент \n регистрация нового пользователя не возможна";
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var tmp = db.Users.Where(e => e.UserNickName == newUserLogin).FirstOrDefault();
                    if (tmp!=null)
                        result = "Текущий логин занят. Нужно задать другой";
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
        private Persone CheckExistPerson(Contacts contacts, int usrId)
        {
            
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    if (usrId == 0)
                    {
                        // here we find  Id of former Contscts
                        int contactId = 0;
                        contactId = _Contacts.Contacts.CheckContacts(contacts);
                        if (contactId == 0)
                            return new Persone { PersoneId = 0 };
                        else
                            return db.Persones.Where(o => o.Contacts.ContactsId == contactId).FirstOrDefault();
                    }
                    else 
                    {
                        // here we are trying to find matches by contacts with the database, excluding contacts
                        // user whose data is currently being edited
                        int? contactId = 0;
                        contactId = _Contacts.Contacts.CheckContacts(contacts, usrId).Value;
                        if (contactId == null)
                            return null;
                        
                        if (contactId==0)
                            return new Persone { PersoneId = 0 };
                        else
                            return db.Persones.Where(o => o.Contacts.ContactsId == contactId).FirstOrDefault();

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

            return null;
        }

        // here we get only the year, month, day, with zero other indicators
        public DateTime ZeroDefaultDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        //=================================COMANDS FOR USER DATA OPERATIONS =====================

        private RelayCommand addNewUserCommand;
        public RelayCommand AddNewUserCommand => addNewUserCommand ?? (addNewUserCommand = new RelayCommand(
                    (obj) =>
                    {
                  
                         AddNewUserByRegUser();
                  
                    }
                    ));

        private void AddNewUserByRegUser()
        {
            if (CheckUserAccessRights(UserId) != null)
                return;
            UserRegistration usver;
            usver = new UserRegistration(UserId,0);
            showWindow.ShowDialog(usver);
        }


        //==================================Command users list show=================================
        private RelayCommand showUsersListCommand;
        public RelayCommand ShowUsersListCommand => showUsersListCommand ?? (showUsersListCommand = new RelayCommand(
                    (obj) =>
                    {
                        
                        ShowUsersList();
                        
                    }
                    ));

        private void ShowUsersList()
        {
            UsersInformationWindow usversWindow;
            usversWindow = new UsersInformationWindow(UserId);
            if (CheckUserAccessRights(UserId)!=null)
                return;
           
            showWindow.ShowDialog(usversWindow);
            
        }

        private string CheckUserAccessRights(int UserId)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    LittleUser = db.Users.Where(e => e.UserId == UserId).FirstOrDefault();
                    if (LittleUser == null)                    
                        return "Проблемы со связью с БД при проверке \n прав доступа пользователя";
                    if (LittleUser.AccessName != "Мастер-админ")
                        return "Нет прав доступа";
                    
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

        private void LoadDataForUsersList()
        {
            Records.Clear();       
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    _Filters = new _Filters();
                   
                    var COrders = db.Users.ToList().OrderBy(o => o.Persone.Surname);
                    if ( COrders == null)
                    {
                        dialogService.ShowMessage("Проблемы со связью с БД при попытке отобразить список пользователей");
                        return;
                    }
                    foreach (var item in COrders)
                    {
                        Dates date = item.Persone.Dates.Where(e => e.Persone.PersoneId == item.Persone.PersoneId).FirstOrDefault();
                        UserRecord record = new UserRecord
                        {
                            UserId = item.UserId,
                            NickName = item.UserNickName,
                            FIO = item.Persone.Surname + " " + item.Persone.Name + " " + item.Persone.Patronimic,
                            Accessname = item.AccessName,
                            StartDateWork = date.StartDateWork,
                            Firedate = date.EndDateWork <date.StartDateWork? "работает" : date.EndDateWork.ToString(("d"))
                        };
                        Records.Add(record);                       
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

        //==================================Command for update users list =================================
        private RelayCommand upddateUsersListCommand;
        public RelayCommand UpddateUsersListCommand => upddateUsersListCommand ?? (upddateUsersListCommand = new RelayCommand(
                    (obj) =>
                    {
                        LoadDataForUsersList();
                    }
                    ));        

        private RelayCommand editUserCommand;
        public RelayCommand EditUserCommand => editUserCommand ?? (editUserCommand = new RelayCommand(
                    (obj) =>
                    {
                        EditUser(SelectedRecord.UserId);
                    }
                    ));

        private void EditUser(int usrId)
        {
            UserRegistration usver;
            usver = new UserRegistration(0, usrId);
            showWindow.ShowDialog(usver);            

        }

        private void DefaultDataLoadForEditUser(int usrId)
        {
            AccessNameList = new ObservableCollection<string>() { "Админ", "Мастер-админ" };
            _Contacts = new _Contacts();
            DefaultPhoto = "default_avatar.png";
            Usver = new PersoneContactsData();
            
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    Usver.User = db.Users.Where(e => e.UserId == usrId).FirstOrDefault();
                    Usver.Persone = Usver.User.Persone;
                    _Contacts.Contacts = Usver.User.Persone.Contacts;
                    Usver.Date = Usver.Persone.Dates.Where(e => e.Persone.PersoneId == Usver.Persone.PersoneId).FirstOrDefault();
                    Usver.PersoneDescription = Usver.Persone.PersoneDescription;
                    //SelectedRecord = Usver;
                    //Usver.Persone = db.Persones.Where(e => e.PersoneId == Usver.User.Persone.PersoneId).FirstOrDefault();
                    //_Contacts.Contacts = db.Contacts.Where(e => e.ContactsId == Usver.User.Persone.Contacts.ContactsId).FirstOrDefault();
                    //Dates date = Usver.Persone.Dates.Where(e => e.Persone.PersoneId == Usver.Persone.PersoneId).FirstOrDefault();
                    //db.Dates.Attach(date);
                    //Usver.Date = date;
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

        private void SaveEditUserData(int usrId)
        {
            //db.Dates.Attach(date);
     
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                   
                    //dialogService.ShowMessage("Изменения сохранены");

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


                    Persone tmpPerson = CheckExistPerson(_Contacts.Contacts, usrId);
                    if (tmpPerson == null)
                    {
                        dialogService.ShowMessage("Проблемы со связью с базой данных\n на стадии проверки существования контактов " +
                            "\n при редактирования данных пользователя");
                        return;
                    }

                    if (tmpPerson.PersoneId != 0)
                    {
                        Persone prs = CompareContacts(tmpPerson.PersoneId);
                        if (prs == null)
                        {
                            dialogService.ShowMessage("Редактирование данных пользователя прервано");
                            return;
                        }
                        prs.PersoneId = Usver.Persone.PersoneId;
                        Usver.Persone = prs;                         
                    }
                    else
                        Usver.Persone.Contacts = _Contacts.Contacts;

                    Dates dt = Usver.Persone.Dates.Where(e => e.Persone.PersoneId == Usver.Persone.PersoneId).FirstOrDefault();
                    dt = Usver.Date;                    
                    Usver.User.Persone = Usver.Persone;
                    Usver.User.Persone.PersoneDescription = Usver.PersoneDescription;                    
                    db.Entry(Usver.User).State = EntityState.Modified;
                    db.Entry(Usver.User.Persone).State = EntityState.Modified;
                    db.Entry(Usver.User.Persone.Contacts).State = EntityState.Modified;
                    db.Entry(Usver.User.Persone.PersoneDescription).State = EntityState.Modified;
                    db.Entry(dt).State = EntityState.Modified;
                    db.SaveChanges();                   
                    
                    dialogService.ShowMessage("Данные сохранены.");

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

        private Persone CompareContacts(int PersoneId)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    Persone persone = new Persone();
                    
                    Contacts OldContacts = new Contacts();
                    //тут мы находим Person , которая уже имеется в БД с такими же контактами для дальнейшей работы с ней
                    //в текущем контексте
                    persone = db.Persones.Where(e => e.PersoneId == PersoneId).FirstOrDefault();
                    OldContacts = db.Contacts.Where(c => c.ContactsId == persone.Contacts.ContactsId).FirstOrDefault();

                    //тут мы подготавливаем данные для вызова окна сравнения текущих данных личности пользователя
                    //и предыдущих его данных 
                    _Contacts.OldPersoneCompare = (Persone)persone.CloneExceptVirtual();
                    _Contacts.CurPersoneCompare = (Persone)Usver.Persone.CloneExceptVirtual();
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
                            Usver.Contacts = OldContacts;
                            cancelSaveUserData = true;
                            return null;
                        }
                        else
                        {
                            dialogService.ShowMessage("В базе данных не могут дублироваться контакты\n" +
                                  "Задайте другие контактные данные пользователя в окне приема заказа.");
                            Usver.Contacts = OldContacts;
                            cancelSaveUserData = true;
                            return null;
                        };
                    }

                    bool personeCompare = persone.ComparePersons(persone, _Contacts.Persone);
                    bool contactsCompare = _Contacts.CompareContacts(persone.Contacts, _Contacts.Contacts);

                    if (!personeCompare)
                        persone.CopyExeptVirtualIdPhoto(persone, _Contacts.Persone);
                    
                   
                    
                    if (!contactsCompare)
                    {
                        _Contacts.Contacts.CopyExceptVirtualAndId(OldContacts, _Contacts.Contacts);
                        persone.Contacts = _Contacts.Contacts;                        
                    }
                    else
                        return null;
                    return persone;
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
            //db.SaveChanges();
        }

        //=============================================================================================
        private RelayCommand closeWindowCommand;        
        public RelayCommand CloseWindowCommand => closeWindowCommand ?? (closeWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        Window win = obj as Window;
                        win.Close();                       

                    }
                    ));

        //======================================================================================================



    }
}
