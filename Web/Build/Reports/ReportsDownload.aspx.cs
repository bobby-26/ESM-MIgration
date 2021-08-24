using System;
using System.IO;


public partial class ReportsDownload : System.Web.UI.Page
{
     protected void Page_Load(object sender, EventArgs e)
    {
         if (!IsPostBack)
        {
            if (Request.QueryString["filename"] != null)
            {
                FileDownload(Request.QueryString["filename"].ToString(),Request.QueryString["type"].ToString());
            }
        }      

    } 
 
    private void FileDownload(string filename,string type)
    {
        if (type.Equals("pdf"))
        {
            Response.ContentType = "Application/pdf";

            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filename));

            Response.WriteFile(filename);

            Response.End();
        }
        else if (type.Equals("word"))
        {
            Response.ContentType = "Application/msword";

            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filename));

            Response.WriteFile(filename);

            Response.End();
        }
        else
        {
            Response.ContentType = "Application/x-excel";

            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filename));

            Response.WriteFile(filename);

            Response.End();
        }
      
    } 
         
}
    

