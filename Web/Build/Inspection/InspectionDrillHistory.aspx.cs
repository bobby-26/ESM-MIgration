using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;

public partial class Registers_DrillHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDrillHistorylist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillHistory.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillHistory.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        Tabstripdrillhistorymenu.MenuList = toolbargrid.Show();

        PhoenixToolbar menu = new PhoenixToolbar();
        menu.AddButton("Drill Planner & Record", "Toggle4", ToolBarDirection.Right);
        menu.AddButton("History", "Toggle3", ToolBarDirection.Right);
        menu.AddButton("Drill Schedule", "Toggle2", ToolBarDirection.Right);

        Tabstripdrillhistoy.MenuList = menu.Show();
        Tabstripdrillhistoy.SelectedMenuIndex = 1;


     
        
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["PAGENUMBER"]         = 1;
           
            gvDrillHistorylist.PageSize     = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            DataTable            dt1        = PhoenixRegisterDrillScenario.drillcheckboxlist();

            radcombodrill.DataSource        = dt1;
            radcombodrill.DataTextField     = "FLDDRILLNAME";
            radcombodrill.DataValueField    = "FLDDRILLID";
            radcombodrill.DataBind();

            int currentvesselid = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;

            ViewState["CURRENTVESSELID"] = currentvesselid.ToString();
            if (currentvesselid == 0)
            {

                int? vesselid = General.GetNullableInteger(Request.QueryString["vesselid"]);
                if (vesselid == null)
                {
                    ViewState["VESSELID"] = null;
                }
                else
                {
                    ViewState["VESSELID"] = vesselid;
                    ddlvessellist.SelectedVessel = vesselid.ToString();
                }
            }
            else {
                ViewState["VESSELID"] = currentvesselid.ToString();
                ddlvessellist.SelectedVessel = currentvesselid.ToString();
                ddlvessellist.Enabled = false;
            }

        }
    }


    protected void ShowExcel()
    {
        int         iRowCount                       = 0;
        int         iTotalPageCount                 = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDDRILLLASTDONEDATE" ,"FLDREASON"};
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date", "Reason for no attachments" };





        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount                       = 10;
        else
                    iRowCount                       = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        int?        vesselid                        = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        Guid?       drillid                         = General.GetNullableGuid(radcombodrill.SelectedValue.ToString());
        DateTime?   fromdate                        = General.GetNullableDateTime(tbfromdateentry.Text);
        DateTime?   todate                          = General.GetNullableDateTime(tbtodateentry.Text);
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        DataTable   dt                              = PhoenixInspectionDrillHistory.drillhistorylist(rowusercode ,vesselid, drillid,
                                                        gvDrillHistorylist.CurrentPageIndex + 1,
                                                        gvDrillHistorylist.PageSize, fromdate, todate,
                                                        ref iRowCount,
                                                        ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Drill History.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drill History</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td>");
        Response.Write("From Date :");
        Response.Write("</td>");
        Response.Write("<td>");
        Response.Write(General.GetDateTimeToString(fromdate));
        Response.Write("</td>");
        Response.Write("<td>");
        Response.Write("To Date :");
        Response.Write("</td>");
        Response.Write("<td>");
        Response.Write(General.GetDateTimeToString(todate));
        Response.Write("</td>");
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
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                if (alColumns[i] == "FLDDRILLLASTDONEDATE")
                {
                    Response.Write(General.GetDateTimeToString(dr[alColumns[i]]));
                }
                else
                {
                    Response.Write(dr[alColumns[i]]);
                }
               
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void drillhistorymenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
                    RadToolBarEventArgs dce         = (RadToolBarEventArgs)e;
                    string              CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                DateTime? fromdate = General.GetNullableDateTime(tbfromdateentry.Text);
                DateTime? todate = General.GetNullableDateTime(tbtodateentry.Text);

                if (!IsValidDrillHistoryDetails(fromdate, todate))
                {
                    ucError.Visible = true;
                    
                }

                gvDrillHistorylist.Rebind();
                //if ((General.GetNullableInteger(ddlvessellist.SelectedValue.ToString()))!= null)
                //{

                    ViewState["VESSELID"] = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
                //}
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                            ddlvessellist.SelectedVessel    =       string.Empty;
                            radcombodrill.ClearSelection();
                            radcombodrill.Text              =       string.Empty;
                            tbfromdateentry.Text            =       string.Empty;
                            tbtodateentry.Text              =       string.Empty;
                            gvDrillHistorylist.Rebind();
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    
    }
    protected void drillhistory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
                    RadToolBarEventArgs dce         = (RadToolBarEventArgs)e;
                    string              CommandName = ((RadToolBarButton)dce.Item).CommandName;


           
           
            if (CommandName.ToUpper().Equals("TOGGLE2"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillSchedule.aspx?vesselid=" + vesselid);
                }
                else 
                Response.Redirect("../Inspection/InspectionDrillSchedule.aspx");
                
            }
            if (CommandName.ToUpper().Equals("TOGGLE3"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillHistory.aspx?vesselid=" + vesselid);
                }
                else 

                Response.Redirect("../Inspection/InspectionDrillHistory.aspx");
               
            }
            if (CommandName.ToUpper().Equals("TOGGLE4"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillReport.aspx?vesselid=" + vesselid);
                }
                else 
                Response.Redirect("../Inspection/InspectionDrillReport.aspx");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

  


    

    
    
    public void gvDrillHistorylist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem        item            = e.Item as GridDataItem;
                Guid?               drillscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDDRILLSCHEDULEID").ToString());
                DataTable           dt              = PhoenixInspectionDrillSchedule.getflddtkey(drillscheduleid);
                Guid?               flddtkey        = General.GetNullableGuid(dt.Rows[0]["FLDDTKEY"].ToString());
                int installcode = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
                LinkButton          db              = ((LinkButton)item.FindControl("btnattachments"));
                if (db != null)
                {
                    db.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + flddtkey + "&mod="
                            + PhoenixModule.QUALITY +"&u=n"+ "'); return false;");
                }


                HtmlAnchor          drillname       = (HtmlAnchor)item.FindControl("DrillNameanchor");

                drillname.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','Drill History','Inspection/InspectionDrillHistorypopup.aspx?drillscheduleid=" + drillscheduleid + "');return false");
                if (General.GetNullableString(dt.Rows[0]["FLDREASON"].ToString()) != null)
                { db.Visible = false; }

                if (installcode == 0)
                {

                    LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Report','Drill Report','Inspection/InspectionDrillScheduleReport.aspx?drillscheduleid=" + drillscheduleid + "&e=y"+"');return false");
                    edit.Visible = true;


                }
            }
            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }


    }

    


    protected void gvDrillHistorylist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int         iRowCount       = 0;
        int         iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDDRILLLASTDONEDATE", "FLDREASON" };
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date", "Reason for no attachments" };
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        Guid?       drillid         = General.GetNullableGuid(radcombodrill.SelectedValue.ToString());
        DateTime?   fromdate        = General.GetNullableDateTime(tbfromdateentry.Text);
        DateTime?   todate          = General.GetNullableDateTime(tbtodateentry.Text);
        int?        vesselid        = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
            

        DataTable   dt              = PhoenixInspectionDrillHistory.drillhistorylist(rowusercode ,vesselid, drillid,
                                                gvDrillHistorylist.CurrentPageIndex + 1,
                                                 gvDrillHistorylist.PageSize, fromdate, todate,
                                                ref iRowCount,
                                                ref iTotalPageCount);




        gvDrillHistorylist.DataSource = dt;
        gvDrillHistorylist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;

        General.SetPrintOptions("gvDrillHistorylist", "Drill History", alCaptions, alColumns, ds);
    }

    private bool IsValidDrillHistoryDetails(DateTime? Fromdate, DateTime? ToDate)
    {

        ucError.HeaderMessage = "Error!";



        if (Fromdate != null && ToDate != null)
        {
            if (!(ToDate > Fromdate))
            {

                ucError.ErrorMessage = "To Date Cannot be less than From Date";
            }
        }
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvDrillHistorylist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}