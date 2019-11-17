using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using STUDENTU_1._06.Views.EditOrderWindows;
using System.Collections.ObjectModel;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Views.EditOrderWindows.Evaluation;
using STUDENTU_1._06.Model.DBModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;

namespace STUDENTU_1._06.ViewModel
{
    public  class RuleOrderLine : Helpes.ObservableObject
    {

        //authors collection
        public ObservableCollection<AuthorsRecord> AuthorsRecords { get; set; }
        //SelectedAuthors collection
        public ObservableCollection<AuthorsRecord> SelectedAuthorsRecords { get; set; }

        IDialogService dialogService;
        IShowWindowService showWindow;


        public RuleOrderLine( )
        {
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();           

            AuthorsRecords = new ObservableCollection<AuthorsRecord>();
            SelectedAuthorsRecords = new ObservableCollection<AuthorsRecord>();
            
            AuthorsRecord = new AuthorsRecord();
          //  SelectetdAuthorContacts = new Contacts();
            _Dir = new _Direction();
            _Evaluation = new _Evaluation();
            Order = new OrderLine();
            if (TMPStaticClass.CurrentOrder != null)
            {
                Order = TMPStaticClass.CurrentOrder;
                Order.DescriptionForClient = "Вариант(ы): " + Order.Variant + ". "  + Order.DescriptionForClient+
                    "\n\nСрок выполнения: "+Order.Dates.DeadLine.ToOADate();
            }
            ExecuteAuthor = new AuthorsRecord();
            ExecuteAuthor.Persone.NickName = "не задан";

            TMPDate = DateTime.Now;

            SelectedExecuteAuthor = new Author();
                        
        }

        DateTime TMPDate;

        private AuthorsRecord authorsRecord;
        public AuthorsRecord AuthorsRecord
        {
            get { return authorsRecord; }
            set
            {
                if (authorsRecord != value)
                {
                    authorsRecord = value;
                    OnPropertyChanged(nameof(AuthorsRecord));
                }
            }
        }

        private Author selectedExecuteAuthor;
        public Author SelectedExecuteAuthor
        {
            get { return selectedExecuteAuthor; }
            set
            {
                if (selectedExecuteAuthor != value)
                {
                    selectedExecuteAuthor = value;
                    OnPropertyChanged(nameof(SelectedExecuteAuthor));
                }
            }
        }


        
        private AuthorsRecord executeAuthor;
        public AuthorsRecord ExecuteAuthor
        {
            get { return executeAuthor; }
            set
            {
                if (executeAuthor != value)
                {
                    executeAuthor = value;
                    OnPropertyChanged(nameof(ExecuteAuthor));
                }
            }
        }

        //for add evaluation records into Orderline table
        private OrderLine order;
        public OrderLine Order
        {
            get { return order; }
            set
            {
                if (order != value)
                {
                    order = value;
                    OnPropertyChanged(nameof(Order));
                }
            }
        }

       

        private _Direction  _dir;
        public _Direction _Dir
        {
            get { return _dir; }
            set
            {
                if (_dir != value)
                {
                    _dir = value;
                    OnPropertyChanged(nameof(_Dir));
                }
            }
        }

        private _Evaluation _evaluation;
        public _Evaluation _Evaluation
        {
            get { return _evaluation; }
            set
            {
                if (_evaluation != value)
                {
                    _evaluation = value;
                    OnPropertyChanged(nameof(_Evaluation));
                }
            }
        }


