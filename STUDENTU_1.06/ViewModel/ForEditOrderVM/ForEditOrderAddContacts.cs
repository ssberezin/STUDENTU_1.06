using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.DBModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views;

//================тут у нас бизнес логика для редактирования данных по дополнительным контактам
//=================here we have business logic for editing data on additional contacts

namespace STUDENTU_1._06.ViewModel
{


    public partial class ForEditOrder : Helpes.ObservableObject
    {

        //====================================Save contact COMMAND================================

        private RelayCommand saveContactCommand;
        public RelayCommand SaveContactCommand => saveContactCommand ?? (saveContactCommand = new RelayCommand(
                    (obj) =>
                    {

                        //тут у нас просто вывод сообщения , т.к. все данные и так привязаны к нужным 
                        //полям в окне редактирования + сохранение данных о контактах происходит
                        //в SaveNewOrder
                        // here we just have a message output, because all data is already tied to the right
                        // fields in the edit window + saving contact data occurs
                        // in SaveNewOrder

                        //а это делаем пока на всякий случай
                        ContactsRecords.Add(Contacts);
                        dialogService.ShowMessage("Данные сохранены");
                    }
                    ));





        //=====================Command for call AddContactsWindow.xaml ======================================

        private RelayCommand newEditContactsCommand;
        public RelayCommand NewEditContactsCommand => newEditContactsCommand ?? 
            (newEditContactsCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddContactsWindow addContactsWindow = new AddContactsWindow(obj);
                        addContactsWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(addContactsWindow);
                    }
                    ));

    }
}
