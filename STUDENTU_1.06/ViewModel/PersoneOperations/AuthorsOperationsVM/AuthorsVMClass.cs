using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;

using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views;
using STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
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

        public ObservableCollection<AuthorsRecord> AuthorsRecords { get; set; }
        //метка указания режима редактирования. 
        bool edit = false;

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


        IDialogService dialogService;
        IShowWindowService showWindow;
        
        //this.DataContext = new AuthorsVMClass(this, new DefaultShowWindowService(),
        //        new DefaultDialogService());

        public AuthorsVMClass()
        {
            DefaultDataLoad();
        }

        public AuthorsVMClass(Author author)
        {
            edit = !edit;
            Author = new Author();
            Author =author;
            DefaultPhoto = "default_avatar.png";
            AuthorDafaultDataLoad(author);
            dialogService = new DefaultDialogService();
            showWindow = new DefaultShowWindowService();
        }

        public AuthorsVMClass(string str)
        {
            AuthorsRecords = new ObservableCollection<AuthorsRecord>();    
            AuthorsRecord = new AuthorsRecord();
            RuleOrderLine = new RuleOrderLine("");
            DefaultDataLoad();
            AllAuthorsCall();
            PropertyChanged += ChangeProperty;
        }

        private void ChangeProperty(object sender, PropertyChangedEventArgs e)
        {           
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    if (AuthorsRecord != null)
                    {
                        db.Authors.Attach(AuthorsRecord.Author);
                        db.Dates.Attach(AuthorsRecord.Persone.Dates[0]);
                        db.PersoneDescriptions.Attach(AuthorsRecord.Persone.PersoneDescription);
                        _Dir.AuthorDirections.Clear();
                        foreach (var item in Author.Direction)
                            _Dir.AuthorDirections.Add(item);
                        _Subj.AuthorSubjects.Clear();
                        foreach (var item in AuthorsRecord.Author.Subject)
                            _Subj.AuthorSubjects.Add(item);
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
            if (AuthorsRecord != null)
            {
                Persone = AuthorsRecord.Persone;
                Author = AuthorsRecord.Author;
                _AuthorStatus.AuthorStatus = Author.AuthorStatus;
                _Contacts.Contacts = Persone.Contacts;
                _Contacts.TmpContacts = _Contacts.Contacts;
               
                Date = AuthorsRecord.Persone.Dates[0];
                PersoneDescription = AuthorsRecord.Persone.PersoneDescription;
            }
            
        }

        private void DefaultDataLoad()
        {
            DefaultPhoto = "default_avatar.png";
            Author = new Author();
            _AuthorStatus = new _AuthorStatus();
            _Contacts = new _Contacts();
            Date = new Dates();
            _Dir = new _Direction();
            Persone = new Persone();
            PersoneDescription = new PersoneDescription();
            _Subj = new _Subject();
            dialogService = new DefaultDialogService();
            showWindow = new DefaultShowWindowService();
        }

        private void AuthorDafaultDataLoad(Author author)
        {

            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    db.Authors.Attach(author);
                    _AuthorStatus = new _AuthorStatus();
                    _AuthorStatus.AuthorStatus = author.AuthorStatus;
                    _Contacts = new _Contacts();
                    _Contacts.Contacts = author.Persone.Contacts;
                    _Contacts.TmpContacts = _Contacts.Contacts;
                    _Dir = new _Direction();                 
                    foreach (var item in author.Direction)
                        _Dir.AuthorDirections.Add(item);
                    Date = new Dates();
                    Date = author.Persone.Dates[0];
                    Persone = author.Persone ;
                    PersoneDescription = author.Persone.PersoneDescription;
                    _Subj = new _Subject();
                    foreach (var item in author.Subject)
                        _Subj.AuthorSubjects.Add(item);

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
                    //var contacts = db.Contacts.Include("Persone").ToList();
                    var result = db.Authors.
                                            Include("Subject")
                                            .Include("Direction")
                                            .Include("Persone")
                                           .Include("AuthorStatus").ToList();
                    AuthorsRecord record;
                    foreach (Author item in result)
                    {
                      
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
            Persone.Photo = File.ReadAllBytes(path);
        }

        //=====================Command for call AuthorRatingEditWindow.xaml======================================
        private RelayCommand newEditRating;
        public RelayCommand NewEditRating => newEditRating ?? (newEditRating = new RelayCommand(
                    (obj) =>
                    {
                        AuthorRatingEditWindow editRating = new AuthorRatingEditWindow(obj);                      
                        showWindow.ShowDialog(editRating);
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
                        _Contacts.NewEditContacts(new AddContactsWindow(obj));                       
                    }
                    ));

        //=====================Comman for call Editing window of AuthorSTATUS ======================================

        private RelayCommand newEditAuthorStatusCommand;
        public RelayCommand NewEditAuthorStatusCommand => newEditAuthorStatusCommand ?? (newEditAuthorStatusCommand = new RelayCommand(
                    (obj) =>
                    {
                        EditAuthorStatusWindow editStatus = new EditAuthorStatusWindow(obj);
                        showWindow.ShowDialog(editStatus);
                    }
                    ));


        //====================================COMMAND FOR CALL AuthorDirectionsWindow.XAML =========================

        private RelayCommand addDirectionsCommand;
        public RelayCommand AddDirectionsCommand => addDirectionsCommand ??
            (addDirectionsCommand = new RelayCommand(
                    (obj) =>
                    {
                        AuthorDirectionsWindow authorDirectionWindow = new AuthorDirectionsWindow(obj);                       
                        showWindow.ShowDialog(authorDirectionWindow);
                    }
                    ));

    
        //====================================COMMAND FOR CALL AuthorSubjectWindow.XAML =========================

        private RelayCommand addSubjectsCommand;
        public RelayCommand AddSubjectsCommand => addSubjectsCommand ??
            (addSubjectsCommand = new RelayCommand(
                    (obj) =>
                    {
                        AuthorSubjectWindow authorSubjectWindow = new AuthorSubjectWindow(obj);                        
                        showWindow.ShowDialog(authorSubjectWindow);
                    }
                    ));



        // =============================================COMMAND FOR SAVE AUTHOR DATA =====================================

        private RelayCommand saveAuthorDataCommand;
        public RelayCommand SaveAuthorDataCommand => saveAuthorDataCommand ??
            (saveAuthorDataCommand = new RelayCommand(
                    (obj) =>
                    {
                        SaveAuthorData();
                    }
                    ));

        private void SaveAuthorData()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {                    
                    string errValidation = ValidAuthorDataCheck();
                    if (errValidation != null)
                    {
                        errValidation += "\n\n Данные автора НЕ были сохранены";
                        dialogService.ShowMessage(errValidation);
                        return;
                    }
                    else
                    {
                        //if we  need to modified entrie
                        if (Author.AuthorId != 0)
                        {
                            db.Entry(Persone).State = EntityState.Modified;
                            db.Entry(Date).State = EntityState.Modified;
                            db.Entry(Author).State = EntityState.Modified;
                            db.Entry(_Dir.Dir).State = EntityState.Modified;
                            db.Entry(_Subj.Subj).State = EntityState.Modified;
                            db.Entry(PersoneDescription).State = EntityState.Modified;                            
                        }
                        Persone.Contacts = _Contacts.Contacts;
                        if (Author.AuthorId != 0)
                            Persone.Dates[0]=Date;
                        else
                            Persone.Dates.Add(Date);
                        
                        Persone.PersoneDescription = PersoneDescription;
                        Author.Persone = Persone;                        
                        Author.AuthorStatus = db.AuthorStatuses.Find(_AuthorStatus.AuthorStatus.AuthorStatusId);
                       if (Author.AuthorId == 0) 
                            db.Authors.Add(Author);
                        db.SaveChanges();

                        //удаляем из списка направлений упоминания об авторе, если в списке направлений автора нет более того или  иного направления после правки
                        // remove the author’s mention from the list of directions if the author’s list does not have more than one direction or another after editing
                        var res = db.Directions.ToList();
                            foreach (var i in res)
                            {                     
                            if (i.Author.Contains(Author) && !_Dir.AuthorDirections.Contains(i))
                                    i.Author.Remove(Author);
                                continue;
                            }
                                                                               

                        foreach (Direction item in _Dir.AuthorDirections)
                        {
                            var res1 = db.Directions.Find(item.DirectionId);
                            if (res1 != null&& !res1.Author.Contains(Author))
                            {
                                //changing DB
                                if (Author.AuthorId != 0)
                                {
                                    
                                    res1.Author.Add(Author);
                                    continue;
                                }
                                else                                    
                                res1.Author.Add(Author);                                
                                continue;
                            }
                        }
                        db.SaveChanges();

                        //удаляем из списка предметов упоминания об авторе, если в списке предметов автора нет более того или  иного направления после правки
                        // remove the author’s mention from the list of subjects if the author’s list does not have more than one subject or another after editing
                        var res2 = db.Subjects.ToList();
                        foreach (var i in res2)
                        {
                            if (i.Authors.Contains(Author) && !_Subj.AuthorSubjects.Contains(i))
                                i.Authors.Remove(Author);
                            continue;
                        }
                        //here we add author in subjects
                        foreach (Subject item in _Subj.AuthorSubjects)
                        {
                            var res1 = db.Subjects.Find(item.SubjectId);
                            if (res1 != null)
                            {
                                //changing DB
                                res1.Authors.Add(Author);                                
                                continue;
                            }
                            else
                                res1.Authors.Add(Author);
                            continue;

                        }
                        db.SaveChanges();

                        dialogService.ShowMessage("Данные автора сохранены");
                        //обнуляем поля окна
                        //clear window fields
                        if (Author.AuthorId == 0)
                        {
                            _Contacts = new _Contacts();
                            Persone = new Persone();
                            _AuthorStatus = new _AuthorStatus();
                            Author = new Author();
                            Date = new Dates();
                            _Subj = new _Subject();
                            _Dir = new _Direction();
                            PersoneDescription = new PersoneDescription();
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

        //chek for  validation of of author name , contacts and direction
        private string ValidAuthorDataCheck()
        {
            string error;
            error = Persone.Name == "" ? "Поле имени не должно быть пустым" : null;
            error += _Dir.AuthorDirections.Count() == 0 ? "\nНЕ добавлено ни одного направления" : null;
            error += !_Contacts.Contacts.ContactsValidation() ?"\nНи одно из полей контактных данных не заполнено":null;
            error = error == "" ? null:error ;
             return error;
        }

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
           
            RuleOrderLine.AuthorsCallByParams
                (_Dir.SelectedDir.DirectionName, _Subj.SelectedSubj.SubName,
                _AuthorStatus.SelectedAuthorStatus.AuthorStatusName, AuthorsRecords);

        }


    }
}
