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

namespace STUDENTU_1._06.ViewModel.PersoneOperations.PersoneOperations
{
    class PersoneOps : Helpes.ObservableObject
    {
        IDialogService dialogService;//for show messages in mvvm pattern order
        IShowWindowService showWindow;//for show messages in mvvm pattern order

        private Persone persone;
        public Persone Persone
        {
            get { return persone; }
            set
            {
                if (persone != value)
                {
                    persone = value;
                    OnPropertyChanged(nameof(Persone));
                }
            }
        }

        private User usver;
        public User Usver
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

        public PersoneOps()
        {

        }


        //chek for  validation of PersoneName, PersoneSurname, Patronimic, Direction, Contacts data
        // PersoneName, PersoneSurname, Patronimic -  have to be not empty 
        // Direction - have to be bigger then 0. 
        //Contacts data - som of contacts entries have to be not empty
        public string ValidPersoneDataCheck(string PersoneName, string PersoneSurname, string Patronimic,
            int DirCount, bool ContactsValidation)
        {        
            if (EmptyStringValidation(PersoneName) != null ||
                EmptyStringValidation(PersoneSurname) != null ||
                EmptyStringValidation(Patronimic) != null)
                return "Какое-то из обязательных полей осталось пустым или заполнено не корректно";
            if ( DirCount == 0 )
                return "НЕ добавлено ни одного направления" ;
            if (!ContactsValidation)
                return "Ни одно из полей контактных данных не заполнено";           
            
            return null;
        }

        private string EmptyStringValidation(string str)
        {
            string error;
            error = null;
            if (str == null)
                return null;
            str = str.Trim(' ');
            if (str.Length < 2)
                error = "Какое-то из обязательных полей осталось пустым или заполнено не корректно";
            return error;
        }
    }
}
