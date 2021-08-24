using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsOfficeAccrual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOfficeAccrual.aspx?" + Request.QueryString.ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageButton("../Accounts/AccountsOfficeAccrual.aspx?", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsOfficeAccrual.aspx?", "Filter", "clear-filter.png", "CLEAR FILTER");
            MenuPBExcel.AccessRights = this.ViewState;
            MenuPBExcel.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = Request.QueryString["pno"] != null ? int.Parse(Request.QueryString["pno"]) : 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvPB.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvPB.SelectedIndexes.Clear();
        gvPB.EditIndexes.Clear();
        gvPB.DataSource = null;
        gvPB.Rebind();
    }
    protected void MenuPBExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

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

                string[] alColumns = { "FLDFILENO", "FLDRANKCODE", "FLDEMPLOYEENAME", "FLDFROMDATE", "FLDTODATE", "FLDSOURCE", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDDESCRIPTION", "FLDSUBACCOUNT", "FLDOWNERBUDGETGROUP", "FLDVOUCHERNUMBER", "FLDINCLUDEDINPBYN" };
                string[] alCaptions = { "File No", "Rank", "Name", "From Date", "To Date", "Source", "Component Name", "Amount", "Vessel Account Code", "Budget Code", "Owner Budget Code", "Voucher Number", "Included in PB Posting" };

                DataTable dt = PhoenixAccountsOfficeAccrual.OfficeAccrualSearch(null, txtFileNo.Text, txtComponentName.Text, txtVoucherNumber.Text
                           , sortexpression, sortdirection
                    , 1, iRowCount, ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("Office Accrual Wages", dt, alColumns, alCaptions, sortdirection, sortexpression);

            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR FILTER"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtFileNo.Text = "";
                txtComponentName.Text = "";
                txtVoucherNumber.Text = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDFILENO", "FLDRANKCODE", "FLDEMPLOYEENAME", "FLDFROMDATE", "FLDTODATE", "FLDSOURCE", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDDESCRIPTION", "FLDSUBACCOUNT", "FLDOWNERBUDGETGROUP", "FLDVOUCHERNUMBER", "FLDINCLUDEDINPBYN" };
            string[] alCaptions = { "File No", "Rank", "Name", "From Date", "To Date", "Source", "Component Name", "Amount", "Vessel Account Code", "ESM Budget Code", "Owner Budget Code", "Voucher Number", "Included in PB Posting" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAccountsOfficeAccrual.OfficeAccrualSearch(null, txtFileNo.Text, txtComponentName.Text, txtVoucherNumber.Text
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvPB.PageSize, ref iRowCount, ref iTotalPageCount);
            gvPB.DataSource = dt;
            gvPB.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPB_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            e.Item.Attributes.Add("title", "Period : " + General.GetDateTimeToString(drv["FLDFROMDATE"].ToString()) + " - " + General.GetDateTimeToString(drv["FLDTODATE"].ToString()));
        }
    }
    protected void gvPB_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        int nCurrentRow = int.Parse(e.CommandArgument.ToString());
        GridView _gridView = (GridView)sender;
        if (e.CommandName.ToUpper().Equals(""))
        {

        }
    }
    protected void gvPB_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ALLOTREQ"))
            {

                NameValueCollection nvc = new NameValueCollection();
                string empid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                nvc.Add("empid", empid);
                Filter.CurrentOfficeAccrualCrew = nvc;
                Response.Redirect("AccountsOfficeAccrualAllotmentRequest.aspx?pno=" + ViewState["PAGENUMBER"].ToString(), true);

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
    protected void gvPB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPB.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
