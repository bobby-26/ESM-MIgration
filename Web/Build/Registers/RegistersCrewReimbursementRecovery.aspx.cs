using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegistersCrewReimbursementRecovery : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersCrewReimbursementRecovery.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvReimbursementRecovery')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersCrewReimbursementRecovery.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuRegistersReimbursementRecovery.AccessRights = this.ViewState;
        MenuRegistersReimbursementRecovery.MenuList = toolbar.Show();        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvReimbursementRecovery.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTCODE", "FLDHARDNAME", "FLDTYPENAME", "FLDACTIVEYNAME", "FLDSUBACCOUNT", "FLDCHARGINGBUDGETCODE" };
        string[] alCaptions = { "Code", "Name", "Type", "Active?", "Budget Code", "Charging Budget" };
        string sortexpression;
        int sortdirection;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersReimbursementRecovery.ReimbursementRecoverySearch(0
            , txtRemReCode.Text
            , txtSearch.Text
            , General.GetNullableInteger(ddlReimbursementRecoveryType.SelectedValue)
            , chkReimbursementRecoveryActiveYN.Checked.Equals(true) ? 1 : 0
            , sortexpression, sortdirection
            , gvReimbursementRecovery.CurrentPageIndex + 1, gvReimbursementRecovery.PageSize
            , ref iRowCount, ref iTotalPageCount);
        General.ShowExcel("Reimbursement/Recovery", dt, alColumns, alCaptions, null, null);
    }
    protected void RegistersRegistersReimbursementRecovery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvReimbursementRecovery.SelectedIndexes.Clear();
            gvReimbursementRecovery.EditIndexes.Clear();
            gvReimbursementRecovery.DataSource = null;
            BindData();
            gvReimbursementRecovery.Rebind();
            //gvReimbursementRecovery.CurrentPageIndex = 0;
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns = { "FLDSHORTCODE", "FLDHARDNAME", "FLDTYPENAME", "FLDACTIVEYNAME", "FLDSUBACCOUNT", "FLDCHARGINGBUDGETCODE" };
            string[] alCaptions = { "Code", "Name", "Type", "Active?", "Budget Code", "Charging Budget" };

            DataTable ds = PhoenixRegistersReimbursementRecovery.ReimbursementRecoverySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtRemReCode.Text, txtSearch.Text
                , General.GetNullableInteger(ddlReimbursementRecoveryType.SelectedValue)
                , chkReimbursementRecoveryActiveYN.Checked.Equals(true) ? 1 : 0
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvReimbursementRecovery.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            DataSet ds1 = new DataSet();
            ds1.Tables.Add(ds.Copy());
            General.SetPrintOptions("gvReimbursementRecovery", "Reimbursement/Recovery", alCaptions, alColumns, ds1);

            gvReimbursementRecovery.DataSource = ds;
            gvReimbursementRecovery.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvReimbursementRecovery_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                _gridView.MasterTableView.ClearEditItems();
            }
            if (e.CommandName == "EDIT")
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }
            else
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)gvReimbursementRecovery.MasterTableView.GetItems(GridItemType.Footer)[0];

                string shortname = ((RadTextBox)item.FindControl("txtReimbursementRecoveryNameAdd")).Text;
                string purposename = ((RadTextBox)item.FindControl("txtPurposeAdd")).Text;
                RadComboBox ddl = (RadComboBox)item.FindControl("ddlReimbursementRecoveryAdd");
                byte? activeyn = General.GetNullableByte(((CheckBox)item.FindControl("chkReimbursementRecoveryActiveYNAdd")).Checked.Equals(true) ? "1" : "0".ToString());
                int? type = General.GetNullableInteger(ddl.SelectedValue.ToString());
                int? budgetcode = General.GetNullableInteger(((UserControlBudgetCode)item.FindControl("ddlBudgetCodeAdd")).SelectedBudgetCode);
                int? chargingbudget = General.GetNullableInteger(((UserControlBudgetCode)item.FindControl("ddlChargingBudgetAdd")).SelectedBudgetCode);
                CheckBoxList chkPaymentModeListAdd = (CheckBoxList)item.FindControl("chkPaymentModeListAdd");

                if (!IsValidReimbursementRecovery(shortname, purposename, type))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                CheckBoxList chka = (CheckBoxList)e.Item.FindControl("chkPaymentModeListAdd");
                string PList = "";
                string PaymentModeList = "";
                foreach (ListItem li in chka.Items)
                {
                    if (li.Selected)
                    {
                        PList += li.Value + ",";
                    }
                }

                if (PList != "")
                {
                    PaymentModeList = "," + PList;
                }
                PhoenixRegistersReimbursementRecovery.ReimbursementRecoveryInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode, shortname, purposename, type, activeyn, budgetcode, chargingbudget,
                    General.GetNullableString(PaymentModeList));

                ucStatus.Text = "Reimbursement/Recovery added successfully.";
                gvReimbursementRecovery.Rebind();
            }
            else
                if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string hardcode = ((RadLabel)e.Item.FindControl("lblHardCodeEdit")).Text;
                if (hardcode == "")
                {
                  
                }
                if (hardcode != "")
                {
                    int hardcodeId = Int16.Parse(((RadLabel)e.Item.FindControl("lblHardCodeEdit")).Text);
                    string shortname = ((RadTextBox)e.Item.FindControl("txtReimbursementRecoveryCodeEdit")).Text;
                    string purposename = ((RadTextBox)e.Item.FindControl("txtPurposeEdit")).Text;
                    RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlReimbursementRecoveryTypeEdit");
                    byte? activeyn = General.GetNullableByte(((CheckBox)e.Item.FindControl("chkReimbursementRecoveryActiveYNEdit")).Checked.Equals(true) ? "1" : "0".ToString());
                    int? type = General.GetNullableInteger(ddl.SelectedValue.ToString());
                    int? budgetcode = General.GetNullableInteger(((UserControlBudgetCode)e.Item.FindControl("ddlBudgetCodeEdit")).SelectedBudgetCode);
                    int? chargingbudget = General.GetNullableInteger(((UserControlBudgetCode)e.Item.FindControl("ddlChargingBudgetEdit")).SelectedBudgetCode);

                    if (!IsValidReimbursementRecovery(shortname, purposename, type))
                    {
                        e.Canceled = true;
                        ucError.Visible = true;
                        return;
                    }
                    CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkPaymentModeListEdit");
                    string PList = "";
                    string PaymentModeList = "";
                    foreach (ListItem li in chk.Items)
                    {
                        if (li.Selected)
                        {
                            PList += li.Value + ",";
                        }
                    }

                    if (PList != "")
                    {
                        PaymentModeList = "," + PList;
                    }
                    PhoenixRegistersReimbursementRecovery.ReimbursementRecoveryUpdate(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , hardcodeId, shortname, purposename, type, activeyn, budgetcode, chargingbudget,
                        General.GetNullableString(PaymentModeList)
                        );
                    ucStatus.Text = "Reimbursement/Recovery updated successfully.";
                }
                gvReimbursementRecovery.SelectedIndexes.Clear();
                gvReimbursementRecovery.EditIndexes.Clear();
                gvReimbursementRecovery.DataSource = null;
                gvReimbursementRecovery.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersReimbursementRecovery.DeleteReimbursementRecovery
                    (PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   Convert.ToInt32(((RadLabel)e.Item.FindControl("lblHardCode")).Text)
                        );

                gvReimbursementRecovery.SelectedIndexes.Clear();
                gvReimbursementRecovery.EditIndexes.Clear();
                gvReimbursementRecovery.DataSource = null;
                gvReimbursementRecovery.Rebind();
            }
            if (e.CommandName == "Page")
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
    protected void gvReimbursementRecovery_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            CheckBoxList chkPaymentModeListEdit = (CheckBoxList)e.Item.FindControl("chkPaymentModeListEdit");
            if (chkPaymentModeListEdit != null)
            {
          
            CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkPaymentModeListEdit");
            foreach (ListItem li in chk.Items)
            {
                string[] slist = drv["FLDPAYMENTMODELISTID"].ToString().Split(',');
                foreach (string s in slist)
                {
                    if (li.Value.Equals(s))
                    {
                        li.Selected = true;
                    }
                }
            }
        }


        LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton map = (LinkButton)e.Item.FindControl("cmdMap");
            if (map != null)
            {
                map.Visible = SessionUtil.CanAccess(this.ViewState, map.CommandName);
                map.Attributes.Add("onclick", "javascript:Openpopup('codeHelpOtherDocMap','','RegistersPoolOtherDocMapping.aspx?pool=" + DataBinder.Eval(e.Item.DataItem, "FLDPOOLID").ToString() + "&type=95'); return false;");
            }

            RadLabel lblType = (RadLabel)e.Item.FindControl("lblReimbursementRecoveryTypeEdit");
            RadComboBox ddlType = (RadComboBox)e.Item.FindControl("ddlReimbursementRecoveryTypeEdit");

            if (ddlType != null)
            {
                if (lblType != null)

                    ddlType.SelectedValue = lblType.Text;
            }

            UserControlBudgetCode budget = (UserControlBudgetCode)e.Item.FindControl("ddlBudgetCodeEdit");
            if (budget != null) budget.SelectedBudgetCode = DataBinder.Eval(e.Item.DataItem, "FLDBUDGETID").ToString();
            UserControlBudgetCode chargingbudget = (UserControlBudgetCode)e.Item.FindControl("ddlChargingBudgetEdit");
            if (chargingbudget != null) chargingbudget.SelectedBudgetCode = DataBinder.Eval(e.Item.DataItem, "FLDCHARGINGBUDGET").ToString();
        }
     
    }
    private void DeleteReimbursementRecovery(int Hardcode)
    {
        PhoenixRegistersReimbursementRecovery.DeleteReimbursementRecovery(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Hardcode);
    }
    private bool IsValidReimbursementRecovery(string shortname, string purposename, int? type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortname.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";
        if (purposename.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";
        if (type == null)
            ucError.ErrorMessage = "Type is required.";
        return (!ucError.IsError);
    }
    protected void gvReimbursementRecovery_UpdateCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvReimbursementRecovery_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReimbursementRecovery.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

