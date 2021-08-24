using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Accounts_AccountsInternalMonthlyBillingDeletedEmployeeList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            MenuBudgetTab.Title = "Crew List";
            MenuBudgetTab.AccessRights = this.ViewState;
            MenuBudgetTab.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                if (Request.QueryString["PORTAGEBILLID"] != null && Request.QueryString["PORTAGEBILLID"].ToString() != "")
                    ViewState["PORTAGEBILLID"] = Request.QueryString["PORTAGEBILLID"].ToString();
                else
                    ViewState["PORTAGEBILLID"] = "";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvMonthlyBillingCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSERIALNUMBER", "FLDRANKCODE", "FLDEMPLOYEENAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Serial Number", "Rank", "Name", "Sign On Date", "Sign Off Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAccountsInternalBilling.DeletedEmployeeSearch(
                                                            General.GetNullableGuid(ViewState["PORTAGEBILLID"].ToString())
                                                           , sortexpression
                                                           , sortdirection
                                                           , (int)ViewState["PAGENUMBER"]
                                                           , gvMonthlyBillingCrew.PageSize
                                                           , ref iRowCount
                                                           , ref iTotalPageCount);

        General.SetPrintOptions("gvMonthlyBillingCrew", "Crew List", alCaptions, alColumns, ds);

        gvMonthlyBillingCrew.DataSource = ds;
        gvMonthlyBillingCrew.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["MONTHLYBILLINGEMPLOYEEID"] == null)
            {
                ViewState["MONTHLYBILLINGEMPLOYEEID"] = ds.Tables[0].Rows[0]["FLDMONTHLYBILLINGEMPLOYEEID"].ToString();
                ViewState["POSTEDYN"] = ds.Tables[0].Rows[0]["FLDPOSTEDYN"].ToString();
                //if (ViewState["POSTEDYN"] != null && ViewState["POSTEDYN"].ToString() == "1")
                //  gvMonthlyBillingCrew.Columns[5].Visible = false;
            }
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvMonthlyBillingCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMonthlyBillingCrew.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMonthlyBillingCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string Script = "";
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                int nCurrentRow = e.Item.ItemIndex;
                BindCurentCrewValue(nCurrentRow);
                PhoenixAccountsInternalBilling.EmployeeAdd(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["PORTAGEBILLID"].ToString())
                                                                , new Guid(ViewState["MONTHLYBILLINGEMPLOYEEID"].ToString()));
                ViewState["MONTHLYBILLINGEMPLOYEEID"] = null;
                Rebind();

                /*ucStatus.Text = "Crew List Saved Successfully";*/


                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo','');";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindCurentCrewValue(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvMonthlyBillingCrew.Items[rowindex];
            RadLabel lblMonthlyBillingEmployeeId = ((RadLabel)gvMonthlyBillingCrew.Items[rowindex].FindControl("lblMonthlyBillingEmployeeId"));
            RadLabel lblName = ((RadLabel)gvMonthlyBillingCrew.Items[rowindex].FindControl("lblName"));
            RadLabel lblPostedYN = ((RadLabel)gvMonthlyBillingCrew.Items[rowindex].FindControl("lblPostedYN"));

            if (lblMonthlyBillingEmployeeId != null)
                ViewState["MONTHLYBILLINGEMPLOYEEID"] = lblMonthlyBillingEmployeeId.Text;
            else
                ViewState["MONTHLYBILLINGEMPLOYEEID"] = "";
            if (lblName != null)
                ViewState["EMPLOYEENAME"] = lblName.Text;
            if (lblPostedYN != null)
                ViewState["POSTEDYN"] = lblPostedYN.Text;
            if (ViewState["POSTEDYN"] != null && ViewState["POSTEDYN"].ToString() == "1")
                gvMonthlyBillingCrew.Columns[5].Visible = false;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvMonthlyBillingCrew.SelectedIndexes.Clear();
        gvMonthlyBillingCrew.EditIndexes.Clear();
        gvMonthlyBillingCrew.DataSource = null;
        gvMonthlyBillingCrew.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();

    }
}
