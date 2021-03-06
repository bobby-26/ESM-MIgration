using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI;
using System.Collections.Specialized;
using Telerik.Web.UI;


public partial class DocumentManagement_DocumentManagementJHATemplateMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Import", "IMPORT", ToolBarDirection.Right);
            MenuTemplateMapping.AccessRights = this.ViewState;
            MenuTemplateMapping.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["RISKASSESSMENTPROCESSID"] = "";

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                if (Request.QueryString["RISKASSESSMENTPROCESSID"] != null)
                {
                    ViewState["RISKASSESSMENTPROCESSID"] = Request.QueryString["RISKASSESSMENTPROCESSID"].ToString();

                }
                if (Request.QueryString["FORMID"] != null)
                {
                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                  

                }
                ViewState["JHALIST"] = "";
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

    //protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

    //        if (CommandName.ToUpper().Equals("IMPORT"))
    //        {
    //            string jhalist = General.ReadCheckBoxList(cblJHA);

    //            PhoenixDocumentManagementForm.FormRiskAssessmentJHAImport(
    //                                                General.GetNullableGuid(ViewState["FORMID"].ToString()),
    //                                                General.GetNullableString(jhalist));

    //            ucStatus.Text = "JHA has been Imported successfully";
    //            BindData();

    //            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("IMPORT"))
            {
                string jhalist = General.RadCheckBoxList(cblJHA); 

                PhoenixDocumentManagementForm.FormRiskAssessmentJHAImport(
                                                    General.GetNullableGuid(ViewState["FORMID"].ToString()),
                                                    General.GetNullableString(jhalist));

                ucStatus.Text = "JHA has been Imported successfully";
                //BindData();
                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        

              //  ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
    //        if (CommandName.ToUpper().Equals("IMPORT"))
    //        {
    //            string jhalist = General.RadCheckBoxList(cblJHA);

    //            PhoenixInspectionRiskAssessmentProcessExtn.InspectionRiskAssessmentJHAImport(
    //                                                General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
    //                                                General.GetNullableString(jhalist));

    //            ucStatus.Text = "JHA has been Imported successfully";
    //            BindData();

    //            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private void BindData()
    {

        DataSet ds = PhoenixInspectionRiskAssessmentProcessExtn.InspectionRiskAssessmentJHAMapping(
                    General.GetNullableInteger(ddlCategory.SelectedValue),
                    General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                    General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        cblJHA.DataSource = ds.Tables[0];
        cblJHA.DataBindings.DataTextField = "FLDJOB";
        cblJHA.DataBindings.DataValueField = "FLDJOBHAZARDID";
        cblJHA.DataBind();

        if (ds.Tables[1].Rows.Count > 0)
        {
            General.RadBindCheckBoxList(cblJHA, ds.Tables[1].Rows[0]["FLDIMPORTEDJHA"].ToString());
        }
    }

    private bool IsValidTemplateMapping(string jhaid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (jhaid.Equals(""))
            ucError.ErrorMessage = "Atleast select 1 JHA to Import.";

        return (!ucError.IsError);
    }

    protected void SelectAll(object sender, EventArgs e)
    {
        if (chkCheckAll.Checked == true)
        {
            foreach (ButtonListItem item in cblJHA.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem item in cblJHA.Items)
            {
                item.Selected = false;
            }
        }
    }

    protected void BindCategory()
    {
        DataSet ds = new DataSet();

        //ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivityByCategory(5);
        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(null, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));


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




//{
//    protected void Page_Load(object sender, EventArgs e)
//    {
//        try
//        {
//            SessionUtil.PageAccessRights(this.ViewState);

//            PhoenixToolbar toolbar = new PhoenixToolbar();
//            toolbar.AddButton("Import", "IMPORT", ToolBarDirection.Right);
//            MenuTemplateMapping.AccessRights = this.ViewState;
//            MenuTemplateMapping.MenuList = toolbar.Show();
//            if (!IsPostBack)
//            {
//                ViewState["RISKASSESSMENTPROCESSID"] = "";


//                ViewState["COMPANYID"] = "";
//                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
//                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
//                {
//                    ViewState["COMPANYID"] = nvc.Get("QMS");
//                }

//                if (Request.QueryString["RISKASSESSMENTPROCESSID"] != null)
//                {
//                    ViewState["RISKASSESSMENTPROCESSID"] = Request.QueryString["RISKASSESSMENTPROCESSID"].ToString();

//                }
//                if (Request.QueryString["FORMID"] != null)
//                {
//                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
//                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();

//                }
//                ViewState["JHALIST"] = "";
//                BindCategory();
//                BindData();
//            }


//        }
//        catch (Exception ex)
//        {

//            ucError.ErrorMessage = ex.Message;
//            ucError.Visible = true;
//        }
//    }
//    protected void MenuRATemplate_TabStripCommand(object sender, EventArgs e)
//    {
//        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
//        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

//        if (CommandName.ToUpper().Equals("FIND"))
//        {
//            BindData();

//        }

//    }
//    protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
//    {
//        try
//        {
//            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
//            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

//            if (CommandName.ToUpper().Equals("IMPORT"))
//            {
//                string jhalist = General.ReadCheckBoxList(cblJHA);

//                PhoenixDocumentManagementForm.FormRiskAssessmentJHAImport(
//                                                    General.GetNullableGuid(ViewState["FORMID"].ToString()),
//                                                    General.GetNullableString(jhalist));

//                ucStatus.Text = "JHA has been Imported successfully";
//                BindData();

//                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
//            }
//        }
//        catch (Exception ex)
//        {
//            ucError.ErrorMessage = ex.Message;
//            ucError.Visible = true;
//        }
//    }
//    private void BindData()
//    {

//        DataSet ds = PhoenixInspectionRiskAssessmentProcess.InspectionRiskAssessmentJHAMapping(
//                    General.GetNullableInteger(ddlCategory.SelectedValue),
//                    General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
//                    General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
//                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

//        cblJHA.DataSource = ds.Tables[0];
//        cblJHA.DataTextField = "FLDJOB";
//        cblJHA.DataValueField = "FLDJOBHAZARDID";
//        cblJHA.DataBind();

//        if (ds.Tables[1].Rows.Count > 0)
//        {
//            General.BindCheckBoxList(cblJHA, ds.Tables[1].Rows[0]["FLDIMPORTEDJHA"].ToString());
//        }
//    }

//    private bool IsValidTemplateMapping(string jhaid)
//    {
//        ucError.HeaderMessage = "Please provide the following required information";

//        if (jhaid.Equals(""))
//            ucError.ErrorMessage = "Atleast select 1 JHA to Import.";

//        return (!ucError.IsError);
//    }

//    protected void SelectAll(object sender, EventArgs e)
//    {
//        if (chkCheckAll.Checked == true)
//        {
//            foreach (ListItem item in cblJHA.Items)
//            {
//                item.Selected = true;
//            }
//        }
//        else
//        {
//            foreach (ListItem item in cblJHA.Items)
//            {
//                item.Selected = false;
//            }
//        }
//    }

//    protected void BindCategory()
//    {
//        DataSet ds = new DataSet();

//        ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivityByCategory(5);
//        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(null, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));


//        if (ds.Tables[0].Rows.Count > 0)
//        {
//            ddlCategory.Items.Clear();
//            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
//            ddlCategory.DataSource = ds.Tables[0];
//            ddlCategory.DataBind();
//        }
//    }

//    protected void ddlCategory_Changed(object sender, EventArgs e)
//    {
//        BindData();
//    }

//    protected void ucType_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        BindData();
//    }
//}