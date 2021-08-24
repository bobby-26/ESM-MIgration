using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;
using System.Text.RegularExpressions;


public partial class DocumentManagementDocumentSectionRevisonGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        // cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Close", "CLOSE");

        if (Request.QueryString["REVISONID"] != null && Request.QueryString["REVISONID"].ToString() != "")
            ViewState["REVISONID"] = Request.QueryString["REVISONID"].ToString();
        else
            ViewState["REVISONID"] = "";

        //if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "")
        //{
        //    ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
        //    MenuClose.AccessRights = this.ViewState;
        //    MenuClose.MenuList = toolbar.Show();
        //}
        //else
        //    ViewState["callfrom"] = "";
        GetData();

    }

    private void GetData()
    {
        DataSet ds = PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonsEdit(new Guid(ViewState["REVISONID"].ToString()));
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["DOCUMENTID"] = dr["FLDDOCUMENTID"].ToString();
            ViewState["SECTIONID"] = dr["FLDSECTIONID"].ToString();

            //string strResult = HttpUtility.HtmlDecode(dr["FLDTEXT"].ToString());
            //String NewLine = System.Environment.NewLine;            
            //form1.Style.Add("font-family:", "Times New Roman");

            //span1.Style.Add("font-size:", "25");
            //span1.Style.Add("margin-top:", "0");
            //span1.Style.Add("margin-left:", "10");
            //span1.Style.Add("margin-right:", "10");   

            string strResult = @"<font size=""2"">" + HttpUtility.HtmlDecode(dr["FLDTEXT"].ToString()) + "</font>";
            string str = @"<object id=""ieooui"" classid=""clsid:38481807-CA0E-42D2-BF39-B33AF135CC4D"" />";
            strResult = strResult.Replace(str, "");
            str = @"</object>";
            strResult = strResult.Replace(str, "");

                    string repl = "../Common/Download.aspx?dtkey=";

                    string find2 = "../attachments/DOCUMENTMANAGEMENT/";

                    string findOffshore = "https://apps.southnests.com/PhoenixOffshore/attachments/DOCUMENTMANAGEMENT/";

                    string findOffshore2 = "http://119.81.76.123:80/PhoenixOffshore/attachments/DOCUMENTMANAGEMENT/";

                    string findOffshore3 = "http://119.81.76.123/PhoenixOffshore/attachments/DOCUMENTMANAGEMENT/";

                    string find = "https://apps.southnests.com/Phoenix/attachments/DOCUMENTMANAGEMENT/";

                    string find3 = "http://119.81.76.123:80/Phoenix/attachments/DOCUMENTMANAGEMENT/";

                    string find4 = "http://119.81.76.123/Phoenix/attachments/DOCUMENTMANAGEMENT/";

                    if (strResult.Contains(find2))
                    {
                        strResult = strResult.Replace(find2, repl);
                    }


                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                    {

                        if (strResult.Contains(findOffshore))
                        {
                            strResult = strResult.Replace(findOffshore, repl);
                        }

                        if (strResult.Contains(findOffshore2))
                        {
                            strResult = strResult.Replace(findOffshore, repl);
                        }

                        if (strResult.Contains(findOffshore3))
                        {
                            strResult = strResult.Replace(findOffshore3, repl);
                        }
                    }
                    else
                    {
                        if (strResult.Contains(find))
                        {
                            strResult = strResult.Replace(find, repl);
                        }

                        if (strResult.Contains(find3))
                        {
                            strResult = strResult.Replace(find3, repl);
                        }

                        if (strResult.Contains(find4))
                        {
                            strResult = strResult.Replace(find4, repl);
                        }

                    }

                    strResult = Regex.Replace(strResult, "\\d+/\\d+/", "");

                    strResult = strResult.Replace(".jpg", "");
                    strResult = strResult.Replace(".png", "");
                    strResult = strResult.Replace(".tif", "");

            span1.InnerHtml = span1.InnerHtml + @"<table width=""100%""><tr><td align=""left""><h2>" + HttpUtility.HtmlDecode(dr["FLDSECTIONDETAILS"].ToString()) + @"</h2></td><td align=""right"" ><h2>" + HttpUtility.HtmlDecode(dr["FLDREVISIONDETAILS"].ToString()) + "</h2></td></tr></table>";
            span1.InnerHtml = span1.InnerHtml + strResult;
            span1.InnerHtml = span1.InnerHtml + "<br/>";
        }
    }

    protected void MenuClose_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("CLOSE"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionRevisonList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString() + "&REVISONID=" + ViewState["REVISONID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
