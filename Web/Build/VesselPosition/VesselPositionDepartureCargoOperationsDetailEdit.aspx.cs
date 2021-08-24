using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionDepartureCargoOperations : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            ViewState["DepDate"] = "";
            ViewState["ArrDate"] = "";

            if (Request.QueryString["operationid"] != null)
            {
                ViewState["OPERATIONID"] = Request.QueryString["operationid"].ToString();

            }

            if (Request.QueryString["vesselid"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
            }

            if (Request.QueryString["voyageid"] != null)
            {
                ViewState["VOYAGEID"] = Request.QueryString["voyageid"].ToString();
            }

        }
    }



    protected void gvCargoOperation_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblDepCargoOprId = ((RadLabel)e.Item.FindControl("lblDepCargoOprId"));
                if (lblDepCargoOprId.Text != "")
                {
                    PhoenixVesselPositionDepartureReport.DeleteDepartureCargoOperation(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(lblDepCargoOprId.Text));
                }

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DISCHARGE"))
            {
                if (Filter.CurrentVPRSDepartureReportSelection == null)
                {
                    ucError.ErrorMessage = "You cannot save. Please save the Cargo first.";
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["OPERATIONID"] == null)
                {
                    ucError.ErrorMessage = "You cannot save. Please save the Cargo first.";
                    ucError.Visible = true;
                    return;
                }

                RadLabel lblValueType = ((RadLabel)e.Item.FindControl("lblValueType"));
                string value = "";

                if (lblValueType != null)
                {
                    if (lblValueType.Text.Equals("1"))
                    {
                        UserControlDate txtValueDateEdit = ((UserControlDate)e.Item.FindControl("txtValueDateEdit"));
                        RadTimePicker txtValueDateEditTime = ((RadTimePicker)e.Item.FindControl("txtValueDateEditTime"));

                        string time = txtValueDateEditTime.SelectedTime != null ? txtValueDateEditTime.SelectedTime.Value.ToString() : "";

                        value = txtValueDateEdit.Text + " " + time;

                        value = String.Format("{0:MM/dd/yyyy HH:mm}", Convert.ToDateTime(value.Trim()));
                    }
                    else if (lblValueType.Text.Equals("2"))
                    {
                        UserControlMaskNumber txtValueDecimalEdit = ((UserControlMaskNumber)e.Item.FindControl("txtValueDecimalEdit"));

                        value = txtValueDecimalEdit.Text;
                    }
                    else if (lblValueType.Text.Equals("3"))
                    {
                        RadTextBox txtValueFreeTextEdit = ((RadTextBox)e.Item.FindControl("txtValueFreeTextEdit"));

                        value = txtValueFreeTextEdit.Text;
                    }

                    PhoenixVesselPositionDepartureReport.UpdateDepartureCargoOperation(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid((((RadLabel)e.Item.FindControl("lblDepCargoOprIdEdit")).Text)),
                        General.GetNullableGuid(ViewState["OPERATIONID"].ToString()),
                        General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                        General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCargoOprId")).Text),
                        value,
                        ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text,
                        General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblValueType")).Text));
                }

                Rebind();

                String script = "javascript:fnReloadList('codehelp1',null,'true');";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCargoOperation_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {


            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridEditableItem)
            {
                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRemarksTT");
                if (lbtn != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lbtn.ClientID;

                }

                lbtn = (RadLabel)e.Item.FindControl("lblValue");
                uct = (UserControlToolTip)e.Item.FindControl("ucValueTT");
                if (lbtn != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lbtn.ClientID;
                }

                UserControlDate txtValueDateEdit = (UserControlDate)e.Item.FindControl("txtValueDateEdit");
                RadTimePicker txtValueDateEditTime = (RadTimePicker)e.Item.FindControl("txtValueDateEditTime");
                UserControlMaskNumber txtValueDecimalEdit = (UserControlMaskNumber)e.Item.FindControl("txtValueDecimalEdit");
                RadTextBox txtValueFreeTextEdit = (RadTextBox)e.Item.FindControl("txtValueFreeTextEdit");

                RadLabel lblValue = (RadLabel)e.Item.FindControl("lblValue");
                UserControlToolTip ucValueTT = (UserControlToolTip)e.Item.FindControl("ucValueTT");

                if (drv["FLDVALUETYPE"].ToString().ToUpper().Equals("1")) // Date & Time
                {
                    if (lblValue != null)
                        lblValue.Text = General.GetDateTimeToString(drv["FLDVALUEDATE"].ToString()) + " " + string.Format("{0:HH:mm}", General.GetNullableDateTime(drv["FLDVALUE"].ToString()));

                    if (ucValueTT != null)
                        ucValueTT.Text = General.GetDateTimeToString(drv["FLDVALUEDATE"].ToString()) + " " + string.Format("{0:HH:mm}", General.GetNullableDateTime(drv["FLDVALUE"].ToString()));
                }
                else
                {
                    if (lblValue != null)
                    {
                        if (drv["FLDVALUETYPE"].ToString().ToUpper().Equals("1"))
                            lblValue.Text = General.GetDateTimeToString(drv["FLDVALUE"].ToString());
                        else lblValue.Text = drv["FLDVALUE"].ToString();
                    }

                    if (ucValueTT != null)
                    {
                        if (drv["FLDVALUETYPE"].ToString().ToUpper().Equals("1"))
                            ucValueTT.Text = General.GetDateTimeToString(drv["FLDVALUE"].ToString());
                        else ucValueTT.Text = drv["FLDVALUE"].ToString();
                    }
                }

                if (txtValueFreeTextEdit != null)
                {
                    if (drv["FLDVALUETYPE"].ToString().ToUpper().Equals("1")) // Date & Time
                    {
                        txtValueDateEdit.Visible = true;
                        txtValueDateEditTime.Visible = true;
                        txtValueDecimalEdit.Visible = false;
                        txtValueFreeTextEdit.Visible = false;

                        txtValueDateEdit.Text = drv["FLDVALUE"].ToString();
                        if (General.GetNullableDateTime(drv["FLDVALUE"].ToString()) != null && string.Format("{0:HH:mm}", General.GetNullableDateTime(drv["FLDVALUE"].ToString())) != "00:00")
                            txtValueDateEditTime.SelectedDate = Convert.ToDateTime(drv["FLDVALUE"].ToString());
                    }
                    else if (drv["FLDVALUETYPE"].ToString().ToUpper().Equals("2")) // Decimal
                    {
                        txtValueDateEdit.Visible = false;
                        txtValueDateEditTime.Visible = false;
                        txtValueDecimalEdit.Visible = true;
                        txtValueFreeTextEdit.Visible = false;

                        txtValueDecimalEdit.Text = drv["FLDVALUE"].ToString();
                    }
                    else if (drv["FLDVALUETYPE"].ToString().ToUpper().Equals("3")) // Free Text
                    {
                        txtValueDateEdit.Visible = false;
                        txtValueDateEditTime.Visible = false;
                        txtValueDecimalEdit.Visible = false;
                        txtValueFreeTextEdit.Visible = true;

                        txtValueFreeTextEdit.Text = drv["FLDVALUE"].ToString();
                    }
                }

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void gvCargoOperation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        DataSet ds = PhoenixVesselPositionDepartureReport.ListDepartureCargoOperation(
            General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , General.GetNullableGuid(ViewState["OPERATIONID"] != null ? ViewState["OPERATIONID"].ToString() : "")
            , ddlCargoOperation.SelectedValue);
        gvCargoOperation.DataSource = ds;

    }
    protected void Rebind()
    {
        gvCargoOperation.SelectedIndexes.Clear();
        gvCargoOperation.EditIndexes.Clear();
        gvCargoOperation.DataSource = null;
        gvCargoOperation.Rebind();
    }
}
