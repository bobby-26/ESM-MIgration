using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
public partial class Accounts_AccountsInvoiceReconcilationComments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Comments", "COMMENTS",ToolBarDirection.Right);
        toolbarmain.AddButton("Form", "FORM", ToolBarDirection.Right);

        MenuMainFilter.AccessRights = this.ViewState;
        MenuMainFilter.Title = "Comments";
        MenuMainFilter.MenuList = toolbarmain.Show();


        PhoenixToolbar toolbarsave = new PhoenixToolbar();
        toolbarsave.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuSaveFilter.AccessRights = this.ViewState;
        MenuSaveFilter.MenuList = toolbarsave.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["Invoicecode"] = null;
            if (Request.QueryString["frmreport"] == null)
                ViewState["pageno"] = Request.QueryString["pageno"].ToString();

        }
        if (Request.QueryString["INVOICECODE"].ToString() != null)
        {
            ViewState["Invoicecode"] = Request.QueryString["INVOICECODE"];
        }
        MenuMainFilter.SelectedMenuIndex = 0;
        BindData();
    }

    protected void MenuMainFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FORM"))
            {
                if (ViewState["Invoicecode"].ToString() == "0")
                    Response.Redirect("../Accounts/AccountsInvoiceAdjustment.aspx?pageno=" + ViewState["pageno"].ToString());
                else
                    Response.Redirect("../Accounts/AccountsInvoiceAdjustment.aspx?qinvoicecode=" + ViewState["Invoicecode"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidComments()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtNotesDescription.Text == null || txtNotesDescription.Text == "")
            ucError.ErrorMessage = "Please enter comments";

        return (!ucError.IsError);
    }

    private void InsertComments()
    {
        if (!IsValidComments())
        {
            ucError.Visible = true;
            return;
        }

        PhoenixInvoiceComments.InsertComments(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                General.GetNullableGuid(ViewState["Invoicecode"].ToString()),
                                                txtNotesDescription.Text);

        BindData();

    }
    protected void MenuSaveFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                InsertComments();
            }

            txtNotesDescription.Text = "";
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDPOSTEDBY", "FLDREMARKS", "FLDPOSTEDDATE" };
        string[] alCaptions = { "Posted By", "Comments", "Posted Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixInvoiceComments.CommentsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              General.GetNullableGuid(ViewState["Invoicecode"].ToString()),
           sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvInvoiceRemarks.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvInvoiceRemarks", "Comments", alCaptions, alColumns, ds);


        gvInvoiceRemarks.DataSource = ds;
        gvInvoiceRemarks.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void MenuInvoiceRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDPOSTEDBY", "FLDREMARKS", "FLDPOSTEDDATE" };
        string[] alCaptions = { "Posted By", "Comments", "Posted Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInvoiceComments.CommentsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              General.GetNullableGuid(ViewState["Invoicecode"].ToString()),
           sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Comments.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Comments</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void gvInvoiceRemarks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInvoiceRemarks.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
