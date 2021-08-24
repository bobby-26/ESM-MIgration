using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DataTransfer;
using SouthNests.Phoenix.Framework;


public partial class DataTransferFolders : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Pending Imports", "PENDINGIMPORTS");
        toolbar.AddButton("Export Folders", "EXPORTFOLDERS");
        toolbar.AddButton("Import Folders", "IMPORTFOLDERS");

        MenuFolderList.MenuList = toolbar.Show();
        BindData();    
    }

    private void BindData()
    {
        if (Request.QueryString["atdtoption"].ToString().Equals("DT"))
        {
            if (Request.QueryString["folderoption"] != null && Request.QueryString["folderoption"].ToString().Equals("EXPORT"))
            {
                DataSet ds = PhoenixDataTransferImport.DataTransferExportFolders(short.Parse(Request.QueryString["vesselid"].ToString()));
                gvFolderList.DataSource = ds;
                gvFolderList.DataBind();
            }
            else
            {
                DataSet ds = PhoenixDataTransferImport.DataTransferImportFolders(short.Parse(Request.QueryString["vesselid"].ToString()));
                gvFolderList.DataSource = ds;
                gvFolderList.DataBind();
            }
        }
        else
        {
            if (Request.QueryString["folderoption"] != null && Request.QueryString["folderoption"].ToString().Equals("EXPORT"))
            {
                DataSet ds = PhoenixDataTransferImport.AttachmentTransferExportFolders(short.Parse(Request.QueryString["vesselid"].ToString()));
                gvFolderList.DataSource = ds;
                gvFolderList.DataBind();
            }
            else
            {
                DataSet ds = PhoenixDataTransferImport.AttachmentTransferImportFolders(short.Parse(Request.QueryString["vesselid"].ToString()));
                gvFolderList.DataSource = ds;
                gvFolderList.DataBind();
            }
        }
    }

    protected void gvFolderList_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void gvFolderList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string foldername = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFolderName")).Text;
                string atdtoption = Request.QueryString["atdtoption"].ToString();
                Response.Redirect("DataTransferXmlFiles.aspx?atdtoption=" + atdtoption + "&vesselid=" + Request.QueryString["vesselid"].ToString() + "&foldername=" + foldername);
            }
            if (e.CommandName.ToUpper().Equals("ARCHIVE") || e.CommandName.ToUpper().Equals("RESTORE"))
            {
                string sourcedirname = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFolderFullName")).Text;
                string destdirname = GetDestinationDirectoryName(sourcedirname);
                PhoenixDataTransferImport.DataTransferArchiveImportFolder(sourcedirname, destdirname);
            }
            BindData();  
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void FolderList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if ((dce.CommandName.ToUpper().Equals("PENDINGIMPORTS")))
        {
            string atdtoption = Request.QueryString["atdtoption"].ToString();
            Response.Redirect("DataTransferZipFiles.aspx?atdtoption="+atdtoption+"&vesselid=" + Request.QueryString["vesselid"].ToString(), false);
        }
        if ((dce.CommandName.ToUpper().Equals("EXPORTFOLDERS")))
        {
            string atdtoption = Request.QueryString["atdtoption"].ToString();
            Response.Redirect("DataTransferFolders.aspx?atdtoption=" + atdtoption + "&folderoption=EXPORT&vesselid=" + Request.QueryString["vesselid"].ToString(), false);
        }
        if ((dce.CommandName.ToUpper().Equals("IMPORTFOLDERS")))
        {
            string atdtoption = Request.QueryString["atdtoption"].ToString();
            Response.Redirect("DataTransferFolders.aspx?atdtoption=" + atdtoption + "&folderoption=IMPORT&vesselid=" + Request.QueryString["vesselid"].ToString(), false);
        }
    }

    private string GetDestinationDirectoryName(string sourcedirname)
    {
        string filename = "";
        string pathname = "";

        pathname = sourcedirname.Substring(0, sourcedirname.LastIndexOf('\\') + 1);
        filename = sourcedirname.Substring(sourcedirname.LastIndexOf('\\') + 1);

        if (filename.Contains("__ARCHIVE__"))
            filename = filename.Substring(11);
        else
            filename = "__ARCHIVE__" + filename;

        return pathname + filename;
    }
}
