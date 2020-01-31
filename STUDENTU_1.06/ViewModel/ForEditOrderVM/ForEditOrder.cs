﻿using System;
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
    public partial class ForEditOrder : ObservableObject
    {
        public ObservableCollection<Contacts> ContactsRecords { get; set; }
        public ObservableCollection<Records> Records { get; set; }

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

        //для выода сообщение об общей потраченной ранее заказчиком сумме, в спарке 
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
                    OnPropertyChanged(nameof(msg));
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
            Records = new ObservableCollection<Records>();
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
                dialogService.ShowMessage("Заказ уже был сохранен");
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
                    //db2.Dates.Attach(Date);
                    //db2.Directions.Attach(_Dir.Dir);
                    db2.Contacts.Attach(_Contacts.Contacts);                   
                    //db2.WorkTypes.Attach(_WorkType.WorkType);                    
                    //db2.Moneys.Attach(Price);                   
                    db2.PersoneDescriptions.Attach(PersoneDescription);
                    db2.Persones.Attach(Persone);
                    //db2.Subjects.Attach(_Subj.Subj);
                    //db2.Statuses.Attach(_Status.Status);
                    //db2.Sources.Attach(_Source.Source);

                    Order.Direction = db2.Directions.Find(_Dir.Dir.DirectionId);
                    Order.WorkType = db2.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    Order.Subject = db2.Subjects.Find(_Subj.Subj.SubjectId);
                    Order.Source = db2.Sources.Find(_Source.Source.SourceId);
                    Order.Dates = Date;
                    Order.Money = Price;
                    Order.Status = db2.Statuses.Find(_Status.Status.StatusId);
                    Order.Saved = true;

                   
                    //тут нужно проверяем текущие контакты с контатными данными родительского заказа
                    //вдруг пользователь изменил чего?...
                    // here we need to check the current contacts with the contact data of the parent order
                    // suddenly the user changed something? ... (
                    //var order = db2.Orderlines.Where(o => o.ParentId == TMPStaticClass.CurrentOrder.ParentId).FirstOrDefault();
                    //var order = db2.Orderlines.Find(TMPStaticClass.CurrentOrder.ParentId);
                    bool ContactsCompare = _Contacts.CompareContacts(_Contacts.Contacts, TMPStaticClass.CurrentOrder.Client.Persone.Contacts);
                    bool PersonFirsDataCompare = Persone.ComparePersons(Persone, TMPStaticClass.CurrentOrder.Client.Persone);
                    if (!ContactsCompare || !PersonFirsDataCompare)
                    {
                        
                       // db2.Entry(Client).State = EntityState.Detached;
                        SaveOrderPartAfterCheckContacts(0, TMPStaticClass.CurrentOrder.ParentId, ContactsCompare, PersonFirsDataCompare);
                        if (doubleSaveCheck)
                        {
                            Client = db2.Clients.Find(TMPStaticClass.CurrentOrder.Client.ClientId);
                            Client.OrderLine.Add(Order);
                          //  Order.Client = db2.Clients.Find(Client.ClientId);
                            doubleSaveCheck = false;
                        }
                        if (doubleSaveCheckDif)
                        {                            
                            int clientId = ChangeContactsPersonDataAndReturnClient(_Contacts);
                            Client = db2.Clients.Find(clientId);
                            Client.OrderLine.Add(Order);
                           // Order.Client = db2.Clients.Find(Client.ClientId);
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
        //осnвил все контактные данные прежними
        bool doubleSaveCheck = false;
        //флаг для возврата в основной контекст , если пользователь
        //заменил прежние контактные данные на другие
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
                        //так как еще не хватает навыков работы с каонтекстом , чтоб сделать это  красиво
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
                        {
                            //ветка первичного сохранения
                            //first save branch
                            dialogService.ShowMessage("Уже есть клиент с такими контактными данными.\n" +
                                                 "Вносим правку в базу данных");                           
                            //persone = db3.Persones.Where(c => c.Contacts.ContactsId == contactsId).FirstOrDefault();
                            //OldContacts = db3.Contacts.Where(c => c.ContactsId == contactsId).FirstOrDefault();
                        }
                        else
                        {
                            //ветка doublesave
                            //doublesave branch
                            dialogService.ShowMessage("Контактные данные в подзаказе не совпадают с теми,.\n" +
                                                 "которые были в исходном заказе. ");
                            //persone = TMPStaticClass.CurrentOrder.Client.Persone;
                            contactsId = TMPStaticClass.CurrentOrder.Client.Persone.Contacts.ContactsId;
                            //persone = db3.Persones.Where(c => c.Contacts.ContactsId == contactsId).FirstOrDefault();
                            ////OldContacts = persone.Contacts;
                            //OldContacts = db3.Contacts.Where(c => c.ContactsId == contactsId).FirstOrDefault();
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
                            persone.Name = _Contacts.Persone.Name;
                            persone.Surname = _Contacts.Persone.Surname;
                            persone.Patronimic = _Contacts.Persone.Patronimic;
                            persone.Male = _Contacts.Persone.Male;
                            persone.Female = _Contacts.Persone.Female;
                        }
                        
                        if (!contactsCompare)
                        {
                            db3.Entry(OldContacts).State = EntityState.Modified;
                            OldContacts.Phone1 = _Contacts.Contacts.Phone1;
                            OldContacts.Phone2 = _Contacts.Contacts.Phone2;
                            OldContacts.Phone3 = _Contacts.Contacts.Phone3;
                            OldContacts.Email1 = _Contacts.Contacts.Email1;
                            OldContacts.Email2 = _Contacts.Contacts.Email2;
                            OldContacts.VK = _Contacts.Contacts.VK;
                            OldContacts.FaceBook = _Contacts.Contacts.FaceBook;
                            OldContacts.Skype = _Contacts.Contacts.Skype;
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
                        persone.Name = _Contacts.Persone.Name;
                        persone.Surname = _Contacts.Persone.Surname;
                        persone.Patronimic = _Contacts.Persone.Patronimic;
                        persone.Male = _Contacts.Persone.Male;
                        persone.Female = _Contacts.Persone.Female;

                    }
                    if (!contactsCompare)
                    {
                        db4.Entry(OldContacts).State = EntityState.Modified;
                        OldContacts.Phone1 = _Contacts.Contacts.Phone1;
                        OldContacts.Phone2 = _Contacts.Contacts.Phone2;
                        OldContacts.Phone3 = _Contacts.Contacts.Phone3;
                        OldContacts.Email1 = _Contacts.Contacts.Email1;
                        OldContacts.Email2 = _Contacts.Contacts.Email2;
                        OldContacts.VK = _Contacts.Contacts.VK;
                        OldContacts.FaceBook = _Contacts.Contacts.FaceBook;
                        OldContacts.Skype = _Contacts.Contacts.Skype;
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
                    if (!persone.ComparePersons(persone, client.Persone))                    
                        dialogService.ShowMessage("Ранее этот клиент оформлял заказы под другими контактными данными\n" +
                            "При сохранении заказа будут предложены варианты дальнейших действий");
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
                    if (TotalSumOrders != 0)
                        Msg = $"Прежние заказы клиента.           Общая сумма всех заказов составляет  {TotalSumOrders} грн";
                    else
                        Msg = $"Информация по прежним заказам пока не определена ";
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
            window.Close();
        }

        public void _CloseWindow()
        {
            throw new NotImplementedException();
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
