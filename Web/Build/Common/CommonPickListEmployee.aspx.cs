using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CommonPickListEmployee : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Search", "SEARCH",ToolBarDirection.Right);
            CrewList.MenuList = toolbar.Show();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ACTIVEYN"] = "";

            gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (Request.QueryString["activeyn"] != null && General.GetNullableInteger(Request.QueryString["activeyn"].ToString()) != null)
                ViewState["ACTIVEYN"] = Request.QueryString["activeyn"].ToString();
        }    
    }
    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvCrewList.Rebind();
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

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = new DataTable();
            if (rblCrewFrom.SelectedValue == "0")
            {
                dt = PhoenixCommonCrew.QueryActivity(null
                                                    , null
                                                   , null
                                                   , txtFileNo.Text
                                                   , ddlRank.SelectedRank
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , null
                                                   , txtName.Text
                                                   , null
                                                   , null
                                                   , General.GetNullableByte(ViewState["ACTIVEYN"].ToString())
                                                   , null
                                                   , null
                                                   , null
                                                   , sortexpression, sortdirection
                                                   , (int)ViewState["PAGENUMBER"], gvCrewList.PageSize
                                                   , ref iRowCount, ref iTotalPageCount
                                                   , null, null, null);
            }
            else
            {
                dt = PhoenixNewApplicantManagement.NewApplicantQueryActivity(null
                                                                   , null
                                                                   , null
                                                                   , txtFileNo.Text
                                                                   , ddlRank.SelectedRank
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , txtName.Text
                                                                   , null
                                                                   , null
                                                                   , General.GetNullableByte(ViewState["ACTIVEYN"].ToString())
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , null
                                                                   , sortexpression, sortdirection
                                                                   , (int)ViewState["PAGENUMBER"], gvCrewList.PageSize
                                                                   , ref iRowCount, ref iTotalPageCount
                                                                   , null);

            }

            gvCrewList.DataSource = dt;
            gvCrewList.VirtualItemCount = iRowCount;
            
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvCrewList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewList.CurrentPageIndex + 1;
        BindData();
    }
    
    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string Script = "";
                NameValueCollection nvc = Filter.CurrentPickListSelection;

                RadLabel id = (RadLabel)e.Item.FindControl("lblemployeeid");
                
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkEployeeName");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                nvc.Set(nvc.GetKey(2), id.Text.ToString());

                Filter.CurrentPickListSelection = nvc;           
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }

            if (e.CommandName == "Page")
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


    protected void gvCrewList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblRank = (RadLabel)e.Item.FindControl("lblAppliedRank");
            RadLabel lblFileNo = (RadLabel)e.Item.FindControl("lblFileNo");

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (lblRank != null)
            {
                if (rblCrewFrom.SelectedValue == "0")
                {
                    lblRank.Text = drv["FLDRANKPOSTEDNAME"].ToString();
                }
                if (rblCrewFrom.SelectedValue == "1")
                {
                    lblRank.Text = drv["FLDRANKNAME"].ToString();
                }
            }
            if (lblFileNo != null)
            {
                if (rblCrewFrom.SelectedValue == "0")
                {
                    lblFileNo.Text = drv["FLDEMPLOYEECODE"].ToString();
                }
                if (rblCrewFrom.SelectedValue == "1")
                {
                    lblFileNo.Text = drv["FLDFILENO"].ToString();
                }
            }

        }
    }

    protected void gvCrewList_SortCommand(object sender, GridSortCommandEventArgs e)
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


    protected void rblCrewFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
        gvCrewList.Rebind();
    }
}
