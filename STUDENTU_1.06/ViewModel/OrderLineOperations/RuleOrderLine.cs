using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Views.EditOrderWindows.Evaluation;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views.EditOrderWindows.RuleOrderLineWindows;
using System.Data.Entity;
using System.ComponentModel;
using STUDENTU_1._06.Views.EditOrderWindows;

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


        public RuleOrderLine()
        {
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();           

            AuthorsRecords = new ObservableCollection<AuthorsRecord>();
            SelectedAuthorsRecords = new ObservableCollection<AuthorsRecord>();
            
            AuthorsRecord = new AuthorsRecord();
            _AuthorStatus = new _AuthorStatus();           
            _Dir = new _Direction();
            _Evaluation = new _Evaluation();
            Order = new OrderLine();
            _Subject = new _Subject();
            _Status = new _Status();
            if (TMPStaticClass.CurrentOrder != null)
            {
                //Order = TMPStaticClass.CurrentOrder;
                Order = (OrderLine)TMPStaticClass.CurrentOrder.Clone();
                Order.DescriptionForClient = "Вариант(ы): " + CheckForEmpty(Order.Variant) + ". \n"  + Order.DescriptionForClient+
                    "\n\nСрок выполнения: "+Order.Dates.AuthorDeadLine.ToShortDateString() + " или свой вариант. "+
                    "\n Время: к " + Order.Dates.AuthorDeadLine.ToShortTimeString()+" или свой вариант. ";
            }
            ExecuteAuthor = new AuthorsRecord();
            ExecuteAuthor.Persone.NickName = "не задан";
            SelectedExecuteAuthor = new Author();

            FillAuthorsRecords();
           // PropertyChanged += Change_AuthorsRecord;
        }

        public RuleOrderLine(string str)
        {
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
            AuthorsRecords = new ObservableCollection<AuthorsRecord>();
            AuthorsRecord = new AuthorsRecord();
            _AuthorStatus = new _AuthorStatus();
            _Dir = new _Direction();
            _Subject = new _Subject();

            FillAuthorsRecords();
        }

        //проверяем не пустое ли поле с вариантами. Возращает "не задано" если пустое. Если не пустое - возвращет исходное значение
        // check if the field with options is empty. Returns "not set" if empty. If not empty, returns the original value.
        private string CheckForEmpty(string str)
        {
            if (str==null ||str==" ")
                return "не задано";
            str.Trim();
            if (str[0] == ' '|| str=="")           
                return "не задано";           
            return str;            
        }      

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

        //for show authorstatus in Complicated filter
        private _AuthorStatus _authorStatus;
        public _AuthorStatus _AuthorStatus
        {
            get { return _authorStatus; }
            set
            {
                if (_authorStatus != value)
                {
                    _authorStatus = value;
                    OnPropertyChanged(nameof(_AuthorStatus));
                }
            }
        }

        //для получения значения индекса выбранной записи в массиве оценок автора
        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                if (index != value)
                {
                    index = value;
                    OnPropertyChanged(nameof(Index));
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

        //for show subject in Complicated filter
        private _Subject _subject;
        public _Subject _Subject
        {
            get { return _subject; }
            set
            {
                if (_subject != value)
                {
                    _subject = value;
                    OnPropertyChanged(nameof(_Subject));
                }
            }
        }

        //for oreder status show
        private _Status _status;
        public _Status _Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(_Status));
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

        //==================================================EDIT AUTHOR EVALUATION COMMAND =============================================
        private RelayCommand setAuthorAvaluationCommand;
        public RelayCommand SetAuthorAvaluationCommand => setAuthorAvaluationCommand ?? (setAuthorAvaluationCommand = new RelayCommand(
                    (obj) =>
                    {
                        Change_AuthorsRecord();
                        EditAvaluationWindow editAvaluationWindow = new EditAvaluationWindow(this);
                        showWindow.ShowDialog(editAvaluationWindow);
                    }
                    ));

        //єта фича меняет значение AuthorsRecord для корректного отображения оценок в EditAvaluationWindow.xaml
        // This feature changes the value of AuthorsRecord to correctly display grades in EditAvaluationWindow.xaml
        private void Change_AuthorsRecord()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var order = db.Orderlines.Where(o => o.OrderLineId == TMPStaticClass.CurrentOrder.OrderLineId).FirstOrDefault();

                    foreach (var item in order.Evaluations)
                    {
                        foreach (var i in item.Authors)
                        {
                            if (i.AuthorId == AuthorsRecord.Author.AuthorId)
                            {
                                _Evaluation.EvaluationRecord = new EvaluationRecord()
                                {
                                    DeadLine = item.AuthorDeadLine,
                                    Price = item.AuthorPrice,
                                    EvaluateDescription = item.Description,
                                    FinalEvaluation = item.Winner
                                };
                                AuthorsRecord.EvaluationRecords.Add(_Evaluation.EvaluationRecord);
                            }
                        }

                    }
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (System.Data.Entity.Core.EntityException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //============================================================================================================================================

        //напоняем  SelectedAuthorsRecords записями, если заказ уже содержит какие-либо оценки от авторов
        // we make  SelectedAuthorsRecords records if the order already contains any ratings from the authors
        private void FillAuthorsRecords()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var order = db.Orderlines.Where(o=>o.OrderLineId==TMPStaticClass.CurrentOrder.OrderLineId).FirstOrDefault();
                    AuthorsRecord record;
                    //Evaluation evaluation;
                    foreach (var item in order.Author)
                    {
                        record = new AuthorsRecord
                        {
                            Author = item,
                            Persone = item.Persone,
                            Contacts = item.Persone.Contacts
                        };
                        SelectedAuthorsRecords.Add(record);
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
            
            if (Order.Direction.DirectionName == "---")
                param = "AllAuthors";
            switch (param)
            {
                case "AllAuthors":
                    AuthorsRecords.Clear();
                    AllAuthorsCall("Все работающие авторы");
                    break;
                case "ThemAuthors":
                    AuthorsRecords.Clear();
                    ThemAuthorsCall();
                    break;
            }
        }

        //call for all authors. If param=="all" et last we'll see all authors with any asuthorstatus
        //if  param=="all"  et last we'll see all authors with asuthorstatus "работает"
        private void AllAuthorsCall(string param)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var contacts = db.Contacts.Include("Persone").ToList();
                    var result = db.Authors.Include("Persone")
                                           .Include("AuthorStatus").ToList();
                    AuthorsRecord record;
                    foreach (Author item in result)
                    {
                        if (param!="all")
                        if (item.AuthorStatus.AuthorStatusName != "работает")
                            continue;
                        
                        record = new AuthorsRecord
                        {
                            Author = item,
                            Persone = item.Persone,
                            Contacts = item.Persone.Contacts
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
                                        Male = item.Persone.Male,
                                        Female=item.Persone.Female,
                                        NickName = item.Persone.NickName

                                    },
                                    Contacts = item.Persone.Contacts
                                };
                                AuthorsRecords.Add(AuthorsRecordTMP);
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

        public void AuthorsCallByParams(string dir, string subj, string authorStatus, ObservableCollection<AuthorsRecord> authorsRecords)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var contacts = db.Contacts.Include("Persone").ToList();
                    var result = db.Authors.Include("Persone")
                                           .Include("Subject")
                                           .Include("AuthorStatus").ToList();
                    AuthorsRecord record;


                    //при фильтрации было принято решение не экономить память машины, но сэкономить ее вычислительные ресурсы
                    // when filtering, it was decided not to save machine memory, but to save its computing resources
                    List<Author> tmpres = new List<Author>();
                    //filtered authors by status
                    if (authorStatus != "---"&& authorStatus!=null)
                        foreach (Author item in result)
                        {
                            if (item.AuthorStatus.AuthorStatusName == authorStatus)
                                tmpres.Add(item);
                        }
                    else
                        tmpres.AddRange(result);
                    //foreach (Author item in tmpres)
                    //        tmpres.Add(item);

                    List<Author> tmpres1 = new List<Author>();
                    //filtered authors by subject

                    if (subj != "---" && subj!=null)
                        foreach (Author item in tmpres)
                        {
                            foreach (var i in item.Subject)
                                if (i.SubName == subj)
                                    tmpres1.Add(item);
                        }
                    else
                        tmpres1.AddRange(tmpres);
                    tmpres = null;
                    List<Author> tmpres2 = new List<Author>();
                    //filtered authors by subject
                    if (dir != "---" && dir!=null)
                        foreach (Author item in tmpres1)
                        {
                            foreach (var i in item.Direction)
                                if (i.DirectionName == dir)
                                    tmpres2.Add(item);
                        }
                    else
                        tmpres2.AddRange(tmpres1);
                    tmpres1 = null;

                    


                    foreach (Author item in tmpres2)
                    {

                       
                        record = new AuthorsRecord
                        {
                            Author = item,
                            Persone = item.Persone,
                            Contacts = item.Persone.Contacts
                        };
                        // AuthorsRecords.Add(record);
                        authorsRecords.Add(record);
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

        //=================Clear selected authors collection =================================================
        private RelayCommand clearSelectedAuthorsListCommand;
        public RelayCommand ClearSelectedAuthorsListCommand =>
            clearSelectedAuthorsListCommand ?? (clearSelectedAuthorsListCommand = new RelayCommand(
                    (obj) =>
                    {
                        ClearSelectedAuthorsList();
                    }
                    ));
        private void ClearSelectedAuthorsList()
        {
            SelectedAuthorsRecords.Clear();
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
            Clipboard.SetText($"{ Order.DescriptionForClient}");
        }


        //==============================COMMAND TO EDIT Order.DescriptionForClient BY EDIT DEADLINE ================================        
        private RelayCommand timePlusDefaultCommand;
        public RelayCommand TimePlusDefaultCommand =>
            timePlusDefaultCommand ?? (timePlusDefaultCommand = new RelayCommand(
                    (obj) =>
                    {
                        TimePlusDefault();
                    }
                    ));
        private void TimePlusDefault()
        {
            if (Order.WorkType.TypeOfWork == "чертежи")
            {
                dialogService.ShowMessage("На чертежи время с запасом поставить нельзя");
                return;
            }
            int tmp = Order.Dates.DeadLine.Day - DateTime.Now.Day;
            
            if (tmp>=7&&tmp<=10)
                Order.Dates.AuthorDeadLine.AddDays(-2);
            else
                if (tmp <7 && tmp>=4)
                Order.Dates.AuthorDeadLine.AddDays(-1);
            else
                if (tmp >11)
                Order.Dates.AuthorDeadLine.AddDays(-3);

        }

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
                                record = new AuthorsRecord
                                {
                                    Author = item,
                                    Persone = item.Persone,
                                    Contacts = item.Persone.Contacts
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
            if (AuthorsRecord.Persone.NickName == "---")
            {
                dialogService.ShowMessage("Нельзя добавить эту запись");
                return;
            }
            if(!SelectedAuthorsRecords.Contains(AuthorsRecord))            
                SelectedAuthorsRecords.Add(AuthorsRecord);         
            else
                dialogService.ShowMessage("Уже есть в списке выбранных авторов");
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
            if (SelectedAuthorsRecords.Count() != 0)
            {
                if (dialogService.YesNoDialog("Если удалить єтого автора из списка," +
                    " то записи о его оценках не сохранятся. Точно удалить?"))
                    SelectedAuthorsRecords.Remove(AuthorsRecord);
            }
            else
                dialogService.ShowMessage("Нечего уже  удалять");
        }

        //=============================Call window SetFinalAvaluationWindow.xaml========================================

        //private RelayCommand setExecuteAuthorCommand;
        //public RelayCommand SetExecuteAuthorCommand =>
        //    setExecuteAuthorCommand ?? (setExecuteAuthorCommand = new RelayCommand(
        //            (obj) =>
        //            {
        //                SetFinalAvaluationWindow setFinalAvaluationWindow = new SetFinalAvaluationWindow(obj);                        
        //                showWindow.ShowDialog(setFinalAvaluationWindow);                        
        //            }
        //            ));

        //ComplicatedFilterAuthorsparamWondow

        //=============================Call window ComplicatedFilterAuthorsparamWondow.xaml========================================

        private RelayCommand callAuthorsComlicatedFilterCommand;
        public RelayCommand CallAuthorsComlicatedFilterCommand =>
            callAuthorsComlicatedFilterCommand ?? (callAuthorsComlicatedFilterCommand = new RelayCommand(
                    (obj) =>
                    {
                        ComplicatedFilterAuthorsparamWondow window = new ComplicatedFilterAuthorsparamWondow(obj);
                        showWindow.ShowWindow(window);                       
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
            //есть подозрение, что эта фича работать будет через мат-перемат.

            //check for the entry before adding
            //if (AuthorsRecord.EvaluationRecords.Count() > 0)

            //{
                foreach (var item in AuthorsRecord.EvaluationRecords)
                {
                    if (item.DeadLine == _Evaluation.EvaluationRecord.DeadLine &&
                          item.Price == _Evaluation.EvaluationRecord.Price &&
                          item.EvaluateDescription == _Evaluation.EvaluationRecord.EvaluateDescription)
                    {
                        dialogService.ShowMessage("Уже есть запись с такой оценкой");
                        return;
                    }
                }
                AuthorsRecord.EvaluationRecords.Add(_Evaluation.EvaluationRecord);
                dialogService.ShowMessage("Данные сохранены");
                _Evaluation.EvaluationRecord = new EvaluationRecord()
                { DeadLine = TMPStaticClass.CurrentOrder.Dates.AuthorDeadLine };
            //}
            //else
            //{
            //    AuthorsRecord.EvaluationRecords.Add(_Evaluation.EvaluationRecord);
            //    dialogService.ShowMessage("Данные сохранены");
            //    _Evaluation.EvaluationRecord = new EvaluationRecord()
            //    { DeadLine = TMPStaticClass.CurrentOrder.Dates.AuthorDeadLine };
            //    //_Evaluation.EvaluationRecord = new EvaluationRecord();
            //}

            //using (StudentuConteiner db = new StudentuConteiner())
            //{
            //    try
            //    {
            //        Order = db.Orderlines.Where(o=>o.OrderLineId==Order.OrderLineId).FirstOrDefault();
            //        Author author = db.Authors.Where(a=>a.AuthorId==AuthorsRecord.Author.AuthorId).FirstOrDefault();
            //        Evaluation evaluation = new Evaluation()
            //        {
            //            AuthorPrice = _Evaluation.EvaluationRecord.Price,
            //            AuthorDeadLine = _Evaluation.EvaluationRecord.DeadLine,
            //            Description= _Evaluation.EvaluationRecord.EvaluateDescription,
            //            Winner= _Evaluation.EvaluationRecord.FinalEvaluation
            //        };
            //        foreach (var i in author.Evaluation)
            //            if (i.AuthorPrice == evaluation.AuthorPrice &&
            //                i.AuthorDeadLine == evaluation.AuthorDeadLine &&
            //                i.Description == evaluation.Description)
            //            {
            //                dialogService.ShowMessage("Уже есть такая оценка от этого автора");
            //                return;
            //            }
            //        //пишем в текущую коллекцию дабы не тащить потом из БД
            //        AuthorsRecord.EvaluationRecords.Add(_Evaluation.EvaluationRecord);                  
            //        _Evaluation.EvaluationRecord = new EvaluationRecord()
            //        { DeadLine = TMPStaticClass.CurrentOrder.Dates.AuthorDeadLine };


            //        //пишем в БД
            //        db.Entry(author).State = EntityState.Modified;
            //        db.Entry(Order).State = EntityState.Modified;

            //        db.Configuration.AutoDetectChangesEnabled = false;
            //        db.Configuration.ValidateOnSaveEnabled = false;

            //        evaluation.Authors.Add(author);
            //        evaluation.OrderLines.Add(Order);
            //        db.Evaluations.Add(evaluation);

            //        Order.Evaluations.Add(evaluation);


            //        bool existEvalInAuthor = false;
            //        foreach (var i in Order.Author)
            //            if (i.AuthorId == author.AuthorId)
            //            {
            //                i.Evaluation.Add(evaluation);
            //                existEvalInAuthor = true;
            //                break;
            //            }
            //        if (!existEvalInAuthor)
            //        {
            //            author.Evaluation.Add(evaluation);
            //            Order.Author.Add(author);
            //        }
            //        db.SaveChanges();
            //        dialogService.ShowMessage("Данные об оценке внесены в базу данных");



        //}
        //        catch (ArgumentNullException ex)
        //        {
        //            dialogService.ShowMessage(ex.Message);
        //        }
        //        catch (OverflowException ex)
        //        {
        //            dialogService.ShowMessage(ex.Message);
        //        }
        //        catch (System.Data.SqlClient.SqlException ex)
        //        {
        //            dialogService.ShowMessage(ex.Message);
        //        }
        //        catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
        //        {
        //            dialogService.ShowMessage(ex.Message);
        //        }
        //        catch (System.Data.Entity.Core.EntityException ex)
        //        {
        //            dialogService.ShowMessage(ex.Message);
        //        }
        //    }

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

        //=======================================here we set final e value of evaluation ==================================================
        private RelayCommand setSelectEvaluationCommand;
        public RelayCommand SetSelectEvaluationCommand =>
            setSelectEvaluationCommand ?? (setSelectEvaluationCommand = new RelayCommand(
                    (obj) =>
                    {

                        if (SetSelectEvaluation())
                            dialogService.ShowMessage("Оценка задана");
                        else
                            dialogService.ShowMessage("Оценка НЕ задана");
                    }
                    ));

        private bool SetSelectEvaluation()
        {
            foreach (EvaluationRecord item in AuthorsRecord.EvaluationRecords)
            {                
                _Evaluation.EvaluationRecord = AuthorsRecord.EvaluationRecords[Index];
                //проверка на наличие финальной оценки
                //check for existing final evaluation
                if (item.FinalEvaluation)
                {
                    if (dialogService.YesNoDialog("Уже заданиа финальная оценка. Переназначить?"))
                    {
                        //AuthorsRecord.EvaluationRecords[Index].FinalEvaluation = true;
                        //_Evaluation.FinalEvaluationRecord = AuthorsRecord.EvaluationRecords[Index];
                        SetExecuteAuthor(Index);
                        item.FinalEvaluation = false;
                        return true;  
                    }
                    else
                        return false;
                }
            }
            SetExecuteAuthor(Index);
            //AuthorsRecord.EvaluationRecords[Index].FinalEvaluation = true;
            //_Evaluation.FinalEvaluationRecord = AuthorsRecord.EvaluationRecords[Index];
            return true;  
        }

        private void SetExecuteAuthor(int index)
        {
            AuthorsRecord.EvaluationRecords[index].FinalEvaluation = true;
            _Evaluation.FinalEvaluationRecord = AuthorsRecord.EvaluationRecords[index];
            ExecuteAuthor = AuthorsRecord;
        }


        //==================================== COMMAND FOR ADD EVALUATION TO SELECTED AUTHOR ====================================
        //Эта команда нахрен не нужна сдесь. Исключил ее из работы в XAML, ранее бЫла на кнопке
        //"Сохранить" в окне EditAvaluationWindow.xaml
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

        private RelayCommand editAuthorEvaluateAuthorRecordCommand;
        public RelayCommand EditAuthorEvaluateAuthorRecordCommand =>
                            editAuthorEvaluateAuthorRecordCommand ??
                            (editAuthorEvaluateAuthorRecordCommand = new RelayCommand(
                    (obj) =>
                    {
                        EditAuthorEvaluateAuthorRecord();
                        Window window = obj as Window;
                        window.Close();
                    }
                    ));

        //редактируем записи по оценке авторов
        // edit entries by authors evaluation
        private void EditAuthorEvaluateAuthorRecord()
        {
            AuthorsRecord.EvaluationRecords[Index] = _Evaluation.EvaluationRecord;
            dialogService.ShowMessage("Изменения внесены");

        }

        //===================================== For show complicated filter window  EditAvaluatonWindow.xaml====================
        private RelayCommand initComplicatedFilterCommand;
        public RelayCommand InitComplicatedFilterCommand =>
            initComplicatedFilterCommand ?? (initComplicatedFilterCommand = new RelayCommand(
                    (obj) =>
                    {
                        InitComplicatedFilter();
                    }
                    ));

        private void InitComplicatedFilter()
        {
            AuthorsRecords.Clear();
            if (_Dir.Dir.DirectionName == "---" && _Subject.Subj.SubName == "---" && _AuthorStatus.AuthorStatus.AuthorStatusName == "---")
            {                
                AllAuthorsCall("all");
                return;
            }
            //AuthorsCallByParams(_Dir.Dir.DirectionName, _Subject.Subj.SubName, _AuthorStatus.AuthorStatus.AuthorStatusName);
            AuthorsCallByParams(_Dir.Dir.DirectionName, _Subject.Subj.SubName, _AuthorStatus.AuthorStatus.AuthorStatusName, AuthorsRecords);

        }


        //===================================== For call EDIT window  EditAvaluatWindow.xaml====================
        private RelayCommand calleditSelectedAvaluateRuleOrderWindowCommand;
        public RelayCommand CallEditSelectedAvaluateRuleOrderWindowCommand =>
                            calleditSelectedAvaluateRuleOrderWindowCommand ??
                            (calleditSelectedAvaluateRuleOrderWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        _Evaluation.EvaluationRecord = AuthorsRecord.EvaluationRecords[Index];
                        EditEvaluateWindow editAvaluatWindow = new EditEvaluateWindow(obj);
                        showWindow.ShowDialog(editAvaluatWindow);
                    }
                    ));


        //===================================== For Cancel set author complicated filter and close window w.xaml====================
        private RelayCommand cancelSetAuthorComlicatedFilterCommand;
        public RelayCommand CancelSetAuthorComlicatedFilterCommand =>
            cancelSetAuthorComlicatedFilterCommand ?? (cancelSetAuthorComlicatedFilterCommand = new RelayCommand(
                    (obj) =>
                    {
                        CancelSetAuthorComlicatedFilter();
                        Window window = obj as Window;
                        window.Close();
                    }
                    ));

        private void CancelSetAuthorComlicatedFilter()
        {
            _Dir = new _Direction();
            _Subject = new _Subject();
            _AuthorStatus = new _AuthorStatus();
            
        }

        //===================================== For Cancel save evaluate order any author in EditAvaluatonWindow.xaml====================
        private RelayCommand cancelEvaluateCommand;
        public RelayCommand 
            CancelEvaluateCommand =>
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
            _Evaluation.EvaluationRecord = new EvaluationRecord() { DeadLine = TMPStaticClass.CurrentOrder.Dates.AuthorDeadLine };
        }

        //===================================== For close any window ===========================================
        private RelayCommand closeWindowCommand;
        public RelayCommand CloseWindowCommand =>
            closeWindowCommand ?? (closeWindowCommand = new RelayCommand(
                    (obj) =>
                    {                        
                        Window window = obj as Window;
                        window.Close();
                    }
                    ));

       
        //private void SetExecuteAuthor(int index)
        //{
        //    AuthorsRecord.EvaluationRecords[index].FinalEvaluation = true;
        //    _Evaluation.FinalEvaluationRecord = AuthorsRecord.EvaluationRecords[index];
        //    ExecuteAuthor = AuthorsRecord;
        //}

        //==================================== COMMAND FOR ADD EVALUATION TO ORDER ====================================

        private RelayCommand addEvaluationToOrderCommand;
        public RelayCommand AddEvaluationToOrderCommand => addEvaluationToOrderCommand ?? (addEvaluationToOrderCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddEvaluationToOrder();
                    }
                    ));

        private void AddEvaluationToOrder()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                   var res = db.Orderlines.Where(o=>o.OrderLineId==TMPStaticClass.CurrentOrder.OrderLineId).FirstOrDefault();

                    foreach (var item1 in SelectedAuthorsRecords)
                    {
                        //Order.Author.Add(item1.Author);
                        foreach (var item2 in item1.EvaluationRecords)
                        {
                            
                            Evaluation evaluation = new Evaluation();
                            evaluation.AuthorDeadLine = item2.DeadLine;
                            evaluation.Description = item2.EvaluateDescription;
                            evaluation.Winner = item2.FinalEvaluation;
                            evaluation.Dates.Add(new Dates() { AuthorDeadLine= item2.DeadLine});                           
                            evaluation.Moneys.Add( new Money() { AuthorPrice=item2.Price});
                            evaluation.Authors.Add(db.Authors.Find(item1.Author.AuthorId));
                            //db.Evaluations.Add(evaluation);//вот тут хз, надло было это или нет. Вроде надо.
                            res.Evaluations.Add(evaluation);
                            var author = db.Authors.Where(a => a.AuthorId == item1.Author.AuthorId).FirstOrDefault();
                            author.Evaluation.Add(evaluation);
                        }
                        res.Author.Add(db.Authors.Find(item1.Author.AuthorId));
                    }
                    if (_Status.Status.StatusId == 1 && SelectedAuthorsRecords.Count() > 0)
                        res.Status = db.Statuses.Find(6);
                    else
                        db.Statuses.Find(_Status.Status.StatusId);
                    db.SaveChanges();                     
                    dialogService.ShowMessage("Данные о заказе сохранены");
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



    }
}
