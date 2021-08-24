using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class InspectionIncidentCriticalFactor : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionCorrectiveAction.aspx?REFFROM=incident&REFERENCEID=" + Filter.CurrentIncidentID, "Add New", "<i class=\"fa fa-plus-circle\"></i>", "CORRECTIVEACTIONADD");
            MenuCA.AccessRights = this.ViewState;
            MenuCA.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPreventiveAction.aspx?REFFROM=incident&REFERENCEID=" + Filter.CurrentIncidentID, "Add New", "<i class=\"fa fa-plus-circle\"></i>", "PREVENTIVEACTIONADD");
            MenuPA.AccessRights = this.ViewState;
            MenuPA.MenuList = toolbar.Show();

            //PhoenixToolbar toolbartitle = new PhoenixToolbar();
            //MenuCARGeneral.AccessRights = this.ViewState;
            //MenuCARGeneral.MenuList = toolbartitle.Show();

            if (!IsPostBack)
            {
                ViewState["INCIDENTSTATUS"] = null;
                ViewState["SELECTEDCRITICALFACTOR"] = null;
                DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));
                if (ds.Tables[0].Rows.Count > 0)
                    ViewState["INCIDENTSTATUS"] = ds.Tables[0].Rows[0]["FLDSTATUSOFINCIDENT"].ToString();
                ucConfirmComplete.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCA_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void MenuPA_TabStripCommand(object sender, EventArgs e)
    {
    }
    /* Actions and Verification begin */

    //protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    //{
    //    RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //    string CommandName = ((RadToolBarButton)dce.Item).CommandName;

    //    if (CommandName.ToUpper().Equals("CLOSE"))
    //    {
    //        ucConfirmComplete.Visible = true;
    //        ucConfirmComplete.Text = "Are you sure you want to close the 'Incident'?";
    //    }
    //}

    protected void btnComplete_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (Filter.CurrentIncidentID != null)
                {
                    PhoenixInspectionIncident.IncidentStatusClose(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , new Guid(Filter.CurrentIncidentID)
                                                                  );

                    ucStatus.Text = "Incident is 'Reviewed & Closed'.";

                    //Filter.CurrentIncidentID = null;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["SELECTEDCRITICALFACTOR"] = null;

        RebindCorrectiveAction();
        RebindPreventiveAction();
    }

    private void BindCorrectiveAction()
    {
        try
        {
            DataSet ds = PhoenixInspectionCorrectiveAction.ListCorrectiveAction(
                General.GetNullableGuid(Filter.CurrentIncidentID != null ? Filter.CurrentIncidentID : null)
                , General.GetNullableGuid(null));

            gvCorrectiveAction.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindPreventiveAction()
    {
        try
        {
            DataSet ds = PhoenixInspectionPreventiveAction.ListPreventiveAction(
                General.GetNullableGuid(Filter.CurrentIncidentID != null ? Filter.CurrentIncidentID : null)
                , General.GetNullableGuid(null));

           gvPreventiveAction.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCorrectiveAction_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("CAEDITROW"))
            {
                RadLabel lblInsCorrectActid = (RadLabel)e.Item.FindControl("lblInsCorrectActid");
                Response.Redirect("../Inspection/InspectionCorrectiveAction.aspx?CORRECTIVEACTIONID=" + lblInsCorrectActid.Text + "&REFFROM=incident&REFERENCEID=" + Filter.CurrentIncidentID);
            }
            else if (e.CommandName.ToUpper().Equals("CADELETE"))
            {
                PhoenixInspectionCorrectiveAction.DeleteCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)e.Item.FindControl("lblInsCorrectActid")).Text));
                RebindCorrectiveAction();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCorrectiveAction_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                if (Filter.CurrentSelectedIncidentMenu == null)
                    db.Visible = true;
                else
                    db.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (Filter.CurrentSelectedIncidentMenu == null)
                    eb.Visible = true;
                else
                    eb.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton cmdReschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
            RadLabel coractionid = (RadLabel)e.Item.FindControl("lblInsCorrectActid");
            if (cmdReschedule != null)
            {
                cmdReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdReschedule.CommandName);
                cmdReschedule.Visible = false;
                if (coractionid != null)
                    cmdReschedule.Attributes.Add("onclick", "openNewWindow('codehelp','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentCARExtensionReason.aspx?correctiveactionid=" + coractionid.Text + "');return true;");
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                att.Attributes.Add("onclick", "openNewWindow('att','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=INCIDENTCORRACTION&VESSELID=" + drv["FLDVESSELID"].ToString() + "'); return true;");

                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName))
                    att.Visible = false;
                att.Visible = false;
            }

            LinkButton CarEdit = (LinkButton)e.Item.FindControl("cmdCarEdit");
            if (CarEdit != null)
            {
                if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "1") && (drv["FLDOPENEDYN"].ToString() == "0"))
                {
                    CarEdit.Visible = SessionUtil.CanAccess(this.ViewState, CarEdit.CommandName);
                    CarEdit.Attributes.Add("onclick", "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionShipBoardTasksDetails.aspx?CallFrom=incident&correctiveactionid=" + drv["FLDINSPECTIONCORRECTIVEACTIONID"].ToString() + "'); return true;");

                }
                else if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "2") && (drv["FLDOPENEDYN"].ToString() == "0"))
                {
                    CarEdit.Visible = SessionUtil.CanAccess(this.ViewState, CarEdit.CommandName);
                    CarEdit.Attributes.Add("onclick", "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionShipBoardTasksDetails.aspx?Officetask=yes&CallFrom=incident&correctiveactionid=" + drv["FLDINSPECTIONCORRECTIVEACTIONID"].ToString() + "'); return true;");
                }
                else 
                {
                    CarEdit.Visible = false;
                }
            }
            
        }
    }

    protected void gvPreventiveAction_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PAEDITROW"))
            {
                RadLabel lblInsPreventActid = (RadLabel)e.Item.FindControl("lblInsPreventActid");
                Response.Redirect("../Inspection/InspectionPreventiveAction.aspx?PREVENTIVEACTIONID=" + lblInsPreventActid.Text + "&REFFROM=incident&REFERENCEID=" + Filter.CurrentIncidentID);
            }
            else if (e.CommandName.ToUpper().Equals("PADELETE"))
            {
                PhoenixInspectionPreventiveAction.DeletePreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)e.Item.FindControl("lblInsPreventActid")).Text));
                RebindPreventiveAction();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPreventiveAction_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                if (Filter.CurrentSelectedIncidentMenu == null)
                    db.Visible = true;
                else
                    db.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (Filter.CurrentSelectedIncidentMenu == null)
                    eb.Visible = true;
                else
                    eb.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            UserControlToolTip ucRefName = (UserControlToolTip)e.Item.FindControl("ucRefName");
            RadLabel lblSubCategory = (RadLabel)e.Item.FindControl("lblSubCategory");

            if (lblSubCategory != null)
            {
                lblSubCategory.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucRefName.ToolTip + "', 'visible');");
                lblSubCategory.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucRefName.ToolTip + "', 'hidden');");
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton cmdReschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
            RadLabel preventiveactid = (RadLabel)e.Item.FindControl("lblInsPreventActid");
            if (cmdReschedule != null)
            {
                cmdReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdReschedule.CommandName);
                cmdReschedule.Visible = false;
                if (preventiveactid != null)
                    cmdReschedule.Attributes.Add("onclick", "openNewWindow('codehelp','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentCARExtensionReason.aspx?correctiveactionid=" + preventiveactid.Text + "');return true;");
            }

            DataRowView dr = (DataRowView)e.Item.DataItem;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                att.Attributes.Add("onclick", "openNewWindow('att','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=INCIDENTPREVACTION&VESSELID=" + drv["FLDVESSELID"].ToString() + "'); return true;");

                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName))
                    att.Visible = false;
                att.Visible = false;
            }
            //gvPreventiveAction.Columns[9].Visible = true;

            LinkButton ParEdit = (LinkButton)e.Item.FindControl("cmdParEdit");
            if (ParEdit != null)
            {
                if ((dr["FLDDEPARTMENTTYPEID"].ToString() == "1") && (dr["FLDOPENEDYN"].ToString() == "0"))
                {
                    ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                    ParEdit.Attributes.Add("onclick", "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionPreventiveTasksDetails.aspx?CallFrom=incident&preventiveactionid=" + dr["FLDINSPECTIONPREVENTIVEACTIONID"].ToString() + "'); return true;");

                }
                else if ((dr["FLDDEPARTMENTTYPEID"].ToString() == "2") && (dr["FLDOPENEDYN"].ToString() == "0"))
                {
                    ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                    ParEdit.Attributes.Add("onclick", "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionOfficeTasksDetails.aspx?CallFrom=incident&preventiveactionid=" + dr["FLDINSPECTIONPREVENTIVEACTIONID"].ToString() + "'); return true;");
                }
                else
                {
                    ParEdit.Visible = false;
                }
            }
        }
    }

    protected void gvCorrectiveAction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCorrectiveAction.CurrentPageIndex + 1;
            BindCorrectiveAction();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RebindCorrectiveAction()
    {
        gvCorrectiveAction.SelectedIndexes.Clear();
        gvCorrectiveAction.EditIndexes.Clear();
        gvCorrectiveAction.DataSource = null;
        gvCorrectiveAction.Rebind();
    }

    protected void gvPreventiveAction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPreventiveAction.CurrentPageIndex + 1;
            BindPreventiveAction();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RebindPreventiveAction()
    {
        gvPreventiveAction.SelectedIndexes.Clear();
        gvPreventiveAction.EditIndexes.Clear();
        gvPreventiveAction.DataSource = null;
        gvPreventiveAction.Rebind();
    }
}
