using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionTrainingIndividualReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingIndividualReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvindvTrainingreport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        TabstripTrainingreportmenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            

          
            ViewState["VESSELID"] = Request.QueryString["vessel"];
            ViewState["TRAININGID"] = Request.QueryString["Training"];
            ViewState["YEAR"] = Request.QueryString["year"];
            ViewState["MONTH"] = Request.QueryString["month"];
            ViewState["SCENARIO"] = Request.QueryString["scenario"];
            gvindvTrainingreport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }


    }

    protected void Trainingreport_TabStripCommand(object sender, EventArgs e)
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

    protected void ShowExcel()
    {
        int rowcount = 0;
        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        string scenario = General.GetNullableString(ViewState["SCENARIO"].ToString());
        Guid? Trainingid = General.GetNullableGuid(ViewState["TRAININGID"].ToString());
        int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());
        int? month = General.GetNullableInteger(ViewState["MONTH"].ToString());
        DataTable dt = PhoenixInspectionTrainingLog.IndividualTrainingReport(vesselid, scenario, Trainingid, year, month, ref rowcount, gvindvTrainingreport.CurrentPageIndex+1, gvindvTrainingreport.PageSize);

       
        string[] alColumns = { "FLDVESSELNAME", "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDLASTDONEDATE", "FLDREASON" };
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date", "Reason for no attachments" };

        General.ShowExcel("Individual Training Log", dt, alColumns, alCaptions, null, null);

    }

    protected void gvindvTrainingreport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int rowcount = 0;
        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        string scenario = General.GetNullableString(ViewState["SCENARIO"].ToString());
        Guid? Trainingid = General.GetNullableGuid(ViewState["TRAININGID"].ToString());
        int? year = General.GetNullableInteger(ViewState["YEAR"].ToString());
        int? month = General.GetNullableInteger(ViewState["MONTH"].ToString());
        DataTable dt = PhoenixInspectionTrainingLog.IndividualTrainingReport(vesselid, scenario, Trainingid, year, month, ref rowcount, gvindvTrainingreport.CurrentPageIndex + 1, gvindvTrainingreport.PageSize);

        gvindvTrainingreport.DataSource = dt;
        gvindvTrainingreport.VirtualItemCount = rowcount;
        string[] alColumns = { "FLDVESSELNAME", "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDSCENARIO", "FLDDESCRIPTION", "FLDREMARKS", "FLDLASTDONEDATE", "FLDREASON" };
        string[] alCaptions = { "Vessel", "Name", "Interval", "Interval type", "Scenario", "Description", "Remarks", "Last done date", "Reason for no attachments" };

        DataSet ds = dt.DataSet;

        General.SetPrintOptions("gvindvTrainingreport", "Training Individual Report", alCaptions, alColumns, ds);

    }
    public void gvindvTrainingreport_ItemDataBound(object sender, GridItemEventArgs e)
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

   
}