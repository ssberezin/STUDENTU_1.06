using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;

using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
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

        //for display author photo
        private string photoFileName;
        public string AuthorPhoto
        {
            set
            {
                if (photoFileName != value)
                {
                    photoFileName = value;
                    OnPropertyChanged(nameof(AuthorPhoto));
                }               
            }
            get{  return "/STUDENTU_1.06;component/Images/Authors/"+ photoFileName;}            
        }

        //for display author photo
        private string photoClientFileName;
        public string ClientPhoto
        {
            set
            {
                if (photoClientFileName != value)
                {
                    photoClientFileName = value;
                    OnPropertyChanged(nameof(ClientPhoto));
                }
            }
            get { return "/STUDENTU_1.06;component/Images/Clients/" + photoClientFileName; }
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
            AuthorPhoto = "a_ek.png";

            Author = new Author();
            _AuthorStatus = new _AuthorStatus();            
            Date = new Dates();
            _Dir = new _Direction();
            Persone = new Persone();
            PersoneDescription = new PersoneDescription();

        }


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
            //FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);

           // file.Read(Photo, 0, System.Convert.ToInt32(file.Length));
            //file.Close();


            Photo = File.ReadAllBytes(path);
            //File.WriteAllBytes(@"sevedFile.someExtention", bData);
            int i = 0;
            //byte[] bData = File.ReadAllBytes(@"someFile.someExtention");
            //File.WriteAllBytes(@"sevedFile.someExtention", bData);

        }


    }
}
