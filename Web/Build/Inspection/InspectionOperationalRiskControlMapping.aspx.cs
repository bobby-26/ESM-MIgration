using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web.UI;

public partial class InspectionOperationalRiskControlMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbartab = new PhoenixToolbar();
        toolbartab.AddButton("Save", "SAVE", ToolBarDirection.Right);
        if ((Request.QueryString["callfrom"] != "") && (Request.QueryString["callfrom"] != null))
        {
            toolbartab.AddButton("Back", "BACK", ToolBarDirection.Right);
        }
        MenuMapping.AccessRights = this.ViewState;
        MenuMapping.MenuList = toolbartab.Show();

        if (!IsPostBack)
        {
            ViewState["OPERATIONALHAZARDID"] = "";
            ViewState["callfrom"] = "";
            ViewState["raid"] = "";

            ViewState["OPERATIONALHAZARDID"] = string.IsNullOrEmpty(Request.QueryString["Operationalhazardid"]) ? "" : Request.QueryString["Operationalhazardid"];
            ViewState["callfrom"] = string.IsNullOrEmpty(Request.QueryString["callfrom"]) ? "" : Request.QueryString["callfrom"];
            ViewState["raid"] = string.IsNullOrEmpty(Request.QueryString["raid"]) ? "" : Request.QueryString["raid"];

            ViewState["companyid"] = "0";

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["companyid"] = nvc.Get("QMS");
            }

            BindCategory();

            Bindoperationalprocedure();
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

            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (ViewState["callfrom"].ToString() == "jha")
                {
                    Response.Redirect("../Inspection/InspectionOperationalHazardPickList.aspx?JhaId=" + ViewState["raid"].ToString(), false);
                }

                if (ViewState["callfrom"].ToString() == "cargo")
                {
                    Response.Redirect("../Inspection/InspectionRACargoOperationalHazardPickList.aspx?RAID=" + ViewState["raid"].ToString(), false);
                }

                if (ViewState["callfrom"].ToString() == "generic")
                {
                    Response.Redirect("../Inspection/InspectionRAGenericOperationalHazardPickList.aspx?RAID=" + ViewState["raid"].ToString(), false);
                }

                if (ViewState["callfrom"].ToString() == "machinery")
                {
                    Response.Redirect("../Inspection/InspectionRAMachineryOperationalHazardPickList.aspx?RAID=" + ViewState["raid"].ToString(), false);
                }

                if (ViewState["callfrom"].ToString() == "navigation")
                {
                    Response.Redirect("../Inspection/InspectionRANavigationOperationalHazardPickList.aspx?RAID=" + ViewState["raid"].ToString(), false);
                }
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidOperationalHazard())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["OPERATIONALHAZARDID"].ToString() == "")
                {
                    PhoenixInspectionOperationalRiskControls.InsertOperationalRiskControls(General.GetNullableGuid(null),
                       General.GetNullableInteger(ddlCategory.SelectedValue), General.GetNullableString(txtopertaionalhazard.Text.Trim()), General.GetNullableString(txtcontrolprecautions.Text.Trim()), General.GetNullableString(txtAspect.Text.Trim()), General.GetNullableGuid(null));
                }
                else
                {
                    PhoenixInspectionOperationalRiskControls.InsertOperationalRiskControls(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString()),
                       General.GetNullableInteger(ddlCategory.SelectedValue), General.GetNullableString(txtopertaionalhazard.Text.Trim()), General.GetNullableString(txtcontrolprecautions.Text.Trim()), General.GetNullableString(txtAspect.Text.Trim()), General.GetNullableGuid(null));
                }

                Bindoperationalprocedure();

                ucStatus.Text = "Information Updated.";

                if (ViewState["callfrom"].ToString() == "")
                {
                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
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

        if (General.GetNullableInteger(ddlCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Process is required";

        if (General.GetNullableString(txtAspect.Text.Trim()) == null)
            ucError.ErrorMessage = "Aspect is required.";

        if (General.GetNullableString(txtopertaionalhazard.Text.Trim()) == null)
            ucError.ErrorMessage = "Hazards / Risks is required.";

        if (General.GetNullableString(txtcontrolprecautions.Text.Trim()) == null)
            ucError.ErrorMessage = "Controls / Precautions is required.";

        return (!ucError.IsError);
    }

    private void Bindoperationalprocedure()
    {
        DataSet ds = PhoenixInspectionOperationalRiskControls.EditOperationalRiskControls(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString()));
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            ddlCategory.SelectedValue = dt.Rows[0]["FLDELEMENTID"].ToString();
            txtAspect.Text = dt.Rows[0]["FLDASPECT"].ToString();
            txtcontrolprecautions.Text = dt.Rows[0]["FLDCONTROLPRECAUTIONS"].ToString();
            txtopertaionalhazard.Text = dt.Rows[0]["FLDOPERATIONALHAZARD"].ToString();
        }
    }
}
