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

public partial class InspectionPNIWelfare : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PNIID"] = null;
            ViewState["PNICASEID"] = null;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            PNIListMain.AccessRights = this.ViewState;
            PNIListMain.MenuList = toolbar.Show();

            if (Request.QueryString["PNIId"] != null)
                ViewState["PNIID"] = Request.QueryString["PNIId"].ToString();
            if (Request.QueryString["PNICASEID"] != null && !string.IsNullOrEmpty(Request.QueryString["PNICASEID"].ToString()))
                ViewState["PNICASEID"] = Request.QueryString["PNICASEID"].ToString();
            lnkPNIChecklist.Visible = false;
            if (ViewState["PNICASEID"] != null && !string.IsNullOrEmpty(ViewState["PNICASEID"].ToString()))
            {
                lnkPNIChecklist.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Inspection/InspectionPNIChecklist.aspx?&PNICASEID=" + ViewState["PNICASEID"].ToString() + "&DEPARTMENTID=2');return true;");
                lnkPNIChecklist.Visible = true;
            }

            EditWelfare();
        }
    }
    protected void PNIListMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidInspectionPNIWelfare())
                {
                    if (ViewState["PNIID"] != null)
                    {
                        PhoenixInspectionPNI.UpdateWelfare(new Guid(ViewState["PNIID"].ToString()), txtRemarks.Text,
                            General.GetNullableDateTime(txtFitDate.Text), General.GetNullableInteger(ucWelfarestatus.SelectedHard), General.GetNullableDateTime(txtClosureDate.Text));

                        ucStatus.Text = "Welfare details Saved.";
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditWelfare()
    {
        if (ViewState["PNIID"] != null)
        {
            DataSet ds = PhoenixInspectionPNI.EditInspectionPNI(new Guid(ViewState["PNIID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtFitDate.Text = dr["FLDMEDICALFITDATE"].ToString();
                txtCBA.Text = dr["FLDCOVERINGCBA"].ToString();
                txtClosureDate.Text = dr["FLDCLOSUREDATE"].ToString();
                txtRemarks.Text = dr["FLDREMARKBYDOCTOR"].ToString();
                txttimelimit.Text = dr["FLDTIMELIMITFORCASE"].ToString();
                ucWelfarestatus.SelectedHard = dr["FLDSTATUS"].ToString();
            }
        }
    }
    private bool IsValidInspectionPNIWelfare()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(txtFitDate.Text) == null)
            ucError.ErrorMessage = "Medical Fit Date is required.";

        if (string.IsNullOrEmpty(ucWelfarestatus.SelectedHard) || ucWelfarestatus.SelectedHard.ToUpper().ToString() == "DUMMY")
            ucError.ErrorMessage = "Status is required.";
        

        return (!ucError.IsError);
    }

}
