
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

namespace STUDENTU_1._06.ViewModel
{
  public partial class ForEditOrder : Helpes.ObservableObject
    {
        public ObservableCollection<Contacts> ContactsRecords { get; set; }       

        private Window editWindow;
        private Window editDirection;

       
         IDialogService dialogService;
        IShowWindowService showWindow;

        //==========================================PROPERTIES============================================
        //for set value of order number. Need to work more
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

       
       

        private Direction selectedRecord;
        public Direction SelectedRecord
        {
            get { return selectedRecord; }
            set
            {
                if (selectedRecord != value)
                {
                    selectedRecord = value;
                    OnPropertyChanged(nameof(SelectedRecord));
                }
            }
        }

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

        private Dates date;
        public Dates Date
        {
            get { return date; }
            set
            {
                if (date!= value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));
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

        public ForEditOrder(Window editWindow, DefaultShowWindowService showWindow,
           IDialogService dialogService)
        {
            AuthorsRecords = new ObservableCollection<AuthorsRecord>();
            DirRecords = new ObservableCollection<Direction>();
            SelectedAuthorsRecords = new ObservableCollection<AuthorsRecord> ();
            SubjRecords = new ObservableCollection<Subject>();
            SourcesRecords = new ObservableCollection<Source>();
            WorkTypesRecords = new ObservableCollection<WorkType>();
            ContactsRecords = new ObservableCollection<Contacts>();
            BlackListRecords = new ObservableCollection<BlackListHelpModel>();
            StatusRecords = new ObservableCollection<Status>();
          
            editWindow.Loaded += EditWindow_Loaded;
            this.showWindow = showWindow;
            this.dialogService = dialogService;
        }



        private void EditWindow_Loaded(object sender, RoutedEventArgs e)
        {

            Order = new OrderLine
            { OrderNumber = GetOrderNumber() };
            Persone = new Persone ();
            EvaluationRecord = new EvaluationRecord();
            PersoneDescription = new PersoneDescription();
            Source = new Source();
            WorkType = new WorkType();
            Dir = new Direction();
            Subj = new Subject();
            Price = new Money();
            Date = new Dates();
            Status = new Status();
            Author = new Author();
            SelectedExecuteAuthor = new Author();
            Contacts = new Contacts();
            SelectetdAuthorContacts = new Contacts();
            AuthorsRecord = new AuthorsRecord();

            Evaluation = new Evaluation();
            WinnerEvaluation = new Evaluation();

            FinalEvaluationRecord = new EvaluationRecord()
                                { DeadLine= new DateTime(1900, 1, 1) ,
                                  Price=0,
                                  EvaluateDescription=""};

            ExecuteAuthor = new AuthorsRecord();
            ExecuteAuthor.Persone.NickName = "не задан";

            LoadDirectionsData();//for load data to combobox DirList in EditOrder.xaml
            LoadSubjectsData();//for load data to combobox SubjList in EditOrder.xaml
            LoadWorkTypesData();//for load data to combobox WorkTypeList in EditOrder.xaml
            LoadSourcesData();//for load data to combobox SourcesList in EditOrder.xaml
            LoadStatusData();//for load data to combobox StatusList in RuleOrderLineWindow.xaml

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

        //===================THIS METHOD IS FOR DELETE RECORDS IN DIRECTIONS, SUBJECTS , WORKTYPES, SOURCES TABLES==============

        //if nameOfObject ="Direction" - delete records in directions, 
        //if nameOfObject ="Subject" - delete records in subjects
        //if nameOfObject ="WorkType" - delete records in worktypes
        //if nameOfObject ="Source" - delete records in source
        private void DeleteDirSubjWorkType(string nameOfObject)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res = db.Orderlines;
                    switch (nameOfObject)
                    {
                        case "WorkType"://delete WorkType
                            if (WorkType.WorkTypeId != 1)
                            {
                                if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                                {
                                    //changing DB
                                    //we find all the records in which we have the desired Id and make a replacement
                                    foreach (OrderLine order in res)
                                    {
                                        if (order.WorkType.WorkTypeId == WorkType.WorkTypeId)
                                            order.WorkType = db.WorkTypes.Find(new WorkType() { WorkTypeId = 1 }.WorkTypeId);
                                    }
                                    db.WorkTypes.Remove(db.WorkTypes.Find(WorkType.WorkTypeId));
                                    db.SaveChanges();

                                    //changing collection
                                    WorkTypesRecords.Remove(WorkType);
                                }
                            }
                            else
                                dialogService.ShowMessage("Нельзя удалить эту запись");
                            return;
                        case "Direction"://delete Direction
                            if (Dir.DirectionId != 1)
                            {
                                if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                                {
                                    //changing DB
                                    //we find all the records in which we have the desired Id and make a replacement
                                    foreach (OrderLine order in res)
                                    {
                                        if (order.Direction.DirectionId == Dir.DirectionId)
                                            order.Direction = db.Directions.Find(new Direction() { DirectionId = 1 }.DirectionId);
                                    }
                                    db.Directions.Remove(db.Directions.Find(Dir.DirectionId));
                                    db.SaveChanges();

                                    //changing collection
                                    DirRecords.Remove(Dir);
                                }
                            }
                            else
                                dialogService.ShowMessage("Нельзя удалить эту запись");
                            return;
                        case "Subject"://delete Subject
                            if (Subj.SubjectId != 1)
                            {
                                if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                                {
                                    //changing DB
                                    //we find all the records in which we have the desired Id and make a replacement
                                    foreach (OrderLine order in res)
                                    {
                                        if (order.Subject.SubjectId == Subj.SubjectId)
                                            order.Subject = db.Subjects.Find(new Subject() { SubjectId = 1 }.SubjectId);
                                    }
                                    db.Subjects.Remove(db.Subjects.Find(Subj.SubjectId));
                                    db.SaveChanges();

                                    //changing collection
                                    SubjRecords.Remove(Subj);
                                }
                            }
                            else
                                dialogService.ShowMessage("Нельзя удалить эту запись");
                            return;
                        case "Source"://delete Source
                            if (Source.SourceId != 1)
                            {
                                if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                                {
                                    //changing DB
                                    //we find all the records in which we have the desired Id and make a replacement
                                    foreach (OrderLine order in res)
                                    {
                                        if (order.Source.SourceId == Source.SourceId)
                                            order.Source = db.Sources.Find(new Source() { SourceId = 1 }.SourceId);
                                    }
                                    db.Sources.Remove(db.Sources.Find(Source.SourceId));
                                    db.SaveChanges();

                                    //changing collection
                                    SourcesRecords.Remove(Source);
                                }
                            }
                            else
                                dialogService.ShowMessage("Нельзя удалить эту запись");
                            return;
                        case "Status"://delete Status
                            if (Status.StatusId != 1)
                            {
                                if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                                {
                                    //changing DB
                                    //we find all the records in which we have the desired Id and make a replacement
                                    foreach (OrderLine order in res)
                                    {
                                        if (order.Status.StatusId == Status.StatusId)
                                            order.Status = db.Statuses.Find(new Status() { StatusId = 1 }.StatusId);
                                    }
                                    db.Statuses.Remove(db.Statuses.Find(Status.StatusId));
                                    db.SaveChanges();

                                    //changing collection
                                    WorkTypesRecords.Remove(WorkType);
                                }
                            }
                            else
                                dialogService.ShowMessage("Нельзя удалить эту запись");
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


        //===================THIS METHOD IS FOR ADD RECORDS IN DIRECTIONS, SUBJECTS , WORKTYPES, SOURCES TABLES==============

        //if nameOfObject ="Direction" - add records in directions, 
        //if nameOfObject ="Subject" - add records in subjects
        //if nameOfObject ="WorkType" - add records in worktypes
        //if nameOfObject ="Source" - add records in sources
        private void AddDirSubjWorkType(string nameOfObject)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    switch (nameOfObject)
                    {
                        case "WorkType"://add to WorkTypes
                            var res = db.WorkTypes.Any(o => o.TypeOfWork == WorkType.TypeOfWork);
                            if (!res)
                            {
                                if (!string.IsNullOrEmpty(WorkType.TypeOfWork))
                                {
                                    WorkType.TypeOfWork = WorkType.TypeOfWork.ToLower();
                                    db.WorkTypes.Add(WorkType);
                                    db.SaveChanges();
                                    WorkTypesRecords.Clear();
                                    LoadWorkTypesData();
                                    WorkType = new WorkType();

                                }
                                else
                                    return;
                            }
                            else
                                dialogService.ShowMessage("Уже есть такое название в базе данных");
                            return;
                        case "Direction"://add to Directions
                            var res1 = db.Directions.Any(o => o.DirectionName == Dir.DirectionName);
                            if (!res1)
                            {
                                if (!string.IsNullOrEmpty(Dir.DirectionName))
                                {
                                    Dir.DirectionName = Dir.DirectionName.ToLower();
                                    db.Directions.Add(Dir);
                                    db.SaveChanges();
                                    DirRecords.Clear();
                                    LoadDirectionsData();
                                    Dir = new Direction();

                                }
                                else
                                    return;
                            }
                            else
                                dialogService.ShowMessage("Уже есть такое название в базе данных");
                            return;
                        case "Subject"://add to Subjects
                            var res2 = db.Subjects.Any(o => o.SubName == Subj.SubName);
                            if (!res2)
                            {
                                if (!string.IsNullOrEmpty(Subj.SubName))
                                {
                                    db.Subjects.Add(Subj);
                                    db.SaveChanges();
                                    SubjRecords.Clear();
                                    LoadSubjectsData();
                                    Subj = new Subject();
                                }
                                else
                                    return;
                            }
                            else
                                dialogService.ShowMessage("Уже есть такое название в базе данных");
                            return;
                        case "Source"://add to Subjects
                            var res3 = db.Sources.Any(o => o.SourceName == Source.SourceName);
                            if (!res3)
                            {
                                if (!string.IsNullOrEmpty(Source.SourceName))
                                {
                                    db.Sources.Add(Source);
                                    db.SaveChanges();
                                    SourcesRecords.Clear();
                                    LoadSourcesData();
                                    Source = new Source();
                                }
                                else
                                    return;
                            }
                            else
                                dialogService.ShowMessage("Уже есть такое название в базе данных");
                            return;
                        case "Status"://add to Statuses
                            var res4 = db.Statuses.Any(o => o.StatusName == Status.StatusName);
                            if (!res4)
                            {
                                if (!string.IsNullOrEmpty(Status.StatusName))
                                {
                                    Status.StatusName = Status.StatusName.ToLower();
                                    db.Statuses.Add(Status);
                                    db.SaveChanges();
                                    StatusRecords.Clear();
                                    LoadStatusData();
                                    Status = new Status();

                                }
                                else
                                    return;
                            }
                            else
                                dialogService.ShowMessage("Уже есть такое название в базе данных");
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
                //catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
                //{
                //    dialogService.ShowMessage(ex.Message);
                //}
                //catch (System.Data.Entity.Core.EntityException ex)
                //{
                //    dialogService.ShowMessage(ex.Message);
                //}
            }
        }


        //===================THIS METHOD IS FOR EDIT RECORDS IN DIRECTIONS, SUBJECTS , WORKTYPES, SOURCES TABLES==============

        //if nameOfObject ="Direction" - edit records in directions, 
        //if nameOfObject ="Subject" - edit records in subjects
        //if nameOfObject ="WorkType" - edit records in worktypes
        //if nameOfObject ="Source" - edit records in sources
        private void EditDirSubjWorkType(string nameOfObject)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    switch (nameOfObject)
                    {
                        case "WorkType"://add to WorkTypes
                            var res = db.WorkTypes.Find(WorkType.WorkTypeId);
                            if (res != null)
                            {
                                //changing DB
                                res.TypeOfWork = WorkType.TypeOfWork.ToLower();
                                db.SaveChanges();
                            }
                            return;
                        case "Direction"://add to Directions
                            var res1 = db.Directions.Find(Dir.DirectionId);
                            if (res1 != null)
                            {
                                //changing DB
                                res1.DirectionName = Dir.DirectionName.ToLower();
                                db.SaveChanges();
                            }
                            return;
                        case "Subject"://add to Subjects
                            var res2 = db.Subjects.Find(Subj.SubjectId);
                            if (res2 != null)
                            {
                                //changing DB
                                res2.SubName = Subj.SubName;
                                db.SaveChanges();
                            }
                            return;
                        case "Source"://add to Source
                            var res3 = db.Sources.Find(Source.SourceId);
                            if (res3 != null)
                            {
                                //changing DB
                                res3.SourceName = Source.SourceName;
                                db.SaveChanges();
                            }
                            return;
                        case "Status"://add to WorkTypes
                            var res4 = db.Statuses.Find(Status.StatusId);
                            if (res4 != null)
                            {
                                //changing DB
                                res4.StatusName = Status.StatusName.ToLower();
                                db.SaveChanges();
                            }
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


       

        //==================================== COMMAND FOR SAVE NEW ORDER ====================================

        private RelayCommand createNewOrderLine;
        public RelayCommand CreateNewOrderLine => createNewOrderLine ?? (createNewOrderLine = new RelayCommand(
                    (obj) =>
                    {
                        SaveNewOrder();
                    }
                    ));

        private void SaveNewOrder()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {


                    //добавили авторов из AuthorsRecords в Evaluation
                    foreach (var item in AuthorsRecords)
                    {
                        Evaluation.Authors.Add(new Author() {
                            AuthorId=item.AuthorRecordId,
                            Sourse=item.Sourse
                    });
                    }
                    //добавили оценки авторам  Evaluation из AuthorsRecord.EvaluationRecords
                    foreach (var i in Evaluation.Authors)
                        {
                            foreach (var item in AuthorsRecord.EvaluationRecords)
                            {
                            
                                    Evaluation.Moneys.Add(new Money() { Price = item.Price });
                                    Evaluation.Description = item.EvaluateDescription;
                                    Evaluation.Dates.Add(new Dates() { DeadLine = item.DeadLine });
                                    i.Evaluation.Add(Evaluation);                               
                            }                       
                        }
                    

                    Order.ExecuteAuthor = db.Authors.Find(new Author() { AuthorId = SelectedExecuteAuthor.AuthorId }.AuthorId);

                    if (Order.ExecuteAuthor is null)
                    {
                        // set a default entry to author field
                        Order.Author = db.Authors.Find(new Author() { AuthorId = 1 }.AuthorId);
                        Order.Author.Evaluation.Add(Evaluation);
                    }
                    else
                    {
                        Order.Author = db.Authors.Find(new Author() { AuthorId = SelectedExecuteAuthor.AuthorId }.AuthorId);
                        Order.Author.Evaluation.Add(Evaluation);
                    }

                    //in SetSelectEvaluation(), whot in EvaluationClass.cs, we get :
                    //Order.ExecuteAuthorComments = item.EvaluateDescription;
                    //Order.ExecuteAuthorPrice = item.Price;
                    //Order.ExecuteAuthorDeadLine = item.DeadLine;

                    //эта ветка уже нафиг не нужна, по идее
                    //foreach (var item in AuthorsRecord.EvaluationRecords)
                    //{
                    //    if (item.FinalEvaluation==true)
                    //    {
                    //        WinnerEvaluation.Moneys.Add(new Money() { Price = item.Price });
                    //        WinnerEvaluation.Description = item.EvaluateDescription;
                    //        WinnerEvaluation.Dates.Add(new Dates() { DeadLine = item.DeadLine });
                    //        SelectedExecuteAuthor.Evaluation.Add(WinnerEvaluation);
                    //        break;
                    //    }
                    //}
                    
                    Persone.Contacts=Contacts;
                    Order.Direction = db.Directions.Find(Dir.DirectionId);
                    Order.Client=new Client() { Persone=Persone};
                    Order.WorkType = db.WorkTypes.Find(WorkType.WorkTypeId);
                    Order.Dates = Date;
                    Order.Subject = db.Subjects.Find(Subj.SubjectId); ;
                    Order.Money = Price;
                    //if (SelectedExecuteAuthor is null)
                    //    // set a default entry to author field
                    //    Order.Author = db.Authors.Find(new Author() { AuthorId = 1 }.AuthorId);
                    //else
                    //    //set realy selected author
                    //    Order.Author = SelectedExecuteAuthor;
                    if (Status is null)
                        // set a default entry to status field
                        Order.Status = db.Statuses.Find(new Status() { StatusId = 1 }.StatusId); 
                    else
                        //set realy selected status
                        Order.Status = Status;

                    db.Orderlines.Add(Order);
                    
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

        //==================================COMMAND FOR CLOSE WINDOW ==========================
        private RelayCommand closeWindowCommand;

        public RelayCommand CloseWindowCommand => closeWindowCommand ?? (closeWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        Window window = obj as Window;
                        window.Close();                       
                    }
                    ));
        //========================================================================================================================


    }
}
