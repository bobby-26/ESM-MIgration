using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DataTransfer;

public partial class DataTransferXmlFiles : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Import Folders", "FOLDERS");
        MenuFolderList.MenuList = toolbar.Show();

        DataSet ds = null;
        if (Request.QueryString["atdtoption"].ToString().Equals("DT"))
        {
            ds = PhoenixDataTransferImport.DataTransferImportFiles(
                short.Parse(Request.QueryString["vesselid"].ToString()),
                Request.QueryString["foldername"].ToString());
        }
        else
        {
            ds = PhoenixDataTransferImport.AttachmentTransferImportFiles(
                short.Parse(Request.QueryString["vesselid"].ToString()),
                Request.QueryString["foldername"].ToString());
        }
        gvFolderList.DataSource = ds;
        gvFolderList.DataBind();
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
                Response.Redirect("DataTransferXmlFiles.aspx?atdtoption="+atdtoption+"&vesselid=" + Request.QueryString["vesselid"].ToString() + "&foldername=" + foldername);
            }

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

        if ((dce.CommandName.ToUpper().Equals("FOLDERS")))
        {
            string atdtoption = Request.QueryString["atdtoption"].ToString();
            Response.Redirect("DataTransferFolders.aspx?atdtoption="  + atdtoption + "&vesselid=" + Request.QueryString["vesselid"].ToString());
        }
    }

}
