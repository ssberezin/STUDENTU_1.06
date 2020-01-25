using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views;
using STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows;
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

        //флаг для отслеживания сохранения результатов сравнения двух объектов Contscts (при приеме заказа, например)
        // flag for tracking the saving of the results of comparing two Contscts objects (when receiving an order, for example)
        public bool saveCompareResults = false;//не востребован

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
        
        private Contacts tmpContactsCompare;
        public Contacts TmpContactsCompare
        {
            get { return tmpContactsCompare; }
            set
            {
                if (tmpContactsCompare != value)
                {
                    tmpContactsCompare = value;
                    OnPropertyChanged(nameof(TmpContactsCompare));
                }
            }
        }

        private Contacts oldTmpContactsCompare;
        public Contacts OldTmpContactsCompare
        {
            get { return oldTmpContactsCompare; }
            set
            {
                if (oldTmpContactsCompare != value)
                {
                    oldTmpContactsCompare = value;
                    OnPropertyChanged(nameof(OldTmpContactsCompare));
                }
            }
        }

        private Persone oldPersoneCompare;
        public Persone OldPersoneCompare
        {
            get { return oldPersoneCompare; }
            set
            {
                if (oldPersoneCompare != value)
                {
                    oldPersoneCompare = value;
                    OnPropertyChanged(nameof(OldPersoneCompare));
                }
            }
        }

        private Persone curPersoneCompare;
        public Persone CurPersoneCompare
        {
            get { return curPersoneCompare; }
            set
            {
                if (curPersoneCompare != value)
                {
                    curPersoneCompare = value;
                    OnPropertyChanged(nameof(CurPersoneCompare));
                }
            }
        }

        private Persone tmpPersoneCompare;
        public Persone TMPPersoneCompare
        {
            get { return tmpPersoneCompare; }
            set
            {
                if (tmpPersoneCompare != value)
                {
                    tmpPersoneCompare = value;
                    OnPropertyChanged(nameof(TMPPersoneCompare));
                }
            }
        }

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

        public _Contacts()
        {
           Contacts = new Contacts();
            CurPersoneCompare = new Persone();
            TmpContacts = new Contacts();
            TmpContactsCompare = new Contacts();
            TMPPersoneCompare = new Persone();
            OldTmpContactsCompare = new Contacts();
            OldPersoneCompare = new Persone();
            Persone = new Persone();
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
        }

        //====================================Save contact COMMAND================================

        private RelayCommand saveContactCommand;
        public RelayCommand SaveContactCommand => saveContactCommand ?? (saveContactCommand = new RelayCommand(
                    (obj) =>
                    {
                        PersonAddContacts();                      
                    }
                    ));
        public void PersonAddContacts()
        {
            Author author = CheckAuthorContacts();
            if (author == null)
            {
                Contacts = TmpContacts;
                dialogService.ShowMessage("Данные сохранены");
            }
            else
                if (dialogService.YesNoDialog("Контактные данные этого автора уже есть в БД авторов\n" +
                                "Показать информацию по найденному совпадению?"))
            {
                AuthorInfo AuthorInfoWindow = new AuthorInfo(author);
                showWindow.ShowDialog(AuthorInfoWindow);
                Contacts = TmpContacts;
            }
        }

        //=========================================COMMAND FOR CANCEL SAVE CONTACTS=========================================== 
        public bool compareContacts = false;
        private RelayCommand cancelSaveContactsCommand;
        public RelayCommand CancelSaveContactsCommand => cancelSaveContactsCommand ??
            (cancelSaveContactsCommand = new RelayCommand(
                    (obj) =>
                    {
                        //CancelSaveContacts();
                        Window window = obj as Window;
                        window.Close();
                    }
                    ));
        private void CancelSaveContacts()
        {
            if (!compareContacts)
                TmpContacts = Contacts;
            else
            {                
                Contacts = TmpContacts;
                compareContacts = false;
            }
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }

        
        public void NewEditContacts(Window addContactsWndow)
        {
            showWindow.ShowDialog(addContactsWndow);
        }


        //load data array from "Directions" table
        public Author CheckAuthorContacts()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {

                    var list = db.Authors.Include("Persone").ToList();
                    foreach (var item in list)
                    {
                        if (TmpContacts.Phone1 != "+380" && item.Persone.Contacts.Phone1 == TmpContacts.Phone1 ||
                           TmpContacts.Phone2 != "---" && item.Persone.Contacts.Phone1 == TmpContacts.Phone2 ||
                           TmpContacts.Phone3 != "---" && item.Persone.Contacts.Phone1 == TmpContacts.Phone3 ||
                           TmpContacts.Phone2 != "---" && item.Persone.Contacts.Phone2 == TmpContacts.Phone2 ||
                           TmpContacts.Phone3 != "---" && item.Persone.Contacts.Phone2 == TmpContacts.Phone3 ||
                           TmpContacts.Phone1 != "---" && item.Persone.Contacts.Phone3 == TmpContacts.Phone1 ||
                           TmpContacts.Email1 != "---" && item.Persone.Contacts.Email1 == TmpContacts.Email1 ||
                           TmpContacts.Email2 != "---" && item.Persone.Contacts.Email1 == TmpContacts.Email2 ||
                           TmpContacts.Email2 != "---" && item.Persone.Contacts.Email2 == TmpContacts.Email2 ||
                           TmpContacts.VK != "---" && item.Persone.Contacts.VK == TmpContacts.VK ||
                           TmpContacts.Skype != "---" && item.Persone.Contacts.Skype == TmpContacts.Skype)

                            return item ;
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

        //===============================================PERSONE DATA RAPLACE COMMANDS ===========================================



        private RelayCommand replaceOldToNewNameCommand;
        public RelayCommand ReplaceOldToNewNameCommand => replaceOldToNewNameCommand ??
                            (replaceOldToNewNameCommand = new RelayCommand(
                    (obj) =>
                    {
                        OldPersoneCompare.Name = CurPersoneCompare.Name;                       
                    }
                    ));
        

        private RelayCommand replaceNewToOldNameCommand;
        public RelayCommand ReplaceNewToOldNameCommand => replaceNewToOldNameCommand ??
                            (replaceNewToOldNameCommand = new RelayCommand(
                    (obj) =>
                    {
                        CurPersoneCompare.Name = OldPersoneCompare.Name;
                    }
                    ));

        private RelayCommand replaceOldToNewSurNameCommand;
        public RelayCommand ReplaceOldToNewSurNameCommand => replaceOldToNewSurNameCommand ??
                            (replaceOldToNewSurNameCommand = new RelayCommand(
                    (obj) =>
                    {
                        OldPersoneCompare.Surname = CurPersoneCompare.Surname;
                    }
                    ));
        private RelayCommand replaceNewToOldSurNameCommand;
        public RelayCommand ReplaceNewToOldSurNameCommand => replaceNewToOldSurNameCommand ??
                            (replaceNewToOldSurNameCommand = new RelayCommand(
                    (obj) =>
                    {
                        CurPersoneCompare.Surname = OldPersoneCompare.Surname;
                    }
                    ));
        private RelayCommand replaceOldToNewPatronimicCommand;
        public RelayCommand ReplaceOldToNewPatronimicCommand => replaceOldToNewPatronimicCommand ??
                            (replaceOldToNewPatronimicCommand = new RelayCommand(
                    (obj) =>
                    {
                        OldPersoneCompare.Patronimic = CurPersoneCompare.Patronimic;
                    }
                    ));
        private RelayCommand replaceNewToOldPatronimicCommand;
        public RelayCommand ReplaceNewToOldPatronimicCommand => replaceNewToOldPatronimicCommand ??
                            (replaceNewToOldPatronimicCommand = new RelayCommand(
                    (obj) =>
                    {
                        CurPersoneCompare.Patronimic = OldPersoneCompare.Patronimic;
                    }
                    ));

        //===============================================CONTACTS RAPLACE COMMANDS ===========================================



        private RelayCommand replaceOldToNewPhone1Command;
        public RelayCommand ReplaceOldToNewPhone1Command => replaceOldToNewPhone1Command ??
                            (replaceOldToNewPhone1Command = new RelayCommand(
                    (obj) =>
                    {
                        //Contacts.Phone1=ReplaceContacts("Phone1","OldToNew", Contacts, TmpContacts );
                        OldTmpContactsCompare.Phone1 = TmpContactsCompare.Phone1;
                    }
                    ));

        private RelayCommand replaceNewToOldPhone1Command;
        public RelayCommand ReplaceNewToOldPhone1Command => replaceNewToOldPhone1Command ??
                            (replaceNewToOldPhone1Command = new RelayCommand(
                    (obj) =>
                    {
                        TmpContactsCompare.Phone1 = OldTmpContactsCompare.Phone1;
                    }
                    ));

        private RelayCommand replaceOldToNewPhone2Command;
        public RelayCommand ReplaceOldToNewPhone2Command => replaceOldToNewPhone2Command ??
                            (replaceOldToNewPhone2Command = new RelayCommand(
                    (obj) =>
                    {
                        OldTmpContactsCompare.Phone2 = TmpContactsCompare.Phone2;
                    }
                    ));

        private RelayCommand replaceNewToOldPhone2Command;
        public RelayCommand ReplaceNewToOldPhone2Command => replaceNewToOldPhone2Command ??
                            (replaceNewToOldPhone2Command = new RelayCommand(
                    (obj) =>
                    {
                        TmpContactsCompare.Phone2 = OldTmpContactsCompare.Phone2;
                    }
                    ));

        private RelayCommand replaceOldToNewPhone3Command;
        public RelayCommand ReplaceOldToNewPhone3Command => replaceOldToNewPhone3Command ??
                            (replaceOldToNewPhone3Command = new RelayCommand(
                    (obj) =>
                    {
                        OldTmpContactsCompare.Phone3 = TmpContactsCompare.Phone3;
                    }
                    ));

        private RelayCommand replaceNewToOldPhone3Command;
        public RelayCommand ReplaceNewToOldPhone3Command => replaceNewToOldPhone3Command ??
                            (replaceNewToOldPhone3Command = new RelayCommand(
                    (obj) =>
                    {
                        TmpContactsCompare.Phone3 = OldTmpContactsCompare.Phone3;
                    }
                    ));

        private RelayCommand replaceOldToNewEmail1Command;
        public RelayCommand ReplaceOldToNewEmail1Command => replaceOldToNewEmail1Command ??
                            (replaceOldToNewEmail1Command = new RelayCommand(
                    (obj) =>
                    {
                        OldTmpContactsCompare.Email1 = TmpContactsCompare.Email1;
                    }
                    ));

        private RelayCommand replaceNewToOldEmail1Command;
        public RelayCommand ReplaceNewToOldEmail1Command => replaceNewToOldEmail1Command ??
                            (replaceNewToOldEmail1Command = new RelayCommand(
                    (obj) =>
                    {
                        TmpContactsCompare.Email1 = OldTmpContactsCompare.Email1;
                    }
                    ));

        private RelayCommand replaceOldToNewEmail2Command;
        public RelayCommand ReplaceOldToNewEmail2Command => replaceOldToNewEmail2Command ??
                            (replaceOldToNewEmail2Command = new RelayCommand(
                    (obj) =>
                    {
                        OldTmpContactsCompare.Email2 = TmpContactsCompare.Email2;
                    }
                    ));

        private RelayCommand replaceNewToOldEmail2Command;
        public RelayCommand ReplaceNewToOldEmail2Command => replaceNewToOldEmail2Command ??
                            (replaceNewToOldEmail2Command = new RelayCommand(
                    (obj) =>
                    {
                        TmpContactsCompare.Email2 = OldTmpContactsCompare.Email2;
                    }
                    ));

        private RelayCommand replaceOldToNewVKCommand;
        public RelayCommand ReplaceOldToNewVKCommand => replaceOldToNewVKCommand ??
                            (replaceOldToNewVKCommand = new RelayCommand(
                    (obj) =>
                    {
                        OldTmpContactsCompare.VK = TmpContactsCompare.VK;
                    }
                    ));

        private RelayCommand replaceNewToOldVKCommand;
        public RelayCommand ReplaceNewToOldVKCommand => replaceNewToOldVKCommand ??
                            (replaceNewToOldVKCommand = new RelayCommand(
                    (obj) =>
                    {
                        TmpContactsCompare.VK = OldTmpContactsCompare.VK;
                    }
                    ));

        private RelayCommand replaceOldToNewFBCommand;
        public RelayCommand ReplaceOldToNewFBCommand => replaceOldToNewFBCommand ??
                            (replaceOldToNewFBCommand = new RelayCommand(
                    (obj) =>
                    {
                        OldTmpContactsCompare.FaceBook = TmpContactsCompare.FaceBook;
                    }
                    ));

        private RelayCommand replaceNewToOldFBCommand;
        public RelayCommand ReplaceNewToOldFBCommand => replaceNewToOldFBCommand ??
                            (replaceNewToOldFBCommand = new RelayCommand(
                    (obj) =>
                    {
                        TmpContactsCompare.FaceBook = OldTmpContactsCompare.FaceBook;
                    }
                    ));

        private RelayCommand replaceOldToNewSkypeCommand;
        public RelayCommand ReplaceOldToNewSkypeCommand => replaceOldToNewSkypeCommand ??
                            (replaceOldToNewSkypeCommand = new RelayCommand(
                    (obj) =>
                    {
                        OldTmpContactsCompare.Skype = TmpContactsCompare.Skype;
                    }
                    ));

        private RelayCommand replaceNewToOldSkypeCommand;
        public RelayCommand ReplaceNewToOldSkypeCommand => replaceNewToOldSkypeCommand ??
                            (replaceNewToOldSkypeCommand = new RelayCommand(
                    (obj) =>
                    {
                        TmpContactsCompare.Skype = OldTmpContactsCompare.Skype;
                    }
                    ));

        private RelayCommand setLeftContactsCommand;
        public RelayCommand SetLeftContactsCommand => setLeftContactsCommand ??
                            (setLeftContactsCommand = new RelayCommand(
                    (obj) =>
                    {
                        Contacts = (Contacts)this.OldTmpContactsCompare.CloneExceptVirtual();

                        Persone.Name = this.OldPersoneCompare.Name;
                        Persone.Surname = this.OldPersoneCompare.Surname;
                        Persone.Patronimic = this.OldPersoneCompare.Patronimic;
                        Persone.Sex = this.OldPersoneCompare.Sex;
                        saveCompareResults = true;//этот маркер пока не востребован
                        CloseWindow(obj as Window);
                    }
                    ));


        private RelayCommand setRightContactsCommand;
        public RelayCommand SetRightContactsCommand => setRightContactsCommand ??
                            (setRightContactsCommand = new RelayCommand(
                    (obj) =>
                    {
                        SetRightContacts(obj as Window);
                    }
                    ));
        private void SetRightContacts(Window window)
        {
            Contacts = (Contacts)this.TmpContactsCompare.CloneExceptVirtual();

            Persone.Name = this.CurPersoneCompare.Name;
            Persone.Surname = this.CurPersoneCompare.Surname;
            Persone.Patronimic = this.CurPersoneCompare.Patronimic;
            Persone.Sex = this.CurPersoneCompare.Sex;
            saveCompareResults = true;//этот маркер пока не востребован
            CloseWindow(window);
        }

        public bool CompareContacts (Contacts obj1, Contacts obj2)
        {
            if ((obj1.Phone1 == obj2.Phone1) && (obj1.Phone2 == obj2.Phone2) &&
                (obj1.Phone3 == obj2.Phone3) && (obj1.Email1 == obj2.Email1) &&
                (obj1.Email2 == obj2.Email2) && (obj1.VK == obj2.VK) &&
                (obj1.FaceBook == obj2.FaceBook) && (obj1.Skype == obj2.Skype))
                return true;
            return false;
        }

        //public static bool operator ==(Contacts obj1, Contacts obj2)
        //{


        //    if ((obj1.Phone1 == obj2.Phone1) && (obj1.Phone2 == obj2.Phone2) &&
        //        (obj1.Phone3 == obj2.Phone3) && (obj1.Email1 == obj2.Email1) &&
        //        (obj1.Email2 == obj2.Email2) && (obj1.VK == obj2.VK) &&
        //        (obj1.FaceBook == obj2.FaceBook) && (obj1.Skype == obj2.Skype))
        //        return true;
        //    return false;
        //}

        //public static bool operator !=(Contacts obj1, Contacts obj2)
        //{

        //    if ((obj1.Phone1 != obj2.Phone1) || (obj1.Phone2 != obj2.Phone2) ||
        //        (obj1.Phone3 != obj2.Phone3) || (obj1.Email1 != obj2.Email1) ||
        //        (obj1.Email2 != obj2.Email2) || (obj1.VK != obj2.VK) ||
        //        (obj1.FaceBook != obj2.FaceBook) || (obj1.Skype != obj2.Skype))
        //        return true;
        //    return false;
        //}

        


    }
}
