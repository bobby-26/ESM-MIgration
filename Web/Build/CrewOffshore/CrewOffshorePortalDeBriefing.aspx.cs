using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewOffshorePortalDeBriefing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePortalDeBriefing.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Appraisal", "APPRAISAL");
            toolbarmain.AddButton("De-Briefing", "DEBRIEFING");
            CrewMain.AccessRights = this.ViewState;
            CrewMain.MenuList = toolbarmain.Show();
            CrewMain.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                DataTable dt = PhoenixCrewManagement.PortalEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                if (dt.Rows.Count > 0)
                {
                    Filter.CurrentCrewSelection = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                }

                gvSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("APPRAISAL"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshorePortalPendingAppraisal.aspx?empid=" + Filter.CurrentCrewSelection, false);
        }
        if (CommandName.ToUpper().Equals("DEBRIEFING"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshorePortalDeBriefing.aspx?empid=" + Filter.CurrentCrewSelection, false);
        }

    }


    protected void PD_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.DebriefingsummarySearch = null;
                BindData();
                gvSearch.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {

        string[] alColumns = {  "FLDVESSELNAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDSENTDATE", "FLDSTATUSNAME" };
        string[] alCaptions = {  "Vessel", "Rank", "From", "To", "Received on", "Status" };


        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        DataSet ds = PhoenixCrewDeBriefing.PortalDeBriefingSummarySearch(General.GetNullableInteger(Filter.CurrentCrewSelection)
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , General.ShowRecords(null)
                                                       , ref iRowCount
                                                       , ref iTotalPageCount);


        General.ShowExcel("Appraisal", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvSearch.Rebind();
    }

    public void BindData()
    {

        try
        {

            string[] alColumns = { "FLDVESSELNAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDSENTDATE", "FLDSTATUSNAME" };
            string[] alCaptions = { "Vessel", "Rank", "From", "To", "Received on", "Status" };

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.DebriefingsummarySearch;

            DataSet ds = PhoenixCrewDeBriefing.PortalDeBriefingSummarySearch(General.GetNullableInteger(Filter.CurrentCrewSelection)            
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , gvSearch.PageSize
                                                       , ref iRowCount
                                                       , ref iTotalPageCount);
            
            General.SetPrintOptions("gvSearch", "De-Briefing Summary/Analysis", alCaptions, alColumns, ds);
            gvSearch.DataSource = ds;
            gvSearch.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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


    protected void gvSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("GETEMPLOYEE"))
            {
                RadLabel lblSignonoffId = (RadLabel)e.Item.FindControl("lblSignonoffId");             
                Response.Redirect("../Options/OptionsOffshoreCrewDeBriefing.aspx?signonoffid=" + lblSignonoffId.Text, false);
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

    protected void gvSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
          
        }
    }
}
