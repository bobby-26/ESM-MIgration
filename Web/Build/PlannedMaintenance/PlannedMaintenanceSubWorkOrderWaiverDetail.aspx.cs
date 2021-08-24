using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI;
using System.Text;

public partial class PlannedMaintenanceSubWorkOrderWaiverDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            //PhoenixToolbar toolbarPTW = new PhoenixToolbar();
            ////toolbarPTW.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobListForPlanWorkOrder.aspx');", "Create WO", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");
            //MenuPTW.MenuList = toolbarPTW.Show();

            PhoenixToolbar toolbarParts = new PhoenixToolbar();
            toolbarParts.AddFontAwesomeButton("", "Create Requisition", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuParts.MenuList = toolbarParts.Show();

            PhoenixToolbar toolbarWaive = new PhoenixToolbar();
            menuWaive.MenuList = toolbarWaive.Show();

            if (!IsPostBack)
            {
                ViewState["WORKORDERID"] = string.Empty;
                if (Request.QueryString["WORKORDERID"] != null)
                {
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                }

                string wrkgrpid = Request.QueryString["WorkGroupId"];
                ViewState["WORKORDERGROUPID"] = string.IsNullOrEmpty(wrkgrpid) ? "" : wrkgrpid;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindPTW()
    {
        try
        {
            DataSet ds;
            ds = PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubworkorderPtwList(new Guid(ViewState["WORKORDERID"].ToString()));
            gvPTWSummary.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindSpare()
    {
        try
        {
            DataSet ds;
            ds = PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubworkorderSpareRequiredList(new Guid(ViewState["WORKORDERID"].ToString()));
            gvPartSummary.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPTWSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPTW();
    }

    protected void gvPTWSummary_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
            }
            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ImageButton imgShowHazardEdit = (ImageButton)e.Item.FindControl("imgShowHazardEdit");
            if (imgShowHazardEdit != null)
            {
                //imgShowHazardEdit.Attributes.Add("onclick", "top.openNewWindow('spnHazardEdit', 'JhaList', '" + Session["sitepath"] + "/Common/CommonPickListJHAExtn.aspx?vesselid=0&status=3');return false;");
                imgShowHazardEdit.Attributes.Add("onclick", "openNewWindow('spnHazardEdit', 'Component','" + Session["sitepath"] + "/Common/CommonPickListJHAExtn.aspx?vesselid=0&status=3&ispopup=spnHazardEdit,codehelp1',null,null,null,null,'spnHazardEdit'); ");
                //cmdShowItem.OnClientClick = "top.openNewWindow('spnPickItem', 'Spares','" + Session["sitepath"] + "/Common/CommonPickListSpareItemByComponent.aspx?ispopup=spnPickItem,dsd&mode=custom&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&txtnumber='+ document.getElementById('" + txtNumber.UniqueID + "').value);";
                if (!SessionUtil.CanAccess(this.ViewState, imgShowHazardEdit.CommandName)) imgShowHazardEdit.Visible = false;

            }
            RadTextBox txtHazardIdEdit = (RadTextBox)e.Item.FindControl("txtHazardIdEdit");
            if (txtHazardIdEdit != null) txtHazardIdEdit.Attributes.Add("style", "display:none");

            LinkButton frm = (LinkButton)e.Item.FindControl("lnkFormNumber");
            string formId = ((RadLabel)e.Item.FindControl("lblFormId")).Text;
            string revisionId = ((RadLabel)e.Item.FindControl("lblFormRevisionid")).Text;
            string reportId = ((RadLabel)e.Item.FindControl("lblReportId")).Text;

            if (frm != null)
            {
                frm.Visible = SessionUtil.CanAccess(this.ViewState, frm.CommandName);
                frm.Attributes.Add("onclick", "javascript:openNewWindow('PTW','','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + formId + "&FORMREVISIONID=" + revisionId + "&workorderId=" + ViewState["WORKORDERID"]+"',400,500); return false;");
                //lnkwaive.Attributes.Add("onclick", "javascript:openNewWindow('WAIVE','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupWaiver.aspx?WorkorderGroupId=" + ViewState["WORKORDERID"] + "&WaiverType=" + waiveType + "',400,500); return false;");
            }
            LinkButton btnJha = (LinkButton)e.Item.FindControl("lnkNumber");
            RadLabel jhaId = ((RadLabel)e.Item.FindControl("lblHazardID"));
            if (btnJha != null)
            {
                if (jhaId != null)
                {
                    btnJha.Attributes.Add("onclick", "javascript:openNewWindow('PTW','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=JOBHAZARDNEW&jobhazardid=" + jhaId.Text + "&showmenu=0&showword=NO&showexcel=NO',400,500); return false;");
                }
            }

            LinkButton report = (LinkButton)e.Item.FindControl("cmdReport");
            if(report != null)
            {
                if (!string.IsNullOrEmpty(reportId))
                {
                    report.Visible = true;
                    report.Attributes.Add("onclick", "javascript:openNewWindow('PTW','','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&ReportId=" + reportId + "',400,500); return false;");
                }
            }

        }
    }

    protected void gvPartSummary_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
        if (cmdDelete != null)
        {
            cmdDelete.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
        }

        DataRowView drv = (DataRowView)e.Item.DataItem;
        LinkButton lnkSpare = (LinkButton)e.Item.FindControl("lnkNumber");
        if (lnkSpare != null)
        {
            lnkSpare.Attributes.Add("onclick", "javascript:openNewWindow('Spare','','" + Session["sitepath"] + "/Inventory/InventorySpareItemGeneral.aspx?SPAREITEMID=" + drv["FLDSPAREITEMID"] + "',400,500); return false;");
        }
    }

    protected void gvPartSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindSpare();
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection != null)
        {
            string Script = string.Empty;
            Script += "populatePick()";
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", "populatePick()", true);
        }
        gvWaiver.Rebind();
    }

    protected void gvPTWSummary_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string hazardId = ((RadTextBox)e.Item.FindControl("txtHazardIdEdit")).Text;
                PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubWorkorderJhaPtwInsert(new Guid(hazardId), new Guid(ViewState["WORKORDERID"].ToString()));

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string formId = ((RadLabel)e.Item.FindControl("lblFormId")).Text;
                string revisionId = ((RadLabel)e.Item.FindControl("lblFormRevisionid")).Text;
                PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubWorkorderJhaPtwDelete(new Guid(formId), General.GetNullableGuid(revisionId),
                                                                                        new Guid(ViewState["WORKORDERID"].ToString()));
            }

            gvWaiver.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPartSummary_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string formId = ((RadLabel)e.Item.FindControl("lblFormId")).Text;
                string revisionId = ((RadLabel)e.Item.FindControl("lblFormRevisionid")).Text;
                PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubWorkorderJhaPtwDelete(new Guid(formId), General.GetNullableGuid(revisionId),
                                                                                        new Guid(ViewState["WORKORDERID"].ToString()));

                gvWaiver.Rebind();
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWaiver_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        WaiveBind();
    }
    private void WaiveBind()
    {
        try
        {
            DataSet ds;
            ds = PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubworkorderWaiveRequiredDetail(new Guid(ViewState["WORKORDERID"].ToString()));
            gvWaiver.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvWaiver_ItemDataBound(object sender, GridItemEventArgs e)
    {
        ImageButton lnkwaive = (ImageButton)e.Item.FindControl("lnkPtwWaive");
        DataRowView drv = (DataRowView)e.Item.DataItem;
        
        if (lnkwaive != null)
        {
            string waiveType = ((RadLabel)e.Item.FindControl("lblWaiveType")).Text;
            lnkwaive.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupWaiver.aspx?WorkorderId=" + ViewState["WORKORDERID"] + "&WaiverType=" + waiveType + "&workordergroupid=" + ViewState["WORKORDERGROUPID"].ToString() + "&inp=1';showDialog('Waive');");
            //lnkwaive.Attributes.Add("onclick", "javascript:openNewWindow('WAIVE','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupWaiver.aspx?WorkorderId="+ ViewState["WORKORDERID"]+"&WaiverType="+ waiveType + "&workordergroupid=" + ViewState["WORKORDERGROUPID"].ToString() + "',400,500); return false;");
            lnkwaive.Visible = SessionUtil.CanAccess(this.ViewState, lnkwaive.CommandName);
            if (drv["FLDREQUIREDMET"].ToString() == "1")
            {
                lnkwaive.Visible = false;
            }
        }
    }


    protected void MenuParts_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            StringBuilder strSpare = new StringBuilder();
            StringBuilder strQty = new StringBuilder();
            if (gvPartSummary.Items.Count > 0)
            {
                foreach (GridDataItem gv in gvPartSummary.SelectedItems)
                {
                    RadLabel lblSpareItem = (RadLabel)gv.FindControl("lblSpareItemId");
                    strSpare.Append(lblSpareItem.Text + ",");

                    RadLabel lblQty = (RadLabel)gv.FindControl("lblShortage");
                    strQty.Append(lblQty.Text + ",");


                }

                PhoenixPlannedMaintenanceWorkOrderGroup.RequisitionCreate(strSpare.ToString(), strQty.ToString());

                RadNotification1.Show("Requisition Created.");
            }
        }
    }
}
