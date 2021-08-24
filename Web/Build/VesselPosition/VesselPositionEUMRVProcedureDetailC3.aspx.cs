using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionEUMRVProcedureDetailC3 : PhoenixBasePage
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
                                                                            null,
                                                                            General.GetNullableString(txtDatasource.Text.Trim()),
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
               // txtDocumentNameEdit.InnerText = dt.Rows[0]["FLDFORMNAME"].ToString();
                txtDocumentName.Text = dt.Rows[0]["FLDFORMACTUALNAME"].ToString();
                txtDocumentId.Text = dt.Rows[0]["FLDDOCUMENTID"].ToString();
                txtDatasource.Text = dt.Rows[0]["FLDDATASOURCE"].ToString();
                string Guidance = General.GetNullableString(dt.Rows[0]["FLDGUIDANCE"].ToString()) != null ? " (" + dt.Rows[0]["FLDGUIDANCE"].ToString() + ")" : "";
                lblProceduretxt.Text = dt.Rows[0]["FLDNEWCODE"].ToString() + "-" + dt.Rows[0]["FLDPROCEDURE"].ToString() + Guidance;
               
            }
            else
            {
                if (dt2.Rows.Count > 0)
                {
                    txtVersion.Text = "0";// dt2.Rows[0]["FLDVERSION"].ToString();
                    ViewState["PROCEDUREID"] = dt2.Rows[0]["FLDEUMRVPROCEDUREID"].ToString();
                    //string Guidance = General.GetNullableString(dt.Rows[0]["FLDGUIDANCE"].ToString()) != null ? " (" + dt.Rows[0]["FLDGUIDANCE"].ToString() + ")" : "";
                    lblProceduretxt.Text = dt2.Rows[0]["FLDNEWCODE"].ToString() + "-" + dt2.Rows[0]["FLDPROCEDURE"].ToString();// + Guidance;

                }
            }
            ddlProcedure.CssClass = "hidden";
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
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                Response.Redirect("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx?Lanchfrom=1");
            else
                Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedure.aspx?Lanchfrom=0");
        }   
    }
}
