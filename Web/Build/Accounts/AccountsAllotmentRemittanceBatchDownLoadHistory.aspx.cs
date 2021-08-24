using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections;
using System.IO;
using System.Web;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceBatchDownLoadHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("List", "BATCHLIST");
            toolbargrid.AddButton("Line Item", "LINEITEM");
            toolbargrid.AddButton("History", "HISTORY");
            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() == "1")
            {
                toolbargrid.AddButton("Tax and Charges", "TAXANDCHARGES");
            }
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbargrid.Show();
            MenuOrderFormMain.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["Batchid"] = null;

                if (Request.QueryString["Batchid"] != null)
                {
                    ViewState["Batchid"] = Request.QueryString["Batchid"].ToString();
                }
                BatchEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BatchEdit()
    {
        DataSet ds = new DataSet();
        ds = PhoenixAccountsAllotmentRemittance.RemittanceBatchEdit(new Guid(ViewState["Batchid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtAccountCode.Text = ds.Tables[0].Rows[0]["FLDBANKACCOUNT"].ToString();
            txtBatchNo.Text = ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString();
            txtpaydate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDPAYMENTDATE"].ToString());
            txtPaymentMode.Text = ds.Tables[0].Rows[0]["FLDPAYMENTMODENAME"].ToString();
        }
    }
  

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alCaptions = { "Batch Number", "Payment date", "Payment mode", "Account Code" };
        string[] alColumns = { "FLDBATCHNUMBER", "FLDPAYMENTDATE", "FLDPAYMENTMODENAME", "FLDBANKACCOUNT" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentRemittenceSelection;

        ds = PhoenixAccountsAllotmentRemittance.RemittanceBankDownloadHistorySearch(new Guid(ViewState["Batchid"].ToString()));
        Response.AddHeader("Content-Disposition", "attachment; filename=AccountRemittance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Remittance</h3></td>");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsAllotmentRemittance.RemittanceBankDownloadHistorySearch(new Guid(ViewState["Batchid"].ToString()));

        string[] alCaptions = { "Batch Number", "Payment date", "Payment mode", "Account Code" };
        string[] alColumns = { "FLDBATCHNUMBER", "FLDPAYMENTDATE", "FLDPAYMENTMODENAME", "FLDBANKACCOUNT" };

        General.SetPrintOptions("gvRemittence", "Remittance", alCaptions, alColumns, ds);

        gvRemittence.DataSource = ds;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["Batchid"] == null)
            {
                ViewState["Batchid"] = ds.Tables[0].Rows[0]["FLDBATCHID"].ToString();

            }

        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvRemittence_RowCommand(object sender, GridCommandEventArgs e)
    {
    }


    protected void gvRemittence_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            {

                RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
                if (lblFileName.Text != string.Empty)
                {
                    RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                    HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
                    lnk.NavigateUrl = "../accounts/download.aspx?filename=" + lblFileName.Text + "&filepath=" + PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + lblFilePath.Text;
                }
            }
        }
    }
    
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("BATCHLIST"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchDownLoadHistory.aspx?type=1&Batchid=" + ViewState["Batchid"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchInstructionList.aspx?Batchid=" + ViewState["Batchid"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("TAXANDCHARGES"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchTaxandChargesList.aspx?type=1&Batchid=" + ViewState["Batchid"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void Rebind()
    {
        gvRemittence.SelectedIndexes.Clear();
        gvRemittence.EditIndexes.Clear();
        gvRemittence.DataSource = null;
        gvRemittence.Rebind();
    }
    protected void gvRemittence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
