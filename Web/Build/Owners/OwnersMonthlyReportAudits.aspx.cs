using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportAudits : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        rdauditinspection.Visible = SessionUtil.CanAccess(this.ViewState, "AUDITINSPECTION");
        rdkpi.Visible = SessionUtil.CanAccess(this.ViewState, "KPI");
        rdtask.Visible = SessionUtil.CanAccess(this.ViewState, "TASK");
        rddeficiency.Visible = SessionUtil.CanAccess(this.ViewState, "DEFICIENCY");
        rdauditinspectionlog.Visible = SessionUtil.CanAccess(this.ViewState, "AUDITINSPECTIONLOG");

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if(!IsPostBack)
        {
            lnkAuditInspection.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Audit / Inspection', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportAuditList.aspx');");
            lnkAuditInspectionComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Audit / Inspection', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=AOI');return false; ");
            lnkDeficiencyComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Deficiency', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=DEF');return false; ");
            lnkTaskComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Task', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=TSK');return false; ");
            lnkKPIsComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'KPI', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=KPI');return false; ");
            lnkAuditInspectionComments2.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Audit / Inspection', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=TI2');return false; ");

            lnkAuditInspectionInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Audit / Inspection','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=AOI" + "',false, 320, 250,'','',options); return false;");
            lnkDeficiencyInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Deficiency','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=DEF" + "',false, 320, 250,'','',options); return false;");
            lnkTaskInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Task','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=TSK" + "',false, 320, 250,'','',options); return false;");
            lnkKPIsInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','KPI','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=KPI" + "',false, 320, 250,'','',options); return false;");
            lnkAuditInspectionInfo2.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Audit / Inspection','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=TI2" + "',false, 320, 250,'','',options); return false;");
            
            CheckComments();
        }
    }
    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("AOI", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkAuditInspectionComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("KPI", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkKPIsComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("DEF", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkDeficiencyComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("TSK", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkTaskComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("TI2", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkAuditInspectionComments2.Controls.Add(html);
        }
    }
    protected void gvInspectionStatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportAuditCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvInspectionStatus.DataSource = dt;

        if (dt.Rows.Count > 0)
        {

            if (dt.Rows[0]["FLD60VISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[1].Visible = false;

            if (dt.Rows[0]["FLDOVRVISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[2].Visible = false;

            if (dt.Rows[0]["FLDCMPVISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[3].Visible = false;

            if (dt.Rows[0]["FLDREVOVDVISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[4].Visible = false;

            if (dt.Rows[0]["FLDREVVISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[5].Visible = false;

            if (dt.Rows[0]["FLDCLDOVDVISIBLE"].ToString().Equals("0"))
                gvInspectionStatus.Columns[6].Visible = false;
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            if ((PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE")))
            {
                Response.Redirect("../Inspection/InspectionAuditOfficeVesselScheduleFilter.aspx");
            }
            else
            {
                Response.Redirect("../Inspection/InspectionAuditScheduleFilter.aspx");
            }
        }
        if (CommandName.ToUpper().Equals("OFFICELIST"))
        {
            Response.Redirect("../Inspection/InspectionAuditOfficeRecordList.aspx");
        }
    }

    protected void gvInspectionStatus_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton lnk60count = (LinkButton)e.Item.FindControl("lnk60count");
            LinkButton lnkOverduecount = (LinkButton)e.Item.FindControl("lnkOverduecount");
            LinkButton lnkCompleted = (LinkButton)e.Item.FindControl("lnkCompleted");
            LinkButton lnkReviewOverduecount = (LinkButton)e.Item.FindControl("lnkReviewOverduecount");
            LinkButton lnkReviewedcount = (LinkButton)e.Item.FindControl("lnkReviewedcount");
            LinkButton lnkClosureOverduecount = (LinkButton)e.Item.FindControl("lnkClosureOverduecount");

            RadLabel lbl60url = (RadLabel)e.Item.FindControl("lbl60url");
            RadLabel lblOverdueurl = (RadLabel)e.Item.FindControl("lblOverdueurl");
            RadLabel lblCompletedurl = (RadLabel)e.Item.FindControl("lblCompletedurl");
            RadLabel lblReviewOverdueurl = (RadLabel)e.Item.FindControl("lblReviewOverdueurl");
            RadLabel lblReviewedurl = (RadLabel)e.Item.FindControl("lblReviewedurl");
            RadLabel lblClosureOverdueurl = (RadLabel)e.Item.FindControl("lblClosureOverdueurl");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if (lnk60count != null)
            {
                lnk60count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 60 Days','" + lbl60url.Text + "'); return false;");
            }

            if (lnkOverduecount != null)
            {
                lnkOverduecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Overdue','" + lblOverdueurl.Text + "'); return false;");
                if (!string.IsNullOrEmpty(dr["FLDOVERCOUNT"].ToString()) && int.Parse(dr["FLDOVERCOUNT"].ToString()) > 0)
                    lnkOverduecount.ForeColor = System.Drawing.Color.Red;
            }

            if (lnkCompleted != null)
            {
                lnkCompleted.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Completed','" + lblCompletedurl.Text + "'); return false;");
            }

            if (lnkReviewOverduecount != null)
            {
                lnkReviewOverduecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Review Overdue','" + lblReviewOverdueurl.Text + "'); return false;");
                if (!string.IsNullOrEmpty(dr["FLDREVOVDCOUNT"].ToString()) && int.Parse(dr["FLDREVOVDCOUNT"].ToString()) > 0)
                    lnkReviewOverduecount.ForeColor = System.Drawing.Color.Red;
            }

            if (lnkReviewedcount != null)
            {
                lnkReviewedcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Reviewed','" + lblReviewedurl.Text + "'); return false;");
            }

            if (lnkClosureOverduecount != null)
            {
                lnkClosureOverduecount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Closure Overdue','" + lblClosureOverdueurl.Text + "'); return false;");
                if (!string.IsNullOrEmpty(dr["FLDCLDOVDCOUNT"].ToString()) && int.Parse(dr["FLDCLDOVDCOUNT"].ToString()) > 0)
                    lnkClosureOverduecount.ForeColor = System.Drawing.Color.Red;
            }


        }
    }

    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportDeficiencyCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvDeficiency.DataSource = dt;

        if (dt.Rows.Count > 0)
        {

            if (dt.Rows[0]["FLDSHIPVISIBLE"].ToString().Equals("0"))
                gvDeficiency.Columns[1].Visible = false;
        }

    }

    protected void gvDeficiency_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton lnkShip = (LinkButton)e.Item.FindControl("lnkShip");


            RadLabel lblShip = (RadLabel)e.Item.FindControl("lblShip");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");            

            if (lnkShip != null)
            {
                lnkShip.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Ship - " + lblmeasure.Text + "','" + lblShip.Text + "'); return false;");
                if (!string.IsNullOrEmpty(dr["FLDSHIPCOUNT"].ToString()) && dr["FLDMEASURE"].ToString().ToLower().Contains("overdue"))
                {
                    if (int.Parse(dr["FLDSHIPCOUNT"].ToString()) > 0)
                        lnkShip.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }





    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        CheckComments();
    }

    protected void GVKPI_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportKPICount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        GVKPI.DataSource = dt;
    }

    protected void GVKPI_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lnkShip = (LinkButton)e.Item.FindControl("lnkShip");


            RadLabel lblShip = (RadLabel)e.Item.FindControl("lblShip");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if (lnkShip != null)
            {
                lnkShip.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','KPI - " + lblmeasure.Text + "','" + lblShip.Text + "'); return false;");
            }
        }
    }

    protected void gvTechTask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportTaaskCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvTechTask.DataSource = dt;

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDOVRVISIBLE"].ToString().Equals("0"))
                gvTechTask.Columns[1].Visible = false;

            if (dt.Rows[0]["FLD30VISIBLE"].ToString().Equals("0"))
                gvTechTask.Columns[2].Visible = false;

            if (dt.Rows[0]["FLDPENVISIBLE"].ToString().Equals("0"))
                gvTechTask.Columns[3].Visible = false;

            if (dt.Rows[0]["FLDEXTVISIBLE"].ToString().Equals("0"))
                gvTechTask.Columns[4].Visible = false;

            if (dt.Rows[0]["FLDPSAVISIBLE"].ToString().Equals("0"))
                gvTechTask.Columns[5].Visible = false;
        }
    }

    protected void gvTechTask_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton lnkOverdue = (LinkButton)e.Item.FindControl("lnkOverdue");
            LinkButton lnk2ryPndg = (LinkButton)e.Item.FindControl("lnk2ryPndg");
            LinkButton lnkExtnReq = (LinkButton)e.Item.FindControl("lnkExtnReq");
            LinkButton lnk30Days = (LinkButton)e.Item.FindControl("lnk30Days");
            LinkButton lnkPndgClosure = (LinkButton)e.Item.FindControl("lnkPndgClosure");

            RadLabel lblOverdueurl = (RadLabel)e.Item.FindControl("lblOverdueurl");
            RadLabel lbl2ryPndg = (RadLabel)e.Item.FindControl("lbl2ryPndg");
            RadLabel lblExtnReq = (RadLabel)e.Item.FindControl("lblExtnReq");
            RadLabel lblPndgClosure = (RadLabel)e.Item.FindControl("lblPndgClosure");
            RadLabel lbl30Daysurl = (RadLabel)e.Item.FindControl("lbl30Daysurl");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if (lnkOverdue != null)
            {
                lnkOverdue.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Overdue','" + lblOverdueurl.Text + "'); return false;");
                if (!string.IsNullOrEmpty(dr["FLDOVERDUECOUNT"].ToString()) && int.Parse(dr["FLDOVERDUECOUNT"].ToString()) > 0)
                    lnkOverdue.ForeColor = System.Drawing.Color.Red;
            }

            if (lnk2ryPndg != null)
            {
                lnk2ryPndg.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Pending Secondary Approval','" + lbl2ryPndg.Text + "'); return false;");
            }

            if (lnkExtnReq != null)
            {
                lnkExtnReq.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Extension Requested','" + lblExtnReq.Text + "'); return false;");
            }

            if (lnk30Days != null)
            {
                lnk30Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 30 Days','" + lbl30Daysurl.Text + "'); return false;");
            }

            if (lnkPndgClosure != null)
            {
                lnkPndgClosure.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Pending Closure','" + lblPndgClosure.Text + "'); return false;");
            }
        }
    }

    protected void gvAuditRecordList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportQuality.OwnersReportAuditSummary(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvAuditRecordList.DataSource = dt;
    }

    protected void gvAuditRecordList_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }

    protected void gvAuditRecordList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            string refno = ((RadLabel)e.Item.FindControl("lblInspectionRefNo")).Text;
            string auditscheduleid = ((RadLabel)e.Item.FindControl("lblInspectionScheduleId")).Text;
            LinkButton lnkInspection = (LinkButton)e.Item.FindControl("lnkInspection");

            if (lnkInspection != null)
            {
                lnkInspection.Attributes.Add("onclick", "javascript:top.openNewWindow('Report','','" + Session["sitepath"] + "/Inspection/InspectionAuditRecordGeneral.aspx?AUDITSCHEDULEID=" + auditscheduleid + "&reffrom=log'); return false;");
            }
        }
    }
}