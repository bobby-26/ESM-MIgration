using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;

public partial class OptionsConfigurationSetting : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save","SAVE", ToolBarDirection.Right);
        MenuConfigurationEdit.MenuList = toolbar.Show();
        BindData();
    }
    public void BindData()
    {
        DataSet ds = new DataSet();
        ds = PhoenixGeneralSettings.ConfigurationSettingEdit(1);
        DataTable dt = ds.Tables[0];
        txtInstallCode.Text = dt.Rows[0]["FLDINSTALLCODE"].ToString();
        txtSyncPath.Text = dt.Rows[0]["FLDSYNCDBPATH"].ToString();
        txtAttachmentPath.Text = dt.Rows[0]["FLDATTACHMENTPATH"].ToString();
        txtMailPath.Text = dt.Rows[0]["FLDOFFICEEMAIL"].ToString();
        txtSmtpHost.Text = dt.Rows[0]["FLDSMTPHOST"].ToString();
        txtSmtpPort.Text = dt.Rows[0]["FLDSMTPPORT"].ToString();
        txtPop3Host.Text = dt.Rows[0]["FLDPOP3HOST"].ToString();
        txtPop3Port.Text = dt.Rows[0]["FLDPOP3PORT"].ToString();
        txtMailExePath.Text = dt.Rows[0]["FLDMAILEXEPATH"].ToString();
        txtusername.Text = dt.Rows[0]["FLDUSERNAME"].ToString();
        txtpassword.Text = dt.Rows[0]["FLDPASSWORD"].ToString();


    }
    protected void MenuConfigurationEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixGeneralSettings.ConfigurationUpdate(1, int.Parse(txtInstallCode.Text),
                                    txtSyncPath.Text,
                                    txtAttachmentPath.Text,
                                    txtMailPath.Text,
                                    txtSmtpHost.Text,
                                    int.Parse(txtSmtpPort.Text),
                                    txtPop3Host.Text,
                                    int.Parse(txtPop3Port.Text),
                                    txtMailExePath.Text,txtusername.Text,txtpassword.Text);

                ucStatus.Text = "Updated Successfully";
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
}
