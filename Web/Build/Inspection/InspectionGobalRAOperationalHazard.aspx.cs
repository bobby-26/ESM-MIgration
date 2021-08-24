using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using SouthNests.Phoenix.Integration;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class InspectionGobalRAOperationalHazard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbartab = new PhoenixToolbar();
        toolbartab.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuMapping.AccessRights = this.ViewState;
        MenuMapping.MenuList = toolbartab.Show();

        if (!IsPostBack)
        {
            ViewState["RATYPE"] = "";

            ViewState["companyid"] = "0";

            ViewState["RATYPE"] = string.IsNullOrEmpty(Request.QueryString["RATYPE"]) ? "" : Request.QueryString["RATYPE"];
            ViewState["RAId"] = string.IsNullOrEmpty(Request.QueryString["RAId"]) ? "" : Request.QueryString["RAId"];

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["companyid"] = nvc.Get("QMS");
            }
            BindCategory();            
        }
    }
    protected void BindCategory()
    {
        DataTable ds = new DataTable();

        ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentElement();

        if (ds.Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds;
            ddlCategory.DataBind();
        }
    }

    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if(CommandName.ToUpper().Equals("SAVE"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                if (!IsValidOperationalHazard())
                {
                    ucError.Visible = true;
                    return;
                }
                if(ViewState["RATYPE"].ToString() == "3")
                {
                    PhoenixInspectionOperationalRiskControls.InsertGenericOperationalRiskControls(General.GetNullableInteger(ddlCategory.SelectedValue)
                                                    , General.GetNullableString(txtopertaionalhazard.Text.Trim())
                                                    , General.GetNullableString(txtcontrolprecautions.Text.Trim())
                                                    , General.GetNullableString(txtAspect.Text.Trim())
                                                    , General.GetNullableGuid(ViewState["RAId"].ToString()));
                }
                if (ViewState["RATYPE"].ToString() == "4")
                {
                    PhoenixInspectionOperationalRiskControls.InsertNavigationOperationalRiskControls(General.GetNullableInteger(ddlCategory.SelectedValue)
                                                    , General.GetNullableString(txtopertaionalhazard.Text.Trim())
                                                    , General.GetNullableString(txtcontrolprecautions.Text.Trim())
                                                    , General.GetNullableString(txtAspect.Text.Trim())
                                                    , General.GetNullableGuid(ViewState["RAId"].ToString()));
                }

                if (ViewState["RATYPE"].ToString() == "5")
                {
                    PhoenixInspectionOperationalRiskControls.InsertMachineryOperationalRiskControls(General.GetNullableInteger(ddlCategory.SelectedValue)
                                                    , General.GetNullableString(txtopertaionalhazard.Text.Trim())
                                                    , General.GetNullableString(txtcontrolprecautions.Text.Trim())
                                                    , General.GetNullableString(txtAspect.Text.Trim())
                                                    , General.GetNullableGuid(ViewState["RAId"].ToString()));
                }

                if (ViewState["RATYPE"].ToString() == "6")
                {
                    PhoenixInspectionOperationalRiskControls.InsertCargoOperationalRiskControls(General.GetNullableInteger(ddlCategory.SelectedValue)
                                                    , General.GetNullableString(txtopertaionalhazard.Text.Trim())
                                                    , General.GetNullableString(txtcontrolprecautions.Text.Trim())
                                                    , General.GetNullableString(txtAspect.Text.Trim())
                                                    , General.GetNullableGuid(ViewState["RAId"].ToString()));
                }

                if (ViewState["RATYPE"].ToString() == "1")
                {
                    PhoenixInspectionOperationalRiskControls.InsertJHAOperationalRiskControls(General.GetNullableInteger(ddlCategory.SelectedValue)
                                                    , General.GetNullableString(txtopertaionalhazard.Text.Trim())
                                                    , General.GetNullableString(txtcontrolprecautions.Text.Trim())
                                                    , General.GetNullableString(txtAspect.Text.Trim())
                                                    , General.GetNullableGuid(ViewState["RAId"].ToString()));
                }

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidOperationalHazard()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RAId"].ToString()) == null)
            ucError.ErrorMessage = "Save the template first";

        if (General.GetNullableInteger(ViewState["RATYPE"].ToString()) == null)
            ucError.ErrorMessage = "Type is required";

        if (General.GetNullableInteger(ddlCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Element is required";

        if (General.GetNullableString(txtAspect.Text.Trim()) == null)
            ucError.ErrorMessage = "Aspect is required.";

        if (General.GetNullableString(txtopertaionalhazard.Text.Trim()) == null)
            ucError.ErrorMessage = "Hazards / Risks is required.";

        if (General.GetNullableString(txtcontrolprecautions.Text.Trim()) == null)
            ucError.ErrorMessage = "Controls / Precautions is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

}
