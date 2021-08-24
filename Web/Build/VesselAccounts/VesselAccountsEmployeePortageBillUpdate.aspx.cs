using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsEmployeePortageBillUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEmployeePortagebillUpdate.aspx", "export to excel", "<i class=\"fas fa-file-excel\"></i>", "excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvPortageBillList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuComponentList.AccessRights = this.ViewState;
            MenuComponentList.MenuList = toolbargrid.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("PB Correction", "PBCORRECTION");
            toolbar.AddButton("Leave Wages Correction", "LEAVEWAGECORRECTION");
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbar.Show();
            MenuPB.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ListPortageBill();
                gvPortageBillList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LEAVEWAGECORRECTION"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountAdminPortagebillList.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("PBCORRECTION"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsEmployeePortageBillUpdate.aspx", false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvPortageBillList.SelectedIndexes.Clear();
        gvPortageBillList.EditIndexes.Clear();
        gvPortageBillList.DataSource = null;
        gvPortageBillList.Rebind();
    }
    protected void MenuComponentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void gvPortageBillList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }

    }
    protected void gvPortageBillList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPortageBillList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDTOTALEARNINGS", "FLDTOTALDEDUCTION", "FLDBROUGHTFORWARD", "FLDFINALBALANCE", "FLDONBACCRUALTOTALDEDUCTION", "FLDONBACCRUALSBF", "FLDONBACCRUALSFB", "FLDOFFACCRUALTOTALEARNINGS", "FLDOFFACCRUALTOTALDEDUCTION", "FLDOFFACCRUALSBF", "FLDOFFACCRUALSFB" };
        string[] alCaptions = { "File No", "Name", "Total Earnings", "Total Deductions", "Brought Forward", "Final Balance", "Accrual Tot Deductions", "Accrual SBF", "Accrual SFB", "Accrual Tot Earnings", "Accrual Tot Deduction", "Accrual SBF", "Accrual SFB" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DateTime d = DateTime.Now;
        DataSet ds = new DataSet();
        if (General.GetNullableDateTime(ddlPbclosingDate.SelectedValue).HasValue)
            d = General.GetNullableDateTime(ddlPbclosingDate.SelectedValue).Value;
        ds = PhoenixVesselAccountsEmployeePortageBill.EmployeePortageBillSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, d, sortexpression, sortdirection,
                                                                            (int)ViewState["PAGENUMBER"], gvPortageBillList.PageSize, ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvPortageBillList", "Employee Portage Bill", alCaptions, alColumns, ds);
        gvPortageBillList.DataSource = ds;
        gvPortageBillList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        DataTable dt;
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDTOTALEARNINGS", "FLDTOTALDEDUCTION", "FLDBROUGHTFORWARD", "FLDFINALBALANCE", "FLDONBACCRUALTOTALDEDUCTION", "FLDONBACCRUALSBF", "FLDONBACCRUALSFB", "FLDOFFACCRUALTOTALEARNINGS", "FLDOFFACCRUALTOTALDEDUCTION", "FLDOFFACCRUALSBF", "FLDOFFACCRUALSFB" };
        string[] alCaptions = { "File No", "Name", "Total Earnings", "Total Deductions", "Brought Forward", "Final Balance", "Accrual Tot Deductions", "Accrual SBF", "Accrual SFB", "Accrual Tot Earnings", "Accrual Tot Deduction", "Accrual SBF", "Accrual SFB" };
        int iRowCount;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DateTime d = DateTime.Now;
        if (General.GetNullableDateTime(ddlPbclosingDate.SelectedValue).HasValue)
            d = General.GetNullableDateTime(ddlPbclosingDate.SelectedValue).Value;

        ds = PhoenixVesselAccountsEmployeePortageBill.EmployeePortageBillSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, d, sortexpression, sortdirection,
                                                                                 (int)ViewState["PAGENUMBER"], iRowCount, ref iRowCount, ref iTotalPageCount);
        dt = ds.Tables[0];
        General.ShowExcel("Employee Portage Bill", dt, alColumns, alCaptions, null, null);

    }
    private void ListPortageBill()
    {
        DataTable dt = PhoenixVesselAccountsPortageBill.ListVesselPortageBillHistory(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlPbclosingDate.DataSource = dt;
        ddlPbclosingDate.DataBind();
        ddlPbclosingDate.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ddlPbclosingDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        Rebind();
    }
    private void UpdateSBFAmount(int vesselid, int employeeid, Guid PortageBillId, string AccSBFUpdate, string AccTotEarning, string AccTotDeduction)
    {
        try
        {
            if (!IsValidSBFAmount(AccSBFUpdate, AccTotEarning, AccTotDeduction))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixVesselAccountsEmployeePortageBill.EmployeePortageBillUpdate(vesselid, employeeid, PortageBillId, General.GetNullableDecimal(AccSBFUpdate), General.GetNullableDecimal(AccTotEarning), General.GetNullableDecimal(AccTotDeduction));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidSBFAmount(string SBFAmount, string AccTotEarning, string AccTotDeduction)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        decimal result;

        if (SBFAmount.Trim() != "")
        {
            if (decimal.TryParse(SBFAmount, out result) == false)
                ucError.ErrorMessage = "Amount should be in numeric.";
        }
        if (AccTotEarning.Trim() != "")
        {
            if (decimal.TryParse(AccTotEarning, out result) == false)
                ucError.ErrorMessage = "Amount should be in numeric.";
        }
        if (AccTotDeduction.Trim() != "")
        {
            if (decimal.TryParse(AccTotDeduction, out result) == false)
                ucError.ErrorMessage = "Amount should be in numeric.";
        }
        return (!ucError.IsError);
    }
    protected void gvPortageBillList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                RadLabel vesselid = ((RadLabel)e.Item.FindControl("lblVesselid"));
                RadLabel employeeId = ((RadLabel)e.Item.FindControl("lblEmployeeId"));
                RadLabel PortagebillId = ((RadLabel)e.Item.FindControl("lblPortageBillId"));
                UserControlMaskNumber AccSBF = ((UserControlMaskNumber)e.Item.FindControl("txtonAccrualSBF"));
                UserControlMaskNumber AccTotEarning = ((UserControlMaskNumber)e.Item.FindControl("txtonAccrualTotEarning"));
                UserControlMaskNumber AccTotDeduction = ((UserControlMaskNumber)e.Item.FindControl("txtonAccrualTotDeduction"));

                UpdateSBFAmount(int.Parse(vesselid.Text), int.Parse(employeeId.Text), new Guid(PortagebillId.Text), AccSBF.Text.Trim('_'), AccTotEarning.Text.Trim('_'), AccTotDeduction.Text.Trim('_'));
                ucStatus.Text = "Data updated successfully";
                Rebind();
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
}