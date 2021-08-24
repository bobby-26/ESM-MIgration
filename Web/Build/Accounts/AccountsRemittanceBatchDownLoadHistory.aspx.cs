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




public partial class AccountsRemittanceBatchDownLoadHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "Display:None");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            MenuOrderForm.Title = "Remittance";
            MenuOrderForm.MenuList = toolbargrid.Show();
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
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRemittence_Sorting(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
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

        ds = PhoenixAccountsRemittance.RemittanceBankDownloadHistorySearch(new Guid(ViewState["Batchid"].ToString()));
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

        ds = PhoenixAccountsRemittance.RemittanceBankDownloadHistorySearch(new Guid(ViewState["Batchid"].ToString()));

        string[] alCaptions = { "Batch Number", "Payment date", "Payment mode", "Account Code" };
        string[] alColumns = { "FLDBATCHNUMBER", "FLDPAYMENTDATE", "FLDPAYMENTMODENAME", "FLDBANKACCOUNT" };

        General.SetPrintOptions("gvRemittence", "Remittance", alCaptions, alColumns, ds);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
            gvRemittence.DataSource = ds;
        gvRemittence.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;


            if (ViewState["Batchid"] == null && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["Batchid"] = ds.Tables[0].Rows[0]["FLDBATCHID"].ToString();
            //    gvRemittence.SelectedIndex = 0;
            }

            SetRowSelection();
      //  }
        //else
        //{
        //    DataTable dt = ds.Tables[0];
        ////    ShowNoRecordsFound(dt, gvRemittence);
        //}
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

 

    protected void gvRemittence_RowCommand(object sender, GridCommandEventArgs e)
    {
        BindPageURL(e.Item.ItemIndex);
        SetRowSelection();
    }

 

    protected void gvRemittence_RowDeleting(object sender, GridCommandEventArgs de)
    {
        Rebind();
    }

   
    protected void gvRemittence_ItemDataBound(Object sender, GridItemEventArgs e)
    {
       

        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {

                RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
                Image imgtype = (Image)e.Item.FindControl("imgfiletype");
                ImageButton cmdPost = (ImageButton)e.Item.FindControl("cmdPost");
                if (lblFileName.Text != string.Empty)
                {
                    RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                    HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
                    lnk.NavigateUrl = "../accounts/download.aspx?filename=" + lblFileName.Text + "&filepath=" + PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + lblFilePath.Text;// Session["sitepath"] + "/attachments/" + lblFilePath.Text;
                    //lnk.NavigateUrl = Session["sitepath"] + "/attachments/" + lblFilePath.Text;
                }
            }
        }
    }


    private string ResolveImageType(string extn)
    {
        string imagepath = Session["images"] + "/";
        if (string.IsNullOrEmpty(extn)) extn = string.Empty;
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                imagepath += "imagefile.png";
                break;
            case ".doc":
                imagepath += "word.png";
                break;
            case ".xls":
                imagepath += "xls.png";
                break;
            case ".pdf":
                imagepath += "pdf.png";
                break;
            case ".rar":
            case ".zip":
                imagepath += "rar.png";
                break;
            case ".txt":
                imagepath += "notepad.png";
                break;
            default:
                imagepath += "anyfile.png";
                break;
        }
        return imagepath;
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("GENERATE"))
            {
                string selectedagents = ",";
                if (Session["CHECKED_ITEMS"] != null)
                {
                    ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                    if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (string index in SelectedPvs)
                        { selectedagents = selectedagents + index + ","; }
                    }
                }

                if (selectedagents.Length > 10)
                {
                    PhoenixAccountsRemittance.RemittanceInstructionBatchGenerate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, selectedagents.Length > 1 ? selectedagents : null);
                }
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

 
    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvRemittence$ctl01$chkAllRemittance")
        {
            GridHeaderItem headerItem = gvRemittence.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllRemittance");
            foreach (GridViewRow row in gvRemittence.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (chkAll.Checked == true)
                {
                    cbSelected.Checked = true;
                }
                else
                {
                    cbSelected.Checked = false;
                }
            }
        }
    }

    private void SetRowSelection()
    {
        //gvRemittence.SelectedIndex = -1;
        //for (int i = 0; i < gvRemittence.Rows.Count; i++)
        //{
        //    if (gvRemittence.DataKeys[i].Value.ToString().Equals(ViewState["Batchid"].ToString()))
        //    {
        //        gvRemittence.SelectedIndex = i;
        //        //   PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Rows[i].FindControl("lnkRemittenceid")).Text;
        //    }
        //}
        if (gvRemittence.SelectedItems.Count > 0)
        {
            DataRowView drv = (DataRowView)gvRemittence.SelectedItems[0].DataItem;
            PhoenixAccountsVoucher.VoucherNumber = drv["FLDBATCHID"].ToString();
        }
        else
        {
            if (gvRemittence.MasterTableView.Items.Count > 0)
            {
                gvRemittence.MasterTableView.Items[0].Selected = true;
                PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Items[0].FindControl("lnkRemittenceid")).Text;
            }
        }
    }

  

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["Batchid"] = ((RadLabel)gvRemittence.Items[rowindex].FindControl("lblBatchId")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvRemittence.SelectedIndexes.Clear();
        gvRemittence.EditIndexes.Clear();
        gvRemittence.DataSource = null;
        gvRemittence.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
         Rebind();
        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
            if (gvRemittence.Items.Count > 0)
                BindPageURL(0);
        }
    }

    protected void gvRemittence_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRemittence.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
