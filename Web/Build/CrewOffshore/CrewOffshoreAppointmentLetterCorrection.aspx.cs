using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI;

public partial class CrewOffshoreAppointmentLetterCorrection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Sign On/Off", "SIGNONOFFCONFIG");
            toolbarsub.AddButton("App. Letter", "APPLETTER");
            toolbarsub.AddButton("Signon Date", "SDC");
            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarsub.Show();
            CrewQuery.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreAppointmentLetterCorrection.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreAppointmentLetterCorrection.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            CrewQueryMenu.AccessRights = this.ViewState;
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
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            //BindData();
            //SetPageNavigator();
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

        if (CommandName.ToUpper().Equals("SIGNONOFFCONFIG"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreSignOnOffConfiguration.aspx", true);
        }
        if (CommandName.ToUpper().Equals("SDC"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreSignOnDateCorrection.aspx", true);
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
                string[] alColumns = { "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDCONTRACTCOMMENCEMENTDATE", "FLDSIGNONDATE", "FLDDAILYRATE", "FLDDPALLOWANCE" };
                string[] alCaptions = { "Vessel", "File No", "Name", "Rank", "Contract Commencement Date", "Sign-On Date", "Daily Rate (USD)", "DP Allowance (USD)" };

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

                DataTable dt = PhoenixCrewOffshoreSignOnOffConfiguration.SearchAppointmentLetter(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                       , sortexpression, sortdirection
                                                                       , 1, iRowCount
                                                                       , ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("Appointment Letter", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
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
        BindData();
        gvCrewSearch.Rebind();
      
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDCONTRACTCOMMENCEMENTDATE", "FLDSIGNONDATE", "FLDDAILYRATE", "FLDDPALLOWANCE" };
        string[] alCaptions = { "Vessel", "File No", "Name", "Rank", "Contract Commencement Date", "Sign-On Date", "Daily Rate (USD)", "DP Allowance (USD)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = PhoenixCrewOffshoreSignOnOffConfiguration.SearchAppointmentLetter(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                       , sortexpression, sortdirection
                                                                       , gvCrewSearch.CurrentPageIndex+1, gvCrewSearch.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewSearch", "Appointment Letter", alCaptions, alColumns, ds);

            gvCrewSearch.DataSource = dt;
            gvCrewSearch.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
  
    protected void gvCrewSearch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }

    protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;
        BindData();
    }



    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        
    }

    private bool IsValidInput(string dailyrate, string dpallowance, string contractstartdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(dailyrate) == null)
        {
            ucError.ErrorMessage = "Daily Rate is required.";
        }
        if (General.GetNullableDateTime(contractstartdate) == null)
        {
            ucError.ErrorMessage = "Contract Commencement Date is required.";
        }        
        return (!ucError.IsError);
    }



    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCrewSearch_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        
        try
        {
            string dailyrate = ((UserControlNumber)e.Item.FindControl("ucDailyRate")).Text;
            string dpallowance = ((UserControlNumber)e.Item.FindControl("ucDPAllowance")).Text;
            string appointmentletterid = ((RadLabel)e.Item.FindControl("lblAppLetterID")).Text;
            string contractstartdate = ((UserControlDate)e.Item.FindControl("ucContractStartDate")).Text;
            if (!IsValidInput(dailyrate, dpallowance, contractstartdate))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewOffshoreSignOnOffConfiguration.AppointmentLetterWageUpdate(new Guid(appointmentletterid), int.Parse(dailyrate), General.GetNullableInteger(dpallowance)
                                                                                    , General.GetNullableDateTime(contractstartdate));
          
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            LinkButton lnkEmployeeName = (LinkButton)e.Item.FindControl("lnkEmployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");

            if (DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE").ToString() != null && General.GetNullableString(DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE").ToString()) != null)
                lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + DataBinder.Eval(e.Item.DataItem, "FLDCREWPLANID").ToString() + "'); return false;");
            else
                lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + DataBinder.Eval(e.Item.DataItem, "FLDCREWPLANID").ToString() + "'); return false;");

            if (lnkEmployeeName != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lnkEmployeeName.CommandName)) lnkEmployeeName.Visible = false;
            }
            LinkButton cmdAppointmentLetter = (LinkButton)e.Item.FindControl("cmdAppointmentLetter");
            if (cmdAppointmentLetter != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    cmdAppointmentLetter.Visible = false;

                cmdAppointmentLetter.Attributes.Add("onclick", "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=APPOINMENTLETTERPDF&showmenu=0&showword=0&showexcel=0"
                    + "&crewplanid=" + DataBinder.Eval(e.Item.DataItem, "FLDCREWPLANID").ToString()
                    + "&appointmentletterid=" + DataBinder.Eval(e.Item.DataItem, "FLDAPPOINTMENTLETTERID").ToString()
                    + "&popup=1" + "');return false;");
            }
        }
    }
}
