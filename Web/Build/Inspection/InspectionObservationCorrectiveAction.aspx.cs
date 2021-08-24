using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class InspectionObservationCorrectiveAction : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (!IsPostBack)
        {
            if (Filter.CurrentSelectedIncidentMenu == null)

            if (Request.QueryString["DIRECTOBSERVATIONID"] != null && Request.QueryString["DIRECTOBSERVATIONID"].ToString() != string.Empty)
            {
                ViewState["DIRECTOBSERVATIONID"] = Request.QueryString["DIRECTOBSERVATIONID"].ToString();
                EditObservation(new Guid(ViewState["DIRECTOBSERVATIONID"].ToString()));
            }
            else
                ViewState["DIRECTOBSERVATIONID"] = null;
            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "" && Request.QueryString["callfrom"].ToString() == "directobservation")
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
        }
        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionCorrectiveAction.aspx?REFFROM=obs&REFERENCEID=" + ViewState["DIRECTOBSERVATIONID"].ToString(), "Add New", "<i class=\"fa fa-plus-circle\"></i>", "CAADD");
        MenuCA.AccessRights = this.ViewState;
        MenuCA.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionPreventiveAction.aspx?REFFROM=obs&REFERENCEID=" + ViewState["DIRECTOBSERVATIONID"].ToString(), "Add New", "<i class=\"fa fa-plus-circle\"></i>", "PAADD");
        MenuPA.AccessRights = this.ViewState;
        MenuPA.MenuList = toolbar.Show();
    }
    protected void MenuPA_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void MenuCA_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void EditObservation(Guid reviewdirectnonconformity)
    {
        DataSet ds = PhoenixInspectionObservation.EditDirectObservation(reviewdirectnonconformity);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["vesselid"] = dr["FLDVESSELID"].ToString();
        }
    }
    protected void InspectionDirectNCGeneral_TabStripCommand(object sender, EventArgs e)
    {
        
    }   

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        RebindCorrectiveAction();
        RebindPreventiveAction();
    } 
    // Action tab coding starts    

    private void BindCorrectiveAction()
    {
        try
        {
            DataSet ds = PhoenixInspectionCorrectiveAction.ListCorrectiveAction(
                General.GetNullableGuid(ViewState["DIRECTOBSERVATIONID"].ToString()));

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
                General.GetNullableGuid(ViewState["DIRECTOBSERVATIONID"].ToString()));

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
                Response.Redirect("../Inspection/InspectionCorrectiveAction.aspx?CORRECTIVEACTIONID=" + lblInsCorrectActid.Text + "&REFFROM=obs&REFERENCEID=" + ViewState["DIRECTOBSERVATIONID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("CAREDIT"))
            {
                e.Item.Selected = true;
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
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");    

                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {                
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
                    cmdReschedule.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Inspection/InspectionIncidentCARExtensionReason.aspx?correctiveactionid=" + coractionid.Text + "');return true;");
            }

            LinkButton CarEdit = (LinkButton)e.Item.FindControl("cmdCarEdit");
            if (CarEdit != null)
            {
                if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "1") && (drv["FLDOPENEDYN"].ToString() == "0"))
                {
                    CarEdit.Visible = SessionUtil.CanAccess(this.ViewState, CarEdit.CommandName);
                    CarEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionShipBoardTasksDetails.aspx?CallFrom=deficiency&correctiveactionid=" + drv["FLDINSPECTIONCORRECTIVEACTIONID"].ToString() + "'); return true;");

                }
                else if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "2") && (drv["FLDOPENEDYN"].ToString() == "0"))
                {
                    CarEdit.Visible = SessionUtil.CanAccess(this.ViewState, CarEdit.CommandName);
                    CarEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionShipBoardTasksDetails.aspx?Officetask=yes&CallFrom=deficiency&correctiveactionid=" + drv["FLDINSPECTIONCORRECTIVEACTIONID"].ToString() + "'); return true;");
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
                Response.Redirect("../Inspection/InspectionPreventiveAction.aspx?PREVENTIVEACTIONID=" + lblInsPreventActid.Text + "&REFFROM=obs&REFERENCEID=" + ViewState["DIRECTOBSERVATIONID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("PAREDIT"))
            {
                e.Item.Selected = true;
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
            DataRowView drv = (DataRowView)e.Item.DataItem;
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
                ucRefName.Position = ToolTipPosition.TopCenter;
                ucRefName.TargetControlId = lblSubCategory.ClientID;
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton cmdReschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
            RadLabel preactionid = (RadLabel)e.Item.FindControl("lblInsPreventActid");
            if (cmdReschedule != null)
            {
                cmdReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdReschedule.CommandName);
                cmdReschedule.Visible = false;
                if (preactionid != null)
                    cmdReschedule.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Inspection/InspectionNonConformityExtensionReason.aspx?ncid=" + preactionid.Text + "');return true;");
            }

            LinkButton ParEdit = (LinkButton)e.Item.FindControl("cmdParEdit");
            if (ParEdit != null)
            {
                if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "1") && (drv["FLDOPENEDYN"].ToString() == "0"))
                {
                    ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                    ParEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionPreventiveTasksDetails.aspx?CallFrom=incident&preventiveactionid=" + drv["FLDINSPECTIONPREVENTIVEACTIONID"].ToString() + "'); return true;");

                }
                else if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "2") && (drv["FLDOPENEDYN"].ToString() == "0"))
                {
                    ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                    ParEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionOfficeTasksDetails.aspx?CallFrom=incident&preventiveactionid=" + drv["FLDINSPECTIONPREVENTIVEACTIONID"].ToString() + "'); return true;");
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
