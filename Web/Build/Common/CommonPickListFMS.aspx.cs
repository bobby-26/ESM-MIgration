using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DocumentManagement;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.StandardForm;
using System.Web;
using Telerik.Web.UI;

public partial class Common_CommonPickListFMS : System.Web.UI.Page
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
            //BindParentCategory();
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

    //protected void BindParentCategory()
    //{

    //    ddlParentCategory.Items.Clear();
    //    ddlParentCategory.DataSource = PhoenixCommonDocumentManagement.ListTreeDesignFormDocumentParentCategory();
    //    ddlParentCategory.DataBind();
    //    ddlParentCategory.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
    //    ddlParentCategory.Visible = true;
    //}

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

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = PhoenixFormBuilder.DmsReportSearch(null, txtFormName.Text, sortexpression
                                                        , sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()), gvCategory.PageSize
                                                        , ref iRowCount
                                                        , ref iTotalPageCount, txtcontent.Text, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

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
                LinkButton lnkReportName = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkReportName");
                nvc.Add(lnkReportName.ID, lnkReportName.Text.ToString());
                RadLabel lblReportId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblReportId");
                nvc.Add(lblReportId.ID, lblReportId.Text);
                RadLabel lblFormId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblFormId");
                nvc.Add(lblFormId.ID, lblFormId.Text);

            }
            else
            {

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;
                LinkButton lnkReportName = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkReportName");
                nvc.Set(nvc.GetKey(1), lnkReportName.Text.ToString());
                RadLabel lblReportId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblReportId");
                nvc.Set(nvc.GetKey(2), lblReportId.Text.ToString());
                RadLabel lblFormId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblFormId");
                nvc.Set(nvc.GetKey(3), lblFormId.Text.ToString());
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