        //call RuleOrderLineWindow
        private RelayCommand newRuleOrderLineWindowCommand;
        public RelayCommand NewRuleOrderLineWindowCommand =>
            newRuleOrderLineWindowCommand ?? (newRuleOrderLineWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        RuleOrderLineWindow ruleOrderLineWindow = new RuleOrderLineWindow();
                        ruleOrderLineWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(ruleOrderLineWindow);
                        
                    }
                    ));

        //=============================fill listbox "Authors" if check "All authors"====================


        private RelayCommand allAuthorsCallCommand;
        public RelayCommand AllAuthorsCallCommand =>
            allAuthorsCallCommand ?? (allAuthorsCallCommand = new RelayCommand(
                    (obj) =>
                    {

                        AuthorsRecords.Clear();
                        AuthorsCall("AllAuthors");
                    }
                    ));

        //=============================fill listbox "Authors" if check "Authors by direction"====================
        private RelayCommand themAuthorsCallCommand;
        public RelayCommand ThemAuthorsCallCommand =>
            themAuthorsCallCommand ?? (themAuthorsCallCommand = new RelayCommand(
                    (obj) =>
                    {

                        AuthorsRecords.Clear();
                        AuthorsCall("ThemAuthors");
                    }
                    ));

        private void AuthorsCall(string param)
        {
            if (_Dir.Dir.DirectionName == "---")
                param = "AllAuthors";
            switch (param)
            {
                case "AllAuthors":
                    AllAuthorsCall();
                    break;
                case "ThemAuthors":
                    ThemAuthorsCall();
                    break;
            }
        }

        //call for all authors
        private void AllAuthorsCall()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var contacts = db.Contacts.Include("Persone").ToList();
                    var result = db.Authors.Include("Persone").ToList();
                    AuthorsRecord record;
                    foreach (Author item in result)
                    {
                        if (item.AuthorStatus.AuthorStatusName != "работает")
                            continue;
                        Author author = new Author()
                        {
                            AuthorId = item.AuthorId,
                            AuthorStatus= item.AuthorStatus,
                            Source = item.Source,
                            Rating = item.Rating,
                            Punctually = item.Punctually,
                            CompletionCompliance = item.CompletionCompliance,
                            WorkQuality = item.WorkQuality,
                            Responsibility = item.Responsibility
                        };                        
                        Persone persone = new Persone()
                        {
                            PersoneId = item.Persone.PersoneId,
                            PersoneDescription = item.Persone.PersoneDescription,
                            Name = item.Persone.Name,
                            Surname = item.Persone.Surname,
                            Patronimic = item.Persone.Patronimic,
                            Sex = item.Persone.Sex,
                            NickName = item.Persone.NickName
                        };                        
                        Contacts _contacts = new Contacts()
                        {
                            Phone1 = item.Persone.Contacts.Phone1,
                            Phone2 = item.Persone.Contacts.Phone2,
                            Phone3 = item.Persone.Contacts.Phone3,
                            Email1 = item.Persone.Contacts.Email1,
                            Email2 = item.Persone.Contacts.Email2,
                            VK = item.Persone.Contacts.VK,
                            FaceBook = item.Persone.Contacts.FaceBook
                        };
                        record = new AuthorsRecord
                        {
                            Author = author,
                            Persone = persone,
                            Contacts = _contacts
                        };
                        AuthorsRecords.Add(record);
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

        //call for only authors by direction order
        private void ThemAuthorsCall()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var result1 = db.Authors.Include("Direction").ToList();
                    foreach (Author item in result1)
                    {
                        if (item.AuthorStatus.AuthorStatusName != "работает")
                            continue;
                        foreach (Direction i in item.Direction)
                            if (i.DirectionName == TMPStaticClass.CurrentOrder.Direction.DirectionName)
                            {
                                AuthorsRecord AuthorsRecordTMP = new AuthorsRecord()
                                {
                                    Author = item,
                                    Persone = new Persone()
                                    {
                                        PersoneId = item.Persone.PersoneId,
                                        PersoneDescription = item.Persone.PersoneDescription,
                                        Name = item.Persone.Name,
                                        Surname = item.Persone.Surname,
                                        Patronimic = item.Persone.Patronimic,
                                        Sex = item.Persone.Sex,
                                        NickName = item.Persone.NickName

                                    },
                                    Contacts = item.Persone.Contacts
                                };
                                SelectedAuthorsRecords.Add(AuthorsRecordTMP);
                            }
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


        //=============================Copy to ClipBoard commands================================
        private RelayCommand copyEmailToClipBoardCommand;
        public RelayCommand CopyEmailToClipBoardCommand =>
            copyEmailToClipBoardCommand ?? (copyEmailToClipBoardCommand = new RelayCommand(
                    (obj) =>
                    {
                        CopyEmailToClipBoard();
                    }
                    ));
        private void CopyEmailToClipBoard()
        {
            Clipboard.SetText($"{AuthorsRecord.Contacts.Email1},{AuthorsRecord.Contacts.Email2}");
        }

        private RelayCommand copyPhone1ToClipBoardCommand;
        public RelayCommand CopyPhone1ToClipBoardCommand =>
            copyPhone1ToClipBoardCommand ?? (copyPhone1ToClipBoardCommand = new RelayCommand(
                    (obj) =>
                    {
                        CopyPhone1ToClipBoard();
                    }
                    ));
        private void CopyPhone1ToClipBoard()
        {
            Clipboard.SetText($"{AuthorsRecord.Contacts.Phone1}");
        }

        private RelayCommand copyPhone2ToClipBoardCommand;
        public RelayCommand CopyPhone2ToClipBoardCommand =>
            copyPhone2ToClipBoardCommand ?? (copyPhone2ToClipBoardCommand = new RelayCommand(
                    (obj) =>
                    {
                        CopyPhone2ToClipBoard();
                    }
                    ));
        private void CopyPhone2ToClipBoard()
        {
            Clipboard.SetText($"{AuthorsRecord.Contacts.Phone2}");
        }

        private RelayCommand copyFBToClipBoardCommand;
        public RelayCommand CopyFBToClipBoardCommand =>
            copyFBToClipBoardCommand ?? (copyFBToClipBoardCommand = new RelayCommand(
                    (obj) =>
                    {
                        CopyFBToClipBoard();
                    }
                    ));
        private void CopyFBToClipBoard()
        {
            Clipboard.SetText($"{AuthorsRecord.Contacts.FaceBook}");
        }

        private RelayCommand copyVKToClipBoardCommand;
        public RelayCommand CopyVKToClipBoardCommand =>
            copyVKToClipBoardCommand ?? (copyVKToClipBoardCommand = new RelayCommand(
                    (obj) =>
                    {
                        CopyVKToClipBoard();
                    }
                    ));
        private void CopyVKToClipBoard()
        {
            Clipboard.SetText($"{AuthorsRecord.Contacts.VK}");
        }

        private RelayCommand copyToClipBoardCommand;
        public RelayCommand CopyToClipBoardCommand =>
            copyToClipBoardCommand ?? (copyToClipBoardCommand = new RelayCommand(
                    (obj) =>
                    {
                        CopyToClipBoard();
                    }
                    ));
        private void CopyToClipBoard()
        {
            Clipboard.SetText($"{AuthorsRecord.Contacts.VK}");
        }
        //==============================COMMAND TO ADD POTENTIAL AUTHORS TO LIST ================================

        //

        private RelayCommand addPotentialAuthorsCommand;
        public RelayCommand AddPotentialAuthorsCommand =>
            addPotentialAuthorsCommand ?? (addPotentialAuthorsCommand = new RelayCommand(
                    (obj) =>
                    {
                        
                    }
                    ));

        

        //==============================COMMAND TO FIND AUTHOR BY NICKNAME ================================        

        private RelayCommand findAuthorByNickCommand;
        public RelayCommand FindAuthorByNickCommand =>
            findAuthorByNickCommand ?? (findAuthorByNickCommand = new RelayCommand(
                    (obj) =>
                    {
                        FindAuthorByNick(obj as string);
                    }
                    ));
        private void FindAuthorByNick(string nick)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    if (nick != null || nick != " " || nick != "")
                    {
                        AuthorsRecord record=null;
                        var res = db.Authors.Include("Persone").ToList();
                        foreach (Author item in res)
                            if (item.Persone.NickName == nick)
                            {
                                Author author = new Author()
                                {
                                    AuthorId = item.AuthorId,                                  
                                    Source = item.Source,
                                    Rating = item.Rating,
                                    Punctually = item.Punctually,
                                    CompletionCompliance = item.CompletionCompliance,
                                    WorkQuality = item.WorkQuality,
                                    Responsibility = item.Responsibility
                                };
                                Persone persone = new Persone()
                                {
                                    PersoneId = item.Persone.PersoneId,
                                    PersoneDescription = item.Persone.PersoneDescription,
                                    Name = item.Persone.Name,
                                    Surname = item.Persone.Surname,
                                    Patronimic = item.Persone.Patronimic,
                                    Sex = item.Persone.Sex,
                                    NickName = item.Persone.NickName
                                };
                                Contacts _contacts = new Contacts()
                                {
                                    Phone1 = item.Persone.Contacts.Phone1,
                                    Phone2 = item.Persone.Contacts.Phone2,
                                    Phone3 = item.Persone.Contacts.Phone3,
                                    Email1 = item.Persone.Contacts.Email1,
                                    Email2 = item.Persone.Contacts.Email2,
                                    VK = item.Persone.Contacts.VK,
                                    FaceBook = item.Persone.Contacts.FaceBook
                                };
                                record = new AuthorsRecord
                                {
                                    Author = author,
                                    Persone = persone,
                                    Contacts = _contacts
                                };
                                SelectedAuthorsRecords.Add(record);                                
                                break;
                            }
                        if (record==null)                        
                            dialogService.ShowMessage("Нет автора с таким ником");
                        else
                            dialogService.ShowMessage("Совпадение найдено.\nЗапись об авторе добавлена в писок");
                    }
                    else
                        dialogService.ShowMessage("Поле ника автора не должно быть пустым");                  
                  
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

        

        //=============================fill listbox "AuthorsAvaluat" if press button "+"====================
        private RelayCommand addSelectedAuthorCommand;
        public RelayCommand AddSelectedAuthorCommand =>
            addSelectedAuthorCommand ?? (addSelectedAuthorCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddSelectedAuthor();
                    }
                    ));
        private void AddSelectedAuthor()
        {
            if (AuthorsRecord.Persone.NickName != "---")           
                SelectedAuthorsRecords.Add(AuthorsRecord);         
            else           
                dialogService.ShowMessage("Нельзя добавить эту запись");
        }

        //=============================edit listbox "AuthorsAvaluat" if press button "-"====================
        private RelayCommand delSelectedAuthorCommand;
        public RelayCommand DelSelectedAuthorCommand =>
            delSelectedAuthorCommand ?? (delSelectedAuthorCommand = new RelayCommand(
                    (obj) =>
                    {
                        DelSelectedAuthor();
                    }
                    ));

        private void DelSelectedAuthor()
        {
            if(SelectedAuthorsRecords.Count()!=0)
             SelectedAuthorsRecords.Remove(AuthorsRecord);
            else
                dialogService.ShowMessage("Нечего уже ничего удалять");
        }

        //=============================Call window SetFinalAvaluationWindow.xaml========================================

        private RelayCommand setExecuteAuthorCommand;
        public RelayCommand SetExecuteAuthorCommand =>
            setExecuteAuthorCommand ?? (setExecuteAuthorCommand = new RelayCommand(
                    (obj) =>
                    {
                        SetFinalAvaluationWindow setFinalAvaluationWindow = new SetFinalAvaluationWindow(obj);                        
                        showWindow.ShowDialog(setFinalAvaluationWindow);                        
                    }
                    ));

        //===================================== For save evaluate order any author in EditAvaluatonWindow.xaml====================
        private RelayCommand saveAuthorEvaluateAuthorRecordCommand;
        public RelayCommand SaveAuthorEvaluateAuthorRecordCommand =>
                            saveAuthorEvaluateAuthorRecordCommand ??
                            (saveAuthorEvaluateAuthorRecordCommand = new RelayCommand(
                    (obj) =>
                    {
                        SaveAuthorEvaluateAuthorRecord();
                    }
                    ));

        private void SaveAuthorEvaluateAuthorRecord()
        {
            
            //check for the entry before adding
            if (AuthorsRecord.EvaluationRecords.Count() > 0)
                
                foreach (var item in AuthorsRecord.EvaluationRecords)
                    if (item.DeadLine == _Evaluation.EvaluationRecord.DeadLine &&
                        item.Price == _Evaluation.EvaluationRecord.Price &&
                        item.EvaluateDescription == _Evaluation.EvaluationRecord.EvaluateDescription)
                        dialogService.ShowMessage("Уже есть запись с такой оценкой");
                    else
                    {
                        AuthorsRecord.EvaluationRecords.Add(_Evaluation.EvaluationRecord);
                        dialogService.ShowMessage("Данные сохранены");                        
                        //_Evaluation.EvaluationRecord = new EvaluationRecord() { DeadLine = TMPDate };
                        _Evaluation.EvaluationRecord = new EvaluationRecord();
                        break;
                    }            
            else
            {
               AuthorsRecord.EvaluationRecords.Add(_Evaluation.EvaluationRecord);
                dialogService.ShowMessage("Данные сохранены");
                //_Evaluation.EvaluationRecord = new EvaluationRecord() { DeadLine = TMPDate };
                _Evaluation.EvaluationRecord = new EvaluationRecord();
            }
        }

        //===================================== For delete evaluate selected author in EditAvaluationWindow.xaml====================
        private RelayCommand deleteSelectedAvaluateCommand;
        public RelayCommand DeleteSelectedAvaluateCommand =>
                            deleteSelectedAvaluateCommand ??
                            (deleteSelectedAvaluateCommand = new RelayCommand(
                    (obj) =>
                    {                       
                        DeleteSelectedAvaluate(obj as EvaluationRecord);
                    }
                    ));

        private void DeleteSelectedAvaluate(EvaluationRecord i)
        {
            AuthorsRecord.EvaluationRecords.Remove(i);
        }

        //==================================== COMMAND FOR ADD EVALUATION TO SELECTED AUTHOR ====================================

        private RelayCommand addEvaluationToAuthorCommand;
        public RelayCommand AddEvaluationToAuthorCommand => addEvaluationToAuthorCommand ?? (addEvaluationToAuthorCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddEvaluationToAuthor();
                    }
                    ));

        private void AddEvaluationToAuthor()
        {
            if (AuthorsRecord.EvaluationRecords.Count() != 0)
            {
                //добавляем к выбранным авторам оценки
                //add evaluations to selected authors
                foreach (var item in SelectedAuthorsRecords)
                    if (item.Author.AuthorId == AuthorsRecord.Author.AuthorId)
                    {
                        foreach (var i in AuthorsRecord.EvaluationRecords)
                        {
                            item.EvaluationRecords.Add(i);
                        }
                        dialogService.ShowMessage("Оценка добавлена");
                        break;
                    }
            }
            else
                dialogService.ShowMessage("Нечего добавлять");

        }

        //===================================== For Cancel save evaluate order any author in EditAvaluatonWindow.xaml====================
        private RelayCommand cancelEvaluateCommand;
        public RelayCommand CancelEvaluateCommand =>
            cancelEvaluateCommand ?? (cancelEvaluateCommand = new RelayCommand(
                    (obj) =>
                    {
                        CancelAuthorEvaluate();
                        Window window = obj as Window;
                        window.Close();
                    }
                    ));

        private void CancelAuthorEvaluate()
        {
            AuthorsRecord = new AuthorsRecord();
            _Evaluation.EvaluationRecord = new EvaluationRecord() { DeadLine = DateTime.Now };
        }



    }
}
