using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using Telerik.Web.UI;

public partial class DocumentManagementFMSMailRead_MailRead : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuMailRead.AccessRights = this.ViewState;
        MenuMailRead.MenuList = toolbar.Show();

        Guid mailid = new Guid(Request.QueryString["mailid"].ToString());
        ViewState["mailid"] = mailid;

        if (!IsPostBack)
        {
            BindFileNos();
            ShowMail(mailid);
        }
    }

    private void BindFileNos()
    {
        DataSet ds = PhoenixDocumentManagementFMSReports.FMSMailFilenoList();
        ddlfileno.DataSource = ds;
        ddlfileno.DataBind();
    }

    protected void MenuMailRead_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["mailid"] != null)
                    Response.Redirect("../DocumentManagement/DocumentManagementFMSMailList.aspx?callfrom=1", false);

            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string filnolist = GetCsvValue(ddlfileno);
                PhoenixDocumentManagementFMSReports.FMSFileNoUpdate(General.GetNullableString(filnolist),General.GetNullableGuid(ViewState["mailid"].ToString()));

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    private void SetCsvValue(RadComboBox radComboBox, string csvValue)
    {
        foreach (RadComboBoxItem item in radComboBox.Items)
        {
            item.Checked = false;
            if (csvValue.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }

    private void ShowMail(Guid mailid)
    {
        DataTable dt = PhoenixDocumentManagementFMSReports.FMSMailEdit(mailid);
        DataRow dr = dt.Rows[0];

        txtFrom.Text = dr["FLDFROM"].ToString();
        txtCc.Text = dr["FLDCC"].ToString();
        txtTo.Text = dr["FLDTO"].ToString();
        txtMessage.Text= dr["FLDBODY"].ToString();
        txtSubject.Text = dr["FLDSUBJECT"].ToString();
        ViewState["MAILID"] = dr["FLDMAILID"].ToString();
        //txtFileNumber.Text = dr["FLDKEYWORD"].ToString();
        txtreceivedon.Text = dr["FLDRECEIVEDON"].ToString();
        SetCsvValue(ddlfileno, dr["FLDFILENO"].ToString());

        BindAttachmentData();
    }

    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindAttachmentData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindAttachmentData()
    {
        DataTable dt = new DataTable();

        if (Request.QueryString["sentitem"] == null)
            dt = PhoenixDocumentManagementFMSReports.FMSMailAttachmentEdit(General.GetNullableGuid(ViewState["MAILID"].ToString()));

        gvAttachment.DataSource = dt;

    }

    protected void gvAttachment_RowDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
            if (!string.IsNullOrEmpty(lblFileName.Text))
            {
                RadLabel dtkey = (RadLabel)e.Item.FindControl("lblDTKey");
                HyperLink hlnkfilename = (HyperLink)e.Item.FindControl("lnkfilename");
                if (hlnkfilename != null)
                {
                    hlnkfilename.NavigateUrl = "../DocumentManagement/FMSDownload.aspx?dtkey=" + dtkey.Text;                   
                }
            }
        }
    }

    protected void gvAttachment_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {           
            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {               
                
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ddlfileno_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox ddl = (RadComboBox)sender;
        int iRowCount = 0;
        int itemOffset = e.NumberOfItems;
        int count = 0;
        DataSet ds = PhoenixDocumentManagementFMSReports.FMSMailFilenoList();

        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            foreach (DataRow dr in dt.Rows)
            {
                ddl.Items.Add(new RadComboBoxItem(dr["FLDNAME"].ToString(), dr["FLDFMSMAILFILENOID"].ToString()));
            }
        }
        ddl.DataSource = ds;
        ddl.DataBind();
    }
}
