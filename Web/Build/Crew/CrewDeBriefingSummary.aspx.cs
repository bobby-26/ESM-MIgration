using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewDeBriefingSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../Crew/CrewDeBriefingSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','"+Session["sitepath"]+"/Crew/CrewDeBriefingSummaryFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewDeBriefingSummary.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                NameValueCollection summary = new NameValueCollection();
                summary.Clear();
                summary.Add("ucstatus", "1097");
                //ucstatus.SelectedHard = "1097";
                Filter.DebriefingsummarySearch = summary;

                gvSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                ViewState["PAGENUMBER"] = 1;
                gvSearch.CurrentPageIndex = 0;
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSENTDATE", "FLDDEBRIEFINGDATE", "FLDVESSELNAME", "FLDEMPLOYEECODE", "FLDNAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDSTATUSNAME", "FLDGENERALCOMMENTS" };
            string[] alCaptions = { "Sent on", "Received on", "Vessel", "File No", "Name", "Rank", "From", "To", "Status", "General Comments" };
        //  string sortexpression;
            int? sortdirection = null;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        
    
       if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


            NameValueCollection nvc = Filter.DebriefingsummarySearch;
    
        DataTable dt = PhoenixCrewDeBriefing.DeBriefingSummary(General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                       , General.GetNullableDateTime(nvc != null ? nvc["txtFrom"] : string.Empty)
                                                       , General.GetNullableDateTime(nvc != null ? nvc["txtTo"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ucstatus"] : string.Empty)
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , iRowCount
                                                       , ref iRowCount
                                                       , ref iTotalPageCount);
    
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
    
        Response.AddHeader("Content-Disposition", "attachment; filename=De-BriefingSummary/Analysis.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>De-Briefing Summary/Analysis</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

            //General.ShowExcel("De-Briefing Summary/Analysis", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
        gvSearch.Rebind();

    }

    public void BindData()
    {

        try
        {

            string[] alColumns = { "FLDSENTDATE", "FLDDEBRIEFINGDATE", "FLDVESSELNAME", "FLDEMPLOYEECODE", "FLDNAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDSTATUSNAME", "FLDGENERALCOMMENTS" };
            string[] alCaptions = { "Sent on", "Received on", "Vessel", "File No", "Name", "Rank", "From", "To", "Status", "General Comments" };


            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.DebriefingsummarySearch;

            DataTable dt = PhoenixCrewDeBriefing.DeBriefingSummary(General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                       , General.GetNullableDateTime(nvc != null ? nvc["txtFrom"] : string.Empty)
                                                       , General.GetNullableDateTime(nvc != null ? nvc["txtTo"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ucstatus"] : string.Empty)
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , gvSearch.PageSize
                                                       , ref iRowCount
                                                       , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvSearch", "De-Briefing Summary/Analysis", alCaptions, alColumns, ds);

            gvSearch.DataSource = dt;
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


    private bool IsValidateRemarks(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(remarks))
            ucError.ErrorMessage = "Mumbai Remarks is required.";
        return (!ucError.IsError);
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


            if (e.CommandName.ToUpper().Equals("EDITROW"))
            {
                RadLabel lblSignonoffId = (RadLabel)e.Item.FindControl("lblSignonoffId");
                Response.Redirect("../Crew/CrewDeBriefingSummaryReview.aspx?signonoffid=" + lblSignonoffId.Text, false);
            }
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                RadLabel lblSignonoffId = (RadLabel)e.Item.FindControl("lblSignonoffId");
                RadLabel lblEmployeeId = (RadLabel)e.Item.FindControl("lblEmployeeId");
                PhoenixCrewDeBriefing.updatereview(General.GetNullableInteger(lblSignonoffId.Text), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                BindData();

            }
            if (e.CommandName.ToUpper().Equals("CREWCOMPLIENT"))
            {
                //PhoenixCrewDeBriefing.updatereview(General.GetNullableInteger(lblSignonoffId.Text));
                //BindData();
                //SetPageNavigator();
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

            RadLabel lblSignonoffId = (RadLabel)e.Item.FindControl("lblSignonoffId");
            RadLabel lblVesselName = (RadLabel)e.Item.FindControl("lblVesselName");
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lblRankName = (RadLabel)e.Item.FindControl("lblRankName");
            RadLabel lblstatusid = (RadLabel)e.Item.FindControl("lblstatusid");
            RadLabel lblempname = (RadLabel)e.Item.FindControl("lblempname");
            RadLabel lblcomplaintid = (RadLabel)e.Item.FindControl("lblcomplaintid");


            LinkButton imgCrewComplaints = (LinkButton)e.Item.FindControl("imgCrewComplaints");

            LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
            if (imgApprove != null)
            {
                imgApprove.Visible = SessionUtil.CanAccess(this.ViewState, imgApprove.CommandName);
                imgApprove.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Review Done?')");
            }


            if (imgApprove != null && lblstatusid != null)
            {
                if (lblstatusid.Text == "1097")
                {
                    imgApprove.Visible = true;
                    imgCrewComplaints.Visible = false;
                }
                if (lblstatusid.Text == "1577")
                {
                    imgApprove.Visible = false;
                    imgCrewComplaints.Visible = true;
                }
            }
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblGenerlComments");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTip");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            if (imgCrewComplaints != null)
            {
                imgCrewComplaints.Attributes.Add("onclick", "parent.openNewWindow('codehelp1','','"+Session["sitepath"]+"/Crew/CrewDeBriefingCrewComplaints.aspx?sigonoffid=" + lblSignonoffId.Text
                    + "&Vesselid=" + lblVesselId.Text + "&Rankid=" + lblRankName.Text + "&Name=" + lblempname.Text + "&Crewcomplaintid=" + lblcomplaintid.Text + "&vesselname=" + lblVesselName.Text + "&popup=1');return false;");
            }

        }
    }
}
