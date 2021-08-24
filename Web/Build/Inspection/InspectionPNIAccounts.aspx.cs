using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionPNIAccounts : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtAcctIncharge.Attributes.Add("style", "visibility:hidden");
        txtAcctInchargeEmailHidden.Attributes.Add("style", "visibility:hidden");
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();            
            toolbar.AddButton("Submit to Singapore A/C", "SUBMIT");
            toolbar.AddButton("Save", "SAVE");
            MenuInspectionPNIAccounts.AccessRights = this.ViewState;
            MenuInspectionPNIAccounts.MenuList = toolbar.Show();
            MenuInspectionPNIAccounts.SetTrigger(pnlInspectionPNI);

            if (Request.QueryString["PNIId"] != null)
            {
                ViewState["PNIID"] = Request.QueryString["PNIId"];
                EditInspectionPNI(new Guid(ViewState["PNIID"].ToString()));
            }
            else
            {
                ViewState["PNIID"] = null;
            }
        }        
    }

    private void EditInspectionPNI(Guid pniid)
    {
        DataSet ds = PhoenixInspectionPNI.EditInspectionPNI(pniid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtAcctAdminId.Text = dr["FLDACCOUNTADMIN"].ToString();
            txtSPA.Text = dr["FLDACCOUNTADMINNAME"].ToString();
            txtCaseSubmittedDate.Text = dr["FLDDATESUBMITTEDTOACCT"].ToString();
            txtRemarks.Text = dr["FLDDESCSUBMITTEDTOACCT"].ToString();
            txtTotalCost.Text = dr["FLDTOTALCOST"].ToString();
            txtPNIClaimDate.Text = dr["FLDPNIDATECLAIM"].ToString();
            txtDateAmtReceived.Text = dr["FLDPNIAMOUNTCLAIMDATE"].ToString();
            txtFinalClosure.Text = dr["FLDFINALCLOSUREDATE"].ToString();
            txtAcctInchargeName.Text = dr["FLDACCOUNTICNAME"].ToString();
            txtAcctInchargeDesignation.Text = dr["FLDACCOUNTICDESIGNATION"].ToString();
            txtAcctIncharge.Text = dr["FLDACCOUNTIC"].ToString();
            txtAcctInchargeEmailHidden.Text = "";
            txtSubmittedtoSingaporeAcct.Text = dr["FLDDATESUBMITTEDTOSINGAPOREACCT"].ToString();

            if (string.IsNullOrEmpty(txtSubmittedtoSingaporeAcct.Text))
            {
                pnlAccountsIndia.Enabled = true;
                pnlAccountsSingapore.Enabled = false;                
            }
            else
            {
                pnlAccountsIndia.Enabled = false;
                pnlAccountsSingapore.Enabled = true;
            }
        }
        else
        {
            pnlAccountsIndia.Enabled = true;
            pnlAccountsSingapore.Enabled = false;
        }
    }

    protected void InspectionPNIAccounts_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if(pnlAccountsIndia.Enabled == true)
            {
                if (isValidRemarks(txtRemarks.Text))
                {
                    if (ViewState["PNIID"] != null)
                    {
                        PhoenixInspectionPNI.UpdateAccountsIndia(new Guid(ViewState["PNIID"].ToString()),
                            General.GetNullableInteger(txtAcctAdminId.Text),
                            null,
                            txtRemarks.Text);

                        ucStatus.Text = "P & I details are updated.";
                        EditInspectionPNI(new Guid(ViewState["PNIID"].ToString()));
                    }                    
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            if (pnlAccountsSingapore.Enabled == true)
            {
                if (ViewState["PNIID"] != null)
                {
                    PhoenixInspectionPNI.UpdateAccountsSingapore(new Guid(ViewState["PNIID"].ToString()),
                        General.GetNullableDecimal(txtTotalCost.Text),
                        General.GetNullableDateTime(txtPNIClaimDate.Text),
                        General.GetNullableDateTime(txtDateAmtReceived.Text),
                        General.GetNullableDateTime(txtFinalClosure.Text),
                        General.GetNullableInteger(txtAcctIncharge.Text));

                    ucStatus.Text = "P & I details are updated.";
                    EditInspectionPNI(new Guid(ViewState["PNIID"].ToString()));
                }
            }
        }
        else if (dce.CommandName.ToUpper().Equals("SUBMIT"))
        {
            DateTime submittosingapore;
            submittosingapore = DateTime.Now;
            if (isValidRemarks(txtRemarks.Text))
            {
                if (ViewState["PNIID"] != null)
                {
                    try
                    {
                        PhoenixInspectionPNI.UpdateAccountsIndia(new Guid(ViewState["PNIID"].ToString()),
                            General.GetNullableInteger(txtAcctAdminId.Text),
                            General.GetNullableDateTime(submittosingapore.ToString()),
                            txtRemarks.Text);
                    }
                    catch (Exception ex)
                    {
                        ucError.ErrorMessage = ex.Message;
                        ucError.Visible = true;
                        return;
                    }

                    ucStatus.Text = "P & I details are updated.";
                    EditInspectionPNI(new Guid(ViewState["PNIID"].ToString()));
                }
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
    }

    private bool isValidRemarks(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtRemarks.Text == "")
            ucError.ErrorMessage = "Remarks is required.";

        return (!ucError.IsError);
    }
}
