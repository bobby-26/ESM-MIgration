using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;

public partial class VesselAccountsPettyCashGeneralNew :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                //  txtToDate.Text = Request.QueryString["todate"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (string.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    txtFromDate.Text = Request.QueryString["fromdate"].ToString();
                    txtToDate.Text = Request.QueryString["todate"].ToString();
                    lblFromDate.Visible = false;
                    txtFromDate.Visible = false;
                    lblToDate.Visible = false;
                    txtToDate.Visible = false;
                    Title1.Text = "Log (" + txtFromDate.Text + " - " + txtToDate.Text + ")";
                }
                else
                {
                    txtFromDate.Text = Request.QueryString["fromdate"].ToString();
                    txtToDate.Text = LastDayOfMonthFromDateTime(DateTime.Parse(txtFromDate.Text)).ToString();
                    Title1.Text = "Captain Cash (" + txtFromDate.Text + " - " + txtToDate.Text + ")";
                    //  txtToDate.Text = Request.QueryString["todate"];
                }
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("UnLock Report", "UNLOCK");
                MenuPettyCash1.AccessRights = this.ViewState;
                MenuPettyCash1.MenuList = toolbarmain.Show();
                // if (!General.GetNullableDateTime(txtToDate.Text).HasValue) MenuPettyCash1.Visible = false;
            }
            else
            {
                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Show Report", "GEN");
                toolbarmain.AddButton("Lock", "LOCK");
                MenuPettyCash1.AccessRights = this.ViewState;
                MenuPettyCash1.MenuList = toolbarmain.Show();
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsPettyCashGeneralNew.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("RECEIPTS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCashReceiptsSummary.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("BONDEDSUMMARY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsBondedSummary.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("PROVISION"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsProvisionSummary.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("CASHADVANCE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCashAdvanceSummary.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("PETTY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsPettyExpensesSummary.aspx";
            }
            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashNew.aspx", false);
            }
            else
            {
                if (string.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"] + "?fromdate=" + txtFromDate.Text + "&todate=" + txtToDate.Text, false);
                }
                else Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"] + "?fromdate=" + txtFromDate.Text + "&todate=" + txtToDate.Text + "&type=n", false);
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GEN"))
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
            else if (dce.CommandName.ToUpper().Equals("LOCK"))
            {
                BindCTMData(DateTime.Parse(txtToDate.Text));

                if (!IsValidCTM(txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                ucConfirm.Visible = true;
                ucConfirm.Text = "Captain Cash will be Locked and no further changes can be made. Please confirm you want to proceed ?";
            }
            else if (dce.CommandName.ToUpper().Equals("UNLOCK"))
            {
                if (!IsValidCTM(txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                ucUnLockConfirm.Visible = true;
                ucUnLockConfirm.Text = "Are you sure you want to UnLock ?";
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
        toolbar.AddButton("List", "BACK");
        MenuPettyCash.AccessRights = this.ViewState;
        MenuPettyCash.MenuList = toolbar.Show();
        MenuPettyCash.SelectedMenuIndex = 0;
    }
    private void BindCTMData(DateTime date)
    {

        try
        {
            DataSet ds = PhoenixVesselAccountsCTMNew.ListCaptainCashCalculation(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , date);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvCTM.DataSource = ds;
                gvCTM.DataBind();
                txtFromDate.Text = ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString();
                txtToDate.Text = ds.Tables[0].Rows[0]["FLDTODATE"].ToString();
                Title1.Text = "Log (" + txtFromDate.Text + " - " + txtToDate.Text + ")";
            }
            else
            {
                txtFromDate.Text = Request.QueryString["fromdate"].ToString();
                txtToDate.Text = date.ToString("dd/MM/yyyy");
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCTM);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    decimal r, p;
    protected void gvCTM_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            r = 0;
            p = 0;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (drv["FLDCREDITDEBIT"].ToString().ToString() != string.Empty && ",1,".Contains("," + drv["FLDCREDITDEBIT"].ToString() + ",")) r = r + decimal.Parse(drv["FLDAMOUNT"].ToString());
            if (drv["FLDCREDITDEBIT"].ToString().ToString() != string.Empty && !",1,".Contains("," + drv["FLDCREDITDEBIT"].ToString() + ",")) p = p + decimal.Parse(drv["FLDAMOUNT"].ToString());
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = r.ToString();
            e.Row.Cells[2].Text = p.ToString();
            txtPayments.Text = p.ToString();
            txtReceipts.Text = r.ToString();
            txtclosingbalance.Text = (r - p).ToString();
        }
    }
    protected void LockCTM_Confirm(object sender, EventArgs e)
    {
        try
        {
            BindCTMData(DateTime.Parse(txtToDate.Text));

            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                PhoenixVesselAccountsCTMNew.InsertCaptainCashBalance(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtToDate.Text), null);
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashNew.aspx", false);
            }
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
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                PhoenixVesselAccountsCTMNew.DeleteCaptainCashBalance(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtToDate.Text));
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashNew.aspx", false);
            }
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
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
