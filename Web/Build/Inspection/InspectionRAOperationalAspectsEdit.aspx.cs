using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web.UI;

public partial class InspectionRAOperationalAspectsEdit : PhoenixBasePage
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
            ViewState["OPERATIONALHAZARDID"] = "";
            ViewState["RATYPE"] = "";
            ViewState["raid"] = "";

            ViewState["OPERATIONALHAZARDID"] = string.IsNullOrEmpty(Request.QueryString["Operationalhazardid"]) ? "" : Request.QueryString["Operationalhazardid"];
            ViewState["RATYPE"] = string.IsNullOrEmpty(Request.QueryString["RATYPE"]) ? "" : Request.QueryString["RATYPE"];

            ViewState["companyid"] = "0";

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["companyid"] = nvc.Get("QMS");
            }

            Bindoperationalprocedure();
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
                if (!IsValidOperationalHazard())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["OPERATIONALHAZARDID"].ToString() != "")
                {
                    if (ViewState["RATYPE"].ToString() == "1")
                    {
                        PhoenixInspectionOperationalRiskControls.UpdateJHAOperationalRiskControls(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString()),
                       General.GetNullableString(txtopertaionalhazard.Text.Trim()), General.GetNullableString(txtcontrolprecautions.Text.Trim()), General.GetNullableString(txtAspect.Text.Trim()));
                    }

                    if (ViewState["RATYPE"].ToString() == "3")
                    {
                        PhoenixInspectionOperationalRiskControls.UpdateRAGenericOperationalRiskControls(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString()),
                       General.GetNullableString(txtopertaionalhazard.Text.Trim()), General.GetNullableString(txtcontrolprecautions.Text.Trim()), General.GetNullableString(txtAspect.Text.Trim()));
                    }

                    if (ViewState["RATYPE"].ToString() == "4")
                    {
                        PhoenixInspectionOperationalRiskControls.UpdateNavigationOperationalRiskControls(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString()),
                       General.GetNullableString(txtopertaionalhazard.Text.Trim()), General.GetNullableString(txtcontrolprecautions.Text.Trim()), General.GetNullableString(txtAspect.Text.Trim()));
                    }

                    if (ViewState["RATYPE"].ToString() == "5")
                    {
                        PhoenixInspectionOperationalRiskControls.UpdateMachineryOperationalRiskControls(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString()),
                       General.GetNullableString(txtopertaionalhazard.Text.Trim()), General.GetNullableString(txtcontrolprecautions.Text.Trim()), General.GetNullableString(txtAspect.Text.Trim()));
                    }

                    if (ViewState["RATYPE"].ToString() == "6")
                    {
                        PhoenixInspectionOperationalRiskControls.UpdateCargoOperationalRiskControls(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString()),
                       General.GetNullableString(txtopertaionalhazard.Text.Trim()), General.GetNullableString(txtcontrolprecautions.Text.Trim()), General.GetNullableString(txtAspect.Text.Trim()));
                    }
                }
                
                Bindoperationalprocedure();

                ucStatus.Text = "Information Updated.";
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

    private bool IsValidOperationalHazard()
    {
        ucError.HeaderMessage = "Please provide the following required information";

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
        DataSet ds = PhoenixInspectionOperationalRiskControls.EditJHARAOperationalRiskControls(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString()),General.GetNullableInteger(ViewState["RATYPE"].ToString()));
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            txtAspect.Text = dt.Rows[0]["FLDASPECT"].ToString();
            txtcontrolprecautions.Text = dt.Rows[0]["FLDCONTROLPRECAUTIONS"].ToString();
            txtopertaionalhazard.Text = dt.Rows[0]["FLDOPERATIONALHAZARD"].ToString();
        }
    }
}