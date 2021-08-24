using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class VesselAccountsPettyCashGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                btnconfirm.Attributes.Add("style", "display:none");
                btnunconfirm.Attributes.Add("style", "display:none");
                SessionUtil.PageAccessRights(this.ViewState);
                //  txtToDate.Text = Request.QueryString["todate"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["vesselid"] = Filter.CurrentMenuCodeSelection == "VAC-PBL-PTC" ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : Request.QueryString["Vesselid"];
                if (string.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    txtFromDate.Text = Request.QueryString["fromdate"].ToString();
                    txtToDate.Text = Request.QueryString["todate"].ToString();
                    lblFromDate.Visible = false;
                    txtFromDate.Visible = false;
                    lblToDate.Visible = false;
                    txtToDate.Visible = false;
                    txtlog.Text = "Log (" + txtFromDate.Text + " - " + txtToDate.Text + ")";
                    txtlog.Visible = true;
                    lbllog.Visible = true;
                }
                else
                {
                    txtFromDate.Text = Request.QueryString["fromdate"].ToString();
                    txtToDate.Text = LastDayOfMonthFromDateTime(DateTime.Parse(txtFromDate.Text)).ToString();
                    //  Title1.Text = "Captain Cash (" + txtFromDate.Text + " - " + txtToDate.Text + ")";
                    txtlog.Visible = false; lbllog.Visible = false;
                }
            }
            if (Filter.CurrentMenuCodeSelection == "VAC-PBL-PTC")
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                if (string.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    toolbarmain = new PhoenixToolbar();
                    toolbarmain.AddButton("UnLock Report", "UNLOCK", ToolBarDirection.Right);
                    MenuPettyCash1.AccessRights = this.ViewState;
                    MenuPettyCash1.MenuList = toolbarmain.Show();
                }
                else
                {
                    toolbarmain = new PhoenixToolbar();
                    toolbarmain.AddButton("Show Report", "GEN", ToolBarDirection.Right);
                    toolbarmain.AddButton("Lock", "LOCK", ToolBarDirection.Right);
                    MenuPettyCash1.AccessRights = this.ViewState;
                    MenuPettyCash1.MenuList = toolbarmain.Show();
                }
            }
            if (General.GetNullableDateTime(txtToDate.Text).HasValue)
            {
                BindCTMData(DateTime.Parse(txtToDate.Text));
            }
            Menu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPettyCash_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsPettyCashGeneral.aspx";
            }
            else if (CommandName.ToUpper().Equals("RECEIPTS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCashReceiptsSummary.aspx";
            }
            else if (CommandName.ToUpper().Equals("BONDEDSUMMARY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsBondedSummary.aspx";
            }
            else if (CommandName.ToUpper().Equals("PROVISION"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsProvisionSummary.aspx";
            }
            else if (CommandName.ToUpper().Equals("CASHADVANCE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCashAdvanceSummary.aspx";
            }
            else if (CommandName.ToUpper().Equals("PETTY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsPettyExpensesSummary.aspx";
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCash.aspx", false);
            }
            else
            {
                if (string.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"] + "?fromdate=" + txtFromDate.Text + "&todate=" + txtToDate.Text + "&vesselid=" + ViewState["vesselid"].ToString(), false);
                }
                else Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"] + "?fromdate=" + txtFromDate.Text + "&todate=" + txtToDate.Text + "&type=n" + "&vesselid=" + ViewState["vesselid"].ToString(), false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPettyCash1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GEN"))
            {
                if (!General.GetNullableDateTime(txtToDate.Text).HasValue)
                {
                    ucError.ErrorMessage = "Closing Date is required";
                    ucError.Visible = true;
                    return;
                }
                BindCTMData(DateTime.Parse(txtToDate.Text));
                Menu();
            }
            else if (CommandName.ToUpper().Equals("LOCK"))
            {
                BindCTMData(DateTime.Parse(txtToDate.Text));

                if (!IsValidCTM(txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                RadWindowManager1.RadConfirm("Captain Cash will be Locked and no further changes can be made. Please confirm you want to proceed ?", "confirm", 320, 150, null, "Confirm");

            }
            else if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                if (!IsValidCTM(txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                RadWindowManager1.RadConfirm("Are you sure you want to UnLock ?", "unconfirm", 320, 150, null, "UnConfirm");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Menu()
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Summary", "GENERAL");
        toolbar.AddButton("Receipts", "RECEIPTS");
        toolbar.AddButton("Bond", "BONDEDSUMMARY");
        toolbar.AddButton("Provision", "PROVISION");
        toolbar.AddButton("Cash Advance", "CASHADVANCE");
        toolbar.AddButton("Petty Cash", "PETTY");
        if (Filter.CurrentMenuCodeSelection == "VAC-PBL-PTC")
            toolbar.AddButton("List", "BACK");
        MenuPettyCash.AccessRights = this.ViewState;
        MenuPettyCash.MenuList = toolbar.Show();
        MenuPettyCash.SelectedMenuIndex = 0;
    }
    private void BindCTMData(DateTime date)
    {
        try
        {
            DataSet ds = PhoenixVesselAccountsCTM.ListCaptainCashCalculation(int.Parse(ViewState["vesselid"].ToString())
                                                                             , date);
            gvCTM.DataSource = ds;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txtFromDate.Text = ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString();
                txtToDate.Text = ds.Tables[0].Rows[0]["FLDTODATE"].ToString();
            }
            else
            {
                txtFromDate.Text = Request.QueryString["fromdate"].ToString();
                txtToDate.Text = date.ToString("dd/MM/yyyy");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    decimal r, p;
    protected void gvCTM_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridHeaderItem)
            {
                r = 0;
                p = 0;
            }
            if (e.Item is GridDataItem)
            {
                if (drv["FLDCREDITDEBIT"].ToString().ToString() != string.Empty && ",1,".Contains("," + drv["FLDCREDITDEBIT"].ToString() + ",")) r = r + decimal.Parse(drv["FLDAMOUNT"].ToString());
                if (drv["FLDCREDITDEBIT"].ToString().ToString() != string.Empty && !",1,".Contains("," + drv["FLDCREDITDEBIT"].ToString() + ",")) p = p + decimal.Parse(drv["FLDAMOUNT"].ToString());
            }
            if (e.Item is GridFooterItem)
            {
                e.Item.Cells[3].Text = r.ToString();
                e.Item.Cells[4].Text = p.ToString();
                txtPayments.Text = p.ToString();
                txtReceipts.Text = r.ToString();
                txtclosingbalance.Text = (r - p).ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            if (General.GetNullableDateTime(txtToDate.Text).HasValue)
            {
                BindCTMData(DateTime.Parse(txtToDate.Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void LockCTM_Confirm(object sender, EventArgs e)
    {
        try
        {
            BindCTMData(DateTime.Parse(txtToDate.Text));

            PhoenixVesselAccountsCTM.InsertCaptainCashBalance(int.Parse(ViewState["vesselid"].ToString()), DateTime.Parse(txtToDate.Text), null);
            Response.Redirect("../VesselAccounts/VesselAccountsPettyCash.aspx", false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void UnLockCTM_Confirm(object sender, EventArgs e)
    {
        try
        {
            PhoenixVesselAccountsCTM.DeleteCaptainCashBalance(int.Parse(ViewState["vesselid"].ToString()), DateTime.Parse(txtToDate.Text));
            Response.Redirect("../VesselAccounts/VesselAccountsPettyCash.aspx", false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidCTM(string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Closing Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Closing Date should be earlier than current date";
        }
        if (General.GetNullableDateTime(txtFromDate.Text).HasValue && General.GetNullableDateTime(date).HasValue
            && (General.GetNullableDateTime(txtFromDate.Text).Value.Month != General.GetNullableDateTime(date).Value.Month
            || General.GetNullableDateTime(txtFromDate.Text).Value.Year != General.GetNullableDateTime(date).Value.Year))
        {
            ucError.ErrorMessage = "Captain Cash can only be locked if the From Date and To Date are of the same month.";
        }
        return (!ucError.IsError);
    }
    private DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
    {
        DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
    }
}
