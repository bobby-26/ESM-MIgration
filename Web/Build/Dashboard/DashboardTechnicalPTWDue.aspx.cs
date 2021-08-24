using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Dashboard_DashboardTechnicalPTWDue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    protected void gvForms_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataTable dt = PhoenixDashboardTechnical.DashboardPTWDueSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7)
                                        , sortexpression, sortdirection
                                        , grid.CurrentPageIndex + 1
                                        , grid.PageSize
                                        , ref iRowCount
                                        , ref iTotalPageCount);

        grid.DataSource = dt;
        grid.VirtualItemCount = iRowCount;
    }

    protected void gvForms_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataRowView drv = (DataRowView)e.Item.DataItem;        
        LinkButton lnkForm = (LinkButton)e.Item.FindControl("lnkForm");
        LinkButton upload = (LinkButton)e.Item.FindControl("cmdUpload");
        if (lnkForm != null)
        {
            lnkForm.Enabled = false;            
            if (drv["FLDTYPENAME"].ToString().ToUpper() == "UPLOAD")
            {
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(drv["FLDREVISIONDTKEY"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    lnkForm.Enabled = true;
                    DataRow drRow = dt.Rows[0];
                    lnkForm.Attributes.Add("onclick", "javascript:top.openNewWindow('frm','Checklist & Forms - [" + drv["FLDFORMNAME"].ToString() + "]','" + Session["sitepath"] + "/Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString() + "'); return false;");
                }                
            }
            else
            {
                lnkForm.Enabled = true;
                lnkForm.Attributes.Add("onclick", "javascript:top.openNewWindow('frm','Work Permits - [" + drv["FLDFORMNAME"].ToString() + "]','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + drv["FLDFORMID"].ToString() + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "&workorderId=" + drv["FLDWORKORDERID"].ToString() + "'); return false;");
            }
            lnkForm.Visible = SessionUtil.CanAccess(this.ViewState, lnkForm.CommandName);
        }
        if (upload != null)
        {
            if (drv["FLDTYPENAME"].ToString().ToUpper() == "UPLOAD")
            {
                upload.Visible = SessionUtil.CanAccess(this.ViewState, upload.CommandName);                
                upload.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl = '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSShipboardFormsAttachmentUpload.aspx?FORMID=" + drv["FLDFORMID"].ToString().ToString() + "&woid=" + drv["FLDWORKORDERID"].ToString().ToString() + "'; showDialog('Checklist & Forms - [" + drv["FLDFORMNAME"].ToString() + "]');");
            }
            else
            {
                upload.Visible = false;
            }
        }
        LinkButton delete = (LinkButton)e.Item.FindControl("cmdDelete");
        if (delete != null)
        {
            delete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            delete.Visible = SessionUtil.CanAccess(this.ViewState, delete.CommandName);
        }
        LinkButton lblGroupNo = (LinkButton)e.Item.FindControl("lnkGroupNo");
        if (lblGroupNo != null)
        {
            string vslid = "";
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            {
                vslid = "&vslid=" + drv["FLDVESSELID"].ToString();
            }
            if (drv["FLDWORKORDERGROUPID"] != null)
            {
                lblGroupNo.Attributes.Add("onclick", "javascript:openNewWindow('maint','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + vslid + "'); return false;");
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvForms.Rebind();
    }

    protected void gvForms_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "DELETE")
            {
                string wogroupid = ((RadLabel)e.Item.FindControl("lblWOGroupId")).Text;
                string jhamapid = ((RadLabel)e.Item.FindControl("lblJHAMapId")).Text;

                PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubWorkorderPtwDelete(new Guid(jhamapid));

                if (General.GetNullableGuid(wogroupid).HasValue)
                    PhoenixPlannedMaintenanceWorkOrderGroup.RefreshToolboxMeet(new Guid(wogroupid));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}