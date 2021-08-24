using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;

public partial class Common_Download : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string dtkey = Request["dtkey"];
            string manualid = Request["manualid"];
            string formid = Request["formid"];

            if (General.GetNullableGuid(dtkey).HasValue)
            {
                DataTable dt = PhoenixCommonFileAttachment.EditAttachment(new Guid(dtkey));
                if (dt.Rows.Count > 0)
                {
                    string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                    string archivedpath = PhoenixGeneralSettings.CurrentGeneralSetting.ArchivedAttachmentPath;
                    if (!path.EndsWith("/"))
                        path = path + "/";
                    if (!archivedpath.EndsWith("/"))
                        archivedpath = archivedpath + "/";

                    fileDownload(dt.Rows[0]["FLDFILENAME"].ToString(), path + dt.Rows[0]["FLDFILEPATH"].ToString(), archivedpath + dt.Rows[0]["FLDFILEPATH"].ToString());
                }
            }
            else if (General.GetNullableGuid(manualid).HasValue)
            {
                DataTable dt = PhoenixPlannedMaintenanceComponentJobManual.Edit(new Guid(manualid));
                if (dt.Rows.Count > 0)
                {
                    string path = PhoenixGeneralSettings.CurrentGeneralSetting.PMSManualsPath;
                    if (!path.EndsWith("/"))
                        path = path + "/";
                    fileDownload(dt.Rows[0]["FLDMANUALNAME"].ToString(), path + dt.Rows[0]["FLDMANUALPATH"].ToString());
                }
            }
            else if (General.GetNullableGuid(formid).HasValue)
            {
                DataSet ds = PhoenixDocumentManagementForm.FormEdit(
                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                  , new Guid(formid));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["FLDFORMREVISIONDTKEY"] != null && General.GetNullableGuid(dr["FLDFORMREVISIONDTKEY"].ToString()) != null)
                    {
                        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(dr["FLDFORMREVISIONDTKEY"].ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                            string archivedpath = PhoenixGeneralSettings.CurrentGeneralSetting.ArchivedAttachmentPath;
                            if (!path.EndsWith("/"))
                                path = path + "/";
                            if (!archivedpath.EndsWith("/"))
                                archivedpath = archivedpath + "/";
                            DMSfileDownload(dt.Rows[0]["FLDFILENAME"].ToString(), path + dt.Rows[0]["FLDFILEPATH"].ToString(), archivedpath + dt.Rows[0]["FLDFILEPATH"].ToString());
                        }
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

    private void DMSfileDownload(string fileName, string fileUrl, string archivedurl)
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
        if (!fileName.EndsWith(".html"))
            Page.Response.End();
    }
}
