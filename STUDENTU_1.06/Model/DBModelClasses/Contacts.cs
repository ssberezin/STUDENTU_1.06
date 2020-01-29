using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model
{
    public class Contacts : Helpes.ObservableObject, ICloneable
    {
        public Contacts()
        {
            this.Phone1 = "+380";
            this.Phone2 = "---";
            this.Phone3 = "---";
            this.Email1 = "---";
            this.Email2 = "---";           
            this.Adress = "---";
            this.Skype = "---";
            this.VK = "---";
            this.FaceBook = "---";
            this.Persone = new ObservableCollection<Persone>();
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
        }
        [Column("Phone1", TypeName = "ntext")]
        [MaxLength(13)]        
        private string phone1;
        public string Phone1
        {
            get { return phone1; }
            set
            {
                if (value != phone1)
                {
                    phone1 = value;
                    OnPropertyChanged(nameof(Phone1));
                }
            }
        }

        [Column("Phone2", TypeName = "ntext")]
        [MaxLength(13)]
        private string phone2;
        public string Phone2
        {
            get { return phone2; }
            set
            {
                if (value != phone2)
                {
                    phone2 = value;
                    OnPropertyChanged(nameof(Phone2));
                }
            }
        }

        [Column("Phone3", TypeName = "ntext")]
        [MaxLength(13)]
        private string phone3;
        public string Phone3
        {
            get { return phone3; }
            set
            {
                if (value != phone3)
                {
                    phone3 = value;
                    OnPropertyChanged(nameof(Phone3));
                }
            }
        }

        [Column("Email1", TypeName = "ntext")]
        [MaxLength(100)]
        private string email1;
        public string Email1
        {
            get { return email1; }
            set
            {
                if (value != email1)
                {
                    email1 = value;
                    OnPropertyChanged(nameof(Email1));
                }
            }
        }


        [Column("Email2", TypeName = "ntext")]
        [MaxLength(100)]
        private string email2;
        public string Email2
        {
            get { return email2; }
            set
            {
                if (value != email2)
                {
                    email2 = value;
                    OnPropertyChanged(nameof(Email2));
                }
            }
        }

        [Column("Adress", TypeName = "ntext")]
        [MaxLength(255)]        
        private string adress;
        public string Adress
        {
            get { return adress; }
            set
            {
                if (value != adress)
                {
                    adress = value;
                    OnPropertyChanged(nameof(Adress));
                }
            }
        }

        [Column("Skype", TypeName = "ntext")]
        [MaxLength(100)]
        private string skype;
        public string Skype
        {
            get { return skype; }
            set
            {
                if (value != skype)
                {
                    skype = value;
                    OnPropertyChanged(nameof(Skype));
                }
            }
        }

        [Column("VK", TypeName = "ntext")]
        [MaxLength(100)]
        private string vk;
        public string VK
        {
            get { return vk; }
            set
            {
                if (value != vk)
                {
                    vk = value;
                    OnPropertyChanged(nameof(VK));
                }
            }
        }

        [Column("FaceBook", TypeName = "ntext")]
        [MaxLength(100)]        
        private string faceBook;
        public string FaceBook
        {
            get { return faceBook; }
            set
            {
                if (value != faceBook)
                {
                    faceBook = value;
                    OnPropertyChanged(nameof(FaceBook));
                }
            }
        }

        public int ContactsId { get; set; }

        //  public virtual Persone Persone { get; set; }
        public virtual ObservableCollection<Persone> Persone { get; set; }

        //check for validation of Contacts fields
        public bool ContactsValidation()
        {
            bool flag = true;

            if (Phone1 == "+380" && Phone2 == "---" && Phone3 == "---" && Email1 == "---" && Email2 == "---" && VK == "---" && FaceBook == "---")
                flag = false;

            return flag;
        }

        IDialogService dialogService;
        IShowWindowService showWindow;


        public int CheckContacts(Contacts contacts)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    int count = db.Contacts.Count();
                    if (count == 0) return 0;

                   
                    var res = db.Contacts.ToList();
                    if (res == null) return 0;
                    foreach (var item in res)
                    {
                        if (item.Phone1!="---"&&(item.Phone1 == contacts.Phone1 || item.Phone1 == contacts.Phone2 || item.Phone1 == contacts.Phone3)||
                            item.Phone2 != "---" && (item.Phone2 == contacts.Phone2 || item.Phone2 == contacts.Phone3)||
                            item.Phone3 != "---" && item.Phone3 == contacts.Phone3 ||
                            item.Email1!="---"&&(item.Email1==contacts.Email1|| item.Email1 == contacts.Email2 )||
                            item.Email2 != "---" && (item.Email2 == contacts.Email2) ||
                            item.VK !="---"&&item.VK == contacts.VK ||
                            item.FaceBook!="---"&&item.FaceBook == contacts.FaceBook||
                            item.Skype!="---"&&item.Skype==item.Skype)
                            return item.ContactsId;
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
            return 0;
        }

       public  object Clone()
        {
            return new Contacts()
            {
                ContactsId=this.ContactsId,
                Phone1 = this.Phone1,
                Phone2 = this.Phone2,
                Phone3 = this.Phone3,
                Email1 = this.Email1,
                Email2 = this.Email2,
                Adress = this.Adress,
                Skype = this.Skype,
                VK = this.VK,
                FaceBook = this.FaceBook,
                Persone = new ObservableCollection<Persone>(this.Persone)
            };
        }

        public object CloneExceptVirtual()
        {
            return new Contacts()
            {
                ContactsId = this.ContactsId,
                Phone1 = this.Phone1,
                Phone2 = this.Phone2,
                Phone3 = this.Phone3,
                Email1 = this.Email1,
                Email2 = this.Email2,
                Adress = this.Adress,
                Skype = this.Skype,
                VK = this.VK,
                FaceBook = this.FaceBook                
            };
        }
    }
}
