using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class DocumentManagement_DocumentManagementDocumentSectionRevisionDetailsOfChange : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["FLDSECTIONID"] != null && Request.QueryString["FLDSECTIONID"].ToString() != "")
                ViewState["FLDSECTIONID"] = Request.QueryString["FLDSECTIONID"].ToString();
            else
                ViewState["FLDSECTIONID"] = "";

            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "")
            {
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
                ViewState["showall"] = 0;
            }
            else
            {
                ViewState["showall"] = 1;
            }
            GetData();
        }
    }

    private void GetData()
    {
        if (ViewState["FLDSECTIONID"] != null && ViewState["FLDSECTIONID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisionData(new Guid(ViewState["FLDSECTIONID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0 )     //to bind document sections data
            {
                repRevision.DataSource = ds.Tables[0];
                repRevision.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                repRevision.DataSource = dt;
                repRevision.DataBind();
                tblFooter.Visible = true;
            }
        }
    }

    protected void repRevision_PreRender(object sender, EventArgs e)
    {
        int i = 0;
        for (i = repRevision.Items.Count - 1; i > 0; i--)
        {
            HtmlTableCell oCell_previous = repRevision.Items[i - 1].FindControl("tdDocumentRevision") as HtmlTableCell;
            HtmlTableCell oCell = repRevision.Items[i].FindControl("tdDocumentRevision") as HtmlTableCell;
            oCell.RowSpan = (oCell.RowSpan == -1) ? 1 : oCell.RowSpan;
            oCell_previous.RowSpan = (oCell_previous.RowSpan == -1) ? 1 : oCell_previous.RowSpan;
            if (oCell.InnerText == oCell_previous.InnerText)
            {
                oCell.Visible = false;
                oCell_previous.RowSpan += oCell.RowSpan;
            }
        }
    }     
}
