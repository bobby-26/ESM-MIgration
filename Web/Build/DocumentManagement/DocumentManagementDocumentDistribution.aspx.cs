using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class DocumentManagementDocumentDistribution : PhoenixBasePage
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
            MenuVessel.AccessRights = this.ViewState;
            MenuVessel.MenuList = toolbar.Show();
            MenuVessel.SelectedMenuIndex = 1;

            //toolbar = new PhoenixToolbar();
            //toolbar.AddButton("General", "GENERAL", ToolBarDirection.Right);
            //toolbar.AddButton("Bulk", "BULK", ToolBarDirection.Right);
            //MenuBulk.AccessRights = this.ViewState;
            //MenuBulk.MenuList = toolbar.Show();
            //MenuBulk.SelectedMenuIndex = 0;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Deny", "HOLD", ToolBarDirection.Right);
            toolbar.AddButton("Distribute", "DISTRIBUTE", ToolBarDirection.Right);
            MenuDocument.AccessRights = this.ViewState;
            MenuDocument.MenuList = toolbar.Show();





            toolbar = new PhoenixToolbar();
            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementDocumentDistribution.aspx", "Filter", "search.png", "FIND");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementDocumentDistribution.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuDocumentList.AccessRights = this.ViewState;
            MenuDocumentList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SelectedVesselList"] = "";
                ViewState["VesselSelection"] = "";

                BindVesselTypeList();
                BindVesselList();
                BindFormCategory();
                BindFormList();
                BindJHACategory();
                BindJHAList();
                BindRACategory();
                BindRAList();
                ViewState["DISTRIBUTEYN"] = "0";
            }
            string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divRA);resizeDiv(divVesselType);resizeDiv(divVessel)\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void BindFormCategory()
    {
        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ddlFormCategory.Items.Clear();
        ddlFormCategory.DataSource = PhoenixDocumentManagementCategory.ListDocumentCategory(companyid);
        ddlFormCategory.DataTextField = "FLDCATEGORYNAME";
        ddlFormCategory.DataValueField = "FLDCATEGORYID";
        ddlFormCategory.DataBind();
    }

    protected void BindFormList()
    {
        if (General.GetNullableGuid(ddlFormCategory.SelectedValue) != null)
        {
            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            chkForm.Items.Clear();
            chkForm.DataSource = PhoenixDocumentManagementDistribution.ListFormByCategory(companyid, General.GetNullableGuid(ddlFormCategory.SelectedValue));
            chkForm.DataBindings.DataTextField = "FLDFORMNAME";
            chkForm.DataBindings.DataValueField = "FLDFORMID";
            chkForm.DataBind();
        }
    }

    protected void BindJHACategory()
    {
        ddlJHACategory.Items.Clear();
        //ddlJHACategory.DataSource = PhoenixDocumentManagementDistribution.RiskAssessmentActivityListByCategory(null);
        ddlJHACategory.DataSource = PhoenixDocumentManagementDistribution.RiskAssessmentActivityListByCategory(null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        ddlJHACategory.DataTextField = "FLDACTIVITYNAME";
        ddlJHACategory.DataValueField = "FLDACTIVITYID";
        ddlJHACategory.DataBind();
    }

    protected void BindCategory()
    {        
        ddlJHACategory.Items.Clear();
        ddlJHACategory.DataSource = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentElement();
        ddlJHACategory.DataTextField = "FLDNAME";
        ddlJHACategory.DataValueField = "FLDCATEGORYID";
        ddlJHACategory.DataBind();
    }

    protected void BindJHAList()
    {
        if (General.GetNullableInteger(ddlJHACategory.SelectedValue) != null)
        {
            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            if (rblJHA.SelectedValue.Equals("1"))
            {                
                chkJHA.Items.Clear();
                chkJHA.DataSource = PheonixDocumentManagementDistributionExtn.ListJHAByCategory(companyid, General.GetNullableInteger(ddlJHACategory.SelectedValue));                
            }
            else            
            {
                chkJHA.Items.Clear();
                chkJHA.DataSource = PhoenixDocumentManagementDistribution.ListJHAByCategory(companyid, General.GetNullableInteger(ddlJHACategory.SelectedValue));                
            }

            chkJHA.DataBindings.DataTextField = "FLDITEMNAME";
            chkJHA.DataBindings.DataValueField = "FLDITEMID";
            chkJHA.DataBind();
        }
    }

    protected void BindRACategory()
    {
        ddlRACategory.Items.Clear();
        //ddlRACategory.DataSource = PhoenixDocumentManagementDistribution.RiskAssessmentActivityListByCategory(null);
        ddlRACategory.DataSource = PhoenixDocumentManagementDistribution.RiskAssessmentActivityListByCategory(null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        ddlRACategory.DataTextField = "FLDACTIVITYNAME";
        ddlRACategory.DataValueField = "FLDACTIVITYID";
        ddlRACategory.DataBind();
    }

    protected void BindNewRACategory()
    {
        ddlRACategory.Items.Clear();
        ddlRACategory.DataSource = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentActivity(null);
        ddlRACategory.DataTextField = "FLDNAME";
        ddlRACategory.DataValueField = "FLDACTIVITYID";
        ddlRACategory.DataBind();
    }

    protected void BindRAList()
    {
        if (General.GetNullableInteger(ddlRACategory.SelectedValue) != null)
        {
            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            if (rblRA.SelectedValue.Equals("1"))
            {
                chkRA.Items.Clear();
                chkRA.DataSource = PheonixDocumentManagementDistributionExtn.ListRAByCategory(companyid, General.GetNullableInteger(ddlRACategory.SelectedValue));
            }
            else
            {
                chkRA.Items.Clear();
                chkRA.DataSource = PhoenixDocumentManagementDistribution.ListRAByCategory(companyid, General.GetNullableInteger(ddlRACategory.SelectedValue));
            }
            chkRA.DataBindings.DataTextField = "FLDITEMNAME";
            chkRA.DataBindings.DataValueField = "FLDITEMID";
            chkRA.DataBind();
        }
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

    protected string GetSelectedForm()
    {
        StringBuilder strForm = new StringBuilder();
        foreach (ButtonListItem item in chkForm.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strForm.Append(item.Value.ToString());
                strForm.Append(",");
            }
        }

        if (strForm.Length > 1)
            strForm.Remove(strForm.Length - 1, 1);

        string formList = strForm.ToString();
        return formList;
    }

    protected string GetSelectedVessel()
    {
        StringBuilder strvesseltype = new StringBuilder();
        foreach (ButtonListItem item in chkVessel.Items)
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

    protected string GetSelectedJHA()
    {
        StringBuilder strJHA = new StringBuilder();
        foreach (ButtonListItem item in chkJHA.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strJHA.Append(item.Value.ToString());
                strJHA.Append(",");
            }
        }

        if (strJHA.Length > 1)
            strJHA.Remove(strJHA.Length - 1, 1);

        string jhalist = strJHA.ToString();
        return jhalist;
    }

    protected string GetSelectedRA()
    {
        StringBuilder strRA = new StringBuilder();
        foreach (ButtonListItem item in chkRA.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strRA.Append(item.Value.ToString());
                strRA.Append(",");
            }
        }

        if (strRA.Length > 1)
            strRA.Remove(strRA.Length - 1, 1);

        string jhalist = strRA.ToString();
        return jhalist;
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

    //protected void SetSelectedVesselTypeAndVessel(Guid? documentid)
    //{
    //    DataSet ds = PhoenixDocumentManagementDocument.GetDistributedDocumentList(documentid);

    //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //    {
    //        string strveseltype = ds.Tables[0].Rows[0]["FLDVESSELTYPE"].ToString();
    //        string strvessel = ds.Tables[0].Rows[0]["FLDVESSELLIST"].ToString();

    //        foreach (ButtonListItem li in chkVesselType.Items)
    //            li.Selected = false;
    //        foreach (ButtonListItem li in chkVessel.Items)
    //            li.Selected = false;

    //        foreach (string item in strveseltype.Split(','))
    //        {
    //            if (item.Trim() != "")
    //            {
    //                if (chkVesselType.Items.FindByValue(item) != null)
    //                {                        
    //                    chkVesselType.Items.FindByValue(item).Selected = true;
    //                    //chkVesselType.Items.FindByValue(item).Attributes.Add("style","font-weight:bold;");
    //                }
    //            }
    //        }

    //        foreach (string item in strvessel.Split(','))
    //        {
    //            if (item.Trim() != "")
    //            {
    //                if (chkVessel.Items.FindByValue(item) != null)
    //                {
    //                    chkVessel.Items.FindByValue(item).Selected = true;
    //                    //chkVessel.Items.FindByValue(item).Attributes.Add("style", "font-weight:bold;");
    //                }
    //            }
    //        }
    //    }
    //}   

    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);

            if (CommandName.ToUpper().Equals("DISTRIBUTE"))
            {
                ViewState["DISTRIBUTEYN"] = "0";
                RadWindowManager1.RadConfirm("Do you want to distribute.? Y/N", "confirm", 320, 150, null, "Confirm");
            }
            else if (CommandName.ToUpper().Equals("HOLD"))
            {
                ViewState["DISTRIBUTEYN"] = "1";
                RadWindowManager1.RadConfirm("Do you want to continue.? Y/N", "confirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuDocumentList_TabStripCommand(object sender, EventArgs e)
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
                foreach (ButtonListItem li in chkJHA.Items)
                    li.Selected = false;
                foreach (ButtonListItem li in chkRA.Items)
                    li.Selected = false;
                foreach (ButtonListItem li in chkForm.Items)
                    li.Selected = false;
                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = false;
                foreach (ButtonListItem li in chkVessel.Items)
                    li.Selected = false;

                chkVesselTypeAll.Checked = false;
                chkJHAAll.Checked = false;
                chkFormAll.Checked = false;
                chkRAAll.Checked = false;

                string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVessel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("VESSEL"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentListByVessel.aspx?");
            }
            if (CommandName.ToUpper().Equals("BULK"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentBulkDistribution.aspx?");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void chkDocument_Changed(object sender, EventArgs e)
    {
        string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
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

        string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);

            string documentlist = GetSelectedForm();
            string vessellist = GetSelectedVessel();
            string jhalist = GetSelectedJHA();
            string ralist = GetSelectedRA();
            string newjhalist = GetSelectedJHA();

            jhalist = jhalist + (General.GetNullableString(ralist) != null ? (',' + ralist) : "");

            int? distributeyn = General.GetNullableInteger(ViewState["DISTRIBUTEYN"].ToString());

            if (General.GetNullableString(vessellist) != null)
            {
                if (General.GetNullableString(documentlist) != null)
                    PhoenixDocumentManagementDocument.DocumentDistributionUpdate(
                              PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , vessellist
                            , documentlist
                            , distributeyn);


                if (General.GetNullableString(jhalist) != null)
                {
                    if ((rblJHA.SelectedValue.Equals("1")) && (General.GetNullableString(newjhalist) != null))
                    {
                        PheonixDocumentManagementDistributionExtn.RiskAssessmentDistributionUpdate(
                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , vessellist
                                    , distributeyn
                                    , newjhalist);
                    }
                    if ((rblRA.SelectedValue.Equals("1")) && (General.GetNullableString(ralist) != null))
                    {
                        PheonixDocumentManagementDistributionExtn.RiskAssessmentDistributionUpdate(
                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , vessellist
                                    , distributeyn
                                    , ralist);
                    }
                    else
                    {
                        PhoenixDocumentManagementDocument.RiskAssessmentDistributionUpdate(
                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , vessellist
                                    , distributeyn
                                    , jhalist);
                    }
                }

                if (distributeyn == 0)
                    ucStatus.Text = "Files are distributed successfully.";
                else
                    ucStatus.Text = "Distribution is denied successfully.";

                ViewState["DISTRIBUTEYN"] = "0";

                chkFormAll.Checked = false;
                foreach (ButtonListItem item in chkForm.Items)
                    item.Selected = false;

                chkJHAAll.Checked = false;
                foreach (ButtonListItem item in chkJHA.Items)
                    item.Selected = false;

                chkRAAll.Checked = false;
                foreach (ButtonListItem item in chkRA.Items)
                    item.Selected = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
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

            string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void ddlFormCategory_Changed(object sender, EventArgs e)
    {
        BindFormList();

        string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    protected void ddlJHACategory_Changed(object sender, EventArgs e)
    {
        BindJHAList();

        string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    protected void chkForm_Changed(object sender, EventArgs e)
    {

    }

    protected void chkFormAll_Changed(object sender, EventArgs e)
    {
        if (chkFormAll.Checked == true)
        {
            foreach (ButtonListItem li in chkForm.Items)
                li.Selected = true;
        }
        else
        {
            foreach (ButtonListItem li in chkForm.Items)
                li.Selected = false;
        }

        string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    protected void chkJHAAll_Changed(object sender, EventArgs e)
    {
        if (chkJHAAll.Checked == true)
        {
            foreach (ButtonListItem li in chkJHA.Items)
                li.Selected = true;
        }
        else
        {
            foreach (ButtonListItem li in chkJHA.Items)
                li.Selected = false;
        }

        string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    //protected void MenuBulk_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

    //        if (CommandName.ToUpper().Equals("BULK"))
    //        {
    //            Response.Redirect("../DocumentManagement/DocumentManagementDocumentBulkDistribution.aspx?");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void chkRAAll_Changed(object sender, EventArgs e)
    {
        if (chkRAAll.Checked == true)
        {
            foreach (ButtonListItem li in chkRA.Items)
                li.Selected = true;
        }
        else
        {
            foreach (ButtonListItem li in chkRA.Items)
                li.Selected = false;
        }

        string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    protected void ddlRACategory_Changed(object sender, EventArgs e)
    {
        BindRAList();

        string script = "resizeDiv(divForm);resizeDiv(divJHA);resizeDiv(divVesselType);resizeDiv(divVessel);resizeDiv(divRA);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    protected void rblJHA_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblJHA.SelectedValue.Equals("1"))
        {
            BindCategory();
        }
        else
        {
            BindJHACategory();
        }
        BindJHAList();
    }

    protected void rblRA_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindNewRACategory();
        BindRAList();
    }
}
