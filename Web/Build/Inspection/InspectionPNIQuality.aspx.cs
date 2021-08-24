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
public partial class InspectionPNIQuality : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PNIID"] = null;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            PNIListMain.AccessRights = this.ViewState;
            PNIListMain.MenuList = toolbar.Show();

            if (Request.QueryString["PNIId"] != null)
                ViewState["PNIID"] = Request.QueryString["PNIId"].ToString();

            EditQuality();
        }
    }
    protected void PNIListMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (IsValidInspectionPNIQuality())
            {
                if (ViewState["PNIID"] != null)
                {
                    PhoenixInspectionPNI.PNIQualityUpdate(new Guid(ViewState["PNIID"].ToString()), chkPNIReportyn.Checked ? 1 : 0,
                        General.GetNullableDateTime(txtReportedDate.Text), int.Parse(ucESMOwner.SelectedHard), txtRefNo.Text.Trim());

                    ucStatus.Text = "Quality details Saved.";
                }

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }

    }
    private void EditQuality()
    {
        if (ViewState["PNIID"] != null)
        {
            DataSet ds = PhoenixInspectionPNI.EditInspectionPNI(new Guid(ViewState["PNIID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtPNIClub.Text = dr["FLDPNICLUBNAME"].ToString();
                txtDeductible.Text = dr["FLDCREWDEDUCTIBLE"].ToString();
                txtQualityincharge.Text = dr["FLDQUALITYINCHARGENAME"].ToString();
                chkPNIReportyn.Checked = dr["FLDREPORTEDTOPAIYN"].ToString() == "1" ? true : false;
                txtReportedDate.Text = dr["FLDREPORTEDDATE"].ToString();
                ucESMOwner.SelectedHard = dr["FLDESMOROWNER"].ToString();
                txtRefNo.Text = dr["FLDPNIREFNO"].ToString();
            }
        }
    }
    private bool IsValidInspectionPNIQuality()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(txtReportedDate.Text) == null)
            ucError.ErrorMessage = "Reported Date is required.";

        if (string.IsNullOrEmpty(ucESMOwner.SelectedHard) || ucESMOwner.SelectedHard.ToUpper().ToString() == "DUMMY")
            ucError.ErrorMessage = "Company/Owner is required.";

        if (string.IsNullOrEmpty(txtRefNo.Text.Trim()))
            ucError.ErrorMessage = "P & I Ref.No is Required.";

        return (!ucError.IsError);
    }

}
