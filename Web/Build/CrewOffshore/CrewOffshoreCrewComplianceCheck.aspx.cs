using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewOffshoreCrewComplianceCheck : PhoenixBasePage
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
            CrewQuery.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreCrewComplianceCheck.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreCrewComplianceCheck.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            CrewQueryMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["VESSELID"] = "";
                ViewState["CURRENTCHARTER"] = "";

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
                BindCurrentCharterer();
                BindNextCharterer();

                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }            
          
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCurrentCharterer()
    {
        ucCurrentCharterer.SelectedAddress = "";
        ViewState["CURRENTCHARTER"] = "";
        DataSet ds = PhoenixCrewOffshoreCrewList.CrewOffshoreCurrentCharterer(General.GetNullableInteger(ViewState["VESSELID"].ToString()), null);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ucCurrentCharterer.DataBind();
            ucCurrentCharterer.SelectedAddress = ds.Tables[0].Rows[0]["FLDCHARTERERID"].ToString();
            ViewState["CURRENTCHARTER"] = ds.Tables[0].Rows[0]["FLDCHARTERERID"].ToString();
        }
    }

    protected void BindNextCharterer()
    {
        DataSet ds = PhoenixCrewOffshoreCrewList.ListCharterer(General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ViewState["CURRENTCHARTER"].ToString()));
        ucDMRCharter.CharterList = ds;
        ucDMRCharter.DataBind();
    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("CREWLIST"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCrewList.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
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
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvCrewSearch.SelectedIndexes.Clear();
                gvCrewSearch.EditIndexes.Clear();
                gvCrewSearch.DataSource = null;
                //BindData();
                gvCrewSearch.Rebind();
               
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            alColumns = new string[] { "FLDROWNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", 
                                 "FLDNATIONALITYNAME","FLDADDITIONALCERTIFICATE",  "FLDPASSPORTNO","FLDDATEOFJOINING", "FLDRELIEFDUEDATE","FLD90RELIEFDATE", "FLDSTCWFLAGCOMPLIANCE", "FLDCHARTERERCOMPLIANCE" };
            alCaptions = new string[] { "Sl.No", "Vessel", "File No", "Name", "Rank",  "Nationality","Additional Certificate", "Passport No", "Sign On Date", "End of Contract",
                                "Max Tour of Duty","STCW & FLAG Requirements", "Charterer's Requirements"};
        }
        else
        {
            alColumns = new string[] { "FLDROWNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", 
                                 "FLDNATIONALITYNAME","FLDADDITIONALCERTIFICATE",  "FLDPASSPORTNO","FLDDAILYRATE", "FLDDPALLOWANCE", "FLDDATEOFJOINING", "FLDRELIEFDUEDATE","FLD90RELIEFDATE", "FLDSTCWFLAGCOMPLIANCE", "FLDCHARTERERCOMPLIANCE" };
            alCaptions = new string[] { "Sl.No", "Vessel", "File No", "Name", "Rank",  "Nationality","Additional Certificate", "Passport No","Daily Rate (USD)", "Daily DP Allowance (USD)", "Sign On Date", "End of Contract",
                                "Max Tour of Duty","STCW & FLAG Requirements", "Charterer's Requirements"};
        }

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

        DataTable dt = PhoenixCrewOffshoreCrewList.SearchVesselCrewSignOnOff(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                               , 1
                                                                               , sortexpression, sortdirection
                                                                               , 1, iRowCount
                                                                               , ref iRowCount, ref iTotalPageCount
                                                                               , General.GetNullableInteger(ViewState["CURRENTCHARTER"].ToString())
                                                                               , General.GetNullableInteger(ucDMRCharter.SelectedValue));        

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewComplianceCheck.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Crew List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length - 2; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }

            for(int i = alColumns.Length - 2; i < alColumns.Length; i++)
            {                
                if(dr[alColumns[i]].ToString().Equals("1"))
                {
                    Response.Write("<td style='background-color:Green;'>");
                    Response.Write("</td>");
                }
                if (dr[alColumns[i]].ToString().Equals("2"))
                {
                    Response.Write("<td style='background-color:Yellow;'>");
                    Response.Write("</td>");
                }
                if (dr[alColumns[i]].ToString().Equals("3"))
                {
                    Response.Write("<td style='background-color:Red;'>");
                    Response.Write("</td>");
                }
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

        //General.ShowExcel("Crew List", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        BindCurrentCharterer();
        BindNextCharterer();
        //BindData();
        gvCrewSearch.SelectedIndexes.Clear();
        gvCrewSearch.EditIndexes.Clear();
        gvCrewSearch.DataSource = null;
        //BindData();
        gvCrewSearch.Rebind();
       
        
    }

    public void BindData()
    {        
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            alColumns = new string[] { "FLDROWNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", 
                                 "FLDNATIONALITYNAME","FLDADDITIONALCERTIFICATE",  "FLDPASSPORTNO","FLDDATEOFJOINING", "FLDRELIEFDUEDATE","FLD90RELIEFDATE", "FLDSTCWFLAGCOMPLIANCE", "FLDCHARTERERCOMPLIANCE" };
            alCaptions = new string[] { "Sl.No", "Vessel", "File No", "Name", "Rank",  "Nationality","Additional Certificate", "Passport No", "Sign On Date", "End of Contract",
                                "Max Tour of Duty","STCW & FLAG Requirements", "Charterer's Requirements"};
        }
        else
        {
            alColumns = new string[] { "FLDROWNUMBER", "FLDVESSELNAME", "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", 
                                 "FLDNATIONALITYNAME","FLDADDITIONALCERTIFICATE",  "FLDPASSPORTNO","FLDDAILYRATE", "FLDDPALLOWANCE", "FLDDATEOFJOINING", "FLDRELIEFDUEDATE","FLD90RELIEFDATE", "FLDSTCWFLAGCOMPLIANCE", "FLDCHARTERERCOMPLIANCE" };
            alCaptions = new string[] { "Sl.No", "Vessel", "File No", "Name", "Rank",  "Nationality","Additional Certificate", "Passport No","Daily Rate (USD)", "Daily DP Allowance (USD)", "Sign On Date", "End of Contract",
                                "Max Tour of Duty","STCW & FLAG Requirements", "Charterer's Requirements"};
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            DataTable dt = PhoenixCrewOffshoreCrewList.SearchVesselCrewSignOnOff(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                       , 1
                                                                       , sortexpression, sortdirection
                                                                       , (int)ViewState["PAGENUMBER"], gvCrewSearch.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount
                                                                       , General.GetNullableInteger(ViewState["CURRENTCHARTER"].ToString())
                                                                       , General.GetNullableInteger(ucDMRCharter.SelectedValue));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewSearch", "Crew Compliance Check", alCaptions, alColumns, ds);
            gvCrewSearch.DataSource = dt;
            gvCrewSearch.VirtualItemCount = iRowCount;

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
               gvCrewSearch.Columns[8].Visible = false;
               gvCrewSearch.Columns[9].Visible = false;
            }
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
   
    protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = sender as GridView;
        Filter.CurrentVesselCrewSelection = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblEmployeeid")).Text;
        Response.Redirect("..\\VesselAccounts\\VesselAccountsEmployeeGeneral.aspx", false);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
       
        BindData();
      
    }

    protected void imgSearch_Click(object sender, EventArgs e)
    {
        gvCrewSearch.Rebind();
       
    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {


            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadLabel lblSTCWFlag = (RadLabel)e.Item.FindControl("lblSTCWFlag");
            RadLabel lblCharterer = (RadLabel)e.Item.FindControl("lblCharterer");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            RadLabel lblTrainingMatrixid = (RadLabel)e.Item.FindControl("lblTrainingMatrixid");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            RadLabel lblCrewPlanid = (RadLabel)e.Item.FindControl("lblCrewPlanid");
            RadLabel lblCurrentMatrix = (RadLabel)e.Item.FindControl("lblCurrentMatrix");
            RadLabel lblNextMatrix = (RadLabel)e.Item.FindControl("lblNextMatrix");
            UserControlCommonToolTip ucCommonToolTip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");

            if (General.GetNullableInteger(ucDMRCharter.SelectedValue) != null)
            {
                lblSTCWFlag.Attributes.Add("onclick", "parent.openNewWindow('course', '', '"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + lblEmployeeid.Text + "&trainingmatrixid=" + lblNextMatrix.Text + "&stage=1&compliance=1&vesselid=" + lblVesselid.Text + "&crewplanid=" + lblCrewPlanid.Text + "');return false;");
                lblCharterer.Attributes.Add("onclick", "parent.openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + lblEmployeeid.Text + "&trainingmatrixid=" + lblNextMatrix.Text + "&stage=2&compliance=1&vesselid=" + lblVesselid.Text + "&crewplanid=" + lblCrewPlanid.Text + "');return false;");
            }
            else
            {
                lblSTCWFlag.Attributes.Add("onclick", "parent.openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + lblEmployeeid.Text + "&trainingmatrixid=" + lblCurrentMatrix.Text + "&stage=1&compliance=1&vesselid=" + lblVesselid.Text + "&crewplanid=" + lblCrewPlanid.Text + "');return false;");
                lblCharterer.Attributes.Add("onclick", "parent.openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + lblEmployeeid.Text + "&trainingmatrixid=" + lblCurrentMatrix.Text + "&stage=2&compliance=1&vesselid=" + lblVesselid.Text + "&crewplanid=" + lblCrewPlanid.Text + "');return false;");
            }

            DataTable dt = new DataTable();
            dt = PhoenixCrewOffshoreCrewChange.WaivedDocumentList(General.GetNullableGuid(lblCrewPlanid.Text));
            if (dt.Rows.Count == 0)
            {
                if (ucCommonToolTip != null)
                {
                    ucCommonToolTip.Visible = false;

                }

            }
            else
            {
                if (ucCommonToolTip != null)
                {
                    ucCommonToolTip.Visible = true;

                }

            }

            if (dr["FLDSTCWFLAGCOMPLIANCE"] != null && dr["FLDSTCWFLAGCOMPLIANCE"].ToString() != "")
            {
                if (dr["FLDSTCWFLAGCOMPLIANCE"].ToString().Equals("1"))
                {
                    lblSTCWFlag.BackColor = System.Drawing.Color.Green;
                    lblSTCWFlag.ForeColor = System.Drawing.Color.Green;
                }
                else if (dr["FLDSTCWFLAGCOMPLIANCE"].ToString().Equals("2"))
                {
                    lblSTCWFlag.BackColor = System.Drawing.Color.Yellow;
                    lblSTCWFlag.ForeColor = System.Drawing.Color.Yellow;
                }
                else if (dr["FLDSTCWFLAGCOMPLIANCE"].ToString().Equals("3"))
                {
                    lblSTCWFlag.BackColor = System.Drawing.Color.Red;
                    lblSTCWFlag.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblSTCWFlag.BackColor = System.Drawing.Color.White;
                    lblSTCWFlag.ForeColor = System.Drawing.Color.White;
                }
                if (dr["FLDSTCWFLAGCOMPLIANCE"].ToString().Equals("4"))
                {
                    lblSTCWFlag.Text = "T & Q matrix not defined";
                    lblSTCWFlag.ForeColor = System.Drawing.Color.Black;
                }
            }

            if (dr["FLDCHARTERERCOMPLIANCE"] != null && dr["FLDCHARTERERCOMPLIANCE"].ToString() != "")
            {
                if (dr["FLDCHARTERERCOMPLIANCE"].ToString().Equals("1"))
                {
                    lblCharterer.BackColor = System.Drawing.Color.Green;
                    lblCharterer.ForeColor = System.Drawing.Color.Green;
                }
                else if (dr["FLDCHARTERERCOMPLIANCE"].ToString().Equals("2"))
                {
                    lblCharterer.BackColor = System.Drawing.Color.Yellow;
                    lblCharterer.ForeColor = System.Drawing.Color.Yellow;
                }
                else if (dr["FLDCHARTERERCOMPLIANCE"].ToString().Equals("3"))
                {
                    lblCharterer.BackColor = System.Drawing.Color.Red;
                    lblCharterer.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblCharterer.BackColor = System.Drawing.Color.White;
                    lblCharterer.ForeColor = System.Drawing.Color.White;
                }
                if (dr["FLDCHARTERERCOMPLIANCE"].ToString().Equals("4"))
                {
                    lblCharterer.Text = "T & Q matrix not defined";
                    lblCharterer.ForeColor = System.Drawing.Color.Black;
                }
            }
            RadLabel lblCer = (RadLabel)e.Item.FindControl("lblCertificate");
            UserControlToolTip uccer = (UserControlToolTip)e.Item.FindControl("ucToolTipCertificate");
            lblCer.Attributes.Add("onmouseover", "showTooltip(ev, '" + uccer.ToolTip + "', 'visible');");
            lblCer.Attributes.Add("onmouseout", "showTooltip(ev, '" + uccer.ToolTip + "', 'hidden');");

            LinkButton lnkEmployeeName = (LinkButton)e.Item.FindControl("lnkEmployeeName");

            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreVesselEmployeeGeneral.aspx?empid=" + lblEmployeeid.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&launchedfrom=offshore'); return false;");
            }

            if (lnkEmployeeName != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (dr["FLDEMPLOYEECODE"] != null && General.GetNullableString(dr["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + dr["FLDCREWPLANID"].ToString() + "'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "&crewplanid=" + dr["FLDCREWPLANID"].ToString() + "'); return false;");
            }
            RadLabel lblName = (RadLabel)e.Item.FindControl("lblEmployeeName");
            if (dr["FLDVIEWPARTICULARSYN"] != null && dr["FLDVIEWPARTICULARSYN"].ToString().Equals("1"))
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

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }
}
