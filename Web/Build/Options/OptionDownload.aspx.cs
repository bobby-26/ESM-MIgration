using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;


public partial class OptionDownload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string dtkey = Request["dtkey"];

            if (General.GetNullableGuid(dtkey).HasValue)
            {
                DataSet ds = PhoenixGeneralSettings.ConfigurationSettingEdit(1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = ds.Tables[0].Rows[0]["FLDATTACHMENTPATH"].ToString();

                    string archivedpath = ds.Tables[0].Rows[0]["FLDARCHIVEDATTACHMENTPATH"].ToString();

                    if (!path.EndsWith("/"))
                        path = path + "/";
                    if (!archivedpath.EndsWith("/"))
                        archivedpath = archivedpath + "/";

                    DataTable dt = PhoenixCommonFileAttachment.EditAttachment(new Guid(dtkey));
                    if (dt.Rows.Count > 0)
                    {

                        fileDownload(dt.Rows[0]["FLDFILENAME"].ToString(), path + dt.Rows[0]["FLDFILEPATH"].ToString(), archivedpath + dt.Rows[0]["FLDFILEPATH"].ToString());
                    }

                }
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

    private void fileDownload(string fileName, string fileUrl, string archivedurl)
    {
        Page.Response.Clear();
        bool success = PhoenixCommonDownload.ResponseFile(Page.Request, Page.Response, fileName, fileUrl, 1024000);
        if (!success)
        {
            bool archivesuccess = PhoenixCommonDownload.ResponseFile(Page.Request, Page.Response, fileName, archivedurl, 1024000);
            if (!archivesuccess)
            {
                Response.Write("Downloading Error!");
            }
        }

        Page.Response.End();

    }
}