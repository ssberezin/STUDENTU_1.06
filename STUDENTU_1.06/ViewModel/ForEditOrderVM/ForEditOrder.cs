using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Windows;
using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.DBModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views;
using STUDENTU_1._06.Views.EditOrderWindows.ContactsWindows;
using STUDENTU_1._06.Views.EditOrderWindows.RuleOrderLineWindows;

namespace STUDENTU_1._06.ViewModel
{
    public partial class ForEditOrder : ObservableObject
    {
        public ObservableCollection<Contacts> ContactsRecords { get; set; }
        public ObservableCollection<Records> Records { get; set; }
        //for stor author list
        public ObservableCollection<AuthorsRecord> AuthorsRecords { get; set; }

        



        bool saved = false;//флаг, для того, чтоб понимать, был ли сохранен заказ в первый раз или нет.

        IDialogService dialogService;
        IShowWindowService showWindow;

        //==========================================PROPERTIES============================================
        //for set value of order number. Need to work more            

        private Author author;
        public Author Author
        {
            get { return author; }
            set
            {
                if (author != value)
                {
                    author = value;
                    OnPropertyChanged(nameof(Author));
                }
            }
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

        private _Contacts _contacts;
        public _Contacts _Contacts
        {
            get { return _contacts; }
            set
            {
                if (_contacts != value)
                {
                    _contacts = value;
                    OnPropertyChanged(nameof(_Contacts));
                }
            }
        }

        private Client client;
        public Client Client
        {
            get { return client; }
            set
            {
                if (client != value)
                {
                    client = value;
                    OnPropertyChanged(nameof(Client));
                }
            }
        }

        private Dates date;
        public Dates Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        //for track changes Date.DeadLine
        private DateTime deadLine;
        public DateTime DeadLine
        {
            get { return deadLine; }
            set
            {
                if (deadLine != value)
                {
                    deadLine = value;
                    OnPropertyChanged(nameof(DeadLine));
                }
            }
        }
        //for track changes Date.DeadLine
        private DateTime deadLineHHMM;
        public DateTime DeadLineHHMM
        {
            get { return deadLineHHMM; }
            set
            {
                if (deadLineHHMM != value)
                {
                    deadLineHHMM = value;
                    OnPropertyChanged(nameof(DeadLineHHMM));
                }
            }
        }

        private _Direction _dir;
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
                
      

        //for save previos state Order and compare 
        private OrderLine nullOrder;
        public OrderLine NullOrder
        {
            get { return nullOrder; }
            set
            {
                if (nullOrder != value)
                {
                    nullOrder = value;
                    OnPropertyChanged(nameof(NullOrder));
                }
            }
        }

        //для вывода сообщение об общей потраченной ранее заказчиком сумме, в спарке 
        // for output, a message about the total amount previously spent by the customer
        private string msg;
        public string Msg
        {
            get { return msg; }
            set
            {
                if (msg != value)
                {
                    msg = value;
                    OnPropertyChanged(nameof(Msg));
                }
            }
        }

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

        private Money price;
        public Money Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

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
        //for temp stre Order in order redaction
        private OrderLine tmpOrder;
        public OrderLine TmpOrder
        {
            get { return tmpOrder; }
            set
            {
                if (tmpOrder != value)
                {
                    tmpOrder = value;
                    OnPropertyChanged(nameof(TmpOrder));
                }
            }
        }

        private _Subject _subj;
        public _Subject _Subj
        {
            get { return _subj; }
            set
            {
                if (_subj != value)
                {
                    _subj = value;
                    OnPropertyChanged(nameof(_Subj));
                }
            }
        }

     

        private _Source _source;
        public _Source _Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    OnPropertyChanged(nameof(_Source));
                }
            }
        }

        private PersoneDescription personeDescription;
        public PersoneDescription PersoneDescription
        {
            get { return personeDescription; }
            set
            {
                if (personeDescription != value)
                {
                    personeDescription = value;
                    OnPropertyChanged(nameof(PersoneDescription));
                }
            }
        }

        private RuleOrderLine ruleOrderLine;
        public RuleOrderLine RuleOrderLine
        {
            get { return ruleOrderLine; }
            set
            {
                if (ruleOrderLine != value)
                {
                    ruleOrderLine = value;
                    OnPropertyChanged(nameof(RuleOrderLine));
                }
            }
        }

        private _University _university;
        public _University _University
        {
            get { return _university; }
            set
            {
                if (_university != value)
                {
                    _university = value;
                    OnPropertyChanged(nameof(_University));
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

        private User usver_curent;
        public User Usver_curent
        {
            get { return usver_curent; }
            set
            {
                if (usver_curent != value)
                {
                    usver_curent = value;
                    OnPropertyChanged(nameof(Usver_curent));
                }
            }
        }


        private _WorkType _workType;
        public _WorkType _WorkType
        {
            get { return _workType; }
            set
            {
                if (_workType != value)
                {
                    _workType = value;
                    OnPropertyChanged(nameof(_WorkType));
                }
            }
        }

        //for new order
        public ForEditOrder(int UserId)
        {
            DefaultLoadData(UserId);
            Order = new OrderLine { OrderNumber = GetOrderNumber() };
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
            PropertyChanged += ChangeRuleOrder;
        }
        //for edit allready exist order
        public ForEditOrder(int OrderLineId, int UserId)
        {
            DefaultLoadDataEditOrder(OrderLineId, UserId);           
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
            PropertyChanged += ChangeRuleOrder;
        }

        //for create new order
        private void DefaultLoadData(int UserId)
        {
            BlackListRecords = new ObservableCollection<BlackListHelpModel>();
            AuthorsRecords = new ObservableCollection<AuthorsRecord>();
            ContactsRecords = new ObservableCollection<Contacts>();
            Records = new ObservableCollection<Records>();

            AuthorsRecord = new AuthorsRecord();
           Author = new Author();
            _Contacts = new _Contacts();
            Client = new Client();
            Date = new Dates();
            DeadLine = Date.DeadLine;
            DeadLineHHMM = Date.DeadLine; 
            _Dir = new _Direction();
            
            Persone = new Persone();
            PersoneDescription = new PersoneDescription();
            Price = new Money();
            RuleOrderLine = new RuleOrderLine();
            RuleOrderLine._Evaluation.FinalEvaluationRecord = new EvaluationRecord()
            {
                DeadLine = Date.AuthorDeadLine,
                Price = 0,
                EvaluateDescription = ""
            };            
            _Subj = new _Subject();
            _Source = new _Source();
            _University = new _University();
            _WorkType = new _WorkType();
            roolMSG = "Заказ не распределен";
            AllAuthorsCall();//fill out the authors list
            TmpOrder = new OrderLine();
            SetTmpOrder(TmpOrder);//for look changes befor close window
            NullOrder = new OrderLine();
            SetTmpOrder(NullOrder);

            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    Usver = db.Users.Where(e => e.UserId == UserId).FirstOrDefault();
                    if (Usver == null)
                    {
                        dialogService.ShowMessage("Проблемы связи с БД при попытке подвязки данных пользователя");
                        return;
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

        //for edit allready exist order
        private void DefaultLoadDataEditOrder(int OrderLineId, int UserId)
        {
            BlackListRecords = new ObservableCollection<BlackListHelpModel>();
            AuthorsRecords = new ObservableCollection<AuthorsRecord>();
            ContactsRecords = new ObservableCollection<Contacts>();
            Records = new ObservableCollection<Records>();

            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    Usver = db.Orderlines.Where(e => e.OrderLineId == OrderLineId).FirstOrDefault().User;
                    Usver_curent = db.Users.Where(e => e.UserId == UserId).FirstOrDefault();
                    if (Usver == null|| Usver_curent==null)
                    {
                        dialogService.ShowMessage("Проблемы связи с БД при попытке подвязки данных пользователя");
                        return;
                    }

                    Order = db.Orderlines.Where(o=>o.OrderLineId==OrderLineId).FirstOrDefault();
                    TMPStaticClass.CurrentOrder = (OrderLine)Order.Clone();
                    TmpOrder = new OrderLine();
                    Author = Order.GetExecuteAuthor(Order.Author);                    
                    if(Author==null)
                      Author = new Author();
                    Order.Saved = true;                    
                    _Contacts = new _Contacts() { Contacts = Order.Client.Persone.Contacts };                    
                    Client = Order.Client;
                    Date = Order.Dates;
                    DeadLine = Date.DeadLine;
                    DeadLineHHMM = Date.DeadLine;
                    _Dir = new _Direction { Dir = Order.Direction };
                    Evaluation evaluation = new Evaluation();
                    // evaluation = Author.GetWinnerEvaluation(Author);
                    evaluation = Order.GetWinnerEvaluation(Order);
                    RuleOrderLine = new RuleOrderLine();
                    if (evaluation == null)
                    {
                        RuleOrderLine._Evaluation.FinalEvaluationRecord = new EvaluationRecord()
                        {
                            DeadLine = Date.AuthorDeadLine,
                            Price = 0,
                            EvaluateDescription = ""
                        };
                        RuleOrderLine.RoolMSG = "Заказ не распределен";
                    }
                    else
                    {
                        DateTime date = db.Dates.Where(d=>d.Evaluation.EvaluationId==evaluation.EvaluationId).FirstOrDefault().AuthorDeadLine;
                        Decimal price = db.Moneys.Where(m => m.Evaluation.EvaluationId == evaluation.EvaluationId).FirstOrDefault().AuthorPrice;
                        RuleOrderLine._Evaluation.FinalEvaluationRecord = new EvaluationRecord()
                        {
                            //DeadLine = evaluation.AuthorDeadLine,
                            DeadLine = date,
                            //Price = evaluation.AuthorPrice,
                            Price=price,
                            EvaluateDescription = evaluation.Description
                        };
                        RuleOrderLine.RoolMSG = $"Заказ закреплен за {Author.Persone.NickName}";
                        evaluationSetWinner = false;//flag for us in CloseWindowCommand
                    }
                    saved = true;//flag for make suborder
                    AuthorsRecord = new AuthorsRecord(){Author = Author};
                    Persone = Order.Client.Persone;
                    PersoneDescription = Order.Client.Persone.PersoneDescription;
                    Price = Order.Money;
                    
                   // RuleOrderLine._Status = new _Status();
                    RuleOrderLine._Status.Status = Order.Status;
                    _Subj = new _Subject();
                    _Subj.Subj = Order.Subject;
                    _Source = new _Source();
                    _Source.Source = Order.Source;
                    _University = new _University();
                    _University.University = Order.Client.Universities[0];
                    _WorkType = new _WorkType();
                    _WorkType.WorkType = Order.WorkType;
                   
                    AllAuthorsCall();//fill out the authors list
                    TmpOrder = (OrderLine)this.Order.Clone();
                    



                    NullOrder = new OrderLine();
                    SetTmpOrder(NullOrder);
                   
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

        
        private void ChangeRuleOrder(object sender, PropertyChangedEventArgs e)
        {
         
                Date.DeadLine = DeadLine.AddHours(9);                
                DeadLineHHMM= Date.DeadLine;
                RuleOrderLine._Evaluation.FinalEvaluationRecord.DeadLine = Date.DeadLine;
              
            
        }

        //=================================METHODS FOR PREVIOS LOAD TO CONTROLS OF EditOrder.xaml ===================

        //create new oreder number (get max namber and add 1)
        private int GetOrderNumber()
        {
            int result = 0;

            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //int name = (from m in db.Таблица select m.Нужное_поле).ToList().Last();
                    if (db.Orderlines.Count() == 0)
                        return 98000;
                    else
                    {
                        //нужно допилить , когда придет время
                        var res = db.Orderlines.OrderBy(p => p.OrderNumber).ToList().Last();                        
                        result = res.OrderNumber;
                        result += 1;
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
            return result;

        }

        private void SetTmpOrder(OrderLine order)
        {
            //TmpOrder = new OrderLine();
            order.Direction = this._Dir.Dir;
            order.WorkType = this._WorkType.WorkType;
            order.Subject = this._Subj.Subj;
            order.Source = this._Source.Source;
            order.Dates = this.Date;
            order.Money = this.Price;
            order.Status = this.RuleOrderLine._Status.Status;
            order.Client = new Client()
            {
                Persone = new Persone()
                {
                    Name= this.Persone.Name,
                    Surname= this.Persone.Surname,
                    Patronimic= this.Persone.Patronimic,
                    Male= this.Persone.Male,
                    Female=this.Persone.Female,
                    Contacts=this._Contacts.Contacts
                },
                Course=this.Client.Course

            };
            TmpOrder.Client.Universities.Add(_University.University);
        }

        //==================================== COMMAND FOR SAVE NEW ORDER ====================================

        private RelayCommand createNewOrderLine;
        public RelayCommand CreateNewOrderLine => createNewOrderLine ?? (createNewOrderLine = new RelayCommand(
                    (obj) =>
                    {

                        SaveNewOrder();
                    }
                    ));

     

        private RelayCommand createDobleNewOrderLine;
        public RelayCommand CreateDobleNewOrderLine => createDobleNewOrderLine ?? (createDobleNewOrderLine = new RelayCommand(
                    (obj) =>
                    {
                        if (saved)
                        {
                            if (dialogService.YesNoDialog("Разбить заказ на подзаказы?") == true)
                                DoubleOrder();
                        }
                    }
                    ));

        bool doubleSave = false;
        private void DoubleOrder()
        {

            Order = new OrderLine() { OrderNumber = TMPStaticClass.CurrentOrder.OrderNumber };
            Order.Saved = false;
            Order.ParentOrder = false;
            saved = false;
            doubleSave = true;
            dialogService.ShowMessage("Действие выполнено");

        }

        private void SaveNewOrder()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    if (doubleSave)
                    {
                        DoubleSaveNewOrder();
                        return;
                    }
                    Order.Direction = db.Directions.Find(_Dir.Dir.DirectionId);
                    Order.WorkType = db.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    Order.Subject = db.Subjects.Find(_Subj.Subj.SubjectId);
                    Order.Source = db.Sources.Find(_Source.Source.SourceId);
                    Order.Dates = Date;
                    Order.Money = Price;
                    if (RuleOrderLine._Status.Status.StatusId == 1)
                    {
                        Order.Status = db.Statuses.Find(2);
                        RuleOrderLine._Status.Status= db.Statuses.Find(2);
                    }
                    else                    
                        Order.Status = db.Statuses.Find(RuleOrderLine._Status.Status.StatusId);                    
                    
                    Order.Saved = true;

                    //ищем совпададения по полям контактов Person в БД. Если "0", то совпадений не найдено
                    //если совпадение есть, то получаем Id нужной записи в Contacts
                    // look for matches on the fields of the Person contacts in the database. If "0", then no matches were found
                    // if there is a match, then we get the Id of the desired entry in Contacts

                    //нашли Id прежнего Contscts
                    // found Id of former Contscts
                    int contactId = 0;
                    contactId = _Contacts.Contacts.CheckContacts(_Contacts.Contacts);
                    if (contactId == 0)
                    {
                        Persone.Contacts = _Contacts.Contacts;
                        Persone.PersoneDescription = PersoneDescription;
                        Order.Client = new Client() { Persone = Persone, Course = Client.Course };
                        Order.Client.Universities.Add(db.Universities.Find(_University.University.UniversityId));

                    }
                    else
                    {
                        var persone = db.Persones.Where(o => o.Contacts.ContactsId == contactId).FirstOrDefault();
                        bool ContactsCompare = _Contacts.CompareContacts(_Contacts.Contacts, persone.Contacts);
                        bool PersonFirsDataCompare = Persone.ComparePersons(Persone, persone);

                        SaveOrderPartAfterCheckContacts(contactId, 0, ContactsCompare, PersonFirsDataCompare);
                        Order.Client = db.Clients.Find(Client.ClientId);
                        if (CancelSaveOrder)
                            return;
                    }
                    Order.User = db.Users.Where(e => e.UserId == Usver.UserId).FirstOrDefault();
                    //нижние две строки кода - признак того, что что-то пошло не тиак, но вроде работает....
                    db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Orderlines.Add(Order);

                    db.SaveChanges();

                    db.Entry(Order).State = EntityState.Modified;
                    Order.ParentId = Order.OrderLineId;

                    db.SaveChanges();

                    TMPStaticClass.CurrentOrder = (OrderLine)Order.Clone();
                    saved = true;
                    doubleSave = false;
                    db.Entry(Order).State = EntityState.Detached;
                    //обновляем информацию о прежних заказах заказчика в окне оформления заказа EditOrder.xaml
                    // update information about previous customer orders in EditOrder.xaml
                    if (contactId != 0)
                        LoadRecords(client.ClientId);
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

        private void DoubleSaveNewOrder()
        {
            using (StudentuConteiner db2 = new StudentuConteiner())
            {
                try
                {
                    db2.Orderlines.Attach(Order);                   
                    db2.Contacts.Attach(_Contacts.Contacts);                                     
                    db2.PersoneDescriptions.Attach(PersoneDescription);
                    db2.Persones.Attach(Persone);
                    Order.Direction = db2.Directions.Find(_Dir.Dir.DirectionId);
                    Order.WorkType = db2.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    Order.Subject = db2.Subjects.Find(_Subj.Subj.SubjectId);
                    Order.Source = db2.Sources.Find(_Source.Source.SourceId);
                    Order.Dates = Date;
                    Order.Money = Price;
                    if (Usver_curent!=null&&Usver_curent.UserId!=Usver.UserId)                        
                        Order.User = db2.Users.Where(e => e.UserId == Usver_curent.UserId).FirstOrDefault();
                    else                        
                        Order.User = db2.Users.Where(e => e.UserId == Usver.UserId).FirstOrDefault();
                    if (RuleOrderLine._Status.Status.StatusId == 1)
                    {
                        Order.Status = db2.Statuses.Find(2);
                        RuleOrderLine._Status.Status = db2.Statuses.Find(2);
                    }
                    else
                        Order.Status = db2.Statuses.Find(RuleOrderLine._Status.Status.StatusId);
                    Order.Saved = true;
                    //тут нужно проверяем текущие контакты с контатными данными родительского заказа
                    //вдруг пользователь изменил чего?...
                    // here we need to check the current contacts with the contact data of the parent order
                    // suddenly the user changed something? ...    
                    bool ContactsCompare = _Contacts.CompareContacts(_Contacts.Contacts, TMPStaticClass.CurrentOrder.Client.Persone.Contacts);
                    bool PersonFirsDataCompare = Persone.ComparePersons(Persone, TMPStaticClass.CurrentOrder.Client.Persone);
                    if (!ContactsCompare || !PersonFirsDataCompare)
                    {   
                        SaveOrderPartAfterCheckContacts(0, TMPStaticClass.CurrentOrder.ParentId, ContactsCompare, PersonFirsDataCompare);
                        if (doubleSaveCheck)
                        {
                            Client = db2.Clients.Find(TMPStaticClass.CurrentOrder.Client.ClientId);
                            Client.OrderLine.Add(Order);                     
                            doubleSaveCheck = false;
                        }
                        if (doubleSaveCheckDif)
                        {
                            int clientId = ChangeContactsPersonDataAndReturnClient(_Contacts);
                            Client = db2.Clients.Find(clientId);
                            Client.OrderLine.Add(Order);                            
                            doubleSaveCheckDif = true;
                        }
                        Order.Client = db2.Clients.Find(Client.ClientId);
                        if (CancelSaveOrder)
                            return;
                    }
                    else
                    {
                        Client = db2.Clients.Find(TMPStaticClass.CurrentOrder.Client.ClientId);
                        Client.OrderLine.Add(Order);
                        Order.Client = db2.Clients.Find(Client.ClientId);
                    }

                    //и тут эти нижние 2 строки кода свидетельствуют о том, что данные в БД могут залетать 
                    //кастыльно ...(
                    db2.Configuration.AutoDetectChangesEnabled = false;
                    db2.Configuration.ValidateOnSaveEnabled = false;
                    db2.Orderlines.Add(Order);

                    db2.SaveChanges();

                    db2.Entry(Order).State = EntityState.Modified;
                    Order.ParentId = TMPStaticClass.CurrentOrder.ParentId;
                    db2.SaveChanges();

                    EditOrderCount(TMPStaticClass.CurrentOrder.OrderLineId, TMPStaticClass.CurrentOrder.OrderNumber);
                    TMPStaticClass.CurrentOrder = (OrderLine)Order.Clone();
                    saved = true;
                    doubleSave = false;
                    db2.Entry(Order).State = EntityState.Detached;
                    //обновляем информацию о прежних заказах заказчика в окне оформления заказа EditOrder.xaml
                    // update information about previous customer orders in EditOrder.xaml
                    LoadRecords(client.ClientId);
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

        bool CancelSaveOrder = false;
        //флаг для возврата в основной контекст , если пользователь
        //оставил все контактные данные прежними
        // flag to return to the main context, if the user
        // left all contact details the same
        bool doubleSaveCheck = false;
        //флаг для возврата в основной контекст , если пользователь
        //заменил прежние контактные данные на другие
        // flag to return to the main context, if the user
        // replaced the previous contact details with other
        bool doubleSaveCheckDif = false;

        private void SaveOrderPartAfterCheckContacts(int contactsId, int doubleId,
                                                    bool contactsCompare, bool personeCompare)
        {
            using (StudentuConteiner db3 = new StudentuConteiner())
            {
                try
                {
                    Persone persone = new Persone();
                    Contacts OldContacts = new Contacts();

                    if (contactsCompare && personeCompare)
                    {
                        Client = db3.Clients.Where(c => c.Persone.Contacts.ContactsId == contactsId).FirstOrDefault();
                        Client = db3.Clients.Find(Client.ClientId);
                        Client.OrderLine.Add(Order);

                        // к  Order.Client мы добавляем полученного Client уже в основном методе SaveNewOrder
                        //так как еще не хватает навыков работы с контекстом , чтоб сделать это  красиво
                        //ниже приведенна конструкция тут не работает. 
                        // to Order.Client we add the received Client already in the main SaveNewOrder method
                        // since there are still not enough skills to work with kaontext, to make it beautiful
                        // the construction below does not work here.

                        // Order.Client = db3.Clients.Find(Client.ClientId);
                        return;
                    }
                    else
                    {

                        if (doubleId == 0)                        
                            //ветка первичного сохранения
                            //first save branch
                            dialogService.ShowMessage("Уже есть клиент с такими контактными данными.\n" +
                                                 "Вносим правку в базу данных");
                        else
                        {
                            //ветка doublesave
                            //doublesave branch
                            dialogService.ShowMessage("Контактные данные в подзаказе не совпадают с теми,.\n" +
                                                 "которые были в исходном заказе. ");                            
                            contactsId = TMPStaticClass.CurrentOrder.Client.Persone.Contacts.ContactsId;                            
                        }
                        persone = db3.Persones.Where(c => c.Contacts.ContactsId == contactsId).FirstOrDefault();
                        OldContacts = db3.Contacts.Where(c => c.ContactsId == contactsId).FirstOrDefault();

                        _Contacts.OldPersoneCompare = (Persone)persone.CloneExceptVirtual();
                        _Contacts.CurPersoneCompare = (Persone)this.Persone.CloneExceptVirtual();
                        _Contacts.TmpContacts = (Contacts)OldContacts.CloneExceptVirtual();
                        _Contacts.OldTmpContactsCompare = (Contacts)OldContacts.CloneExceptVirtual();
                        _Contacts.TmpContactsCompare = (Contacts)this._Contacts.Contacts.CloneExceptVirtual();

                        CompareContatctsWindow compareContatctsWindow = new CompareContatctsWindow(this);
                        showWindow.ShowDialog(compareContatctsWindow);

                        if (!_Contacts.saveCompareResults)
                        {
                            //тут лучше придумать диалоговое окно с радиокнопками , для выбора вариантов действия
                            // - отменить прием заказа и отправить пользователя закрыть окно приема заказа
                            //т.к. не понятно как реализовать закрытие окна из вьюмодел не вмешиваяся в сраный мввм
                            //но в идеале закрыть окно приема заказа. Думаю, что это потянет за собой перепил по всему проекту
                            //процедуры закрытия окна.
                            // - 
                            if (dialogService.YesNoDialog("Не сохранен ни один из вариантов...\n" +
                                    "Отменить прием заказа?"))
                            {
                                dialogService.ShowMessage("Ок. Тогда просто закройте окно приема заказа.");
                                _Contacts.Contacts = OldContacts;
                                CancelSaveOrder = true;
                                return;
                            }
                            else
                            {
                                dialogService.ShowMessage("В базе данных не могут дублироваться контакты у разных клиентов.\n" +
                                      "Задайте другие контактные данные заказчика в окне приема заказа.");
                                _Contacts.Contacts = OldContacts;
                                CancelSaveOrder = true;
                                return;
                            };
                        }

                        personeCompare = persone.ComparePersons(persone, _Contacts.Persone);
                        contactsCompare = _Contacts.CompareContacts(persone.Contacts, _Contacts.Contacts);
                        if (contactsCompare && personeCompare)
                        {
                            doubleSaveCheck = true;
                            return;
                        }
                        else
                        if (doubleSave)
                        {
                            doubleSaveCheckDif = true;
                            return;
                        }
                        if (!personeCompare)
                        {
                            db3.Entry(persone).State = EntityState.Modified;
                            persone.CopyExeptVirtualIdPhoto(persone, _Contacts.Persone);                            
                        }
                        if (!contactsCompare)
                        {
                            db3.Entry(OldContacts).State = EntityState.Modified;
                            _Contacts.Contacts.CopyExceptVirtualAndId(OldContacts, _Contacts.Contacts);                            
                        }

                        db3.SaveChanges();
                        Client = db3.Clients.Where(c => c.Persone.PersoneId == persone.PersoneId).FirstOrDefault();
                        Client.OrderLine.Add(Order);

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

        private int ChangeContactsPersonDataAndReturnClient(_Contacts _Contacts)
        {

            Contacts OldContacts = new Contacts();
            using (StudentuConteiner db4 = new StudentuConteiner())
            {
                try
                {
                    Persone persone = new Persone();
                    int contactsId = TMPStaticClass.CurrentOrder.Client.Persone.PersoneId;
                    persone = db4.Persones.Where(c => c.Contacts.ContactsId == contactsId).FirstOrDefault();
                    OldContacts = db4.Contacts.Where(c => c.ContactsId == contactsId).FirstOrDefault();

                    bool personeCompare = persone.ComparePersons(persone, _Contacts.Persone);
                    bool contactsCompare = _Contacts.CompareContacts(persone.Contacts, _Contacts.Contacts);
                    if (!personeCompare)
                    {
                        db4.Entry(persone).State = EntityState.Modified;
                        persone.CopyExeptVirtualIdPhoto(persone,_Contacts.Persone);
                    }
                    if (!contactsCompare)
                    {
                        db4.Entry(OldContacts).State = EntityState.Modified;
                        _Contacts.Contacts.CopyExceptVirtualAndId(OldContacts, _Contacts.Contacts);                        
                    }
                    Client client = new Client();
                    client = db4.Clients.Where(c => c.Persone.PersoneId == persone.PersoneId).FirstOrDefault();
                    db4.SaveChanges();
                    return client.ClientId;

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
            return 0;
        }
        //=========================================================================================================================    

        //========================================COMMAND FOR GET PREVIOS CLIENT ORDERS ==========================================
        private RelayCommand loadPreviosOrdersCommand;
        public RelayCommand LoadPreviosOrdersCommand => loadPreviosOrdersCommand ??
            (loadPreviosOrdersCommand = new RelayCommand(
                    (obj) =>
                    {
                        LoadPrreviosClientOrdersData(_Contacts.Contacts, Persone);
                    }
                    ));


        private void LoadPrreviosClientOrdersData(Contacts contacts, Persone persone)
        {

            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    int contactsId = contacts.CheckContacts(contacts);
                    if (contactsId == 0)
                    {
                        Msg = $"По этим контактным данным ни одного совпадения в базе данных нет. ";
                        return;
                    }

                    var client = db.Clients.Where(c => c.Persone.Contacts.ContactsId == contactsId).FirstOrDefault();
                    Persone = client.Persone;
                    _Contacts.Contacts = client.Persone.Contacts;
                    Client = client;
                    _University.University = client.Universities[0];
                    if (!persone.ComparePersons(persone, client.Persone))                       
                        LoadRecords(client.ClientId);
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

        private void LoadRecords(int clientId)
        {
            Records.Clear();
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var COrders = db.Orderlines.Where(c => c.Client.ClientId == clientId).ToList();
                    decimal TotalSumOrders = 0;
                    string authorNickName;
                    foreach (var item in COrders)
                    {
                        if (item.GetExecuteAuthor(item.Author) == null)
                            authorNickName = "---";
                        else
                            authorNickName = item.GetExecuteAuthor(item.Author).Persone.NickName;
                        Records record = new Records
                        {
                            RecordId = item.OrderLineId,
                            OrderNumber = item.OrderNumber,
                            DateOfReception = item.Dates.DateOfReception,
                            DeadLine = item.Dates.DeadLine,
                            DateDone = item.Dates.DateDone,
                            Price = item.Money.Price,
                            OrderCount = item.OrderCount,
                            Prepayment = item.Money.Prepayment,
                            Status = item.Status.StatusName,
                            TypeOfWork = item.WorkType.TypeOfWork,
                            AuthorNickName = authorNickName,
                            SubName = item.Direction.DirectionName
                        };
                        TotalSumOrders += item.Money.Price;
                        Records.Add(record);
                    }
                    //if (TotalSumOrders != 0)
                    Msg = $"Прежние заказы клиента.           Общая сумма всех заказов составляет  {TotalSumOrders} грн";
                    //else
                    //    Msg = $"Информация по прежним заказам пока не определена ";
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


        //========================================================================================================================


        //============================================Edit OrderCount field in Order ==============================================
        //to edit OrderCount in Order
        private void EditOrderCount(int orderId, int orderNumber)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {

                    var res = db.Orderlines.Where(c => c.OrderNumber == orderNumber).ToList();
                    int count = res.Count;
                    if (!CheckPreviosOrder(orderId))
                        foreach (var i in res)
                            i.OrderCount = count;
                    else
                        foreach (var i in res)
                            i.OrderCount = -count;
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

        //проверяем предыдущий заказ на наличие подзаказов. 
        // check the previous order for sub-orders
        private bool CheckPreviosOrder(int orderId)
        {
            //если заказ вообще первый, то проверка не имеет смысла
            // if the order is first, then the check does not make sense
            if (orderId <= 1)
                return false;
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res = db.Orderlines.Where(c => c.OrderLineId == (orderId - 1)).FirstOrDefault();
                    if (res != null && Math.Abs((res as OrderLine).OrderCount) > 1 && (res as OrderLine).OrderCount > 0)
                        return true;
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

        //===========================================================================================================================================================

        //===========================================COMMAND FOR EDIT ORDER ======================================
        private RelayCommand editOrderLineCommand;

        public RelayCommand EditOrderLineCommand => editOrderLineCommand ?? (editOrderLineCommand = new RelayCommand(
                    (obj) =>
                    {
                        EditOrderLine();
                    }
                    ));
        //редактирование при оформлении заказа
        //переделать надо нахрен все в этом методе
        private void EditOrderLine()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                { 
                    Order = db.Orderlines.Where(o=>o.OrderLineId==TMPStaticClass.CurrentOrder.OrderLineId).FirstOrDefault();
                    db.Entry(Order).State = EntityState.Modified;

                    if (Usver_curent != null && Usver_curent.UserId != Usver.UserId)
                        Order.User = db.Users.Where(e => e.UserId == Usver_curent.UserId).FirstOrDefault();
                    else
                        Order.User = db.Users.Where(e => e.UserId == Usver.UserId).FirstOrDefault();

                    if (!Persone.ComparePersons(Persone, Order.Client.Persone))
                    {
                        Persone pers = db.Persones.Where(p=> p.PersoneId== Order.Client.Persone.PersoneId).FirstOrDefault();
                        db.Entry(pers).State = EntityState.Modified;
                        Persone.CopyExeptVirtualIdPhoto(pers, Persone);
                    }
                    if (!_Contacts.CompareContacts(Persone.Contacts, Order.Client.Persone.Contacts))
                    {
                        Contacts contacts = db.Contacts.Where(c=>c.ContactsId== Order.Client.Persone.Contacts.ContactsId).FirstOrDefault();
                        db.Entry(contacts).State = EntityState.Modified;
                        _Contacts.Contacts.CopyExceptVirtualAndId(contacts,_Contacts.Contacts);                       
                    }
                    db.Entry(Order).State = EntityState.Modified;                    
                    if (_Dir.Dir.DirectionId != Order.Direction.DirectionId)
                        Order.Direction = db.Directions.Find(_Dir.Dir.DirectionId);
                    if (_WorkType.WorkType.WorkTypeId != Order.WorkType.WorkTypeId)
                        Order.WorkType = db.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    if (!Date.CompareDate(Date, Order.Dates))
                    {
                        Dates date = db.Dates.Where(d => d.DatesId == Order.Dates.DatesId).FirstOrDefault();
                        db.Entry(date).State = EntityState.Modified;
                        date.DateOfReception = Date.DateOfReception;
                        date.DeadLine = Date.DeadLine;
                    }
                    if (_Subj.Subj.SubjectId != Order.Subject.SubjectId)
                        Order.Subject = db.Subjects.Find(_Subj.Subj.SubjectId);
                    if (!Price.CompareMoney(Price, TMPStaticClass.CurrentOrder.Money))
                        {
                            Money money = db.Moneys.Where(m => m.MoneyId == Order.Money.MoneyId).FirstOrDefault();
                            db.Entry(money).State = EntityState.Modified;
                            money.Price = Price.Price;
                            money.Prepayment = Price.Prepayment;
                        }
                    if (RuleOrderLine._Status.Status.StatusId != Order.Status.StatusId)                       
                            Order.Status = db.Statuses.Find(RuleOrderLine._Status.Status.StatusId);
                    if (_University.University.UniversityId != Order.Client.Universities[0].UniversityId)                    
                        if (_University.University.UniversityId == 1)
                            Order.Client.Universities.Add(_University.University);                    
                        else
                        {
                            Order.Client.Universities.Clear();
                            Order.Client.Universities.Add(_University.University);
                        }
                    Order.Client.Course = Client.Course;
                    if (_Source.Source.SourceId!= Order.Source.SourceId)  
                        Order.Source = db.Sources.Find(_Source.Source.SourceId);
                    Order.User = db.Users.Where(e => e.UserId == Usver.UserId).FirstOrDefault();
                    Order.Saved = true;
                    db.SaveChanges();
                    dialogService.ShowMessage("Данные о заказе сохранены");
                    //4.2.20: can't clone Order yets
                    TMPStaticClass.CurrentOrder =(OrderLine)Order.Clone();
                    saved = true;
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
        //========================================================================================================================




        //this method not used
        //этот метод пока нигде не задейстован, но пусть типа будет
        //взят от сюдава https://professorweb.ru/my/entity-framework/6/level3/3_6.php
        public static void Update<TEntity>(TEntity entity, DbContext context) where TEntity : class
        {
            //// Настройки контекста
            //context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry<TEntity>(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        //==================================COMMAND FOR CLOSE WINDOW ============================================
        private RelayCommand closeWindowCommand;

        public RelayCommand CloseWindowCommand => closeWindowCommand ?? (closeWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        CloseWindow(obj as Window);
                    }
                    ));
        private void CloseWindow(Window window)
        {
            if (!Order.Saved&& CheckForChangesBeforClose())
                window.Close();
            else
            {
                if (!Order.Saved)
                {
                    if (dialogService.YesNoDialog("Не сохранены внесенные изменения\n" +
                                       "Точно закрыть окно?"))
                        window.Close();
                    
                }
                else
                if (Order.Saved && !evaluationSetWinner && AuthorsRecord.Author.AuthorId != 1)
                    if (dialogService.YesNoDialog("Не сохранены результаты экспрес распределния заказа\n" +
                                       "Сохранить перед закрытием?"))
                    {
                        SetFastRoolOrder();
                        window.Close();
                    }
                    else
                        window.Close();
            }
        }

       


        private bool CheckForChangesBeforClose()
        {
            if (TmpOrder.Direction.DirectionId == _dir.Dir.DirectionId &&
            TmpOrder.WorkType.WorkTypeId == _WorkType.WorkType.WorkTypeId &&
            TmpOrder.Subject.SubjectId == _Subj.Subj.SubjectId &&
            TmpOrder.Source.SourceId == _Source.Source.SourceId &&
            TmpOrder.Dates.CompareDate(TmpOrder.Dates, Date) &&
            TmpOrder.Money.CompareMoney(TmpOrder.Money, Price) &&
            TmpOrder.Status.StatusId == RuleOrderLine._Status.Status.StatusId &&
            Persone.ComparePersons(TmpOrder.Client.Persone, Persone) &&
            TmpOrder.Client.Course == Client.Course &&
            TmpOrder.Client.Universities[0].UniversityId == _University.University.UniversityId)
                return true;
            return false;
        }


        //========================================================================================================================



        //==================================COMMAND FOR CLOSE WINDOW =============================================================


        private RelayCommand cancelSaveContactsCommand;

        public RelayCommand CancelSaveContactsCommand => cancelSaveContactsCommand ?? (cancelSaveContactsCommand = new RelayCommand(
                    (obj) =>
                    {

                        CloseWindow(obj as Window);
                    }
                    ));

        ////call RuleOrderLineWindow
        //private RelayCommand newRuleOrderLineWindowCommand;
        //public RelayCommand NewRuleOrderLineWindowCommand =>
        //    newRuleOrderLineWindowCommand ?? (newRuleOrderLineWindowCommand = new RelayCommand(
        //            (obj) =>
        //            {
        //                RuleOrderLineWindow ruleOrderLineWindow = new RuleOrderLineWindow();
        //                showWindow.ShowDialog(ruleOrderLineWindow);

        //            }
        //            ));

        //=====================Command for call AddContactsWindow.xaml ======================================
        private RelayCommand newEditContactCommand;
        public RelayCommand NewEditContactCommand => newEditContactCommand ??
            (newEditContactCommand = new RelayCommand(
                    (obj) =>
                    {
                        _Contacts.TmpContacts = _Contacts.Contacts;
                        _Contacts.NewEditContacts(new AddContactsWindow(obj));
                    }
                    ));

        //========================================COMMAND FOR EDIT ORDER  ==========================================
        private RelayCommand setFastRoolOrderCommand;
        public RelayCommand SetFastRoolOrderCommand => setFastRoolOrderCommand ??
            (setFastRoolOrderCommand = new RelayCommand(
                    (obj) =>
                    {
                        SetFastRoolOrder();
                    }
                    ));
        bool evaluationSetWinner = false;

        private void SetFastRoolOrder()
        {
            //AuthorsRecord
            using (StudentuConteiner db = new StudentuConteiner())
            {
                if (AuthorsRecord.Author.AuthorId <= 1)
                {
                    dialogService.ShowMessage("Не задан автор. Сохранять нечего.");
                    return;
                }
                try
                {
                    db.Orderlines.Attach(Order);
                    db.Entry(Order).State = EntityState.Modified;
                                           
                        //if author not fake
                        Author = Order.GetExecuteAuthor(TMPStaticClass.CurrentOrder.Author);
                        if (Author != null)
                        {
                            Evaluation WinnerEvaluation = Author.GetWinnerEvaluation(Author);
                            db.Entry(WinnerEvaluation).State = EntityState.Modified;
                            WinnerEvaluation.Winner = false;
                        }
                        Author = db.Authors.Find(AuthorsRecord.Author.AuthorId);
                    
                    Evaluation evaluation = new Evaluation();                    
                    evaluation.Authors.Add(Author);
                    
                    //evaluation.AuthorDeadLine = FinalEvaluationRecord.DeadLine;                    
                    evaluation.Dates.Add(new Dates() { AuthorDeadLine= RuleOrderLine._Evaluation.FinalEvaluationRecord.DeadLine });

                    evaluation.Description = RuleOrderLine._Evaluation.FinalEvaluationRecord.EvaluateDescription;

                    //evaluation.AuthorPrice = FinalEvaluationRecord.Price;
                    evaluation.Moneys.Add(new Money() { AuthorPrice= RuleOrderLine._Evaluation.FinalEvaluationRecord.Price});

                    evaluation.Winner = true;                    
                    Author.Evaluation.Add(evaluation);                    
                    Order.Author.Add(Author);
                    Order.Evaluations.Add(evaluation);
                    Order.User = db.Users.Where(e => e.UserId == Usver.UserId).FirstOrDefault();
                    db.SaveChanges();                  
                    TMPStaticClass.CurrentOrder.Author.Add(Author);
                    RuleOrderLine.RoolMSG = $"Заказ выполняет {Author.Persone.NickName}";
                    evaluationSetWinner = true;
                    dialogService.ShowMessage("Заказ распределен");                   
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

        private void AllAuthorsCall()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var result = db.Authors.Include("Persone").ToList();
                    AuthorsRecord record;
                    foreach (Author item in result)
                    {
                        record = new AuthorsRecord
                        {
                            Author = new Author() { AuthorId = item.AuthorId },
                            Persone = new Persone()
                            {
                                PersoneId = item.Persone.PersoneId,
                                NickName = item.Persone.NickName,
                                Name = item.Persone.Name,
                                Surname = item.Persone.Surname
                            }
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
    }
}
