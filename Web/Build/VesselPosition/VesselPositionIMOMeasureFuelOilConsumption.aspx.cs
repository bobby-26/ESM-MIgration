using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class VesselPositionIMOMeasureFuelOilConsumption : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "PROCEDUREDETAIL",ToolBarDirection.Right);
            MenuProcedureDetailList.AccessRights = this.ViewState;
            MenuProcedureDetailList.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("Save", "SAVE",ToolBarDirection.Right);
            TabProcedure.AccessRights = this.ViewState;
            TabProcedure.MenuList = toolbartab.Show();

            if (!IsPostBack)
            {
                DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(8);
                ddlmethod.DataSource = ds;
                ddlmethod.DataTextField = "FLDNAME";
                ddlmethod.DataValueField = "FLDNAME";
                ddlmethod.DataBind();
                ddlmethod.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ddlmethod.SelectedIndex = 0;
                ProcedureDetailEdit();
            }
            int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + companyid + "', true); ");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEUMRVFuelType_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFOConsumptionLineInsert(General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDocumentNameadd")).Text)
                                                            , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtDocumentIdadd")).Text)
                                                            , int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                            , int.Parse(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
                                                            , General.GetNullableString("MFC"));

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFOConsumptionLineDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(((RadLabel)e.Item.FindControl("lbluniqueid")).Text));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   protected void gvEUMRVFuelType_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            int company = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            LinkButton btnShowDocumentsadd = (LinkButton)e.Item.FindControl("btnShowDocumentsadd");
            if (btnShowDocumentsadd != null)
            {
                btnShowDocumentsadd.Attributes.Add("onclick", "return showPickList('spnPickListDocumentadd', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + company + "', true); ");
            }
        }
    }
    protected void TabProcedure_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixVesselPositionEUMRV.InsertIMODCSProcedureDetail(General.GetNullableString(txtDocumentName.Text.Trim())
                                                                    , General.GetNullableString(HttpUtility.HtmlDecode(txteuprocedure.Content.Trim()))
                                                                    , General.GetNullableGuid(txtDocumentId.Text)
                                                                    , int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                    , int.Parse(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
                                                                    , General.GetNullableString(ddlmethod.SelectedValue));
                ucStatus.Text = "Information saved successfully.";
                ProcedureDetailEdit();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ProcedureDetailEdit()
    {
       DataSet ds = PhoenixVesselPositionEUMRVConfig.IMODCSProcedureConfigDetailEdit(int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
       DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            txteuprocedure.Content = dt.Rows[0]["FLDDESCRIPTION"].ToString();
            txtDocumentName.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
            ddlmethod.SelectedValue = dt.Rows[0]["FLDMETHOD"].ToString();
        }
    }
    protected void txtDocumentName_OnTextChanged(object sender, EventArgs e)
    {

    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            Response.Redirect("../VesselPosition/VesselPositionIMODCSConfiguration.aspx?Lanchfrom=0");
        }
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        //PhoenixVesselPositionEUMRV.InsertIMODCSProcedureDetail( General.GetNullableString(txtDocumentName.Text.Trim())
        //                                                            , General.GetNullableString(txteuprocedure.Content.Trim())                                                                        
        //                                                            , General.GetNullableGuid(txtDocumentId.Text)
        //                                                            , int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
        //                                                            , int.Parse(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
        //                                                            , General.GetNullableString(ddlmethod.SelectedValue));
        //ProcedureDetailEdit();
    }

    protected void gvEUMRVFuelType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("MFC"));
            gvEUMRVFuelType.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvEUMRVFuelType.SelectedIndexes.Clear();
        gvEUMRVFuelType.EditIndexes.Clear();
        gvEUMRVFuelType.DataSource = null;
        gvEUMRVFuelType.Rebind();
    }
}
