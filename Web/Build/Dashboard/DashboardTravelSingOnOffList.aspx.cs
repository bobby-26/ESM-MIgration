using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;


public partial class Dashboard_DashboardTravelSingOnOffList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvTravelSignOn.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvTravelSignOn.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }        
        foreach (GridViewRow r in gvTravelSignOff.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvTravelSignOff.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarOnSigner = new PhoenixToolbar();
        toolbarOnSigner.AddImageButton("../Dashboard/DashboardTravelSingOnOffList.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
        toolbarOnSigner.AddImageLink("javascript:CallPrint('gvTravelSignOn')", "Print Grid", "icon_print.png", "PRINT");
        MenuTravelOnSignerList.AccessRights = this.ViewState;
        MenuTravelOnSignerList.MenuList = toolbarOnSigner.Show();

        PhoenixToolbar toolbarOffSigner = new PhoenixToolbar();
        toolbarOffSigner.AddImageButton("../Dashboard/DashboardTravelSingOnOffList.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
        toolbarOffSigner.AddImageLink("javascript:CallPrint('gvTravelSignOff')", "Print Grid", "icon_print.png", "PRINT");
        MenuTravelOffSignerList.AccessRights = this.ViewState;
        MenuTravelOffSignerList.MenuList = toolbarOffSigner.Show();

        //PhoenixToolbar toolbarmain = new PhoenixToolbar();

        //toolbarmain.AddButton("Vessel Overview", "PARTICULARS");
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
        //MenuDdashboradVesselParticulrs.MenuList = toolbarmain.Show();
        //MenuDdashboradVesselParticulrs.SelectedMenuIndex = 6;

        BindToolBar();

        AttachmentEdit();

        if (!IsPostBack)
        {
           
        }
        BindDataSignOn();
        BindDataSignOff();
     
    }

    protected void BindToolBar()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCommonDashboard.DashboardPreferencesList(PhoenixSecurityContext.CurrentSecurityContext.UserType
                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
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

            DataRow[] drindex = ds.Tables[0].Select("FLDCOMMAND='TRAVEL'");
            if (drindex != null)
            {
                index = int.Parse(drindex[0]["FLDSEQUENCE"].ToString()) - 1;
            }
            MenuDdashboradVesselParticulrs.MenuList = toolbar.Show();
            MenuDdashboradVesselParticulrs.SelectedMenuIndex = index;
        }
    }


    public void AttachmentEdit()
    {
        DataSet dsedit = PhoenixCommonDashboard.DashboardVesselAdminEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (dsedit.Tables[0].Rows.Count > 0)
            ViewState["FLDDTKEY"] = dsedit.Tables[0].Rows[0]["FLDDTKEY"].ToString();
        else
            ViewState["FLDDTKEY"] = "";
    }

    protected void TravelOnSignerList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcelOnSigner();
        }       
    }
    protected void TravelOffSignerList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcelOnSigner();
        }
    }

    private void BindDataSignOn()
    {
        string[] alColumns = { "FLDEMPLOYEEID", "FLDNAME", "FLDCREWPLANVESSELID", "FLDRANKNAME", "FLDDATEOFBIRTH", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDUSVISANUMBER",  "FLDTRAVELDATE", "FLDARRIVALDATE", "FLDONSIGNERYN", "FLDFROMSEAPORTID", "FLDREQUISITIONNO", "FLDORIGINNAME", "FLDDESTINATIONNAME", "FLDPAYMENTMODE", "FLDDEPARTUREAMPM", "FLDARRIVALAMPM", "FLDDEPARTUREAMPMID", "FLDARRIVALAMPMID" };
        string[] alCaptions = { "Employee ID", "Name", "Crewplanvessel ID", "Rank name", "Date of birth", "Passport no", "Seaman bookno", "Usvisa number", "Travel date", "Arrival date", "Onsigner yn", "Fromseaport ID", "Requisition no", "Origin name", "Destination name", "Payment mode", "Departure ampm", "Arrival ampm", "Departure ampm ID", "Arrival ampm ID" };

        DataSet ds = PhoenixCommonDashboard.DashboardTravelOnSignerList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        General.SetPrintOptions("gvTravelSignOn", "Travel SignOn List", alCaptions, alColumns, ds);

        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            gvTravelSignOn.DataSource = dt;
            gvTravelSignOn.DataBind();
        }
        else
            ShowNoRecordsFound(dt, gvTravelSignOn);
    }

    private void BindDataSignOff()
    {
        string[] alColumns = { "FLDEMPLOYEEID", "FLDNAME", "FLDCREWPLANVESSELID", "FLDRANKNAME", "FLDDATEOFBIRTH", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDUSVISANUMBER", "FLDTRAVELDATE", "FLDARRIVALDATE", "FLDONSIGNERYN", "FLDFROMSEAPORTID", "FLDREQUISITIONNO", "FLDORIGINNAME", "FLDDESTINATIONNAME", "FLDPAYMENTMODE", "FLDDEPARTUREAMPM", "FLDARRIVALAMPM", "FLDDEPARTUREAMPMID", "FLDARRIVALAMPMID" };
        string[] alCaptions = { "Employee ID", "Name", "Crewplanvessel ID", "Rank name", "Date of birth", "Passport no", "Seaman bookno", "Usvisa number", "Travel date", "Arrival date", "Onsigner yn", "Fromseaport ID", "Requisition no", "Origin name", "Destination name", "Payment mode", "Departure ampm", "Arrival ampm", "Departure ampm ID", "Arrival ampm ID" };

        DataSet ds = PhoenixCommonDashboard.DashboardTravelOffSignerList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        General.SetPrintOptions("gvTravelSignOff", "Travel SignOff List", alCaptions, alColumns, ds);

        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            gvTravelSignOff.DataSource = dt;
            gvTravelSignOff.DataBind();
        }
        else
            ShowNoRecordsFound(dt, gvTravelSignOff);
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
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected void gvTravelSignOn_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvTravelSignOn, "Select$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelSignOff_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvTravelSignOff, "Select$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void  ShowExcelOnSigner()
    {

        string[] alColumns = { "FLDEMPLOYEEID", "FLDNAME", "FLDCREWPLANVESSELID", "FLDRANKNAME", "FLDDATEOFBIRTH", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDUSVISANUMBER", "FLDTRAVELDATE", "FLDARRIVALDATE", "FLDONSIGNERYN", "FLDFROMSEAPORTID", "FLDREQUISITIONNO", "FLDORIGINNAME", "FLDDESTINATIONNAME", "FLDPAYMENTMODE", "FLDDEPARTUREAMPM", "FLDARRIVALAMPM", "FLDDEPARTUREAMPMID", "FLDARRIVALAMPMID" };
        string[] alCaptions = { "Employee ID", "Name", "Crewplanvessel ID", "Rank name", "Date of birth", "Passport no", "Seaman bookno", "Usvisa number", "Travel date", "Arrival date", "Onsigner yn", "Fromseaport ID", "Requisition no", "Origin name", "Destination name", "Payment mode", "Departure ampm", "Arrival ampm", "Departure ampm ID", "Arrival ampm ID" };

        DataSet ds = PhoenixCommonDashboard.DashboardTravelOnSignerList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"TravelOnSignerList.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel OnSigner List</h3></td>");
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
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

    }
    protected void ShowExcelOffSigner()
    {

        string[] alColumns = { "FLDEMPLOYEEID", "FLDNAME", "FLDCREWPLANVESSELID", "FLDRANKNAME", "FLDDATEOFBIRTH", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDUSVISANUMBER", "FLDTRAVELDATE", "FLDARRIVALDATE", "FLDONSIGNERYN", "FLDFROMSEAPORTID", "FLDREQUISITIONNO", "FLDORIGINNAME", "FLDDESTINATIONNAME", "FLDPAYMENTMODE", "FLDDEPARTUREAMPM", "FLDARRIVALAMPM", "FLDDEPARTUREAMPMID", "FLDARRIVALAMPMID" };
        string[] alCaptions = { "Employee ID", "Name", "Crewplanvessel ID", "Rank name", "Date of birth", "Passport no", "Seaman bookno", "Usvisa number", "Travel date", "Arrival date", "Onsigner yn", "Fromseaport ID", "Requisition no", "Origin name", "Destination name", "Payment mode", "Departure ampm", "Arrival ampm", "Departure ampm ID", "Arrival ampm ID" };

        DataSet ds = PhoenixCommonDashboard.DashboardTravelOffSignerList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"TravelOffSignerList.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel OffSigner List</h3></td>");
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
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

    }

    protected void MenuDdashboradVesselParticulrs_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("PARTICULARS"))
        {
            Response.Redirect("../Dashboard/DashboardVesselParticulars.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("CREWLIST"))
        {
            Response.Redirect("../Dashboard/DashboardVesselCrewList.aspx", true);
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
        if (dce.CommandName.ToUpper().Equals("NEW"))
        {
            Response.Redirect("../Dashboard/DashboardBlank.aspx", true);
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
