using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;

public partial class PlannedMaintenanceJobPtwList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobPtwList.aspx?" + Request.QueryString.ToString(), "Add PTW", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbarmain.Show();                       

            if (!IsPostBack)
            {
                ViewState["PTWCATEGORYID"] = string.Empty;
                ViewState["workorderId"] = string.Empty;                
                if (Request.QueryString["workorderId"] != null)
                    ViewState["workorderId"] = Request.QueryString["workorderId"];
                ViewState["WORKORDERGROUPID"] = string.Empty;
                if (Request.QueryString["WORKORDERGROUPID"] != null)
                    ViewState["WORKORDERGROUPID"] = Request.QueryString["WORKORDERGROUPID"];
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvJhaList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubworkorderPtwList(new Guid(ViewState["workorderId"].ToString()));
        gvJhaList.DataSource = ds;
        if(ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            ViewState["PTWCATEGORYID"] = ds.Tables[1].Rows[0][0].ToString();
        }
        RadWindow_NavigateUrl.NavigateUrl = "PlannedMaintenancePTWForms.aspx?woid="+ ViewState["workorderId"].ToString() + "&cid=" + ViewState["PTWCATEGORYID"].ToString();
    }

    protected void gvJhaList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                string staus = drv["FLDSTATUS"].ToString();
                string type = drv["FLDTYPENAME"].ToString();
                ImageButton cmdAtt = (ImageButton)e.Item.FindControl("cmdAtt");
                
                LinkButton frm = (LinkButton)e.Item.FindControl("lnkPtwName");
                if (frm != null)
                {
                    frm.Visible = SessionUtil.CanAccess(this.ViewState, frm.CommandName);

                    if (type.ToUpper() == "UPLOAD")
                    {
                        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(drv["FLDAPPROVEDREVISIONDTKEY"].ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            DataRow drRow = dt.Rows[0];
                            frm.Attributes.Add("onclick", "javascript:top.openNewWindow('frm','Checklist & Forms - [" + drv["FLDFORMNAME"].ToString() + "]','" + Session["sitepath"] + "/Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString() + "'); return false;");
                        }
                        if (cmdAtt != null)
                        {
                            cmdAtt.Attributes.Add("onclick", "javascript:openNewWindow('cmnattachment','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDREPORTID"].ToString() + "&mod=" + PhoenixModule.DOCUMENTMANAGEMENT + "&DocSource=PTW&RefreshWindowName=maint,RadWindow_NavigateUrl'); return false;");
                            cmdAtt.Visible = drv["FLDREPORTID"].ToString() != string.Empty && SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);                            
                        }
                    }
                    else
                    {
                        frm.Attributes.Add("onclick", "javascript:top.openNewWindow('frm','Checklist & Forms - [" + drv["FLDFORMNAME"].ToString() + "]','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + drv["FLDFORMID"].ToString() + "&FORMREVISIONID=" + drv["FLDFORMREVISION"].ToString() + "&ReportId=" + drv["FLDREPORTID"].ToString() + "'); return false;");
                    }
                }
                LinkButton delete = (LinkButton)e.Item.FindControl("cmdDelete");
                if (delete != null)
                {
                    delete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    delete.Visible = SessionUtil.CanAccess(this.ViewState, delete.CommandName);
                }
                LinkButton upload = (LinkButton)e.Item.FindControl("cmdUpload");
                if (upload != null)
                {
                    if (type.ToUpper() == "UPLOAD" && drv["FLDREPORTID"].ToString() == string.Empty)
                    {
                        upload.Visible = SessionUtil.CanAccess(this.ViewState, upload.CommandName);
                        upload.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl = '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSShipboardFormsAttachmentUpload.aspx?FORMID=" + drv["FLDFORMID"].ToString().ToString() + "&woid=" + drv["FLDWORKORDERID"].ToString().ToString() + "&DocSource=PTW'; showDialog('Upload - [" + drv["FLDFORMNAME"].ToString() + "]');");
                    }
                    else
                    {
                        upload.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "ADD")
            {
                string script = "function sd(){$modalWindow.modalWindowUrl='PlannedMaintenancePTWForms.aspx?woid=" + ViewState["workorderId"] + "&cid=" + ViewState["PTWCATEGORYID"] + "'; showDialog('Add');Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
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
        gvJhaList.Rebind();
        if (General.GetNullableGuid(ViewState["WORKORDERGROUPID"].ToString()).HasValue)
            PhoenixPlannedMaintenanceWorkOrderGroup.RefreshToolboxMeet(new Guid(ViewState["WORKORDERGROUPID"].ToString()));
        string script = "refresh()";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void gvJhaList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "DELETE")
            {
                string reportid = ((RadLabel)e.Item.FindControl("lblReportId")).Text;
                string jhamapid = ((RadLabel)e.Item.FindControl("lblJHAMapId")).Text;
                if (General.GetNullableGuid(reportid).HasValue)
                {
                    PhoenixDocumentManagementForm.FormAnChecklistDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(reportid));
                }
                else
                {
                    PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubWorkorderPtwDelete(new Guid(jhamapid));
                }
                if (General.GetNullableGuid(ViewState["WORKORDERGROUPID"].ToString()).HasValue)
                    PhoenixPlannedMaintenanceWorkOrderGroup.RefreshToolboxMeet(new Guid(ViewState["WORKORDERGROUPID"].ToString()));
                string script = "refresh()";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
