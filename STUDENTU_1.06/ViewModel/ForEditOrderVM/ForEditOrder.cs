using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public partial class ForEditOrder : Helpes.ObservableObject
    {
        public ObservableCollection<Contacts> ContactsRecords { get; set; }

        private Window editWindow;
        private Window editDirection;

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

      
        public ForEditOrder()
        {

            DefaultLoadData();
            Order = new OrderLine { OrderNumber = GetOrderNumber() };
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();            
        }

        private void DefaultLoadData()
        {
            ContactsRecords = new ObservableCollection<Contacts>();
            BlackListRecords = new ObservableCollection<BlackListHelpModel>();
            Author = new Author();
            _Contacts = new _Contacts();
            Client = new Client();
            Date = new Dates();
            _Dir = new _Direction();
            Persone = new Persone();
            PersoneDescription = new PersoneDescription();
            Price = new Money();
            _Status = new _Status();
            _Subj = new _Subject();
            _Source = new _Source();
            _WorkType = new _WorkType();
        }


        //=================================METHODS FOR PREVIOS LOAD TO CONTROLS OF EditOrder.xaml =====

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

                        //result = (from m in db.Orderlines.OrderBy(p => p.OrderNumber) select m.OrderNumber).ToList().Last();
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


        //==================================== COMMAND FOR SAVE NEW ORDER ====================================

        private RelayCommand createNewOrderLine;
        public RelayCommand CreateNewOrderLine => createNewOrderLine ?? (createNewOrderLine = new RelayCommand(
                    (obj) =>
                    {
                      
                        SaveOrderChanges();
                    }
                    ));
       
        private void SaveOrderChanges()
        {
            if (saved)
                dialogService.ShowMessage("Заказ уже сохранен");
            else
                SaveNewOrder();
        }

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
                    Order.Status = db.Statuses.Find(_Status.Status.StatusId);
                    Order.Saved = true;

                    //ищем совпададения по полям контактов Person в БД. Если "0", то совпадений не найдено
                    //если совпадение есть, то получаем Id нужной записи в Contacts
                    // look for matches on the fields of the Person contacts in the database. If "0", then no matches were found
                    // if there is a match, then we get the Id of the desired entry in Contacts

                    //нашли Id прежнего Contscts
                    // found Id of former Contscts
                    int contactId =0;
                    contactId = _Contacts.Contacts.CheckContacts(_Contacts.Contacts);
                    if (contactId == 0)
                    {
                        Persone.Contacts = _Contacts.Contacts;
                        Persone.PersoneDescription = PersoneDescription;
                        Order.Client = new Client() { Persone = Persone };
                    }
                    else
                    {
                        var persone = db.Persones.Where(o => o.Contacts.ContactsId == contactId).FirstOrDefault();
                        bool ContactsCompare = _Contacts.CompareContacts(_Contacts.Contacts,persone.Contacts);
                        bool PersonFirsDataCompare = Persone.ComparePersons(Persone, persone);
                        
                        SaveOrderPartAfterCheckContacts(contactId, 0, ContactsCompare, PersonFirsDataCompare);
                        Order.Client = db.Clients.Find(Client.ClientId);
                        if (CancelSaveOrder)
                            return;
                    }
                    
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
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    db.Orderlines.Attach(Order);
                    db.Dates.Add(Date);
                    db.Directions.Attach(_Dir.Dir);
                    db.Contacts.Attach(_Contacts.Contacts);
                    db.Orderlines.Attach(Order);
                    db.WorkTypes.Attach(_WorkType.WorkType);
                    //db.Moneys.Add(Price);
                    //db.Clients.Add(Client);
                    db.Moneys.Attach(Price);
                    db.Clients.Attach(Client);
                    db.PersoneDescriptions.Attach(PersoneDescription);
                    db.Persones.Attach(Persone);
                    db.Subjects.Attach(_Subj.Subj);
                    db.Statuses.Attach(_Status.Status);
                    db.Sources.Attach(_Source.Source);

                    Order.Direction = db.Directions.Find(_Dir.Dir.DirectionId);
                    Order.WorkType = db.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    Order.Subject = db.Subjects.Find(_Subj.Subj.SubjectId);
                    Order.Source = db.Sources.Find(_Source.Source.SourceId);
                    Order.Dates = Date;
                    Order.Money = Price;
                    Order.Status = db.Statuses.Find(_Status.Status.StatusId);
                    Order.Saved = true;

                    //тут нужно проверяем текущие контакты с контатными данными родительского заказа
                    //вдруг пользователь изменил чего?...
                    // here we need to check the current contacts with the contact data of the parent order
                    // suddenly the user changed something? ... (
                    var order = db.Orderlines.Where(o => o.ParentId == TMPStaticClass.CurrentOrder.ParentId).FirstOrDefault();
                    bool ContactsCompare = _Contacts.CompareContacts(_Contacts.Contacts, order.Client.Persone.Contacts);
                    bool PersonFirsDataCompare = Persone.ComparePersons(Persone, order.Client.Persone);
                    if (!ContactsCompare || !PersonFirsDataCompare)
                        {
                            SaveOrderPartAfterCheckContacts(0, order.ParentId, ContactsCompare, PersonFirsDataCompare);
                            if (CancelSaveOrder)
                                return;
                        }

                    else
                    {
                        Client = db.Clients.Find(TMPStaticClass.CurrentOrder.Client.ClientId);
                        Client.OrderLine.Add(Order);
                        Order.Client = db.Clients.Find(Client.ClientId);

                    }

                    db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Orderlines.Add(Order);                    

                    db.SaveChanges();

                    db.Entry(Order).State = EntityState.Modified;
                    Order.ParentId = TMPStaticClass.CurrentOrder.ParentId;
                    db.SaveChanges();

                    EditOrderCount(TMPStaticClass.CurrentOrder.OrderLineId, TMPStaticClass.CurrentOrder.OrderNumber);
                    TMPStaticClass.CurrentOrder = (OrderLine)Order.Clone();
                    saved = true;
                    doubleSave = false;                    
                    db.Entry(Order).State = EntityState.Detached;
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
        private void SaveOrderPartAfterCheckContacts(int contactsId, int doubleId, 
                                                    bool contactsCompare, bool personeCompare)
        {
            

            using (StudentuConteiner db = new StudentuConteiner())
            {
               
                try
                {
                    Persone persone = new Persone();
                    Contacts OldContacts = new Contacts();

                    if (contactsCompare && personeCompare)
                    {
                        Client = db.Clients.Where(c => c.Persone.Contacts.ContactsId == contactsId).FirstOrDefault();                       
                        Client = db.Clients.Find(Client.ClientId);
                        Client.OrderLine.Add(Order);

                        // к  Order.Client мы добавляем полученного Client уже в основном методе SaveNewOrder
                        //так как еще не хватает навыков работы с каонтекстом , чтоб сделать это  красиво
                        //ниже приведенна конструкция тут не работает. 
                        // to Order.Client we add the received Client already in the main SaveNewOrder method
                        // since there are still not enough skills to work with kaontext, to make it beautiful
                        // the construction below does not work here.

                        // Order.Client = db.Clients.Find(Client.ClientId);

                        return;                       
                    }
                    else
                    {
                       
                        if (doubleId == 0)
                        {
                            //ветка первичного сохранения
                            dialogService.ShowMessage("Уже есть клиент с такими контактными данными.\n" +
                                                 "Вносим правку в базу данных");                           
                            persone = db.Persones.Where(c => c.Contacts.ContactsId == contactsId).FirstOrDefault();
                            OldContacts = db.Contacts.Where(c => c.ContactsId == contactsId).FirstOrDefault();
                        }
                        else
                        {
                            //ветка doublesave
                            dialogService.ShowMessage("Контактные данные в подзаказе не совпадают с теми,.\n" +
                                                 "которые были в исходном заказе. ");                            
                            persone = TMPStaticClass.CurrentOrder.Client.Persone;
                            OldContacts = persone.Contacts;
                        }
                        _Contacts.OldPersoneCompare =(Persone)this.persone.CloneExceptVirtual();
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
                      

                        db.Entry(persone).State = EntityState.Modified;
                        personeCompare = persone.ComparePersons(persone, _Contacts.Persone);
                        if (!personeCompare)
                        {                           
                            persone.Name = _Contacts.Persone.Name;
                            persone.Surname = _Contacts.Persone.Surname;
                            persone.Patronimic = _Contacts.Persone.Patronimic;
                            persone.Sex = _Contacts.Persone.Sex;
                        }

                        contactsCompare = _Contacts.CompareContacts(persone.Contacts, _Contacts.Contacts);
                        if (!contactsCompare)
                        {
                            Contacts tmpContacts = (Contacts)this._Contacts.Contacts.Clone();                                
                            int tmpId = OldContacts.ContactsId;
                            db.Entry(OldContacts).State = EntityState.Modified;
                            OldContacts = tmpContacts;
                            OldContacts.ContactsId = tmpId;                            
                            persone.Contacts = db.Contacts.Find(OldContacts.ContactsId);
                            OldContacts.Persone.Add(persone);
                        }

                        if (contactsCompare && personeCompare)
                        {
                            Client = db.Clients.Where(c => c.Persone.Contacts.ContactsId == contactsId).FirstOrDefault();
                            Client = db.Clients.Find(Client.ClientId);
                            Client.OrderLine.Add(Order);
                            return;
                        }

                        Client = db.Clients.Where(c => c.Persone.PersoneId == persone.PersoneId).FirstOrDefault();
                        db.Entry(Client).State = EntityState.Modified;

                        Client.Persone = db.Persones.Find(persone.PersoneId);
                        //Client.OrderLine.Add(Order);
                        //Order.Client = db.Clients.Find(Client.ClientId);



                        // Client = db.Clients.Where(c => c.Persone.Contacts.ContactsId == contactsId).FirstOrDefault();

                        //Client = db.Clients.Find(Client.ClientId);
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


        //to edit OrderCount in Order
        private void EditOrderCount(int orderId, int orderNumber)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    
                    var res = db.Orderlines.Where(c => c.OrderNumber == orderNumber).ToList();
                    int count = res.Count;
                    
                    if (!CheckPreviosOrder(orderId, orderNumber))
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
        private bool CheckPreviosOrder(int orderId, int oredrNumber)
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
                    
                    if (res != null && (res as OrderLine).OrderNumber == oredrNumber|| res != null && !(res as OrderLine).ParentOrder)
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

        //EditOrderLineCommand

        //===========================================COMMAND FOR EDIT ORDER ======================================
        private RelayCommand editOrderLineCommand;

        public RelayCommand EditOrderLineCommand => editOrderLineCommand ?? (editOrderLineCommand = new RelayCommand(
                    (obj) =>
                    {
                        EditOrderLine();
                    }
                    ));
        //========================================================================================================================

        private void EditOrderLine()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    db.Entry(Order).State = EntityState.Modified;
                    Persone.Contacts = _Contacts.Contacts;
                    Order.Client = new Client() { Persone = Persone };
                    Order.Direction = db.Directions.Find(_Dir.Dir.DirectionId);
                    Order.WorkType = db.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    Order.Dates = Date;
                    Order.Subject = db.Subjects.Find(_Subj.Subj.SubjectId); ;
                    Order.Money = Price;
                    _Status.Status = db.Statuses.Find(new Status() { StatusId = 2 }.StatusId);
                    Order.Status = _Status.Status;
                    Order.Source = db.Sources.Find(_Source.Source.SourceId);
                    Order.Saved = true;
                    db.SaveChanges();
                    dialogService.ShowMessage("Данные о заказе сохранены");
                    TMPStaticClass.CurrentOrder = Order;
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


        //this method not used
        //этот метод пока нигде не задейстован, но пусть типа будет
        //взят от сюдава https://professorweb.ru/my/entity-framework/6/level3/3_6.php
        public static void Update<TEntity>(TEntity entity, DbContext context)
    where TEntity : class
        {
            //// Настройки контекста
            //context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry<TEntity>(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        //==================================COMMAND FOR CLOSE WINDOW ==========================
        private RelayCommand closeWindowCommand;

        public RelayCommand CloseWindowCommand => closeWindowCommand ?? (closeWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        CloseWindow(obj as Window);
                    }
                    ));
        //========================================================================================================================

        private void CloseWindow(Window window)
        {
            window.Close();
        }


        //==================================COMMAND FOR CLOSE WINDOW ==========================
        private RelayCommand cancelSaveContactsCommand;

        public RelayCommand CancelSaveContactsCommand => cancelSaveContactsCommand ?? (cancelSaveContactsCommand = new RelayCommand(
                    (obj) =>
                    {

                        CloseWindow(obj as Window);
                    }
                    ));

        //call RuleOrderLineWindow
        private RelayCommand newRuleOrderLineWindowCommand;
        public RelayCommand NewRuleOrderLineWindowCommand =>
            newRuleOrderLineWindowCommand ?? (newRuleOrderLineWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        RuleOrderLineWindow ruleOrderLineWindow = new RuleOrderLineWindow();
                        showWindow.ShowDialog(ruleOrderLineWindow);

                    }
                    ));

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


   

    }
}
