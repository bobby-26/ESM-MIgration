using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Inspection_InspectionPSCCopyMOURegisterData : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Copy", "SAVE", ToolBarDirection.Right);       
        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["type"] != null)
                ViewState["TYPE"] = Request.QueryString["type"].ToString();
            else
                ViewState["TYPE"] = "";

            BindPSCMOU();
        }
    }

    protected void BindPSCMOU()
    {
        ddlCompanyfrm.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        ddlCompanyfrm.DataTextField = "FLDCOMPANYNAME";
        ddlCompanyfrm.DataValueField = "FLDCOMPANYID";
        ddlCompanyfrm.DataBind();
        ddlCompanyfrm.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        ddlCompanyto.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        ddlCompanyto.DataTextField = "FLDCOMPANYNAME";
        ddlCompanyto.DataValueField = "FLDCOMPANYID";
        ddlCompanyto.DataBind();
        ddlCompanyto.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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

                if (ddlCompanyfrm.SelectedValue == "")
                {
                    ucError.ErrorMessage = "MOU from is required";
                    ucError.Visible = true;
                    return;
                }
                if (ddlCompanyto.SelectedValue == "")
                {
                    ucError.ErrorMessage = "MOU to is required";
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionPSCMOUMatrix.PSCMOUCopyData(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        General.GetNullableGuid(ddlCompanyfrm.SelectedValue),
                                                        General.GetNullableGuid(ddlCompanyto.SelectedValue),
                                                        ViewState["TYPE"].ToString()
                                                        );

                Page.ClientScript.RegisterStartupScript(typeof(Page), "AddressAddNew", scriptRefreshDontClose);

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}