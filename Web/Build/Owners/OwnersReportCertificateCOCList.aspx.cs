using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Owners;

public partial class OwnersReportCertificateCOCList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            gvSurveyCOC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);            
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Owners/OwnersReportCertificateCOCList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSurveyCOC')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuSurveyCOC.AccessRights = this.ViewState;
        MenuSurveyCOC.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SELECTEDINDEX"] = 0;

            gvSurveyCOC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //BindData();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            BindData();
            gvSurveyCOC.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSURVEYTYPENAME", "FLDCERTIFICATENAME", "FLDDUEDATE", "FLDITEMTOOLTIP", "FLDDESCRIPTIONTOOLTIP", "FLDSTATUSDESC", "FLDCOMPLETEDDATE", "FLDCOMPLETEDREMARKSTOOLTIP" };
        string[] alCaptions = { "Survey Type", "Certificate", "Due", "COC", "Description", "Status", "Completed", "Completed Remarks" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixOwnerReportQuality.OwnersCertificatesCOC(General.GetNullableInteger(Filter.SelectedOwnersReportVessel)
           , General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
               , 1
               , iRowCount
               , ref iRowCount
               , ref iTotalPageCount
               );

        General.ShowExcel("Survey COC List", ds.Tables[0], alColumns, alCaptions, null, "");
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSURVEYTYPENAME", "FLDCERTIFICATENAME", "FLDDUEDATE", "FLDITEMTOOLTIP", "FLDDESCRIPTIONTOOLTIP", "FLDSTATUSDESC", "FLDCOMPLETEDDATE", "FLDCOMPLETEDREMARKSTOOLTIP" };
        string[] alCaptions = { "Survey Type", "Certificate", "Due", "COC", "Description", "Status", "Completed", "Completed Remarks" };

        try
        {
            DataSet ds = PhoenixOwnerReportQuality.OwnersCertificatesCOC(General.GetNullableInteger(Filter.SelectedOwnersReportVessel)
            , General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
                , gvSurveyCOC.CurrentPageIndex + 1
               , gvSurveyCOC.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );
            General.SetPrintOptions("gvSurveyCOC", "Survey COC List", alCaptions, alColumns, ds);
            gvSurveyCOC.DataSource = ds;
            gvSurveyCOC.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            if (General.GetNullableString(Filter.SelectedOwnersReportLockedYN.ToString()).Equals("1"))
            {
                gvSurveyCOC.Columns[8].Visible = false;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuSurveyCOC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName == "EXCEL")
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.VesselSurveyCOCFilter = null;
                BindData();
                gvSurveyCOC.Rebind();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCOCFilter.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSurveyCOC_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        GridView _gridview = (GridView)sender;
        int CurrentIndex = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {

        }
    }

    protected void gvSurveyCOC_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvSurveyCOC_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem dataBoundItem = e.Item as GridDataItem;

            //LinkButton lblSurveyNo = (LinkButton)e.Row.FindControl("lblSurveyNo");
            RadLabel lblVesselId = item.FindControl("lblVesselId") as RadLabel;
            RadLabel lblSheduleId = item.FindControl("lblSheduleId") as RadLabel;
            RadLabel lblStatusCode = item.FindControl("lblStatusCode") as RadLabel;
            RadLabel lblCertificateCOCId = item.FindControl("lblCertificateCOCId") as RadLabel;
            RadLabel lblDescription = item.FindControl("lblDescription") as RadLabel;
            LinkButton cmdComplete = item.FindControl("cmdComplete") as LinkButton;
            LinkButton cmdSvyAtt = item["Action"].FindControl("cmdSvyAtt") as LinkButton;
            RadLabel lblCOC = item.FindControl("lblCOC") as RadLabel;
            RadLabel lblDtkey = item.FindControl("lblDtkey") as RadLabel;
            RadLabel lblIsAtt = item.FindControl("lblIsAtt") as RadLabel;
            RadLabel lblCompletedRemarks = item.FindControl("lblCompletedRemarks") as RadLabel;

            if (lblIsAtt.Text != "YES")
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                cmdSvyAtt.Controls.Add(html);
            }
            cmdComplete.Visible = lblStatusCode.Text.Trim().Equals("3") ? false : true;//Completed
            string sScript = "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyCOCComplete.aspx?CertificateCOCId=" + lblCertificateCOCId.Text.Trim()
                       + "');";
            cmdComplete.Attributes.Add("onclick", sScript);

            if (cmdSvyAtt != null)
                cmdSvyAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text.Trim() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=SURVEYCOC'); return false;");

            if (drv["FLDDUEDATE"].ToString() != string.Empty && drv["FLDCOMPLETEDDATE"].ToString() == string.Empty)
            {
                DateTime dt = DateTime.Parse(drv["FLDDUEDATE"].ToString());
                if (DateTime.Now > dt)
                    dataBoundItem["DueDate"].Attributes.Add("style", "background-color:#ff4d4d");

                if (DateTime.Now >= dt.AddDays(-15) && DateTime.Now <= dt)
                    dataBoundItem["DueDate"].Attributes.Add("style", "background-color:#FFFFAA");

            }
        }
    }
}