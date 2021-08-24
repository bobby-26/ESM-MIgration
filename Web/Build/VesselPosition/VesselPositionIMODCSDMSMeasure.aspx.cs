using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;
using System.Web;

public partial class VesselPositionIMODCSDMSMeasure : PhoenixBasePage
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

            

            if (!IsPostBack)
            {
                ViewState["CODE"] = "";
                if((Request.QueryString["code"] != "") && (Request.QueryString["code"] != null))
                {
                    ViewState["CODE"] = Request.QueryString["code"].ToString();
                }
                ProcedureDetailEdit();
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuProcedureDetailList.AccessRights = this.ViewState;
            MenuProcedureDetailList.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("Save", "SAVE", ToolBarDirection.Right);
            TabProcedure.AccessRights = this.ViewState;
            TabProcedure.MenuList = toolbartab.Show();

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
                                                            , General.GetNullableString(ViewState["CODE"].ToString()));

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
                PhoenixVesselPositionEUMRV.InsertIMODCSDMSProcedureDetail(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
                                                                        , General.GetNullableString(txtDocumentName.Text.Trim())
                                                                        , General.GetNullableString(HttpUtility.HtmlDecode(txteuprocedure.Content.Trim()))
                                                                        , General.GetNullableGuid(txtDocumentId.Text)
                                                                        , General.GetNullableString(ViewState["CODE"].ToString()));
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
        DataSet ds = PhoenixVesselPositionEUMRVConfig.IMODCSDMSProcedureConfigDetailEdit(General.GetNullableString(ViewState["CODE"].ToString()));
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            txteuprocedure.Content = dt.Rows[0]["FLDDESCRIPTION"].ToString();
            txtDocumentName.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
            txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();            
        }

        DataTable dt1 = ds.Tables[1];
        if (dt1.Rows.Count > 0)
        {
            MenuProcedureDetailList.Title = dt1.Rows[0]["FLDTITLE"].ToString();
        }
    }
    protected void txtDocumentName_OnTextChanged(object sender, EventArgs e)
    {

    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../VesselPosition/VesselPositionIMODCSConfiguration.aspx");
        }
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        //PhoenixVesselPositionEUMRV.InsertIMODCSDMSProcedureDetail(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
        //                                                                , General.GetNullableString(txtDocumentName.Text.Trim())
        //                                                                , General.GetNullableString(txteuprocedure.Content.Trim())
        //                                                                , General.GetNullableGuid(txtDocumentId.Text)
        //                                                                , General.GetNullableString(ViewState["CODE"].ToString()));

        //ProcedureDetailEdit();
    }

    protected void gvEUMRVFuelType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString(ViewState["CODE"].ToString()));

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
