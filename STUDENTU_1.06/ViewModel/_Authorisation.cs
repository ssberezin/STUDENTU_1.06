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
using System.ComponentModel;

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
            User = new User();

        }
       
        private RelayCommand checkPersoneCommand;
        public RelayCommand CheckPersone => checkPersoneCommand ?? (checkPersoneCommand = new RelayCommand(
                    (obj) =>
                    {                        
                       //сначала делаем проверку на "не первый ли пользователь"
                       //то есть есть ли вообще записи в таблице пользователей
                       // если нет, то нужно предложить такую запись создать
                       //если есть, то работаем процедуру проверки данных для идентификации с последующей авторизацией
                       

                    }
                    ));

        private RelayCommand cancelAuthorezitionCommand;
        public RelayCommand CancelAuthorezitionCommand => cancelAuthorezitionCommand ?? (cancelAuthorezitionCommand = new RelayCommand(
                    (obj) =>
                    {
                       
                    }
                    ));


        private CheckUser FirstUserCheck ()
        {
            CheckUser chu;

            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    if (db.Users.Count() == 0)

                        CheckUser.No ;
                    else
                        return 1;
                    


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
            return 2;
        }


    }
}
