using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;

public partial class VesselAccounts_VesselAccountsComponentTaxTypeMapAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuComponentTaxTypeMap.AccessRights = this.ViewState;
            MenuComponentTaxTypeMap.MenuList = toolbar.Show();


            if (!IsPostBack)
            {

                if (Request.QueryString["Id"] != null)
                {
                    ViewState["Id"] = Request.QueryString["Id"].ToString();
                    EditComponentTaxType(new Guid(Request.QueryString["Id"].ToString()));
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ddlUnion.AddressList = PhoenixRegistersAddress.ListAddress("134");
                lblUnion.Visible = true;
                ddlUnion.Visible = true;
                lblCBARevision.Visible = true;
                ddlRevision.Visible = true;
                lblWageComponents.Visible = false;
                ucWageComponents.Visible = false;


                //BindComponent();
                BindComponentType();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    //protected void BindComponent()
    //{
    //    ddlComponent.DataSource = PhoenixVesselAccountsComponentTaxTypeMap.ListCBAcomponent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
    //    ddlComponent.DataTextField = "FLDCOMPONENTNAME";
    //    ddlComponent.DataValueField = "FLDCOMPONENTID";
    //    ddlComponent.DataBind();
    //    ddlComponent.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    //}

    protected void BindComponentType()
    {
        ddlComponentType.DataSource = PhoenixVesselAccountsComponentTaxTypeMap.Listcomponenttaxtype(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
        ddlComponentType.DataTextField = "FLDNAME";
        ddlComponentType.DataValueField = "FLDID";
        ddlComponentType.DataBind();
        ddlComponentType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    private void EditComponentTaxType(Guid Id)
    {
        try
        {
            DataSet ds = PhoenixVesselAccountsComponentTaxTypeMap.EditComponentTaxType(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Id);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ddlType.SelectedValue = dr["FLDTYPEID"].ToString();
                ddlCountry.SelectedCountry = dr["FLDCOUNTRY"].ToString();
                ucWageComponents.SelectedHard = dr["FLDUNIONORSTANDARD"].ToString();
                ddlUnion.SelectedAddress = dr["FLDUNIONORSTANDARD"].ToString();
                ddlRevision.SelectedValue = dr["FLDREVISIONID"].ToString();
                ddlComponent.SelectedValue = dr["FLDCOMPONENTID"].ToString();
                ddlComponentType.SelectedValue = dr["FLDCOMPONENTTYPE"].ToString();
                //ddlComponentType.SelectedValue = dr["FLDCOMPONENTTYPEID"].ToString();
                chkActiveYN.Checked = dr["FLDISACTIVE"].ToString() == "1" ? true : false;

                //ViewState["ID"] = dr["ID"].ToString();

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Id"] = null;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComponentTaxTypeMap_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='ComponentTaxTypeMap'>" + "\n";
            scriptClosePopup += "fnReloadList('City');";
            scriptClosePopup += "</script>" + "\n";

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='ComponentTaxTypeMapnew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('OfficeStaff', null, 'yes');";
            scriptRefreshDontClose += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {


                if (ViewState["Id"] != null)
                {
                    if (ddlType.SelectedValue == "1")
                    {
                        if (!IsValidComponentTaxType())
                        {

                            ucError.Visible = true;
                            return;
                        }


                        PhoenixVesselAccountsComponentTaxTypeMap.UpdateComponentTaxTypeMap(PhoenixSecurityContext.CurrentSecurityContext.UserCode, chkActiveYN.Checked.Equals(true) ? 1 : 0, new Guid(ViewState["Id"].ToString()));
                    }
                    if (ddlType.SelectedValue == "2")
                    {
                        if (!IsValidComponentTaxTypeWage())
                        {

                            ucError.Visible = true;
                            return;
                        }

                        PhoenixVesselAccountsComponentTaxTypeMap.UpdateComponentTaxTypeMap(PhoenixSecurityContext.CurrentSecurityContext.UserCode, chkActiveYN.Checked.Equals(true) ? 1 : 0, new Guid(ViewState["Id"].ToString()));
                    }
                    if (ddlType.SelectedValue == "3")
                    {
                        if (!IsValidComponentTaxTypeCrew())
                        {

                            ucError.Visible = true;
                            return;
                        }

                        PhoenixVesselAccountsComponentTaxTypeMap.UpdateComponentTaxTypeMap(PhoenixSecurityContext.CurrentSecurityContext.UserCode, chkActiveYN.Checked.Equals(true) ? 1 : 0, new Guid(ViewState["Id"].ToString()));
                    }
                
            
                ucStatus.Text = "Information Updated";
            }
            else
            {
                    if (ddlType.SelectedValue == "1")
                    {
                        if (!IsValidComponentTaxType())
                        {

                            ucError.Visible = true;
                            return;
                        }


                        PhoenixVesselAccountsComponentTaxTypeMap.InsertComponentTaxTypeMap(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ddlUnion.SelectedAddress), General.GetNullableGuid(ddlRevision.SelectedValue),
                                                                                       Convert.ToInt32(ddlCountry.SelectedCountry), new Guid(ddlComponent.SelectedValue), int.Parse(ddlComponentType.SelectedValue),
                                                                                       int.Parse(ddlType.SelectedValue), chkActiveYN.Checked.Equals(true) ? 1 : 0);

                        //PhoenixVesselAccountsComponentTaxTypeMap.InsertComponentTaxTypeMap(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ddlUnion.SelectedAddress), General.GetNullableGuid(ddlRevision.SelectedValue),
                        //                                                               Convert.ToInt32(ddlCountry.SelectedCountry), new Guid(ddlComponent.SelectedValue), int.Parse(ddlType.SelectedValue),
                        //                                                               int.Parse(ddlComponentType.SelectedValue), chkActiveYN.Checked.Equals(true) ? 1 : 0);
                    }
                    if (ddlType.SelectedValue == "2")
                    {
                        if (!IsValidComponentTaxTypeWage())
                        {

                            ucError.Visible = true;
                            return;
                        }


                        PhoenixVesselAccountsComponentTaxTypeMap.InsertComponentTaxTypeMap(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ucWageComponents.SelectedHard), null,
                                                                                       Convert.ToInt32(ddlCountry.SelectedCountry), new Guid(ddlComponent.SelectedValue), int.Parse(ddlComponentType.SelectedValue), int.Parse(ddlType.SelectedValue), chkActiveYN.Checked.Equals(true) ? 1 : 0);
                    }
                    if (ddlType.SelectedValue == "3")
                    {
                           

                        if (!IsValidComponentTaxTypeCrew())
                        {

                            ucError.Visible = true;
                            return;
                        }


                        PhoenixVesselAccountsComponentTaxTypeMap.InsertComponentTaxTypeMap(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 0, null,
                                                                                       Convert.ToInt32(ddlCountry.SelectedCountry), new Guid(ddlComponent.SelectedValue), int.Parse(ddlComponentType.SelectedValue), int.Parse(ddlType.SelectedValue), chkActiveYN.Checked.Equals(true) ? 1 : 0);
                    }

                    ucStatus.Text = "Information Added";

            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "ComponentTaxTypeMap", scriptClosePopup);

        }
            if (CommandName.ToUpper().Equals("NEW"))
            Reset();
    }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidComponentTaxType()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ddlComponent.SelectedValue) == null)
            ucError.ErrorMessage = "Component is required";

        if (General.GetNullableInteger(ddlComponentType.SelectedValue) == null)
            ucError.ErrorMessage = "Component Type is required";

        if (General.GetNullableInteger(ddlUnion.SelectedAddress) == null)
            ucError.ErrorMessage = "Union is required";

        if (General.GetNullableInteger(ddlCountry.SelectedCountry) == null)
            ucError.ErrorMessage = "Country is required";

        if (General.GetNullableInteger(ddlType.SelectedValue) == null)
            ucError.ErrorMessage = "Type is required";

   
        return (!ucError.IsError);
    }

    public bool IsValidComponentTaxTypeWage()
    {
        ucError.HeaderMessage = "Please provide the following required information";

     

        if (General.GetNullableGuid(ddlComponent.SelectedValue) == null)
            ucError.ErrorMessage = "Component is required";

        if (General.GetNullableInteger(ddlComponentType.SelectedValue) == null)
            ucError.ErrorMessage = "Component Type is required";

        if (General.GetNullableInteger(ucWageComponents.SelectedHard) == null)
            ucError.ErrorMessage = "Wage Components is required";

        if (General.GetNullableInteger(ddlCountry.SelectedCountry) == null)
            ucError.ErrorMessage = "Country is required";

        if (General.GetNullableInteger(ddlType.SelectedValue) == null)
            ucError.ErrorMessage = "Type is required";


        return (!ucError.IsError);
    }

    public bool IsValidComponentTaxTypeCrew()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ddlComponent.SelectedValue) == null)
            ucError.ErrorMessage = "Component is required";

        if (General.GetNullableInteger(ddlComponentType.SelectedValue) == null)
            ucError.ErrorMessage = "Component Type is required";

        if (General.GetNullableInteger(ddlCountry.SelectedCountry) == null)
            ucError.ErrorMessage = "Country is required";

        if (General.GetNullableInteger(ddlType.SelectedValue) == null)
            ucError.ErrorMessage = "Type is required";


        return (!ucError.IsError);
    }

    private void FetchCBARevision()
    {
        DataTable dt = PhoenixRegistersContract.FetchCBARevision(General.GetNullableInteger(ddlUnion.SelectedAddress));
        ddlRevision.DataSource = dt;
        ddlRevision.DataBind();
        if (dt.Rows.Count > 0) ddlRevision.SelectedIndex = 1;
    }

    private void FetchCBAComponentRevision()
    {
        DataTable dt = PhoenixVesselAccountsComponentTaxTypeMap.FetchCBARevisionComponent(General.GetNullableInteger(ddlUnion.SelectedAddress),General.GetNullableGuid(ddlRevision.SelectedValue));
        ddlComponent.DataSource = dt;
        ddlComponent.DataBind();
        if (dt.Rows.Count > 0) ddlComponent.SelectedIndex = 1;

    }

    private void FetchWageComponent()
    {
        DataTable dt = PhoenixVesselAccountsComponentTaxTypeMap.FetchWageComponent(General.GetNullableInteger(ucWageComponents.SelectedHard));
        ddlComponent.DataSource = dt;
        ddlComponent.DataBind();
        if (dt.Rows.Count > 0) ddlComponent.SelectedIndex = 1;
    }

    private void FetchComponentagreedwithcrew()
    {
        DataTable dt = PhoenixRegistersContract.ListContractCrew(null);
        ddlComponent.DataSource = dt;
        ddlComponent.DataBind();
        if (dt.Rows.Count > 0) ddlComponent.SelectedIndex = 1;
    }

 
    protected void ucWageComponents_Changed(object sender, EventArgs e)
    {
        FetchWageComponent();
        ddlComponent.ClearSelection();
        //ViewState["COMPONENTID"] = string.Empty;
        // Rebind();
    }
    protected void ddlUnion_TextChangedEvent(object sender, EventArgs e)
    {
        FetchCBARevision();
    }
    protected void ddlType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlType.SelectedValue == "1")
        {
            lblUnion.Visible = true;
            ddlUnion.Visible = true;
            lblCBARevision.Visible = true;
            ddlRevision.Visible = true;
            lblWageComponents.Visible = false;
            ucWageComponents.Visible = false;
        }
        if (ddlType.SelectedValue == "2")
        {
            lblUnion.Visible = false;
            ddlUnion.Visible = false;
            lblCBARevision.Visible = false;
            ddlRevision.Visible = false;
            lblWageComponents.Visible = true;
            ucWageComponents.Visible = true;
        }
        if (ddlType.SelectedValue == "3")
        {
            lblUnion.Visible = false;
            ddlUnion.Visible = false;
            lblCBARevision.Visible = false;
            ddlRevision.Visible = false;
            lblWageComponents.Visible = false;
            ucWageComponents.Visible = false;
        }
    }
    private void Reset()
    {
        ViewState["FLDID"] = null;

    }

    protected void ddlRevision_TextChangedEvent(object sender, EventArgs e)
    {
        FetchCBAComponentRevision();
        ddlComponent.ClearSelection();
    }

    protected void ddlType_TextChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "3")
        {
            FetchComponentagreedwithcrew();
            ddlComponent.ClearSelection();
        }
    }
}