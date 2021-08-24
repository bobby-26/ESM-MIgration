using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;

public partial class DryDock_Drydockdownload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string drydockquotationid = Request.QueryString["Drydockquotationid"];
            string vesselid = Request.QueryString["Vslid"];

            if (General.GetNullableGuid(drydockquotationid).HasValue)
            {
                string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                if (!path.EndsWith("/"))
                    path = path + "/";
                string filename = PhoenixDryDockQuotation.GetDryDockQuotationYardZipFileName(new Guid(drydockquotationid), int.Parse(vesselid)) + ".zip";
                fileDownload(filename, path + "Drydock//" + filename);
            }
            else
            {
                Response.Write("Downloading Error!");
                Page.Response.End();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Downloading Error! " + ex.Message);
            Page.Response.End();
        }
    }

    private void fileDownload(string fileName, string fileUrl)
    {
        Page.Response.Clear();
        bool success = PhoenixCommonDownload.ResponseFile(Page.Request, Page.Response, fileName, fileUrl, 1024000);
        if (!success)
            Response.Write("Downloading Error!");
        Page.Response.End();

    }
}