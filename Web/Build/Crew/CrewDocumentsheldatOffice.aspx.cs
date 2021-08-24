using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewReports;
using Telerik.Web.UI;
public partial class CrewDocumentsheldatOffice : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {

        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Save", "SAVE");
        MenuCrewDocumentsheldatOffice.AccessRights = this.ViewState;
        MenuCrewDocumentsheldatOffice.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            
        }
    }

    protected void CrewDocumentsheldatOffice_TabStripCommand(object sender, EventArgs e)
    {
        //String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        //String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDocument(lblEmpId.Text, ddlZone.SelectedZone.ToString(), txtRemarks.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewDocumentsheldatOffice.DocumentsheldatOfficeInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , int.Parse(lblEmpId.Text), 1, 1, General.GetNullableInteger(ddlZone.SelectedZone)
                                                                                , General.GetNullableString(txtRemarks.Text));

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null);", true);
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    protected void ImgBtnValidFileno_Click(object sender, ImageClickEventArgs e)
    {
        if (IsValidFileNoCheck())
        {
            SetEmployeePrimaryDetails();
        }
        else
        {
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidFileNoCheck()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(txtFileNo.Text.Trim()))
            ucError.ErrorMessage = "File Number is required";

        return (!ucError.IsError);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeListByFileNo(txtFileNo.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                lblEmpId.Text = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                lblZoneId.Text = dt.Rows[0]["FLDZONE"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidDocument(string EmpId,string zone,string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(EmpId).HasValue)
        {
            ucError.ErrorMessage = "Employee Name is required.";
        }
        if (General.GetNullableInteger(zone) == null)
        {
            ucError.ErrorMessage = "Field Office is required.";
        }
        if (string.IsNullOrEmpty(remarks))
        {
            ucError.ErrorMessage = "Remarks is required.";
        }
        return (!ucError.IsError);
    }
}
