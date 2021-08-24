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
public partial class CommonPickListVesselAccount : PhoenixBasePage
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


            ds = PhoenixRegistersAccount.VesselAccountSearch(txtAccountCodeSearch.Text.Trim(), txtAccountDescSearch.Text.Trim() 
                , sortexpression, sortdirection,
                gvAccount.CurrentPageIndex + 1,
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
        if (e.CommandName.ToUpper().Equals("PICKADDRESS"))
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lblAccountCode = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkAccountCode");
                nvc.Add(lblAccountCode.ID, lblAccountCode.Text.ToString());
                RadLabel lnkAccountDescription = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblAccountDescription");
                nvc.Add(lnkAccountDescription.ID, lnkAccountDescription.Text.ToString());
                RadLabel lblAccountId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblAccountId");
                nvc.Add(lblAccountId.ID, lblAccountId.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;
                LinkButton lblAccountCode = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkAccountCode");
                nvc.Set(nvc.GetKey(1), lblAccountCode.Text.ToString());
                RadLabel lnkAccountDescription = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblAccountDescription");
                nvc.Set(nvc.GetKey(2), lnkAccountDescription.Text.ToString());
                RadLabel lblAccountId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblAccountId");
                nvc.Set(nvc.GetKey(3), lblAccountId.Text.ToString());
            }
            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
        }
        
        if (e.CommandName.ToUpper().Equals("PAGE"))
        {
            gvAccount.Rebind();
        }
    }
    protected void gvAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAccount_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            //{
            //}
        }
    }
}
