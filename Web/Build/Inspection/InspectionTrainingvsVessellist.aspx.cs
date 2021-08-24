using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.WebControls;
using System.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class Inspection_InspectionTrainingvsVessellist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingvsVesselList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTrainingvsVessels')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        TabstripMenu.MenuList = toolbargrid.Show();
        if (!IsPostBack)
        {
           

            ViewState["TYPE"] = Request.QueryString["type"];
            ViewState["DUE"] = Request.QueryString["i"];
            ViewState["DUE1"] = Request.QueryString["j"];
            ViewState["TRAININGID"] = Request.QueryString["Trainingid"];
            ViewState["TRAINING"] = "";
            ViewState["VESSELID"] = "";
            ViewState["DUEIN"] = "";
            ViewState["FROMDATE"] = "";
            ViewState["TODATE"] = "";
            gvTrainingvsVessels.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            if (ViewState["TYPE"].ToString() == "Mandatory")
            {
                DataTable dt = PhoenixRegisterTraining.TrainingEditList(General.GetNullableGuid(ViewState["TRAININGID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    ViewState["TRAINING"] = dt.Rows[0]["FLDTRAININGNAME"].ToString();
                }

            }
        }

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDTRAININGNAME", "FLDVESSELNAME", "FLDTRAININGDUEDATE", "DUEIN", };
        string[] alCaptions = { "Training", "Vessel ", "Due on", "Overdue by" };
        Guid? Trainingid = General.GetNullableGuid(ViewState["TRAININGID"].ToString());
        string type = General.GetNullableString(ViewState["TYPE"].ToString());
       
        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        DateTime? FromDate = General.GetNullableDateTime(ViewState["FROMDATE"].ToString());
        DateTime? ToDate = General.GetNullableDateTime(ViewState["TODATE"].ToString());
        int? DueIn = General.GetNullableInteger(ViewState["DUEIN"].ToString());
        string Training = General.GetNullableString(ViewState["TRAINING"].ToString());
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        DataTable dt = PhoenixInspectionTrainingSummary.TrainingvsVessellist(gvTrainingvsVessels.CurrentPageIndex + 1,
                                                gvTrainingvsVessels.PageSize,  type, 
                                                ref iRowCount,
                                                ref iTotalPageCount
                                                , rowusercode
                                                , Training
                                                , vesselid
                                                , FromDate
                                                , ToDate
                                                , DueIn);
        General.ShowExcel("Overdue Trainings", dt, alColumns, alCaptions, null, null);

    }
    protected void Trainingvsvessels_TabStripMenuCommand(object sender, EventArgs e)
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
    protected void gvTrainingvsVessels_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string type = General.GetNullableString(ViewState["TYPE"].ToString());

        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        DateTime? FromDate = General.GetNullableDateTime(ViewState["FROMDATE"].ToString());
        DateTime? ToDate = General.GetNullableDateTime(ViewState["TODATE"].ToString());
        int? DueIn = General.GetNullableInteger(ViewState["DUEIN"].ToString());
        string Training = General.GetNullableString(ViewState["TRAINING"].ToString());
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        DataTable dt = PhoenixInspectionTrainingSummary.TrainingvsVessellist(gvTrainingvsVessels.CurrentPageIndex + 1,
                                                gvTrainingvsVessels.PageSize, type,
                                                ref iRowCount,
                                                ref iTotalPageCount
                                                , rowusercode
                                                , Training
                                                , vesselid
                                                , FromDate
                                                , ToDate
                                                , DueIn);

        gvTrainingvsVessels.DataSource = dt;
        gvTrainingvsVessels.VirtualItemCount = iRowCount;
        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDTRAININGNAME", "FLDVESSELNAME", "FLDTRAININGDUEDATE", "DUEIN", };
        string[] alCaptions = { "Training", "Vessel ", "Due on", "Overdue by" };
        General.SetPrintOptions("gvTrainingvsVessels", "Overdue Trainings", alCaptions, alColumns, ds);

    }

    protected void gvTrainingvsVessels_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? Trainingscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDTRAININGONBOARDSCHEDULEID").ToString());
                LinkButton vesselname = (LinkButton)item.FindControl("RadlblVesselName");
                vesselname.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + DataBinder.Eval(e.Item.DataItem, "FLDVESSELID") + "');return false");
            }
            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem item = e.Item as GridFilteringItem;
                RadTextBox TrainingName = (RadTextBox)e.Item.FindControl("radtbtraining");
                if (TrainingName != null)
                {
                    TrainingName.Text = ViewState["TRAINING"].ToString();
                }
                RadDropDownList Type = (RadDropDownList)e.Item.FindControl("radtype");
                if (Type != null)
                {
                    Type.SelectedValue = ViewState["TYPE"].ToString();
                }
                UserControlVesselCommon vessel = (UserControlVesselCommon)e.Item.FindControl("ddlvessellist");
                if (vessel != null && ViewState["VESSELID"] != null)
                {
                    vessel.SelectedVessel = ViewState["VESSELID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvTrainingvsVessels_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.CommandName == RadGrid.FilterCommandName)
        {


            ViewState["DUEIN"] = gvTrainingvsVessels.MasterTableView.GetColumn("DUEIN").CurrentFilterValue;



            ViewState["TRAINING"] = ((RadTextBox)(e.Item.FindControl("radtbtraining"))).Text;

            string daterange = grid.MasterTableView.GetColumn("FLDDATE").CurrentFilterValue;
            string Trainingfilters = grid.MasterTableView.GetColumn("FLDTRAININGNAME").CurrentFilterValue;
            if (daterange != "")
            {
                ViewState["FROMDATE"] = daterange.Split('~')[0];
                ViewState["TODATE"] = daterange.Split('~')[1];
            }

            if (Trainingfilters != "")
            {
                ViewState["TRAINING"] = Trainingfilters.Split('~')[0];
                ViewState["TYPE"] = Trainingfilters.Split('~')[1];
            }
            gvTrainingvsVessels.Rebind();

        }
    }
    protected void ddlvessellist_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlVesselCommon vessel = (UserControlVesselCommon)sender;

        ViewState["VESSELID"] = vessel.SelectedVessel;
        gvTrainingvsVessels.Rebind();
    }
}