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
public partial class Inspection_InspectionOfficeDeficiencyCARList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "DEFICIENCYLIST", ToolBarDirection.Right);
            MenuCARGeneral.AccessRights = this.ViewState;
            MenuCARGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["DEFICIENCYID"] = "";
                ViewState["DEFICIENCYID"] = Request.QueryString["deficiencyid"];
            }
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
            if (CommandName.ToUpper().Equals("DEFICIENCYLIST"))
            {
                Response.Redirect("../Inspection/InspectionDeficiencyOfficeList.aspx", false);
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
            DataSet ds = PhoenixInspectionCorrectiveAction.ListCorrectiveAction(
                General.GetNullableGuid(ViewState["DEFICIENCYID"].ToString())
                , null);

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
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblInsCorrectActDTKey");
                if (lblDtkey != null)
                    ViewState["DTKEY"] = lblDtkey.Text;
                LinkButton ev = (LinkButton)e.Item.FindControl("imgEvidence");
                RadLabel lblIsAttchment = (RadLabel)e.Item.FindControl("lblInsCorrectActISAttachment");
                if (ev != null)
                {
                    ev.Visible = SessionUtil.CanAccess(this.ViewState, ev.CommandName);

                    HtmlGenericControl html = new HtmlGenericControl();

                    if (lblIsAttchment.Text == "0")
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                        ev.Controls.Add(html);
                    }
                    else
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color:skyblue\"><i class=\"fas fa-paperclip\"></i></span>";
                        ev.Controls.Add(html);
                    }

                    if (ev != null)
                        ev.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text
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
                        CarEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionShipBoardTasksDetails.aspx?CallFrom=deficiency&correctiveactionid=" + drv["FLDINSPECTIONCORRECTIVEACTIONID"].ToString() + "'); return true;");
                    }
                    else if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "2") && (drv["FLDOPENEDYN"].ToString() == "0"))
                    {
                        CarEdit.Visible = SessionUtil.CanAccess(this.ViewState, CarEdit.CommandName);
                        CarEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionShipBoardTasksDetails.aspx?Officetask=yes&CallFrom=deficiency&correctiveactionid=" + drv["FLDINSPECTIONCORRECTIVEACTIONID"].ToString() + "'); return true;");
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        RebindCorrectiveAction();
        RebindPreventiveAction();
    }
    private void BindPreventiveAction()
    {
        try
        {
            DataSet ds = PhoenixInspectionPreventiveAction.ListPreventiveAction(
                General.GetNullableGuid(ViewState["DEFICIENCYID"].ToString())
                , General.GetNullableGuid(null));

            gvPreventiveAction.DataSource = ds;
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
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton lnkWorkOrder = (LinkButton)e.Item.FindControl("lnkWorkOrderNumber");
                RadLabel lblWorkOrderID = (RadLabel)e.Item.FindControl("lblWorkOrderId");

                if (lnkWorkOrder != null)
                {
                    lnkWorkOrder.Attributes.Add("onclick", "javascript:openNewWindow('source','','../Inspection/InspectionLongTermActionWorkOrderDetails.aspx?WORKORDERID=" + lblWorkOrderID.Text + "&viewonly=1'); return true;");
                }
                LinkButton ParEdit = (LinkButton)e.Item.FindControl("cmdParEdit");
                if (ParEdit != null)
                {
                    if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "1") && (drv["FLDOPENEDYN"].ToString() == "0"))
                    {
                        ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                        ParEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionPreventiveTasksDetails.aspx?CallFrom=incident&preventiveactionid=" + drv["FLDINSPECTIONPREVENTIVEACTIONID"].ToString() + "'); return true;");

                    }
                    else if ((drv["FLDDEPARTMENTTYPEID"].ToString() == "2") && (drv["FLDOPENEDYN"].ToString() == "0"))
                    {
                        ParEdit.Visible = SessionUtil.CanAccess(this.ViewState, ParEdit.CommandName);
                        ParEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionOfficeTasksDetails.aspx?CallFrom=incident&preventiveactionid=" + drv["FLDINSPECTIONPREVENTIVEACTIONID"].ToString() + "'); return true;");
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
