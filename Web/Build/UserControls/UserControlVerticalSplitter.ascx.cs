using System;
using System.Web.UI;

public partial class UserControls_UserControlVerticalSplitter : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        divSplit.Style.Add("background-image", "url('" + Session["images"] + "/toolbar_left.png')");
        if (!IsPostBack)
        {
            txtTargetWidth.Attributes["style"] = "display:none";
        }
        if (txtTargetWidth.Text != string.Empty)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "resizeWidth", "document.getElementById('" + spnTargetControlID.InnerText + "').style.width='" + txtTargetWidth.Text + "';", true);
        }
    }

    public string TargetControlID
    {
        get
        {
            return spnTargetControlID.InnerText;
        }
        set
        {
            spnTargetControlID.InnerText = value;
        }
    }
}
