using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewOffshorePendingWaivingRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Crew List", "CREWLIST");
            toolbarsub.AddButton("Compliance Check", "CHECK");
            toolbarsub.AddButton("Crew List Format", "CREWLISTFORMAT");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbarsub.AddButton("Vessel Manning Scale", "MANNINGSCALE");
                toolbarsub.AddButton("Manning", "MANNING");
                toolbarsub.AddButton("Budget", "BUDGET");
            }
            //CrewQuery.AccessRights = this.ViewState;
            //CrewQuery.MenuList = toolbarsub.Show();
            //CrewQuery.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePendingWaivingRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePendingWaivingFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePendingWaivingRequest.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
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
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDVESSELNAME", "FLDRANKCODE", "FLDCOUNT" };
                string[] alCaptions = { "File No", "Name", "Planned Vessel", "Planned Rank", "No. of Waiver Requested" };

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
                NameValueCollection nvc = Filter.TrainingNeedsSearch;
                DataTable dt = PhoenixCrewOffshoreWaivingRequest.SearchWaivingRequestList(nvc != null ? nvc["txtName"] : ""
                                                                                         , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : "")
                                                                                         , General.GetNullableInteger(nvc != null ? nvc["ucVessel"] : "")
                                                                                         , General.GetNullableDateTime(nvc != null ? nvc["ucCompletedFrom"] : "")
                                                                                         , General.GetNullableDateTime(nvc != null ? nvc["ucCompletedTo"] : "")
                                                                                         , nvc != null ? nvc["txtFileNo"] : ""
                                                                                         , General.GetNullableInteger(nvc != null ? nvc["Status"] : "")
                                                                                         , sortexpression, sortdirection
                                                                                         , 1, iRowCount
                                                                                         , ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("Pending Waiving Request", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvCrewSearch.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("txtName", null);
                nvc.Add("txtFileNo", null);
                nvc.Add("ddlRank", null);
                nvc.Add("ucVessel", null);
                Filter.TrainingNeedsSearch = nvc;
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

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDVESSELNAME", "FLDRANKCODE", "FLDCOUNT" };
        string[] alCaptions = { "File No", "Name", "Planned Vessel", "Planned Rank", "No. of Waiver Requested" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            NameValueCollection nvc = Filter.TrainingNeedsSearch;


            DataTable dt = PhoenixCrewOffshoreWaivingRequest.SearchWaivingRequestList(nvc != null ? nvc["txtName"] : ""
                                                                                         , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : "")
                                                                                         , General.GetNullableInteger(nvc != null ? nvc["ucVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                                         , General.GetNullableDateTime(nvc != null ? nvc["ucCompletedFrom"] : "")
                                                                                         , General.GetNullableDateTime(nvc != null ? nvc["ucCompletedTo"] : "")
                                                                                         , nvc != null ? nvc["txtFileNo"] : ""
                                                                                         , General.GetNullableInteger(nvc != null ? nvc["Status"] : "2")
                                                                                         , sortexpression, sortdirection
                                                                                         , (int)ViewState["PAGENUMBER"], gvCrewSearch.PageSize
                                                                                         , ref iRowCount, ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewSearch", "Pending Waiving Request", alCaptions, alColumns, ds);
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

    protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = sender as GridView;
        Filter.CurrentVesselCrewSelection = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblEmployeeid")).Text;
        Response.Redirect("..\\VesselAccounts\\VesselAccountsEmployeeGeneral.aspx", false);
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvCrewSearch.Rebind();
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

    protected void gvCrewSearch_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("DETAIL"))
            {
                string querystring = "empid=" + ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;

                querystring += "&rankid=" + ((RadLabel)e.Item.FindControl("lblrankid")).Text;
                querystring += "&vesselid=" + ((RadLabel)e.Item.FindControl("lblvesselid")).Text;
                querystring += "&trainingmatrixid=" + ((RadLabel)e.Item.FindControl("lblmatrixid")).Text;
                querystring += "&expectedjoiningdate=" + ((RadLabel)e.Item.FindControl("lbljoiningdate")).Text;
                querystring += "&suitability=1";
                querystring += "&personalmaster=1";
                querystring += "&calledfrom=1";
                querystring += "&crewplanid=" + ((RadLabel)e.Item.FindControl("lblcrewplanid")).Text;


                Response.Redirect("CrewOffshorePendingWaivingDetail.aspx?" + querystring, true);
            }
            else if (e.CommandName == "Page")
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

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        NameValueCollection nvc = Filter.TrainingNeedsSearch;


        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkEmployeeName = (LinkButton)e.Item.FindControl("lnkEmployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&WaiveYN=" + nvc["Status"] + "&launchedfrom=offshore'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&WaiveYN=" + nvc["Status"] + "&launchedfrom=offshore'); return false;");
            }

            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
            }

           
        }
    }
}
