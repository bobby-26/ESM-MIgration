using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;

public partial class Dashboard_DashboardCrewList : PhoenixBasePage
{
    int vesselid;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Dashboard/DashboardCrewList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCrewList')", "Print Grid", "icon_print.png", "PRINT");


            //PhoenixToolbar toolbarmain = new PhoenixToolbar();

            //toolbarmain.AddButton("Particulars", "PARTICULARS");
            //toolbarmain.AddButton("Crew List", "CREWLIST");
            //toolbarmain.AddButton("Certificates", "CERTIFICATES");
            //toolbarmain.AddButton("Admin", "ADMIN");
            //toolbarmain.AddButton("Office Admin", "OFFICE");
            //toolbarmain.AddButton("Manning", "MANNING");
            //toolbarmain.AddButton("Travel", "TRAVEL");
            //toolbarmain.AddButton("Attachments", "ATTACHMENTS");
            //toolbarmain.AddButton("Summary", "SUMMARY");
            //toolbarmain.AddButton("Sync Status", "OLD");
            //toolbarmain.AddButton("Alerts", "ALERTS");
            //MenuDdashboradVesselParticulrs.AccessRights = this.ViewState;

            if (Request.QueryString["launchedfrom"] != null)
            {
                ucTitle.ShowMenu = "false";
            }

            if (Request.QueryString["vesselid"] != null)
            {
                ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                vesselid = int.Parse(ViewState["vesselid"].ToString());
            }
            else
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

                //MenuDdashboradVesselParticulrs.MenuList = toolbarmain.Show();
                //MenuDdashboradVesselParticulrs.SelectedMenuIndex = 1;

                BindToolBar();

                MenuCrewList.MenuList = toolbar.Show();
            }

            if (!IsPostBack)
            {
                
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindToolBar()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCommonDashboard.DashboardPreferencesList(PhoenixSecurityContext.CurrentSecurityContext.UserType
                                                , vesselid
                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            int index = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                toolbar.AddButton(dr["FLDURLDESCRIPTION"].ToString(), dr["FLDCOMMAND"].ToString());
            }

            DataRow[] drindex = ds.Tables[0].Select("FLDCOMMAND='CREWLIST'");
            if (drindex != null)
            {
                index = int.Parse(drindex[0]["FLDSEQUENCE"].ToString()) - 1;
            }
            MenuDdashboradVesselParticulrs.MenuList = toolbar.Show();
            MenuDdashboradVesselParticulrs.SelectedMenuIndex = index;
        }
    }

    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEENAME", "FLDEMPLOYEECODE", "FLDRANKNAME", "FLDNATIONALITYNAME", "FLDSTATUS", "FLDPASSPORTNO", "FLDDATEOFBIRTH", "FLDSIGNONDATE", "FLDDECIMALEXPERIENCE", "FLDRELIEFDUEDATE", "FLDSEAMANBOOKNO" };
        string[] alCaptions = { "Sr.No", "Name", "Employee Code", "Rank", "Nationality", "Status", "PP No.", "Birth Date", "(Exp.) Join", "Exp(M)", "Relief Due", "SMBK No" };

        DataSet ds = PhoenixCommonDashboard.DashboardCrewOnboardList(vesselid);

        //DataTable dt = ds.Tables[0].Copy();
        //General.ShowExcel("Crew List For " + vesselname, dt, alColumns, alCaptions, sortdirection, sortexpression);
        Response.AddHeader("Content-Disposition", "attachment; filename=CrewList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h3><center>Crew List</center></h3></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'></td></tr>");
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

    }

    private void BindData()
    {
        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEENAME", "FLDEMPLOYEECODE", "FLDRANKNAME", "FLDNATIONALITYNAME", "FLDSTATUS", "FLDPASSPORTNO", "FLDDATEOFBIRTH", "FLDSIGNONDATE", "FLDDECIMALEXPERIENCE", "FLDRELIEFDUEDATE", "FLDSEAMANBOOKNO" };
        string[] alCaptions = { "Sr.No", "Name", "Employee Code", "Rank", "Nationality", "Status", "PP No.", "Birth Date", "(Exp.) Join", "Exp(M)", "Relief Due", "SMBK No" };

        DataSet ds = PhoenixCommonDashboard.DashboardCrewOnboardList(vesselid);

        General.SetPrintOptions("gvCrewList", "Crew List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrewList.DataSource = ds;
            gvCrewList.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCrewList);
        }

        DataSet dsedit = PhoenixCommonDashboard.DashboardVesselAdminEdit(vesselid);

        if (dsedit.Tables[0].Rows.Count > 0)
            ViewState["FLDDTKEY"] = dsedit.Tables[0].Rows[0]["FLDDTKEY"].ToString();
        else
            ViewState["FLDDTKEY"] = "";

    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }

    protected void MenuDdashboradVesselParticulrs_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("PARTICULARS"))
        {
            Response.Redirect("../Dashboard/DashboardVesselParticulars.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("CERTIFICATES"))
        {
            Response.Redirect("../Dashboard/DashboardVesselCertificates.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("ADMIN"))
        {
            Response.Redirect("../Dashboard/DashboardAdmin.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("OFFICE"))
        {
            Response.Redirect("../Dashboard/DashboardOfficeAdmin.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("MANNING"))
        {
            Response.Redirect("../Dashboard/DashboardManning.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Dashboard/DashboardAttachments.aspx?dtkey=" + ViewState["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.REGISTERS, true);
        }
        if (dce.CommandName.ToUpper().Equals("TRAVEL"))
        {
            Response.Redirect("../Dashboard/DashboardTravelSingOnOffList.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("SUMMARY"))
        {
            Response.Redirect("../Dashboard/DashboardSummary.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("OLD"))
        {
            Response.Redirect("../Dashboard/DashboardVesselSynchronizationStatus.aspx", false);
        }

        if (dce.CommandName.ToUpper().Equals("ALERTS"))
        {
            Response.Redirect("../Dashboard/DashboardAlerts.aspx", false);
        }
    }
}
