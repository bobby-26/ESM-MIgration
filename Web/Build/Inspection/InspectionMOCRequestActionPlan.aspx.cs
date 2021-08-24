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
using System.Collections;
public partial class InspectionMOCRequestActionPlan : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarstatus = new PhoenixToolbar();
            toolbarstatus.AddButton("List", "LIST", ToolBarDirection.Left);
            toolbarstatus.AddButton("Details", "DETAILS", ToolBarDirection.Left);
            toolbarstatus.AddButton("Action Plan", "ACTIONPLAN", ToolBarDirection.Left);
            toolbarstatus.AddButton("Evaluation & Approval", "EVALUATION", ToolBarDirection.Left);
            toolbarstatus.AddButton("Intermediate Verification", "INTERMEDIATE", ToolBarDirection.Left);
            toolbarstatus.AddButton("Implementation & Verification", "IMPLEMENTATION", ToolBarDirection.Left);

            MenuMOCStatus.MenuList = toolbarstatus.Show();
            MenuMOCStatus.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                if (Request.QueryString["MOCID"] != null && Request.QueryString["MOCID"].ToString() != string.Empty)
                    ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMOCRequestActionPlanEdit.aspx?VESSELID=" + Request.QueryString["VESSELID"].ToString() + "&MOCID=" + Request.QueryString["MOCID"].ToString(), "Add New", "<i class=\"fa fa-plus-circle\"></i>", "CORRECTIVEACTIONADD");
            //toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMOCRequestActionPlanComment.aspx?')", "Bulk Office Comments", "<i class=\"fa fa-comments\"></i>", "BULKOFFICECOMMENTS");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMOCRequestActionPlanComment.aspx?MOCID=" + ViewState["MOCID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString(), "Action Plan Edit", "<i class=\"fa fa-comments\"></i>", "ADD");
            MenuCA.AccessRights = this.ViewState;
            MenuCA.MenuList = toolbar.Show();
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

    protected void MenuMOCStatus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionManagementOfChangeList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestAdd.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("EVALUATION"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestEvalutionApproval.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("INTERMEDIATE"))
            {
                Response.Redirect("../Inspection/InspectionMOCIntermediateVerificationList.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("IMPLEMENTATION"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestImplementationVerification.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
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
    }

    private void BindCorrectiveAction()
    {
        try
        {
            DataSet ds = PhoenixInspectionMOCActionPlan.MOCActionPlanList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , new Guid((ViewState["MOCID"]).ToString()));

            gvCorrectiveAction.DataSource = ds;
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
            if (e.CommandName.ToUpper().Equals("EDITROW"))
            {
                RadLabel lblMOCActionPlanid = (RadLabel)e.Item.FindControl("lblMOCActionPlanid");
                Response.Redirect("../Inspection/InspectionMOCRequestActionPlanEdit.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&ACTIONPLANID=" + lblMOCActionPlanid.Text + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCActionPlan.MOCActionPlanDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , new Guid(((RadLabel)e.Item.FindControl("lblMOCActionPlanid")).Text));
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

            RadLabel pic = (RadLabel)e.Item.FindControl("lblPIC");
            LinkButton cb = (LinkButton)e.Item.FindControl("lnkTaskEdit");

            if ((pic.Text != null) && (pic.Text != ""))
                cb.Visible = true;

            LinkButton lnk = (LinkButton)e.Item.FindControl("lnkTaskEdit");
            RadLabel lblMOCActionPlanid = (RadLabel)e.Item.FindControl("lblMOCActionPlanid");
            if (lnk != null)
            {
                //lnk.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                lnk.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionMOCShipBoardTaskDetails.aspx?&Vesselid=" + ViewState["VESSELID"].ToString() + "&MOCActionplanid=" + lblMOCActionPlanid.Text + "&MOCID=" + ViewState["MOCID"].ToString() + "');return true;");

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

    protected void CheckAll(Object sender, EventArgs e)
    {
        RadCheckBox headerCheckBox = (sender as RadCheckBox);
        ArrayList SelectedForms = new ArrayList();
        Guid index = new Guid();

        foreach (GridDataItem dataItem in gvCorrectiveAction.MasterTableView.Items)
        {
            bool result = false;
            if (headerCheckBox.Checked == true)
            {
                if (dataItem.GetDataKeyValue("FLDMOCACTIONPLANID").ToString() != "")
                {
                    index = new Guid(dataItem.GetDataKeyValue("FLDMOCACTIONPLANID").ToString());
                    (dataItem.FindControl("chkSelect") as RadCheckBox).Checked = true;
                    result = true;
                }
                //dataItem.Selected = true;
            }
            else
            {
                (dataItem.FindControl("chkSelect") as RadCheckBox).Checked = false;
                InspectionFilter.CurrentMOCActionPlanComment = null;
            }

            // Check in the Session
            if (InspectionFilter.CurrentMOCActionPlanComment != null)
                SelectedForms = (ArrayList)InspectionFilter.CurrentMOCActionPlanComment;
            if (result)
            {
                if (!SelectedForms.Contains(index))
                    SelectedForms.Add(index);
            }
            else
                SelectedForms.Remove(index);
        }
        if (SelectedForms != null && SelectedForms.Count > 0)
            InspectionFilter.CurrentMOCActionPlanComment = SelectedForms;
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedSections = new ArrayList();
        Guid index = new Guid();

        foreach (GridDataItem item in gvCorrectiveAction.Items)
        {
            bool result = false;

            if (item.GetDataKeyValue("FLDMOCACTIONPLANID").ToString() != "")
            {
                index = new Guid(item.GetDataKeyValue("FLDMOCACTIONPLANID").ToString());

                if (((RadCheckBox)(item.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session
                if (InspectionFilter.CurrentMOCActionPlanComment != null)
                    SelectedSections = (ArrayList)InspectionFilter.CurrentMOCActionPlanComment;
                if (result)
                {
                    if (!SelectedSections.Contains(index))
                        SelectedSections.Add(index);
                }
                else
                    SelectedSections.Remove(index);
            }
        }
        if (SelectedSections != null && SelectedSections.Count > 0)
            InspectionFilter.CurrentMOCActionPlanComment = SelectedSections;
    }

}
