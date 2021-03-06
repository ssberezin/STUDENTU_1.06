﻿using STUDENTU_1._06.Helpes;
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
        //общий списко оценок авторов по заказу
        // general list of author ratings for the order
        public ObservableCollection<EvaluationRecord> ExistOrderEvaluations { get; set; }
        IDialogService dialogService;
        IShowWindowService showWindow;

        //rating view  evaluations
        bool LookEvaluations = false;

        //basic constructor. 
        public RuleOrderLine()
        {
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();           

            AuthorsRecords = new ObservableCollection<AuthorsRecord>();
            SelectedAuthorsRecords = new ObservableCollection<AuthorsRecord>();
            ExistOrderEvaluations = new ObservableCollection<EvaluationRecord>();

            AuthorsRecord = new AuthorsRecord();
            _AuthorStatus = new _AuthorStatus();           
            _Dir = new _Direction();
            _Evaluation = new _Evaluation();
            Order = new OrderLine();
            _Subject = new _Subject();
            _Status = new _Status();

            if (TMPStaticClass.CurrentOrder != null)
                _Status.Status = TMPStaticClass.CurrentOrder.Status;
            ExecuteAuthor = new AuthorsRecord();
            RoolMSG = "Заказ не распределен";
            if (TMPStaticClass.CurrentOrder != null)
                PushInitial();

           

        }
       

        //этот конструктор задействуется при редакции данных автора 
        // this constructor is used when editing author data
        public RuleOrderLine(string str)
        {
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
            AuthorsRecords = new ObservableCollection<AuthorsRecord>();
            AuthorsRecord = new AuthorsRecord();
            _AuthorStatus = new _AuthorStatus();
            _Dir = new _Direction();
            _Subject = new _Subject();
            _Evaluation = new _Evaluation();
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
        // to get the index value of the selected record in the author’s rating array
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

        //для отображения сообщения об авторстве 
        // to display the authorship message
        private string roolMSG;
        public string RoolMSG
        {
            get { return roolMSG; }
            set
            {
                if (roolMSG != value)
                {
                    roolMSG = value;
                    OnPropertyChanged(nameof(roolMSG));
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

//===============================================INITIAL METHODS ===================================================================

        //нужен потому что на момент сосздания объекта RuleOrderLine в контекте ForEditOrder.cs 
        //TMPStaticClass.CurrentOrder может бЫть равен null.Актуально при попытке распределния заказа
        //сразу в момент его создания, т.е.  не закрывая окно приема заказа
        // needed because at the time of creation of the RuleOrderLine object in the ForEditOrder.cs context
        //TMPStaticClass.CurrentOrder may be null. Ideally, when trying to distribute an order
        // immediately at the time of its creation, i.e. without closing the order acceptance window
        private void PushInitial()
        {   
            Order = (OrderLine)TMPStaticClass.CurrentOrder.Clone();
            Order.DescriptionForClient = "Вариант(ы): " + CheckForEmpty(Order.Variant) + ". \n" + Order.DescriptionForClient +
                "\n\nСрок выполнения: " + Order.Dates.AuthorDeadLine.ToShortDateString() + " или свой вариант. " +
                "\n Время: к " + Order.Dates.AuthorDeadLine.ToShortTimeString() + " или свой вариант. ";
            FillAuthorsRecords();
            CheckWinnerEvaluation();
        }
        private void CheckWinnerEvaluation()
        {
            //if (TMPStaticClass.CurrentOrder == null)
            //    return;
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var order = db.Orderlines.Where(o => o.OrderLineId == TMPStaticClass.CurrentOrder.OrderLineId).FirstOrDefault();
                    foreach (var item in order.Evaluations)
                    {
                        int authorId = 0;
                        if (item.Winner)
                        {
                            foreach (var i in order.Author)
                            {
                                foreach (var j in i.Evaluation)
                                    if (j.EvaluationId == item.EvaluationId)
                                    {
                                        authorId = i.AuthorId;
                                        break;
                                    }
                                if (authorId != 0)
                                    break;
                            }
                            SetExecuteAuthor(item.EvaluationId, authorId);
                            return;
                        }
                    }

                    foreach (var item in SelectedAuthorsRecords)
                    {
                        int j = 0;
                        foreach (var i in item.EvaluationRecords)
                            if (i.FinalEvaluation)
                            {
                                SetExecuteAuthor(item.EvaluationRecords[j].EvalCopyId, item.Author.AuthorId);
                                return;
                            }
                        j++;
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
        //проверяем не пустое ли поле с вариантами. Возращает "не задано" если пустое. Если не пустое - возвращет исходное значение
        // check if the field with options is empty. Returns "not set" if empty. If not empty, returns the original value.
        private string CheckForEmpty(string str)
        {
            if (str == null || str == " ")
                return "не задано";
            str.Trim();
            if (str[0] == ' ' || str == "")
                return "не задано";
            return str;
        }

        //напоняем  SelectedAuthorsRecords записями, если заказ уже содержит какие-либо оценки от авторов
        // we make  SelectedAuthorsRecords records if the order already contains any ratings from the authors
        private void FillAuthorsRecords()
        {
            //if (TMPStaticClass.CurrentOrder == null)
            //    return;
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var order = db.Orderlines.Where(o => o.OrderLineId == TMPStaticClass.CurrentOrder.OrderLineId).FirstOrDefault();
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
//====================================================================================================================================


//==================================================EDIT AUTHOR EVALUATION COMMAND =============================================
        private RelayCommand setAuthorAvaluationCommand;
        public RelayCommand SetAuthorAvaluationCommand => setAuthorAvaluationCommand ?? (setAuthorAvaluationCommand = new RelayCommand(
                    (obj) =>
                    {
                        LookEvaluations = false;
                        AuthorsRecord.EvaluationRecords.Clear();
                        Change_AuthorsRecord();                        
                        EditAvaluationWindow editAvaluationWindow = new EditAvaluationWindow(obj);
                        showWindow.ShowDialog(editAvaluationWindow);
                    }
                    ));
        //эта фича меняет значение AuthorsRecord для корректного отображения оценок в EditAvaluationWindow.xaml
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
                        int i = 0;
                        if (item.Authors[i].AuthorId != AuthorsRecord.Author.AuthorId)
                        {
                            i++;
                            continue;
                        }
                        _Evaluation.EvaluationRecord = new EvaluationRecord()
                        {
                            EvalCopyId = item.EvaluationId,
                            DeadLine = item.Dates[i].AuthorDeadLine,
                            Price = item.Moneys[i].AuthorPrice,
                            EvaluateDescription = item.Description,
                            FinalEvaluation = item.Winner
                        };
                        AuthorsRecord.EvaluationRecords.Add(_Evaluation.EvaluationRecord);
                        i++;
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

//==============================Command for call CompareEvaluationWindow =======================

        //CompareAvaluationCommand
        private RelayCommand compareAvaluationCommand;
        public RelayCommand CompareAvaluationCommand =>
            compareAvaluationCommand ?? (compareAvaluationCommand = new RelayCommand(
                    (obj) =>
                    {
                        //rating view  evaluations
                         LookEvaluations = true;
                        ExistOrderEvaluations.Clear();
                        FillExistOrderEvaluations();
                        CompareEvaluationWindow window = new CompareEvaluationWindow(obj);
                        showWindow.ShowWindow(window);
                    }
                    ));

        private void FillExistOrderEvaluations()
        {

            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var order = db.Orderlines.Where(o => o.OrderLineId == TMPStaticClass.CurrentOrder.OrderLineId).FirstOrDefault();
                    foreach (var item in order.Evaluations)
                    {

                        int i = 0;
                        _Evaluation.EvaluationRecord = new EvaluationRecord()
                        {
                            EvalCopyId = item.EvaluationId,
                            DeadLine = item.Dates[i].AuthorDeadLine,
                            Price = item.Moneys[i].AuthorPrice,
                            EvaluateDescription = item.Description,
                            FinalEvaluation = item.Winner,
                            Author = item.Authors[i]
                        };
                        ExistOrderEvaluations.Add(_Evaluation.EvaluationRecord);
                        i++;
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
 //=========================================================================================================================
        
 //=============================FILTERS FOR fill listbox "Authors" if check "Authors by direction"====================
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
            if (Order == null)
                return;
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
                                    Persone=(Persone)item.Persone.CloneExceptVirtual(),                                    
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
//=============================================================================================================================================================
//=====================================COMMAND For show complicated filter window  EditAvaluatonWindow.xaml====================
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

//==============================================================================================================================
        ////=================Clear selected authors collection =================================================
        //private RelayCommand clearSelectedAuthorsListCommand;
        //public RelayCommand ClearSelectedAuthorsListCommand =>
        //    clearSelectedAuthorsListCommand ?? (clearSelectedAuthorsListCommand = new RelayCommand(
        //            (obj) =>
        //            {
        //                ClearSelectedAuthorsList();
        //            }
        //            ));
        //private void ClearSelectedAuthorsList()
        //{
        //    SelectedAuthorsRecords.Clear();
        //}


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
//==========================================================================================================================

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
                                    Persone =item.Persone,
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
            if (!SelectedAuthorsRecords.Contains(AuthorsRecord))
            {
                SelectedAuthorsRecords.Add(AuthorsRecord);
                AddSelectedAuthorToDB(AuthorsRecord);
            }
            else
                dialogService.ShowMessage("Уже есть в списке выбранных авторов");
        }
        private void AddSelectedAuthorToDB(AuthorsRecord authorsRecord)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    OrderLine order = db.Orderlines.Where(o => o.OrderLineId == TMPStaticClass.CurrentOrder.OrderLineId).FirstOrDefault();
                    db.Entry(order).State = EntityState.Modified;
                    order.Author.Add(db.Authors.Find(authorsRecord.Author.AuthorId));
                    db.SaveChanges();                  
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

//=============================DELETE AUTHOR FROM SELECTED LIST ============================================================
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
                if (dialogService.YesNoDialog("Если удалить этого автора из списка," +
                    " то записи о его оценках не сохранятся. Точно удалить?"))
                {
                    DelSelectedAuthorFromDB(AuthorsRecord);
                    SelectedAuthorsRecords.Remove(AuthorsRecord);
                }
            }
            else
                dialogService.ShowMessage("Нечего уже  удалять");            
        }
        private void DelSelectedAuthorFromDB(AuthorsRecord authorsRecord)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    string msg = "Среди оценок этого автора есть финальная. Точно удалять?";
                  
                    while (authorsRecord.EvaluationRecords.Count() != 0)                        
                        {
                            DeleteSelectedAvaluate(authorsRecord.EvaluationRecords.First(), msg);
                        };                 
                   
                        OrderLine order = db.Orderlines.Where(o => o.OrderLineId == TMPStaticClass.CurrentOrder.OrderLineId).FirstOrDefault();                       
                        db.Entry(order).State = EntityState.Modified;
                        order.Author.Remove(db.Authors.Find(authorsRecord.Author.AuthorId));

                    //меняем значение статуса заказа на "принят" в случае удаления всех авторов из списка оценок
                    // change the status value of the order to “accepted” if all authors are removed from the list of ratings
                    if (_Status.Status.StatusId != 1 && SelectedAuthorsRecords.Count() == 0)
                    {                        
                        order.Status = db.Statuses.Find(2);
                        _Status.Status = order.Status;
                    }
                    db.SaveChanges();
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

//=========================================================================================================================
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
            AddEvaluationToOrder();
            AuthorsRecord.EvaluationRecords.Add(_Evaluation.EvaluationRecord);         
            _Evaluation.EvaluationRecord = new EvaluationRecord()
            { DeadLine = TMPStaticClass.CurrentOrder.Dates.AuthorDeadLine };         
        }
//====================================================================================================================================
//===================================== For delete evaluate selected author in EditAvaluationWindow.xaml====================
        private RelayCommand deleteSelectedAvaluateCommand;
        public RelayCommand DeleteSelectedAvaluateCommand =>
                            deleteSelectedAvaluateCommand ??
                            (deleteSelectedAvaluateCommand = new RelayCommand(
                    (obj) =>
                    {
                        string msg = "Это финальная оценка заказа. Точно удалять?";
                        DeleteSelectedAvaluate(obj as EvaluationRecord, msg);
                    }
                    ));
        //====================================================================================================================================
        private void DeleteSelectedAvaluate(EvaluationRecord i, string msg)
        {   
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    if (i.FinalEvaluation)
                        if (dialogService.YesNoDialog(msg) == true)
                        {
                            ExecuteAuthor = new AuthorsRecord();
                            _Evaluation.FinalEvaluationRecord = new EvaluationRecord();
                        }
                        else
                            return;
                    Evaluation eval = db.Evaluations.Where(e => e.EvaluationId == i.EvalCopyId).FirstOrDefault();
                    Dates date = db.Dates.Where(d => d.Evaluation.EvaluationId == eval.EvaluationId).FirstOrDefault();
                    Money money = db.Moneys.Where(m => m.Evaluation.EvaluationId == eval.EvaluationId).FirstOrDefault();
                    OrderLine order = db.Orderlines.Where(o => o.OrderLineId == TMPStaticClass.CurrentOrder.OrderLineId).FirstOrDefault();
                    
                    db.Entry(order.Evaluations.Where(e=>e.EvaluationId== eval.EvaluationId).First()).State = EntityState.Deleted;
                    db.Entry(date).State = EntityState.Deleted;
                    db.Entry(money).State = EntityState.Deleted;                                
                    db.Entry(eval).State= EntityState.Deleted;
                    db.SaveChanges();
                    AuthorsRecord.EvaluationRecords.Remove(i);                    
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
//====================================================================================================================================
        

//=======================================COMMAND FOR SET final evalue of evaluation ==================================================
        private RelayCommand setSelectEvaluationCommand;
        public RelayCommand SetSelectEvaluationCommand =>
            setSelectEvaluationCommand ?? (setSelectEvaluationCommand = new RelayCommand(
                    (obj) =>
                    {
                        if (SetSelectEvaluation())
                        {
                            if (LookEvaluations)
                            {
                                ExistOrderEvaluations.Clear();
                                FillExistOrderEvaluations();                                
                            }
                            else
                            {
                                AuthorsRecord.EvaluationRecords.Clear();
                                Change_AuthorsRecord();                                
                            }                          
                        }
                        else
                            dialogService.ShowMessage("Оценка НЕ задана");
                    }
                    ));

        private bool SetSelectEvaluation()
        {
            EvaluationRecord evalRecord = (EvaluationRecord)this._Evaluation.EvaluationRecord.Clone
                                           (_Evaluation.EvaluationRecord.DeadLine, _Evaluation.EvaluationRecord.Price);
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var order = db.Orderlines.Where(o => o.OrderLineId == Order.OrderLineId).FirstOrDefault();
                    Evaluation eval = db.Evaluations.Where(e => e.EvaluationId == evalRecord.EvalCopyId).FirstOrDefault();
                    if (eval.Winner)
                    {
                        dialogService.ShowMessage("Эта оценка и так указана как финальная...");
                        return false;
                    };
                    foreach (var item in order.Evaluations)
                        if (item.Winner&&item.EvaluationId!=eval.EvaluationId)
                        {
                            if (dialogService.YesNoDialog("Уже задана финальная оценка. Переназначить?"))
                            {
                                db.Entry(item).State = EntityState.Modified;
                                db.Entry(eval).State = EntityState.Modified;
                                item.Winner = false;
                                eval.Winner = true;
                                db.SaveChanges();
                                SetExecuteAuthor(eval.EvaluationId, ExistOrderEvaluations[Index].Author.AuthorId);                                
                                return true;
                            }
                            else
                                return false;                            
                        }
                    if (!eval.Winner)
                    {
                        dialogService.ShowMessage("Финальная оценка задана");
                        db.Entry(eval).State = EntityState.Modified;
                        eval.Winner = true;
                        db.SaveChanges();
                        SetExecuteAuthor(eval.EvaluationId, AuthorsRecord.Author.AuthorId);
                        return true;
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
                return false;
            }
        }
        //для корректного отображения атвора работа в окне распределения заказа.
        //все данные достаем из базы данных
        //index - эквивалентно Evaluation.EvaluationId, authorId - эквивалентно Author.AuthorId
        // to display the author correctly, work in the order distribution window.
        // we get all the data from the database
        // index - equivalent to Evaluation.EvaluationId, authorId - equivalent to Author.AuthorId
        private void SetExecuteAuthor(int index, int authorId)
        {
            if (index == 0)
                return;
           
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {                   
                    Evaluation eval = db.Evaluations.Where(e => e.EvaluationId == index).FirstOrDefault();
                    _Evaluation.FinalEvaluationRecord = new EvaluationRecord()
                    {
                        EvalCopyId = eval.EvaluationId,
                        DeadLine = db.Dates.Where(d => d.Evaluation.EvaluationId == eval.EvaluationId).FirstOrDefault().AuthorDeadLine,
                        Price = db.Moneys.Where(m=>m.Evaluation.EvaluationId==eval.EvaluationId).FirstOrDefault().AuthorPrice,
                        EvaluateDescription = eval.Description,
                        FinalEvaluation = eval.Winner
                    };
                    Author author = db.Authors.Where(a => a.AuthorId == authorId).FirstOrDefault();
                    ExecuteAuthor.Author = author;
                    RoolMSG = $"Заказ закреплен за {ExecuteAuthor.Author.Persone.NickName}";
                   

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

//==================================== COMMAND FOR EDIT EVALUATION ================================================
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
            if (CheckEval(_Evaluation.EvaluationRecord, AuthorsRecord.EvaluationRecords))
                return;

           // AuthorsRecord.EvaluationRecords[Index] = _Evaluation.EvaluationRecord;
            //dialogService.ShowMessage("Изменения внесены");
            //нижеприведененный закомментированный кусок кода для редакции с проямой записью в БД
            // below commented out piece of code for the editor with a record in the database

            EvaluationRecord evalRecord = (EvaluationRecord)this._Evaluation.EvaluationRecord.Clone
                                           (_Evaluation.EvaluationRecord.DeadLine, _Evaluation.EvaluationRecord.Price);
            AuthorsRecord.EvaluationRecords[Index] = evalRecord;
            if (evalRecord.EvalCopyId == 0)//ну мало ли...))
                return;

            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {

                    Evaluation eval = db.Evaluations.Where(e => e.EvaluationId == evalRecord.EvalCopyId).FirstOrDefault();
                    Dates date = db.Dates.Where(d => d.Evaluation.EvaluationId == eval.EvaluationId).FirstOrDefault();
                    Money money = db.Moneys.Where(m => m.Evaluation.EvaluationId == eval.EvaluationId).FirstOrDefault();

                    db.Entry(eval).State = EntityState.Modified;
                    db.Entry(date).State = EntityState.Modified;
                    db.Entry(money).State = EntityState.Modified;

                    eval.Description = evalRecord.EvaluateDescription;
                    eval.Winner = evalRecord.FinalEvaluation;
                    date.AuthorDeadLine = evalRecord.DeadLine;
                    money.AuthorPrice = evalRecord.Price;
                    db.SaveChanges();


                    dialogService.ShowMessage("Изменения внесены");
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
        public RelayCommand  CancelEvaluateCommand =>
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
        //============================================================================================================

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

        //============================================================================================================       
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
        //проверяем AuthorsRecord на наличие идентичной оценки 
        // check AuthorsRecord for an identical evaluation
        private bool CheckEval(EvaluationRecord evRec, ObservableCollection<EvaluationRecord> evRecColl)
        {
            foreach (var item in evRecColl)
                if (evRec.CompareEvaluationRecordsWNotId(evRec, item))
                {
                    dialogService.ShowMessage("Уже есть такая оценка. Оценка не добавлена");
                    return true;
                };
            return false;
        }

        private void AddEvaluationToOrder()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //проверяем наличие такой же оценки перед сохранением
                    // check for the same evaluation before saving
                    if (CheckEval(_Evaluation.EvaluationRecord, AuthorsRecord.EvaluationRecords))
                        return;
                    var res = db.Orderlines.Where(o => o.OrderLineId == TMPStaticClass.CurrentOrder.OrderLineId).FirstOrDefault();
                    Evaluation evaluation = new Evaluation();
                    evaluation.Description = _Evaluation.EvaluationRecord.EvaluateDescription;
                    evaluation.Winner = _Evaluation.EvaluationRecord.FinalEvaluation;
                    evaluation.Dates.Add(new Dates() { AuthorDeadLine = _Evaluation.EvaluationRecord.DeadLine });
                    evaluation.Moneys.Add(new Money() { AuthorPrice = _Evaluation.EvaluationRecord.Price });
                    evaluation.Authors.Add(db.Authors.Find(AuthorsRecord.Author.AuthorId));
                    res.Evaluations.Add(evaluation);
                    var author = db.Authors.Where(a => a.AuthorId == AuthorsRecord.Author.AuthorId).FirstOrDefault();
                    author.Evaluation.Add(evaluation);
                    res.Author.Add(db.Authors.Find(AuthorsRecord.Author.AuthorId));

                    if (_Status.Status.StatusId == 1 && SelectedAuthorsRecords.Count() > 0)
                        res.Status = db.Statuses.Find(6);
                    else
                        db.Statuses.Find(_Status.Status.StatusId);
                    db.SaveChanges();                    
                    dialogService.ShowMessage("Данные об оценке сохранены");



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
        //============================================================================================================

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
        //==========================================================================================================================

        //==================================================call RuleOrderLineWindow==================================================
        private RelayCommand newRuleOrderLineWindowCommand;
        public RelayCommand NewRuleOrderLineWindowCommand =>
            newRuleOrderLineWindowCommand ?? (newRuleOrderLineWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        if (TMPStaticClass.CurrentOrder == null)
                            PushInitial();
                        RuleOrderLineWindow ruleOrderLineWindow = new RuleOrderLineWindow(obj);
                        showWindow.ShowDialog(ruleOrderLineWindow);

                    }
                    ));
        //==============================================================================================================================

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
        //============================================================================================  

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
        //=========================================================================================================================

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
        //====================================================================================================================================
    }
}
