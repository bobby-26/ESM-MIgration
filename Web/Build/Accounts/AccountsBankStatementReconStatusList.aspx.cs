using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class Accounts_AccountsBankStatementReconStatusList : PhoenixBasePage
{
    public int iCompanyid;

    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Bank Recon Status", "RECONSTATUS",ToolBarDirection.Right);
        toolbar1.AddButton("Allocation/Bank Report", "ALLOCATIONREPORT",ToolBarDirection.Right);
        toolbar1.AddButton("Bank Statement", "BANKSTATEMENT",ToolBarDirection.Right);

        MenutravelInvoice.AccessRights = this.ViewState;
        MenutravelInvoice.Title = "Bank Recon Status List";
        MenutravelInvoice.MenuList = toolbar1.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsBankStatementReconStatusList.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
        toolbargrid.AddImageLink("javascript:CallPrint('gvAttachment')", "Print Grid", "icon_print.png", "PRINT");
        //toolbargrid.AddImageButton("../Accounts/AccountsBankStatementReconExcelUploadFilter.aspx", "Find", "search.png", "FIND");
        MenuAttachment.AccessRights = this.ViewState;
        MenuAttachment.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            MenutravelInvoice.SelectedMenuIndex = 0;
        }
    }

    //protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlBankAccount.SelectedBankAccount.ToUpper() != "DUMMY")
    //    {
    //        DataSet ds = PhoenixRegistersAccount.ListBankAccount(
    //            Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), null,
    //            PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            DataRow dr = ds.Tables[0].Rows[0];
    //            //txtCurrencyId.Text = dr["FLDBANKCURRENCYID"].ToString();
    //            txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
    //            txtDesc.Text = dr["FLDDESCRIPTION"].ToString();
    //            //txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
    //            //txtSubAccountCode.Text = dr["FLDSUBACCOUNT"].ToString();
    //        }
    //    }
    //}

    protected void MenuAttachment_OnTabStripCommand(object sender, EventArgs e)
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

    protected void MenutravelInvoice_OnTabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BANKSTATEMENT"))
            {
                Response.Redirect("../Accounts/AccountsBankStatementReconExcelUpload.aspx", false);
            }

            if (CommandName.ToUpper().Equals("ALLOCATIONREPORT"))
            {
                Response.Redirect("../Accounts/AccountsBankStatementReconExcelUploadAllocation.aspx", false);
            }             
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void Rebind()
    {
        gvAttachment.SelectedIndexes.Clear();
        gvAttachment.EditIndexes.Clear();
        gvAttachment.DataSource = null;
        gvAttachment.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDMONTHANDYEAR", "FLDRECONCOMPLETEDDATE", "FLDUSERNAME", "FLDALLOCATIONSTATUS", "FLDREMAININGAMOUNT", "FLDRECONSTATUS" };
        string[] alCaptions = { "Bank Account", "Bank Account Desc", "Currency", "Month and Year", "Recon Completed Date", "User Id", "Allocation Report", "Amount Difference", "Recon Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentBankStatementUploadSelection;

        ds = PhoenixAccountsBankStatementReconUpload.BankStatementReconStatusSearch(
            General.GetNullableString(nvc != null ? nvc.Get("txtBankAccount") : null),
            General.GetNullableString(nvc != null ? nvc.Get("txtAccDesc") : null),
            General.GetNullableInteger(nvc != null ? nvc.Get("ucCurrency") : ""),
            //General.GetNullableString(nvc != null ? nvc.Get("ddlType") : null),
            General.GetNullableInteger(nvc != null ? nvc.Get("ucMonth") : ""),
            General.GetNullableInteger(nvc != null ? nvc.Get("chkExclPostedBankStmt") : "1"),
            sortdirection,
            sortexpression,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount, ref iTotalPageCount,
            General.GetNullableInteger(nvc != null ? nvc.Get("chkExclArchivedBankStmt") : "1"), General.GetNullableString(nvc != null ? nvc.Get("txtBankTaggingId") : null));

        General.SetPrintOptions("gvAttachment", "Bank Recon Status List", alCaptions, alColumns, ds);
        
            gvAttachment.DataSource = ds;
            gvAttachment.VirtualItemCount=iRowCount;
    
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDMONTHANDYEAR", "FLDRECONCOMPLETEDDATE", "FLDUSERNAME", "FLDALLOCATIONSTATUS", "FLDREMAININGAMOUNT", "FLDRECONSTATUS" };
        string[] alCaptions = { "Bank Account", "Bank Account Desc", "Currency", "Month and Year", "Recon Completed Date", "User Id", "Allocation Report", "Amount Difference", "Recon Status" };


        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentBankStatementUploadSelection;

        ds = PhoenixAccountsBankStatementReconUpload.BankStatementReconStatusSearch(
            General.GetNullableString(nvc != null ? nvc.Get("txtBankAccount") : null),
            General.GetNullableString(nvc != null ? nvc.Get("txtAccDesc") : null),
            General.GetNullableInteger(nvc != null ? nvc.Get("ucCurrency") : ""),
            //General.GetNullableString(nvc != null ? nvc.Get("ddlType") : null),
            General.GetNullableInteger(nvc != null ? nvc.Get("ucMonth") : ""),
            General.GetNullableInteger(nvc != null ? nvc.Get("chkExclPostedBankStmt") : "1"),
            sortdirection,
            sortexpression,
            1,
            PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
            ref iRowCount, ref iTotalPageCount,
            General.GetNullableInteger(nvc != null ? nvc.Get("chkExclArchivedBankStmt") : "1"), General.GetNullableString(nvc != null ? nvc.Get("txtBankTaggingId") : null));


        Response.AddHeader("Content-Disposition", "attachment; filename=BankReconStatusList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Bank Recon Status List</h3></td>");
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



    protected void gvAttachment_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

         
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                PhoenixAccountsBankStatementReconUpload.BankStatementUploadPost(new Guid(((RadLabel)e.Item.FindControl("lblexceluploadId")).Text));
                Rebind();
                ucStatus.Text = "Bank Statement is posted successfully.";
            }
            if (e.CommandName.ToUpper().Equals("ARCHIVE"))
            {
                PhoenixAccountsBankStatementReconUpload.ArchiveBankReconUpload(new Guid(((RadLabel)e.Item.FindControl("lblexceluploadId")).Text));
                Rebind();
                ucStatus.Text = "Bank Statement is Archived Successfully.";
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsBankStatementReconUpload.BankStatementUploadDelete(new Guid(((RadLabel)e.Item.FindControl("lblexceluploadId")).Text));
                Rebind();
                ucStatus.Text = "Bank Statement is deleted successfully.";
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_RowDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
                ImageButton post = (ImageButton)e.Item.FindControl("cmdPost");
                ImageButton archive = (ImageButton)e.Item.FindControl("cmdArchive");
                ImageButton delete = (ImageButton)e.Item.FindControl("cmdDelete");

                if (ed != null)
                {

                    //ed.Visible = false;
                    ed.Attributes["onclick"] = "javascript:Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey="
                       + drv["FLDUPLOADID"].ToString() + "&mod=ACCOUNTS&U=1'); return false;";
                    e.Item.Attributes["ondblclick"] = string.Empty;
                }

                RadLabel lblVoucherRefUpdatedYN = (RadLabel)e.Item.FindControl("lblVoucherRefUpdatedYN");


                if (post != null)
                {
                    if (General.GetNullableInteger(drv["FLDISARCHIVED"].ToString()) == 1)
                    {
                        post.Visible = false;
                        delete.Visible = false;
                    }
                    else
                    {
                        post.Visible = true;
                        delete.Visible = true;
                    }
                }

                if (archive != null)
                {
                    if (General.GetNullableInteger(drv["FLDISARCHIVED"].ToString()) == 1)
                    {
                        archive.Visible = false;
                    }
                    else
                    {
                        archive.Visible = true;
                    }
                }

                if (e.Item is GridDataItem)
                {

                    RadLabel lblexceluploadId = (RadLabel)e.Item.FindControl("lblexceluploadId");
                    ImageButton cmdEditInfo = (ImageButton)e.Item.FindControl("cmdEditInfo");
                    if (cmdEditInfo != null)
                    {
                        cmdEditInfo.Attributes.Add("onclick", "javascript:parent.Openpopup('codeHelp1', '', '../Accounts/AccountsBankStatementReconBankAndLedgerdifflist.aspx?uploadid=" + lblexceluploadId.Text + "');return false;");
                    }

                }

                //ImageButton cmdLineItems = (ImageButton)e.Row.FindControl("cmdLineItems");
                //if (cmdLineItems != null)
                //{
                //    DataRowView drv = (DataRowView)e.Row.DataItem;
                //    //ed.Visible = false;
                //    cmdLineItems.Attributes["onclick"] = "javascript:Openpopup('NATD','','../Accounts/AccountsERMVoucherDetail.aspx?exceluploadid="
                //       + drv["FLDUPLOADID"].ToString() + "'); return false;";
                //}
            }



        }
        if (e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

  

    protected void gvAttachment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttachment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
