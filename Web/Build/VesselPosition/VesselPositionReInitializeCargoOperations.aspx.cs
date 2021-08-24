using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionReInitializeCargoOperations : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCargoOperations.AccessRights = this.ViewState;
        MenuCargoOperations.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["operationid"] != null)
            {
                ViewState["OPERATIONID"] = Request.QueryString["operationid"].ToString();
                OperationEdit(new Guid(Request.QueryString["operationid"].ToString()));
            }
            else
            {
                toolbar.AddButton("Save", "SAVE");
            }

            if (Request.QueryString["NoonReportID"] != null)
                ViewState["NoonReportID"] = Request.QueryString["NoonReportID"].ToString();

            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

            if (Request.QueryString["voyageid"] != null)
                ViewState["VOYAGEID"] = Request.QueryString["voyageid"].ToString();

            if (Request.QueryString["ReportDate"] != null)
            {
                txtCommenced.Text = Request.QueryString["ReportDate"].ToString();
                txtCompleted.Text = Request.QueryString["ReportDate"].ToString();
            }

            ucCargo.VesselId = int.Parse(ViewState["VESSELID"].ToString());
            ucCargo.CargoTypeShowYesNo = "1";

            if (chkOilMajor.Checked == false)
            {
                ddlOilMajor.SelectedValue = "";
            }
        }
    }

    private bool IsValidOperation()
    {

        if ((ddlCargoOperation.SelectedValue == ""))
            ucError.ErrorMessage = "Cargo Operation is required";

        if (General.GetNullableGuid(ucCargo.SelectedCargoType) == null)
            ucError.ErrorMessage = "Cargo is required.";

        if (General.GetNullableDecimal(ucQuantity.Text) == null)
            ucError.ErrorMessage = "Quantity is required.";

        return (!ucError.IsError);
    }

    protected void MenuCargoOperations_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        if (((RadToolBarButton)dce.Item).CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                if (!IsValidOperation())
                {
                    ucError.Visible = true;
                    return;
                }
                string commencedtime="";// = txtCommencedTime.Text.Trim().Equals("__:__") ? string.Empty : txtCommencedTime.Text;
                string completedtime="";// = txtCompletedTime.Text.Trim().Equals("__:__") ? string.Empty : txtCompletedTime.Text;

                PhoenixVesselPositionDepartureReport.UpdateDepartureOperation(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(ViewState["OPERATIONID"] != null ? ViewState["OPERATIONID"].ToString() : ""),
                    General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                    General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(ViewState["NoonReportID"].ToString()),
                    General.GetNullableGuid(ucCargo.SelectedCargoType),
                    General.GetNullableDecimal(""),
                    ddlCargoOperation.SelectedValue,
                    General.GetNullableDateTime(txtCommenced.Text + " " + commencedtime),
                    General.GetNullableDateTime(txtCompleted.Text + " " + completedtime),
                    chkOilMajor.Checked == true ? 1 : 0, General.GetNullableString(ddlOilMajor.SelectedValue.ToString()),
                    General.GetNullableDecimal(ucQuantity.Text),
                    General.GetNullableInteger(ucPortEdit.SelectedValue),
                    General.GetNullableDateTime(txtLoadedDate.Text));
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

            String script = "javascript:fnReloadList('codehelp1',null,'true');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    private void OperationEdit(Guid operationid)
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.EditDepartureOperation(operationid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ucCargo.SelectedCargoType = dr["FLDCARGOID"].ToString();
            ddlCargoOperation.SelectedValue = dr["FLDOPERATION"].ToString();
            ucQuantity.Text = dr["FLDQUANTITY"].ToString();
            if (dr["FLDOILMAJORCARGOYN"].ToString() == "1")
            {
                chkOilMajor.Checked = true;
            }
            else
            {
                chkOilMajor.Checked = false;
            }
            ddlOilMajor.SelectedValue = dr["FLDOILMAJOR"].ToString();
            ucPortEdit.SelectedValue = dr["FLDPORT"].ToString();
            ucPortEdit.Text = dr["FLDSEAPORTNAME"].ToString();
            txtLoadedDate.Text = dr["FLDDATELOADED"].ToString();
        }
    }
}
