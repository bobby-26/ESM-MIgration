using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Inspection_InspectionIncidentActionsView : PhoenixBasePage
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "INCIDENTLIST",ToolBarDirection.Right);
            MenuCARGeneral.AccessRights = this.ViewState;
            MenuCARGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                Filter.CurrentIncidentID = Request.QueryString["inspectionincidentid"];
            }

            BindCorrectiveAction();
            BindPreventiveAction();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("INCIDENTLIST"))
            {
                Response.Redirect("../Inspection/InspectionIncidentNearMissList.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    /* Actions and Verification begin */    

    private void BindCorrectiveAction()
    {
        try
        {
            DataSet ds = PhoenixInspectionIncident.ListCorrectiveAction(
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
    protected void gvCorrectiveAction_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {

                RadLabel lblCorrectiveAction = (RadLabel)e.Item.FindControl("lblCorrectiveAction");
                LinkButton lnkCorrectiveAction = (LinkButton)e.Item.FindControl("lnkCorrectiveAction");
                RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblInsCorrectActDTKey");
                if (lblDtkey != null)
                    ViewState["DTKEY"] = lblDtkey.Text;
                LinkButton ev = (LinkButton)e.Item.FindControl("imgEvidence");
                RadLabel lblIsAttchment = (RadLabel)e.Item.FindControl("lblInsCorrectActISAttachment");

                if (ev != null)
                {
                    HtmlGenericControl html = new HtmlGenericControl();

                    if (lblIsAttchment.Text == "0")
                    {
                        html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip-na\"></i></span>";
                        ev.Controls.Add(html);
                    }
                    else
                    {
                        html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip\"></i></span>";
                        ev.Controls.Add(html);
                    }
                    ev.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text
                          + "&mod=" + PhoenixModule.QUALITY
                          + "&type=SHIPBOARDEVIDENCE"
                          + "&cmdname=SHIPBOARDEVIDENCEUPLOAD"
                          + "&U=0"
                          + "'); return false;");
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
            DataSet ds = PhoenixInspectionIncident.ListPreventiveAction(
                General.GetNullableGuid(Filter.CurrentIncidentID != null ? Filter.CurrentIncidentID : null)
                , General.GetNullableGuid(null));

            gvPreventiveAction.DataSource = ds;
            gvPreventiveAction.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPreventiveAction_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                LinkButton lnkWorkOrder = (LinkButton)e.Item.FindControl("lnkWorkOrderNumber");
                RadLabel lblWorkOrderID = (RadLabel)e.Item.FindControl("lblWorkOrderId");
                RadLabel lblPreventiveAction = (RadLabel)e.Item.FindControl("lblPreventiveAction");
                LinkButton lnkPreventiveAction = (LinkButton)e.Item.FindControl("lnkPreventiveAction");

                if (lnkWorkOrder != null)
                {
                    lnkWorkOrder.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Inspection/InspectionLongTermActionWorkOrderDetails.aspx?WORKORDERID=" + lblWorkOrderID.Text  + "&viewonly=1'); return true;");
                }

                LinkButton ParEdit = (LinkButton)e.Item.FindControl("cmdParEdit");
                if (ParEdit != null)
                {
                    if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "1") && (drv["FLDOPENEDYN"].ToString() == "0"))
                    {
                        ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                        ParEdit.Attributes.Add("onclick", "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionPreventiveTasksDetails.aspx?CallFrom=incident&preventiveactionid=" + drv["FLDINSPECTIONPREVENTIVEACTIONID"].ToString() + "'); return true;");

                    }
                    else if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "2") && (drv["FLDOPENEDYN"].ToString() == "0"))
                    {
                        ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                        ParEdit.Attributes.Add("onclick", "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionOfficeTasksDetails.aspx?CallFrom=incident&preventiveactionid=" + drv["FLDINSPECTIONPREVENTIVEACTIONID"].ToString() + "'); return true;");
                    }
                    else
                    {
                        ParEdit.Visible = false;
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindCorrectiveAction();
        BindPreventiveAction();
    }

    protected void gvCorrectiveAction_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("CAREDIT"))
        {
            LinkButton lnkCorrectiveAction = ((LinkButton)e.Item.FindControl("lnkCorrectiveAction"));
            RadLabel lblInsCorrectActid = (RadLabel)e.Item.FindControl("lblInsCorrectActid");

            if (lnkCorrectiveAction != null)
                Response.Redirect("../Inspection/InspectionCorrectiveAction.aspx?View=1&CORRECTIVEACTIONID=" + lblInsCorrectActid.Text + "&REFFROM=incident&REFERENCEID=" + Filter.CurrentIncidentID , false);
        }            
          
    }
    protected void gvPreventiveAction_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("PAEDIT"))
        {
            LinkButton lnkPreventiveAction = ((LinkButton)e.Item.FindControl("lnkPreventiveAction"));
            RadLabel lblInsPreventActid = (RadLabel)e.Item.FindControl("lblInsPreventActid");

            if (lnkPreventiveAction != null)
                Response.Redirect("../Inspection/InspectionPreventiveAction.aspx?View=1&PREVENTIVEACTIONID=" + lblInsPreventActid.Text + "&REFFROM=incident&REFERENCEID=" + Filter.CurrentIncidentID, false);
        }
    }
}
