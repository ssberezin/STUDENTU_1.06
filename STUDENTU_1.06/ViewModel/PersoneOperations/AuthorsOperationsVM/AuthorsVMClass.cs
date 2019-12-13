using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;

using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views;
using STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows;
using System;
using System.Collections.Generic;
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

        //to be able to display authors data in ListBox x:Name="Authors" AuthorEditWindow.xaml
        private RuleOrderLine _ruleOrderLine;
        public RuleOrderLine _RuleOrderLine
        {
            get { return _ruleOrderLine; }
            set
            {
                if (_ruleOrderLine != value)
                {
                    _ruleOrderLine = value;
                    OnPropertyChanged(nameof(_RuleOrderLine));
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
             PropertyChanged += ChangeProperty;
        }

        public AuthorsVMClass(Author author)
        {
            Author = new Author();
            //Author =author;
            DefaultPhoto = "default_avatar.png";
            AuthorDafaultDataLoad(author);
           
            dialogService = new DefaultDialogService();
            showWindow = new DefaultShowWindowService();
        }

        private void ChangeProperty(object sender, PropertyChangedEventArgs e)
        {
            
        }


        private void DefaultDataLoad()
        {
            DefaultPhoto = "default_avatar.png";
            Author = new Author();
            _AuthorStatus = new _AuthorStatus();
            _Contacts = new _Contacts();
            Date = new Dates();
            _Dir = new _Direction();
            _RuleOrderLine = new RuleOrderLine("");
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
                        AuthorRatingCreate(_RuleOrderLine.AuthorsRecord.Author);
                    }
                    ));

        private void AuthorRatingCreate(Author author)
        {
            if (author == null)
                Author.Rating = Author.RatingCreate();
            else
                _RuleOrderLine.AuthorsRecord.Author.Rating = _RuleOrderLine.AuthorsRecord.Author.RatingCreate();
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
                        //authorSubjectWindow.Owner = Application.Current.MainWindow;
                  
                        //showWindow.ShowWindow(authorSubjectWindow);
                        showWindow.ShowDialog(authorSubjectWindow);
                    }
                    ));



        // =============================================COMMAND FOR SAVE AUTHOR DATA =====================================

        private RelayCommand saveAuthorDataCommand;
        public RelayCommand SaveAuthorDataCommand => saveAuthorDataCommand ??
            (saveAuthorDataCommand = new RelayCommand(
                    (obj) =>
                    {
                        if (_RuleOrderLine.AuthorsRecord.Author==null)
                            SaveAuthorData();
                        else
                            SaveAuthorData(_RuleOrderLine.AuthorsRecord.Author);
                    }
                    ));

        //private void SaveAuthorData()
        private void SaveAuthorData()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {                    
                    string errValidation = ValidAuthorDataCheck(Author);
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
                        //here we add author in directions
                        foreach (Direction item in _Dir.AuthorDirections)
                        {
                            var res1 = db.Directions.Find(item.DirectionId);
                            if (res1 != null&& !res1.Author.Contains(Author))
                            {
                                //changing DB
                                if (Author.AuthorId != 0)
                                {
                                  // db.Entry(res1).State = EntityState.Modified;
                                    res1.Author.Add(Author);
                                    continue;
                                }
                                else                                    
                                res1.Author.Add(Author);                                
                                continue;
                            }
                        }
                        db.SaveChanges();
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

        private void SaveAuthorData(Author author)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    string errValidation = ValidAuthorDataCheck(author);
                    if (errValidation != null)
                    {
                        errValidation += "\n\n Данные автора НЕ были сохранены";
                        dialogService.ShowMessage(errValidation);
                        return;
                    }
                    else
                    {
                        //Author = _RuleOrderLine.AuthorsRecord.Author;                        
                        //if we  need to modified entrie
                        if (Author.AuthorId != 0)
                        {
                            db.Entry(_RuleOrderLine.AuthorsRecord.Persone).State = EntityState.Modified;
                            db.Entry(Date).State = EntityState.Modified;
                            db.Entry(_RuleOrderLine.AuthorsRecord.Author).State = EntityState.Modified;
                            db.Entry(_RuleOrderLine._Dir.Dir).State = EntityState.Modified;
                            db.Entry(_RuleOrderLine._Subject.Subj).State = EntityState.Modified;
                            db.Entry(_RuleOrderLine.AuthorsRecord.Persone.Contacts).State = EntityState.Modified;
                            db.Entry(_RuleOrderLine.AuthorsRecord.Persone.PersoneDescription).State = EntityState.Modified;
                        }
                        _RuleOrderLine.AuthorsRecord.Persone.Contacts = _Contacts.Contacts;
                        if (Author.AuthorId != 0)
                            _RuleOrderLine.AuthorsRecord.Persone.Dates[0] = Date;
                        else
                            _RuleOrderLine.AuthorsRecord.Persone.Dates.Add(Date);

                        //Persone.PersoneDescription = _RuleOrderLine.AuthorsRecord.Persone.PersoneDescription;
                        //Author.Persone = Persone;
                        _RuleOrderLine.AuthorsRecord.Author.AuthorStatus = db.AuthorStatuses.Find(_AuthorStatus.AuthorStatus.AuthorStatusId);
                        if (_RuleOrderLine.AuthorsRecord.Author.AuthorId == 0)
                            db.Authors.Add(_RuleOrderLine.AuthorsRecord.Author);
                        db.SaveChanges();
                        //here we add author in directions
                        foreach (Direction item in _RuleOrderLine._Dir.AuthorDirections)
                        {
                            var res1 = db.Directions.Find(item.DirectionId);
                            if (res1 != null && !res1.Author.Contains(Author))
                            {
                                //changing DB
                                if (_RuleOrderLine.AuthorsRecord.Author.AuthorId != 0)
                                {
                                    // db.Entry(res1).State = EntityState.Modified;
                                    res1.Author.Add(Author);
                                    continue;
                                }
                                else
                                    res1.Author.Add(Author);
                                continue;
                            }
                        }
                        db.SaveChanges();
                        //here we add author in subjects
                        foreach (Subject item in _RuleOrderLine._Subject.AuthorSubjects)
                        {
                            var res1 = db.Subjects.Find(item.SubjectId);
                            if (res1 != null)
                            {
                                //changing DB
                                res1.Authors.Add(_RuleOrderLine.AuthorsRecord.Author);
                                continue;
                            }
                            else
                                res1.Authors.Add(_RuleOrderLine.AuthorsRecord.Author);
                            continue;

                        }
                        db.SaveChanges();

                        dialogService.ShowMessage("Данные автора сохранены");
                        //обнуляем поля окна
                        //clear window fields
                        //if (_RuleOrderLine.AuthorsRecord.Author.AuthorId == 0)
                        //{

                        //    _Contacts = new _Contacts();
                        //    Persone = new Persone();
                        //    _AuthorStatus = new _AuthorStatus();
                        //    Author = new Author();
                        //    Date = new Dates();
                        //    _Subj = new _Subject();
                        //    _Dir = new _Direction();
                        //    PersoneDescription = new PersoneDescription();
                        //}
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
        private string ValidAuthorDataCheck(Author author)
        {
            string error;
            if (author == null)
            {
                error = Persone.Name == "" ? "Поле имени не должно быть пустым" : null;
                error += _Dir.AuthorDirections.Count() == 0 ? "\nНЕ добавлено ни одного направления" : null;
                error += !_Contacts.Contacts.ContactsValidation() ? "\nНи одно из полей контактных данных не заполнено" : null;
                error = error == "" ? null : error;
                return error;
            }
            else
            {
                error = _RuleOrderLine.AuthorsRecord.Persone.Name == "" ? "Поле имени не должно быть пустым" : null;
                error += _RuleOrderLine._Dir.AuthorDirections.Count() == 0 ? "\nНЕ добавлено ни одного направления" : null;
                error += !_Contacts.Contacts.ContactsValidation() ? "\nНи одно из полей контактных данных не заполнено" : null;
                error = error == "" ? null : error;
                return error;
            }
            
        }

        //===================================COMMAND FOR ADD DIRECTIONS INTO AuthorDirections ============      

        private RelayCommand addAuthorDirectionCommand;
        public RelayCommand AddAuthorDirectionCommand => addAuthorDirectionCommand ??
            (addAuthorDirectionCommand = new RelayCommand((selectedItem) =>
            {
                //AddAuthorDirection(ObservableCollection < Direction > authorDirections, Direction _dir)
                _RuleOrderLine._Dir.AddAuthorDirection(_RuleOrderLine._Dir.AuthorDirections, _Dir.Dir);
            }
           ));

        //===================================COMMAND FOR ADD DIRECTIONS INTO AuthorSubjects ============      

        private RelayCommand addAuthorSubjectCommand;
        public RelayCommand AddAuthorSubjectCommand => addAuthorSubjectCommand ??
            (addAuthorSubjectCommand = new RelayCommand((selectedItem) =>
            {
                _RuleOrderLine._Subject.AddAuthorSubject(_RuleOrderLine._Subject.AuthorSubjects, _Subj.Subj);
            }
           ));




    }
}
