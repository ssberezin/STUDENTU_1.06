using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STUDENTU_1._06.ViewModel
{
    public  class _Contacts : Helpes.ObservableObject
    {

        //Window AddContactsWindow;
        IDialogService dialogService;
        IShowWindowService showWindow;

        private Contacts contacts;
        public Contacts Contacts
        {
            get { return contacts; }
            set
            {
                if (contacts != value)
                {
                    contacts = value;
                    OnPropertyChanged(nameof(contacts));
                }
            }
        }

        private Contacts tmpContacts;
        public Contacts TmpContacts
        {
            get { return tmpContacts; }
            set
            {
                if (tmpContacts != value)
                {
                    tmpContacts = value;
                    OnPropertyChanged(nameof(TmpContacts));
                }
            }
        }       


        public _Contacts()
        {
           Contacts = new Contacts();
            TmpContacts = new Contacts();            
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
        }

        //====================================Save contact COMMAND================================

        private RelayCommand saveContactCommand;
        public RelayCommand SaveContactCommand => saveContactCommand ?? (saveContactCommand = new RelayCommand(
                    (obj) =>
                    {
                        PersonAddContacts();
                        dialogService.ShowMessage("Данные сохранены");
                    }
                    ));
        public void PersonAddContacts()
        {
            Contacts = TmpContacts;          
        }
       
        //=========================================COMMAND FOR CANCEL SAVE CONTACTS=========================================== 

        private RelayCommand cancelSaveContactsCommand;
        public RelayCommand CancelSaveContactsCommand => cancelSaveContactsCommand ??
            (cancelSaveContactsCommand = new RelayCommand(
                    (obj) =>
                    {
                        CancelSaveContacts();
                        Window window = obj as Window;
                        window.Close();
                    }
                    ));
        private void CancelSaveContacts()
        {
            TmpContacts = Contacts;
        }

        
        public void NewEditContacts(Window addContactsWndow)
        {
            showWindow.ShowDialog(addContactsWndow);
        }


    }
}
