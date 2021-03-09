using STUDENTU_1._06.Helpes;
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
           
            User = new User();
            dialogService = new DefaultDialogService();
            showWindow = new DefaultShowWindowService();
        }


      



        private RelayCommand checkPersoneCommand;
        public RelayCommand CheckPersoneCommand => checkPersoneCommand ?? (checkPersoneCommand = new RelayCommand(
                    (obj) =>
                    {
                        Window win = obj as Window;

                        //check for exist eny user data in Users                        
                        switch (FirstUserCheck())
                        {
                            case CheckUser.Yes:
                                //here we have user i identification and authorezation    
                                User = Identification(User);
                                if (User != null)
                                {
                                    //here we call main window fo work with application
                                    CallFirstWindow(User.UserId);
                                    win.Close();//this gave to close authoresation window
                                }

                                else
                                    dialogService.ShowMessage("Не верная пара логин-пароль");                                
                                User = new User();
                                break;
                            case CheckUser.No:
                                dialogService.ShowMessage("В БД нет ни одного пользователя. Нужно зарегистрироваться.");
                                //here we call user registration window
                                //this is relevat only at the first application start
                                //UserRegistration usver;
                                //usver = new UserRegistration();
                                //showWindow.ShowWindow(usver);
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
                        User.Pass = "";
                        User.UserNickName = "";
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
