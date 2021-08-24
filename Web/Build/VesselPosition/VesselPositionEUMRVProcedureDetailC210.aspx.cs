using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionEUMRVProcedureDetailC210 : PhoenixBasePage
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
            if (Request.QueryString["Lanchfrom"].ToString() == "0")
                MenuProcedureDetailList.Title = "Company Procedure Edit";
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                MenuProcedureDetailList.Title = "Ship Specific Procedure Edit";
            if (Request.QueryString["Lanchfrom"].ToString() == "0")
                toolbarmain.AddButton("EUMRV Procedure", "PROCEDUREDETAIL",ToolBarDirection.Right);
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                toolbarmain.AddButton("Ship Specific Procedure", "PROCEDUREDETAIL", ToolBarDirection.Right);
            MenuProcedureDetailList.AccessRights = this.ViewState;
            MenuProcedureDetailList.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("Save", "SAVE",ToolBarDirection.Right);
            TabProcedure.AccessRights = this.ViewState;
            TabProcedure.MenuList = toolbartab.Show();

            if (!IsPostBack)
            {
                BindProcedureDropDown();
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
    private void BindProcedureDropDown()
    {
        DataSet ds = PhoenixVesselPositionEUMRV.ListEUMRVProcedure(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

        ddlProcedure.DataSource = ds;
        ddlProcedure.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlProcedure.DataTextField = "FLDPROCEDURE";
        ddlProcedure.DataValueField = "FLDEUMRVPROCEDUERID";
        ddlProcedure.DataBind();
        ddlProcedure.SelectedIndex = -1;
    }
    protected void TabProcedure_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                    PhoenixVesselPositionEUMRV.InsertEUMRVProcedureDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                            General.GetNullableGuid(Request.QueryString["ProcedureID"].ToString()),
                                                                            General.GetNullableString(txtReferencetoExisting.Text.Trim()),
                                                                            General.GetNullableInteger(txtVersion.Text.Trim()),
                                                                            General.GetNullableString(txteuprocedure.Text.Trim()),
                                                                            General.GetNullableString(txtxpersonreponsible.Text.Trim()),
                                                                            General.GetNullableString(txtlocation.Text.Trim()),
                                                                            General.GetNullableString(txtSystemUsed.Text.Trim()),
                                                                            "NEW", General.GetNullableGuid(txtDocumentId.Text),
                                                                            General.GetNullableString(txtFormulae.Text.Trim()),
                                                                            null,
                                                                            null, null, null, null);

                ucStatus.Text = "Procedure saved successfully.";
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
        if (Request.QueryString["Table"] != null && General.GetNullableString(Request.QueryString["Table"].ToString()) != null)
        {
            DataSet ds = PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigDetailEdit(Request.QueryString["Table"].ToString());
            DataTable dt = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            if (dt.Rows.Count > 0)
            {
                ddlProcedure.Enabled = false;
                ddlProcedure.CssClass = "readonly";
                txtVersion.Text = dt.Rows[0]["FLDPROCEDUREVERSION"].ToString();
                txteuprocedure.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
                txtxpersonreponsible.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
                txtlocation.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
                txtSystemUsed.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();
                ViewState["PROCEDUREID"] = dt.Rows[0]["FLDPROCEDUREID"].ToString();
                //txtDocumentNameEdit.InnerText = dt.Rows[0]["FLDFORMNAME"].ToString();
                txtDocumentName.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
                txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
                txtFormulae.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
                //txtDatasource.Text = dt.Rows[0]["FLDDATASOURCE"].ToString();
                string Guidance = General.GetNullableString(dt.Rows[0]["FLDGUIDANCE"].ToString()) != null ? " (" + dt.Rows[0]["FLDGUIDANCE"].ToString() + ")" : "";
                lblProceduretxt.Text = dt.Rows[0]["FLDNEWCODE"].ToString() + "-" + dt.Rows[0]["FLDPROCEDURE"].ToString() + Guidance;
               
            }
            else 
            {
                txteuprocedure.Text = "";
                txtxpersonreponsible.Text = "";
                txtlocation.Text = "";
                txtSystemUsed.Text = "";
                ViewState["PROCEDUREID"] = "";
                //txtDocumentNameEdit.InnerText = "";
                txtDocumentName.Text = "";
                txtDocumentId.Text = "";
                txtFormulae.Text = "";

                if (dt2.Rows.Count > 0)
                {
                    txtVersion.Text = "0";
                    ViewState["PROCEDUREID"] = dt2.Rows[0]["FLDEUMRVPROCEDUREID"].ToString();
                    lblProceduretxt.Text = dt2.Rows[0]["FLDNEWCODE"].ToString() + "-" + dt2.Rows[0]["FLDPROCEDURE"].ToString();
                }
            }

            ddlProcedure.CssClass = "hidden";

            if(dt2.Rows.Count>0)
            {

                if (dt2.Rows[0]["FLDAPPLICABLEYN"].ToString() == "0")
                {
                    chkapplicableYN.Checked = true;
                    TabProcedure.Visible = false;
                }
                else
                {
                    chkapplicableYN.Checked = false;
                    TabProcedure.Visible = true;
                }
                    
                        
                if (dt2.Rows[0]["FLDOPTIONALYN"].ToString() == "0")
                {
                    lblapplicableyn.Visible = false;
                    chkapplicableYN.Visible = false;
                }
            }
            
        }
    }
    protected void txtDocumentName_OnTextChanged(object sender, EventArgs e)
    { 
        
    }
    protected void chkapplicableYN_CheckedChanged(object sender,EventArgs e)
    {
        if (chkapplicableYN.Checked)
        {
            TabProcedure.Visible = false;
            PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigUpdate(new Guid(Request.QueryString["ProcedureID"].ToString()), 0);
            ProcedureDetailEdit();
        }
        else
        {
            TabProcedure.Visible = true;
            PhoenixVesselPositionEUMRVConfig.EUMRVProcedureConfigUpdate(new Guid(Request.QueryString["ProcedureID"].ToString()), 1);
            ProcedureDetailEdit();
        }
            
    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                Response.Redirect("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx?Lanchfrom=1");
            else
                Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedure.aspx?Lanchfrom=0");
        }   
    }
}
