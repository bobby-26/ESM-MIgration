using System;
using System.Web.UI;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class DocumentManagementDocumentBulkDistribution : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            confirm.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Vessel", "VESSEL", ToolBarDirection.Right);
            toolbar.AddButton("General Distribution", "GENERAL", ToolBarDirection.Right);
            toolbar.AddButton("Bulk Distribution", "BULK", ToolBarDirection.Right);
            MenuDistribution.AccessRights = this.ViewState;
            MenuDistribution.MenuList = toolbar.Show();
            MenuDistribution.SelectedMenuIndex = 2;

            //toolbar = new PhoenixToolbar();
            //toolbar.AddButton("General", "GENERAL", ToolBarDirection.Right);
            //toolbar.AddButton("Bulk", "BULK", ToolBarDirection.Right);
            //MenuBulk.AccessRights = this.ViewState;
            //MenuBulk.MenuList = toolbar.Show();
            //MenuBulk.SelectedMenuIndex = 1;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Deny", "HOLD", ToolBarDirection.Right);
            toolbar.AddButton("Distribute", "DISTRIBUTE", ToolBarDirection.Right);
            MenuDocument.AccessRights = this.ViewState;
            MenuDocument.MenuList = toolbar.Show();


            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementDocumentBulkDistribution.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuFilter.AccessRights = this.ViewState;
            MenuFilter.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["SelectedVesselList"] = "";
                ViewState["VesselSelection"] = "";

                BindManualList();
                BindJHACategory();
                BindVesselTypeList();
                BindVesselList();
                BindParentFormCategory();
                BindFormCategory();
                ViewState["DISTRIBUTEYN"] = "0";
            }
            string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindManualList()
    {
        chkManual.Items.Clear();
        chkManual.DataSource = PhoenixDocumentManagementDistribution.DocumentListByCategory(null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        chkManual.DataBindings.DataTextField = "FLDDOCUMENTNAME";
        chkManual.DataBindings.DataValueField = "FLDDOCUMENTID";
        chkManual.DataBind();
    }

    protected void BindParentFormCategory()
    {
        ddlParentFormCategory.Items.Clear();
        ddlParentFormCategory.DataSource = PhoenixDocumentManagementDistribution.ListCategory(
                                                          PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                        , null);
        ddlParentFormCategory.DataTextField = "FLDCATEGORYNAME";
        ddlParentFormCategory.DataValueField = "FLDCATEGORYID";
        ddlParentFormCategory.DataBind();
    }

    protected void BindFormCategory()
    {
        if (General.GetNullableGuid(ddlParentFormCategory.SelectedValue) != null)
        {
            chkFormCategory.Items.Clear();
            chkFormCategory.DataSource = PhoenixDocumentManagementDistribution.ListCategory(
                                                              PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                            , General.GetNullableGuid(ddlParentFormCategory.SelectedValue));
            chkFormCategory.DataBindings.DataTextField = "FLDCATEGORYNAME";
            chkFormCategory.DataBindings.DataValueField = "FLDCATEGORYID";
            chkFormCategory.DataBind();
        }
    }

    protected void BindJHACategory()
    {
        chkJHACategory.Items.Clear();
        //chkJHACategory.DataSource = PhoenixDocumentManagementDistribution.RiskAssessmentActivityListByCategory(null);
        chkJHACategory.DataSource = PhoenixDocumentManagementDistribution.RiskAssessmentActivityListByCategory(null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        chkJHACategory.DataBindings.DataTextField = "FLDACTIVITYNAME";
        chkJHACategory.DataBindings.DataValueField = "FLDACTIVITYID";
        chkJHACategory.DataBind();
    }

    protected void BindVesselTypeList()
    {
        chkVesselType.Items.Clear();
        chkVesselType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
        chkVesselType.DataBindings.DataTextField = "FLDHARDNAME";
        chkVesselType.DataBindings.DataValueField = "FLDHARDCODE";
        chkVesselType.DataBind();
    }

    protected void BindVesselList()
    {
        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        chkVessel.Items.Clear();
        chkVessel.DataSource = PhoenixDocumentManagementDocument.DMSVesselList(null, companyid);
        chkVessel.DataBindings.DataTextField = "FLDVESSELNAME";
        chkVessel.DataBindings.DataValueField = "FLDVESSELID";
        chkVessel.DataBind();
    }

    protected string GetSelectedManuals()
    {
        StringBuilder strManuals = new StringBuilder();
        foreach (ButtonListItem item in chkManual.Items)
        {
            if (item.Selected == true)
            {
                strManuals.Append(item.Value.ToString());
                strManuals.Append(",");
            }
        }

        if (strManuals.Length > 1)
            strManuals.Remove(strManuals.Length - 1, 1);

        string categoryList = strManuals.ToString();
        return categoryList;
    }

    protected string GetSelectedJHACategory()
    {
        StringBuilder strJHACategory = new StringBuilder();
        foreach (ButtonListItem item in chkJHACategory.Items)
        {
            if (item.Selected == true)
            {
                strJHACategory.Append(item.Value.ToString());
                strJHACategory.Append(",");
            }
        }

        if (strJHACategory.Length > 1)
            strJHACategory.Remove(strJHACategory.Length - 1, 1);

        string categoryList = strJHACategory.ToString();
        return categoryList;
    }

    protected string GetSelectedFormCategory()
    {
        StringBuilder strFormCategory = new StringBuilder();
        foreach (ButtonListItem item in chkFormCategory.Items)
        {
            if (item.Selected == true)
            {
                strFormCategory.Append(item.Value.ToString());
                strFormCategory.Append(",");
            }
        }

        if (strFormCategory.Length > 1)
            strFormCategory.Remove(strFormCategory.Length - 1, 1);

        string categoryList = strFormCategory.ToString();
        return categoryList;
    }

    protected string GetSelectedVessels()
    {
        StringBuilder strVessel = new StringBuilder();
        foreach (ButtonListItem item in chkVessel.Items)
        {
            if (item.Selected == true)
            {
                strVessel.Append(item.Value.ToString());
                strVessel.Append(",");
            }
        }

        if (strVessel.Length > 1)
            strVessel.Remove(strVessel.Length - 1, 1);

        string categoryList = strVessel.ToString();
        return categoryList;
    }

    protected string GetSelectedVesselType()
    {
        StringBuilder strvesseltype = new StringBuilder();
        foreach (ButtonListItem item in chkVesselType.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strvesseltype.Append(item.Value.ToString());
                strvesseltype.Append(",");
            }
        }

        if (strvesseltype.Length > 1)
            strvesseltype.Remove(strvesseltype.Length - 1, 1);

        string vesseltype = strvesseltype.ToString();
        return vesseltype;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void MenuDistribution_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("VESSEL"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentListByVessel.aspx?");
            }

            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentDistribution.aspx?");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void MenuBulk_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

    //        if (CommandName.ToUpper().Equals("GENERAL"))
    //        {
    //            Response.Redirect("../DocumentManagement/DocumentManagementDocumentDistribution.aspx?");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);

            if (CommandName.ToUpper().Equals("DISTRIBUTE"))
            {
                ViewState["DISTRIBUTEYN"] = "0";
                RadWindowManager1.RadConfirm("Do you want to distribute.? Y/N", "confirm", 320, 150, null, "Distribute");
            }
            else if (CommandName.ToUpper().Equals("HOLD"))
            {
                ViewState["DISTRIBUTEYN"] = "1";
                RadWindowManager1.RadConfirm("Do you want to continue.? Y/N", "confirm", 320, 150, null, "Deny");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {

            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                foreach (ButtonListItem li in chkManual.Items)
                    li.Selected = false;
                foreach (ButtonListItem li in chkJHACategory.Items)
                    li.Selected = false;
                foreach (ButtonListItem li in chkFormCategory.Items)
                    li.Selected = false;
                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = false;
                foreach (ButtonListItem li in chkVessel.Items)
                    li.Selected = false;

                //chkFormCategoryAll.Checked = false;
                chkManualAll.Checked = false;
                chkVesselTypeAll.Checked = false;
                chkJHACategoryAll.Checked = false;
            }
            string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void chkVesselType_Changed(object sender, EventArgs e)
    {
        string selectedcategorylist = GetSelectedVesselType();

        //ViewState["SelectedVesselList"] = "";
        foreach (ButtonListItem item in chkVessel.Items)
            item.Selected = false;

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        DataSet ds = PhoenixDocumentManagementDocument.DMSVesselList(selectedcategorylist, companyid);

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (ButtonListItem item in chkVessel.Items)
                {
                    string vesselid = dr["FLDVESSELID"].ToString();

                    if (item.Value == vesselid && !item.Selected)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }

        string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    protected void chkVesselTypeAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkVesselTypeAll.Checked == true)
            {
                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = true;

                foreach (ButtonListItem li in chkVessel.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = false;

                foreach (ButtonListItem li in chkVessel.Items)
                    li.Selected = false;
            }

            string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void chkFormCategory_Changed(object sender, EventArgs e)
    {
        RadCheckBoxList cbk = (RadCheckBoxList)sender;

        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "chkFormCategory$ctl00")
        {
            ButtonListItem item = cbk.Items[0];
            if (item.Selected == true)
            {
                foreach (ButtonListItem li in chkFormCategory.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ButtonListItem li in chkFormCategory.Items)
                    li.Selected = false;
            }
        }
        string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    protected void chkJHACategoryAll_Changed(object sender, EventArgs e)
    {
        if (chkJHACategoryAll.Checked == true)
        {
            foreach (ButtonListItem li in chkJHACategory.Items)
                li.Selected = true;
        }
        if (chkJHACategoryAll.Checked == false)
        {
            foreach (ButtonListItem li in chkJHACategory.Items)
                li.Selected = false;
        }

        string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    protected void chkManualAll_Changed(object sender, EventArgs e)
    {
        if (chkManualAll.Checked == true)
        {
            foreach (ButtonListItem li in chkManual.Items)
                li.Selected = true;
        }
        if (chkManualAll.Checked == false)
        {
            foreach (ButtonListItem li in chkManual.Items)
                li.Selected = false;
        }

        string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);

            string strManuals = GetSelectedManuals();
            string strJHACategory = GetSelectedJHACategory();
            string strFormCategory = GetSelectedFormCategory();
            string strVessels = GetSelectedVessels();
            int? distributionyn = General.GetNullableInteger(ViewState["DISTRIBUTEYN"].ToString());

            PhoenixDocumentManagementDistribution.DocumentBulkDistribution(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            strVessels,
                                            strManuals,
                                            strFormCategory,
                                            PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                            distributionyn);

            if (rblJHARA.SelectedValue.Equals("1"))
            {
                PheonixDocumentManagementDistributionExtn.RiskAssessmentBulkDistribution(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            strVessels,
                                                            strJHACategory,
                                                            PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                            distributionyn,
                                                            General.GetNullableInteger(ddlRADistributionType.SelectedValue));
            }
            else
            {
                PhoenixDocumentManagementDistribution.RiskAssessmentBulkDistribution(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                strVessels,
                                                                strJHACategory,
                                                                PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                distributionyn,
                                                                General.GetNullableInteger(ddlRADistributionType.SelectedValue));
            }

            if (distributionyn == 0)
                ucStatus.Text = "Document distribution has been initiated successfully.";
            else
                ucStatus.Text = "Document deny has been initiated successfully.";

            chkManualAll.Checked = false;
            chkJHACategoryAll.Checked = false;

            foreach (ButtonListItem item in chkManual.Items)
                item.Selected = false;

            foreach (ButtonListItem item in chkJHACategory.Items)
                item.Selected = false;

            foreach (ButtonListItem item in chkFormCategory.Items)
                item.Selected = false;

            ViewState["DISTRIBUTEYN"] = "0";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void ddlParentFormCategory_Changed(object sender, EventArgs e)
    {
        BindFormCategory();

        string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    protected void BindCategory()
    {
        chkJHACategory.Items.Clear();
        chkJHACategory.DataSource = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentElement();
        chkJHACategory.DataBindings.DataTextField = "FLDNAME";
        chkJHACategory.DataBindings.DataValueField = "FLDCATEGORYID";
        chkJHACategory.DataBind();
    }

    protected void BindNewRAActivity()
    {
        chkJHACategory.Items.Clear();
        chkJHACategory.DataSource = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentActivity(null);
        chkJHACategory.DataBindings.DataTextField = "FLDNAME";
        chkJHACategory.DataBindings.DataValueField = "FLDACTIVITYID";
        chkJHACategory.DataBind();
    }

    protected void ddlRADistributionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((rblJHARA.SelectedValue.Equals("1")) && (ddlRADistributionType.SelectedValue.Equals("1")))
        {
            BindCategory();
        }
        else if ((rblJHARA.SelectedValue.Equals("1")) && (ddlRADistributionType.SelectedValue.Equals("2")))
        {
            BindNewRAActivity();
        }
        else
        {
            BindJHACategory();
        }
    }

    protected void rblJHARA_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((rblJHARA.SelectedValue.Equals("1")) && (ddlRADistributionType.SelectedValue.Equals("1")))
        {
            BindCategory();
        }
        else if ((rblJHARA.SelectedValue.Equals("1")) && (ddlRADistributionType.SelectedValue.Equals("2")))
        {
            BindNewRAActivity();
        }
        else
        {
            BindJHACategory();
        }
    }
}
