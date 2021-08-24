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
using Telerik.Web.UI;
public partial class InspectionSupdtEventFeedback : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["OTHERREFERENCEID"] = "";
                if (Request.QueryString["SOURCEREFERENCEID"] != null && Request.QueryString["SOURCEREFERENCEID"].ToString() != string.Empty)
                    ViewState["SOURCEREFERENCEID"] = Request.QueryString["SOURCEREFERENCEID"].ToString();
                if (Request.QueryString["OTHERREFERENCEID"] != null && Request.QueryString["OTHERREFERENCEID"].ToString() != string.Empty)
                    ViewState["OTHERREFERENCEID"] = Request.QueryString["OTHERREFERENCEID"].ToString();

                if (Request.QueryString["sourcefrom"] != null && Request.QueryString["sourcefrom"].ToString() != string.Empty)
                    ViewState["sourcefrom"] = Request.QueryString["sourcefrom"].ToString();

                ViewState["VESSELID"] = "0";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindInspectionEvent();
                BindEventFeedback();
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (Request.QueryString["viewonly"] == null)
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuSupdtEventFeedback.AccessRights = this.ViewState;
                MenuSupdtEventFeedback.MenuList = toolbar.Show();
            }
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
            , General.GetNullableGuid(ViewState["SOURCEREFERENCEID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["EVENTFEEDBACKID"] = dr["FLDEVENTFEEDBACKID"].ToString();
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            ucEventDate.Text = dr["FLDEVENTFEEDBACKDATE"].ToString();
            ucRecordedDate.Text = dr["FLDRECORDEDDATE"].ToString();
            txtSupdtName.Text = dr["FLDSUPDTNAME"].ToString();
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
            ddlEvent.SelectedValue = dr["FLDINSPECTIONEVENTID"].ToString();
        }
    }

    protected void MenuSupdtEventFeedback_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSupdtEvent(ddlEvent.SelectedValue, ucEventDate.Text, ucRecordedDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    if (General.GetNullableGuid(ViewState["EVENTFEEDBACKID"].ToString()) == null)
                    {
                        Guid? eventfeedbackidout = null;
                        PhoenixInspectionEventSupdtFeedback.InsertEventSupdtFeedback(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableGuid(ddlEvent.SelectedValue)
                            , null
                            , int.Parse(ViewState["sourcefrom"].ToString())
                            , General.GetNullableGuid(ViewState["SOURCEREFERENCEID"].ToString())
                            , General.GetNullableDateTime(ucEventDate.Text)
                            , General.GetNullableDateTime(ucRecordedDate.Text)
                            , ref eventfeedbackidout);
                        ucStatus.Text = "Information Updated";
                    }
                    if (General.GetNullableGuid(ViewState["EVENTFEEDBACKID"].ToString()) != null)
                    {
                        PhoenixInspectionEventSupdtFeedback.UpdateEventSupdtFeedback(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableGuid(ViewState["EVENTFEEDBACKID"].ToString())
                            , General.GetNullableGuid(ddlEvent.SelectedValue)
                            , General.GetNullableDateTime(ucEventDate.Text)
                            , General.GetNullableDateTime(ucRecordedDate.Text)
                            );

                        PhoenixInspectionEventSupdtFeedback.UpdateEventSupdtEmployeewiseFeedbackShowYN(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableGuid(ViewState["EVENTFEEDBACKID"].ToString())
                            , General.GetNullableGuid(ViewState["OTHERREFERENCEID"].ToString()));
                        ucStatus.Text = "Information Updated";
                    }
                    BindEventFeedback();
                    Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["EMPLOYEEFEEDBACKID"] = null;
            Rebind();

            for (int i = 0; i < gvFeedback.Items.Count; i++)
            {
                if (gvFeedback.MasterTableView.Items[i].GetDataKeyValue("FLDEMPLOYEEFEEDBACKID").ToString() == (ViewState["EMPLOYEEFEEDBACKID"] == null ? null : ViewState["EMPLOYEEFEEDBACKID"].ToString()))
                {
                    gvFeedback.MasterTableView.Items[i].Selected = true;
                    break;
                }
            }
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
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDFEEDBACKCATEGORYNAME", "FLDVESSELNAME", "FLDEMPLOYEENAME", "FLDREMARKS" };
            string[] alCaptions = { "Feedback Category Name", "Vessel Name", "Assigned To", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInspectionEventSupdtFeedback.EventSupdtEmployeewiseFeedbackSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(ViewState["EVENTFEEDBACKID"].ToString())
                , General.GetNullableGuid(ViewState["OTHERREFERENCEID"].ToString())
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvFeedback", "Supt Remarks", alCaptions, alColumns, ds);

            gvFeedback.DataSource = ds;
            gvFeedback.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["EMPLOYEEFEEDBACKID"] == null || ViewState["EMPLOYEEFEEDBACKID"].ToString() == "")
                {
                    ViewState["EMPLOYEEFEEDBACKID"] = ds.Tables[0].Rows[0]["FLDEMPLOYEEFEEDBACKID"].ToString();
                    gvFeedback.SelectedIndexes.Clear();
                }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
            }
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidSupdtRemarks(string feedbackcategory, string crewid, string remarks, string keyanchor)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableGuid(feedbackcategory) == null)
            ucError.ErrorMessage = "Feedback Category is required.";

        if (General.GetNullableInteger(crewid) == null)
            ucError.ErrorMessage = "Employee is required.";

        if (General.GetNullableString(remarks) == null)
            ucError.ErrorMessage = "Remarks is required.";

        if (General.GetNullableInteger(keyanchor) == null)
            ucError.ErrorMessage = "Key Anchor is required.";

        return (!ucError.IsError);
    }

    private bool IsValidSupdtEvent(string feedbackevent, string eventdate, string recordeddate)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableGuid(feedbackevent) == null)
            ucError.ErrorMessage = "Event is required.";

        if (General.GetNullableDateTime(eventdate) == null)
            ucError.ErrorMessage = "Event date is required.";

        if (General.GetNullableDateTime(recordeddate) == null)
            ucError.ErrorMessage = "Date is required.";

        return (!ucError.IsError);
    }
    protected void gvFeedback_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindValue(e.Item.ItemIndex);
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadComboBox ddlFeedbackCategoryAdd = (RadComboBox)e.Item.FindControl("ddlFeedbackCategoryAdd");
                RadComboBox ddlFeedbackSubCategoryAdd = (RadComboBox)e.Item.FindControl("ddlFeedbackSubCategoryAdd");
                RadComboBox ddlAssignedToAdd = (RadComboBox)e.Item.FindControl("ddlAssignedToAdd");
                RadTextBox txtRemarksAdd = (RadTextBox)e.Item.FindControl("txtRemarksAdd");
                string keyAnchorAdd = ((UserControlQuick)e.Item.FindControl("ucKeyAnchorAdd")).SelectedValue;

                if (General.GetNullableGuid(ViewState["EVENTFEEDBACKID"].ToString()) == null)
                {
                    if (!IsValidSupdtEvent(ddlEvent.SelectedValue, ucEventDate.Text, ucRecordedDate.Text))
                    {
                        e.Canceled = true;
                        ucError.Visible = true;
                        return;
                    }
                    if (!IsValidSupdtRemarks(ddlFeedbackCategoryAdd.SelectedValue, ddlAssignedToAdd.SelectedValue, txtRemarksAdd.Text, keyAnchorAdd))
                    {
                        e.Canceled = true;
                        ucError.Visible = true;
                        return;
                    }
                    Guid? eventfeedbackidout = null;
                    PhoenixInspectionEventSupdtFeedback.InsertEventSupdtFeedback(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ddlEvent.SelectedValue)
                        , null
                        , int.Parse(ViewState["sourcefrom"].ToString())
                        , General.GetNullableGuid(ViewState["SOURCEREFERENCEID"].ToString())
                        , General.GetNullableDateTime(ucEventDate.Text)
                        , General.GetNullableDateTime(ucRecordedDate.Text)
                        , ref eventfeedbackidout);
                    ucStatus.Text = "Information Updated";
                    ViewState["EVENTFEEDBACKID"] = eventfeedbackidout;

                    PhoenixInspectionEventSupdtFeedback.InsertEventSupdtEmployeewiseFeedback(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , General.GetNullableGuid(ViewState["EVENTFEEDBACKID"].ToString())
                     , General.GetNullableInteger(ddlAssignedToAdd.SelectedValue)
                     , General.GetNullableGuid(ViewState["OTHERREFERENCEID"].ToString())
                     , General.GetNullableGuid(ddlFeedbackCategoryAdd.SelectedValue)
                     , General.GetNullableGuid(ddlFeedbackSubCategoryAdd.SelectedValue)
                     , General.GetNullableString(txtRemarksAdd.Text)
                     , General.GetNullableInteger(keyAnchorAdd));

                    ucStatus.Text = "Information Updated";
                    Rebind();
                }
                else
                {
                    if (!IsValidSupdtRemarks(ddlFeedbackCategoryAdd.SelectedValue, ddlAssignedToAdd.SelectedValue, txtRemarksAdd.Text, keyAnchorAdd))
                    {
                        e.Canceled = true;
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        PhoenixInspectionEventSupdtFeedback.InsertEventSupdtEmployeewiseFeedback(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableGuid(ViewState["EVENTFEEDBACKID"].ToString())
                            , General.GetNullableInteger(ddlAssignedToAdd.SelectedValue)
                            , General.GetNullableGuid(ViewState["OTHERREFERENCEID"].ToString())
                            , General.GetNullableGuid(ddlFeedbackCategoryAdd.SelectedValue)
                            , General.GetNullableGuid(ddlFeedbackSubCategoryAdd.SelectedValue)
                            , General.GetNullableString(txtRemarksAdd.Text)
                            , General.GetNullableInteger(keyAnchorAdd));

                        ucStatus.Text = "Information Updated";
                    }
                    Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFeedback_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName)) eb.Visible = false;
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlQuick ucKeyAnchor = (UserControlQuick)e.Item.FindControl("ucKeyAnchorEdit");
            if (ucKeyAnchor != null)
            {

                if (General.GetNullableInteger(drv["FLDKEYANCHOR"].ToString()) != null)
                {
                    ucKeyAnchor.SelectedValue = drv["FLDKEYANCHOR"].ToString();
                }
            }

            RadComboBox ddlFeedbackCategoryEdit = (RadComboBox)e.Item.FindControl("ddlFeedbackCategoryEdit");
            RadComboBox ddlFeedbackSubCategoryEdit = (RadComboBox)e.Item.FindControl("ddlFeedbackSubCategoryEdit");

            if (ddlFeedbackCategoryEdit != null)
            {
                ddlFeedbackCategoryEdit.DataSource = PhoenixInspectionFeedBackCategory.ListFeedBackCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null);
                ddlFeedbackCategoryEdit.DataTextField = "FLDFEEDBACKCATEGORYNAME";
                ddlFeedbackCategoryEdit.DataValueField = "FLDFEEDBACKCATEGORYID";
                ddlFeedbackCategoryEdit.DataBind();
                ddlFeedbackCategoryEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                if (General.GetNullableGuid(drv["FLDFEEDBACKCATEGORYID"].ToString()) != null)
                {
                    ddlFeedbackCategoryEdit.SelectedValue = drv["FLDFEEDBACKCATEGORYID"].ToString();
                }
            }
            if (ddlFeedbackSubCategoryEdit != null)
            {
                ddlFeedbackSubCategoryEdit.DataSource = PhoenixInspectionFeedbackSubCategory.ListFeedbackSubCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null);
                ddlFeedbackSubCategoryEdit.DataTextField = "FLDFEEDBACKSUBCATEGORYNAME";
                ddlFeedbackSubCategoryEdit.DataValueField = "FLDFEEDBACKSUBCATEGORYID";
                ddlFeedbackSubCategoryEdit.DataBind();
                ddlFeedbackSubCategoryEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                if (General.GetNullableGuid(drv["FLDFEEDBACKSUBCATEGORYID"].ToString()) != null)
                {
                    ddlFeedbackSubCategoryEdit.SelectedValue = drv["FLDFEEDBACKSUBCATEGORYID"].ToString();
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlFeedbackCategoryAdd = (RadComboBox)e.Item.FindControl("ddlFeedbackCategoryAdd");
            RadComboBox ddlFeedbackSubCategoryAdd = (RadComboBox)e.Item.FindControl("ddlFeedbackSubCategoryAdd");
            RadComboBox ddlAssignedToAdd = (RadComboBox)e.Item.FindControl("ddlAssignedToAdd");
            if (ddlFeedbackCategoryAdd != null)
            {
                ddlFeedbackCategoryAdd.DataSource = PhoenixInspectionFeedBackCategory.ListFeedBackCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null);
                ddlFeedbackCategoryAdd.DataTextField = "FLDFEEDBACKCATEGORYNAME";
                ddlFeedbackCategoryAdd.DataValueField = "FLDFEEDBACKCATEGORYID";
                ddlFeedbackCategoryAdd.DataBind();
                ddlFeedbackCategoryAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
            if (ddlFeedbackSubCategoryAdd != null)
            {
                ddlFeedbackSubCategoryAdd.DataSource = PhoenixInspectionFeedbackSubCategory.ListFeedbackSubCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null);
                ddlFeedbackSubCategoryAdd.DataTextField = "FLDFEEDBACKSUBCATEGORYNAME";
                ddlFeedbackSubCategoryAdd.DataValueField = "FLDFEEDBACKSUBCATEGORYID";
                ddlFeedbackSubCategoryAdd.DataBind();
                ddlFeedbackSubCategoryAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
            if (ddlAssignedToAdd != null)
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                DataSet ds = PhoenixCommonInspection.SearchVesselEmployee(General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                         General.GetNullableDateTime(ucEventDate.Text), null,
                         null, 0,
                         1,
                         40,
                         ref iRowCount,
                         ref iTotalPageCount);
                ddlAssignedToAdd.DataSource = ds;
                ddlAssignedToAdd.DataTextField = "FLDNAMEANDRANK";
                ddlAssignedToAdd.DataValueField = "FLDSIGNONOFFID";
                ddlAssignedToAdd.DataBind();
                ddlAssignedToAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
        }
    }

    //protected void gvFeedback_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvFeedback.SelectedIndex = se.NewSelectedIndex;
    //}

    private void SetRowSelection()
    {
        gvFeedback.SelectedIndexes.Clear();
        for (int i = 0; i < gvFeedback.Items.Count; i++)
        {
            if (gvFeedback.MasterTableView.Items[i].GetDataKeyValue("FLDEMPLOYEEFEEDBACKID").ToString().Equals(ViewState["EMPLOYEEFEEDBACKID"].ToString()))
            {
                gvFeedback.MasterTableView.Items[i].Selected = true;
            }
        }
    }
    protected void BindInspectionEvent()
    {
        ddlEvent.DataSource = PhoenixInspectionEvent.ListInspectionEvent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , null);
        ddlEvent.DataTextField = "FLDEVENTNAME";
        ddlEvent.DataValueField = "FLDINSPECTIONEVENTID";
        ddlEvent.DataBind();
        ddlEvent.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindValue(int rowindex)
    {
        try
        {
            RadLabel lblEmployeeFeedbackId = (RadLabel)gvFeedback.Items[rowindex].FindControl("lblEmployeeFeedbackId");
            if (lblEmployeeFeedbackId != null)
            {
                if (General.GetNullableGuid(lblEmployeeFeedbackId.Text) != null)
                    ViewState["EMPLOYEEFEEDBACKID"] = lblEmployeeFeedbackId.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFeedback_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblEmployeeFeedbackIdEdit = (RadLabel)e.Item.FindControl("lblEmployeeFeedbackIdEdit");
            RadLabel lblSignonoffIdEdit = (RadLabel)e.Item.FindControl("lblSignonoffIdEdit");
            RadComboBox ddlFeedbackCategoryEdit = (RadComboBox)e.Item.FindControl("ddlFeedbackCategoryEdit");
            RadComboBox ddlFeedbackSubCategoryEdit = (RadComboBox)e.Item.FindControl("ddlFeedbackSubCategoryEdit");
            RadComboBox ddlAssignedToEdit = (RadComboBox)e.Item.FindControl("ddlAssignedToEdit");
            RadTextBox txtRemarksEdit = (RadTextBox)e.Item.FindControl("txtRemarksEdit");
            string keyAnchor = ((UserControlQuick)e.Item.FindControl("ucKeyAnchorEdit")).SelectedValue;

            if (!IsValidSupdtRemarks(ddlFeedbackCategoryEdit.SelectedValue, lblSignonoffIdEdit.Text, txtRemarksEdit.Text, keyAnchor))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }
            else
            {
                PhoenixInspectionEventSupdtFeedback.UpdateEventSupdtEmployeewiseFeedback(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(lblEmployeeFeedbackIdEdit.Text)
                    , General.GetNullableInteger(lblSignonoffIdEdit.Text)
                    , General.GetNullableGuid(ddlFeedbackCategoryEdit.SelectedValue)
                    , General.GetNullableGuid(ddlFeedbackSubCategoryEdit.SelectedValue)
                    , General.GetNullableString(txtRemarksEdit.Text)
                    , General.GetNullableInteger(keyAnchor));
                ucStatus.Text = "Information Updated";
            }

            Rebind();
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFeedback_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblEmployeeFeedbackId = ((RadLabel)e.Item.FindControl("lblEmployeeFeedbackId"));
            PhoenixInspectionEventSupdtFeedback.DeleteEventSupdtEmployeewiseFeedback(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(lblEmployeeFeedbackId.Text));

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvFeedback.SelectedIndexes.Clear();
        gvFeedback.EditIndexes.Clear();
        gvFeedback.DataSource = null;
        gvFeedback.Rebind();
    }
    protected void gvFeedback_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFeedback.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
