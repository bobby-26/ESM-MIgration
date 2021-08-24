using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionProcessRATeamComposition : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        if (Request.QueryString["ActivityName"] != null && Request.QueryString["ActivityName"].ToString() != "")
        {
            MenuOperational.Title = "Activity - " + Request.QueryString["ActivityName"].ToString();
        }
        MenuOperational.AccessRights = this.ViewState;       
        MenuOperational.MenuList = toolbarmain.Show();

        if(!IsPostBack)
        { 
            ViewState["ACTIVITYID"] = "";
            ViewState["LEVELID"] = "-1";
            ViewState["COMPOSITIONID"] = "";
            ViewState["FUNCTIONID"] = "-1";
            ViewState["DEPARTMENTID"] = "-1";
            ViewState["GROUPRANKLIST"] = "";

            if (Request.QueryString["Activityid"] != null && Request.QueryString["Activityid"].ToString() != "")
            {
                ViewState["ACTIVITYID"] = Request.QueryString["Activityid"].ToString();
            }

            
        }
    }

    protected void gvOperationalHazard_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void gvOperationalHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                LinkButton cmdRankGroupMapping = (LinkButton)e.Item.FindControl("cmdRankGroupMapping");
                if (cmdRankGroupMapping != null) cmdRankGroupMapping.Visible = SessionUtil.CanAccess(this.ViewState, cmdRankGroupMapping.CommandName);
            }
            GridDecorator.MergeRows(gvOperationalHazard, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindGrid()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentJobHazardExtn.RAActivityTeamCompositionPendingList(General.GetNullableInteger(ViewState["ACTIVITYID"].ToString()));
        gvOperationalHazard.DataSource = ds;
    }

    protected void ucGroupAdd_TextChanged(object sender, EventArgs e)
    {
        gvOperationalHazard.Rebind();
    }

    protected void Operational_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                if (General.GetNullableInteger(ViewState["ACTIVITYID"].ToString()) == null)
                {
                    ucError.ErrorMessage = "Activity is required";
                    ucError.Visible = true;
                    return;
                }

                foreach (GridDataItem gvr in gvOperationalHazard.Items)
                {

                    PhoenixInspectionRiskAssessmentJobHazardExtn.RAActivityTeamCompositionMapping(General.GetNullableInteger((ViewState["ACTIVITYID"]).ToString())
                                                      , General.GetNullableInteger(((RadLabel)gvr.FindControl("lblDepartmentid")).Text)
                                                      , General.GetNullableInteger(((RadLabel)gvr.FindControl("lbllevelid")).Text)
                                                      , General.GetNullableString(((RadLabel)gvr.FindControl("lblRankGroupList")).Text)
                                                      , General.GetNullableInteger(((UserControlMaskNumber)gvr.FindControl("txtcount")).Text)
                                                      , General.GetNullableGuid(((RadLabel)gvr.FindControl("lblcombositionid")).Text)
                                                      , General.GetNullableInteger(((RadLabel)gvr.FindControl("lblfunctionid")).Text));
                }
                gvOperationalHazard.Rebind();
                ucStatus.Text = "Informantion Updated";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }
            else
            {
                ucError.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindGroupRank()
    {
        DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.GroupRankByLevelList(General.GetNullableInteger(ViewState["LEVELID"].ToString()), General.GetNullableInteger(ViewState["DEPARTMENTID"].ToString()));
        chkRankGroup.DataSource = dss;
        chkRankGroup.DataBindings.DataTextField = "FLDGROUPRANK";
        chkRankGroup.DataBindings.DataValueField = "FLDGROUPRANKID";
        chkRankGroup.DataBind();

        General.RadBindCheckBoxList(chkRankGroup, ViewState["GROUPRANKLIST"].ToString());
    }

    protected void RadAjaxPanel2_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        var args = e.Argument;
        var array = args.Split(',');
        var id = array[0];
        var cmd = array[1];

        if (cmd.ToUpper() == "EDIT")
        {
            BindGroupRank();
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                PhoenixInspectionRiskAssessmentJobHazardExtn.RAActivityTeamCompositionMappingUpdate(General.GetNullableInteger(ViewState["ACTIVITYID"].ToString()), General.RadCheckBoxList(chkRankGroup), General.GetNullableGuid(ViewState["COMPOSITIONID"].ToString()), General.GetNullableInteger(ViewState["FUNCTIONID"].ToString()));
                ucStatus.Text = "Group Rank added.";
                BindGroupRank();
                gvOperationalHazard.DataBind();
                string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                return;
            }
        }
    }
    public class GridDecorator
    {       
        public static void MergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;
                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentDocumentName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblDeparment")).Text;
                string previousDocumentName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblDeparment")).Text;

                if (currentDocumentName == previousDocumentName)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                     previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }
            }
        }
    }

    protected void gvOperationalHazard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                var editableItem = ((GridDataItem)e.Item);

                GridDataItem eeditedItem = e.Item as GridDataItem;

                if (e.CommandName.ToUpper() == "GROUPRANKMAPPING")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    string id = item.GetDataKeyValue("FLDRAACTIVITYTEAMCOMPOSITIONID").ToString();
                    ViewState["LEVELID"] = ((RadLabel)editableItem.FindControl("lbllevelid")).Text;
                    ViewState["FUNCTIONID"] = ((RadLabel)editableItem.FindControl("lblfunctionid")).Text;
                    ViewState["COMPOSITIONID"] = ((RadLabel)editableItem.FindControl("lblcombositionid")).Text;
                    ViewState["DEPARTMENTID"] = ((RadLabel)editableItem.FindControl("lblDepartmentid")).Text;
                    ViewState["GROUPRANKLIST"] = ((RadLabel)editableItem.FindControl("lblRankGroupList")).Text;
                    string script = "function sd(){showDialog('Group Rank Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",EDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
