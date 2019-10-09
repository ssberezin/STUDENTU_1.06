using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Data.Entity ;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using STUDENTU_1._06.Model.HelpModelClasses;
using System.Collections.ObjectModel;

namespace STUDENTU_1._06.ViewModel
{
    public partial class ForEditOrder : Helpes.ObservableObject
    {

        //here we have blacklist functionality

        //DataTable BlackListIds;//tmp table for transfer info to "newBlackListInfoWindow"

        public ObservableCollection<BlackListHelpModel> BlackListRecords { get; set; }

        private BlackListHelpModel blackListRecord;
        public BlackListHelpModel BlackListRecord
        {
            get { return blackListRecord; }
            set
            {
                if (blackListRecord != value)
                {
                    blackListRecord = value;
                    OnPropertyChanged(nameof(BlackListRecord));
                }
            }
        }

        private RelayCommand newBlackListInfo;
        public RelayCommand NewBlackListInfo => newBlackListInfo ?? (newBlackListInfo = new RelayCommand(
                    (obj) =>
                    {
                        BlackListWindow newBlackListInfoWindow = new BlackListWindow(obj);
                        newBlackListInfoWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(newBlackListInfoWindow);
                    }
                    ));

        //==================Commands for CHECK CONTACTS IN BLACK LIST============================

        private RelayCommand checkBlackListCommand;
        public RelayCommand CheckBlackListCommand => checkBlackListCommand ?? (checkBlackListCommand = new RelayCommand(
                    (obj) =>
                    {
                       // string phoneNumber = obj as string;
                        BlackListCheck();
                    }
                    ));

