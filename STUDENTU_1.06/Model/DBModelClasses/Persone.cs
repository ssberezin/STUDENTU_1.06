using STUDENTU_1._06.Model.HelpModelClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model 
{
    public class Persone : Helpes.ObservableObject
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
            this.PhotoFileName = "/Images/";
        }

        public int PersoneId { get; set; }

        [Column("Name", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Column("NickName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string NickName { get; set; }
        [Column("Surname", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Surname { get; set; }
        [Column("Patronimic", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Patronimic { get; set; }

        //убрать нахер это поле
        [Column("PhotoFileName", TypeName = "nvarchar")]
        [MaxLength(500)]
        public string PhotoFileName { get; set; }

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

        public bool Sex { get; set; }

       
        public virtual Contacts Contacts { get; set; }
        public virtual PersoneDescription PersoneDescription { get; set; }
     
        public virtual ObservableCollection<User> User { get; set; }        
        public virtual ObservableCollection<Client> Client { get; set; }        
        public virtual ObservableCollection<Author> Author { get; set; }        
        public virtual ObservableCollection<Dates> Dates { get; set; }

        public string ToString()
        {
            return $"{Name} " + $" {Surname} " + $"{Patronimic}" ;
        }


        //public string this[string columnName]
        //{
        //    get
        //    {
        //        string error = String.Empty;
        //        switch (columnName)
        //        {

        //            case "Name":
        //                //Обработка ошибок для свойства Name
        //                if (Name.Length ==0)
        //                {
        //                    error = "Поле имени не должно быть пустым";
        //                }
        //                break;
        //            case "Sex":
        //                //Обработка ошибок для свойства Name
        //                if (Sex == null)
        //                {
        //                    error = "Это поле не должно быть пустым";
        //                }
        //                break;


        //        }
        //        return error;
        //    }
        //}

        //public string Error
        //{
        //    get { throw new NotImplementedException(); }
        //}

    }
}
