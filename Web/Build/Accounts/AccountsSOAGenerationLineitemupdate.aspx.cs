using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsSOAGenerationLineitemupdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenPick.Attributes.Add("style", "display:none;");

        if (!IsPostBack)
        {           

            if (Request.QueryString["voucherdetailid"] != null)
            {

                if (Request.QueryString["debitnotereference"] != null)
                    ViewState["debitnotereference"] = Request.QueryString["debitnotereference"].ToString();

                if (Request.QueryString["accountid"] != null)
                {
                    ViewState["vessselaccountid"] = Request.QueryString["accountid"].ToString();
                    ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                }

                if (General.GetNullableGuid(Request.QueryString["debitnoteid"].ToString()) != null)
                    ViewState["debitnotereferenceid"] = Request.QueryString["debitnoteid"].ToString();
                else
                    ViewState["debitnotereferenceid"] = "";               

                if (Request.QueryString["Ownerid"] != null)
                    ViewState["Ownerid"] = Request.QueryString["Ownerid"].ToString();

                if (Request.QueryString["SOAStatus"] != null)
                    ViewState["SOAStatus"] = Request.QueryString["SOAStatus"].ToString(); 

                ViewState["OWNERBUDGETCODE"] = "";
                ViewState["BUDGETID"] = "";
                if (Request.QueryString["voucherdetailid"] != null && Request.QueryString["voucherdetailid"]!="")
                {
                    ViewState["voucherdetailid"] = Request.QueryString["voucherdetailid"].ToString();
                    VoucherEdit(new Guid(Request.QueryString["voucherdetailid"].ToString()));
                }
                
            }
        }

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Soa", "SOA");
        toolbarmain.AddButton("Line Items", "LINEITEMS");
        toolbarmain.AddButton("History", "HISTORY");
        MenuGeneral.AccessRights = this.ViewState;
        MenuGeneral.MenuList = toolbarmain.Show();
        MenuGeneral.SelectedMenuIndex = 1;

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Details", "DETAILS");
        toolbarsub.AddButton("Voucher", "VOUCHER");

        MenuGenralSub.AccessRights = this.ViewState;
        MenuGenralSub.MenuList = toolbarsub.Show();
        MenuGenralSub.SelectedMenuIndex = 1;

        if (ViewState["SOAStatus"] != null)
        {
            if (ViewState["SOAStatus"].ToString() == "Pending")
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuCompanyList.AccessRights = this.ViewState;
                MenuCompanyList.MenuList = toolbar.Show();

                txtBudgetCode.Attributes.Add("readonly", "readonly");
                txtBudgetCodeDescription.Attributes.Add("readonly", "readonly");
            }
        }

        if (ViewState["BUDGETID"]!=null)
        ucOwnerBudgetCode.BudgetId = ViewState["BUDGETID"].ToString();

        if(ViewState["Ownerid"]!=null)
            ucOwnerBudgetCode.OwnerId = ViewState["Ownerid"].ToString();

        //if (ucOwnerBudgetCode.Text.Equals(""))
        //    ucOwnerBudgetCode.Text = ViewState["OWNERBUDGETCODE"].ToString();

        txtBudgetCodeId.Attributes.Add("Style", "visibility:hidden");
        txtBudgetgroupId.Attributes.Add("Style", "visibility:hidden");
        
        BindData();
    }

    protected void CompanyList_TabStripCommand(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, null);";
        Script += "</script>" + "\n";

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidCompany())
            {
                ucError.Visible = true;
                return;
            }
            try
            {
                if (ViewState["voucherdetailid"] != null)
                {
                    PhoenixAccountsSoaChecking.SoaCheckingLineitemUpdate(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["voucherdetailid"].ToString()),
                        ucOwnerBudgetCode.Text.Trim(),
                        txtdescription.Text.Trim(),
                        General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue),
                        General.GetNullableInteger(ViewState["Ownerid"].ToString()),
                        int.Parse(txtBudgetCodeId.Text),
                        chkIncludeYNEdit.Checked ? 1 : 0,
                        chkShowInSummaryBalance.Checked ? 1 : 0
                        );
                    ucStatus.Text = "Data updated successfully";
                }

                BindData();

            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SOA"))
            {
                Response.Redirect("../Accounts/AccountsSOAGeneration.aspx", true);
            }
            if (CommandName.ToUpper().Equals("HISTORY"))
            {
                //Response.Redirect("../Accounts/AccountsSOAGenerationPdfAttachment.aspx?debitnotereferenceid=" + ViewState["debitnotereferenceid"], true);
                Response.Redirect("../Accounts/AccountsDebitNoteReferenceHistory.aspx?DebitNoteReferenceid=" + ViewState["debitnotereferenceid"] + "&qfrom=SOAGeneration");
            }
            //if (dce.CommandName.ToUpper().Equals("LINEITEMS"))
            //{
            //    Response.Redirect("../Accounts/AccountsSoaCheckingLineItems.aspx?debitnotereference=" + ViewState["debitnotereference"].ToString() + "&accountid=" + ViewState["vessselaccountid"].ToString() + "&Ownerid=" + ViewState["Ownerid"].ToString(), true);

            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGenralSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsSOAGenerationLineitemupdate.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&voucherdetailid=" + ViewState["FLDVOUCHERDETAILID"] + "&VesselId=" + Request.QueryString["VesselId"].ToString(), true);
            }
            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../Accounts/AccountsSoaGenerationLineItems.aspx?debitnotereference=" + ViewState["debitnotereference"].ToString() + "&accountid=" + ViewState["accountid"].ToString() + "&Ownerid=" + ViewState["Ownerid"] + "&debitnoteid=" + ViewState["debitnotereferenceid"], true);
                //Response.Redirect("../Accounts/AccountsSoaCheckingLineItems.aspx?debitnotereference=" + ViewState["debitnotereference"].ToString() + "&accountid=" + ViewState["vessselaccountid"].ToString() + "&Ownerid=" + ViewState["Ownerid"].ToString() + "&VesselId=" + Request.QueryString["VesselId"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"], true);

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

        string[] alColumns = { "FLDDEBITNOTEREFERENCE", "FLDTYPE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDNAME", "FLDHARDNAME", "FLDQUICKNAME", "FLDSOASTATUSNAME" };
        string[] alCaptions = { "Statement Reference", "Type", "Account Code", "Account Code Description", "Billing Party", "Year", "Month", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsSoaChecking.SoaCheckingVoucherMergeSearch(General.GetNullableInteger(ViewState["accountid"].ToString()), ViewState["vouchernumber"].ToString());

        gvOwnersAccount.DataSource = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {         
            if (!IsPostBack)
            {
                //gvOwnersAccount.SelectedIndex = 0;
                //ViewState["voucherdetailid"] = ds.Tables[0].Rows[0]["FLDVOUCHERDETAILID"].ToString();
                VoucherEdit(new Guid(ViewState["voucherdetailid"].ToString()));
                SetRowSelection();
            }

        }
       

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

   
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
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

    private void SetRowSelection()
    {
       
        for (int i = 0; i < gvOwnersAccount.Items.Count; i++)
        {
            if (gvOwnersAccount.MasterTableView.DataKeyValues[i].ToString().Equals(ViewState["voucherdetailid"].ToString()))
            {
                gvOwnersAccount.MasterTableView.Items[i].Selected = true;
                break;
            }
        }
    }

  
    protected void ShowExcel()
    {
        //int iRowCount = 0;
        //int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDEBITNOTEREFERENCE", "FLDTYPE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDNAME", "FLDHARDNAME", "FLDQUICKNAME", "FLDSOASTATUSNAME" };
        string[] alCaptions = { "Statement Reference", "Type", "Account Code", "Account Code Description", "Billing Party", "Year", "Month", "Status" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsSoaChecking.SoaCheckingVoucherMergeSearch(General.GetNullableInteger(ViewState["accountid"].ToString()), ViewState["vouchernumber"].ToString());

        Response.AddHeader("Content-Disposition", "attachment; filename=Requisition.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Purchase order form</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

   

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private bool IsValidCompany()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(txtBudgetCodeId.Text) == null)
            ucError.ErrorMessage = "Budget Code is required.";

        if (ucOwnerBudgetCode.Text.Equals(""))
            ucError.ErrorMessage = "Owner Budget code is required.";

        //if (txtdescription.Text.Equals(""))
        //    ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }

    private void VoucherEdit(Guid gVoucherDetailId)
    {
        DataSet ds = PhoenixAccountsSoaChecking.SoaCheckingLineitemEdit(gVoucherDetailId);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtVoucherNumber.Text = dr["FLDPHOENIXVOUCHER"].ToString();
            txtVoucherDate.Text = dr["FLDVOUCHERDATE"].ToString();
            txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
            txtAccountDescription.Text = dr["FLDACCOUNTDESCRIPTION"].ToString();
            //txtOwnerbudgetcode.Text = dr["FLDOWNERBUDGETCODE"].ToString();

            if (dr["FLDOWNERBUDGETGROUPID"].ToString() != "")
            {
                ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETGROUPID"].ToString();
                ucOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            }

            ViewState["OWNERBUDGETCODE"] = dr["FLDOWNERBUDGETCODE"].ToString();
            ViewState["Ownerid"] = dr["FLDOWNERID"].ToString();
            ViewState["BUDGETID"] = dr["FLDBUDGETID"].ToString();

            ucOwnerBudgetCode.OwnerId = dr["FLDOWNERID"].ToString();
            ucOwnerBudgetCode.BudgetId = dr["FLDBUDGETID"].ToString();

            txtdescription.Text = dr["FLDDESCRIPTION"].ToString();
            ViewState["vouchernumber"] = dr["FLDPHOENIXVOUCHER"].ToString();
            ViewState["accountid"] = dr["FLDACCOUNTID"].ToString();
            txtBudgetCode.Text = dr["FLDSUBACCOUNT"].ToString();
            txtBudgetCodeDescription.Text = dr["FLDBUDGETCODEDESCRIPTION"].ToString();
            txtBudgetCodeId.Text = dr["FLDBUDGETID"].ToString();

            chkIncludeYNEdit.Checked = dr["FLDINCULDEINOWNERREPORT"].ToString().Equals("1") ? true : false;
            chkShowInSummaryBalance.Checked = dr["FLDSHOWINSUMMARYBALANCE"].ToString().Equals("1") ? true : false;

            imgShowBudgetCode.Attributes.Add("onclick", "return showPickList('spnPickListBudgetCode', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&budgetdate=" + DateTime.Now.ToString() + "', true); ");
        }
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        ViewState["BUDGETID"] = txtBudgetCodeId.Text;
        ViewState["OWNERBUDGETCODE"] = "";

        ucOwnerBudgetCode.Text = string.Empty;
        ucOwnerBudgetCode.SelectedValue = string.Empty;
        //ucOwnerBudgetCode.OwnerId = ViewState["Ownerid"].ToString();
        //ucOwnerBudgetCode.BudgetId = ViewState["BUDGETID"].ToString();

        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? iownerid = 0;
        DataSet ds1 = PhoenixCommonRegisters.InternalBillingOwnerBudgetCodeSearch(null
                                                                                 , null
                                                                                 , General.GetNullableInteger(ViewState["Ownerid"].ToString()), null
                                                                                 , General.GetNullableInteger(ViewState["BUDGETID"].ToString())
                                                                                 , null, null
                                                                                 , 1
                                                                                 , General.ShowRecords(null)
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount
                                                                                 , ref iownerid);

        if (ds1.Tables[0].Rows.Count == 1)
        {
            DataRow dr = ds1.Tables[0].Rows[0];
            ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODEID"].ToString();
            ucOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            ViewState["OWNERBUDGETCODE"] = dr["FLDOWNERBUDGETCODE"].ToString();
        }
    }

    protected void gvOwnersAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvOwnersAccount_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            ImageButton imgMerge = (ImageButton)e.Item.FindControl("cmdMerge");

            if (imgMerge != null && ViewState["SOAStatus"] != null)
            {
                if (ViewState["SOAStatus"].ToString() == "Pending")
                {
                    imgMerge.Visible = true;
                    imgMerge.Attributes.Add("onclick", "javascript:openNewWindow('Requisition','','" + Session["sitepath"] + "/Accounts/AccountSoaCheckingMerge.aspx?accountid=" + ViewState["accountid"].ToString() + "&vouchernumber=" + ViewState["vouchernumber"].ToString() + "'); return false;");
                }
            }

            ImageButton imgUnMerge = (ImageButton)e.Item.FindControl("cmdUnmerge");
            if (imgUnMerge != null && ViewState["SOAStatus"] != null)
            {
                if (ViewState["SOAStatus"].ToString() == "Pending")
                    imgUnMerge.Visible = true;
                imgUnMerge.Attributes.Add("onclick", "javascript:openNewWindow('Requisition','','" + Session["sitepath"] + "/Accounts/AccountsSoaCheckingUnMerge.aspx?accountid=" + ViewState["accountid"].ToString() + "&vouchernumber=" + ViewState["vouchernumber"].ToString() + "'); return false;");
            }

       
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
                ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
                RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblDtKey");

                if (drv != null)
                {
                    if (drv["FLDISATTACHMENT"].ToString() == "1")
                    {
                        if (cmdAttachment != null)
                        {
                            cmdAttachment.Visible = true;

                            if (lblDtkey != null)
                            {
                                cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text + "&mod="
                                                    + PhoenixModule.ACCOUNTS + "&mimetype=.pdf" + "&source=voucher" + "');return true;");
                            }
                        }

                    }
                    else
                    {
                        if (cmdNoAttachment != null)
                        {
                            cmdNoAttachment.Visible = true;

                            if (lblDtkey != null)
                            {
                                cmdNoAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text + "&mod="
                                                    + PhoenixModule.ACCOUNTS + "&mimetype=.pdf" + "&source=voucher" + "');return true;");
                            }
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

       
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            ViewState["voucherdetailid"] = ((RadLabel)e.Item.FindControl("lblVoucherId")).Text;
            VoucherEdit(new Guid(ViewState["voucherdetailid"].ToString()));
        }
    }
}
