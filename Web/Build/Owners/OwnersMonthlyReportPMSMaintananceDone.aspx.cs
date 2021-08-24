using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportPMSMaintananceDone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        rdMaintenance.Visible = SessionUtil.CanAccess(this.ViewState, "MAINTENANCEDONE");
        rdMaintenanceDue.Visible = SessionUtil.CanAccess(this.ViewState, "MAINTENANCEDUE");
        rdPostponementJob.Visible = SessionUtil.CanAccess(this.ViewState, "POSTPONEMENTJOB");

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            lnkMADone.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Maintenance Done', '" + Session["sitepath"] + "/Owners/OwnersReportWorkOrderReportList.aspx',500,900); return false;");
            lnkMADue.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Maintenance Due', '" + Session["sitepath"] + "/Owners/OwnersReportPmsMaitenanceDue.aspx?PostponedYN=0',500,900); return false;");
            lnkMAPost.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Postponed Jobs', '" + Session["sitepath"] + "/Owners/OwnersReportPmsMaitenanceDue.aspx?PostponedYN=1',500,900); return false;");
            lnkMADoneComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Maintenance Done', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=MDN');return false; ");
            lnkMADueComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Maintenance Due', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=MDU');return false; ");
            lnkMAPostComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Postponed Jobs', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=PPJ');return false; ");
            CheckComments();
        }
    }
    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("MDN", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkMADoneComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("MDU", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkMADueComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("PPJ", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkMAPostComments.Controls.Add(html);
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        CheckComments();
    }

    protected void gvMaintenance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportPMS.OwnersReportWorkDone(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvMaintenance.DataSource = dt;
    }

    protected void gvMaintenanceDue_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportPMS.OwnersReportWorkDue(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvMaintenanceDue.DataSource = dt;
    }

    protected void gvMaintenanceDue_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (drv["FLDWORKORDERSTATUS"].ToString().ToUpper() == "2ND POSTPONED" || drv["FLDWORKORDERSTATUS"].ToString().ToUpper() == "POSTPONED")
            {
                //item.BackColor = System.Drawing.Color.Yellow;
                item.Attributes["style"] = "background-color: yellow !important;";
            }

            
            LinkButton lblGroupNo = (LinkButton)e.Item.FindControl("lnkGroupNo");
            if (lblGroupNo != null)
            {
                if (drv["FLDWORKORDERGROUPID"] != null)
                {
                    lblGroupNo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + "&vslid=" + Filter.SelectedOwnersReportVessel + "'); return false;");
                }
            }
            LinkButton lnkpostpone = (LinkButton)e.Item.FindControl("cmdReschedule");
            if (lnkpostpone != null)
            {
                lnkpostpone.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + Filter.SelectedOwnersReportVessel + "'); return false;");
            }
            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");
            if (lnkTitle != null)
            {
                string cjid = drv["FLDCOMPONENTJOBID"].ToString();
                if (General.GetNullableGuid(cjid).HasValue && General.GetNullableGuid(cjid).Value != Guid.Empty)
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&Cancelledjob=0&vesselid="+ Filter.SelectedOwnersReportVessel + "'); ");
                else
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "&vesselid=" + Filter.SelectedOwnersReportVessel + "','','1200','600');return false");

                //lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?tv=1&COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&hierarchy=1&Cancelledjob=0'); ");
                if (General.GetNullableInteger(drv["FLDISCRITICAL"].ToString()) == 1)
                    lnkTitle.Text = "<font color=red>" + drv["FLDWORKORDERNAME"].ToString() + "</font>";
                else
                    lnkTitle.Text = drv["FLDWORKORDERNAME"].ToString();
            }
            LinkButton docking = (LinkButton)e.Item.FindControl("cmdDocking");
            if (docking != null)
            {
                docking.Attributes.Add("onclick", "javascript:openNewWindow('DOCING','Add To Drydock','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWODocking.aspx?woid=" + drv["FLDWORKORDERID"] + "&vesselid=" + Filter.SelectedOwnersReportVessel + "'); return false;");
            }
        }
    }

    protected void gvPostponementJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportPMS.OwnersReportPostponementJob(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvPostponementJob.DataSource = dt;
    }

    protected void gvPostponementJob_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (drv["FLDWORKORDERSTATUS"].ToString().ToUpper() == "2ND POSTPONED" || drv["FLDWORKORDERSTATUS"].ToString().ToUpper() == "POSTPONED")
            {
                //item.BackColor = System.Drawing.Color.Yellow;
                item.Attributes["style"] = "background-color: yellow !important;";
            }

            if (drv["FLDWORKORDERGROUPID"].ToString() != "")
            {
                //CheckBox checkBox = (CheckBox)item["ClientSelectColumn"].Controls[0];
                //checkBox.Enabled = false;
                //item.SelectableMode = GridItemSelectableMode.None;
                //item["ClientSelectColumn"].Attributes.Add( = GridItemSelectableMode.None;
            }
            LinkButton lblGroupNo = (LinkButton)e.Item.FindControl("lnkGroupNo");
            if (lblGroupNo != null)
            {
                if (drv["FLDWORKORDERGROUPID"] != null)
                {
                    lblGroupNo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + "&vslid=" + Filter.SelectedOwnersReportVessel + "'); return false;");
                }
            }
            LinkButton lnkpostpone = (LinkButton)e.Item.FindControl("cmdReschedule");
            if (lnkpostpone != null)
            {
                lnkpostpone.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + Filter.SelectedOwnersReportVessel + "'); return false;");
            }
            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");
            if (lnkTitle != null)
            {
                string cjid = drv["FLDCOMPONENTJOBID"].ToString();
                if (General.GetNullableGuid(cjid).HasValue && General.GetNullableGuid(cjid).Value != Guid.Empty)
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&Cancelledjob=0&vesselid=" + Filter.SelectedOwnersReportVessel + "'); ");
                else
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "&vesselid=" + Filter.SelectedOwnersReportVessel + "','','1200','600');return false");

                //lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?tv=1&COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&hierarchy=1&Cancelledjob=0'); ");
                if (General.GetNullableInteger(drv["FLDISCRITICAL"].ToString()) == 1)
                    lnkTitle.Text = "<font color=red>" + drv["FLDWORKORDERNAME"].ToString() + "</font>";
                else
                    lnkTitle.Text = drv["FLDWORKORDERNAME"].ToString();
            }
            LinkButton docking = (LinkButton)e.Item.FindControl("cmdDocking");
            if (docking != null)
            {
                docking.Attributes.Add("onclick", "javascript:openNewWindow('DOCING','Add To Drydock','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWODocking.aspx?woid=" + drv["FLDWORKORDERID"] + "&vesselid=" + Filter.SelectedOwnersReportVessel + "'); return false;");
            }
        }
    }

    protected void gvMaintenance_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton RA = (LinkButton)e.Item.FindControl("cmdRA");
            if (RA != null)
            {
                RA.Attributes.Add("onclick", "javascript:openNewWindow('Risk Assessment','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRAList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + Filter.SelectedOwnersReportVessel + "',500,600); return false;");
                RA.Visible = SessionUtil.CanAccess(this.ViewState, RA.CommandName);
            }
            LinkButton cmdReschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
            if (cmdReschedule != null)
            {
                cmdReschedule.Attributes.Add("onclick", "javascript:openNewWindow('Postpone','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?LOG=Y&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + Filter.SelectedOwnersReportVessel + "',500,600); return false;");

            }
            ImageButton cmdAttachments = (ImageButton)e.Item.FindControl("cmdAttachments");
            if (cmdAttachments != null)
            {
                cmdAttachments.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachments.CommandName);
                cmdAttachments.Attributes.Add("onclick", "javascript:openNewWindow('Attachments','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?DTKEY=" + drv["FLDDTKEY"] + "&MOD=PLANNEDMAINTENANCE',500,600); return false;");

            }

            LinkButton cmdRTemplates = (LinkButton)e.Item.FindControl("cmdRTemplates");
            if (cmdRTemplates != null)
            {
                cmdRTemplates.Attributes.Add("onclick", "javascript:openNewWindow('Reporting Templates','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogHistoryTemplate.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + Filter.SelectedOwnersReportVessel + "',500,600); return false;");

            }
            LinkButton cmdParts = (LinkButton)e.Item.FindControl("cmdParts");
            if (cmdParts != null)
            {
                cmdParts.Attributes.Add("onclick", "javascript:openNewWindow('Parts','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogUsesParts.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + Filter.SelectedOwnersReportVessel + "',500,600); return false;");

            }
            LinkButton cmdParameters = (LinkButton)e.Item.FindControl("cmdParameters");
            if (cmdParameters != null)
            {
                cmdParameters.Attributes.Add("onclick", "javascript:openNewWindow('Parameters','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogParameterList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + Filter.SelectedOwnersReportVessel + "',500,600); return false;");

            }
            LinkButton cmdPTW = (LinkButton)e.Item.FindControl("cmdPTW");
            if (cmdPTW != null)
            {
                cmdPTW.Attributes.Add("onclick", "javascript:openNewWindow('PTW','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogPTW.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + Filter.SelectedOwnersReportVessel + "',500,600); return false;");

            }

            if (General.GetNullableInteger(drv["FLDRAYN"].ToString()) == 1)
            {
                RA.Visible = SessionUtil.CanAccess(this.ViewState, RA.CommandName);
            }
            else
                RA.Visible = false;

            if (General.GetNullableInteger(drv["FLDTEMPLATE"].ToString()) == 1)
                cmdRTemplates.Visible = SessionUtil.CanAccess(this.ViewState, cmdRTemplates.CommandName);
            else
                cmdRTemplates.Visible = false;

            if (drv["FLDATTACHMENTCODE"].ToString() == "0")
                cmdAttachments.ImageUrl = Session["images"] + "/no-attachment.png";

            if (General.GetNullableInteger(drv["FLDPARAMETERSYN"].ToString()) == 1)
                cmdParameters.Visible = SessionUtil.CanAccess(this.ViewState, cmdParameters.CommandName);
            else
                cmdParameters.Visible = false;
            if (General.GetNullableInteger(drv["FLDPTWYN"].ToString()) == 1)
                cmdPTW.Visible = SessionUtil.CanAccess(this.ViewState, cmdPTW.CommandName);
            else
                cmdPTW.Visible = false;

            if (General.GetNullableInteger(drv["FLDRESCHEDULEYN"].ToString()) == 1)
                cmdReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdReschedule.CommandName);
            else
                cmdReschedule.Visible = false;

            if (General.GetNullableInteger(drv["FLDPARTSYN"].ToString()) == 1)
                cmdParts.Visible = SessionUtil.CanAccess(this.ViewState, cmdParts.CommandName);
            else
                cmdParts.Visible = false;

            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");

            if (lnkTitle != null)
            {
                if (General.GetNullableGuid(drv["FLDCOMPONENTJOBID"].ToString()) != null)
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"].ToString() + "&COMPONENTID=" + drv["FLDCOMPONENTID"].ToString() + "&Cancelledjob=0&vesselid=" + Filter.SelectedOwnersReportVessel + "','','1200','600');return false");
                else
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "&vesselid=" + Filter.SelectedOwnersReportVessel + "','','1200','600');return false");
            }

        }
    }
}