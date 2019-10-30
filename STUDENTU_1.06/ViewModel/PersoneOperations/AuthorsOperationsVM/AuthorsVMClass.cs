using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;

using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views;
using STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace STUDENTU_1._06.ViewModel.PersoneOperations.AuthorsOperationsVM
{
   public partial class AuthorsVMClass : Helpes.ObservableObject
    {

        //экспериментируем с хранение изображения в виде массива байтов 
        private byte[] photo;
        public byte[] Photo
        {
            get { return photo; }
            set
            {
                if (value != photo)
                {
                    photo = value;                    
                    OnPropertyChanged(nameof(Photo));
                }
            }
        }

        //for edit DB table contacts
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

        //for display default image
        private string defaultPhoto;
        public string DefaultPhoto
        {
            set
            {
                if (defaultPhoto != value)
                {
                    defaultPhoto = value;
                    OnPropertyChanged(nameof(DefaultPhoto));
                }
            }
            get {return "/STUDENTU_1.06;component/Images/" + defaultPhoto;}
        }        

        
        //simple "musthave" because whithot it we can't do any operations in this class
        //in particular, in order to be able to make further changes to the Persone database table
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


        //for operations with records in AuthorStatus table
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

        //to be able to make further changes to the Authors database table
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

        //to be able to make further changes to the Dates database table
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

        //to be able to make further changes to the Subjects database table
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

        //to be able to make further changes to the Directions database table
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


        //to be able to make further changes to the PersoneDescription database table
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

        IDialogService dialogService;
        IShowWindowService showWindow;
        
        public AuthorsVMClass(Window editWindow, DefaultShowWindowService showWindow,
           IDialogService dialogService)
        {
            this.showWindow = showWindow;
            this.dialogService = dialogService;
            editWindow.Loaded += EditWindow_Loaded;

        }

        private void EditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultPhoto = "default_avatar.png";          

            Author = new Author();
            _AuthorStatus = new _AuthorStatus();
            Contacts = new Contacts();
            Date = new Dates();
            _Dir = new _Direction();
            Persone = new Persone();
            PersoneDescription = new PersoneDescription();
            _Subj = new _Subject();
            
        }

        //==================================Command for add new photo to persone profile================
        private RelayCommand openFileDialogCommand;
        public RelayCommand OpenFileDialogCommand => openFileDialogCommand ?? (openFileDialogCommand = new RelayCommand(
                    (obj) =>
                    {
                        PathToFile();
                    }
                    ));
        private void PathToFile()
        {   
            string path;            
            path = dialogService.OpenFileDialog("C:\\");
            Photo = File.ReadAllBytes(path);
        }

        //=====================Command for call AuthorRatingEditWindow.xaml======================================
        private RelayCommand newEditRating;
        public RelayCommand NewEditRating => newEditRating ?? (newEditRating = new RelayCommand(
                    (obj) =>
                    {
                        AuthorRatingEditWindow editRating = new AuthorRatingEditWindow(obj);
                        editRating.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(editRating);
                    }
                    ));

        //==================================COMMAND FOR SET AUTHOR RATING ==========================
        private RelayCommand createRatingCommand;
        public RelayCommand CreateRatingCommand => createRatingCommand?? (createRatingCommand = new RelayCommand(
                    (obj) =>
                    {
                        AuthorRatingCreate();
                    }
                    ));

        private void AuthorRatingCreate()
        {
            Author.Rating = Author.RatingCreate();
            int i = 4;
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


        //=====================Command for call AddContactsWindow.xaml ======================================
        private RelayCommand newEditContactsCommand;
        public RelayCommand NewEditContactsCommand => newEditContactsCommand ??
            (newEditContactsCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddContactsWindow addContactsWindow = new AddContactsWindow(obj);
                        addContactsWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(addContactsWindow);
                    }
                    ));

        //====================================Save contact COMMAND================================

        private RelayCommand saveContactCommand;
        public RelayCommand SaveContactCommand => saveContactCommand ?? (saveContactCommand = new RelayCommand(
                    (obj) =>
                    {
                        //тут над придумать, шо толком сюад впилить, 
                        //т.к. банальный вывод сообщения о том, гр все сохраненор - это не айс
                        
                        
                        //тут у нас просто вывод сообщения , т.к. все данные и так привязаны к нужным 
                        //полям в окне редактирования + сохранение данных о контактах происходит
                        
                        //в SaveNewOrder
                        // here we just have a message output, because all data is already tied to the right
                        // fields in the edit window + saving contact data occurs
                        // in SaveNewOrder

                        //а это делаем пока на всякий случай
                       // ContactsRecords.Add(Contacts);
                        dialogService.ShowMessage("Данные сохранены");
                    }
                    ));
        //======================================================================================================================== 

        private RelayCommand cencelSaveAuthorDataCommand;
        public RelayCommand CencelSaveAuthorDataCommand => cencelSaveAuthorDataCommand ??
            (cencelSaveAuthorDataCommand = new RelayCommand(
                    (obj) =>
                    {
                        CencelSave();
                    }
                    ));
        private void CencelSave()
        {
            //эта команда в принципе нахер не нужна, потому логику придумывать смысла не т. 
            //мож потом будет какая мысля...
        }


        //====================================COMMAND FOR CALL AuthorDirectionsWindow.XAML =========================

        private RelayCommand addDirectionsCommand;
        public RelayCommand AddDirectionsCommand => addDirectionsCommand ??
            (addDirectionsCommand = new RelayCommand(
                    (obj) =>
                    {
                        AuthorDirectionsWindow authorDirectionWindow = new AuthorDirectionsWindow(obj);
                        authorDirectionWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(authorDirectionWindow);

                    }
                    ));

    
        //====================================COMMAND FOR CALL AuthorSubjectWindow.XAML =========================

        private RelayCommand addSubjectsCommand;
        public RelayCommand AddSubjectsCommand => addSubjectsCommand ??
            (addSubjectsCommand = new RelayCommand(
                    (obj) =>
                    {
                        AuthorSubjectWindow authorSubjectWindow = new AuthorSubjectWindow(obj);
                        authorSubjectWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(authorSubjectWindow);
                    }
                    ));



        // =============================================COMMAND FOR SAVE AUTHOR DATA =====================================

        private RelayCommand saveAuthorDataCommand;
        public RelayCommand SaveAuthorDataCommand => saveAuthorDataCommand ??
            (saveAuthorDataCommand = new RelayCommand(
                    (obj) =>
                    {

                    }
                    ));
        private void SaveAuthorData()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //Order.ExecuteAuthor = db.Authors.Find(new Author() { AuthorId = SelectedExecuteAuthor.AuthorId }.AuthorId);
                    string errValidation = ValidAuthorDataCheck()+"\n\n Данные НЕ были сохранены";
                    if (errValidation != null)
                    {
                        dialogService.ShowMessage(errValidation);
                        return;
                    }
                    else
                    {
                        ////добавили авторов из AuthorsRecords в Evaluation
                        //foreach (var item in AuthorsRecords)
                        //{
                        //    Evaluation.Authors.Add(new Author()
                        //    {
                        //        AuthorId = item.AuthorRecordId,
                        //        Source = item.Source
                        //    });
                        //}
                        ////добавили оценки авторам  Evaluation из AuthorsRecord.EvaluationRecords
                        //foreach (var i in Evaluation.Authors)
                        //{
                        //    foreach (var item in AuthorsRecord.EvaluationRecords)
                        //    {

                        //        Evaluation.Moneys.Add(new Money() { Price = item.Price });
                        //        Evaluation.Description = item.EvaluateDescription;
                        //        Evaluation.Dates.Add(new Dates() { DeadLine = item.DeadLine });
                        //        i.Evaluation.Add(Evaluation);
                        //    }
                        //}

                        //Order.ExecuteAuthor = db.Authors.Find(new Author() { AuthorId = SelectedExecuteAuthor.AuthorId }.AuthorId);
                        //Order.Author = db.Authors.Find(new Author() { AuthorId = SelectedExecuteAuthor.AuthorId }.AuthorId);
                        //Order.Author.Evaluation.Add(Evaluation);
                    }

                   

                    Persone.Contacts = Contacts;
                    Author.Persone = Persone;
                    Author.AuthorStatus = _AuthorStatus.AuthorStatus;
                    //_Dir.Dir.Author = Author;

                    //Order.Direction = db.Directions.Find(_Dir.Dir.DirectionId);
                    //Order.Client = new Client() { Persone = Persone };
                    //Order.WorkType = db.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    //Order.Dates = Date;
                    //Order.Subject = db.Subjects.Find(_Subj.Subj.SubjectId); ;
                    //Order.Money = Price;
                    ////if (SelectedExecuteAuthor is null)
                    ////    // set a default entry to author field
                    ////    Order.Author = db.Authors.Find(new Author() { AuthorId = 1 }.AuthorId);
                    ////else
                    ////    //set realy selected author
                    ////    Order.Author = SelectedExecuteAuthor;                    if (_Status.Status.StatusName == "принимается")
                    //    // set a default entry to status field
                    //    Order.Status = db.Statuses.Find(new Status() { StatusId = 1 }.StatusId);
                    //else
                    //    //set realy selected status
                    //    Order.Status = _Status.Status;

                    //db.Orderlines.Add(Order);

                    //db.SaveChanges();
                    //dialogService.ShowMessage("Данные о заказе сохранены");

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
        //chek for  validation of of author name , contacts and direction
        private string ValidAuthorDataCheck()
        {
            string error=null;
            error = Persone.Name == "" ? "Поле имени не должно быть пустым" : null;
            error += _Dir.Dir.DirectionName == "---" ? "\nПоле направления не должно быть пустым" : null;
            error += !Contacts.ContactsValidation() ?"\nНи одно из полей контактных данных не заполнено":null;
            return error;
        }
    }
}
