using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;

public partial class Inspection_InspectionTrainingSchedule : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingSchedule.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTrainingSchedulelist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            toolbargrid.AddFontAwesomeButton("javascript:radconfirm('You are about to create Training schedule for this vessel.Continue ? ', confirmCallBackFn, 320, 150, null, 'Confirm');", "Refresh", "<i class=\"fas fa-sync\"></i>", "REFRESH");
        }
        TabstripMenu.MenuList = toolbargrid.Show();

        PhoenixToolbar menu = new PhoenixToolbar();
        menu.AddButton("Training Planner & Record", "Toggle4", ToolBarDirection.Right);
        menu.AddButton("History", "Toggle3", ToolBarDirection.Right);
        menu.AddButton("Training Schedule", "Toggle2", ToolBarDirection.Right);
        TabstripTrainingschedule.MenuList = menu.Show();
        TabstripTrainingschedule.SelectedMenuIndex = 2;

        int currentvesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        ViewState["CURRENTVESSELID"] = currentvesselid.ToString();

        if (!IsPostBack)
        {
           
            ViewState["TRAINING"] = string.Empty;
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

            gvTrainingSchedulelist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDFIXEDORVARIABLE", "FLDTYPE", "FLDTRAININGONBOARDDUEDATE", "FLDTRAININGONBOARDLASTDONEDATE" };
        string[] alCaptions = { "Name", "Interval", "Interval Type", "Fixed/Variable", "Type", "Due Date", "Last done Date" };

        int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());



        DataSet ds = PhoenixInspectionTrainingSchedule.TrainingScheduleSearch(General.GetNullableString(ViewState["TRAINING"].ToString()),
                                                vesselid,
                                                General.GetNullableInteger(ViewState["OVERDUE"].ToString()),
                                                General.GetNullableString(ViewState["SORTEXPRESSION"].ToString()),
                                                General.GetNullableInteger(ViewState["SORTDIRECTION"].ToString()),
                                                gvTrainingSchedulelist.CurrentPageIndex + 1,
                                                gvTrainingSchedulelist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        General.ShowExcel("Training Schedule", ds.Tables[0], alColumns, alCaptions, null, null);

    }
    protected void TabstripMenu_TabStripMenuCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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
    protected void TabstripTrainingschedulee_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("TOGGLE2"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionTrainingSchedule.aspx?vesselid=" + vesselid);
                }
                else
                    Response.Redirect("~/Inspection/InspectionTrainingSchedule.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE3"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionTrainingHistory.aspx?vesselid=" + vesselid);
                }
                else

                    Response.Redirect("~/Inspection/InspectionTrainingHistory.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE4"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionTrainingReport.aspx?vesselid=" + vesselid);
                }
                else

                    Response.Redirect("~/Inspection/InspectionTrainingReport.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void ddlvessellist_textchange(object sender, EventArgs e)
    {
        gvTrainingSchedulelist.Rebind();
        if (ddlvessellist.SelectedVessel != null)
        {

            ViewState["VESSELID"] = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        }

    }

    public void gvTrainingSchedulelist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            int installcode = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                GridDataItem item = e.Item as GridDataItem;
                int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel);
                Guid?TrainingScheduleId = General.GetNullableGuid(item.GetDataKeyValue("FLDTRAININGONBOARDSCHEDULEID").ToString());
             
                Guid? flddtkey = General.GetNullableGuid(drv["FLDDTKEY"].ToString());
                DateTime? Trainingduedate = General.GetNullableDateTime(drv["OVERDUE"].ToString());
                string submissionstatus = General.GetNullableString(drv["FLDSUBMISSIONSTATUS"].ToString());
                LinkButton history = ((LinkButton)item.FindControl("btnhistory"));
                if (history != null)
                {
                    history.Attributes.Add("onclick", "javascript:parent.openNewWindow('History','','Inspection/InspectionTrainingIndividualHistory.aspx?trainingscheduleid=" + TrainingScheduleId + "&vesselid=" + vesselid + "');return false");

                }

                LinkButton reporttraining = ((LinkButton)item.FindControl("reportbtn"));
                if (reporttraining != null)
                {
                    reporttraining.Attributes.Add("onclick", "javascript:parent.openNewWindow('Report','','Inspection/InspectionTrainingScheduleReport.aspx?Trainingscheduleid=" + TrainingScheduleId + "&vesselid=" + vesselid + "');return false");

                }

                HtmlAnchor TrainingName = (HtmlAnchor)item.FindControl("TrainingNameanchor");

                TrainingName.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','Training Due Across Vessels','Inspection/InspectionTrainingSchedulePopupList.aspx?trainingscheduleid=" + TrainingScheduleId + "');return false");


                int? currentvesselid = General.GetNullableInteger(ViewState["CURRENTVESSELID"].ToString());

                if (currentvesselid > 0)
                {
                    reporttraining.Visible = true;

                }

                LinkButton imgFlag = (LinkButton)e.Item.FindControl("imgFlag");

                if (imgFlag != null)
                {
                    if (submissionstatus == "Draft")
                    {
                        imgFlag.Visible = true;

                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-star-yellow\"></i></span>";
                        imgFlag.Controls.Add(html);
                    }
                    else if (Trainingduedate == null || Trainingduedate < DateTime.Now)
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
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Edit Lastdonedate','','Inspection/InspectionTrainingdoneedit.aspx?trainingscheduleid=" + TrainingScheduleId + "','false','300px','420px');return false");


                }
            }
            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                TextBox textBox = (TextBox)filterItem["FLDTRAININGNAME"].Controls[0];
                textBox.MaxLength = 198;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvTrainingSchedulelist_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvTrainingSchedulelist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDFIXEDORVARIABLE", "FLDTYPE", "FLDTRAININGONBOARDDUEDATE", "FLDTRAININGONBOARDLASTDONEDATE" };
        string[] alCaptions = { "Name", "Interval", "Interval Type", "Fixed/Variable", "Type", "Due Date", "Last done Date" };

        int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());



        DataSet ds = PhoenixInspectionTrainingSchedule.TrainingScheduleSearch(General.GetNullableString(ViewState["TRAINING"].ToString()),
                                                vesselid,
                                                General.GetNullableInteger(ViewState["OVERDUE"].ToString()),
                                                General.GetNullableString(ViewState["SORTEXPRESSION"].ToString()),
                                                General.GetNullableInteger(ViewState["SORTDIRECTION"].ToString()),
                                                gvTrainingSchedulelist.CurrentPageIndex + 1,
                                                gvTrainingSchedulelist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);


        General.SetPrintOptions("gvTrainingSchedulelist", "Training Schedule ", alCaptions, alColumns, ds);

        gvTrainingSchedulelist.DataSource = ds.Tables[0];
        gvTrainingSchedulelist.VirtualItemCount = iRowCount;
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvTrainingSchedulelist.Rebind();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel);
            PhoenixInspectionTrainingSchedule.CreateTrainingSchedule(rowusercode, vesselid);
            gvTrainingSchedulelist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvTrainingSchedulelist_Sort(object source, GridSortCommandEventArgs e)
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
        gvTrainingSchedulelist.Rebind();
    }
}