using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCIntermediateVerificationList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarstatus = new PhoenixToolbar();
            toolbarstatus.AddButton("List", "LIST", ToolBarDirection.Left);
            toolbarstatus.AddButton("Details", "DETAILS", ToolBarDirection.Left);
            toolbarstatus.AddButton("Action Plan", "ACTIONPLAN", ToolBarDirection.Left);
            toolbarstatus.AddButton("Evaluation & Approval", "EVALUATION", ToolBarDirection.Left);
            toolbarstatus.AddButton("Intermediate Verification", "INTERMEDIATE", ToolBarDirection.Left);
            toolbarstatus.AddButton("Implementation & Verification", "IMPLEMENTATION", ToolBarDirection.Left);

            MenuMOCStatus.MenuList = toolbarstatus.Show();
            MenuMOCStatus.SelectedMenuIndex = 4;

            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();
               
                ViewState["VESSELID"] = Request.QueryString["Vesselid"].ToString();
                if (Request.QueryString["MOCRequestid"] != null && Request.QueryString["MOCRequestid"].ToString() != "")
                    ViewState["MOCRequestid"] = Request.QueryString["MOCRequestid"].ToString();
                else
                    ViewState["MOCRequestid"] = "";
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../Inspection/InspectionMOCExtentionAdd.aspx?MOCID=" + ViewState["MOCID"].ToString()+"&VESSELID="+ ViewState["VESSELID"].ToString(), "Add New", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MOCExtentionAdd.MenuList = toolbarmain.Show();
            //MenuMOC.MenuList = toolbargrid.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            DataSet ds = PhoenixInspectionMOCIntermediateVerification.MOCIntermediateVerificationList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                              , new Guid(ViewState["MOCID"].ToString()));
                gvMOC.DataSource = ds;
         }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MOCIntermediateVerification_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestAdd.aspx?MOCID=" + ViewState["MOCID"].ToString() + "&MOCRequestid=" + ViewState["MOCRequestid"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            if (CommandName.ToUpper().Equals("ACTIONPLAN"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestActionPlan.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
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
    protected void gvMOC_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {

    }

    protected void gvMOC_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDITMOCMAINSECTION"))
            {
                Response.Redirect("../Inspection/InspectionManagementOfChangeList.aspx?MOCID=" + ((RadLabel)e.Item.FindControl("lblmocid")).Text +"&MOCINTERMEDIATEID="+ ((RadLabel)e.Item.FindControl("lblmocintermediateverificationid")).Text, false);
            }
            else if (e.CommandName.ToUpper().Equals("EDITEXTENTION"))
            {
                Response.Redirect("../Inspection/InspectionMOCIntermediateVerificationAdd.aspx?MOCID=" + ViewState["MOCID"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString() + "&MOCINTERMEDIATEID=" + ((RadLabel)e.Item.FindControl("lblmocintermediateverificationid")).Text, false);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOC_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridEditableItem)
            {
                RadLabel lblActiontaken = (RadLabel)e.Item.FindControl("lblActiontaken");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("Actiontaken");
                if (uct != null)
                {
                    lblActiontaken.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lblActiontaken.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
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
        ViewState["PAGENUMBER"] = 1;
        BindEtentionData();
        gvMOCExtention.Rebind();
    }

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string strCurrentVerificationDate = ((Label)gridView.Rows[rowIndex].FindControl("lblVerificationDate")).Text;
                string strPreviousVerificationDate = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblVerificationDate")).Text;

                if (strCurrentVerificationDate == strPreviousVerificationDate)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                }

                string strCurrentMOCProgressingasplannedYN = ((Label)gridView.Rows[rowIndex].FindControl("lblMOCProgressingasplannedYN")).Text;
                string strPreviousMOCProgressingasplannedYN = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblMOCProgressingasplannedYN")).Text;

                if (strCurrentMOCProgressingasplannedYN == strPreviousMOCProgressingasplannedYN)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }
                string strCurrentActiontaken = ((Label)gridView.Rows[rowIndex].FindControl("lblActiontaken")).Text;
                string strPreviousActiontaken = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblActiontaken")).Text;

                if (strCurrentActiontaken == strPreviousActiontaken)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                           previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }

                string strCurrentNextVerificationDueDate = ((Label)gridView.Rows[rowIndex].FindControl("lblNextVerificationDueDate")).Text;
                string strPreviousNextVerificationDueDate = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblNextVerificationDueDate")).Text;

                if (strCurrentNextVerificationDueDate == strPreviousNextVerificationDueDate)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                           previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }

            }
        }
    }

    protected void gvMOC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMOC.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvMOC.SelectedIndexes.Clear();
        gvMOC.EditIndexes.Clear();
        gvMOC.DataSource = null;
        gvMOC.Rebind();
    }
    protected void gvMOCExtention_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMOCExtention.CurrentPageIndex + 1;
            BindEtentionData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindEtentionData()
    {
        try
        {
            DataTable dt = PhoenixInspectionMOCExtention.MOCExtentionList(General.GetNullableGuid(ViewState["MOCID"].ToString()));
                gvMOCExtention.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMOCExtention_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {

    }
    protected void gvMOCExtention_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("EDITMOCEXTENTION"))
            {
                string Moc = ((RadLabel)e.Item.FindControl("lblmocid")).Text;
                string Id =((RadLabel)e.Item.FindControl("lblmocextentionid")).Text;
                string Vessel = ViewState["VESSELID"].ToString();
                Response.Redirect("../Inspection/InspectionMOCExtentionAdd.aspx?MOCID=" + Moc + "&MOCEXTENTIONID=" + Id+"&VESSELID="+Vessel, false);
            }
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                LinkButton approve = (LinkButton)e.Item.FindControl("cmdApproved");
                if (approve != null)
                {
                    approve.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Approval', '" + Session["sitepath"] + "/Inspection/InspectionMOCExtentionApproval.aspx?MOCEXTENTIONID=" +((RadLabel)e.Item.FindControl("lblmocextentionid")).Text + "&Vesselid=" + ViewState["VESSELID"].ToString() + "');return true;");
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCExtention_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadLabel lblActiontaken = (RadLabel)e.Item.FindControl("lblActiontaken");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("Actiontaken");
                if (uct != null)
                {
                    lblActiontaken.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lblActiontaken.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }

                LinkButton Approve = (LinkButton)e.Item.FindControl("cmdApproved");
                RadLabel Status = (RadLabel)e.Item.FindControl("lblstatusid");

                if(Status.Text=="3")
                {
                    Approve.Visible = true;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCExtention_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void gvMOCExtention_PreRender(object sender, EventArgs e)
    {
        //GridDecorator1.MergeRows(gvMOCExtention);
    }

    public class GridDecorator1
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string strCurrentDateExtentionissued = ((Label)gridView.Rows[rowIndex].FindControl("lblDateExtentionissued")).Text;
                string strPreviousDateExtentionissued = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblDateExtentionissued")).Text;

                if (strCurrentDateExtentionissued == strPreviousDateExtentionissued)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                }

                string strCurrentOriginalTargetDate = ((Label)gridView.Rows[rowIndex].FindControl("lblOriginalTargetDate")).Text;
                string strPreviousOriginalTargetDate = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblOriginalTargetDate")).Text;

                if (strCurrentOriginalTargetDate == strPreviousOriginalTargetDate)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }
                string strCurrentRevisedTargetDate = ((Label)gridView.Rows[rowIndex].FindControl("lblRevisedTargetDate")).Text;
                string strPreviousRevisedTargetDate = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblRevisedTargetDate")).Text;

                if (strCurrentRevisedTargetDate == strPreviousRevisedTargetDate)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                           previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }

                string strCurrentActiontaken = ((Label)gridView.Rows[rowIndex].FindControl("lblActiontaken")).Text;
                string strPreviousActiontaken = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblActiontaken")).Text;

                if (strCurrentActiontaken == strPreviousActiontaken)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                           previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }

                string strCurrentApprovingAuthority = ((Label)gridView.Rows[rowIndex].FindControl("lblApprovingAuthority")).Text;
                string strPreviousApprovingAuthority = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblApprovingAuthority")).Text;

                if (strCurrentApprovingAuthority == strPreviousApprovingAuthority)
                {
                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                           previousRow.Cells[4].RowSpan + 1;
                    previousRow.Cells[4].Visible = false;
                }
            }
        }
    }

}

