using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class AccountsLeaveBalanceBreakDown : PhoenixBasePage
{
    string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            empid = Request.QueryString["empid"];
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                //toolbar.AddImageButton("../VesselAccounts/VesselAccountsEarningDeduction.aspx", "Export to Excel", "icon_xls.png", "Excel");
                //toolbar.AddImageLink("javascript:CallPrint('gvLVP')", "Print Grid", "icon_print.png", "PRINT");
                toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsLeaveOpeningBalance.aspx', true);", "Create Opening Balance", "add.png", "ADDOPENINGBALANCE");
                MenuLeaveBreakDown.AccessRights = this.ViewState;
                MenuLeaveBreakDown.MenuList = toolbar.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGENUMBER1"] = 1;
                ViewState["SORTEXPRESSION1"] = null;
                ViewState["SORTDIRECTION1"] = null;
                gvLVP.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            if (General.GetNullableInteger(empid).HasValue)
            {
                BindData1();
                gvLAR.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuLeaveBreakDown_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {

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
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 0; //DEFAULT DESC SORT
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAccountsLeaveAllotment.SearchLeaveBalanceBreakDown(General.GetNullableInteger(empid), null, null, null
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["PAGENUMBER"], gvLVP.PageSize
                                                        , ref iRowCount, ref iTotalPageCount);
            gvLVP.DataSource = dt;
            gvLVP.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData1()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
            int? sortdirection = 0; //DEFAULT DESC SORT
            if (ViewState["SORTDIRECTION1"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

            DataTable dt = PhoenixAccountsLeaveAllotment.SearchLeaveAllotment(General.GetNullableInteger(empid), null, null, null, null, null
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"], gvLAR.PageSize
                                                       , ref iRowCount, ref iTotalPageCount);
            gvLAR.DataSource = dt;
            gvLAR.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT1"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidRankMapping(string description, string ranklist)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (description.Equals(""))
            ucError.ErrorMessage = "Description is required.";
        if (ranklist.Equals(""))
            ucError.ErrorMessage = "Group rank is required.";

        return (!ucError.IsError);
    }
    protected void gvLVP_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if(e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvLVP_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            ImageButton req = (ImageButton)e.Item.FindControl("cmdRequest");
            if (req != null)
            {
                req.Visible = SessionUtil.CanAccess(this.ViewState, req.CommandName);
                req.Attributes.Add("onclick", "openNewWindow('LAR', '', '" + Session["sitepath"] + "/Accounts/AccountsLeaveAllotment.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "');return false;");
            }
        }
    }

    protected void gvLVP_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLVP.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
