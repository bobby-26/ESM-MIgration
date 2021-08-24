using System;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Web;
using System.Security.Permissions;
using System.Security;
using Telerik.Web.UI.FileExplorer;
using Telerik.Web.UI.Widgets;
using System.Xml;
public partial class CrewPlanEventEmail : PhoenixBasePage
{
    string empid = string.Empty;
    string eventid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Send", "SEND", ToolBarDirection.Right);

            EmailMenu.AccessRights = this.ViewState;
            EmailMenu.MenuList = toolbarmain.Show();
            FileExplorer1.Grid.ClientSettings.ClientEvents.OnRowDataBound = "OnGridRowDataBound";
            GridTemplateColumn tc = (GridTemplateColumn)FileExplorer1.Grid.Columns.FindByUniqueName("Size");
            tc.HeaderStyle.Width = Unit.Parse("100px");
            if (!IsPostBack)
            {
                RemoveEditorToolBarIcons(); // remove the unwanted icons in editor

                Session["mailsessionid"] = "";
                ViewState["empid"] = "";
                Session["AttachFiles"] = null;

                ViewState["EVENTID"] = "";

                if (Request.QueryString["eventid"] != null)
                {
                    ViewState["FILENAME"] = null;

                    eventid = Request.QueryString["eventid"];
                    ViewState["mailsessionid"] = eventid;
                    Session["mailsessionid"] = ViewState["mailsessionid"];

                    ViewState["EVENTID"] = Request.QueryString["eventid"];
                    EventRequestMail(eventid);

                }


                FileExplorer1.ExplorerMode = FileExplorerMode.Default;

                FileExplorer1.Configuration.ViewPaths = new string[] { PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + @"\EmailAttachments\" + ViewState["mailsessionid"].ToString() + @"\" };
                FileExplorer1.Configuration.UploadPaths = new string[] { PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + @"\EmailAttachments\" + ViewState["mailsessionid"].ToString() + @"\" };
                FileExplorer1.Configuration.DeletePaths = new string[] { PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + @"\EmailAttachments\" + ViewState["mailsessionid"].ToString() + @"\" };
                FileExplorer1.Configuration.ContentProviderTypeName = typeof(CustomFileSystemProvider).AssemblyQualifiedName;

                FileExplorer1.Localization.SetString("Common_ConfirmDelete", "Are you sure you want to delete the selected item? Press \"OK\" to confirm deletion");

                FileExplorer1.Configuration.MaxUploadFileSize = 4096 * 1024;

            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    protected void RemoveEditorToolBarIcons()
    {
        edtBody.EnsureToolsFileLoaded();
        RemoveButton("ImageManager");
        RemoveButton("DocumentManager");
        RemoveButton("FlashManager");
        RemoveButton("MediaManager");
        RemoveButton("TemplateManager");
        RemoveButton("XhtmlValidator");
        RemoveButton("InsertSnippet");
        RemoveButton("ModuleManager");
        RemoveButton("ImageMapDialog");
        RemoveButton("AboutDialog");
        RemoveButton("InsertFormElement");

        edtBody.EnsureToolsFileLoaded();
        edtBody.Modules.Remove("RadEditorHtmlInspector");
        edtBody.Modules.Remove("RadEditorNodeInspector");
        edtBody.Modules.Remove("RadEditorDomInspector");
        edtBody.Modules.Remove("RadEditorStatistics");

    }

    protected void RemoveButton(string name)
    {
        foreach (EditorToolGroup group in edtBody.Tools)
        {
            EditorTool tool = group.FindTool(name);
            if (tool != null)
            {
                group.Tools.Remove(tool);
            }
        }

    }

    protected void EmailMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEND"))
            {
                if (!IsValidateEmail())
                {
                    ucError.Visible = true;
                    return;
                }

                SendMail();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('NAFA', 'ifMoreInfo', null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SendMail()
    {

        try
        {
            PhoenixMail.SendMail(txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, true, (MailPriority)Convert.ToInt32(ddlPriority.SelectedValue.ToString()), ViewState["mailsessionid"].ToString());
        }
        catch (Exception ex)
        {
            throw ex.InnerException;
        }
    }
    private bool IsValidateEmail()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.IsvalidEmail(txtTO.Text))
        {
            ucError.ErrorMessage = "Please enter valid To E-Mail Address";
        }
        if (txtCC.Text.Trim() != string.Empty && !General.IsvalidEmail(txtTO.Text))
        {
            ucError.ErrorMessage = "Please enter valid Cc E-Mail Address";
        }
        if (txtBCC.Text.Trim() != string.Empty && !General.IsvalidEmail(txtBCC.Text))
        {
            ucError.ErrorMessage = "Please enter valid Bcc E-Mail Address";
        }
        return (!ucError.IsError);
    }
    private void EventRequestMail(string eventid)
    {
        DataTable dtdetail = PhoenixCrewChangeEventDetail.ListCrewPlanEventDetail(new Guid(eventid));

        StringBuilder sbemailbody = new StringBuilder();
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Good Day,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");

        if (dtdetail.Rows.Count > 0)
        {
            DataRow dr1 = dtdetail.Rows[0];

            sbemailbody.AppendLine("Below are the planned seafarer in the port " + dr1["FLDPORTNAME"].ToString() + " between " + dr1["FLDEVENTDATE"].ToString() + " and " + dr1["FLDEVENTTODATE"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>Crew Change Details");

            foreach (DataRow dt1 in dtdetail.Rows)
            {
                DataTable dtDOC = PhoenixCrewChangeEventDetail.ListCrewPlanEventDetailDoc(new Guid(eventid)
                , new Guid(dt1["FLDCREWEVENTDETAILID"].ToString()));

                if (dtDOC.Rows.Count > 0)
                {
                    DataRow drDoc = dtDOC.Rows[0];

                    sbemailbody.AppendLine("<br/>");

                    if (drDoc["FLDEMPLOYEEID"].ToString() != "-1")
                    {
                        sbemailbody.AppendLine("<br/>Onsigner");
                        sbemailbody.AppendLine("<br/>Name       :" + " " + drDoc["FLDEMPLOYEENAME"].ToString());
                        sbemailbody.AppendLine("<br/>Rank       :" + " " + drDoc["FLDRANKNAME"].ToString());
                        sbemailbody.AppendLine("<br/>Nationality:" + " " + drDoc["FLDNATIONALITY"].ToString());
                        sbemailbody.AppendLine("<br/>Passport No:" + " " + drDoc["FLDPASSPORTNO"].ToString());
                        sbemailbody.AppendLine("<br/>Issued - Valid until - Place :" + " " + drDoc["FLDDATEOFISSUE"].ToString() + " - " + drDoc["FLDDATEOFEXPIRY"].ToString() + " - " + drDoc["FLDPLACEOFISSUE"].ToString());
                        sbemailbody.AppendLine("<br/>Seaman Book No:" + " " + drDoc["FLDSEAMANBOOKNO"].ToString());
                        sbemailbody.AppendLine("<br/>Issued - Valid until - Place:" + " " + drDoc["FLDSDATEOFISSUE"].ToString() + " - " + drDoc["FLDSDATEOFEXPIRY"].ToString() + " - " + drDoc["FLDSPLACEOFISSUE"].ToString());

                        DataTable dtTravelonsigner = PhoenixCrewChangeEventDetail.ListCrewPlanEventTravelDetail(new Guid(dt1["FLDCREWEVENTDETAILID"].ToString())
                                                                , General.GetNullableInteger(drDoc["FLDEMPLOYEEID"].ToString())
                                                                , null);

                        if (dtTravelonsigner.Rows.Count > 0)
                        {
                            sbemailbody.AppendLine("<br/> ");
                            sbemailbody.AppendLine("<br/>Travel Details of " + drDoc["FLDEMPLOYEENAME"].ToString() +" :");
                           
                            for (int i = 0; i < dtTravelonsigner.Rows.Count; i++)
                            {
                                DataRow drTravelonsigner = dtTravelonsigner.Rows[i];
                                
                                sbemailbody.AppendLine("<br/>Departure          :" + " " + drTravelonsigner["FLDORIGINCITY"].ToString() + " On " + drTravelonsigner["FLDDEPARTUREDATE"].ToString());
                                sbemailbody.AppendLine("<br/>Arrival            :" + " " + drTravelonsigner["FLDDESTINATIONCITY"].ToString() + " On " + drTravelonsigner["FLDARRIVALDATE"].ToString());
                                sbemailbody.AppendLine("<br/>Ticket No          :" + " " + drTravelonsigner["FLDTICKETNO"].ToString());
                                sbemailbody.AppendLine("<br/>Airline Code       :" + " " + drTravelonsigner["FLDAIRLINECODE"].ToString());
                            }
                        }



                    }
                    if (drDoc["FLDOFFSIGNERID"].ToString() != "-1")
                    {
                        sbemailbody.AppendLine("<br/>Offsigner");
                        sbemailbody.AppendLine("<br/>Name       :" + " " + drDoc["FLDOFFSIGNERNAME"].ToString());
                        sbemailbody.AppendLine("<br/>Rank       :" + " " + drDoc["FLDOFFSIGNERRANKNAME"].ToString());
                        sbemailbody.AppendLine("<br/>");

                        DataTable dtTraveloffsigner = PhoenixCrewChangeEventDetail.ListCrewPlanEventTravelDetail(new Guid(dt1["FLDCREWEVENTDETAILID"].ToString())                                                              
                                                              , null
                                                              , General.GetNullableInteger(drDoc["FLDOFFSIGNERID"].ToString()));

                        if (dtTraveloffsigner.Rows.Count > 0)
                        {                         
                            sbemailbody.AppendLine("<br/>Travel Details of " + drDoc["FLDOFFSIGNERNAME"].ToString() + " :");
                            
                            for (int i = 0; i < dtTraveloffsigner.Rows.Count; i++)
                            {
                                DataRow drTraveloffsigner = dtTraveloffsigner.Rows[i];

                                sbemailbody.AppendLine("<br/>Departure          :" + " " + drTraveloffsigner["FLDORIGINCITY"].ToString() + " On " + drTraveloffsigner["FLDDEPARTUREDATE"].ToString());
                                sbemailbody.AppendLine("<br/>Arrival            :" + " " + drTraveloffsigner["FLDDESTINATIONCITY"].ToString() + " On " + drTraveloffsigner["FLDARRIVALDATE"].ToString());
                                sbemailbody.AppendLine("<br/>Ticket No          :" + " " + drTraveloffsigner["FLDTICKETNO"].ToString());
                                sbemailbody.AppendLine("<br/>Airline Code       :" + " " + drTraveloffsigner["FLDAIRLINECODE"].ToString());
                            }
                        }
                    }
                }

                DataTable dtAtt = PhoenixCrewChangeEventDetail.ListCrewPlanEventAttachmentDetail(new Guid(eventid)
                           , new Guid(dt1["FLDCREWEVENTDETAILID"].ToString()));

                if (dtAtt.Rows.Count > 0)
                {
                    string dirpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/" + ViewState["mailsessionid"].ToString();
                    if (!Directory.Exists(dirpath))
                        Directory.CreateDirectory(dirpath);
                    string filepath = string.Empty;
                    for (int i = 0; i < dtAtt.Rows.Count; i++)
                    {
                        filepath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + dtAtt.Rows[i]["FLDFILEPATH"].ToString().ToString();
                        if (File.Exists(filepath))
                        {
                            FileInfo fi = new FileInfo(filepath);
                            fi.CopyTo(dirpath + "/" + dtAtt.Rows[i]["FLDEMPLOYEENAME"].ToString() + " - " + dtAtt.Rows[i]["FLDFILENAME"].ToString(), true);
                        }

                    }
                }

            }
        }
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>Thank you,");



        DataTable dtEmail = PhoenixCrewChangeEventDetail.CrewPlanEventEmailDetails(new Guid(eventid), int.Parse(rblEmailTo.SelectedValue));

        if (dtEmail.Rows.Count > 0)
        {
            DataRow dr = dtEmail.Rows[0];
            sbemailbody.AppendLine("<br/>" + dr["FLDUSERNAME"].ToString() + "");

            sbemailbody.AppendLine("<br/>");
            txtTO.Text = dr["FLEMAIL"].ToString().Replace(";", ",").TrimEnd(',');
            txtCC.Text = dr["FLDUSERMAIL"].ToString().Replace(";", ",").TrimEnd(',');

            txtSubject.Text = dr["FLDVESSELNAME"].ToString() + " - " + dr["FLDPORTNAME"].ToString() + " - Crew Change ";

        }

        edtBody.Content = sbemailbody.ToString();

    }

    protected void rblEmailTo_SelectedIndexChanged(object sender, EventArgs e)
    {

        DataTable dtEmail = PhoenixCrewChangeEventDetail.CrewPlanEventEmailDetails(new Guid(ViewState["EVENTID"].ToString()), int.Parse(rblEmailTo.SelectedValue));

        if (dtEmail.Rows.Count > 0)
        {
            DataRow dr = dtEmail.Rows[0];

            txtTO.Text = dr["FLEMAIL"].ToString().Replace(";", ",").TrimEnd(',');
            txtCC.Text = dr["FLDUSERMAIL"].ToString().Replace(";", ",").TrimEnd(',');
        }
        else
        {
            txtTO.Text = "";
            txtCC.Text = "";
        }
    }

}
public class CustomFileSystemProvider : FileBrowserContentProvider
{
    private string _itemHandlerPath;
    protected string ItemHandlerPath
    {
        get
        {
            return this._itemHandlerPath;
        }
    }

    private Dictionary<string, string> mappedPathsInConfigFile;
    //private string pathToConfigFile = "";

    /// <summary>
    /// Returns the mappings from the configuration file
    /// </summary>
    protected Dictionary<string, string> MappedPaths
    {
        get { return mappedPathsInConfigFile; }
    }

    public CustomFileSystemProvider(HttpContext context, string[] searchPatterns, string[] viewPaths, string[] uploadPaths, string[] deletePaths, string selectedUrl, string selectedItemTag)
        :
        base(context, searchPatterns, viewPaths, uploadPaths, deletePaths, selectedUrl, selectedItemTag)
    {
        // The 'viewPaths' contains values like "C:\Foder_1\Folder_2" or "C:\Foder_1\Folder_2\"


        this.Initialize();
    }

    private void Initialize()
    {
        string id = "";
        if (HttpContext.Current != null)
        {
            var request = HttpContext.Current.Session;
            id = request["mailsessionid"].ToString();
        }
        string xml = @"<CustomFileBrowserProvider>
  <Paths>
    <genericHandlerPath>FileSystemHandler.ashx</genericHandlerPath>
  </Paths>
  <Mappings>
    <Mapping>
      <PhysicalPath>
        <![CDATA[@physicalpath@]]>
      </PhysicalPath>
      <VirtualPath><![CDATA[/]]></VirtualPath>
    </Mapping>    
  </Mappings>
</CustomFileBrowserProvider>".Replace("@physicalpath@", PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + @"\EmailAttachments\");
        XmlDocument configFile = new XmlDocument();
        //string physicalPathToConfigFile = Context.Server.MapPath(this.pathToConfigFile);
        configFile.LoadXml(xml);// Load the configuration file
        XmlElement rootElement = configFile.DocumentElement;
        XmlNode handlerPathSection = rootElement.GetElementsByTagName("genericHandlerPath")[0]; // get all mappings ;dddddd
        this._itemHandlerPath = handlerPathSection.InnerText;

        this.mappedPathsInConfigFile = new Dictionary<string, string>();
        XmlNode mappingsSection = rootElement.GetElementsByTagName("Mappings")[0]; // get all mappings ;
        foreach (XmlNode mapping in mappingsSection.ChildNodes)
        {
            XmlNode virtualPathAsNode = mapping.SelectSingleNode("child::VirtualPath");
            XmlNode physicalPathAsNode = mapping.SelectSingleNode("child::PhysicalPath");
            this.mappedPathsInConfigFile.Add(PathHelper.RemoveEndingSlash(virtualPathAsNode.InnerText, '/'), PathHelper.RemoveEndingSlash(physicalPathAsNode.InnerText, '\\'));
        }
    }

    public override DirectoryItem ResolveRootDirectoryAsTree(string path)
    {
        string physicalPath;
        string virtualPath = string.Empty;

        if (PathHelper.IsSharedPath(path) || PathHelper.IsPhysicalPath(path))
        {// The path is a physical path
            physicalPath = path;

            foreach (KeyValuePair<string, string> mappedPath in MappedPaths)
            {
                // Checks whether a mapping exists for the current physical path
                // 'mappedPath.Value' does not end with trailing slash. It looks like : "C:\Path\Dir"
                if (physicalPath.StartsWith(mappedPath.Value, StringComparison.CurrentCultureIgnoreCase))
                {// Exists 

                    // Get the part of the physical path which does not contain the mappeed part
                    string restOfPhysicalPath = physicalPath.Substring(mappedPath.Value.Length);

                    // 'mappedPath.Value' does not end with '\'
                    // // The 'restOfVirtualPath' is something like Folder_1/SubFolder_2/ ==> convert it to Folder_1\SubFolder_2\
                    virtualPath = mappedPath.Key + restOfPhysicalPath.Replace('\\', '/');


                    virtualPath = PathHelper.AddEndingSlash(virtualPath, '/');
                    break;// Exit the 'foreach' loop ;
                }
            }
        }
        else
        {// Virtual path ;
            virtualPath = PathHelper.AddEndingSlash(path, '/');
            physicalPath = this.GetPhysicalFromVirtualPath(path);
            if (physicalPath == null)
                return null;
        }

        DirectoryItem result = new DirectoryItem(PathHelper.GetDirectoryName(physicalPath),
                                                    string.Empty,
                                                    virtualPath,
                                                    string.Empty,
                                                    GetPermissions(physicalPath),
                                                    new FileItem[] { }, // Files are added in the ResolveDirectory method
                                                    GetDirectories(virtualPath)
                                                );

        return result;
    }

    public override DirectoryItem ResolveDirectory(string virtualPath)
    {
        string physicalPath;
        physicalPath = this.GetPhysicalFromVirtualPath(virtualPath);

        if (physicalPath == null)
            return null;

        DirectoryItem result = new DirectoryItem(PathHelper.GetDirectoryName(physicalPath),
                                                    virtualPath,
                                                    virtualPath,
                                                    virtualPath,
                                                    GetPermissions(physicalPath),
                                                    GetFiles(virtualPath),
                                                    new DirectoryItem[] { }// Directories are added in ResolveRootDirectoryAsTree method
                                                );

        return result;
    }

    public override string MoveDirectory(string virtualSourcePath, string virtualDestPath)
    {
        virtualSourcePath = PathHelper.AddEndingSlash(virtualSourcePath, '/');
        virtualDestPath = PathHelper.AddEndingSlash(virtualDestPath, '/');

        string physicalSourcePath;

        physicalSourcePath = this.GetPhysicalFromVirtualPath(virtualSourcePath);
        if (physicalSourcePath == null)
            return string.Format("The virtual path :'{0}' cannot be converted to a physical path", virtualSourcePath);

        string physicalDestinationPath;
        physicalDestinationPath = this.GetPhysicalFromVirtualPath(virtualDestPath);
        if (physicalDestinationPath == null)
            return string.Format("The virtual path :'{0}' cannot be converted to a physical path", virtualDestPath);

        string newFolderName = physicalDestinationPath;

        // Checks whether the folder already exists in the destination folder ;
        if (Directory.Exists(newFolderName))
        {// Yes the folder exists :
            string message = string.Format("The folder '{0}' already exists", virtualDestPath);
            return message;
        }

        // Checks whether the source directory is parent of the destination directory ;
        if (PathHelper.IsParentOf(virtualSourcePath, virtualDestPath))
        {
            string message = string.Format("The folder  '{0}' is parent of the '{1}' directory. Operation is canceled!", virtualSourcePath, virtualDestPath);
            return message;
        }

        // There is not a permission issue with the FileExplorer's permissions ==> Move can be performed
        // But, there can be some FileSystem permissions issue (file system's read/write permissions) ;
        string errorMessage = FileSystem.MoveDirectorty(physicalSourcePath, physicalDestinationPath, virtualSourcePath, virtualDestPath);
        return errorMessage;
    }

    public override string MoveFile(string virtualSourcePath, string virtualDestPath)
    {
        string physicalSourcePath;
        physicalSourcePath = this.GetPhysicalFromVirtualPath(virtualSourcePath);
        if (physicalSourcePath == null)
            return string.Format("The virtual path :'{0}' cannot be converted to a physical path", virtualSourcePath);

        string physicalDestinationPath = this.GetPhysicalFromVirtualPath(virtualDestPath);

        if (physicalDestinationPath == null)
            return string.Format("The virtual path :'{0}' cannot be converted to a physical path", virtualDestPath);

        // Check whether the file already exists in the destination folder
        if (File.Exists(physicalDestinationPath))
        {// Yes the file exists :
            string message = string.Format("The file '{0}' already exists", virtualDestPath);
            return message;
        }

        // There is not permission issue with the FileExplorer's permissions ==> Move can be performed
        // There can be some FileSystem error 
        string errorMessage = FileSystem.MoveFile(physicalSourcePath, physicalDestinationPath, virtualSourcePath, virtualDestPath);
        return errorMessage;
    }

    public override string DeleteDirectory(string virtualTargetPath)
    {
        string physicalTargetPath;
        physicalTargetPath = this.GetPhysicalFromVirtualPath(virtualTargetPath);
        if (physicalTargetPath == null)
            return string.Format("The virtual path : '{0}' cannot be converted to a physical path", virtualTargetPath);

        // There is not permission issue with the FileExplorer's permissions ==> Delete can be performed
        // but there can be some FileSystem restrictions 
        string errorMessage = FileSystem.DeleteDirectory(physicalTargetPath, virtualTargetPath);
        return errorMessage;
    }
    public override string DeleteFile(string virtualTargetPath)
    {
        string physicalTargetPath = this.GetPhysicalFromVirtualPath(virtualTargetPath);
        if (physicalTargetPath == null)
            return string.Format("The virtual path :'{0} cannot be converted to a physical path", virtualTargetPath);

        // There is not a permission issue with the FileExplorer's permissions ==> Delete can be performed,
        // but there can be some FileSystem restriction
        string errorMessage = FileSystem.DeleteFile(physicalTargetPath, virtualTargetPath);
        return errorMessage;
    }

    public override string CopyDirectory(string virtualSourcePath, string virtualDestPath)
    {
        string physicalSourcePath = this.GetPhysicalFromVirtualPath(virtualSourcePath);
        if (physicalSourcePath == null)
            return string.Format("The virtual path : '{0}' cannot be converted to a physical path", virtualSourcePath);

        string physicalDestinationPath = this.GetPhysicalFromVirtualPath(virtualDestPath);
        if (physicalDestinationPath == null)
            return string.Format("The virtual path : '{0}' cannot be converted to a physical path", virtualDestPath);

        string newFolderName = physicalDestinationPath + PathHelper.GetDirectoryName(physicalSourcePath);

        // Check whether the folder already exists in the destination folder
        if (Directory.Exists(newFolderName))
        {// Yes the folder exists:
            string message = string.Format("The folder: '{0}{1}' already exists", virtualDestPath, PathHelper.GetDirectoryName(physicalSourcePath));
            return message;
        }

        // A check whether the source directory is parent of the destination directory
        if (PathHelper.IsParentOf(virtualSourcePath, virtualDestPath))
        {
            string message = string.Format("The directory: '{0}' is parent of the '{1}' directory. Operation is canceled!", virtualSourcePath, virtualDestPath);
            return message;
        }

        // FileSystem.CopyDirectory returns a string that contains the error or an empty string
        string errorMessage = FileSystem.CopyDirectory(physicalSourcePath, physicalDestinationPath, virtualSourcePath, virtualDestPath);
        return errorMessage;
    }

    public override string CopyFile(string virtualSourcePath, string virtualDestPath)
    {
        string physicalSourcePath = this.GetPhysicalFromVirtualPath(virtualSourcePath);
        if (physicalSourcePath == null)
            return string.Format("The virtual path: '{0}' cannot be converted to a physical path", virtualSourcePath);

        string physicalDestinationPath = this.GetPhysicalFromVirtualPath(virtualDestPath);
        if (physicalDestinationPath == null)
            return string.Format("The virtual path: '{0}' cannot be converted to a physical path", virtualDestPath);

        // Checks whether the file already exists in the destination folder ;
        if (File.Exists(physicalDestinationPath))
        {// Yes the file exists :
            string message = string.Format("The file: '{0}' already exists. Operation IS canceled!", virtualDestPath);
            return message;
        }

        // There is not a permission issue with the FileExplorer's permissions ==> Copy can be performed,
        // but there can be some FileSystem restrictions 
        string errorMessage = FileSystem.CopyFile(physicalSourcePath, physicalDestinationPath, virtualSourcePath, virtualDestPath);
        return errorMessage;
    }

    public override string CreateDirectory(string virtualTargetPath, string name)
    {
        string physicalTargetPath = this.GetPhysicalFromVirtualPath(virtualTargetPath);
        if (physicalTargetPath == null)
            return string.Format("The virtual path: '{0}' cannot be converted to a physical path", virtualTargetPath);

        string virtualNewFolderPath = PathHelper.AddEndingSlash(virtualTargetPath, '/') + name;
        string physicalNewFolderPath = this.GetPhysicalFromVirtualPath(virtualNewFolderPath);
        if (physicalNewFolderPath == null)
            return string.Format("The virtual path: '{0}'  cannot be converted to a physical path", virtualNewFolderPath);

        if (Directory.Exists(physicalNewFolderPath))
        {
            string error = string.Format("The directory: '{0}' already exists", virtualNewFolderPath); ;
            return error;
        }

        // There is no restriction with the FileExplorer's permissions ==> Create can be performed
        // but there can be some FileSystems restrictions  
        string errorMessage = FileSystem.CreateDirectory(physicalTargetPath, name, virtualTargetPath);
        return errorMessage;
    }

    public override bool CheckWritePermissions(string virtualTargetPath)
    {
        string physicalTargetPath = this.GetPhysicalFromVirtualPath(virtualTargetPath);
        if (physicalTargetPath == null)
            return false;

        // The upload permission is not set ==> no write permission;
        // Also check whether the write is allowed by the filesystem 
        return CheckPermissions(physicalTargetPath, PathPermissions.Upload) && FileSystem.CheckWritePermission(physicalTargetPath, virtualTargetPath);
    }

    public override bool CheckDeletePermissions(string virtualTargetPath)
    {
        string physicalTargetPath = this.GetPhysicalFromVirtualPath(virtualTargetPath);
        if (physicalTargetPath == null)
            return false;

        // The Delete permission is not set ==> no Delete permission;
        // Also check whether the delete permission is allowed by the filesystem 
        return CheckPermissions(physicalTargetPath, PathPermissions.Delete) && FileSystem.CheckWritePermission(physicalTargetPath, virtualTargetPath);
    }

    public override bool CheckReadPermissions(string folderPath)
    {
        string physicalTargetPath = this.GetPhysicalFromVirtualPath(folderPath);
        if (physicalTargetPath == null)
            return false;

        var canRead = CheckPermissions(physicalTargetPath, PathPermissions.Read);

        return canRead;
    }

    private bool CheckPermissions(string folderPath, PathPermissions permToCheck)
    {
        //add a ending slash to the upload folder
        folderPath = folderPath.TrimEnd(PhysicalPathSeparator) + PhysicalPathSeparator;


        string[] pathsToCheck;
        if ((permToCheck & PathPermissions.Upload) != 0)
            pathsToCheck = UploadPaths;
        else if ((permToCheck & PathPermissions.Delete) != 0)
            pathsToCheck = DeletePaths;
        else
            pathsToCheck = ViewPaths;


        //Compare the 'folderPath' to all paths in the 'pathsToCheck' collection and check if it is a child or one of them.
        foreach (string pathToCheck in pathsToCheck)
        {
            if (!String.IsNullOrEmpty(pathToCheck) && folderPath.StartsWith(pathToCheck, StringComparison.OrdinalIgnoreCase))
            {
                // Remove trailing slash from the path
                string trimmedPath = pathToCheck.TrimEnd(PhysicalPathSeparator);
                //if (trimmedPath.Length == 0)
                //{
                //    //Path contains only the Path separator ==> give permissions everywhere
                //    return true;
                //}
                if (folderPath.Length > trimmedPath.Length && folderPath[trimmedPath.Length] == PhysicalPathSeparator)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override string StoreFile(Telerik.Web.UI.UploadedFile file, string path, string name, params string[] arguments)
    {
        string physicalPath = this.GetPhysicalFromVirtualPath(path);
        if (physicalPath == null)
            return string.Empty;

        physicalPath = PathHelper.AddEndingSlash(physicalPath, '\\') + name;
        file.SaveAs(physicalPath);


        // Returns the path to the newly created file
        return PathHelper.AddEndingSlash(path, '/') + name;
    }

    // This function is obsolete ;
    //public override string StoreFile(HttpPostedFile file, string path, string name, params string[] arguments)
    //{
    //    return base.StoreFile(file, path, name, arguments);
    //}

    public override string StoreBitmap(System.Drawing.Bitmap bitmap, string url, System.Drawing.Imaging.ImageFormat format)
    {
        string virtualPath = RemoveProtocolNameAndServerName(url);
        string physicalPath = this.GetPhysicalFromVirtualPath(virtualPath);
        if (physicalPath == null)
            return string.Empty;

        StreamWriter bitmapWriter = StreamWriter.Null;

        try
        {
            bitmapWriter = new StreamWriter(physicalPath);
            bitmap.Save(bitmapWriter.BaseStream, format);
        }
        catch (IOException)
        {
            string errMessage = "The image cannot be stored!";
            return errMessage;
        }
        finally
        {
            bitmapWriter.Close();
        }
        return string.Empty;
    }

    public override string GetFileName(string url)
    {
        string fileName = Path.GetFileName(RemoveProtocolNameAndServerName(url));
        return fileName;
    }

    public override Stream GetFile(string url)
    {
        string virtualPath = RemoveProtocolNameAndServerName(url);
        string physicalPath = this.GetPhysicalFromVirtualPath(virtualPath);
        if (physicalPath == null)
            return null;

        if (!File.Exists(physicalPath))
        {
            return null;
        }

        return File.OpenRead(physicalPath);
    }

    public override bool CanCreateDirectory
    {
        get
        {
            return true;
        }
    }

    public override string GetPath(string path)
    {
        // First add the '~/' signs in order to use the VirtualPathUtility.GetDirectory() method ;
        string PathWithTilde = "~/" + path;
        string virtualPath = VirtualPathUtility.GetDirectory(PathWithTilde);
        virtualPath = virtualPath.Remove(0, 2);// remove the '~' signs

        return virtualPath;
    }

    public override char PathSeparator
    {
        get
        {
            return '/';
        }
    }

    private char PhysicalPathSeparator
    {
        get
        {
            return '\\';
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  The helper methods 
    //////////////////////////////////////////////////////////////////////////////////////////////////////////


    private PathPermissions GetPermissions(string physicalPath)
    {
        PathPermissions permission = PathPermissions.Read;
        permission = CheckPermissions(physicalPath, PathPermissions.Delete) ? permission | PathPermissions.Delete : permission;
        permission = CheckPermissions(physicalPath, PathPermissions.Upload) ? permission | PathPermissions.Upload : permission;

        return permission;
    }


    /// <summary>
    /// Gets the physical path from a virtual one by using the applied mappings and 
    /// returns null if no mappings are found
    /// </summary>
    /// <param name="virtualPath">A virtual path.</param>
    /// <returns> The converted physical path or 'null' if no mapping is found </returns>
    private string GetPhysicalFromVirtualPath(string virtualPath)
    {   // 'virtualPath' contains value similar to:  "/MyCusomRootDir/"

        virtualPath = PathHelper.RemoveEndingSlash(virtualPath, '/');
        string resultPhysicalPath;// Contains the result - physical path

        // Iterates through all mapped directories
        foreach (KeyValuePair<string, string> mappedPath in MappedPaths)
        {
            if (virtualPath.StartsWith(mappedPath.Key, StringComparison.CurrentCultureIgnoreCase))
            {// A mapping is found

                // Replase the virtual root directory with the physical one
                string restOfVirtualPath = virtualPath.Substring(mappedPath.Key.Length);
                restOfVirtualPath = restOfVirtualPath.Replace('/', '\\');
                restOfVirtualPath = PathHelper.AddStartingSlash(restOfVirtualPath, '\\');

                // 'mappedPath.Value' always ends with '\'
                // // The 'restOfVirtualPath' is something like Folder_1/SubFolder_2/ ==> convert it to Folder_1\SubFolder_2\
                resultPhysicalPath = mappedPath.Value + restOfVirtualPath;

                // Break the iteration - a physical path is found
                return resultPhysicalPath;
            }
        }
        // No mapping found
        return null;
    }

    /// <summary>
    /// Returns the files as an array of 'FileItem'
    /// </summary>
    /// <param name="virtualPath">Virtual path to the filder</param>
    /// <returns>An array of 'FileItem'</returns>
    private FileItem[] GetFiles(string virtualPath)
    {
        List<FileItem> filesItems = new List<FileItem>();
        string physicalPath = this.GetPhysicalFromVirtualPath(virtualPath);
        if (physicalPath == null)
            return null;
        List<string> files = new List<string>();// The files in this folder : 'physicalPath' ;

        try
        {
            foreach (string patern in this.SearchPatterns)
            {// Applied flters in the 'SearchPatterns' property;
                foreach (string filePath in Directory.GetFiles(physicalPath, patern))
                {
                    if (!files.Contains(filePath))
                        files.Add(filePath);
                }
            }

            foreach (string filePath in files)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                string url = ItemHandlerPath + "?path=" + virtualPath + fileInfo.Name;
                FileItem fileItem = new FileItem(fileInfo.Name,
                                                    fileInfo.Extension,
                                                    fileInfo.Length,
                                                    string.Empty,
                                                    url,
                                                    null,
                                                    GetPermissions(filePath)
                                                );
                filesItems.Add(fileItem);
            }
        }
        catch (IOException)
        {// The parent directory is moved or deleted

        }

        return filesItems.ToArray();
    }

    /// <summary>
    /// Gets the folders that are contained in a specific virtual directory
    /// </summary>
    /// <param name="virtualPath">The virtual directory that contains the folders</param>
    /// <returns>Array of 'DirectoryItem'</returns>
    private DirectoryItem[] GetDirectories(string virtualPath)
    {
        List<DirectoryItem> directoryItems = new List<DirectoryItem>();
        string physicalPath = this.GetPhysicalFromVirtualPath(virtualPath);
        if (physicalPath == null)
            return null;
        string[] directories;

        try
        {
            directories = Directory.GetDirectories(physicalPath);// Can throw an exeption ;
            foreach (string dirPath in directories)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                string newVirtualPath = PathHelper.AddEndingSlash(virtualPath, '/') + PathHelper.GetDirectoryName(dirPath) + "/";
                DirectoryItem dirItem = new DirectoryItem(PathHelper.GetDirectoryName(dirPath),
                                                            string.Empty,
                                                            newVirtualPath,
                                                            PathHelper.AddEndingSlash(virtualPath, '/'),
                                                            GetPermissions(dirPath),
                                                            GetFiles(virtualPath),
                                                            null
                                                          );
                directoryItems.Add(dirItem);
            }
        }
        catch (IOException)
        {// The parent directory is moved or deleted

        }

        return directoryItems.ToArray();
    }
}

public class PathHelper
{
    /// <summary>
    /// Adds a symbol at the begining of a path if the symblol does not exist
    /// </summary>
    /// <param name="path"></param>
    /// <param name="symbolToAdd"></param>
    /// <returns>The modified path</returns>
    public static string AddStartingSlash(string path, char symbolToAdd)
    {
        if (path.StartsWith(symbolToAdd.ToString()))
        {
            return path;
        }
        else
        {
            return symbolToAdd + path;
        }
    }

    /// <summary>
    /// Removes a symblol from the begining of a path
    /// </summary>
    /// <param name="path"></param>
    /// <param name="symbolToRemove"></param>
    /// <returns></returns>
    /// 
    public static string RemoveStartingSlash(string path, char symbolToRemove)
    {
        if (path.StartsWith(symbolToRemove.ToString()))
        {
            return path.Substring(1);
        }
        else
        {
            return path;
        }
    }


    /// <summary>
    /// Adds a symbol at the end of a path if the symbol does not exist
    /// </summary>
    /// <param name="path"></param>
    /// <param name="symbolToRemove"></param>
    /// <returns>The modified path</returns>
    public static string AddEndingSlash(string path, char symbolToAdd)
    {
        if (path.EndsWith(symbolToAdd.ToString()))
        {
            return path;
        }
        else
        {
            return path + symbolToAdd;
        }
    }

    /// <summary>
    /// Removes a backslash from the end of a path
    /// </summary>
    /// <param name="path"></param>
    /// <returns>The modified path</returns>
    public static string RemoveEndingSlash(string path, char symbolToRemove)
    {
        if (path.EndsWith(symbolToRemove.ToString()))
        {
            return path.Substring(0, path.Length - 1);
        }
        else
        {
            return path;
        }
    }

    /// <summary>
    /// Gets the name of a directory.
    /// Example C:\Folder1\Folder2 ==> the function returns 'Folder2' 
    /// </summary>
    /// <param name="physicalPath"></param>
    /// <returns></returns>
    public static string GetDirectoryName(string physicalPath)
    {
        if (physicalPath.EndsWith("\\"))
        {
            int lastIndexOfSlash = physicalPath.Substring(0, physicalPath.Length - 1).LastIndexOf("\\");

            //if (lastIndexOfSlash == -1)
            //{// If the passsd path is C:\ for example
            //    return string.Empty;
            //}

            string name = physicalPath.Substring(lastIndexOfSlash + 1);
            return name.Replace("\\", "");
        }
        else
        {
            int lastIndexOfSlash = physicalPath.LastIndexOf("\\");
            string name = physicalPath.Substring(lastIndexOfSlash + 1);
            return name;
        }
    }

    /// <summary>
    /// Checks whether the passed path is a physical path 
    /// </summary>
    /// <param name="path">The paths looks likee: 'C:\Path\Dir' </param>
    /// <returns></returns>
    public static bool IsPhysicalPath(string path)
    {
        return path.Contains(@":\");
    }

    /// <summary>
    /// Checks whether the passed path is a shared folder's path 
    /// </summary>
    /// <param name="path">The path looks like: '\\Path\Dir'</param>
    /// <returns></returns>
    public static bool IsSharedPath(string path)
    {
        return path.StartsWith(@"\\"); ;
    }

    /// <summary>
    /// Checks whether a path is child of another path
    /// </summary>
    /// <param name="virtualParent">Should be the virtual parent directory's path</param>
    /// <param name="virtualChild">Should be the virtual child path. This parameter can be a path to file as well</param>
    /// <returns></returns>
    public static bool IsParentOf(string virtualParent, string virtualChild)
    {
        if (virtualChild.Equals(virtualParent, StringComparison.CurrentCultureIgnoreCase))
        {
            return false;
        }

        // else if
        if (virtualChild.StartsWith(virtualParent, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        // else
        return false;
    }
}
public class FileSystem
{
    public FileSystem()
    {
    }

    public static string MoveDirectorty(string physicalSourcePath, string physicalDestPath, string virtualSourcePath, string virtualDestPath)
    {
        try
        {
            if ((PathHelper.IsSharedPath(physicalSourcePath) && PathHelper.IsSharedPath(physicalDestPath))
                                        ||
                 (PathHelper.IsPhysicalPath(physicalSourcePath) && PathHelper.IsPhysicalPath(physicalDestPath))
                )
            {
                Directory.Move(physicalSourcePath, physicalDestPath);
            }
            else
            {
                // When the 'physicalSourcePath' is a shared path and the Directory.Move does not work, and thrown and 
                // exception like: "Source and destination path must have identical roots. Move will not work across volumes."

                // The solution:
                // 1. First copy the directory
                // 2. remove the directory from the old location

                string destinationDirectoryForCopy = Path.GetDirectoryName(physicalDestPath);
                string destVirtualDirForCopy = System.Web.VirtualPathUtility.GetDirectory(PathHelper.AddStartingSlash(virtualDestPath, '/'));
                destVirtualDirForCopy = PathHelper.RemoveStartingSlash(destVirtualDirForCopy, '/');
                destVirtualDirForCopy = PathHelper.RemoveEndingSlash(destVirtualDirForCopy, '/');
                FileSystem.CopyDirectory(physicalSourcePath.ToString(), destinationDirectoryForCopy, virtualSourcePath.ToString(), destVirtualDirForCopy);
                FileSystem.DeleteDirectory(physicalSourcePath, virtualSourcePath);
            }
        }
        catch (DirectoryNotFoundException ex)
        {
            string message = ex.Message;                    // to handle the warning of variable never used
            message = string.Format("One of the directories: '{0}' or '{1}' does not exist!", virtualSourcePath, virtualDestPath);
            return message;
        }
        catch (UnauthorizedAccessException ex)
        {
            string message = ex.Message;                               // to handle the warning of variable never used
            message = "You do not have enough permissions for this operation!";
            return message;
        }
        catch (Exception ex)
        {
            string message = ex.Message;                                 // to handle the warning of variable never used
            message = "The operation cannot be completed";
            return message;
        }

        return string.Empty;
    }

    public static string MoveFile(string physicalSourcePath, string physicalDestPath, string virtualSourcePath, string virtualDestPath)
    {
        try
        {
            File.Move(physicalSourcePath, physicalDestPath);
        }

        catch (FileNotFoundException)
        {
            string message = string.Format("File: '{0}' does not exist!", virtualSourcePath);
            return message;
        }
        catch (UnauthorizedAccessException)
        {
            string message = "FileSystem's restriction: You do not have enough permissions for this operation!";
            return message;
        }
        catch (IOException)
        {
            string message = "The operation cannot be compleated";
            return message;
        }

        return string.Empty;
    }

    public static string DeleteDirectory(string physicalTargetPath, string virtualTargetPath)
    {
        try
        {
            Directory.Delete(physicalTargetPath, true);
        }
        catch (DirectoryNotFoundException)
        {
            string message = string.Format("FileSystem restriction: Directory '{0}' is not found!", virtualTargetPath);
            return message;
        }
        catch (UnauthorizedAccessException)
        {
            string message = "FileSystem's restriction: You do not have enough permissions for this operation!";
            return message;
        }
        catch (IOException)
        {
            string message = string.Format("FileSystem restriction: The directory '{0}' cannot be deleted!", virtualTargetPath);
            return message;
        }

        return string.Empty;
    }

    public static string DeleteFile(string physicalTargetPath, string virtualTargetPath)
    {
        try
        {
            File.Delete(physicalTargetPath);
        }

        catch (FileNotFoundException)
        {
            string message = string.Format("File: '{0}' does not exist!", virtualTargetPath);
            return message;
        }
        catch (UnauthorizedAccessException)
        {
            string message = "FileSystem restriction: You do not have enough permissions for this operation!";
            return message;
        }
        catch (IOException)
        {
            string message = "The operation cannot be compleated";
            return message;
        }

        return string.Empty;
    }

    public static string CopyFile(string physicalSourcePath, string physicalDestPath, string virtualSourcePath, string virtualDestPath)
    {
        try
        {
            File.Copy(physicalSourcePath, physicalDestPath, true);
        }

        catch (FileNotFoundException)
        {
            string message = string.Format("File: '{0}' does not exist!", virtualSourcePath);
            return message;
        }
        catch (UnauthorizedAccessException)
        {
            string message = "FileSystem's restriction: You do not have enough permissions for this operation!";
            return message;
        }
        catch (IOException)
        {
            string message = "The operation cannot be compleated";
            return message;
        }

        return string.Empty;
    }

    public static string CopyDirectory(string physycalSourcePath, string physicalDestPath, string virtualSourcePath, string virtualDestPath)
    {
        // Copy all files ;
        string newDirPhysicalFullPath;// Contains the physical path to the new directory ;
        DirectoryInfo dirInfoSource;
        try
        {
            dirInfoSource = new DirectoryInfo(physycalSourcePath);
            newDirPhysicalFullPath = string.Format("{0}{1}{2}", PathHelper.AddEndingSlash(physicalDestPath, '\\'), dirInfoSource.Name, "\\");

            // Else ;
            Directory.CreateDirectory(newDirPhysicalFullPath, dirInfoSource.GetAccessControl());
        }
        catch (UnauthorizedAccessException ex)
        {
            string message = ex.Message;                  // to avoid build warning (variable not used)
            message = "FileSystem's restriction: You do not have enough permissions for this operation!";
            return message;
        }

        // Directory is created ;

        foreach (string currentFilePath in Directory.GetFiles(physycalSourcePath))
        {
            FileInfo fileInfo = new FileInfo(currentFilePath);

            string newFilePath = newDirPhysicalFullPath + fileInfo.Name;

            try
            {
                File.Copy(currentFilePath, newFilePath);
            }

            catch (FileNotFoundException ex)
            {
                string message = ex.Message;                            // to avoid build warning (variable not used)
                message = string.Format("File: '{0}' does not exist!", virtualSourcePath);
                return message;
            }
            catch (UnauthorizedAccessException ex)
            {
                string message = ex.Message;                // to avoid build warning (variable not used)
                message = "You do not have enough permissions for this operation!";
                return message;
            }
            catch (IOException ex)
            {
                string message = ex.Message;                  // to avoid build warning (variable not used)
                message = "The operation cannot be compleated";
                return message;
            }
        }

        // Copy all subdirectories ;
        foreach (string physicalCurrentSourcePath in Directory.GetDirectories(physycalSourcePath))
        {
            DirectoryInfo dirInfo = new DirectoryInfo(physicalCurrentSourcePath);
            string physicalCurrentDestPath = newDirPhysicalFullPath;// Change the name of the variable ;
            string virtualCurrentSourcePath = string.Format("{0}{1}{2}", PathHelper.AddEndingSlash(virtualSourcePath, '/'), dirInfo.Name, "/");
            string virtualCurrentDestPath = string.Format("{0}{1}{2}", PathHelper.AddEndingSlash(virtualDestPath, '/'), dirInfoSource.Name, "/");

            // Call recursively the Directory copy function ;
            string returnedError = CopyDirectory(physicalCurrentSourcePath, physicalCurrentDestPath, virtualCurrentSourcePath, virtualCurrentDestPath);
            if (returnedError != string.Empty)
            {// An error occured ;
                return returnedError;
            }
        }

        // No errors. 
        return string.Empty;
    }

    public static string CreateDirectory(string physicalTargetPath, string directoryName, string virtualTargetPath)
    {
        try
        {
            DirectoryInfo parentDir = new DirectoryInfo(physicalTargetPath);

            Directory.CreateDirectory(PathHelper.AddEndingSlash(physicalTargetPath, '\\') + directoryName, parentDir.GetAccessControl());
        }
        catch (DirectoryNotFoundException)
        {
            string message = string.Format("FileSystem restriction: Directory with name '{0}' is not found!", virtualTargetPath);
            return message;
        }
        catch (UnauthorizedAccessException)
        {
            string message = "FileSystem's restriction: You do not have enough permissions for this operation!";
            return message;
        }
        catch (IOException)
        {
            string message = string.Format("FileSystem restriction: The directory '{0}' cannot be created!", virtualTargetPath);
            return message;
        }

        return string.Empty;
    }

    public static byte[] GetFileContent(string physicalTargetPath, string virtualTargetPath)
    {
        FileStream fileStream = new FileStream(physicalTargetPath, FileMode.Open, FileAccess.Read);
        byte[] content = new byte[fileStream.Length];
        fileStream.Read(content, 0, (int)fileStream.Length);
        fileStream.Close();

        return content;
    }

    public static bool CheckWritePermission(string physicalTargetPath, string virtualTargetPath)
    {
        FileIOPermission f = new FileIOPermission(FileIOPermissionAccess.Write, physicalTargetPath);
        try
        {
            f.Demand();
            return true;
        }
        catch (SecurityException)
        {
            return false;
        }
    }
}
