using STUDENTU_1._06.Model.HelpModelClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model 
{
    public class Persone : Helpes.ObservableObject, ICloneable
    {
        public Persone()
        {
            this.User = new ObservableCollection<User>();
            this.Client = new ObservableCollection<Client>();
            this.Author = new ObservableCollection<Author>();
            // this.Contacts = new List<Contacts>();

            this.Dates = new ObservableCollection<Dates>();
            this.Sex = true;
            this.Name = "";
            this.NickName = null;
            
        }

        public int PersoneId { get; set; }

        [Column("Name", TypeName = "nvarchar")]
        [MaxLength(50)]        
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [Column("NickName", TypeName = "nvarchar")]
        [MaxLength(50)]        
        private string nickName;
        public string NickName
        {
            get { return nickName; }
            set
            {
                if (value != nickName)
                {
                    nickName = value;
                    OnPropertyChanged(nameof(NickName));
                }
            }
        }

        [Column("Surname", TypeName = "nvarchar")]
        [MaxLength(50)]
        private string surname;
        public string Surname
        {
            get { return surname; }
            set
            {
                if (value != surname)
                {
                    surname = value;
                    OnPropertyChanged(nameof(Surname));
                }
            }
        }

        [Column("Patronimic", TypeName = "nvarchar")]
        [MaxLength(50)]
        //public string Patronimic { get; set; }
        private string patronimic;
        public string Patronimic
        {
            get { return patronimic; }
            set
            {
                if (value != patronimic)
                {
                    patronimic = value;
                    OnPropertyChanged(nameof(Patronimic));
                }
            }
        }


        [Column("Photo",TypeName = "image")]
        //for storing image of persone
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

        
        private bool sex;
        public bool Sex 
        {
            get { return sex; }
            set
            {
                if (value != sex)
                {
                    sex = value;
                    OnPropertyChanged(nameof(Sex));
                }
            }
        }


        public virtual Contacts Contacts { get; set; }
        public virtual PersoneDescription PersoneDescription { get; set; }
     
        public virtual ObservableCollection<User> User { get; set; }        
        public virtual ObservableCollection<Client> Client { get; set; }        
        public virtual ObservableCollection<Author> Author { get; set; }        
        public virtual ObservableCollection<Dates> Dates { get; set; }

        public object Clone()
        {
            return new Persone()
            {
                PersoneId = this.PersoneId,
                Name = this.Name,
                NickName = this.NickName,
                Surname = this.Surname,
                Patronimic = this.Patronimic,
                Photo = PhotoCopy(this.Photo),
                Sex = this.Sex,
                Contacts = (Contacts)this.Contacts.Clone(),
                PersoneDescription = (PersoneDescription)this.PersoneDescription.Clone(),
                User = new ObservableCollection<User>(this.User),
                Client = new ObservableCollection<Client>(this.Client),
                Author = new ObservableCollection<Author>(this.Author),
                Dates = new ObservableCollection<Dates>(this.Dates)
            };
        }

        public byte[] PhotoCopy(byte[] obj)
        {
            if (obj == null)
                return null;
            int len = obj.Length;
            byte[] ar = new byte[len];
            for (int i = 0; i < len; i++)
                ar[i] = obj[i];
            return ar;
        }

        public string ToString()
        {
            return $"{Name} " + $" {Surname} " + $"{Patronimic}" ;
        }
        
        //тут у нас проверка только по полям Name, Surname, Patronimic, Sex
        public bool ComparePersons(Persone obj1, Persone obj2)
        {
            if (obj1.Name==obj2.Name&&obj1.Surname==obj2.Surname&&
                obj1.Patronimic==obj2.Patronimic&&obj1.Sex==obj2.Sex)
                return true;
            return false;
        }

    }
}
