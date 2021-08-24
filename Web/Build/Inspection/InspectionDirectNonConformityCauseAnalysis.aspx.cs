using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionDirectNonConformityCauseAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		if (!IsPostBack)
		{            
			if (Request.QueryString["REVIEWDNC"] != null && Request.QueryString["REVIEWDNC"].ToString() != string.Empty)
			{
				ViewState["REVIEWDNC"] = Request.QueryString["REVIEWDNC"].ToString();
				EditNonConformity(new Guid(ViewState["REVIEWDNC"].ToString()));
			}
			else
				ViewState["REVIEWDNC"] = null;
            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "" && Request.QueryString["callfrom"].ToString() == "directobservation")
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionCorrectiveAction.aspx?REFFROM=nc&REFERENCEID=" + ViewState["REVIEWDNC"].ToString(), "Add New", "<i class=\"fa fa-plus-circle\"></i>", "CAADD");
        MenuCA.AccessRights = this.ViewState;
        MenuCA.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionPreventiveAction.aspx?REFFROM=nc&REFERENCEID=" + ViewState["REVIEWDNC"].ToString(), "Add New", "<i class=\"fa fa-plus-circle\"></i>", "PAADD");
        MenuPA.AccessRights = this.ViewState;
        MenuPA.MenuList = toolbar.Show();
	}
    protected void MenuCA_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void MenuPA_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void EditNonConformity(Guid reviewdirectnonconformity)
	{
		DataSet ds = PhoenixInspectionAuditNonConformity.EditAuditNC(reviewdirectnonconformity);
		if (ds.Tables[0].Rows.Count > 0)
		{
			DataRow dr = ds.Tables[0].Rows[0];
			ViewState["vesselid"] = dr["FLDVESSELID"].ToString();
		}
	}
	protected void MenuRootCause_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("IMMEDIATECAUSEEXCEL"))
			{
				string[] alColumns = { "FLDROOTCAUSENAME", "FLDSUBCAUSENAME", "FLDREASON" };
				string[] alCaptions = { "Main Cause", "Sub Cause", "Reason" };

				DataSet ds = PhoenixInspectionAuditDirectNonConformity.ListRootCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, null
					, null
					, new Guid(ViewState["REVIEWDNC"].ToString())
					);

				if (ds.Tables.Count > 0)
					General.ShowExcel("Immediate Cause", ds.Tables[0], alColumns, alCaptions, null, null);
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
        ReBindCorrectiveAction();
        ReBindPreventiveAction();
    }

    // Action tab coding starts

    private void BindCorrectiveAction()
    {
        try
        {
            DataSet ds = PhoenixInspectionCorrectiveAction.ListCorrectiveAction(
                General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()));

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
                General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()));

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
                Response.Redirect("../Inspection/InspectionCorrectiveAction.aspx?CORRECTIVEACTIONID=" + lblInsCorrectActid.Text + "&REFFROM=nc&REFERENCEID=" + ViewState["REVIEWDNC"].ToString());                
            }
            else if (e.CommandName.ToUpper().Equals("CAREDIT"))
            {
                e.Item.Selected = true;
            }
            else if (e.CommandName.ToUpper().Equals("CADELETE"))
            {
                PhoenixInspectionCorrectiveAction.DeleteCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)e.Item.FindControl("lblInsCorrectActid")).Text));
                ReBindCorrectiveAction();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvCorrectiveAction_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    gvCorrectiveAction.SelectedIndex = e.NewSelectedIndex;
    //}
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
                Response.Redirect("../Inspection/InspectionPreventiveAction.aspx?PREVENTIVEACTIONID=" + lblInsPreventActid.Text + "&REFFROM=nc&REFERENCEID=" + ViewState["REVIEWDNC"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("PAREDIT"))
            {
                e.Item.Selected = true;
            }
            else if (e.CommandName.ToUpper().Equals("PADELETE"))
            {
                PhoenixInspectionPreventiveAction.DeletePreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)e.Item.FindControl("lblInsPreventActid")).Text));
                ReBindPreventiveAction();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvPreventiveAction_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    gvPreventiveAction.SelectedIndex = e.NewSelectedIndex;
    //}
    protected void gvPreventiveAction_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
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
                if ((dr["FLDDEPARTMENTTYPEID"].ToString() == "1") && (dr["FLDOPENEDYN"].ToString() == "0"))
                {
                    ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                    ParEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionPreventiveTasksDetails.aspx?CallFrom=incident&preventiveactionid=" + dr["FLDINSPECTIONPREVENTIVEACTIONID"].ToString() + "'); return true;");

                }
                else if ((dr["FLDDEPARTMENTTYPEID"].ToString() == "2") && (dr["FLDOPENEDYN"].ToString() == "0"))
                {
                    ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                    ParEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionOfficeTasksDetails.aspx?CallFrom=incident&preventiveactionid=" + dr["FLDINSPECTIONPREVENTIVEACTIONID"].ToString() + "'); return true;");
                }
                else
                {
                    ParEdit.Visible = false;
                }
            }
        }
    }
    protected void ReBindCorrectiveAction()
    {
        gvCorrectiveAction.SelectedIndexes.Clear();
        gvCorrectiveAction.EditIndexes.Clear();
        gvCorrectiveAction.DataSource = null;
        gvCorrectiveAction.Rebind();
    }

    protected void ReBindPreventiveAction()
    {
        gvPreventiveAction.SelectedIndexes.Clear();
        gvPreventiveAction.EditIndexes.Clear();
        gvPreventiveAction.DataSource = null;
        gvPreventiveAction.Rebind();
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
}
