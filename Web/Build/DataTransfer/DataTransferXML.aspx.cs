using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DataTransfer;

public partial class DataTransfer_DataTransferXML : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string currentpath = Request.QueryString["currentpath"].ToString();
        if (!IsPostBack)
            BindData();
    }

    protected void gvXml_RowCommand(object sender, GridViewCommandEventArgs gvce)
    {
        if (gvce.CommandName.ToUpper().Equals("IMPORT"))
        {
            string currentpath = Request.QueryString["currentpath"].ToString();
            string filename = currentpath.Substring(currentpath.LastIndexOf(@"\")+1);
            string tablename = filename.Split('_')[0];

            int currentrow = int.Parse(gvce.CommandArgument.ToString());
            PhoenixDataTransferImport.DataTransferImportDataRow(tablename, currentpath, currentrow);
            
        }
    }
    private void BindData()
    {        
        string currentpath = Request.QueryString["currentpath"].ToString().Replace("/", "\\").Replace("//", "\\\\");
        DataSet ds = PhoenixDataTransferImport.DataSynchronizerXmlViewer(currentpath);
        gvXml.DataSource = ds;
        gvXml.DataBind();
    }
}
