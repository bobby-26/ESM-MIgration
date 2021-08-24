using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;

public partial class Registers_DrillSchedule : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillSchedule.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDrillSchedulelist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            toolbargrid.AddFontAwesomeButton("javascript:radconfirm('You are about to create drill schedule for this vessel.Continue ? ', confirmCallBackFn, 320, 150, null, 'Confirm');", "Refresh", "<i class=\"fas fa-sync\"></i>", "REFRESH");
        }
            TabstripMenu.MenuList = toolbargrid.Show();

        PhoenixToolbar menu = new PhoenixToolbar();
        menu.AddButton("Drill Planner & Record", "Toggle4", ToolBarDirection.Right);
        menu.AddButton("History", "Toggle3", ToolBarDirection.Right);
        menu.AddButton("Drill Schedule", "Toggle2", ToolBarDirection.Right);
        Tabstripdrillschedule.MenuList = menu.Show();
        Tabstripdrillschedule.SelectedMenuIndex = 2;

        int currentvesselid = -1;

        if (Request.QueryString["FMSYN"] == null || Request.QueryString["FMSYN"].ToString() == string.Empty)
        {
            if (Request.QueryString["VESSELID"] != null)
                currentvesselid = int.Parse(Request.QueryString["VESSELID"].ToString());
            else
                currentvesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ViewState["CURRENTVESSELID"] = currentvesselid.ToString();
        }
        else
        {
            ViewState["FMSYN"] = Request.QueryString["FMSYN"].ToString();
            ViewState["CURRENTVESSELID"] = Request.QueryString["VESSELID"].ToString();
            currentvesselid = int.Parse(Request.QueryString["VESSELID"].ToString());
            ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
        }
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
          
            ViewState["DRILL"] = string.Empty;
            ViewState["SORTEXPRESSION"] = string.Empty;
            ViewState["SORTDIRECTION"] = string.Empty;
            ViewState["OVERDUE"] = "0";
            ViewState["OVERDUE"] = Request.QueryString["overdue"];
            if (ViewState["OVERDUE"] == null)
            {
                ViewState["OVERDUE"] = "0";
            }
            if (ViewState["OVERDUE"].ToString() == "1")
            {
                radoverdue.Checked = true;
            }
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
            else
            {
                ViewState["VESSELID"] = currentvesselid.ToString();
                ddlvessellist.SelectedVessel = currentvesselid.ToString();
                ddlvessellist.Enabled = false;

            }

            gvDrillSchedulelist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            
        }
        

    }
    protected void ShowExcel()
    {
        int         iRowCount       = 0;
        int         iTotalPageCount = 0;
        string[] alColumns = { "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDFIXEDORVARIABLE", "FLDTYPE", "FLDDRILLDUEDATE", "FLDDRILLLASTDONEDATE" };
        string[] alCaptions = { "Name", "Interval", "Interval Type", "Fixed/Variable", "Type", "Due Date", "Last done Date" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount       = 10;
        else
                    iRowCount       = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        int?        vesselid        = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());

        DataSet     dt              = PhoenixInspectionDrillSchedule.drillschedulesearch(General.GetNullableString(ViewState["DRILL"].ToString()),
                                                vesselid,
                                                General.GetNullableInteger(ViewState["OVERDUE"].ToString()),
                                                 General.GetNullableString(ViewState["SORTEXPRESSION"].ToString()),
                                                General.GetNullableInteger(ViewState["SORTDIRECTION"].ToString()),
                                                gvDrillSchedulelist.CurrentPageIndex + 1,
                                                 gvDrillSchedulelist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Drill Schedule.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drill Schedule</h3></td>");
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
        foreach (DataRow dr in dt.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                if (alColumns[i] == "FLDDRILLDUEDATE" || alColumns[i] == "FLDDRILLLASTDONEDATE")
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
    protected void drillschedule_TabStripMenuCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs     dce         = (RadToolBarEventArgs)e;
            string                  CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;

        }

    }
    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel);
            PhoenixInspectionDrillSchedule.CreateDrillSchedule(rowusercode, vesselid);
            gvDrillSchedulelist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void drillschedule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs     dce         = (RadToolBarEventArgs)e;
            string                  CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("TOGGLE2"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillSchedule.aspx?vesselid=" + vesselid);
                }
                else 
                Response.Redirect("~/Inspection/InspectionDrillSchedule.aspx");
               
            }
            if (CommandName.ToUpper().Equals("TOGGLE3"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillHistory.aspx?vesselid=" + vesselid);
                }
                else

                    Response.Redirect("~/Inspection/InspectionDrillHistory.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE4"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillReport.aspx?vesselid=" + vesselid);
                }
                else

                    Response.Redirect("~/Inspection/InspectionDrillReport.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;

        }
    }

    protected void ddlvessellist_textchange(object sender, EventArgs e)
    {
        gvDrillSchedulelist.Rebind();
        if (ddlvessellist.SelectedVessel != null)
        {

            ViewState["VESSELID"] = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        }

    }

    public void gvDrillSchedulelist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            int installcode = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
            if (e.Item is GridDataItem)
            {
                DataRowView drv             = (DataRowView)e.Item.DataItem;
                GridDataItem        item            = e.Item as GridDataItem;
                int?                vesselid        = General.GetNullableInteger(ddlvessellist.SelectedVessel);
                Guid?               drillscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDDRILLSCHEDULEID").ToString());
              
                Guid?               flddtkey        = General.GetNullableGuid(drv["FLDDTKEY"].ToString());
                DateTime?           drillduedate    = General.GetNullableDateTime(drv["OVERDUE"].ToString());
                string              submissionstatus = General.GetNullableString(drv["FLDSUBMISSIONSTATUS"].ToString());
                LinkButton history = ((LinkButton)item.FindControl("btnhistory"));
                if (history != null)
                {
                    history.Attributes.Add("onclick", "javascript:parent.openNewWindow('History','','Inspection/InspectionDrillIndividualHistory.aspx?drillscheduleid=" + drillscheduleid + "&vesselid=" + vesselid + "');return false");

                }

                LinkButton reportdrill = ((LinkButton)item.FindControl("reportbtn"));
                if (reportdrill != null)
                {
                    reportdrill.Attributes.Add("onclick", "javascript:parent.openNewWindow('Report','','Inspection/InspectionDrillScheduleReport.aspx?drillscheduleid=" + drillscheduleid + "&vesselid=" + vesselid + "');return false");

                }

                HtmlAnchor drillname = (HtmlAnchor)item.FindControl("DrillNameanchor");

                drillname.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','Drill Due Across Vessels','Inspection/InspectionDrillSchedulePopupList.aspx?drillscheduleid=" + drillscheduleid + "');return false");


                int? currentvesselid = General.GetNullableInteger(ViewState["CURRENTVESSELID"].ToString());

                if (currentvesselid > 0)
                {
                    reportdrill.Visible = true;
                    
                }

                LinkButton imgFlag = (LinkButton)e.Item.FindControl("imgFlag");

               
                if (imgFlag != null)
                {
                    if ( submissionstatus == "Draft")
                    {
                        imgFlag.Visible = true;

                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-star-yellow\"></i></span>";
                        imgFlag.Controls.Add(html);
                    }
                    else if (drillduedate == null || drillduedate < DateTime.Today)
                    {
                        imgFlag.Visible = true;
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-star-red\"></i></span>";
                        imgFlag.Controls.Add(html);

                    }
                   
                }

                if (installcode == 0)
                {

                    LinkButton edit = (LinkButton)e.Item.FindControl("btnedit");
                    edit.Visible = true;
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Edit Lastdonedate','','Inspection/InspectionDrilldoneedit.aspx?drillscheduleid=" + drillscheduleid  + "','false','300px','420px');return false");


                }
            }
            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                TextBox textBox = (TextBox)filterItem["FLDDRILLNAME"].Controls[0];
                textBox.MaxLength = 198;

            }

        }
           
            
       

        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;

        }
    }


    protected void gvDrillSchedulelist_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvDrillSchedulelist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDFIXEDORVARIABLE", "FLDTYPE", "FLDDRILLDUEDATE", "FLDDRILLLASTDONEDATE" };
        string[] alCaptions = { "Name", "Interval", "Interval Type", "Fixed/Variable", "Type", "Due Date", "Last done Date" };

        int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());



        DataSet ds = PhoenixInspectionDrillSchedule.drillschedulesearch(General.GetNullableString(ViewState["DRILL"].ToString()),
                                                vesselid,
                                                 General.GetNullableInteger(ViewState["OVERDUE"].ToString()),
                                                General.GetNullableString(ViewState["SORTEXPRESSION"].ToString()),
                                                General.GetNullableInteger(ViewState["SORTDIRECTION"].ToString()),
                                                gvDrillSchedulelist.CurrentPageIndex + 1,
                                                gvDrillSchedulelist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);


        General.SetPrintOptions("gvDrillSchedulelist", "Drill Schedule ", alCaptions, alColumns,ds);
       
        gvDrillSchedulelist.DataSource = ds.Tables[0];
        gvDrillSchedulelist.VirtualItemCount = iRowCount;
    }

  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvDrillSchedulelist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDrillSchedulelist_Sort(object source, GridSortCommandEventArgs e)
    {
        if (e.SortExpression == "FLDDRILLDUEDATE")
        {
            ViewState["SORTEXPRESSION"] = "FLDDRILLDUEDATE";
           
        }
        if (e.SortExpression == "FLDDRILLDUEDATE")
        {
            ViewState["SORTEXPRESSION"] = "FLDDRILLDUEDATE";
        }

        if (e.NewSortOrder == GridSortOrder.Descending)
        {
            ViewState["SORTDIRECTION"] = 1;
        }
           
    }

    protected void radoverdue_CheckedChanged(object sender, EventArgs e)
    {
        if (ViewState["OVERDUE"].ToString() == "0") { ViewState["OVERDUE"] = "1"; } else { ViewState["OVERDUE"] = "0"; }
        gvDrillSchedulelist.Rebind();
    }
}
