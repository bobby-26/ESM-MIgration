using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;

public partial class Inspection_InspectionPSCMOUCompanyPerformanceConfiguration : PhoenixBasePage
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
            ViewState["NOOFCOLUMNS"] = null;
            ViewState["FLDCLASSID"] = null;
            DataSet ds = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            ViewState["NOOFCOLUMNS"] = ds.Tables[0].Rows.Count;

            if (Request.QueryString["classid"] != null)
                ViewState["FLDCLASSID"] = Request.QueryString["classid"].ToString();
            else
                ViewState["FLDCLASSID"] = null;

            if (Request.QueryString["action"] != null)
                ViewState["ACTION"] = Request.QueryString["action"].ToString();
            else
                ViewState["ACTION"] = "";

            BindPSCMOU();
            BindDeficiencyIndex();
            BindDetentionIndex();
            BindPerformanceLevel();

        }        
    }

    protected void BindDeficiencyIndex()
    {
        ddlDefindexAdd.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUIndexList();
        ddlDefindexAdd.DataTextField = "FLDNAME";
        ddlDefindexAdd.DataValueField = "FLDDEFANDDETINDEXID";
        ddlDefindexAdd.DataBind();
        ddlDefindexAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindDetentionIndex()
    {
        ddlDetindexAdd.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUIndexList();
        ddlDetindexAdd.DataTextField = "FLDNAME";
        ddlDetindexAdd.DataValueField = "FLDDEFANDDETINDEXID";
        ddlDetindexAdd.DataBind();
        ddlDetindexAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void BindPSCMOU()
    {
        ddlCompany.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindPerformanceLevel()
    {
        ddlCompanyperfAdd.DataSource = PhoenixInspectionPSCMOUMatrix.ListCompanyPerformanceList();
        ddlCompanyperfAdd.DataTextField = "FLDNAME";
        ddlCompanyperfAdd.DataValueField = "FLDCOMPANYPERFORMANCEID";
        ddlCompanyperfAdd.DataBind();
        ddlCompanyperfAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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

                if (ddlCompany.SelectedValue == "")
                {
                    ucError.ErrorMessage = "Class Society is required";
                    ucError.Visible = true;
                    return;
                }
                if (ddlDefindexAdd.SelectedValue == "")
                {
                    ucError.ErrorMessage = "Class Society is required";
                    ucError.Visible = true;
                    return;
                }
                if (ddlDetindexAdd.SelectedValue == "")
                {
                    ucError.ErrorMessage = "Class Society is required";
                    ucError.Visible = true;
                    return;
                }
                if (ddlCompanyperfAdd.SelectedValue == "")
                {
                    ucError.ErrorMessage = "Class Society is required";
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionPSCMOUMatrix.PSCMOUCompanyPerformanceScoreInsert(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        General.GetNullableInteger(ddlDefindexAdd.SelectedValue),
                                                        General.GetNullableInteger(ddlDetindexAdd.SelectedValue),
                                                        General.GetNullableInteger(ddlCompanyperfAdd.SelectedValue),
                                                        General.GetNullableGuid(ddlCompany.SelectedValue),
                                                        null
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