using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;

public partial class InspectionReportCAR : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Show Report", "SHOWREPORT");
                MenuReportsFilter.AccessRights = this.ViewState;
                MenuReportsFilter.MenuList = toolbar.Show();
                ViewState["PAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";                
                BindNonconformity();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=9&reportcode=CORRECTIVEACTION&recordandresponseid=" + ddlNonconformity.SelectedValue + "&showmenu=0&showexcel=no";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlNonconformity.SelectedIndex == 0)
        {
            ucError.ErrorMessage = "Please select Nonconformity.";
        }
        return (!ucError.IsError);
    }
    public void BindNonconformity()
    {
        DataTable dt = PhoenixInspectionAuditSchedule.ListAuditNonConformity(General.GetNullableGuid(ucAuditSchedule.SelectedAuditSchedule));
        ddlNonconformity.Items.Add("select");
        ddlNonconformity.DataSource = dt;
        ddlNonconformity.DataTextField = "FLDNONCONFORMITYNUMBER";
        ddlNonconformity.DataValueField = "FLDREVIEWRECORDANDRESPONSEID";
        ddlNonconformity.DataBind();
        ddlNonconformity.Items.Insert(0, new ListItem("--Select--", "Dummy"));        
    }
    protected void ucAuditSchedule_TextChanged(object sender, EventArgs e)
    {
        BindNonconformity();
        ifMoreInfo.Attributes["src"] = "";
    }
}
