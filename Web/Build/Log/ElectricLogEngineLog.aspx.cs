using SouthNests.Phoenix.Framework;
using System;

public partial class Log_ElectricLogEngineLog : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AssignLink();
    }

    private void AssignLink()
    {
        btnEngineLogBook.Attributes.Add("onclick", string.Format("openWindow();"));
    }
}