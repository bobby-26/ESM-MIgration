using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAImpactGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuMapping.AccessRights = this.ViewState;
            MenuMapping.MenuList = toolbartab.Show();

            if (!IsPostBack)
            {
                ViewState["IMPACTID"] = "";
                if ((Request.QueryString["impactid"] != null) && (Request.QueryString["impactid"] != ""))
                {
                    ViewState["IMPACTID"] = Request.QueryString["impactid"].ToString();
                }
                BindCategory();
                DetailEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRASubHazard())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["IMPACTID"].ToString().Equals(""))
                {
                    Guid? impactid = General.GetNullableGuid(null);

                    PhoenixInspectionOperationalRiskControls.InsertRiskAssessmentImpact(ref impactid
                    , General.GetNullableInteger(ddlCategory.SelectedValue)
                    , General.GetNullableString(txtImpact.Text.Trim())
                   , General.GetNullableInteger(ucSeverity.SelectedSeverity)
                   , General.GetNullableString(txtconsequence.Text.Trim()));

                    ViewState["IMPACTID"] = impactid;
                }
                else
                {
                    PhoenixInspectionOperationalRiskControls.UpdateRiskAssessmentImpact(new Guid(ViewState["IMPACTID"].ToString())
                   , General.GetNullableInteger(ddlCategory.SelectedValue)
                    , General.GetNullableString(txtImpact.Text.Trim())
                   , General.GetNullableInteger(ucSeverity.SelectedSeverity)
                   , General.GetNullableString(txtconsequence.Text.Trim()));
                }

                DetailEdit();
                ucStatus.Text = "Information updated.";

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRASubHazard()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableString(txtImpact.Text.Trim()) == null)
            ucError.ErrorMessage = "Impact is required.";

        if (General.GetNullableInteger(ucSeverity.SelectedSeverity) == null)
            ucError.ErrorMessage = "Severity is required.";

        return (!ucError.IsError);
    }

    protected void BindCategory()
    {
        DataTable dt = new DataTable();
        dt = PhoenixInspectionOperationalRiskControls.ListRiskAssessmentHazardType();

        if (dt.Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = dt;
            ddlCategory.DataBind();
        }
    }

    private void DetailEdit()
    {
        DataSet ds = PhoenixInspectionOperationalRiskControls.EditRiskAssessmentImpact(General.GetNullableGuid(ViewState["IMPACTID"].ToString()));
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            txtImpact.Text = dt.Rows[0]["FLDIMPACTNAME"].ToString();
            txtscore.Text = dt.Rows[0]["FLDSCORE"].ToString();
            ViewState["IMPACTID"] = dt.Rows[0]["FLDRISKASSESSMENTIMPACTID"].ToString();
            txtconsequence.Text = dt.Rows[0]["FLDCONSCATEGORY"].ToString();
            ddlCategory.SelectedValue = dt.Rows[0]["FLDHAZARDTYPE"].ToString();
            ucSeverity.SelectedSeverity = dt.Rows[0]["FLDSEVERITYID"].ToString();
        }
    }
}