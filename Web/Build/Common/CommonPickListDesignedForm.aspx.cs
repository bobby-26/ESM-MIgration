using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Common_CommonPickListDesignedForm : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH");
        //MenuCategory.MenuList = toolbarmain.Show();
        //MenuCategory.SetTrigger(pnlCategory);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            BindParentCategory();
        }
    }

    protected void MenuCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                gvCategory.CurrentPageIndex = 0;
                gvCategory.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindParentCategory()
    {

        ddlParentCategory.Items.Clear();
        ddlParentCategory.DataSource = PhoenixCommonDocumentManagement.ListTreeDesignFormDocumentParentCategory();
        ddlParentCategory.DataBind();
        ddlParentCategory.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
        ddlParentCategory.Visible = true;
    }

    protected void gvCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
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

            int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            ds = PhoenixCommonDocumentManagement.DocumentChildDesignedFormCategorySearch(General.GetNullableInteger(ddlParentCategory.SelectedValue.ToString()),
                txtNameSearch.Text,
                sortexpression, sortdirection,
                gvCategory.CurrentPageIndex + 1,
                gvCategory.PageSize,
                ref iRowCount,
                ref iTotalPageCount
                );


            gvCategory.DataSource = ds;
            gvCategory.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCategory_OnItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;

        if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
            return;

        int nCurrentRow = e.Item.ItemIndex;

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();
                LinkButton lbFormName = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkFormName");
                nvc.Add(lbFormName.ID, lbFormName.Text.ToString());
                RadLabel lblFormId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblFormId");
                nvc.Add(lblFormId.ID, lblFormId.Text);

            }
            else
            {

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;
                LinkButton lnkFormName = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkFormName");
                nvc.Set(nvc.GetKey(1), lnkFormName.Text.ToString());
                RadLabel lblFormId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblFormId");
                nvc.Set(nvc.GetKey(2), lblFormId.Text.ToString());
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
        }
    }

    protected void gvCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }

    protected void gvCategory_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void ddlParentCategory_Changed(object sender, EventArgs e)
    {
        gvCategory.CurrentPageIndex = 0;
        gvCategory.Rebind();
    }
}