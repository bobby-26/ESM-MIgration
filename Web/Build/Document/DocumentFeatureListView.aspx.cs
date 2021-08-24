using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Document; 


public partial class DocumentFeatureListView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
           
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Close", "CLOSE");

            //MenuClose.AccessRights = this.ViewState;
            //MenuClose.MenuList = toolbar.Show();                       

            
            GetData();
        }
    }

    private void GetData()
    {
        DataSet ds = PhoenixDocumentFeatureList.MenuFeatureList();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            span1.InnerHtml = span1.InnerHtml + @"<table width=""100%""><tr><td align =""left"">";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //DataRow drr = ds.Tables[0].Rows[i];

                if (dr["FLDPARENTVALUE"].ToString() != null && dr["FLDPARENTVALUE"].ToString() != "")
                {
                    span1.InnerHtml = span1.InnerHtml + @"<font size=""6"" face =""BernhardMod BT ""><ul type = ""Square""><li style=""font-size: 12pt"">" + HttpUtility.HtmlDecode(dr["FLDFEATURE"].ToString()) + @"</li></ul></font>";
                }
                else
                {
                    //span1.InnerHtml = span1.InnerHtml + "<br/>";
                    span1.InnerHtml = span1.InnerHtml + @"</td></tr></table><table width=""100%""><tr><td align=""left""><h2>" + HttpUtility.HtmlDecode(dr["FLDFEATURE"].ToString()) + @"</h2></td></tr><tr><td>";
                    //span1.InnerHtml = span1.InnerHtml + "<br/>";
                }
            
            }
            span1.InnerHtml = span1.InnerHtml + @"</td></tr></table>";
                
            //span1.InnerHtml = span1.InnerHtml + "<br/>";

            //string strResult = HttpUtility.HtmlDecode(dr["FLDTEXT"].ToString());
            //String NewLine = System.Environment.NewLine;
            //span1.InnerHtml = NewLine + strResult;
            //form1.Style.Add("font-family:", "Times New Roman");

            //span1.Style.Add("font-size:", "25");
            //span1.Style.Add("margin-top:", "0");
            //span1.Style.Add("margin-left:", "10");
            //span1.Style.Add("margin-right:", "10");
        }
    }

    protected void MenuClose_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("CLOSE"))
            {                
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuFind_TabStripCommand(object sender, EventArgs e)
    {
 
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;        
    }
}
