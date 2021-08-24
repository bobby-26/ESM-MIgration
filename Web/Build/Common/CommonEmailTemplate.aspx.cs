using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web.UI.WebControls;


public partial class CommonEmailTemplate : PhoenixBasePage
{    
    
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("New", "NEW");
            toolbarmain.AddButton("Save", "SAVE");
            MenuEmailTemplate.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                string[] str = { "#FIRSTNAME#", "#MIDDLENAME#", "#LASTNAME#", "#DOB#", "#ADDRESS1#"
                                   , "#ADDRESS2#", "#CITY#", "#STATE#", "#COUNTRY#", "#POSTALCODE#", "#EMAIL#"
                                   , "#JOININGVESSEL#", "#JOININGDATE#", "#PORTALLINK#" };
   
                lstField.DataSource = str;
                lstField.DataBind();
                ViewState["TEMPLATEID"] = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuEmailTemplate_TabStripCommand(object sender, EventArgs e)
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
                if (string.IsNullOrEmpty(ViewState["TEMPLATEID"].ToString()))
                {
                    PhoenixCommonEmailTemplate.InsertEmailTemplate(General.GetNullableInteger(ddlEmailType.SelectedHard).Value
                                                    , txtTempleteName.Text.ToString().Trim()
                                                    , txtTempleteDesc.Text.ToString().Trim()
                                                    , txtsubject.Text.Trim()
                                                    , Editor1.Content.ToString());
                    SetEmailTemplate(null);
                    ddlEmailType_SelectedIndexChanged(null, null);
                }
                else
                {
                    PhoenixCommonEmailTemplate.UpdateEmailTemplate(General.GetNullableInteger(ViewState["TEMPLATEID"].ToString()).Value
                                                   , txtTempleteName.Text.ToString().Trim()
                                                   , txtTempleteDesc.Text.ToString().Trim()
                                                   , txtsubject.Text.Trim()
                                                   , Editor1.Content.ToString());
                    SetEmailTemplate(General.GetNullableInteger(ViewState["TEMPLATEID"].ToString()).Value);
                }
                ucStatus.Text = "Email Template Information Updated.";
               
            }
            else if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                SetEmailTemplate(null);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
       
    protected void ddlEmailType_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetEmailTemplate(null);
        if (!string.IsNullOrEmpty(ddlEmailType.SelectedHard))
        {
            ddlEmailTemplate.DataSource = PhoenixCommonEmailTemplate.ListEmailTemplate(General.GetNullableInteger(ddlEmailType.SelectedHard), null);
            ddlEmailTemplate.DataValueField = "FLDTEMPLATEID";
            ddlEmailTemplate.DataTextField = "FLDTEMPLATENAME";
            ddlEmailTemplate.DataBind();
        }
    }
    private bool IsValidateTemplate()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(ddlEmailType.SelectedHard))
        {
            ucError.ErrorMessage = "Template Type is Required.";
        }
        if (string.IsNullOrEmpty(txtTempleteName.Text.Trim()))
        {
            ucError.ErrorMessage = "Template Name is Required.";
        }      
        return (!ucError.IsError);
    }
    protected void ddlEmailTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetEmailTemplate(General.GetNullableInteger(ddlEmailTemplate.SelectedValue));
    }
    protected void SetEmailTemplate(int? iTempldateId)
    {
        if (iTempldateId.HasValue)
        {
            DataSet ds = PhoenixCommonEmailTemplate.ListEmailTemplate(null, iTempldateId);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["TEMPLATEID"] = ds.Tables[0].Rows[0]["FLDTEMPLATEID"].ToString();
                Editor1.Content = ds.Tables[0].Rows[0]["FLDTEMPLATE"].ToString();
                txtTempleteDesc.Text = ds.Tables[0].Rows[0]["FLDTEMPLATEDESC"].ToString();
                txtTempleteName.Text = ds.Tables[0].Rows[0]["FLDTEMPLATENAME"].ToString();
                txtsubject.Text = ds.Tables[0].Rows[0]["FLDSUBJECT"].ToString();
            }
        }
        else
        {
            ViewState["TEMPLATEID"] = string.Empty;
            Editor1.Content = String.Empty;
            txtTempleteDesc.Text = "";
            txtTempleteName.Text = "";
            txtsubject.Text = "";
            ddlEmailTemplate.Items.Clear();
            ddlEmailTemplate.Items.Add(new ListItem("--Select--", ""));
        }
    }
}
