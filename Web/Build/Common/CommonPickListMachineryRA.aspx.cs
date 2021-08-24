using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class CommonPickListMachineryRA : PhoenixBasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
            MenuPortAgent.MenuList = toolbarmain.Show();    

            if (!IsPostBack)
            {
                gvRA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["type"] = "3";
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }
                ViewState["STATUS"] = Request.QueryString["status"];
                BindCategory();
            }
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPortAgent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvRA.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvRA.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;        
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        DataTable dt = PhoenixInspectionDailyWorkPlanActivity.DailyWorkRiskAssessmentMachineryList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                    ,General.GetNullableString(ViewState["STATUS"].ToString())
                                                    ,int.Parse(ViewState["PAGENUMBER"].ToString())
                                                    ,gvRA.PageSize
                                                    , ref iRowCount
                                                    ,ref iTotalPageCount
                                                    ,txtActivity.Text
                                                    ,General.GetNullableInteger(ddlCategory.SelectedValue)
                                                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        gvRA.DataSource = dt;
        gvRA.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
        gvRA.Rebind();
        if (rblType.SelectedValue == "1") //PROCESS
            ViewState["type"] = "5";
        else if (rblType.SelectedValue == "2") //GENERIC
            ViewState["type"] = "2";
        else if (rblType.SelectedValue == "3") //MACHINERY
            ViewState["type"] = "3";
        else if (rblType.SelectedValue == "4") //NAVIGATION
            ViewState["type"] = "1";

        BindCategory();
    }

    protected void BindCategory()
    {        
        DataSet ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(General.GetNullableInteger(Request.QueryString["catid"]), General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        ddlCategory.DataSource = ds;
        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDACTIVITYID";
        ddlCategory.DataBind();
        //ddlCategory.Items.Add(new ButtonListItem("--Select--", "Dummy"));
    }

    protected void gvRA_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string Script = "";
        NameValueCollection nvc;
        if (e.CommandName.ToUpper().Equals("PICKLIST"))
        {
            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                Label lblRefNo = (Label)e.Item.FindControl("lblRefNo");
                nvc.Add(lblRefNo.ID, lblRefNo.Text);
                if (rblType.SelectedValue == "1")
                {
                    Label lblRA = (Label)e.Item.FindControl("lblProcessName");
                    nvc.Add(lblRA.ID, lblRA.Text);
                }
                else
                {
                    Label lblRA = (Label)e.Item.FindControl("lblActivity");
                    nvc.Add(lblRA.ID, lblRA.Text);
                }
                Label lblRAId = (Label)e.Item.FindControl("lblRAId");
                nvc.Add(lblRAId.ID, lblRAId.Text);
                Label lblType = (Label)e.Item.FindControl("lblType");
                nvc.Add(lblType.ID, lblType.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                Label lblRefNo = (Label)e.Item.FindControl("lblRefNo");
                nvc.Set(nvc.GetKey(1), lblRefNo.Text);
                if (rblType.SelectedValue == "1")
                {
                    Label lblRA = (Label)e.Item.FindControl("lblProcessName");
                    nvc.Set(nvc.GetKey(2), lblRA.Text);
                }
                else
                {
                    Label lblRA = (Label)e.Item.FindControl("lblActivity");
                    nvc.Set(nvc.GetKey(2), lblRA.Text);
                }
                Label lblRAId = (Label)e.Item.FindControl("lblRAId");
                nvc.Set(nvc.GetKey(3), lblRAId.Text);
                Label lblType = (Label)e.Item.FindControl("lblType");
                nvc.Set(nvc.GetKey(4), lblType.Text);
            }
            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvRA_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvRA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRA.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRA_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridHeaderItem)
        {
            if (rblType.SelectedValue != "1")
            {
                //e.Item.Cells[4].Visible = false;
                (gvRA.MasterTableView.GetColumn("Process") as GridTemplateColumn).Visible = false;
            }
        }
        if(e.Item is GridDataItem)
        {
            Label lbtn = (Label)e.Item.FindControl("lblActivity");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipActivity");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            if (rblType.SelectedValue != "1")
            {
                //e.Item.Cells[4].Visible = false;
                (gvRA.MasterTableView.GetColumn("Process") as GridTemplateColumn).Visible = false;
            }
        }
    }
}
