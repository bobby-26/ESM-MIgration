using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using System.Net;
using System.Xml;

public partial class DefectTrackerAttachmentExportList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Data", "DATA");
        toolbar.AddButton("Attachment", "ATTACHMENT");
        MenuEDMSExport.AccessRights = this.ViewState;
        MenuEDMSExport.MenuList = toolbar.Show();
        MenuEDMSExport.SelectedMenuIndex = 1;

        BindData();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        string xml = "function=GetAttachmentExportList";
        string phoenixurl = ConfigurationManager.AppSettings.Get("phoenixurl").ToString();
        string url = phoenixurl + "Options/OptionsAuthenticateUser.aspx";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

        byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(xml);
        req.Method = "POST";
        req.ContentType = "text/xml;charset=utf-8";
        req.ContentLength = requestBytes.Length;
        Stream requestStream = req.GetRequestStream();
        requestStream.Write(requestBytes, 0, requestBytes.Length);
        requestStream.Close();

        HttpWebResponse res = (HttpWebResponse)req.GetResponse();
        StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);

        string response = sr.ReadToEnd();
        DataSet ds = new DataSet();

        StringReader stringReader = new StringReader(response);
        ds.ReadXml(stringReader);
        if ((ds.Tables.Count > 0))
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDataExport.DataSource = ds;
                gvDataExport.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                ShowNoRecordsFound(dt, gvDataExport);
            }
        }
        res.Close();
    }

    protected void MenuEDMSExport_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName == "DATA")
        {
            Response.Redirect("../DefectTracker/DefectTrackerDataExportList.aspx");
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
}
