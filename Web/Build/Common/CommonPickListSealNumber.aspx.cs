using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class CommonPickListSealNumber : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        Menuseal.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["SEALSTATUS"] = null;
            if (Request.QueryString["status"] != null && !string.IsNullOrEmpty(Request.QueryString["status"].ToString()))
                ViewState["SEALSTATUS"] = Request.QueryString["status"].ToString();

            gvSeal.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void Menuseal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvSeal.Rebind();
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
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonInspection.SealSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                            , General.GetNullableInteger(ucSealType.SelectedQuick)
                            , ViewState["SEALSTATUS"] == null ? null : General.GetNullableInteger(ViewState["SEALSTATUS"].ToString())
                            , General.GetNullableString(txtSealNumber.Text)
                            , sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            gvSeal.PageSize,
                            ref iRowCount,
                            ref iTotalPageCount);

        gvSeal.DataSource = ds;
        gvSeal.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }




   
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();

    }

    protected void gvSeal_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSeal.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSeal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
       
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName.ToUpper() == "SELECT")
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;

            string Script = "";
            NameValueCollection nvc;
            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lb = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkSealNumber");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lbl = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblSealid");
                nvc.Add(lbl.ID, lbl.Text);
            }
            else
            {
                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)_gridView.Items[nCurrentRow].FindControl("lnkSealNumber");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lbl = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblSealid");
                nvc.Set(nvc.GetKey(2), lbl.Text);


                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            Filter.CurrentPickListSelection = nvc;
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }


    protected void gvSeal_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
