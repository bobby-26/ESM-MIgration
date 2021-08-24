using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionTrainingIndividualHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingIndividualHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTrainingHistorylist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingIndividualHistory.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");

        Tabstriptraininghistorymenu.MenuList = toolbargrid.Show();
        if (!IsPostBack)
        {
          
            ViewState["VESSELID"] = Request.QueryString["vesselid"];
            ViewState["TRAININGSCHEDULEID"] = Request.QueryString["trainingscheduleid"];
            gvTrainingHistorylist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }


    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string vesselname = string.Empty;
        DateTime? fromdate = General.GetNullableDateTime(tbfromdateentry.Text);
        DateTime? todate = General.GetNullableDateTime(tbtodateentry.Text);

        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        Guid? trainingscheduleid = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());

        DataTable dt = PhoenixInspectionTrainingHistory.TrainingIndividualHistorySearch(vesselid, trainingscheduleid,
                                                gvTrainingHistorylist.CurrentPageIndex + 1,
                                                gvTrainingHistorylist.PageSize, fromdate, todate,
                                                ref iRowCount,
                                                ref iTotalPageCount,
                                                ref vesselname);

       

        string[] alColumns = { "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDTRAININGONBOARDLASTDONEDATE", "FLDREMARKS", "FLDREASON" };
        string[] alCaptions = { "Name", "Interval", "Interval type", "Scenario", "Description", "Done Date", "Remarks", "Reason for no attachments" };

        General.ShowExcel("Training Individual History", dt, alColumns, alCaptions, null, null);

    }

    protected void traininghistorymenu_TabStripCommand(object sender, EventArgs e)
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

                gvTrainingHistorylist.Rebind();
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

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? trainingscheduleid = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());
                DataTable dt = PhoenixInspectionTrainingSchedule.Getflddtkey(trainingscheduleid);
                Guid? flddtkey = General.GetNullableGuid(dt.Rows[0]["FLDDTKEY"].ToString());

                LinkButton db = ((LinkButton)item.FindControl("btnattachments"));
                if (db != null)
                {
                    db.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + flddtkey + "&mod="
                            + PhoenixModule.QUALITY +"&u=n"+ "'); return false;");
                }
                if (General.GetNullableString(dt.Rows[0]["FLDREASON"].ToString()) != null)
                { db.Visible = false; }

            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }


    }
    protected void datechange(object sender, EventArgs e)
    {
        gvTrainingHistorylist.Rebind();

    }
    protected void gvTrainingHistorylist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string vesselname = string.Empty;
        DateTime? fromdate = General.GetNullableDateTime(tbfromdateentry.Text);
        DateTime? todate = General.GetNullableDateTime(tbtodateentry.Text);

        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        Guid? trainingscheduleid = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());

        DataTable dt = PhoenixInspectionTrainingHistory.TrainingIndividualHistorySearch(vesselid, trainingscheduleid,
                                                gvTrainingHistorylist.CurrentPageIndex + 1,
                                                gvTrainingHistorylist.PageSize, fromdate, todate,
                                                ref iRowCount,
                                                ref iTotalPageCount,
                                                ref vesselname);

        DataSet ds = dt.DataSet;

        string[] alColumns = { "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDTRAININGONBOARDLASTDONEDATE", "FLDREMARKS", "FLDREASON" };
        string[] alCaptions = { "Name", "Interval", "Interval type", "Scenario", "Description", "Done Date", "Remarks", "Reason for no attachments" };

        General.SetPrintOptions("gvTrainingHistorylist", "Training Individual History ", alCaptions, alColumns, ds);

        gvTrainingHistorylist.DataSource = dt;
        gvTrainingHistorylist.VirtualItemCount = iRowCount;

    }
}