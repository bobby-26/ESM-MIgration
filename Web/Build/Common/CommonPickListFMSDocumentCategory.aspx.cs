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

public partial class CommonPickListFMSDocumentCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != "")
                ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();

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
                ViewState["PAGENUMBER"] = 1;
                BindData();
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
        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        DataSet ds = PhoenixCommonDocumentManagement.FMSShipboardFormCategory(companyid, int.Parse(ViewState["CATEGORYID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblParentCategory.Text = dr["FLDCATEGORYID"].ToString();
            txtParentCategory.Text = dr["FLDCATEGORYNAME"].ToString();
        }

        //ddlParentCategory.Items.Clear();
        //ddlParentCategory.DataSource = PhoenixCommonDocumentManagement.ListTreeDocumentParentCategory(companyid);
        //ddlParentCategory.DataBind();
        //ddlParentCategory.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
        //ddlParentCategory.Visible = true;
    }

    protected void gvCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCategory.CurrentPageIndex + 1;
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

            ds = PhoenixCommonDocumentManagement.DocumentChildCategorySearch(General.GetNullableGuid(lblParentCategory.Text),
                txtNameSearch.Text,
                sortexpression, sortdirection,
                int.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCategory.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                companyid
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

        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;

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
                LinkButton lbCategoryName = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkCategoryName");
                nvc.Add(lbCategoryName.ID, lbCategoryName.Text.ToString());
                RadLabel lblCategoryId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblCategoryId");
                nvc.Add(lblCategoryId.ID, lblCategoryId.Text);
            }
            else
            {

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;
                LinkButton lbCategoryName = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkCategoryName");
                nvc.Set(nvc.GetKey(1), lbCategoryName.Text.ToString());
                RadLabel lblCategoryId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblCategoryId");
                nvc.Set(nvc.GetKey(2), lblCategoryId.Text.ToString());
            }
            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

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
