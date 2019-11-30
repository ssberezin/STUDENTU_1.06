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


        //load data array from "Directions" table
        public bool CheckAuthorContacts()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var list = db.Persones.Include("Author").Include("Client").ToList();
                    foreach (var item in list)
                    {
                        if (item.Author.Count()>0)                        
                        if (item.Contacts.Phone1 == Contacts.Phone1 ||
                           item.Contacts.Phone1 == Contacts.Phone2 ||
                           item.Contacts.Phone1 == Contacts.Phone3 ||
                           item.Contacts.Phone2 == Contacts.Phone2 ||
                           item.Contacts.Phone2 == Contacts.Phone3 ||
                           item.Contacts.Phone3 == Contacts.Phone1 ||
                           item.Contacts.Email1 == Contacts.Email1 ||
                           item.Contacts.Email1 == Contacts.Email2 ||
                           item.Contacts.Email2 == Contacts.Email2 ||
                           item.Contacts.VK == Contacts.VK ||
                           item.Contacts.Skype == Contacts.Skype)
                        {
                                if (dialogService.YesNoDialog("Контактные данные этого автора уже есть в БД авторов\n" +
                                    "Показать информацию по найденному совпадению?"))
                                {
                                    //тут надо запилить вызов окна с инфой об авторе

                                    RuleOrderLineWindow ruleOrderLineWindow = new RuleOrderLineWindow();
                                    showWindow.ShowDialog(ruleOrderLineWindow);

                                    return true;
                                }
                                else
                                    return false;

                        }
                        
                    }                   

                    return false;
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
            return false;
        }


    }
}
