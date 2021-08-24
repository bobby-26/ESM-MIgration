using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class RegistersFMSFileNoAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (Request.QueryString["FILENOID"] != null)
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                ViewState["FileNoID"] = Request.QueryString["FileNoID"].ToString();
                FileNoEdit(Request.QueryString["FileNoID"].ToString());
            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            MenuFMSFileNo.AccessRights = this.ViewState;
            MenuFMSFileNo.MenuList = toolbar.Show();
            bindsource();
        }
    }


    private void bindsource()
    {
        ddlsource.DataSource = PhoenixRegisterFMSMail.FMSTypeList();
        ddlsource.DataTextField = "FLDFMSTYPENAME";
        ddlsource.DataValueField = "FLDFMSTYPEID";
        ddlsource.DataBind();
    }
    protected void FMSFileNo_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidFileNo())
            {
                ucError.Visible = true;
                return;
            }
            try
            {
                if (ViewState["FileNoID"] != null)
                {
                    PhoenixRegisterFMSMail.FMSInsertFileNo(General.GetNullableGuid(ViewState["FileNoID"].ToString())
                                                        , txtFileNo.Text
                                                        , txtDescription.Text
                                                        , txtHint.Text
                                                        , General.GetNullableInteger(ddlsource.SelectedValue));
                    ucStatus.Text = "File No Updated";
                }
                else
                {
                    PhoenixRegisterFMSMail.FMSInsertFileNo(null
                                                     , txtFileNo.Text
                                                     , txtDescription.Text
                                                     , txtHint.Text
                                                     , General.GetNullableInteger(ddlsource.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

            String script = String.Format("javascript:fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    private bool IsValidFileNo()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtFileNo.Text.Equals(""))
            ucError.ErrorMessage = "File No is required.";

        if (txtDescription.Text.Equals(""))
            ucError.ErrorMessage = "Description is required.";

        if (txtHint.Text.Equals(""))
            ucError.ErrorMessage = "Mail Hint is required.";

        if (ddlsource.SelectedValue.Equals(""))
            ucError.ErrorMessage = "Source Type is required.";


        return (!ucError.IsError);
    }

    private void FileNoEdit(string FileNoID)
    {
        try
        {
            DataSet ds = PhoenixRegisterFMSMail.EditFMSFileNo(new Guid(FileNoID));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtFileNo.Text = dr["FLDFILENO"].ToString();
                txtFileNo.Enabled = false;
                txtDescription.Text = dr["FLDFILENODESCRIPTION"].ToString();
                txtHint.Text = dr["FLDFILENOHINT"].ToString();
                ddlsource.SelectedValue = dr["FLDSOURCETYPE"].ToString();
                //rcbActive.Checked = dr["FLDACTIVEYN"].ToString() == "1" ? true : false;                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
