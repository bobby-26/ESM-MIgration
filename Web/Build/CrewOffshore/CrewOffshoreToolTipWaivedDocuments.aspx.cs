using System;
using System.Data;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;

public partial class CrewOffshoreToolTipWaivedDocuments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["crewplanid"] = "";
            if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"] != null)
                ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();

            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void BindData()
    {
        string[] alColumns = { "FLDDOCUMENTNAME", "FLDWAIVINGREASON" };
        string[] alCaptions = { "Document", "Comments" };
        DataTable dt = PhoenixCrewOffshoreCrewChange.WaivedDocumentList(General.GetNullableGuid(ViewState["crewplanid"].ToString()));
        lblGrid.Text = General.ShowGrid(dt, alColumns, alCaptions); 
    }
}
