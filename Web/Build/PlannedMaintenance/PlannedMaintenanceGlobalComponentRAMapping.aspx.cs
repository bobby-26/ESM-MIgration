using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Web.UI;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class PlannedMaintenanceGlobalComponentRAMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Map", "MAP",ToolBarDirection.Right);
            MenuTemplateMapping.AccessRights = this.ViewState;
            MenuTemplateMapping.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["GLOBALCOMPONENTID"] = "";

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                if (Request.QueryString["GLOBALCOMPONENTID"] != null)
                {
                    ViewState["GLOBALCOMPONENTID"] = Request.QueryString["GLOBALCOMPONENTID"].ToString();

                }
                if (Request.QueryString["VESSELID"] != null)
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();

                }

                ViewState["RALIST"] = "";
                BindCategory();
                BindData();
            }


        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("MAP"))
            {
                string ralist = General.RadCheckBoxList(cblRA);

                if (IsValidTemplateMapping(ralist))
                {

                    PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentRAMap(new Guid(ViewState["GLOBALCOMPONENTID"].ToString()), ralist, ViewState["VESSELID"] != null ? General.GetNullableInteger(ViewState["VESSELID"].ToString()) : null);

                    ucStatus.Text = "RA mapped successfully";
                    BindData();

                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " closeTelerikWindow('codehelp1','RA','true');";
                    Script += "</script>" + "\n";
                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                    RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);

                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1', 'RA','t');", true);
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
    private void BindData()
    {
        DataSet ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentRAList(General.GetNullableGuid(ViewState["GLOBALCOMPONENTID"].ToString()), General.GetNullableInteger(ddlCategory.SelectedValue));
        
        cblRA.DataSource = ds.Tables[0];
        cblRA.DataBindings.DataTextField = "FLDDESCRIPTION";
        cblRA.DataBindings.DataValueField = "FLDRISKASSESSMENTMACHINERYID";
        cblRA.DataBind();

        if (ds.Tables[1].Rows.Count > 0)
        {
            BindCheckbox(ds.Tables[1].Rows[0]["FLDRALIST"].ToString());
        }
    }
    protected void BindCheckbox(string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                cblRA.SelectedValue = item.ToLower();
            }
        }
    }
    private bool IsValidTemplateMapping(string jhaid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (jhaid.Equals(""))
            ucError.ErrorMessage = "Atleast select 1 RA to map.";

        return (!ucError.IsError);
    }

    protected void SelectAll(object sender, EventArgs e)
    {
        if (chkCheckAll.Checked == true)
        {
            foreach (ButtonListItem item in cblRA.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem item in cblRA.Items)
            {
                item.Selected = false;
            }
        }
    }

    protected void BindCategory()
    {
        DataSet ds = new DataSet();

        //ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivityByCategory(5);
        ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentActivity(null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
        }
    }

    protected void ddlCategory_Changed(object sender, EventArgs e)
    {
        BindData();
    }

    protected void ucType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
}
