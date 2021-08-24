using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.IO;
using Telerik.Web.UI;

public partial class InspectionSupdtEventFeedbackBanner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["sourcefrom"] = "0";
                BindInspectionEvent();
                BindFeedbackCategory();
                BindFeedbackSubCategory();
                BindEventFeedback();
                PhoenixToolbar toolbar = new PhoenixToolbar();
                ViewState["FEEDBACKDTKEY"] = null;


                toolbar.AddButton("Attachments", "ATTACHMENT", ToolBarDirection.Right);
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New", "CLEAR", ToolBarDirection.Right);
                MenuSupdtEventFeedback.AccessRights = this.ViewState;
                MenuSupdtEventFeedback.MenuList = toolbar.Show();
            }
            BindAssignedTo();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindEventFeedback()
    {
        DataSet ds = PhoenixInspectionEventSupdtFeedback.EventSupdtFeedbackEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableInteger(ViewState["sourcefrom"].ToString())
            , null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            if (General.GetNullableDateTime(dr["FLDEVENTFEEDBACKDATE"].ToString()) != null)
            {
                ucEventDate.Text = dr["FLDEVENTFEEDBACKDATE"].ToString();
            }
            else
            {
                ucEventDate.Text = DateTime.Now.ToString();
            }

            ucRecordedDate.Text = dr["FLDRECORDEDDATE"].ToString();
            txtSupdtName.Text = dr["FLDSUPDTNAME"].ToString();
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
            ddlEvent.SelectedValue = dr["FLDINSPECTIONEVENTID"].ToString();

            if (General.GetNullableInteger(dr["FLDVESSELID"].ToString()) != null)
                ucVessel.Enabled = false;
        }
    }

    protected void MenuSupdtEventFeedback_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE") && ViewState["FEEDBACKDTKEY"] == null)
            {
                if (!IsValidSupdtEvent())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    Guid? eventfeedbackidout = null;
                    Guid? eventfeedbackdtkeyout = null;
                    PhoenixInspectionEventSupdtFeedback.InsertEventSupdtFeedback(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ddlEvent.SelectedValue)
                        , General.GetNullableInteger(ucVessel.SelectedVessel)
                        , int.Parse(ViewState["sourcefrom"].ToString())
                        , null
                        , General.GetNullableDateTime(ucEventDate.Text)
                        , General.GetNullableDateTime(ucRecordedDate.Text)
                        , ref eventfeedbackidout
                        , ref eventfeedbackdtkeyout);

                    string source = GetAssignedTo();
                    PhoenixInspectionEventSupdtFeedback.InsertSupdtEmployeewiseFeedbackBulk(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , eventfeedbackidout
                        , General.GetNullableGuid(ddlFeedbackCategory.SelectedValue)
                        , General.GetNullableGuid(ddlFeedbackSubCategory.SelectedValue)
                        , General.GetNullableString(txtRemarks.Text)
                        , source
                        , General.GetNullableInteger(ucKeyAnchor.SelectedQuick)
                        , General.GetNullableDateTime(ucExpectedClosingDate.Text)
                        , null
                        , null
                        , null);

                    ucStatus.Text = "Information Updated";

                    ViewState["FEEDBACKDTKEY"] = new Guid(eventfeedbackdtkeyout.ToString());
                    ViewState["VESSELID"] = ucVessel.SelectedVessel;
                }
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearControls();
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                if (ViewState["FEEDBACKDTKEY"] != null)
                {
                    String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey= " + ViewState["FEEDBACKDTKEY"].ToString()
                                 + "&mod=" + PhoenixModule.QUALITY
                                 + "&type=AUDITINSPECTION"
                                 + "&cmdname=AUDITINSPECTIONUPLOAD"
                                 + "&VESSELID=" + ViewState["VESSELID"].ToString()
                                 + "');");

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    ucError.ErrorMessage = "Save before adding attachment";
                    ucError.Visible = true;
                    return;
                }
            }
            if (ViewState["FEEDBACKDTKEY"] == null || ViewState["FEEDBACKDTKEY"].ToString() == "")
            {
                Response.Redirect("../Inspection/InspectionSupdtEventFeedbackBanner.aspx", false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void ClearControls()
    {
        ucVessel.SelectedVessel = "";
        ddlEvent.SelectedValue = "Dummy";
        ddlFeedbackCategory.SelectedValue = "Dummy";
        ddlFeedbackSubCategory.SelectedValue = "Dummy";
        txtRemarks.Text = "";
        ucEventDate.Text = DateTime.Today.ToString();
        ucRecordedDate.Text = DateTime.Today.ToString();
        ucKeyAnchor.SelectedQuick = "";
        ViewState["FEEDBACKDTKEY"] = null;
        foreach (GridDataItem gvr in gvAssignedTo.Items)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkSelect");
            if (chk != null)
            {
                if (chk.Checked == true)
                {
                    chk.Checked = false;
                }
            }
        }
        BindAssignedTo();
    }
    private bool IsValidSupdtEvent()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableGuid(ddlEvent.SelectedValue) == null)
            ucError.ErrorMessage = "Event is required.";

        if (General.GetNullableDateTime(ucEventDate.Text) == null)
            ucError.ErrorMessage = "Event date is required.";

        if (General.GetNullableDateTime(ucRecordedDate.Text) == null)
            ucError.ErrorMessage = "Date is required.";

        if (chkaction.Checked == true)
        {
            if (General.GetNullableDateTime(ucExpectedClosingDate.Text) == null)
                ucError.ErrorMessage = "Expected Closing Date is required.";
        }

        if (General.GetNullableGuid(ddlFeedbackCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Feedback Category is required.";

        if (General.GetNullableString(txtRemarks.Text) == null)
            ucError.ErrorMessage = "Remarks is required.";

        if (General.GetNullableInteger(ucKeyAnchor.SelectedQuick) == null)
            ucError.ErrorMessage = "Key Anchor is required.";

        bool flag = false;
        foreach (GridDataItem gvr in gvAssignedTo.Items)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkSelect");
            if (chk != null)
            {
                if (chk.Checked == true)
                {
                    flag = true;
                    break;
                }
            }
        }
        if (flag == false)
        {
            ucError.ErrorMessage = "Please check atleast one Crew member.";
        }

        return (!ucError.IsError);
    }

    protected void BindInspectionEvent()
    {
        ddlEvent.DataSource = PhoenixInspectionEvent.ListInspectionEvent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , null);
        ddlEvent.DataTextField = "FLDEVENTNAME";
        ddlEvent.DataValueField = "FLDINSPECTIONEVENTID";
        ddlEvent.DataBind();
        ddlEvent.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));

    }
    protected void BindFeedbackCategory()
    {
        ddlFeedbackCategory.DataSource = PhoenixInspectionFeedBackCategory.ListFeedBackCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null);
        ddlFeedbackCategory.DataTextField = "FLDFEEDBACKCATEGORYNAME";
        ddlFeedbackCategory.DataValueField = "FLDFEEDBACKCATEGORYID";
        ddlFeedbackCategory.DataBind();
        ddlFeedbackCategory.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));

    }
    protected void BindFeedbackSubCategory()
    {
        ddlFeedbackSubCategory.DataSource = PhoenixInspectionFeedbackSubCategory.ListFeedbackSubCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        , null);
        ddlFeedbackSubCategory.DataTextField = "FLDFEEDBACKSUBCATEGORYNAME";
        ddlFeedbackSubCategory.DataValueField = "FLDFEEDBACKSUBCATEGORYID";
        ddlFeedbackSubCategory.DataBind();
        ddlFeedbackSubCategory.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
    }

    protected void ucEventDate_Changed(object sender, EventArgs e)
    {
        BindAssignedTo();
    }
    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        BindAssignedTo();
    }
    protected void ddlFeedback_Changed(object sender, EventArgs e)
    {
        DataSet ds = PhoenixInspectionFeedBackCategory.ListFeedBackCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ddlFeedbackCategory.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {
            chkaction.Checked = ds.Tables[0].Rows[0]["FLDACTIONREQUIRED"].ToString() == "1" ? true : false;
        }
        if(chkaction.Checked == true )
        {
            ucExpectedClosingDate.CssClass = "input_mandatory";
            ucExpectedClosingDate.Enabled = true;
        }
        else
        {
            ucExpectedClosingDate.CssClass = "input";
            ucExpectedClosingDate.Enabled = false;
        }
    }
   
    public void BindAssignedTo()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = string.Empty;
        date = ucEventDate.Text;
        if (General.GetNullableDateTime(date) == null)
        {
            date = "1900/01/01";
        }

        DataSet ds = PhoenixCommonInspection.SearchVesselEmployee(General.GetNullableInteger(ucVessel.SelectedVessel),
                 General.GetNullableDateTime(date), null,
                 null, 0,
                 1,
                 40,
                 ref iRowCount,
                 ref iTotalPageCount);


        gvAssignedTo.DataSource = ds;



    }

    private string GetAssignedTo()
    {
        string list = string.Empty;

        foreach (GridDataItem gvr in gvAssignedTo.Items)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkSelect");
            RadLabel lblSignonoffId = (RadLabel)gvr.FindControl("lblSignonoffId");

            if (chk != null && lblSignonoffId != null)
            {
                if (chk.Checked == true)
                {
                    list = list + lblSignonoffId.Text.Trim() + ",";
                }
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindAssignedTo();
    }

    protected void gvAssignedTo_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindAssignedTo();
    }

    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvAssignedTo$ctl01$chkAllAssignedTo")
        {
            CheckBox chkAll = (CheckBox)gvAssignedTo.FindControl("chkAllAssignedTo");
            foreach (GridDataItem row in gvAssignedTo.Items)
            {
                CheckBox cbSelected = (CheckBox)row.FindControl("chkSelect");
                if (cbSelected != null)
                {
                    if (chkAll.Checked == true)
                    {
                        cbSelected.Checked = true;
                    }
                    else
                    {
                        cbSelected.Checked = false;
                    }
                }
            }
        }

    }


}
