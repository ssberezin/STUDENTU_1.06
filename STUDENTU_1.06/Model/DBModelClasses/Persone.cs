using STUDENTU_1._06.Model.HelpModelClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STUDENTU_1._06.Model
{
    public class Persone //: IDataErrorInfo
    {
        public Persone()
        {
            this.User = new List<User>();
            this.Client = new List<Client>();
            this.Author = new List<Author>();
            // this.Contacts = new List<Contacts>();

            this.Dates = new List<Dates>();
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
        [Column("PhotoFileName", TypeName = "nvarchar")]
        [MaxLength(500)]
        public string PhotoFileName { get; set; }

        [Column("Photo",TypeName = "image")]
        //for storing image of persone
        public byte[] Photo { get; set; }

        public bool Sex { get; set; }

       
        public virtual Contacts Contacts { get; set; }
        public virtual PersoneDescription PersoneDescription { get; set; }
      //  public virtual List<Contacts> Contacts { get; set; }        
        public virtual List<User> User { get; set; }        
        public virtual List<Client> Client { get; set; }        
        public virtual List<Author> Author { get; set; }        
        public virtual List<Dates> Dates { get; set; }
     



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
