#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void RefreshWarehouseMovements()
        {
            try
            {
                WarehouseMovementsInDb = HispaniaDataAccess.Instance.ReadWarehouseMovements();
                _WarehouseMovements = new ObservableCollection<WarehouseMovementsView>();
                _WarehouseMovementsInDictionary = new Dictionary<string, WarehouseMovementsView>();
                foreach (HispaniaCompData.WarehouseMovement WarehouseMovement in WarehouseMovementsInDb)
                {
                    WarehouseMovementsView NewWarehouseMovementsView = new WarehouseMovementsView(WarehouseMovement);
                    _WarehouseMovements.Add(NewWarehouseMovementsView);
                    _WarehouseMovementsInDictionary.Add(GetKeyWarehouseMovementView(WarehouseMovement), NewWarehouseMovementsView);
                }
            }
            catch (Exception ex)
            {
                _WarehouseMovements = null;
                throw ex;
            }
        }

        public WarehouseMovementsView GetWarehouseMovementFromDb(WarehouseMovementsView warehouseMovementsView)
        {
            return new WarehouseMovementsView(GetWarehouseMovementInDb(warehouseMovementsView.WarehouseMovement_Id));
        }

        public ObservableCollection<WarehouseMovementsView> GetWarehouseMovements(int WarehouseMovement_Id)
        {
            try
            {
                ObservableCollection<WarehouseMovementsView> WarehouseMovementsFiltered = new ObservableCollection<WarehouseMovementsView>();
                foreach (HispaniaCompData.WarehouseMovement warehouseMovementMovement in ReadWarehouseMovementsInDb(WarehouseMovement_Id))
                {
                    WarehouseMovementsView NewWarehouseMovementsView = new WarehouseMovementsView(warehouseMovementMovement);
                    WarehouseMovementsFiltered.Add(NewWarehouseMovementsView);
                }
                return WarehouseMovementsFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<WarehouseMovementsView> _WarehouseMovements = null;

        public ObservableCollection<WarehouseMovementsView> WarehouseMovements
        {
            get
            {
                return _WarehouseMovements;
            }
        }

        private Dictionary<string, WarehouseMovementsView> _WarehouseMovementsInDictionary = null;

        public Dictionary<string, WarehouseMovementsView> WarehouseMovementsDict
        {
            get
            {
                return _WarehouseMovementsInDictionary;
            }
        }

        public string GetKeyWarehouseMovementView(WarehouseMovementsView WarehouseMovementsView)
        {
            return GetKeyWarehouseMovementView(WarehouseMovementsView.WarehouseMovement_Id);
        }

        private string GetKeyWarehouseMovementView(HispaniaCompData.WarehouseMovement WarehouseMovement)
        {
            return GetKeyWarehouseMovementView(WarehouseMovement.WarehouseMovement_Id);
        }

        private string GetKeyWarehouseMovementView(int CoustomerOrder_Id)
        {
            return string.Format("{0}", CoustomerOrder_Id);
        }

        public void CreateWarehouseMovement(WarehouseMovementsView NewWarehouseMovementView)
        {
            HispaniaCompData.WarehouseMovement WarehouseMovementToCreate = NewWarehouseMovementView.GetWarehouseMovement();
            if (NewWarehouseMovementView.According)
            {
                //  Create the Good instance to modify
                    HispaniaCompData.Good NewGood = NewWarehouseMovementView.Good.GetGood();
                //  Update information of the new Good
                    AddGoodInformation(NewWarehouseMovementView, NewGood);
                //  Create Warehouse Movement in Database
                    CreateWarehouseMovementInDb(WarehouseMovementToCreate, NewGood);
                //  Update good information for the Warehouse Movement
                    NewWarehouseMovementView.Good = new GoodsView(NewGood);
            }
            else CreateWarehouseMovementInDb(WarehouseMovementToCreate);
            NewWarehouseMovementView.WarehouseMovement_Id = WarehouseMovementToCreate.WarehouseMovement_Id;
            RefreshWarehouseMovements();
            RefreshGoods();
        }

        public void UpdateWarehouseMovement(WarehouseMovementsView NowadaysWarehouseMovementView,
                                            WarehouseMovementsView NewWarehouseMovementView,
                                            bool UpdateCosts)
        {
            HispaniaCompData.WarehouseMovement WarehouseMovementToUpdate = NewWarehouseMovementView.GetWarehouseMovement();
            bool NowadaysAccording = NowadaysWarehouseMovementView.According;
            bool NewAccording = NewWarehouseMovementView.According;
            if (NowadaysAccording && NewAccording)
            {
                //  Execute update method in the Database
                    if (NowadaysWarehouseMovementView.Good.Good_Id == NewWarehouseMovementView.Good.Good_Id)
                    {
                        //  Stores NewGood Information.
                            HispaniaCompData.Good NewGood = NewWarehouseMovementView.Good.GetGood();
                        //  Determine if the movement type is equals in all movements.
                            if (NowadaysWarehouseMovementView.Type == NewWarehouseMovementView.Type) 
                            {
                                //  Update information of the new Good
                                    UpdateGoodInformation(NowadaysWarehouseMovementView, NewWarehouseMovementView, NewGood, UpdateCosts);
                            }
                            else
                            {
                                //  Remove source movement information in NewGood.
                                    RemoveGoodInformation(NowadaysWarehouseMovementView, NewGood, false);
                                //  Update information of the new Good
                                    AddGoodInformation(NewWarehouseMovementView, NewGood);
                            }
                        //  Update information in Database
                            UpdateWarehouseMovementInDb(WarehouseMovementToUpdate, null, NewGood);
                        //  Update good information for the Warehouse Movement
                            NewWarehouseMovementView.Good = new GoodsView(NewGood);
                    }
                    else
                    {
                        //  Treure el vell (Good vell)
                            HispaniaCompData.Good OldGood = NowadaysWarehouseMovementView.Good.GetGood();
                            RemoveGoodInformation(NowadaysWarehouseMovementView, OldGood, UpdateCosts);
                        //  Update information of the new Good
                            HispaniaCompData.Good NewGood = NewWarehouseMovementView.Good.GetGood();
                            AddGoodInformation(NewWarehouseMovementView, NewGood);
                        //  Executar l'update
                            UpdateWarehouseMovementInDb(WarehouseMovementToUpdate, OldGood, NewGood);
                        //  Update good information for the Warehouse Movement
                            NewWarehouseMovementView.Good = new GoodsView(NewGood);
                    }
            }
            //else if (!NowadaysAccording && NewAccording)
            //{
            //    //  Update information of the new Good
            //        HispaniaCompData.Good NewGood = NewWarehouseMovementView.Good.GetGood();
            //        AddGoodInformation(NewWarehouseMovementView, NewGood);
            //    //  Execute update method in the Database
            //        UpdateWarehouseMovementInDb(WarehouseMovementToUpdate, null, NewGood);
            //    //  Update good information for the Warehouse Movement
            //        NewWarehouseMovementView.Good = new GoodsView(NewGood);
            //}
            //else if (NowadaysAccording && !NewAccording)
            //{
            //    //  Treure el vell (Good vell)
            //        HispaniaCompData.Good OldGood = NowadaysWarehouseMovementView.Good.GetGood();
            //        RemoveGoodInformation(NowadaysWarehouseMovementView, OldGood, UpdateCosts);
            //    //  Execute update method in the Database
            //        UpdateWarehouseMovementInDb(WarehouseMovementToUpdate, OldGood, null);
            //    //  Update good information for the Warehouse Movement
            //        NewWarehouseMovementView.Good = new GoodsView(OldGood);
            //}
            //else UpdateWarehouseMovementInDb(WarehouseMovementToUpdate); // This option doesn't produce good changes.
            RefreshWarehouseMovements();
            RefreshGoods();
        }

        public void DeleteWarehouseMovement(WarehouseMovementsView warehouseMovementView, bool UpdateCosts)
        {
            HispaniaCompData.WarehouseMovement WarehouseMovementToDelete = warehouseMovementView.GetWarehouseMovement();
            if (warehouseMovementView.According)
            {
                HispaniaCompData.Good Good = warehouseMovementView.Good.GetGood();
                RemoveGoodInformation(warehouseMovementView, Good, UpdateCosts);
                DeleteWarehouseMovementInDb(WarehouseMovementToDelete, Good);
            }
            else DeleteWarehouseMovementInDb(WarehouseMovementToDelete);
            RefreshWarehouseMovements();
            RefreshGoods();
        }

        public HispaniaCompData.WarehouseMovement GetWarehouseMovement(int WarehouseMovement_Id)
        {
            return GetWarehouseMovementInDb(WarehouseMovement_Id);
        }
        
        private void AddGoodInformation(WarehouseMovementsView warehouseMovementView, HispaniaCompData.Good good)
        {
            TypeOfUpdateAVGPC TypeOfAVGPC;
            switch (warehouseMovementView.Type)
            {
                case 1:
                     TypeOfAVGPC = TypeOfUpdateAVGPC.AddUnitsEntry;
                     break;
                case 5:
                     TypeOfAVGPC = TypeOfUpdateAVGPC.AddUnitsDeparture;
                     break;
                default:
                     throw new ArgumentException(
                                   string.Format("Error, tipus de moviment de magatzem no reconegut '{0}'.",
                                                  warehouseMovementView.Type));
            }
            GlobalViewModel.Instance.HispaniaViewModel.UpdateGoodUnitInfo(good,
                                                                          warehouseMovementView.Price,
                                                                          warehouseMovementView.Amount_Unit_Billing,
                                                                          warehouseMovementView.Amount_Unit_Shipping,
                                                                          TypeOfAVGPC,
                                                                          true);
        }

        private void RemoveGoodInformation(WarehouseMovementsView warehouseMovementView, HispaniaCompData.Good good, bool UpdateCosts)
        {
            TypeOfUpdateAVGPC TypeOfAVGPC;
            switch (warehouseMovementView.Type)
            {
                case 1:
                        TypeOfAVGPC = TypeOfUpdateAVGPC.RemoveUnitsEntry;
                        break;
                case 5:
                        TypeOfAVGPC = TypeOfUpdateAVGPC.RemoveUnitsDeparture;
                        break;
                default:
                        throw new ArgumentException(string.Format("Error, tipus de moviment de magatzem no reconegut '{0}'.", warehouseMovementView.Type));
            }
            GlobalViewModel.Instance.HispaniaViewModel.UpdateGoodUnitInfo(good,
                                                                          warehouseMovementView.Price,
                                                                          warehouseMovementView.Amount_Unit_Billing,
                                                                          warehouseMovementView.Amount_Unit_Shipping, 
                                                                          TypeOfAVGPC,
                                                                          UpdateCosts);
        }
        
        private void UpdateGoodInformation(WarehouseMovementsView NowadaysWarehouseMovementView, 
                                           WarehouseMovementsView NewWarehouseMovementView, 
                                           HispaniaCompData.Good good, bool UpdateCosts)
        {
            TypeOfUpdateAVGPC TypeOfAVGPC;
            switch (NewWarehouseMovementView.Type)
            {
                case 1:
                     TypeOfAVGPC = TypeOfUpdateAVGPC.AddUnitsEntry;
                     break;
                case 5:
                     TypeOfAVGPC = TypeOfUpdateAVGPC.AddUnitsDeparture;
                     break;
                default:
                     throw new ArgumentException(
                                   string.Format("Error, tipus de moviment de magatzem no reconegut '{0}'.",
                                                  NewWarehouseMovementView.Type));
            }
            decimal Amount_Unit_Billing = NewWarehouseMovementView.Amount_Unit_Billing - NowadaysWarehouseMovementView.Amount_Unit_Billing;
            decimal Amount_Unit_Shipping = NewWarehouseMovementView.Amount_Unit_Shipping - NowadaysWarehouseMovementView.Amount_Unit_Shipping;
            GlobalViewModel.Instance.HispaniaViewModel.UpdateGoodUnitInfo(good, 
                                                                          NewWarehouseMovementView.Price,
                                                                          Amount_Unit_Billing, 
                                                                          Amount_Unit_Shipping,
                                                                          TypeOfAVGPC,
                                                                          UpdateCosts);
        }

        public void UpdateGoodInformation(GoodsView good, decimal Nowadays_Amount_Unit_Billing, decimal Nowadays_Amount_Unit_Shipping, 
                                          decimal New_Amount_Unit_Billing, decimal New_Amount_Unit_Shipping, 
                                          decimal New_Price, int New_type, out GoodsView Updated_SelectedGood)
        {
            TypeOfUpdateAVGPC TypeOfAVGPC;
            switch (New_type)
            {
                case 1:
                     TypeOfAVGPC = TypeOfUpdateAVGPC.AddUnitsEntry;
                     break;
                case 5:
                     TypeOfAVGPC = TypeOfUpdateAVGPC.AddUnitsDeparture;
                     break;
                default:
                    throw new ArgumentException(
                                  string.Format("Error, tipus de moviment de magatzem no reconegut '{0}'.", New_type));
            }
            decimal Updated_Amount_Unit_Billing = New_Amount_Unit_Billing - Nowadays_Amount_Unit_Billing;
            decimal Updated_Amount_Unit_Shipping = New_Amount_Unit_Shipping - Nowadays_Amount_Unit_Shipping;
            GlobalViewModel.Instance.HispaniaViewModel.UpdateGoodUnitInfo(good,
                                                                          New_Price,
                                                                          Updated_Amount_Unit_Billing,
                                                                          Updated_Amount_Unit_Shipping,
                                                                          TypeOfAVGPC,
                                                                          out Updated_SelectedGood);
        }

        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.WarehouseMovement> ReadWarehouseMovementsInDb(int WarehouseMovement_Id)
        {
            return (HispaniaDataAccess.Instance.ReadWarehouseMovements(WarehouseMovement_Id));
        }

        private void CreateWarehouseMovementInDb(HispaniaCompData.WarehouseMovement warehouseMovementMovement,
                                                 HispaniaCompData.Good good = null)
        {
            HispaniaDataAccess.Instance.CreateWarehouseMovement(warehouseMovementMovement, good);
        }

        private List<HispaniaCompData.WarehouseMovement> _WarehouseMovementsInDb;

        private List<HispaniaCompData.WarehouseMovement> WarehouseMovementsInDb
        {
            get
            {
                return (this._WarehouseMovementsInDb);
            }
            set
            {
                this._WarehouseMovementsInDb = value;
            }
        }

        private void UpdateWarehouseMovementInDb(HispaniaCompData.WarehouseMovement warehouseMovementMovement,
                                                 HispaniaCompData.Good goodOld = null,
                                                 HispaniaCompData.Good goodUpdated = null)
        {
            HispaniaDataAccess.Instance.UpdateWarehouseMovement(warehouseMovementMovement, goodOld, goodUpdated);
        }

        private void DeleteWarehouseMovementInDb(HispaniaCompData.WarehouseMovement warehouseMovementMovement,
                                                 HispaniaCompData.Good good = null)
        {
            HispaniaDataAccess.Instance.DeleteWarehouseMovement(warehouseMovementMovement, good);
        }

        private HispaniaCompData.WarehouseMovement GetWarehouseMovementInDb(int WarehouseMovement_Id)
        {
            return HispaniaDataAccess.Instance.GetWarehouseMovement(WarehouseMovement_Id);
        }

        #endregion
    }
}
