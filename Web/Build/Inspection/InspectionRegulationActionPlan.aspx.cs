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
public partial class InspectionMOCRequestActionPlan : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

      

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("javascript: openNewWindow('MoreInfo', '', '"+ Session["sitepath"] + "/Inspection/InspectionRegulationActionPlanEdit.aspx?RegulationId=" + Request.QueryString["REGULATIONID"]+"'); return false;", "Add New Rule", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuRegulation.AccessRights = this.ViewState;
            MenuRegulation.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            TabAction.AccessRights = this.ViewState;
            TabAction.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["RegulationId"] != null && Request.QueryString["RegulationID"].ToString() != string.Empty)
                    ViewState["RegulationId"] = Request.QueryString["REGULATIONID"].ToString();
                if (Request.QueryString["MOCID"] != null)
                    ViewState["MOCID"] = Request.QueryString["MOCID"];
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

        BindDdata();
    }

    private void BindDdata()
    {
        try
        {
            DataSet ds = PhoenixInspectionNewRegulationActionPlan.RegulationList(new Guid((ViewState["RegulationId"]).ToString()));

            RegulationActionPlan.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegulationActionPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionNewRegulationActionPlan.RegulationDelete( new Guid(((RadLabel)e.Item.FindControl("lblRegulationActionPlanid")).Text));
                RebindCorrectiveAction();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RegulationActionPlan_ItemDataBound(Object sender, GridItemEventArgs e)
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

            LinkButton lnk = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel lblRegulationActionPlanid = (RadLabel)e.Item.FindControl("lblRegulationActionPlanid");
            if (lnk != null)
            {
                lnk.Visible = SessionUtil.CanAccess(this.ViewState, lnk.CommandName);
                lnk.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionRegulationActionPlanEdit.aspx?ACTIONPLANID=" + lblRegulationActionPlanid.Text + "&RegulationId=" + ViewState["RegulationId"] + "');return true;");
                
            }

            LinkButton lnkPedingList = (LinkButton)e.Item.FindControl("cmdPending");
            if (lnkPedingList != null)
            {
                lnkPedingList.Visible = SessionUtil.CanAccess(this.ViewState, lnkPedingList.CommandName);
                lnkPedingList.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionRegulationNotCompleted.aspx?RegulationActionID=" + lblRegulationActionPlanid.Text + "&RegulationId=" + ViewState["RegulationId"] + "');return true;");

            }
        }
    }

    protected void RegulationActionPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : RegulationActionPlan.CurrentPageIndex + 1;
            BindDdata();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RebindCorrectiveAction()
    {
        RegulationActionPlan.SelectedIndexes.Clear();
        RegulationActionPlan.EditIndexes.Clear();
        RegulationActionPlan.DataSource = null;
        RegulationActionPlan.Rebind();
    }

    protected void TabAction_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName == "BACK")
        {
            if (ViewState["MOCID"] != null)
                Response.Redirect("../Inspection/InspectionRegulationAdd.aspx?Mocid=" + ViewState["MOCID"]+ "&LaunchFrom="+Request.QueryString["LaunchFrom"]+ "&RegulationId="+Request.QueryString["REGULATIONID"] , false);
            else
                Response.Redirect("../Inspection/InspectionRegulationRule.aspx?RegulationId=" + Request.QueryString["REGULATIONID"] + "&LaunchFrom=" + Request.QueryString["LaunchFrom"], false);
        }
    }
}
