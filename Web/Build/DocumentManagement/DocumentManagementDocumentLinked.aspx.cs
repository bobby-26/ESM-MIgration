using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Integration;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class DocumentManagementDocumentLinked : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["FORMID"] = "";

            if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != "")
                ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();

            BindDocumentList();
        }
    }
    protected void BindDocumentList()
    {
        DataSet dss = PhoenixDocumentManagementQuestion.DocumentList(new Guid(ViewState["FORMID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblDocuments.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDSECTIONID"].ToString() + "');return false;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tblDocuments.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblDocuments.Rows.Add(tr);
                number = number + 1;
            }
        }
    }
}