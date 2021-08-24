using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class OptionsDocument : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();


        toolbarmain.AddButton("Serial", "SERIAL", ToolBarDirection.Right);
        toolbarmain.AddButton("Format", "FORMAT", ToolBarDirection.Right);
        toolbarmain.AddButton("Fields", "FIELDS", ToolBarDirection.Right);
        toolbarmain.AddButton("Document Type", "DOCTYPE", ToolBarDirection.Right);

        MenuOrderFormMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            MenuOrderFormMain.SelectedMenuIndex = 3;
            ViewState["PAGEURL"] = "OptionsDocumentType.aspx";
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();
        }

    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("DOCTYPE"))
            {
                MenuOrderFormMain.SelectedMenuIndex = 3;
                ViewState["PAGEURL"] = "../Options/OptionsDocumentType.aspx";
                ifMoreInfo.Attributes["src"] = "../Options/OptionsDocumentType.aspx";
            }
            if (CommandName.ToUpper().Equals("FIELDS"))
            {
                MenuOrderFormMain.SelectedMenuIndex = 2;
                ViewState["PAGEURL"] = "../Options/OptionsDocumentFields.aspx";
                ifMoreInfo.Attributes["src"] = "../Options/OptionsDocumentFields.aspx";
            }
            if (CommandName.ToUpper().Equals("FORMAT"))
            {
                MenuOrderFormMain.SelectedMenuIndex = 1;
                ViewState["PAGEURL"] = "../Options/OptionsDocumentNumberFormat.aspx";
                ifMoreInfo.Attributes["src"] = "../Options/OptionsDocumentNumberFormat.aspx";
            }
            if (CommandName.ToUpper().Equals("SERIAL"))
            {
                MenuOrderFormMain.SelectedMenuIndex = 0;
                ViewState["PAGEURL"] = "../Options/OptionsDocumentSerial.aspx";
                ifMoreInfo.Attributes["src"] = "../Options/OptionsDocumentSerial.aspx";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
