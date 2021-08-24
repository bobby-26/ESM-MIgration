using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CommonPickListAccount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuAccount.MenuList = toolbarmain.Show();
        //MenuAccount.SetTrigger(pnlAccount);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            if (Session["sAccountCode"] != null && Session["sAccountCode"].ToString() != string.Empty)
                ViewState["ACCOUNTCODE"] = Session["sAccountCode"];
            if (Session["sAccountCodeDescription"] != null && Session["sAccountCodeDescription"].ToString() != string.Empty)
                ViewState["ACCOUNTDESCRIPTION"] = Session["sAccountCodeDescription"].ToString();
            if (Request.QueryString["companyid"] != null && Request.QueryString["companyid"] != string.Empty)
                ViewState["companyid"] = Request.QueryString["companyid"];

            gvAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void MenuAccount_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvAccount.SelectedIndexes.Clear();
                gvAccount.EditIndexes.Clear();
                gvAccount.DataSource = null;
                gvAccount.Rebind();
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

            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (txtAccountCodeSearch.Text != string.Empty)
                ViewState["ACCOUNTCODE"] = txtAccountCodeSearch.Text;
            if (txtDescriptionSearch.Text != string.Empty)
                ViewState["ACCOUNTDESCRIPTION"] = txtDescriptionSearch.Text;

            ds = PhoenixRegistersAccount.AccountSearch(ViewState["companyid"] != null ? int.Parse(ViewState["companyid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                , ViewState["ACCOUNTCODE"] != null ? General.GetNullableString(ViewState["ACCOUNTCODE"].ToString()) : null
                , ViewState["ACCOUNTDESCRIPTION"] != null ? General.GetNullableString(ViewState["ACCOUNTDESCRIPTION"].ToString()) : null
                , null
                , null
                , null
                , 1
                , sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvAccount.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvAccount.DataSource = ds;
            gvAccount.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lnkAccCode = (LinkButton)e.Item.FindControl("lnkAccountCode");
                nvc.Add(lblAccountCode.ID, lnkAccCode.Text.ToString());
                RadLabel lnkAccountDescription = (RadLabel)e.Item.FindControl("lblAccountDescription");
                nvc.Add(lnkAccountDescription.ID, lnkAccountDescription.Text.ToString());
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                nvc.Add(lblAccountId.ID, lblAccountId.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframename"] != null && Request.QueryString["iframename"] == "true")
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                else
                    Script += "fnClosePickList('codehelp1');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                LinkButton lnkAccCode = (LinkButton)e.Item.FindControl("lnkAccountCode");
                nvc.Set(nvc.GetKey(1), lnkAccCode.Text.ToString());
                RadLabel lnkAccountDescription = (RadLabel)e.Item.FindControl("lblAccountDescription");
                nvc.Set(nvc.GetKey(2), lnkAccountDescription.Text.ToString());
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                nvc.Set(nvc.GetKey(3), lblAccountId.Text.ToString());
                RadLabel lblAccountSource = (RadLabel)e.Item.FindControl("lblAccountSource");
                nvc.Set(nvc.GetKey(4), lblAccountSource.Text.ToString());
                RadLabel lblAccountUsage = (RadLabel)e.Item.FindControl("lblAccountUsage");
                nvc.Set(nvc.GetKey(5), lblAccountUsage.Text.ToString());
                nvc.Set(nvc.GetKey(6), string.Empty);
                nvc.Set(nvc.GetKey(7), string.Empty);
                nvc.Set(nvc.GetKey(8), string.Empty);
                nvc.Set(nvc.GetKey(9), string.Empty);
                Session["SelectedAccountId"] = lblAccountId.Text.ToString();
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void gvAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {

    }
    protected void gvAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAccount.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
