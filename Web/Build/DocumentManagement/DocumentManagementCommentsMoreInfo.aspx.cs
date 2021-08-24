using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagement_DocumentManagementCommentsMoreInfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["CommentsId"] = Request.QueryString["CommentsId"];
                BindData();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        DataSet ds = PhoenixDocumentManagementDocument.CommentsMoreInfo(new Guid(ViewState["CommentsId"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblArchivedBy.Text = dr["FLDARCHIVEDBY"].ToString();
            lblArchivedOn.Text = dr["FLDARCHIVEDDATE"].ToString();
          
        }
    }
}
