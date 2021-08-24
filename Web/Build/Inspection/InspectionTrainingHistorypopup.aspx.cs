using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;


public partial class Inspection_InspectionTrainingHistorypopup : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingHistorypopup.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTrainingHistorypopuplist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        TabstripTraininghistoy.MenuList = toolbargrid.Show();


        if (!IsPostBack)
        {
            
           

            ViewState["TRAININGSCHEDULEID"] = Request.QueryString["trainingscheduleid"].ToString();

            gvTrainingHistorypopuplist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDTRAININGONBOARDLASTDONEDATE" };
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date" };


        Guid? Trainingscheduleid = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());

        DataTable dt = PhoenixInspectionTrainingHistory.TrainingHistoryPopupSearch(Trainingscheduleid,
                                               gvTrainingHistorypopuplist.CurrentPageIndex + 1,
                                               gvTrainingHistorypopuplist.PageSize,
                                               ref iRowCount
                                               );




        gvTrainingHistorypopuplist.DataSource = dt;
        gvTrainingHistorypopuplist.VirtualItemCount = iRowCount;

        General.ShowExcel("Training History Across Vessels", dt, alColumns, alCaptions, null, null);


    }


    protected void Traininghistory_TabStripCommand(object sender, EventArgs e)
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
    public void gvTrainingHistorypopuplist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

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
                            + PhoenixModule.QUALITY + "&u=n"+"'); return false;");
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

    protected void gvTrainingHistorypopuplist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDTRAININGONBOARDLASTDONEDATE" };
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date" };


        Guid? Trainingscheduleid = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());

        DataTable dt = PhoenixInspectionTrainingHistory.TrainingHistoryPopupSearch(Trainingscheduleid,
                                               gvTrainingHistorypopuplist.CurrentPageIndex + 1,
                                               gvTrainingHistorypopuplist.PageSize,
                                               ref iRowCount
                                               );




        gvTrainingHistorypopuplist.DataSource = dt;
        gvTrainingHistorypopuplist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;

        General.SetPrintOptions("gvTrainingHistorypopuplist", "Training History Across Vessels", alCaptions, alColumns, ds);
    }

}