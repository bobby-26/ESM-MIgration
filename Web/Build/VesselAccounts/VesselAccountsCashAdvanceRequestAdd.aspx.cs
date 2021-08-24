using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Accounts;

public partial class VesselAccountsCashAdvanceRequestAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCashAdvanceRequestAdd.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCashAdvanceRequest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCashRequest.AccessRights = this.ViewState;
            MenuCashRequest.MenuList = toolbargrid.Show();
            //PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);

            //MenuCashAdvanceRequest.AccessRights = this.ViewState;
            //MenuCashAdvanceRequest.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["UID"] = Guid.Empty;
                Guid? uid = Guid.Empty;
                DateTime d = DateTime.Now;
                DataSet ds;
                ds = PhoenixVesselAccountsPortageBill.ListPortageBill(PhoenixSecurityContext.CurrentSecurityContext.VesselID, d, ref uid);
                ViewState["UID"] = uid;
                ddlCurrency.VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                if (Request.QueryString["requestid"] != null)
                {
                    ViewState["requestid"] = Request.QueryString["requestid"].ToString();
                    EditCashAdvanceRequest(new Guid(Request.QueryString["requestid"].ToString()));
                }

                ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pageno"] == null ? "1" : Request.QueryString["pageno"].ToString());
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCashAdvanceRequest.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditCashAdvanceRequest(Guid requestid)
    {
        DataTable dt = PhoenixVesselAccountsCashAdvanceRequest.EditCashAdvanceRequest(requestid);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtDate.Text = dr["FLDDATE"].ToString();
            ddlCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
        }
    }
    protected void MenuCashRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDSIGNOFFDATE", "FLDFINALBALANCE", "FLDTRANSACTIONAMOUNT", "FLDDATE" };
        string[] alCaptions = { "File No.", "Name", "Rank", "SignOff Date", "Final Balance", "Transaction Amount", "Date" };
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        DateTime dLastActiveDate = General.GetNullableDateTime(DateTime.Now.ToString()).Value;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
        DataTable dt = PhoenixVesselAccountsCashAdvanceRequest.CashAdvanceRequestInsertSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                    , General.GetNullableGuid(ViewState["requestid"].ToString()), new Guid(ViewState["UID"].ToString())
                                   , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], iRowCount, ref iRowCount, ref iTotalPageCount);
        General.ShowExcel("Cash Advance Request", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDSIGNOFFDATE", "FLDFINALBALANCE", "FLDTRANSACTIONAMOUNT", "FLDDATE" };
        string[] alCaptions = { "File No.", "Name", "Rank", "SignOff Date", "Final Balance", "Transaction Amount", "Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
        DataTable dt = PhoenixVesselAccountsCashAdvanceRequest.CashAdvanceRequestInsertSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                   , General.GetNullableGuid(ViewState["requestid"].ToString()), new Guid(ViewState["UID"].ToString())
                                   , sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                   , gvCashAdvanceRequest.PageSize, ref iRowCount, ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        ds.AcceptChanges();
        General.SetPrintOptions("gvCashAdvanceRequest", "Cash Advance Request", alCaptions, alColumns, ds);
        gvCashAdvanceRequest.DataSource = dt;
        gvCashAdvanceRequest.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvCashAdvanceRequest_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCashAdvanceRequest.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCashAdvanceRequest_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
        }
    }
    private void InsertCashAdvanceEarningDeduction(int VesselId, int EmployeeId, int SignOnOffId, DateTime Date, Guid RequestId, decimal TransactionAmount,
                                                    int TransactionCurrency, string Purpose, decimal? ReportingExchangeRate, int? ReportingCurrency,
                                                    decimal? ContractExchangeRate, int? ContractCurrency, int? Status, int? PaidFrom, ref Guid? ReturnEarnDeductid)
    {

        PhoenixVesselAccountsCashAdvanceRequest.InsertCashAdvanceEarningDeduction(VesselId, EmployeeId, SignOnOffId, Date, RequestId, TransactionAmount, TransactionCurrency, Purpose, ReportingExchangeRate, ReportingCurrency,
            ContractExchangeRate, ContractCurrency, Status, PaidFrom, ref ReturnEarnDeductid);

    }
    private void UpdateCashAdvanceEarningDeduction(int VesselId, int EmployeeId, int SignOnOffId, DateTime Date, Guid RequestId, decimal TransactionAmount,
                                                 int TransactionCurrency, string Purpose, decimal? ReportingExchangeRate, int? ReportingCurrency,
                                                 decimal? ContractExchangeRate, int? ContractCurrency, int? Status, int? PaidFrom, Guid? ReturnEarnDeductid)
    {

        PhoenixVesselAccountsCashAdvanceRequest.UpdateCashAdvanceEarningDeduction(VesselId, EmployeeId, SignOnOffId, Date, RequestId, TransactionAmount, TransactionCurrency, Purpose, ReportingExchangeRate, ReportingCurrency,
            ContractExchangeRate, ContractCurrency, Status, PaidFrom, ReturnEarnDeductid);

    }

    protected void Rebind()
    {
        gvCashAdvanceRequest.SelectedIndexes.Clear();
        gvCashAdvanceRequest.EditIndexes.Clear();
        gvCashAdvanceRequest.DataSource = null;
        gvCashAdvanceRequest.Rebind();
    }

    protected void gvCashAdvanceRequest_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string transactionamount = ((UserControlMaskNumber)e.Item.FindControl("txtTransactionAmountEdit")).Text;
                string purpose = ((RadTextBox)e.Item.FindControl("txtPurposeEdit")).Text;
                string EarningDeductionId = ((RadLabel)e.Item.FindControl("lblEarningDeductionId")).Text;
                if (!IsValidCashRequest(transactionamount, purpose))
                {
                    ucError.Visible = true;
                    return;
                }
                if (EarningDeductionId == "")
                {

                    Guid Requestid = new Guid();

                    Guid? ReturnEarnDeductid = new Guid();

                    NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
                    InsertCashAdvanceEarningDeduction(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                            int.Parse(((RadLabel)e.Item.FindControl("lblEmployeeId")).Text),
                            int.Parse(((RadLabel)e.Item.FindControl("lblSignOnOffId")).Text),
                            DateTime.Parse(txtDate.Text), new Guid(ViewState["requestid"].ToString()),
                            decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtTransactionAmountEdit")).Text),
                            int.Parse(ddlCurrency.SelectedCurrency), purpose, null, null, null, null,2, 1, ref ReturnEarnDeductid);
                    ViewState["REQUESTID"] = Requestid;
                    ViewState["RETURNEARNDEDUCTID"] = ReturnEarnDeductid;
                }
                else
                {
                    NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
                    UpdateCashAdvanceEarningDeduction(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                            int.Parse(((RadLabel)e.Item.FindControl("lblEmployeeId")).Text),
                            int.Parse(((RadLabel)e.Item.FindControl("lblSignOnOffId")).Text),
                             DateTime.Parse(txtDate.Text),
                            new Guid(((RadLabel)e.Item.FindControl("lblRequestId")).Text),
                            decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtTransactionAmountEdit")).Text),
                              int.Parse(ddlCurrency.SelectedCurrency), purpose, null, null, null, null, 2, 1,
                              General.GetNullableGuid(EarningDeductionId)
                              );
                }
                string Script = "";
                Script += "<script language='JavaScript' id='BookMarkScript'>";
                Script += "fnReloadList('codehelp1',null,'keepopen');;";
                Script += "</script>";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

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

    private bool IsValidCashRequest(string transactionamount, string purpose)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(transactionamount))
            ucError.ErrorMessage = "Transaction Amount is required.";
        if (string.IsNullOrEmpty(purpose))
            ucError.ErrorMessage = "Purpose is required.";
        return (!ucError.IsError);
    }

    protected void MenuCashAdvanceRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../VesselAccounts/VesselAccountsCashAdvanceRequestGeneral.aspx?requestid=" + ViewState["requestid"].ToString() + "&pageno=" + ViewState["PAGENUMBER"], false);
        }

    }
}