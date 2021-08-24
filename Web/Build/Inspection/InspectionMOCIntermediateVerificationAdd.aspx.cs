using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCIntermediateVerificationAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Int.Verification Summary", "INTVERIFICATION", ToolBarDirection.Right);
        //toolbarmain.AddButton("List", "BACK", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

        MenuMOCRequest.MenuList = toolbarmain.Show();       

        if (!IsPostBack)
        {
            ViewState["MOCID"] = "";
            ViewState["DepartmentAdd"] = "";
            ViewState["DepartmentEdit"] = "";
            ViewState["txtActionToBeTakenAdd"] = "";
            ViewState["DepartmentExtentionAdd"] = "";
            ViewState["DepartmentExtentionEdit"] = "";
            ViewState["txtActionToBeTakenExtentionAdd"] = "";
            ViewState["MOCINTERMEDIATEID"] = "";
            ViewState["MOCACTIONID"] = "";
            ViewState["MOCRISKASSESSMENTID"] = "";
            ViewState["RISKASSESSMENTID"] = "";
            ViewState["VESSELID"] = "";

            if (Request.QueryString["MOCID"] != null && Request.QueryString["MOCID"].ToString() != "")
                ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();
            
            if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != "")
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            
            if (Request.QueryString["MOCINTERMEDIATEID"] != null && Request.QueryString["MOCINTERMEDIATEID"].ToString() != "")
                ViewState["MOCINTERMEDIATEID"] = Request.QueryString["MOCINTERMEDIATEID"].ToString();

            BindMOCIntermediateVerification();

        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMOCRequestIntermediateActionPlanEdit.aspx?VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&MOCINTERMEDIATEID=" + ViewState["MOCINTERMEDIATEID"].ToString(), "Add New", "<i class=\"fa fa-plus-circle\"></i>", "CORRECTIVEACTIONADD");
        MenuCA.AccessRights = this.ViewState;
        MenuCA.MenuList = toolbar.Show();
        BindActionPlan();
        MOCPlanned();
    }

    protected void MenuCA_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void MenuExtention_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void gvMOCActionPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMOCActionPlan.CurrentPageIndex + 1;
            BindActionPlan();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void MOCRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidMOCRequest(txtCompletionDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionMOCIntermediateVerification.MOCIntermediateVerificationInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableGuid((ViewState["MOCINTERMEDIATEID"]).ToString())
                                                            , General.GetNullableGuid((ViewState["MOCID"]).ToString())
                                                            , int.Parse(ViewState["VESSELID"].ToString())
                                                            , General.GetNullableDateTime(txtIntVerificationDate.Text)
                                                            , General.GetNullableInteger((rblMOCPlanned.SelectedValue))
                                                            , General.GetNullableDateTime(txtCompletionDate.Text)
                                                            , General.GetNullableInteger((rblMOCPlanned.SelectedValue))
                                                            , General.GetNullableInteger((rblMOCTargetDateExtended.SelectedValue))
                                                            ,null
                                                            , null
                                                            , "10"
                                                            , null
                                                            , "MOC COMMITTEE"
                                                            , null
                                                            , null
                                                            , null
                                                            , 10);

                BindMOCIntermediateVerification();
                ucStatus.Text = "MOC Intermediate Verification updated successfully.";
                BindActionPlan();
            }
            //else if (CommandName.ToUpper().Equals("BACK"))
            //{
            //    Response.Redirect("../Inspection/InspectionMOCIntermediateVerificationList.aspx?MOCID=" + new Guid((ViewState["MOCID"]).ToString()) + "&MOCRequestid=" + ViewState["MOCREQUESTID"].ToString() + "&Vesselid=" + ViewState["Vesselid"].ToString(), true);
            //}
            else if (CommandName.ToUpper().Equals("INTVERIFICATION"))
            {
                Response.Redirect("../Inspection/InspectionMOCIntermediateVerificationList.aspx?MOCID=" + new Guid((ViewState["MOCID"]).ToString()) +  "&Vesselid=" + ViewState["VESSELID"].ToString(), true);
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
    private void BindMOCIntermediateVerification()
    {
        DataSet ds;

        if (ViewState["MOCINTERMEDIATEID"] != null && !string.IsNullOrEmpty(ViewState["MOCINTERMEDIATEID"].ToString()))
        {
            ds = PhoenixInspectionMOCIntermediateVerification.MOCIntermediateVerificationEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , new Guid(ViewState["MOCINTERMEDIATEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtIntVerificationDate.Text = dr["FLDMOCDUEDATE"].ToString();
                txtCompletionDate.Text = dr["FLDCOMPLETEDDATE"].ToString();
                rblMOCPlanned.SelectedValue = dr["FLDMOCACTIONSADEQUATEYN"].ToString();
                rblMOCTargetDateExtended.SelectedValue = dr["FLDTARGETDATEEXTENDED"].ToString();
                txtmoctype.Text = dr["FLDVESSELID"].ToString();               
            }
        }
    }
    protected void rblMOCPlanned_changed(object sender, EventArgs e)
    {
        MOCPlanned();
    }
    protected void txtCompletionDate_TextChanged(object sender, EventArgs e)
    {
    }
    protected void rblMOCTargetDateExtended_changed(object sender, EventArgs e)
    {
 
    }

    private void BindActionPlan()
    {
        string[] alColumns = { "FLDACTIONTOBETAKEN", "FLDPERSONINCHARGE", "FLDDOCUMENTATTACHMENT", "FLDTARGETDATE", "FLDCOMPLETIONDATE" };
        string[] alCaptions = { "Actions to be taken", "Person In Charge", "Documents to be uploaded as evidence", "Target date", "Completion date" };

        DataSet ds = PhoenixInspectionMOCIntermediateVerification.MOCIntermediateActionPlanList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , General.GetNullableGuid((ViewState["MOCID"]).ToString())
                                                                                        , General.GetNullableGuid((ViewState["MOCINTERMEDIATEID"]).ToString())
                                                                                        , General.GetNullableGuid(null));

        gvMOCActionPlan.DataSource = ds;

    }
    protected void gvMOCActionPlan_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindActionPlan();
        ViewState["DepartmentEdit"] = "";
    }
    protected void gvMOCActionPlan_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDITROW"))
            {
                RadLabel lblMOCActionPlanid = (RadLabel)e.Item.FindControl("lblMOCActionPlanid");
                Response.Redirect("../Inspection/InspectionMOCRequestIntermediateActionPlanEdit.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&ACTIONPLANID=" + lblMOCActionPlanid.Text + "&MOCID=" + ViewState["MOCID"].ToString() + "&MOCINTERMEDIATEID=" + Request.QueryString["MOCINTERMEDIATEID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCActionPlan.MOCActionPlanDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , new Guid(((RadLabel)e.Item.FindControl("lblMOCActionPlanid")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCActionPlan_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindActionPlan();
    }
    protected void gvMOCActionPlan_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindActionPlan();
    }
    protected void gvMOCActionPlan_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            string deparmenttypeid = "";
            DataSet ds;
            if (((UserControlDepartment)e.Item.FindControl("ucDepartmentedit")).SelectedDepartment != "Dummy")
            {
                ds = PhoenixRegistersDepartment.EditDepartment(int.Parse(((UserControlDepartment)e.Item.FindControl("ucDepartmentedit")).SelectedDepartment));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    deparmenttypeid = dr["FLDDEPARTMENTTYPEID"].ToString();
                }
            }

            if (!IsValidMOCActionPlan(((RadTextBox)e.Item.FindControl("txtActionToBeTakenEdit")).Text
                                        , ((UserControlDate)e.Item.FindControl("txtTargetdateEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionMOCActionPlan.MOCActionPlanUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                     , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblMOCActionPlanid")).Text)
                                                                     , new Guid((ViewState["MOCID"]).ToString())
                                                                     , int.Parse(ViewState["VESSELID"].ToString())
                                                                     , General.GetNullableGuid((ViewState["MOCINTERMEDIATEID"]).ToString())
                                                                     , General.GetNullableGuid(null)
                                                                     , ((RadTextBox)e.Item.FindControl("txtActionToBeTakenEdit")).Text
                                                                     , (deparmenttypeid == "2") ? General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtPersonOfficeIdEdit")).Text) : General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtCrewIdEdit")).Text)
                                                                     , (deparmenttypeid == "2") ? ((RadTextBox)e.Item.FindControl("txtPersonNameEdit")).Text : ((RadTextBox)e.Item.FindControl("txtCrewNameEdit")).Text
                                                                     , (deparmenttypeid == "2") ? ((RadTextBox)e.Item.FindControl("txtPersonRankEdit")).Text : ((RadTextBox)e.Item.FindControl("txtCrewRankEdit")).Text
                                                                     , ((RadTextBox)e.Item.FindControl("txtDocumentsEdit")).Text
                                                                     , DateTime.Parse(((UserControlDate)e.Item.FindControl("txtTargetdateEdit")).Text)
                                                                     , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtCompletiondateEdit")).Text)
                                                                     , General.GetNullableInteger(((UserControlDepartment)e.Item.FindControl("ucDepartmentedit")).SelectedDepartment)
                                                                     , General.GetNullableInteger(deparmenttypeid)
                                                                     , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtremarksedit")).Text)
                                                                     , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtRescheduledateEdit")).Text)
                                                                     , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRescheduleremarksEdit")).Text)
                                                                     , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtcloseddateEdit")).Text)
                                                                     , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblcompletedby")).Text)
                                                                     , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblclosedby")).Text)
                                                                     , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblverfication")).Text));


            //_gridView.EditIndex = -1;
            BindActionPlan();
            ViewState["DepartmentEdit"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void gvMOCActionPlan_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            LinkButton imgPersonOfficeEdit = (LinkButton)e.Item.FindControl("imgPersonOfficeEdit");
            HtmlControl actionplanofficeedit = (HtmlControl)e.Item.FindControl("actionplanofficeedit");
            HtmlControl actionplancrewedit = (HtmlControl)e.Item.FindControl("actionplancrewedit");
            LinkButton imgPersonInChargeEdit = (LinkButton)e.Item.FindControl("imgPersonInChargeEdit");
            RadLabel lbldepartmentid = (RadLabel)e.Item.FindControl("lbldepartmentid");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselId");
            UserControlDepartment ucDepartmentEdit = (UserControlDepartment)e.Item.FindControl("ucDepartmentEdit");
            RadTextBox txtCrewNameEdit = (RadTextBox)e.Item.FindControl("txtCrewNameEdit");
            RadTextBox txtCrewRankEdit = (RadTextBox)e.Item.FindControl("txtCrewRankEdit");
            RadTextBox txtCrewIdEdit = (RadTextBox)e.Item.FindControl("txtCrewIdEdit");
            RadTextBox txtPersonNameEdit = (RadTextBox)e.Item.FindControl("txtPersonNameEdit");
            RadTextBox txtPersonRankEdit = (RadTextBox)e.Item.FindControl("txtPersonRankEdit");
            RadTextBox txtPersonOfficeIdEdit = (RadTextBox)e.Item.FindControl("txtPersonOfficeIdEdit");
            RadTextBox txtPersonOfficeEmailEdit = (RadTextBox)e.Item.FindControl("txtPersonOfficeEmailEdit");

            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                //if (drv["FLDISATTACHMENT"].ToString() == "0")
                //    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=MOCACTIONPLAN&cmdname=MOCACTIONPLANUPLOAD&VESSELID=" + lblVesselid.Text + "'); return true;");
            }

            if (e.Item.IsInEditMode)
            {
                if ((ViewState["DepartmentEdit"].ToString() == ""))
                {
                    ucDepartmentEdit.SelectedDepartment = lbldepartmentid.Text;
                }
                else
                {
                    ucDepartmentEdit.SelectedDepartment = ViewState["DepartmentEdit"].ToString();
                }
                string deparmenttypeid = "";
                DataSet ds;
                ds = PhoenixRegistersDepartment.EditDepartment(int.Parse(ucDepartmentEdit.SelectedDepartment));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    deparmenttypeid = dr["FLDDEPARTMENTTYPEID"].ToString();
                }
                if (ucDepartmentEdit.SelectedDepartment != lbldepartmentid.Text)
                {
                    txtCrewNameEdit.Text = "";
                    txtCrewRankEdit.Text = "";
                    txtCrewIdEdit.Text = "";
                    txtPersonNameEdit.Text = "";
                    txtPersonRankEdit.Text = "";
                    txtPersonOfficeIdEdit.Text = "";
                    txtPersonOfficeEmailEdit.Text = "";
                }


                if (deparmenttypeid == "2")
                {
                    actionplancrewedit.Visible = false;
                    if (imgPersonOfficeEdit != null)
                    {
                        imgPersonOfficeEdit.Attributes.Add("onclick", "return showPickList('spnActionPlanPersonOfficeEdit', 'codehelp1', '', '../Common/CommonPickListUser.aspx?Departmentid=" + ucDepartmentEdit.SelectedDepartment + "&MOC=true', true);");
                    }
                }
                if (deparmenttypeid == "1")
                {
                    actionplanofficeedit.Visible = false;
                    if (imgPersonInChargeEdit != null)
                    {
                        imgPersonInChargeEdit.Attributes.Add("onclick", "return showPickList('spnPersonInChargeactionplanEdit', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                                                        + ViewState["VESSELID"].ToString().ToString() + "', true); ");
                    }
                }
                if ((ucDepartmentEdit.SelectedDepartment == "0") || (ucDepartmentEdit.SelectedDepartment == "Dummy"))
                {
                    actionplancrewedit.Visible = false;
                    actionplanofficeedit.Visible = false;
                }
            }
        }
    }
    private bool IsValidMOCActionPlan(string ActionToBeTaken, string Targetdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ActionToBeTaken.Equals(""))
            ucError.ErrorMessage = "Actions to be taken is Required";

        if (General.GetNullableDateTime(Targetdate) == null)
            ucError.ErrorMessage = "Target date is Required";

        return (!ucError.IsError);
    }
    private void MOCPlanned()
    {
        if ((rblMOCPlanned.SelectedValue == "0") || (rblMOCPlanned.SelectedValue == ""))
        {
            lblCompletionDate.Visible = true;
            txtCompletionDate.Visible = true;
            lblActionPlan.Visible = false;
            lblMOCTargetDateExtended.Visible = false;
            rblMOCTargetDateExtended.Visible = false;
            gvMOCActionPlan.Visible = false;
            moetarget.Visible = false;
            MenuCA.Visible = false;
           
        }
        else
        {
            lblActionPlan.Visible = true;
            lblMOCTargetDateExtended.Visible = true;
            rblMOCTargetDateExtended.Visible = true;
            gvMOCActionPlan.Visible = true;
            moetarget.Visible = true;
            MenuCA.Visible = true;
        }
    }
     private bool IsValidMOCRequest(string completionDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(completionDate) == null)
            ucError.ErrorMessage = "Date Intermediate verification completed is required.";
        return (!ucError.IsError);
    }
    public void ucDepartmentAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        //UserControlDepartment ucDepartmentAdd = gvMOCActionPlan.FooterRow.FindControl("ucDepartmentAdd") as UserControlDepartment;
        //RadTextBox txtActionToBeTakenAdd = gvMOCActionPlan.FooterRow.FindControl("txtActionToBeTakenAdd") as RadTextBox;

        //ViewState["DepartmentAdd"] = ucDepartmentAdd.SelectedDepartment;
        //ViewState["txtActionToBeTakenAdd"] = txtActionToBeTakenAdd.Text;
        //BindActionPlan();
    }
    public void ucDepartmentEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        //int nCurrentRow = gvMOCActionPlan.SelectedRow.RowIndex;
        //UserControlDepartment ucDepartmentEdit = gvMOCActionPlan.Rows[nCurrentRow].FindControl("ucDepartmentEdit") as UserControlDepartment;

        //ViewState["DepartmentEdit"] = ucDepartmentEdit.SelectedDepartment;
        //BindActionPlan();
    }

    public void ucDepartmentExtentionAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        //UserControlDepartment ucDepartmentAdd = gvMOCActionPlanExtention.FooterRow.FindControl("ucDepartmentAdd") as UserControlDepartment;
        //RadTextBox txtActionToBeTakenAdd = gvMOCActionPlanExtention.FooterRow.FindControl("txtActionToBeTakenAdd") as RadTextBox;

        //ViewState["DepartmentExtentionAdd"] = ucDepartmentAdd.SelectedDepartment;
        //ViewState["txtActionToBeTakenExtentionAdd"] = txtActionToBeTakenAdd.Text;
        //BindExtetention();
    }
    public void ucDepartmentExtentionEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        //int nCurrentRow = gvMOCActionPlanExtention.SelectedRow.RowIndex;
        //UserControlDepartment ucDepartmentEdit = gvMOCActionPlanExtention.Rows[nCurrentRow].FindControl("ucDepartmentEdit") as UserControlDepartment;

        //ViewState["DepartmentExtentionEdit"] = ucDepartmentEdit.SelectedDepartment;
        //BindExtetention();
    }
}
