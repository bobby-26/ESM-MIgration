using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;

public partial class Inspection_InspectionTrainingHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTrainingHistorylist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingHistory.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingHistory.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        TabstripTraininghistorymenu.MenuList = toolbargrid.Show();

        PhoenixToolbar menu = new PhoenixToolbar();
        menu.AddButton("Training Planner & Record", "Toggle4", ToolBarDirection.Right);
        menu.AddButton("History", "Toggle3", ToolBarDirection.Right);
        menu.AddButton("Training Schedule", "Toggle2", ToolBarDirection.Right);

        TabstripTraininghistoy.MenuList = menu.Show();
        TabstripTraininghistoy.SelectedMenuIndex = 1;

        if (!IsPostBack)
        {
           


            gvTrainingHistorylist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            DataTable dt1 = PhoenixRegisterTrainingScenario.TrainingList();

            radcomboTraining.DataSource = dt1;
            radcomboTraining.DataTextField = "FLDTRAININGNAME";
            radcomboTraining.DataValueField = "FLDTRAININGID";
            radcomboTraining.DataBind();

            int currentvesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

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
            else
            {
                ViewState["VESSELID"] = currentvesselid.ToString();
                ddlvessellist.SelectedVessel = currentvesselid.ToString();
                ddlvessellist.Enabled = false;
            }

        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDTRAININGONBOARDLASTDONEDATE", "FLDREASON" };
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date", "Reason for no attachments" };

        Guid? Trainingid = General.GetNullableGuid(radcomboTraining.SelectedValue.ToString());
        DateTime? fromdate = General.GetNullableDateTime(tbfromdateentry.Text);
        DateTime? todate = General.GetNullableDateTime(tbtodateentry.Text);
        int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;

        DataTable dt = PhoenixInspectionTrainingHistory.TrainingHistorySearch(rowusercode ,vesselid, Trainingid,
                                                gvTrainingHistorylist.CurrentPageIndex + 1,
                                                 gvTrainingHistorylist.PageSize, fromdate, todate,
                                                ref iRowCount,
                                                ref iTotalPageCount);

        General.ShowExcel("Training  History", dt, alColumns, alCaptions, null, null);

    }

    protected void Traininghistorymenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                DateTime? fromdate = General.GetNullableDateTime(tbfromdateentry.Text);
                DateTime? todate = General.GetNullableDateTime(tbtodateentry.Text);

                if (!IsValidTrainingHistoryDetails(fromdate, todate))
                {
                    ucError.Visible = true;

                }

                gvTrainingHistorylist.Rebind();
                //if ((General.GetNullableInteger(ddlvessellist.SelectedValue.ToString()))!= null)
                //{

                ViewState["VESSELID"] = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
                //}
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlvessellist.SelectedVessel = string.Empty;
                radcomboTraining.ClearSelection();
                radcomboTraining.Text = string.Empty;
                tbfromdateentry.Text = string.Empty;
                tbtodateentry.Text = string.Empty;
                gvTrainingHistorylist.Rebind();
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
    protected void Traininghistory_TabStripCommand(object sender, EventArgs e)
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
                    Response.Redirect("../Inspection/InspectionTrainingSchedule.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE3"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionTrainingHistory.aspx?vesselid=" + vesselid);
                }
                else

                    Response.Redirect("../Inspection/InspectionTrainingHistory.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE4"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionTrainingReport.aspx?vesselid=" + vesselid);
                }
                else
                    Response.Redirect("../Inspection/InspectionTrainingReport.aspx");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }








    public void gvTrainingHistorylist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            int installcode = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? Trainingscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDTRAININGONBOARDSCHEDULEID").ToString());
                DataTable dt = PhoenixInspectionTrainingSchedule.Getflddtkey(Trainingscheduleid);
                Guid? flddtkey = General.GetNullableGuid(dt.Rows[0]["FLDDTKEY"].ToString());

                LinkButton db = ((LinkButton)item.FindControl("btnattachments"));
                if (db != null)
                {
                    db.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + flddtkey + "&mod="
                            + PhoenixModule.QUALITY +"&u=n"+ "'); return false;");
                }


                HtmlAnchor Trainingname = (HtmlAnchor)item.FindControl("TrainingNameanchor");

                Trainingname.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','Training History Across Vessels','Inspection/InspectionTrainingHistorypopup.aspx?Trainingscheduleid=" + Trainingscheduleid + "');return false");
                if (General.GetNullableString(dt.Rows[0]["FLDREASON"].ToString()) != null)
                { db.Visible = false; }

                if (installcode == 0)
                {

                    LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Report','Training Report','Inspection/InspectionTrainingScheduleReport.aspx?Trainingscheduleid=" + Trainingscheduleid + "&e=y" + "');return false");
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvTrainingHistorylist.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvTrainingHistorylist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDTRAININGONBOARDLASTDONEDATE", "FLDREASON" };
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date", "Reason for no attachments" };

        Guid? Trainingid = General.GetNullableGuid(radcomboTraining.SelectedValue.ToString());
        DateTime? fromdate = General.GetNullableDateTime(tbfromdateentry.Text);
        DateTime? todate = General.GetNullableDateTime(tbtodateentry.Text);
        int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;

        DataTable dt = PhoenixInspectionTrainingHistory.TrainingHistorySearch(rowusercode,vesselid, Trainingid,
                                                gvTrainingHistorylist.CurrentPageIndex + 1,
                                                 gvTrainingHistorylist.PageSize, fromdate, todate,
                                                ref iRowCount,
                                                ref iTotalPageCount);




        gvTrainingHistorylist.DataSource = dt;
        gvTrainingHistorylist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;

        General.SetPrintOptions("gvTrainingHistorylist", "Training History", alCaptions, alColumns, ds);
    }

    private bool IsValidTrainingHistoryDetails(DateTime? Fromdate, DateTime? ToDate)
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


}