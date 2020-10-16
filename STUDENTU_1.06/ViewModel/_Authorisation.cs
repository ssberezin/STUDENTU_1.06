﻿using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views.PersoneOperations.UserOperations;
using System.ComponentModel;
using System.IO;

namespace STUDENTU_1._06.ViewModel
{
    class _Authorisation : Helpes.ObservableObject
    {
        IDialogService dialogService;
        IShowWindowService showWindow;

        enum CheckUser
        {
            Yes,
            No,
            DB_trabl
        }

        ////for display default image
        //private string defaultPhoto;
        //public string DefaultPhoto
        //{
        //    set
        //    {
        //        if (defaultPhoto != value)
        //        {
        //            defaultPhoto = value;
        //            OnPropertyChanged(nameof(DefaultPhoto));
        //        }
        //    }
        //    get { return "/STUDENTU_1.06;component/Images/" + defaultPhoto; }
        //}

        //private _Contacts _contacts;
        //public _Contacts _Contacts
        //{
        //    get { return _contacts; }
        //    set
        //    {
        //        if (_contacts != value)
        //        {
        //            _contacts = value;
        //            OnPropertyChanged(nameof(_Contacts));
        //        }
        //    }
        //}

        private User user;
        public User User
        {
            get { return user; }
            set
            {
                if (value != user)
                {
                    user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        public _Authorisation()
        {
            DefaultDataLoad();            
        }

        private void DefaultDataLoad()
        {
            //_Contacts = new _Contacts();
            //DefaultPhoto = "default_avatar.png";
            User = new User();
            dialogService = new DefaultDialogService();
            showWindow = new DefaultShowWindowService();
        }



        private RelayCommand checkPersoneCommand;
        public RelayCommand CheckPersoneCommand => checkPersoneCommand ?? (checkPersoneCommand = new RelayCommand(
                    (obj) =>
                    {
                        //сначала делаем проверку на "не первый ли пользователь"
                        //то есть есть ли вообще записи в таблице пользователей
                        // если нет, то нужно предложить такую запись создать
                        //если есть, то работаем процедуру проверки данных для идентификации с последующей авторизацией

                        switch (FirstUserCheck())
                        {
                            case CheckUser.Yes:
                                //тут уже сверяем пару логин-пароль на наличие в БД
                                if (Identification(User) != null)
                                    CallFirstWindow(User.UserId);                                
                                else
                                    dialogService.ShowMessage("Не верная пара логин-пароль");                                                                       
                                break;
                            case CheckUser.No:
                                //регистрация первого пользователя. Типа с правами админа
                                //также актуально только при первом старте приложения
                                UserRegistration usver;
                                usver = new UserRegistration();
                                showWindow.ShowWindow(usver);
                                break;
                            case CheckUser.DB_trabl:
                                dialogService.ShowMessage("Проблемы установки связи с базой данных...");
                                break;
                        }

                    }
                    ));

        private RelayCommand cancelAuthorezitionCommand;
        public RelayCommand CancelAuthorezitionCommand => cancelAuthorezitionCommand ?? (cancelAuthorezitionCommand = new RelayCommand(
                    (obj) =>
                    {
                       
                    }
                    ));

        //проверка наличия пользователей как таковых в БД. Нужно для первого запуска 
        // check for the presence of users as such in the database. Needed for the first launch
        private CheckUser FirstUserCheck ()
        {  
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //int u = db.Universities.Count();
                    //int g = db.Users.Count();
                    if (db.Users.Count() == 0)
                        return CheckUser.No ;
                    else
                        return CheckUser.Yes;
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
            return CheckUser.DB_trabl;
        }

        //make user indentification
        private User Identification(User usver)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var tmp = db.Users.ToList();
                    foreach (var item in tmp)
                    {
                        if (item.UserNickName == usver.UserNickName)
                            if (item.Pass == usver.Pass)
                                return item;                           
                    }
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
            return null;
        }

        //Call first window after authorezation
        private void CallFirstWindow(int userId)
        {
            FirstAfterMein FirstAfterMeinWindow;
            FirstAfterMeinWindow =new FirstAfterMein(userId);
            showWindow.ShowWindow(FirstAfterMeinWindow);
        }

        //Call window of user registration
        private void UsverRegistration()
        {            
           

            switch (FirstUserCheck())
            {
                case CheckUser.Yes:                   
                        dialogService.ShowMessage("Обратитесь к администратору");
                    break;
                case CheckUser.No:
                    UserRegistration usver;
                    usver = new UserRegistration();
                    showWindow.ShowWindow(usver);
                    break;
                case CheckUser.DB_trabl:
                    dialogService.ShowMessage("Проблемы установки связи с базой данных...");
                    break;
            }

            
        }


        //for new user regigtration 
        private RelayCommand newRigistrationCommand;
        public RelayCommand NewRigistrationCommand => newRigistrationCommand ?? (newRigistrationCommand = new RelayCommand(
                   (obj) =>
                   {
                       UsverRegistration();
                   }
                   ));
    }
    
}
