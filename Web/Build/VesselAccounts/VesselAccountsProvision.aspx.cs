using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Web.UI;
using SouthNests.Phoenix.Export2XL;
using Telerik.Web.UI;
public partial class VesselAccountsProvision : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("UnLock", "UNLOCK", ToolBarDirection.Right); toolbar.AddButton("Lock", "LOCK", ToolBarDirection.Right);
            MenuPRV.AccessRights = this.ViewState;
            MenuPRV.MenuList = toolbar.Show();
            PhoenixVesselAccountsOrderForm.OrderFormFinalAmountBulkUpdate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, null, null);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsProvision.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsProvision.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsProvision.aspx", "Bulk Update", "<i class=\"fas fa-database\"></i>", "BULKUPDATE");
            MenuStoreItem.AccessRights = this.ViewState;
            MenuStoreItem.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                txtClosingDate.Text = DateTime.Now.ToString();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ListProvisionHistory();
                gvStoreItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPRV_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LOCK"))
            {
                if (!IsValidLock(txtClosingDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                DataTable dt = PhoenixVesselAccountsVictuallingRate.VesselVictuallingRateEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtClosingDate.Text));
                if (dt.Rows.Count > 0)
                {
                    ucConfirmCrew.BudgetedVictualRate = dt.Rows[0]["FLDBUGETEDRATE"].ToString();
                    ucConfirmCrew.ActualVictualRate = dt.Rows[0]["FLDVICTUALLINGRATE"].ToString();
                    ucConfirmCrew.reason = dt.Rows[0]["FLDREASON"].ToString();
                }
                ucConfirmCrew.Visible = true;
                ucConfirmCrew.Text = " <br/>Provision Stock Closing will be Locked and no further changes can be made. <br/>Please confirm you want to proceed?<br/><br/>Reason for High Victualling is mandatory if Actual is higher than Budgeted.";

                ((RadTextBox)ucConfirmCrew.FindControl("txtRemarks")).Focus();
            }
            else if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                if (!IsValidLock(txtClosingDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                ucUnLockConfirm.Visible = true;
                ucUnLockConfirm.Text = "Are you sure you want unlock. Please confirm you want to proceed ?";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ListProvisionHistory()
    {
        DataTable dt = PhoenixVesselAccountsProvision.ListProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlClosingDate.DataSource = dt;
        ddlClosingDate.DataBind();
        ddlClosingDate.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void Rebind()
    {
        gvStoreItem.SelectedIndexes.Clear();
        gvStoreItem.EditIndexes.Clear();
        gvStoreItem.DataSource = null;
        gvStoreItem.Rebind();
    }
    protected void MenuStoreItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDOPENINGSTOCK", "FLDPURCHASEDSTOCK", "FLDCLOSINGSTOCK", "FLDCONSUMEDSTOCK" };
                string[] alCaptions = { "Number", "Name", "Unit", "Opening Stock", "Purchased Stock", "Closing Stock", "Consumption" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataTable dt = PhoenixVesselAccountsProvision.SearchProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                           , General.GetNullableDateTime(txtClosingDate.Text)
                                                           , txtNumber.Text, txtName.Text
                                                           , sortexpression, sortdirection
                                                           , 1, iRowCount
                                                           , ref iRowCount, ref iTotalPageCount
                                                           , General.GetNullableInteger(chkROB.Checked == true ? "1" : "0"));
                General.ShowExcel("Stock check of Provision Items", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }

            else if (CommandName.ToUpper().Equals("BULKUPDATE"))
            {
                UpdateExcelDownload();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void UpdateExcelDownload()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            PhoenixVesselAccounts2XL.PopulateStockCheckProvisionUpdateXl(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                       , General.GetNullableDateTime(txtClosingDate.Text)
                                                       , null, null
                                                       , null, null
                                                       , 1, iRowCount
                                                       , ref iRowCount, ref iTotalPageCount
                                                       , 1);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStoreItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {

                string id = ((RadLabel)e.Item.FindControl("lblstoreitem")).Text;

                string closingstock = ((UserControlMaskNumber)e.Item.FindControl("txtClosingStock")).Text;
                string openingstock = ((RadLabel)e.Item.FindControl("lblOpeningStock")).Text;
                string purchasequantity = ((RadLabel)e.Item.FindControl("lblPurchaseQuantity")).Text;
                if (!IsValidProvisionConsumption(txtClosingDate.Text, openingstock, closingstock, purchasequantity))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsProvision.UpdateProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(id), DateTime.Parse(txtClosingDate.Text)
                    , decimal.Parse(closingstock), decimal.Parse(openingstock));
                ListProvisionHistory();
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblstoreitem")).Text;
                PhoenixVesselAccountsProvision.DeleteProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(id), DateTime.Parse(txtClosingDate.Text));
                ListProvisionHistory();  Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
            if (de != null)
            {
                de.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
                if (drv["FLDCONSUMEDSTOCK"].ToString() == string.Empty) de.Visible = false;
            }
        }
    
    }
    private bool IsValidateRemarks(string remarks, string Budgeted, string Actual)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(Budgeted) < General.GetNullableDecimal(Actual))
        {
            if (string.IsNullOrEmpty(remarks))
                ucError.ErrorMessage = "Reason for High Victualling is required .";
        }
        return (!ucError.IsError);
    }
    protected void btnCrewApprove_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessageProvision ucCM = sender as UserControlConfirmMessageProvision;
            RadTextBox txtrem = (RadTextBox)ucConfirmCrew.FindControl("txtRemarks");
            if (ucCM.confirmboxvalue == 1)
            {
                RadTextBox txtrem1 = (RadTextBox)ucConfirmCrew.FindControl("txtRemarks");
                RadTextBox txtBudgeted = (RadTextBox)ucConfirmCrew.FindControl("txtBudgeted");
                RadTextBox txtActual = (RadTextBox)ucConfirmCrew.FindControl("txtActual");
                if (!IsValidateRemarks(txtrem.Text, txtBudgeted.Text, txtActual.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsProvision.LockProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtClosingDate.Text), General.GetNullableString(txtrem.Text));
                ListProvisionHistory();
                BindData();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval','ifMoreInfo',null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDOPENINGSTOCK", "FLDPURCHASEDSTOCK", "FLDCLOSINGSTOCK", "FLDCONSUMEDSTOCK" };
            string[] alCaptions = { "Number", "Name", "Unit", "Opening Stock", "Purchased Stock", "Closing Stock", "Consumption" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixVesselAccountsProvision.SearchProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableDateTime(txtClosingDate.Text)
                , txtNumber.Text, txtName.Text
                , sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount, General.GetNullableInteger(chkROB.Checked == true ? "1" : "0"));
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvStoreItem", "Stock check of Provision Items", alCaptions, alColumns, ds);

            gvStoreItem.DataSource = dt;
            gvStoreItem.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           
        }
    }

  
  
    protected void LockProvision_Confirm(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                //    PhoenixVesselAccountsProvision.LockProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtClosingDate.Text),"");
            }
            ListProvisionHistory();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void UnLockProvision_Confirm(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                PhoenixVesselAccountsProvision.UnLockProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtClosingDate.Text));
            }
            ListProvisionHistory();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void txtClosingDate_TextChanged(object sender, EventArgs e)
    {
        if (ddlClosingDate.SelectedValue != string.Empty)
            txtClosingDate.Text = ddlClosingDate.SelectedValue;
        else
            txtClosingDate.Text = DateTime.Now.ToString();
        Rebind();
    }
    protected void btnChange_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidChangeDate(ddlClosingDate.SelectedValue, txtChageDate.Text)) { ucError.Visible = true; return; }
            PhoenixVesselAccountsProvision.ChangeClosingDate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(ddlClosingDate.SelectedValue), DateTime.Parse(txtChageDate.Text));
            ListProvisionHistory();
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidLock(string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
            ucError.ErrorMessage = "Closing Date is required.";
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)// && DateTime.Compare(LastDayOfMonthFromDateTime(resultDate).AddDays(-10), resultDate) > 0)
            ucError.ErrorMessage = "Closing Date should be earlier than current";// "date or 10 days prior to monthend.";
        return (!ucError.IsError);
    }
    private bool IsValidChangeDate(string olddate, string newdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(olddate).HasValue)
            ucError.ErrorMessage = "Select the Closing Date in the drop down which needs to changed.";
        if (!General.GetNullableDateTime(newdate).HasValue)
            ucError.ErrorMessage = "Change Closing Date is required.";
        else if (DateTime.TryParse(newdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
            ucError.ErrorMessage = "Change Closing Date should be earlier than current";
        if (General.GetNullableDateTime(olddate).HasValue && General.GetNullableDateTime(newdate).HasValue && DateTime.Parse(olddate).Equals(DateTime.Parse(newdate)))
            ucError.ErrorMessage = "Old Closing date and New Closing date cannot be same.";
        return (!ucError.IsError);
    }
    private bool IsValidProvisionConsumption(string date, string openingstock, string closingstock, string purchasequantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
            ucError.ErrorMessage = "Closing Date is required.";
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
            ucError.ErrorMessage = "Closing Date should be earlier than current date";
        if (!General.GetNullableDecimal(closingstock).HasValue)
            ucError.ErrorMessage = "Closing Stock is required.";
        else if (General.GetNullableDecimal(closingstock).Value < 0)
            ucError.ErrorMessage = "Closing Stock cannot be less than 0.";
        if (General.GetNullableDecimal(openingstock).HasValue && General.GetNullableDecimal(purchasequantity).HasValue
            && General.GetNullableDecimal(closingstock).HasValue && decimal.Parse(closingstock) > (decimal.Parse(openingstock) + decimal.Parse(purchasequantity)))
        {
            ucError.ErrorMessage = "Closing Stock should be lesser than Opening Stock.";
        }
        return (!ucError.IsError);
    }

}
