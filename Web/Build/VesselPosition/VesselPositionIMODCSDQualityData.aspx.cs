using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionIMODCSDQualityData : PhoenixBasePage
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
            toolbarmain.AddButton("Back", "BACK",ToolBarDirection.Right);
            MenuProcedureDetailList.AccessRights = this.ViewState;
            MenuProcedureDetailList.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("Save", "SAVE", ToolBarDirection.Right);
            TabProcedure.AccessRights = this.ViewState;
            TabProcedure.MenuList = toolbartab.Show();

            if (!IsPostBack)
            {
                ViewState["DATAQUALITYID"] = "";
                if ((Request.QueryString["code"] != "") && (Request.QueryString["code"] != null))
                {
                    ViewState["CODE"] = Request.QueryString["code"].ToString();
                }
                ProcedureDetailEdit();                
            }
            int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            btnShowDocuments1.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + companyid + "', true); ");
            btnShowDocuments2.Attributes.Add("onclick", "return showPickList('spnPickListDocument1', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + companyid + "', true); ");
            btnShowDocuments3.Attributes.Add("onclick", "return showPickList('spnPickListDocument2', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + companyid + "', true); ");
            btnShowDocuments4.Attributes.Add("onclick", "return showPickList('spnPickListDocument3', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + companyid + "', true); ");
            btnShowDocuments5.Attributes.Add("onclick", "return showPickList('spnPickListDocument4', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + companyid + "', true); ");
            btnShowDocuments6.Attributes.Add("onclick", "return showPickList('spnPickListDocument5', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + companyid + "', true); ");
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
                                                            , General.GetNullableString("FC"));

                Rebind1();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFOConsumptionLineDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(((RadLabel)e.Item.FindControl("lbluniqueid")).Text));
                Rebind1();
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
                btnShowDocumentsadd.Attributes.Add("onclick", "return showPickList('spnPickListDocumentadd1', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + company + "', true); ");
            }
        }
    }
    protected void gvEUMRVFuelType2_RowCommand(object sender, GridCommandEventArgs e)
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
                                                            , General.GetNullableString("DT"));

                Rebind2();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFOConsumptionLineDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(((RadLabel)e.Item.FindControl("lbluniqueid")).Text));
                Rebind2();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEUMRVFuelType2_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            int company = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            LinkButton btnShowDocumentsadd = (LinkButton)e.Item.FindControl("btnShowDocumentsadd");
            if (btnShowDocumentsadd != null)
            {
                btnShowDocumentsadd.Attributes.Add("onclick", "return showPickList('spnPickListDocumentadd2', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + company + "', true); ");
            }
        }
    }
    protected void gvEUMRVFuelType3_RowCommand(object sender, GridCommandEventArgs e)
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
                                                            , General.GetNullableString("HU"));

                Rebind3();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFOConsumptionLineDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(((RadLabel)e.Item.FindControl("lbluniqueid")).Text));
                Rebind3();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEUMRVFuelType3_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            int company = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            LinkButton btnShowDocumentsadd = (LinkButton)e.Item.FindControl("btnShowDocumentsadd");
            if (btnShowDocumentsadd != null)
            {
                btnShowDocumentsadd.Attributes.Add("onclick", "return showPickList('spnPickListDocumentadd3', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + company + "', true); ");
            }
        }
    }
    protected void gvEUMRVFuelType4_RowCommand(object sender, GridCommandEventArgs e)
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
                                                            , General.GetNullableString("ADC"));

                Rebind4();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFOConsumptionLineDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(((RadLabel)e.Item.FindControl("lbluniqueid")).Text));
                Rebind4();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEUMRVFuelType4_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            int company = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            LinkButton btnShowDocumentsadd = (LinkButton)e.Item.FindControl("btnShowDocumentsadd");
            if (btnShowDocumentsadd != null)
            {
                btnShowDocumentsadd.Attributes.Add("onclick", "return showPickList('spnPickListDocumentadd4', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + company + "', true); ");
            }
        }
    }
    protected void gvEUMRVFuelType5_RowCommand(object sender, GridCommandEventArgs e)
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
                                                            , General.GetNullableString("IT"));

                Rebind5();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFOConsumptionLineDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(((RadLabel)e.Item.FindControl("lbluniqueid")).Text));
                Rebind5();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEUMRVFuelType5_ItemDataBound(Object sender,GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            int company = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            LinkButton btnShowDocumentsadd = (LinkButton)e.Item.FindControl("btnShowDocumentsadd");
            if (btnShowDocumentsadd != null)
            {
                btnShowDocumentsadd.Attributes.Add("onclick", "return showPickList('spnPickListDocumentadd5', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + company + "', true); ");
            }
        }
    }
    protected void gvEUMRVFuelType6_RowCommand(object sender, GridCommandEventArgs e)
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
                                                            , General.GetNullableString("IRV"));

                Rebind6();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionIMODCSFuelConsumption.IMODCSFOConsumptionLineDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(((RadLabel)e.Item.FindControl("lbluniqueid")).Text));
                Rebind6();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEUMRVFuelType6_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            int company = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            LinkButton btnShowDocumentsadd = (LinkButton)e.Item.FindControl("btnShowDocumentsadd");
            if (btnShowDocumentsadd != null)
            {
                btnShowDocumentsadd.Attributes.Add("onclick", "return showPickList('spnPickListDocumentadd6', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + company + "', true); ");
            }
        }
    }

    protected void gvEUMRVFuelType6_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        Rebind6();
    }

    protected void TabProcedure_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixVesselPositionEUMRV.InsertIMODCSDMSQualityData( General.GetNullableGuid(ViewState["DATAQUALITYID"].ToString())
                                                                        , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
                                                                        , General.GetNullableString(txteuprocedure1.Content.Trim())
                                                                        , General.GetNullableGuid(txtDocumentId1.Text)
                                                                        , General.GetNullableString(txteuprocedure2.Content.Trim())
                                                                        , General.GetNullableGuid(txtDocumentId2.Text)
                                                                        , General.GetNullableString(txteuprocedure3.Content.Trim())
                                                                        , General.GetNullableGuid(txtDocumentId3.Text)
                                                                        , General.GetNullableString(txteuprocedure4.Content.Trim())
                                                                        , General.GetNullableGuid(txtDocumentId4.Text)
                                                                        , General.GetNullableString(txteuprocedure5.Content.Trim())
                                                                        , General.GetNullableGuid(txtDocumentId5.Text)
                                                                        , General.GetNullableString(txteuprocedure6.Content.Trim())
                                                                        , General.GetNullableGuid(txtDocumentId6.Text)
                                                                        , General.GetNullableString(txtDocumentName1.Text.Trim())
                                                                        , General.GetNullableString(txtDocumentName2.Text.Trim())
                                                                        , General.GetNullableString(txtDocumentName3.Text.Trim())
                                                                        , General.GetNullableString(txtDocumentName4.Text.Trim())
                                                                        , General.GetNullableString(txtDocumentName5.Text.Trim())
                                                                        , General.GetNullableString(txtDocumentName6.Text.Trim()));
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
        DataSet ds = PhoenixVesselPositionEUMRVConfig.IMODCSDMSQualityDataEdit();
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            ViewState["DATAQUALITYID"] = dt.Rows[0]["FLDIMODCSDATAQUALITYID"].ToString();
            txteuprocedure1.Content = dt.Rows[0]["FLDFUELCONSDMSDESCRIPTION"].ToString();
            txtDocumentName1.Text = dt.Rows[0]["FLDFUELCONSNAME"].ToString();
            txtDocumentId1.Text = dt.Rows[0]["FLDFUELCONSDMSREFRENCEID"].ToString();

            txteuprocedure2.Content = dt.Rows[0]["FLDDISTANCETRAVELLEDDESCRIPTION"].ToString();
            txtDocumentName2.Text = dt.Rows[0]["FLDDISTANCETRAVELLEDNAME"].ToString();
            txtDocumentId2.Text = dt.Rows[0]["FLDDISTANCETRAVELLEDDMSREFRENCEID"].ToString();

            txteuprocedure3.Content = dt.Rows[0]["FLDHOURSUNDERWAYDMSDESCRIPTION"].ToString();
            txtDocumentName3.Text = dt.Rows[0]["FLDHOURSUNDERWAYNAME"].ToString();
            txtDocumentId3.Text = dt.Rows[0]["FLDHOURSUNDERWAYDMSREFRENCEID"].ToString();

            txteuprocedure4.Content = dt.Rows[0]["FLDADEQUECYDATADESCRIPTION"].ToString();
            txtDocumentName4.Text = dt.Rows[0]["FLDADEQUECYDATANAME"].ToString();
            txtDocumentId4.Text = dt.Rows[0]["FLDADEQUECYDATADMSREFRENCEID"].ToString();

            txteuprocedure5.Content = dt.Rows[0]["FLDITDMSDESCRIPTION"].ToString();
            txtDocumentName5.Text = dt.Rows[0]["FLDITNAME"].ToString();
            txtDocumentId5.Text = dt.Rows[0]["FLDITDMSREFRENCEID"].ToString();

            txteuprocedure6.Content = dt.Rows[0]["FLDINTERNALREVIEWSDMSDESCRIPTION"].ToString();
            txtDocumentName6.Text = dt.Rows[0]["FLDINTERNALREVIEWNAME"].ToString();
            txtDocumentId6.Text = dt.Rows[0]["FLDINTERNALREVIEWSDMSREFRENCEID"].ToString();
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
        //PhoenixVesselPositionEUMRV.InsertIMODCSDMSQualityData(General.GetNullableGuid(ViewState["DATAQUALITYID"].ToString())
        //                                                                , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
        //                                                                , General.GetNullableString(txteuprocedure1.Content.Trim())
        //                                                                , General.GetNullableGuid(txtDocumentId1.Text)
        //                                                                , General.GetNullableString(txteuprocedure2.Content.Trim())
        //                                                                , General.GetNullableGuid(txtDocumentId2.Text)
        //                                                                , General.GetNullableString(txteuprocedure3.Content.Trim())
        //                                                                , General.GetNullableGuid(txtDocumentId3.Text)
        //                                                                , General.GetNullableString(txteuprocedure4.Content.Trim())
        //                                                                , General.GetNullableGuid(txtDocumentId4.Text)
        //                                                                , General.GetNullableString(txteuprocedure5.Content.Trim())
        //                                                                , General.GetNullableGuid(txtDocumentId5.Text)
        //                                                                , General.GetNullableString(txteuprocedure6.Content.Trim())
        //                                                                , General.GetNullableGuid(txtDocumentId6.Text)
        //                                                                , General.GetNullableString(txtDocumentName1.Text.Trim())
        //                                                                , General.GetNullableString(txtDocumentName2.Text.Trim())
        //                                                                , General.GetNullableString(txtDocumentName3.Text.Trim())
        //                                                                , General.GetNullableString(txtDocumentName4.Text.Trim())
        //                                                                , General.GetNullableString(txtDocumentName5.Text.Trim())
        //                                                                , General.GetNullableString(txtDocumentName6.Text.Trim()));
        //ProcedureDetailEdit();
    }

    protected void gvEUMRVFuelType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("FC"));

            gvEUMRVFuelType.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind1()
    {
        gvEUMRVFuelType.SelectedIndexes.Clear();
        gvEUMRVFuelType.EditIndexes.Clear();
        gvEUMRVFuelType.DataSource = null;
        gvEUMRVFuelType.Rebind();
    }

    protected void gvEUMRVFuelType2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("DT"));

            gvEUMRVFuelType2.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind2()
    {
        gvEUMRVFuelType2.SelectedIndexes.Clear();
        gvEUMRVFuelType2.EditIndexes.Clear();
        gvEUMRVFuelType2.DataSource = null;
        gvEUMRVFuelType2.Rebind();
    }
    protected void gvEUMRVFuelType3_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("HU"));

            gvEUMRVFuelType3.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind3()
    {
        gvEUMRVFuelType3.SelectedIndexes.Clear();
        gvEUMRVFuelType3.EditIndexes.Clear();
        gvEUMRVFuelType3.DataSource = null;
        gvEUMRVFuelType3.Rebind();
    }
    protected void gvEUMRVFuelType4_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("ADC"));

            gvEUMRVFuelType4.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind4()
    {
        gvEUMRVFuelType4.SelectedIndexes.Clear();
        gvEUMRVFuelType4.EditIndexes.Clear();
        gvEUMRVFuelType4.DataSource = null;
        gvEUMRVFuelType4.Rebind();
    }
    protected void gvEUMRVFuelType5_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("IT"));

            gvEUMRVFuelType5.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind5()
    {
        gvEUMRVFuelType5.SelectedIndexes.Clear();
        gvEUMRVFuelType5.EditIndexes.Clear();
        gvEUMRVFuelType5.DataSource = null;
        gvEUMRVFuelType5.Rebind();
    }
    protected void gvEUMRVFuelType6_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixVesselPositionIMODCSFuelConsumption.ListIMODCSFOConsumptionLine(General.GetNullableString("IRV"));

            gvEUMRVFuelType6.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind6()
    {
        gvEUMRVFuelType6.SelectedIndexes.Clear();
        gvEUMRVFuelType6.EditIndexes.Clear();
        gvEUMRVFuelType6.DataSource = null;
        gvEUMRVFuelType6.Rebind();
    }
}
