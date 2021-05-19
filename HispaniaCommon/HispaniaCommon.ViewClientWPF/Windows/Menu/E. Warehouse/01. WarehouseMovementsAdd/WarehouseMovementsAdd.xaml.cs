#region Librerias usadas por la clase

using HispaniaCommon.ViewClientWPF.UserControls;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Lógica de interacción para WarehouseMovementsAdd.xaml
    /// </summary>
    public partial class WarehouseMovementsAdd : Window
    {
        #region Attributes

        /// <summary>
        /// Store the data to the id associated to the movement created.
        /// </summary>
        private static int m_NonSavedId = 0;

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private Dictionary<string, GoodsView> m_DataGoods = new Dictionary<string, GoodsView>();

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private Dictionary<string, ProvidersView> m_DataProviders = new Dictionary<string, ProvidersView>();

        /// <summary>
        /// Store a value that indicates if one Warehouse Movement is editing.
        /// </summary>
        private static int m_IsEditingWarehouseMovement = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Store the data to the id associated to the movement created.
        /// </summary>
        public static int NonSavedId
        {
            get
            {
                m_NonSavedId = m_NonSavedId - 1;
                return m_NonSavedId;
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public Dictionary<string, GoodsView> DataGoods
        {
            get
            {
                return (m_DataGoods);
            }
            set
            {
                if (value != null) m_DataGoods = new Dictionary<string, GoodsView>(value);
                else m_DataGoods = new Dictionary<string, GoodsView>();
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public Dictionary<string, ProvidersView> DataProviders
        {
            get
            {
                return (m_DataProviders);
            }
            set
            {
                if (value != null) m_DataProviders = new Dictionary<string, ProvidersView>(value);
                else m_DataProviders = new Dictionary<string, ProvidersView>();
            }
        }


        /// <summary>
        /// Store a value that indicates if one Warehouse Movement is editing.
        /// </summary>
        public static int IsEditingWarehouseMovement
        {
            get
            {
                return m_IsEditingWarehouseMovement;
            }
            set
            {
                m_IsEditingWarehouseMovement = value;
            }
        }

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default
        /// </summary>
        public WarehouseMovementsAdd()
        {
            InitializeComponent();
            InitializeWindow();
            LoadManagers();
        }

        private void InitializeWindow()
        {
            //  Define the manager that will do the actions needed for initialize the window when this is loaded.
                Loaded += WarehouseMovementsAdd_Loaded;
        }

        /// <summary>
        /// Manage the actions that must be done when the interaction with the Window is ready. 
        /// </summary>
        /// <param name="sender">Object that sends this event.</param>
        /// <param name="e">Parameters with the event has been sent.</param>
        private void WarehouseMovementsAdd_Loaded(object sender, RoutedEventArgs e)
        {
            //  Create the first control to enter Warehouse Movements in the Window
                CreateNewWarehouseMovementData();
        }

        #endregion
        
        #region Managers

        private void LoadManagers()
        {
            //  Exit Window Button
                Closing += WarehouseMovementsAdd_Closing;
            //  Button
                btnExit.Click += BtnExit_Click;
        }

        #region Window

        private void WarehouseMovementsAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !ValidateOKForClose();
        }

        #endregion

        #region Button

        /// <summary>
        /// Manage the action produced when the user try to close the window.
        /// </summary>
        /// <param name="sender">Object that send the event.</param>
        /// <param name="e">Parameters with the event was sent</param>
        private void BtnExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ValidateOKForClose()) Close();
        }

        #endregion

        #endregion

        #region Managers for events of the Warehouse Movement Add Data control.

        /// <summary>
        /// Manage the event that's produced when the user press the button for Add a New Movement.
        /// </summary>
        /// <param name="sender">Movement to add.</param>
        private void WarehouseMovementAddDataControl_evAddWarehouseMovement(WarehouseMovementsAddData sender)
        {
            //  Create a new control that allow at user to introduce a new Warehouse Movement.
                if (CreateNewWarehouseMovementData())
                {
                    //  Update the variable that indicate that the new control has been created
                        sender.AddNewMovementPressed = true;
                }
        }

        /// <summary>
        /// Manage the event that's produced when the user press the button for delete a Movement saved.
        /// </summary>
        /// <param name="MovementControl">Movement to delete.</param>
        /// <param name="RowDefinitionControl">Control associated at the Movement deleted.</param>
        private void WarehouseMovementAddDataControl_evDeleteWarehouseMovement(WarehouseMovementsAddData MovementControl, RowDefinition RowDefinitionControl, GoodsView good)
        {
            try
            {
                //  Update Goods List
                    if (good != null)
                    {
                        if (DataGoods.ContainsKey(good.Good_Code)) DataGoods.Remove(good.Good_Code);
                        DataGoods.Add(good.Good_Code, good);
                    }
                //  Remove the control from the list of Warehouse Movement Data controls (using index).
                    if (RemoveWarehouseMovementData(MovementControl, RowDefinitionControl, out int IndexRemovedItem))
                    {
                        //  Create a new Warehouse Movement Item if there are no items in the list.
                            if (DataWarehouseMovementsGrid.RowDefinitions.Count == 0)
                            {
                                CreateNewWarehouseMovementData();
                            }
                            else
                            {
                                int MovementsCount = DataWarehouseMovementsGrid.Children.Count;
                                for (int index = IndexRemovedItem; index < MovementsCount; index++)
                                {
                                    Grid.SetRow((WarehouseMovementsAddData)DataWarehouseMovementsGrid.Children[index], index);
                                }
                                WarehouseMovementsAddData WarehouseMovementsAddDataControl = (WarehouseMovementsAddData)DataWarehouseMovementsGrid.Children[MovementsCount - 1];
                                if (WarehouseMovementsAddDataControl.State != WarehouseMovementsAddData.ActionState.InitialState)
                                {
                                    WarehouseMovementsAddDataControl.ShowPressedButton = true;
                                    WarehouseMovementsAddDataControl.UpdateLayout();
                                }
                    }
                    }
            }
            catch (Exception ex)
            {
                //  Show at user the error that has ocurred during the creation of the new control.
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'esborrar el control associat al moviment eliminat.\r\nDetalls: {0}", 
                                     MsgManager.ExcepMsg(ex)));

            }
        }

        /// <summary>
        /// Update in the main window the data of the movement selected.
        /// </summary>
        /// <param name="movement">Movement that contains the data to update.</param>
        /// <param name="good">Good associated at the selected movement</param>
        private void WarehouseMovementAddDataControl_evUpdateInfoWarehouseMovement(WarehouseMovementsView movement, GoodsView good)
        {
            //  Update Goods List
                if (good != null)
                {
                    if (DataGoods.ContainsKey(good.Good_Code)) DataGoods.Remove(good.Good_Code);
                    DataGoods.Add(good.Good_Code, good);
                }
            //  Call to the method that Update the information of the movement.
                UpdateInfoWarehouseMovement(movement, good);
        }

        #endregion

        #region Shared Methods

        /// <summary>
        /// Method that determine if it's possible close the Movements Add Window.
        /// </summary>
        private bool ValidateOKForClose()
        {
            bool Close = false;
            string ErrMsg = "Error, al tancar la finestra d'entrada de moviments de magatzem.\r\nDetalls: {0}";
            string ErrDefault = "Hi ha un moviment de magatzem en fase de creació pendent de gravar.";
            string ErrEditing = "El moviment {0} no está guardat a la Base de Dades.\r\n";
            try
            {
                //  Prepare the variable that contains the validation errors produced.
                    StringBuilder sbErrValidation = new StringBuilder(string.Empty);
                //  Validate the State of the Warehouse Movements containeds in the Window.
                    foreach (WarehouseMovementsAddData WarehouseMovementControl in DataWarehouseMovementsGrid.Children)
                    {
                        if (!WarehouseMovementControl.IsDataSaved)
                        {
                            if (WarehouseMovementControl.WarehouseMovement_Id < 0)
                            {
                                sbErrValidation.AppendLine(ErrDefault);
                            }
                            else
                            {
                                sbErrValidation.AppendFormat(ErrEditing, WarehouseMovementControl.WarehouseMovement_Id);
                            }
                        }
                    }
                //  If all validations are ok we proceed to close this window.
                    if (sbErrValidation.Length == 0) Close = true;
                    else
                    {
                        MsgManager.ShowMessage(string.Format(ErrMsg, sbErrValidation.ToString()));
                    }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format(ErrMsg, MsgManager.ExcepMsg(ex)));
            }
            return Close;
        }

        /// <summary>
        /// Do the required actions to create a new control for introduce a Warehouse Movement in the Window.
        /// </summary>
        private bool CreateNewWarehouseMovementData()
        {
            try
            {
                //  Create the New Row that will contains the new control for the Warehouse Movement.
                    RowDefinition NewRow = new RowDefinition
                    {
                        Height = new GridLength(40.0),
                        Name = string.Format("Row_{0}", DataWarehouseMovementsGrid.RowDefinitions.Count)
                    };
                //  Add the new row to the grid that contains the Warehouse Movement's data.
                    DataWarehouseMovementsGrid.RowDefinitions.Add(NewRow);
                //  Create the new control for the Warehouse Movement.
                    WarehouseMovementsAddData WarehouseMovementAddDataControl = new WarehouseMovementsAddData
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(4.0, 2.0, 4.0, 2.0),
                        ParentControl = NewRow,
                        DataGoods = DataGoods,
                        DataProviders = DataProviders, 
                    };
                    Grid.SetRow(WarehouseMovementAddDataControl, DataWarehouseMovementsGrid.RowDefinitions.IndexOf(NewRow));
                    Grid.SetColumn(WarehouseMovementAddDataControl, 0);
                    WarehouseMovementAddDataControl.EvAddWarehouseMovement += WarehouseMovementAddDataControl_evAddWarehouseMovement;
                    WarehouseMovementAddDataControl.EvDeleteWarehouseMovement += WarehouseMovementAddDataControl_evDeleteWarehouseMovement;
                    WarehouseMovementAddDataControl.EvUpdateInfoWarehouseMovement += WarehouseMovementAddDataControl_evUpdateInfoWarehouseMovement;
                //  Add this control to the new row.
                    DataWarehouseMovementsGrid.Children.Add(WarehouseMovementAddDataControl);
                //  Clear the information of the movement in this window.
                    ClearInfoWarehouseMovement();
                //  Indicate that the creation of new control has finished without errors
                    return true;
            }
            catch (Exception ex)
            {
                //  Show at user the error that has ocurred during the creation of the new control.
                    MsgManager.ShowMessage(
                       string.Format("Error, al crear el control per introduir un nou moviment.\r\nDetalls: {0}", 
                                     MsgManager.ExcepMsg(ex)));
                //  Indicate that the creation of new control has finished with errors
                    return false;
            }
        }

        /// <summary>
        /// Do the required actions to create a new control for introduce a Warehouse Movement in the Window.
        /// </summary>
        private bool RemoveWarehouseMovementData(WarehouseMovementsAddData WarehouseMovementAddDataControl, RowDefinition RowDefinitionControl, out int IndexRemovedItem)
        {
            try
            {
                //  Remove The WarehouseMovementControl.
                    WarehouseMovementAddDataControl.EvAddWarehouseMovement -= WarehouseMovementAddDataControl_evAddWarehouseMovement;
                    WarehouseMovementAddDataControl.EvDeleteWarehouseMovement -= WarehouseMovementAddDataControl_evDeleteWarehouseMovement;
                    WarehouseMovementAddDataControl.EvUpdateInfoWarehouseMovement -= WarehouseMovementAddDataControl_evUpdateInfoWarehouseMovement;
                //  Remove this control to the new row.
                    DataWarehouseMovementsGrid.Children.Remove(WarehouseMovementAddDataControl);
                //  Remove RowDefinition
                    IndexRemovedItem = DataWarehouseMovementsGrid.RowDefinitions.IndexOf(RowDefinitionControl);
                    DataWarehouseMovementsGrid.RowDefinitions.RemoveAt(IndexRemovedItem);
                //  Clear the information of the movement in this window.
                    ClearInfoWarehouseMovement();
                //  Indicate that the creation of new control has finished without errors
                    return true;
            }
            catch (Exception ex)
            {
                //  Show at user the error that has ocurred during the creation of the new control.
                    MsgManager.ShowMessage(
                       string.Format("Error, al crear el control per introduir un nou moviment.\r\nDetalls: {0}", 
                                     MsgManager.ExcepMsg(ex)));
                //  Index if error.
                    IndexRemovedItem = -1;
                //  Indicate that the creation of new control has finished with errors
                    return false;
            }
        }

        /// <summary>
        /// Clear values of controls of this Window that contains information of the Warehouse Movement.
        /// </summary>
        private void ClearInfoWarehouseMovement()
        {
            tbUnitBillingDefinition.ToolTip = tbUnitBillingDefinition.Text = string.Empty;
            tbUnitShippingDefinition.ToolTip = tbUnitShippingDefinition.Text = string.Empty;
            tbAmount.Text = string.Empty;
            tbValue.Text = string.Empty;
            tbPriceCost.Text = string.Empty;
            tbAveragePriceCost.Text = string.Empty;
            tbBillingUnitStocks.Text = string.Empty;
            tbShippingUnitStocks.Text = string.Empty;
            tbBillingUnitAvailable.Text = string.Empty;
            tbShippingUnitAvailable.Text = string.Empty;
            tbBillingUnitEntrance.Text = string.Empty;
            tbShippingUnitEntrance.Text = string.Empty;
            tbBillingUnitDepartures.Text = string.Empty;
            tbShippingUnitDepartures.Text = string.Empty;
        }

        /// <summary>
        /// Update in the main window the data of the movement selected.
        /// </summary>
        /// <param name="movement">Movement that contains the data to update.</param>
        /// <param name="good">Good associated at the selected movement</param>
        private void UpdateInfoWarehouseMovement(WarehouseMovementsView movement, GoodsView good)
        {
            try
            {
                string Empty = string.Empty;
                bool IsGoodNull = good is null;
                tbUnitBillingDefinition.Text = IsGoodNull ? Empty : movement.Unit_Billing_Definition;
                tbUnitBillingDefinition.ToolTip = tbUnitBillingDefinition.Text;
                tbUnitShippingDefinition.Text = IsGoodNull ? Empty : movement.Unit_Shipping_Definition;
                tbUnitShippingDefinition.ToolTip = tbUnitShippingDefinition.Text;
                tbAmount.Text = GlobalViewModel.GetStringFromDecimalValue(movement.Amount, DecimalType.Currency, true);
                tbValue.Text = GlobalViewModel.GetStringFromDecimalValue(movement.AmountCost, DecimalType.Currency, true);
                tbPriceCost.Text = IsGoodNull ? Empty : GlobalViewModel.GetStringFromDecimalValue(good.Price_Cost, DecimalType.Currency, true);
                tbAveragePriceCost.Text = IsGoodNull ? Empty : GlobalViewModel.GetStringFromDecimalValue(good.Average_Price_Cost, DecimalType.Currency, true);
                tbBillingUnitStocks.Text = IsGoodNull ? Empty : GlobalViewModel.GetStringFromDecimalValue(good.Billing_Unit_Stocks, DecimalType.Unit, true);
                tbShippingUnitStocks.Text = IsGoodNull ? Empty : GlobalViewModel.GetStringFromDecimalValue(good.Shipping_Unit_Stocks, DecimalType.Unit, true);
                tbBillingUnitAvailable.Text = IsGoodNull ? Empty : GlobalViewModel.GetStringFromDecimalValue(good.Billing_Unit_Available, DecimalType.Unit, true);
                tbShippingUnitAvailable.Text = IsGoodNull ? Empty : GlobalViewModel.GetStringFromDecimalValue(good.Shipping_Unit_Available, DecimalType.Unit, true);
                tbBillingUnitEntrance.Text = IsGoodNull ? Empty : GlobalViewModel.GetStringFromDecimalValue(good.Billing_Unit_Entrance, DecimalType.Unit, true);
                tbShippingUnitEntrance.Text = IsGoodNull ? Empty : GlobalViewModel.GetStringFromDecimalValue(good.Shipping_Unit_Entrance, DecimalType.Unit, true);
                tbBillingUnitDepartures.Text = IsGoodNull ? Empty : GlobalViewModel.GetStringFromDecimalValue(good.Billing_Unit_Departure, DecimalType.Unit, true);
                tbShippingUnitDepartures.Text = IsGoodNull ? Empty : GlobalViewModel.GetStringFromDecimalValue(good.Shipping_Unit_Departure, DecimalType.Unit, true);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                   string.Format("Error, al carregar les dades del moviment de la pantalla principal.\r\nDetalls:{0}",
                                 MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion
    }
}
