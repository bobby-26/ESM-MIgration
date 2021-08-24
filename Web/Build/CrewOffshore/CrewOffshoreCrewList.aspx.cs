using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewOffshoreCrewList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Crew List", "CREWLIST");
            toolbarsub.AddButton("Compliance", "CHECK");
            toolbarsub.AddButton("Crew Format", "CREWLISTFORMAT");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbarsub.AddButton("Vessel Scale", "MANNINGSCALE");
                toolbarsub.AddButton("Manning", "MANNING");
                toolbarsub.AddButton("Budget", "BUDGET");
            }
            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarsub.Show();
            CrewQuery.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreCrewList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreCrewList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            CrewQueryMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["VESSELID"] = "";

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }
                else
                {
                    if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                        UcVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                }
                txtDate.Text = General.GetDateTimeToString(System.DateTime.Now);
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
          
           
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("CHECK"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCrewComplianceCheck.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("CREWLISTFORMAT"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreReportCrewListFormat.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("MANNINGSCALE"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreVesselManningScale.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("MANNING"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreManningScale.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("BUDGET"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreBudget.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }

    }

    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDDATEOFBIRTH", "FLDNATIONALITYNAME", "FLDPASSPORTNO", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
                string[] alCaptions = { "Sl.No", "File No", "Name", "Rank", "DOB", "Nationality", "Passport No", "Sign On Date", "End of contract" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataTable dt = PhoenixCrewOffshoreCrewList.SearchVesselCrewList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                                       , General.GetNullableDateTime(txtDate.Text)
                                                                                       , sortexpression, sortdirection
                                                                                       , 1, iRowCount
                                                                                       , ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("Crew List", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvCrewSearch.Rebind();
               
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        gvCrewArticelSearch.Rebind();
        gvCrewSearch.Rebind();
        //BindArticleDate();
        //BindData();
        //SetPageNavigator();
    }

    public void BindArticleDate()
    {
        if (ViewState["VESSELID"].ToString() != "" && ViewState["VESSELID"].ToString() != null)
        {
            DataTable dt = PhoenixCrewOffshoreArticles.ArtcelsUpdatedSearch(General.GetNullableInteger(ViewState["VESSELID"].ToString()));

            gvCrewArticelSearch.DataSource = dt;

        }
    }
    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDDATEOFBIRTH", "FLDNATIONALITYNAME", "FLDPASSPORTNO", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
        string[] alCaptions = { "Sl.No", "File No", "Name", "Rank", "DOB", "Nationality", "Passport No", "Sign On Date", "End of contract" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            DataTable dt = PhoenixCrewOffshoreCrewList.SearchVesselCrewList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                       , General.GetNullableDateTime(txtDate.Text)
                                                                       , sortexpression, sortdirection
                                                                       , (int)ViewState["PAGENUMBER"], gvCrewSearch.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewSearch", "Crew List", alCaptions, alColumns, ds);

            gvCrewSearch.DataSource = dt;
            gvCrewSearch.VirtualItemCount = iRowCount;
          
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                gvCrewSearch.Columns[10].Visible = false;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewSearch_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
   
    //protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView _gridView = sender as GridView;
    //    Filter.CurrentVesselCrewSelection = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblEmployeeid")).Text;
    //    Response.Redirect("..\\VesselAccounts\\VesselAccountsEmployeeGeneral.aspx", false);
    //}
  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
      
        BindData();
       
    }

    protected void gvCrewArticelSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindArticleDate();
    }

    protected void gvCrewSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkEmployeeName = (LinkButton)e.Item.FindControl("lnkEmployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
            }

            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
            }

            RadLabel lblName = (RadLabel)e.Item.FindControl("lblName");
            if (drv["FLDVIEWPARTICULARSYN"] != null && drv["FLDVIEWPARTICULARSYN"].ToString().Equals("1"))
            {
                lblName.Visible = false;
                lnkEmployeeName.Visible = true;
            }
            else
            {
                lblName.Visible = true;
                lnkEmployeeName.Visible = false;
            }

            if (lnkEmployeeName != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lnkEmployeeName.CommandName)) lnkEmployeeName.Visible = false;
            }
            LinkButton cmdAppointmentLetter = (LinkButton)e.Item.FindControl("cmdAppointmentLetter");
            if (cmdAppointmentLetter != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    cmdAppointmentLetter.Visible = false;

                cmdAppointmentLetter.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=APPOINMENTLETTERPDF&showmenu=0&showword=0&showexcel=0"
                    + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                    + "&appointmentletterid=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                    + "&popup=1" + "');return false;");
            }

            ImageButton cmdrenewval = (ImageButton)e.Item.FindControl("cmdrenewval");
            if (cmdrenewval != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    cmdrenewval.Visible = false;

                cmdrenewval.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreContractRenewal.aspx?employeeid="+ lblEmployeeid.Text 
                    + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                    + "&appointmentletterid=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                    + "&popup=1" + "');return false;");
            }
        }
    }

    protected void gvCrewSearch_SortCommand(object sender, GridSortCommandEventArgs e)
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
