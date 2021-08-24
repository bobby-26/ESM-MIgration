using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportPMSPostponedJobView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

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
                CheckBox checkBox = (CheckBox)item["ClientSelectColumn"].Controls[0];
                checkBox.Enabled = false;
                //item.SelectableMode = GridItemSelectableMode.None;
                //item["ClientSelectColumn"].Attributes.Add( = GridItemSelectableMode.None;
            }
            LinkButton lblGroupNo = (LinkButton)e.Item.FindControl("lnkGroupNo");
            if (lblGroupNo != null)
            {
                if (drv["FLDWORKORDERGROUPID"] != null)
                {
                    lblGroupNo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + "'); return false;");
                }
            }
            LinkButton lnkpostpone = (LinkButton)e.Item.FindControl("cmdReschedule");
            if (lnkpostpone != null)
            {
                lnkpostpone.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return false;");
            }
            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");
            if (lnkTitle != null)
            {
                string cjid = drv["FLDCOMPONENTJOBID"].ToString();
                if (General.GetNullableGuid(cjid).HasValue && General.GetNullableGuid(cjid).Value != Guid.Empty)
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&Cancelledjob=0'); ");
                else
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "','','1200','600');return false");

                //lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?tv=1&COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&hierarchy=1&Cancelledjob=0'); ");
                if (General.GetNullableInteger(drv["FLDISCRITICAL"].ToString()) == 1)
                    lnkTitle.Text = "<font color=red>" + drv["FLDWORKORDERNAME"].ToString() + "</font>";
                else
                    lnkTitle.Text = drv["FLDWORKORDERNAME"].ToString();
            }
            LinkButton docking = (LinkButton)e.Item.FindControl("cmdDocking");
            if (docking != null)
            {
                docking.Attributes.Add("onclick", "javascript:openNewWindow('DOCING','Add To Drydock','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWODocking.aspx?woid=" + drv["FLDWORKORDERID"] + "'); return false;");
            }
        }
    }
}