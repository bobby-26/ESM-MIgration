using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;

public partial class Inspection_InspectionPSCMOUSRPPriorityDurationAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {            

            BindPSCMOU();
            BindRiskLevel();
            BindRiskPriority();

        }
    }

    protected void BindRiskLevel()
    {
        ddlRiskAdd.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURiskLevel();
        ddlRiskAdd.DataTextField = "FLDNAME";
        ddlRiskAdd.DataValueField = "FLDLEVELID";
        ddlRiskAdd.DataBind();
        ddlRiskAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindPSCMOU()
    {
        ddlCompany.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindRiskPriority()
    {
        ddlprioritylvlAdd.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUSRPPriority();
        ddlprioritylvlAdd.DataTextField = "FLDSHORTCODE";
        ddlprioritylvlAdd.DataValueField = "FLDPRIORITYID";
        ddlprioritylvlAdd.DataBind();
        ddlprioritylvlAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }


    protected void MenuDocumentCategoryMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string scriptClosePopup = "";
                scriptClosePopup += "<script language='javaScript' id='AddressAddNew'>" + "\n";
                scriptClosePopup += "fnReloadList('AddAddress');";
                scriptClosePopup += "</script>" + "\n";

                string scriptRefreshDontClose = "";
                scriptRefreshDontClose += "<script language='javaScript' id='AddressAddNew'>" + "\n";
                scriptRefreshDontClose += "fnReloadList('VesselOtherCompany');";
                scriptRefreshDontClose += "</script>" + "\n";


                if (ddlRiskAdd.SelectedValue == "")
                {
                    ucError.ErrorMessage = "Risk is required";
                    ucError.Visible = true;
                    return;
                }
                if (ddlprioritylvlAdd.SelectedValue == "")
                {
                    ucError.ErrorMessage = "Class Society is required";
                    ucError.Visible = true;
                    return;
                }
                if (txtdurationfrom.Text == "")
                {
                    ucError.ErrorMessage = "Duration From is required";
                    ucError.Visible = true;
                    return;
                }
                if (txtdurationto.Text == "")
                {
                    ucError.ErrorMessage = "Duration To is required";
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionPSCMOUMatrix.PSCMOUSRPPriorityDurationInsert(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        General.GetNullableInteger(ddlRiskAdd.SelectedValue),
                                                        General.GetNullableInteger(ddlprioritylvlAdd.SelectedValue),
                                                        General.GetNullableGuid(ddlCompany.SelectedValue),
                                                        General.GetNullableInteger(txtdurationfrom.Text),
                                                        General.GetNullableInteger(txtdurationto.Text),
                                                        General.GetNullableString(txtremarks.Text)
                                                        );

                Page.ClientScript.RegisterStartupScript(typeof(Page), "AddressAddNew", scriptRefreshDontClose);
                //DefaultColumnEdit();

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}