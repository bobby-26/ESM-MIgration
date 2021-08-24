using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewNewApplicantAppraisalSeamanComment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string canedit = "1";
        string canview = "1";
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["AppraisalStatus"] = 1;
            EditAppraisal();
            SetEmployeePrimaryDetails();

            canedit = "1";
            canview = "1";
            DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(ViewState["Rankid"].ToString()), General.GetNullableGuid(Filter.CurrentAppraisalSelection));

            if (ds.Tables[0].Rows.Count > 0)
            {
                canedit = ds.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
                canview = ds.Tables[0].Rows[0]["FLDCANVIEW"].ToString();
            }
            if (txtSeaman.Enabled)
                txtSeaman.Focus();
        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        if (canview.Equals("1"))
        {
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        }
        MenuSeamanComment.AccessRights = this.ViewState;
        MenuSeamanComment.MenuList = toolbarmain.Show();

        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Appraisal", "APPRAISAL");
        toolbarmain.AddButton("Form", "FORM");
        toolbarmain.AddButton("Seafarer  Comment", "SEAMANCOMMENT");
        if (Filter.CurrentAppraisalSelection != null && ViewState["AppraisalStatus"].ToString() == "0")
        {
            toolbarmain.AddButton("Appraisal Report", "APPRAISALREPORT");
            toolbarmain.AddButton("Promotion Report", "PROMOTION");
        }
        AppraisalTabs.MenuList = toolbarmain.Show();
        AppraisalTabs.AccessRights = this.ViewState;
        AppraisalTabs.SelectedMenuIndex = 2;
    }

    protected void SeamanComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidAppraisal())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewAppraisal.UpdateAppraisalSeamanComment(
                    new Guid(Filter.CurrentAppraisalSelection)
                    , txtSeaman.Text);
                ucStatus.Text = "Seafarer Comment is updated.";
                EditAppraisal();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AppraisalTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("APPRAISAL"))
            {
                Response.Redirect("CrewNewApplicantAppraisal.aspx", false);
            }

            if (CommandName.ToUpper().Equals("FORM"))
            {
                Response.Redirect("CrewNewApplicantAppraisalActivity.aspx", false);
            }
            if (CommandName.ToUpper().Equals("APPRAISALREPORT"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=NEWAPPRAISAL&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0", false);
            }
            if (CommandName.ToUpper().Equals("PROMOTION"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=NEWPROMOTION&appraisalid=" + Filter.CurrentAppraisalSelection + "&showmenu=0", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidAppraisal()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (txtSeaman.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Seafarer comment is required.";

        return (!ucError.IsError);
    }

    private void EditAppraisal()
    {
        DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtSeaman.Text = ds.Tables[0].Rows[0]["FLDSEAMANCOMMENT"].ToString();
            ViewState["AppraisalStatus"] = int.Parse(ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString());
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            if (Filter.CurrentAppraisalSelection != null)
            {
                DataSet ds = PhoenixCrewAppraisal.EditAppraisal(new Guid(Filter.CurrentAppraisalSelection));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtvessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                    txtoccassion.Text = ds.Tables[0].Rows[0]["FLDOCCASSION"].ToString();
                    txtdate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDAPPRAISALDATE"].ToString());
                    ViewState["Rankid"] = ds.Tables[0].Rows[0]["FLDRANKID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
