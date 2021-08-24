using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;


public partial class PreSeaEmailTemplate : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("New", "NEW");
            toolbarmain.AddButton("Save", "SAVE");
            MenuReportTemplate.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                string[] str = { "#BATCHNO#", "#ROLLNO#", "#CANDIDATENAME#","#STUDENTNAME#" , "#DOB#", "#ADDRESS1#"
                                   , "#ADDRESS2#", "#CITY#", "#STATE#", "#COUNTRY#", "#POSTALCODE#"};
                ddlEmail.DataSource = PhoenixPreSeaTemplate.ListEmail(null);
                ddlEmail.DataValueField = "FLDEMAILTEMPLATECODE";
                ddlEmail.DataTextField = "FLDEMAILTEMPLATENAME";
                ddlEmail.DataBind();
                lstField.DataSource = str;
                lstField.DataBind();
                ViewState["TEMPLATECODE"] = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReportTemplate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidateTemplate())
                {
                    ucError.Visible = true;
                    return;
                }
                if (string.IsNullOrEmpty(ViewState["TEMPLATECODE"].ToString()))
                {
                    PhoenixPreSeaTemplate.InsertEmailTemplate(int.Parse(ddlEmail.SelectedValue)
                                                    , txtSubject.Text.ToString().Trim()
                                                    , Editor1.Content.ToString());
                    SetReportTemplate(null);
                    ddlEmail_SelectedIndexChanged(null, null);
                }
                else
                {
                    PhoenixPreSeaTemplate.UpdateEmailTemplate(General.GetNullableInteger(ViewState["TEMPLATECODE"].ToString()).Value
                                                   , txtSubject.Text.ToString().Trim()
                                                   , Editor1.Content.ToString());

                    SetReportTemplate(General.GetNullableInteger(ViewState["TEMPLATECODE"].ToString()).Value);
                }
                ucStatus.Text = "Email Template Information Saved Successfully.";

            }
            else if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                SetReportTemplate(null);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlEmail_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetReportTemplate(General.GetNullableInteger(ddlEmail.SelectedValue));
    }

    private bool IsValidateTemplate()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(txtSubject.Text.Trim()))
        {
            ucError.ErrorMessage = "Subject for the Mail is Required.";
        }
        return (!ucError.IsError);
    }

    protected void SetReportTemplate(int? iTempldateId)
    {
        if (iTempldateId.HasValue)
        {
            DataTable dt = PhoenixPreSeaTemplate.ListEmailTemplate(iTempldateId);
            if (dt.Rows.Count > 0)
            {
                ViewState["TEMPLATECODE"] = dt.Rows[0]["FLDEMAILTEMPLATECODE"].ToString();
                txtSubject.Text = dt.Rows[0]["FLDSUBJECT"].ToString();
                Editor1.Content = dt.Rows[0]["FLDEMAILCONTENT"].ToString();
            }
            else
            {
                ViewState["TEMPLATECODE"] = string.Empty;
                Editor1.Content = String.Empty;
                txtSubject.Text = "";
            }
        }
        else
        {
            ViewState["TEMPLATECODE"] = string.Empty;
            Editor1.Content = String.Empty;
            txtSubject.Text = "";
        }
    }
}
