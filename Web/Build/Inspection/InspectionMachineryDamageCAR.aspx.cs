using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class InspectionMachineryDamageCAR : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);        

        if (!IsPostBack)
        {
            if (Request.QueryString["MACHINERYDAMAGEID"] != null && Request.QueryString["MACHINERYDAMAGEID"].ToString() != string.Empty)
                ViewState["MACHINERYDAMAGEID"] = Request.QueryString["MACHINERYDAMAGEID"].ToString();
            else
                ViewState["MACHINERYDAMAGEID"] = "";

            if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != string.Empty)
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            else
                ViewState["VESSELID"] = "";

            if (Request.QueryString["REFERENCENUMBER"] != null && Request.QueryString["REFERENCENUMBER"].ToString() != string.Empty)
                ViewState["REFERENCENUMBER"] = Request.QueryString["REFERENCENUMBER"].ToString();
            
			ViewState["DashboardYN"] = string.IsNullOrEmpty(Request.QueryString["DashboardYN"]) ? "" : Request.QueryString["DashboardYN"];
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (ViewState["DashboardYN"].ToString() == "")
        {
            toolbar.AddButton("List", "LIST");
        }
        toolbar.AddButton("Details", "DETAILS");
        toolbar.AddButton("CAR", "CAR");
        MenuCARGeneral.AccessRights = this.ViewState;
        MenuCARGeneral.MenuList = toolbar.Show();
        if (ViewState["DashboardYN"].ToString() == "")
        {
            MenuCARGeneral.SelectedMenuIndex = 2;
        }
        else
            MenuCARGeneral.SelectedMenuIndex = 1;

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMachineryDamageCorrectiveAction.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString(), "Add New", "<i class=\"fa fa-plus-circle\"></i>", "CAADD");
        MenuCA.AccessRights = this.ViewState;
        MenuCA.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMachineryDamagePreventiveAction.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString(), "Add New", "<i class=\"fa fa-plus-circle\"></i>", "PAADD");
        MenuPA.AccessRights = this.ViewState;
        MenuPA.MenuList = toolbar.Show();

        if (ViewState["REFERENCENUMBER"] != null)
        {
            lblCorrectiveAction.Text = "Corrective Action( " + ViewState["REFERENCENUMBER"] + " )";
            lblPreventiveAction.Text = "Preventive Action( " + ViewState["REFERENCENUMBER"] + " )";
        }
        else
        {
            lblCorrectiveAction.Text = "Corrective Action";
            lblPreventiveAction.Text = "Preventive Action";
        }

        BindCorrectiveAction();
        BindPreventiveAction();
    }

    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionMachineryDamageList.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString());
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../Inspection/InspectionMachineryDamageGeneral.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&DashboardYN=" + ViewState["DashboardYN"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
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

    protected void MenuMachineryDamageCAR_TabStripCommand(object sender, EventArgs e)
    {
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
                    , new Guid(ViewState["MACHINERYDAMAGEID"].ToString())
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
        BindCorrectiveAction();
        BindPreventiveAction();
    }
    // Action tab coding starts

    private void BindCorrectiveAction()
    {
        try
        {
            DataSet ds = PhoenixInspectionCorrectiveAction.ListCorrectiveAction(
                General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()));

            gvCorrectiveAction.DataSource = ds;
            gvCorrectiveAction.DataBind();
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
                General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()));

            gvPreventiveAction.DataSource = ds;
            gvPreventiveAction.DataBind();
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
                Response.Redirect("../Inspection/InspectionMachineryDamageCorrectiveAction.aspx?CORRECTIVEACTIONID=" + lblInsCorrectActid.Text + "&MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("CADELETE"))
            {
                PhoenixInspectionCorrectiveAction.DeleteCorrectiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)e.Item.FindControl("lblInsCorrectActid")).Text));
                BindCorrectiveAction();
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
                    cmdReschedule.Attributes.Add("onclick", "openNewWindow('codehelp','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentCARExtensionReason.aspx?correctiveactionid=" + coractionid.Text + "');return true;");
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton CarEdit = (LinkButton)e.Item.FindControl("lnkViewCAR");
            if (CarEdit != null)
            {
                if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "1") && (drv["FLDOPENEDYN"].ToString() == "0"))
                {
                    CarEdit.Visible = SessionUtil.CanAccess(this.ViewState, CarEdit.CommandName);
                    CarEdit.Attributes.Add("onclick", "openNewWindow('Details','', '" + Session["sitepath"] + "/Inspection/InspectionShipBoardTasksDetails.aspx?CallFrom=MachineryDamage&correctiveactionid=" + coractionid.Text + "'); return true;");

                }
                else if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "2") && (drv["FLDOPENEDYN"].ToString() == "0"))
                {
                    CarEdit.Visible = SessionUtil.CanAccess(this.ViewState, CarEdit.CommandName);
                    CarEdit.Attributes.Add("onclick", "openNewWindow('Details','', '" + Session["sitepath"] + "/Inspection/InspectionShipBoardTasksDetails.aspx?Officetask=yes&CallFrom=MachineryDamage&correctiveactionid=" + coractionid.Text + "'); return true;");
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
                Response.Redirect("../Inspection/InspectionMachineryDamagePreventiveAction.aspx?PREVENTIVEACTIONID=" + lblInsPreventActid.Text + "&MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("PADELETE"))
            {
                PhoenixInspectionPreventiveAction.DeletePreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)e.Item.FindControl("lblInsPreventActid")).Text));
                BindPreventiveAction();
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
            RadLabel preactionid = (RadLabel)e.Item.FindControl("lblInsPreventActid");
            if (cmdReschedule != null)
            {
                cmdReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdReschedule.CommandName);
                cmdReschedule.Visible = false;
                if (preactionid != null)
                    cmdReschedule.Attributes.Add("onclick", "openNewWindow('codehelp','', '" + Session["sitepath"] + "/Inspection/InspectionNonConformityExtensionReason.aspx?ncid=" + preactionid.Text + "');return true;");
            }

            LinkButton ParEdit = (LinkButton)e.Item.FindControl("lnkViewPa");
            DataRowView dr = (DataRowView)e.Item.DataItem;

            if (ParEdit != null)
            {
                if ((dr["FLDDEPARTMENTTYPEID"].ToString() == "1") && (dr["FLDOPENEDYN"].ToString() == "0"))
                {
                    ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                    ParEdit.Attributes.Add("onclick", "openNewWindow('Details','', '" + Session["sitepath"] + "/Inspection/InspectionPreventiveTasksDetails.aspx?CallFrom=MachineryDamage&preventiveactionid=" + preactionid.Text + "'); return true;");

                }
                else if ((dr["FLDDEPARTMENTTYPEID"].ToString() == "2") && (dr["FLDOPENEDYN"].ToString() == "0"))
                {
                    ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                    ParEdit.Attributes.Add("onclick", "openNewWindow('Details','', '" + Session["sitepath"] + "/Inspection/InspectionOfficeTasksDetails.aspx?CallFrom=MachineryDamage&preventiveactionid=" + preactionid.Text + "'); return true;");
                }
                else
                {
                    ParEdit.Visible = false;
                }
            }
        }
    }

    protected void MenuPA_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void MenuCA_TabStripCommand(object sender, EventArgs e)
    {

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

    protected void gvCorrectiveAction_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        BindCorrectiveAction();
    }

    protected void gvPreventiveAction_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        BindPreventiveAction();
    }
}
