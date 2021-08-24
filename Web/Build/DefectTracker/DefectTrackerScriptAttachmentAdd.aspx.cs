using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerScriptAttachmentAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["SERVERLIST"] = "";
            PhoenixToolbar toolbaredit = new PhoenixToolbar();
            toolbaredit.AddButton("Save", "SAVE");
            MenuDefectTrackerScriptAdd.AccessRights = this.ViewState;
            MenuDefectTrackerScriptAdd.MenuList = toolbaredit.Show();
            BindDeploymentServer();
        }
    }

    protected void BindDeploymentServer()
    {
        DataTable dt = new DataTable();
        dt = PhoenixDefectTracker.DeploymentServerList();

        if (dt.Rows.Count > 0)
        {
            chklstDeploymentServer.DataSource = dt;
            chklstDeploymentServer.DataValueField = "FLDSERVERID";
            chklstDeploymentServer.DataValueField = "FLDSERVERNAME";
            chklstDeploymentServer.DataBind();
        }
    }

    protected void chklstDeploymentServer_TextChanged(object sender, EventArgs e)
    {
        string serverlist = "";

        foreach (ListItem item in chklstDeploymentServer.Items)
        {
            if (item.Selected)
            {
                if (serverlist == "")
                    serverlist = serverlist + (item.Text);
                else
                    serverlist = serverlist + "," + (item.Text);
            }
        }
        ViewState["SERVERLIST"] = serverlist;
    }
    private bool IsValidValues(string subject, string createdby, string module, string deployon, string filename)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (subject == "")
            ucError.ErrorMessage = "Subject is required";

        if (createdby == "")
            ucError.ErrorMessage = "Createdby is required";

        if (module == "")
            ucError.ErrorMessage = "Module is required";

        if (deployon == "")
            ucError.ErrorMessage = "Database name is required";

        if (filename == "")
            ucError.ErrorMessage = " File is required";

        return (!ucError.IsError);
    }
    protected void DefectTrackerScriptAdd_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            HttpFileCollection postedFiles = Request.Files;
            if (Request.Files["filPatchAttachment"].ContentLength > 0)
            {
                string origpath = HttpContext.Current.Request.MapPath("~/");
                string path = "";

                for (int i = 0; i < postedFiles.Count; i++)
                {
                    HttpPostedFile postedFile = postedFiles[i];
                    path = origpath + "Attachments\\Scripts";

                    if (postedFile.ContentLength > 0)
                    {
                        string orginalfilename = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1);

                        if (postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')) != ".zip")
                        {
                            ucError.ErrorMessage = "Upload file with .zip extension.";
                            ucError.Visible = true;
                            break;
                        }
                        if (File.Exists(path + "\\" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1)))
                        {
                            ucError.ErrorMessage = "The file already exists. Cannot upload file with same name.";
                            ucError.Visible = true;
                            break;
                        }
                        string modulename = ucModule.ModuleName;
                        string filename = "Phoenix1.0.Script." + modulename + "." + DateTime.Now.ToShortDateString().Replace("/", "") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + ".zip";
                        postedFile.SaveAs(path + "\\" + filename);

                        if (!IsValidValues(txtSubject.Text, txtCreatedby.Text, modulename, ViewState["SERVERLIST"].ToString(), filename))
                        {
                            ucError.Visible = true;
                            break;
                        }
                        PhoenixDefectTracker.ScriptAttachmentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            txtSubject.Text, filename, path, int.Parse(ucModule.SelectedValue), orginalfilename, ViewState["SERVERLIST"].ToString(), txtCreatedby.Text);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1');", true);
                    }
                }
            }
        }
    }
}