        private void BlackListCheck()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
  try
                {
                    //нашли всех, кто есть в черном списке
                    // found everyone on the blacklist
                    var listP = db.Orderlines.
                                             Include("Client").                                            
                                             ToList();
                    //тут не верно заданы критерии сравнения, нужно доработать..ппц
                    var resListPersons = (from a in listP
                                          where 
                                          (a.Client.Persone.PersoneDescription.BlackList == true&&
                                            ((a.Client.Persone.Contacts.Phone1 == Contacts.Phone1 && Contacts.Phone1 != "---")||
                                             (a.Client.Persone.Contacts.Phone1 == Contacts.Phone2 && Contacts.Phone2 != "---") ||
                                             (a.Client.Persone.Contacts.Phone1 == Contacts.Phone3 && Contacts.Phone3 != "---") ||
                                             (a.Client.Persone.Contacts.Phone2 == Contacts.Phone2 && Contacts.Phone2 != "---") ||
                                             (a.Client.Persone.Contacts.Phone2== Contacts.Phone3 && Contacts.Phone3 != "---") ||
                                             (a.Client.Persone.Contacts.Phone3 == Contacts.Phone3 && Contacts.Phone3 != "---") ||
                                             (a.Client.Persone.Contacts.Email1 == Contacts.Email1 && Contacts.Email1 != "---") ||
                                             (a.Client.Persone.Contacts.Email1 == Contacts.Email2 && Contacts.Email2 != "---") ||
                                             (a.Client.Persone.Contacts.Email2 == Contacts.Email2 && Contacts.Email2 != "---") ||
                                             (a.Client.Persone.Contacts.VK == Contacts.VK && Contacts.VK != "---") ||
                                             (a.Client.Persone.Contacts.Skype == Contacts.Skype && Contacts.Skype != "---")
                                            )
                                          )
                                          select new BlackListHelpModel
                                          {
                                              OrderLineId = a.OrderLineId,
                                              PersoneId = a.Client.Persone.PersoneId,
                                              PersoneDescriptionId = a.Client.Persone.PersoneDescription.PersoneDescriptionId,
                                              ContactsId=a.Client.Persone.Contacts.ContactsId
                                          }
                                );
                    if (resListPersons != null)
                    {
                        if (dialogService.YesNoDialog("Номер клиента в черном списке!\nПросмотреть причины?") == true)
                        {
                            foreach (var item in resListPersons)
                            {
                                BlackListRecord.ContactsId = item.ContactsId;
                                BlackListRecord.PersoneDescriptionId = item.PersoneDescriptionId;
                                BlackListRecord.PersoneId = item.PersoneId;
                                BlackListRecord.OrderLineId = item.OrderLineId;

                                BlackListRecords.Add(BlackListRecord);
                            }
                            //show info about blacklist records
                            NewBlackListInfo.Execute(null);
                        }
                    }
                    
                    ////проверили целесообразность дальнейших действий
                    //// checked the feasibility of further actions
                    //if (resListPersons == null || resListPersons.Count() == 0)
                    //{
                    //    dialogService.ShowMessage("Этих контактных данных \n      Н Е Т\nв черном списке");
                    //    return;
                    //}

                        ////нашли всех, у кого совпдают контактные данные
                        //// found everyone with matching contact details
                        //var listC = db.Contacts.Include("Persone").ToList();
                        //var resListСontacts = (from a in db.Contacts
                        //                       where (
                        //                       ((a.Phone1 == Contacts.Phone1 && Contacts.Phone1 != "---") ||
                        //                          (a.Phone1 == Contacts.Phone2 && Contacts.Phone2 != "---") ||
                        //                          (a.Phone1 == Contacts.Phone3 && Contacts.Phone3 != "---") ||
                        //                          (a.Phone2 == Contacts.Phone1 && Contacts.Phone1 != "---") ||
                        //                          (a.Phone2 == Contacts.Phone2 && Contacts.Phone2 != "---") ||
                        //                          (a.Phone2 == Contacts.Phone3 && Contacts.Phone3 != "---") ||
                        //                          (a.Phone3 == Contacts.Phone1 && Contacts.Phone1 != "---") ||
                        //                          (a.Phone3 == Contacts.Phone2 && Contacts.Phone2 != "---") ||
                        //                          (a.Phone3 == Contacts.Phone3 && Contacts.Phone3 != "---") ||
                        //                          (a.Email1 == Contacts.Email1 && Contacts.Email1 != "---") ||
                        //                          (a.Email1 == Contacts.Email2 && Contacts.Email2 != "---") ||
                        //                          (a.Email2 == Contacts.Email1 && Contacts.Email1 != "---") ||
                        //                          (a.Email2 == Contacts.Email2 && Contacts.Email2 != "---") ||
                        //                          (a.VK == Contacts.VK && Contacts.VK != "---") ||
                        //                          (a.Skype == Contacts.Skype && Contacts.Skype != "---"))
                        //                       )
                        //                       select new
                        //                       {
                        //                           PersoneId = a.Persone.PersoneId,
                        //                           ContactsId = a.ContactsId
                        //                       });
                        ////проверили целесообразность дальнейших действий
                        //// checked the feasibility of further actions
                        //if (listC == null || listC.Count() == 0)
                        //{
                        //    dialogService.ShowMessage("Этих контактных данных \n      Н Е Т\nв черном списке");
                        //    return;
                        //}
                        ////сформировали таблицу, куда будем писать данные в случае совпадения resListPersons и resListСontacts
                        ////по критерию PersoneId
                        //// formed a table where we will write the data if resListPersons and resListСontacts match
                        //// by PersoneId criterion
                        //BlackListIds = new DataTable("BlackListPersones");

                        //BlackListIds.Columns.Add("OrderLineId", typeof(Int32));
                        //BlackListIds.Columns.Add("PersoneId", typeof(Int32));
                        //BlackListIds.Columns.Add("PersoneDescriptionId", typeof(Int32));
                        //BlackListIds.Columns.Add("ContactsId", typeof(Int32));
                        ////ищим совпадения
                        //// look for matches
                        //foreach (var item1 in resListPersons)
                        //{
                        //    foreach (var item2 in resListСontacts)
                        //    {
                        //        if (item1.PersoneId == item2.PersoneId)
                        //            //наполняем таблицу данными
                        //            // fill the table with data
                        //            BlackListIds.Rows.Add(new Object[] {
                        //                                                 item1.OrderLineId,
                        //                                                 item1.PersoneId,
                        //                                                 item1.PersoneDescriptionId,
                        //                                                 item2.ContactsId});
                        //    }
                        //}

                        //if (dialogService.YesNoDialog("Номер клиента в черном списке!\nПросмотреть причины?") == true)
                        //{
                        //    NewBlackListInfo.Execute(null);
                        //}

                        //==========================================НЕ состоявщаяся хрень ================================
                        //var result = from oreder in db.Orderlines
                        //             join client in db.Clients on order.ClientId equals client.Id
                        //             join persone in db.Persones on client.PersoneId equals persone.Id
                        //             join persDecr in db.PersoneDescriptions on persone.PersoneDescriptionId equals persDecr.Id
                        //             join contacts in db.Contacts on persone.ContactsId equals contacts.Id
                        //             where(
                        //             ((contacts.Phone1 == Contacts.Phone1 && Contacts.Phone1 != "---") ||
                        //                   (contacts.Phone1 == Contacts.Phone2 && Contacts.Phone2 != "---") ||
                        //                   (contacts.Phone1 == Contacts.Phone3 && Contacts.Phone3 != "---") ||
                        //                   (contacts.Phone2 == Contacts.Phone1 && Contacts.Phone1 != "---") ||
                        //                   (contacts.Phone2 == Contacts.Phone2 && Contacts.Phone2 != "---") ||
                        //                   (contacts.Phone2 == Contacts.Phone3 && Contacts.Phone3 != "---") ||
                        //                   (contacts.Phone3 == Contacts.Phone1 && Contacts.Phone1 != "---") ||
                        //                   (contacts.Phone3 == Contacts.Phone2 && Contacts.Phone2 != "---") ||
                        //                   (contacts.Phone3 == Contacts.Phone3 && Contacts.Phone3 != "---") ||
                        //                   (contacts.Email1 == Contacts.Email1 && Contacts.Email1 != "---") ||
                        //                   (contacts.Email1 == Contacts.Email2 && Contacts.Email2 != "---") ||
                        //                   (contacts.Email2 == Contacts.Email1 && Contacts.Email1 != "---") ||
                        //                   (contacts.Email2 == Contacts.Email2 && Contacts.Email2 != "---") ||
                        //                   (contacts.VK == Contacts.VK && Contacts.VK != "---") ||
                        //                   (contacts.Skype == Contacts.Skype && Contacts.Skype != "---")) &&
                        //              persDecr.BlackList == true
                        //             )
                        //             select new
                        //             {
                        //                 OrdeLineId = order.Id,
                        //                 ClientId=client.Id,
                        //                 PersoneId=persone.Id,
                        //                 ContactsId=contacts.Id,
                        //                 PersoneDescriptionId=persDecr.Id
                        //             };


                        //if (result == null)
                        //{
                        //    dialogService.ShowMessage("Этих контактных данных \n      Н Е Т\nв черном списке");
                        //    return;
                        //}
                        //else
                        //{
                        //    BlackListIds = new DataTable("BlackListPersones");

                        //    BlackListIds.Columns.Add("OrderLineId", typeof(Int32));
                        //    BlackListIds.Columns.Add("ClientId", typeof(Int32));
                        //    BlackListIds.Columns.Add("PersoneId", typeof(Int32));
                        //    BlackListIds.Columns.Add("PersoneDescriptionId", typeof(Int32));
                        //    BlackListIds.Columns.Add("ContactsId", typeof(Int32));                       


                        //    foreach (var item in result)
                        //    {
                        //        BlackListIds.Rows.Add(new Object[]
                        //        {
                        //            item.OrdeLineId,
                        //            item.ClientId,
                        //            item.PersoneId,
                        //            item.PersoneDescriptionId,
                        //            item.ContactsId
                        //        });
                        //    }

                        //    if (dialogService.YesNoDialog("Номер клиента в черном списке!\nПросмотреть причины?") == true)
                        //    {
                        //        NewBlackListInfo.Execute(null);
                        //    }

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


        private void OrderLineCall()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {


                    var list = db.Orderlines.Include("AfterDoneDescriptions")
                                          .Include("Directions")
                                          .Include("Statuses")
                                          .Include("Dates")
                                          .ToList();
                    var res = (from a in list
                               select new
                               {
                                   OrderNumber = a.OrderNumber,
                                   Direction = a.Direction.DirectionName,
                                   Status = a.Status.StatusName,
                                   Deadline = a.Dates.DeadLine
                               });
                    if (res != null)
                    {
                        if (dialogService.YesNoDialog("Номер клиента в черном списке!\nПросмотреть причины?") == true)
                        {
                            //тут над допиливать
                        }
                        else
                            dialogService.ShowMessage("Этот номер телефона \n\n Н Е\n\n  в черном списке");
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
    }
}
