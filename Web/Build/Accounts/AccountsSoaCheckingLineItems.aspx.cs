using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Accounts_AccountsSoaCheckingLineItems : PhoenixBasePage
{
    public string vesselcurrency = string.Empty;
    
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("SOA", "SOA");
            toolbarmain.AddButton("Line Items", "LINEITEMS");
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbarmain.Show();
            MenuGeneral.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Details", "DETAILS");
            toolbarsub.AddButton("Report", "REPORT");
            toolbarsub.AddButton("Voucher", "VOUCHER");

            MenuGenralSub.AccessRights = this.ViewState;
            MenuGenralSub.MenuList = toolbarsub.Show();
            MenuGenralSub.SelectedMenuIndex = 0;

            PhoenixToolbar toolbarlineitem = new PhoenixToolbar();
            toolbarlineitem.AddButton("Next Voucher", "NEXTVOUCHER",ToolBarDirection.Right);
            toolbarlineitem.AddButton("Row Verified", "ROWVERIFIED", ToolBarDirection.Right);
            toolbarlineitem.AddButton("All Verified", "ALLVERIFIED", ToolBarDirection.Right);
            toolbarlineitem.AddButton("Queries", "QUERY", ToolBarDirection.Right);
           
            
            
            MenuSOALineItems.AccessRights = this.ViewState;
            MenuSOALineItems.MenuList = toolbarlineitem.Show();

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["RECORDNUMBER"] = General.ShowRecords(null);

                if (Request.QueryString["debitnotereference"].ToString() != null)
                    ViewState["debitnotereference"] = Request.QueryString["debitnotereference"].ToString();
                else
                    ViewState["debitnotereference"] = "";

                if (Request.QueryString["Status"].ToString() != null)
                {
                    ViewState["Status"] = Request.QueryString["Status"].ToString();
                }
                else
                {
                    ViewState["Status"] = "";
                }

                if (General.GetNullableGuid(Request.QueryString["debitnoteid"].ToString()) != null)
                    ViewState["debitnotereferenceid"] = Request.QueryString["debitnoteid"].ToString();
                else
                    ViewState["debitnotereferenceid"] = "";

                if (Request.QueryString["Ownerid"].ToString() != null)
                    ViewState["Ownerid"] = Request.QueryString["Ownerid"].ToString();
                else
                    ViewState["Ownerid"] = "";

                if (Request.QueryString["accountid"].ToString() != null)
                    ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                else
                    ViewState["accountid"] = "";
         
                ViewState["FLDVOUCHERDETAILID"] = "";
            }
            BindData();
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

        string[] alColumns = { "FLDDEBITNOTEREFERENCE", "FLDTYPE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDNAME", "FLDHARDNAME", "FLDQUICKNAME", "FLDSOASTATUSNAME" };
        string[] alCaptions = { "Statement Reference", "Type", "Account Code", "Account Code Description", "Billing Party", "Year", "Month", "Status" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsSoaChecking.SoaCheckingVoucherList(ViewState["debitnotereference"].ToString()
                                                , int.Parse(ViewState["Ownerid"].ToString())
                                                , Filter.CurrentOwnerBudgetCodeSelection
                                                , int.Parse(ViewState["accountid"].ToString())
                                                , (int)ViewState["PAGENUMBER"]
                                                , iRowCount
                                                , ref iRowCount
                                                , ref iTotalPageCount
                                                , new Guid(ViewState["debitnotereferenceid"].ToString()));


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

    protected void MenuGenralSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsSOAcheckingLineitemupdate.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&voucherdetailid=" + ViewState["FLDVOUCHERDETAILID"] + "&VesselId=" + Request.QueryString["VesselId"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "&Status=" + ViewState["Status"], true);
            }
            else if (CommandName.ToUpper().Equals("REPORT"))
            {
                Response.Redirect("../Accounts/AccountsSoaCheckingVesselReports.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&voucherdetailid=" + ViewState["FLDVOUCHERDETAILID"] + "&VesselId=" + Request.QueryString["VesselId"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "&Status=" + ViewState["Status"], true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                Response.Redirect("../Accounts/AccountsSoaChecking.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSOALineItems_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("QUERY"))
            {
                String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsSoaCheckingVoucherQuery.aspx?voucherid=" + ViewState["FLDVOUCHERDETAILID"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

            else if (CommandName.ToUpper().Equals("ROWVERIFIED"))
            {
                PhoenixAccountsSoaChecking.VerifyVoucher(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["RVDtKey"].ToString()));
                Rebind();
                ViewState["RVDtKey"] = null;
            }
            else if (CommandName.ToUpper().Equals("ALLVERIFIED"))
            {
                PhoenixAccountsSoaChecking.SoaCheckingVerifyAll(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ViewState["accountid"].ToString()), ViewState["accountCode"].ToString(), ViewState["debitnotereference"].ToString(), int.Parse(ViewState["Ownerid"].ToString()), ViewState["ownerbudgetcode"].ToString());

                DataSet ds = new DataSet();

                ds = PhoenixAccountsSoaChecking.StatementOfAccountsBudgetSearch(int.Parse(ViewState["accountid"].ToString()), ViewState["accountCode"].ToString(), ViewState["debitnotereference"].ToString(), int.Parse(ViewState["Ownerid"].ToString()));

                int rowcount = 0;

                DataRow[] drindex = ds.Tables[0].Select("FLDOWNERBUDGETCODE='" + Filter.CurrentOwnerBudgetCodeSelection + "'");

                if (drindex != null)
                {
                    rowcount = int.Parse(drindex[0]["FLDROWNUMER"].ToString());
                }

                rowcount++;

                DataRow[] drnxtrow = ds.Tables[0].Select("FLDROWNUMER=" + rowcount);

                if (drnxtrow != null)
                {
                    if (drnxtrow.Length > 0)
                        Filter.CurrentOwnerBudgetCodeSelection = drnxtrow[0]["FLDOWNERBUDGETCODE"].ToString();
                    else
                    {
                        //drnxtrow = ds.Tables[0].Select("FLDROWNUMER=1");
                        Filter.CurrentOwnerBudgetCodeSelection = ds.Tables[0].Rows[0]["FLDOWNERBUDGETCODE"].ToString();
                    }
                }

                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSoaCheckingLineItemDetails.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["DtKey"] + "&voucherdtkey=" + ViewState["VoucherDtKey"] + "&debitnoteid=" + ViewState["debitnotereferenceid"];

                Rebind();
            }
            else if (CommandName.ToUpper().Equals("NEXTVOUCHER"))
            {
                BindData();

                int index = int.Parse(ViewState["index"].ToString()) + 1;
                if (gvOwnersAccount.Items.Count > index)
                {
                    RadLabel lbl = ((RadLabel)gvOwnersAccount.Items[index].FindControl("lblVoucherId"));
                    if (lbl != null && lbl.Text != "")
                    {
                        ViewState["FLDVOUCHERDETAILID"] = lbl.Text;
                        ViewState["DtKey"] = ((RadLabel)gvOwnersAccount.Items[index].FindControl("lblDtKey")).Text;
                        ViewState["VoucherDtKey"] = ((RadLabel)gvOwnersAccount.Items[index].FindControl("lblVoucherDtKey")).Text;
                        SetRowSelection();
                        ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSoaCheckingLineItemDetails.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["DtKey"] + "&voucherdtkey=" + ViewState["VoucherDtKey"] + "&debitnoteid=" + ViewState["debitnotereferenceid"];
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
    protected void gvOwnersAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOwnersAccount.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvOwnersAccount.SelectedIndexes.Clear();
        gvOwnersAccount.EditIndexes.Clear();
        gvOwnersAccount.DataSource = null;
        gvOwnersAccount.Rebind();
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

        ds = PhoenixAccountsSoaChecking.SoaCheckingVoucherList(General.GetNullableString(ViewState["debitnotereference"].ToString())
                                                , General.GetNullableInteger(ViewState["Ownerid"].ToString())
                                                , Filter.CurrentOwnerBudgetCodeSelection
                                                , General.GetNullableInteger(ViewState["accountid"].ToString())
                                                , (int)ViewState["PAGENUMBER"]
                                                , (int)ViewState["RECORDNUMBER"]
                                                , ref iRowCount
                                                , ref iTotalPageCount
                                                , new Guid(ViewState["debitnotereferenceid"].ToString()));
        ShowReport();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["accountid"] = ds.Tables[0].Rows[0]["FLDACCOUNTID"].ToString();

            if (string.IsNullOrEmpty(ViewState["FLDVOUCHERDETAILID"].ToString()))
            {
                ViewState["FLDVOUCHERDETAILID"] = ds.Tables[0].Rows[0]["FLDVOUCHERDETAILID"].ToString();
            }

            ViewState["accountCode"] = ds.Tables[0].Rows[0]["FLDACCOUNTCODE"].ToString();
            ViewState["ownerbudgetcode"] = ds.Tables[0].Rows[0]["FLDOWNERBUDGETCODE"].ToString();
            vesselcurrency = ds.Tables[0].Rows[0]["FLDVESSELCURRENCYCODE"].ToString();
            string dtkey = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            ViewState["DtKey"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            ViewState["VoucherDtKey"] = ds.Tables[0].Rows[0]["FLDVOUCHERDTKEY"].ToString();

          
            gvOwnersAccount.DataSource = ds;
            gvOwnersAccount.VirtualItemCount = iRowCount;

            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSoaCheckingLineItemDetails.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["DtKey"] + "&voucherdtkey=" + ViewState["VoucherDtKey"] + "&debitnoteid=" + ViewState["debitnotereferenceid"];

            if (!IsPostBack)
            {
                //  gvOwnersAccount.SelectedIndex = 0;
               
            }
            if (General.GetNullableInteger(Filter.CurrentSOARowSelectedFilter) == 1)
            {
                //  gvOwnersAccount.SelectedIndex = 0;
             
                Filter.CurrentSOARowSelectedFilter = "0";
            }
            if (ViewState["RVDtKey"] == null)
            {
                ViewState["RVDtKey"] = ViewState["DtKey"];
            }

            DataSet dsno = PhoenixAccountsSoaChecking.SoaCheckingVoucherSearch(ViewState["accountCode"].ToString(), ViewState["ownerbudgetcode"].ToString());

            if (dsno.Tables[0].Rows.Count > 0)
            {
                lnkNumber.Text = dsno.Tables[0].Rows[0]["FLDCOUNT"].ToString() + "Voucher has not been billed for this budget";
                lnkNumber.Visible = true;
                lnkNumber.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','"+Session["sitepath"]+"/Accounts/AccountsSoaCheckingVoucherUpdate.aspx?ACCOUNTCODE=" + ViewState["accountCode"].ToString() + "&OWNERBUDGETCODE=" + ViewState["ownerbudgetcode"].ToString() + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "'); return false;");
            }
            else
                lnkNumber.Visible = false;
        }
   

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Order Form List", alCaptions, alColumns, ds);

    }

 
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
  
    private void SetRowSelection()
    {
        gvOwnersAccount.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvOwnersAccount.Items)
        {
            if (item.GetDataKeyValue("FLDVOUCHERDETAILID").ToString().Equals(ViewState["FLDVOUCHERDETAILID"].ToString()))
            {
                gvOwnersAccount.SelectedIndexes.Add(item.ItemIndex);
                ViewState["RVDtKey"] = ((RadLabel)gvOwnersAccount.Items[item.ItemIndex].FindControl("lblDtKey")).Text;
                ViewState["index"] = item.ItemIndex;
            }
        }
    }

 


    protected void gvOwnersAccount_ItemDataBound(object sender, GridItemEventArgs e)
   
    {
        if (e.Item is GridHeaderItem)
        {

        }
        if (e.Item is GridDataItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkAccountCOde");

            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton cmdQuery = (ImageButton)e.Item.FindControl("cmdQueries");
            if (drv != null && drv["FLDISQUERY"].ToString() == "1")
            {
                cmdQuery.Visible = true;
            }

            if (drv != null && drv["FLDVERFIED"].ToString() == "1")
            {
                ImageButton imgv = (ImageButton)e.Item.FindControl("cmdRowVerified");
                if (imgv != null)
                    imgv.Visible = false;

                ImageButton imbverified = (ImageButton)e.Item.FindControl("imbVeified");
                if (imbverified != null)
                {
                    imbverified.Visible = true;

                    string tooltip = "Verified By: " + drv["FLDNAME"].ToString() + "\nVerified Date: " + drv["FLDVERFIEDDATE"].ToString();

                    imbverified.ToolTip = tooltip;
                }
            }

            if (e.Item is GridDataItem)
            {
                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
            }
            if (e.Item is GridDataItem)
            {
                // DataRowView drv = (DataRowView)e.Row.DataItem;
                /* Voucher attachments.. */
                ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
                ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
                RadLabel lblDtKey = (RadLabel)e.Item.FindControl("lblDtKey");
                RadLabel lblVoucherLineItemId = (RadLabel)e.Item.FindControl("lblVoucherLineItemId");
                RadLabel lblVoucherDetailId = (RadLabel)e.Item.FindControl("lblVoucherDetailId");

                if (drv["FLDISATTACHMENT"].ToString() == "1")
                {
                    if (cmdAttachment != null)
                    {
                        if (lblVoucherLineItemId != null && General.GetNullableGuid(lblVoucherLineItemId.Text) != null)
                        {
                            cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','"+Session["sitepath"]+"/Accounts/AccountsVoucherAttachments.aspx?dtkey="
                            + lblDtKey.Text + "&qvoucherlineid=" + lblVoucherLineItemId.Text + "&voucherdetailid=" + lblVoucherDetailId.Text + "&Status=" + ViewState["Status"] + "');return true;");
                        }
                        else
                        {
                            cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Accounts/AccountsVoucherAttachments.aspx?dtkey="
                            + lblDtKey.Text + "&voucherdetailid=" + lblVoucherDetailId.Text + "&Status=" + ViewState["Status"] + "');return true;");
                        }

                        cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                    }
                }
                else
                {
                    if (cmdNoAttachment != null)
                    {
                        cmdNoAttachment.Visible = true;

                        if (lblVoucherLineItemId != null && General.GetNullableGuid(lblVoucherLineItemId.Text) != null)
                        {
                            cmdNoAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','"+Session["sitepath"]+"/Accounts/AccountsVoucherAttachments.aspx?dtkey="
                            + lblDtKey.Text + "&qvoucherlineid=" + lblVoucherLineItemId.Text + "&voucherdetailid=" + lblVoucherDetailId.Text + "&Status=" + ViewState["Status"] + "');return true;");
                        }
                        else
                        {
                            cmdNoAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Accounts/AccountsVoucherAttachments.aspx?dtkey="
                            + lblDtKey.Text + "&voucherdetailid=" + lblVoucherDetailId.Text + "&Status=" + ViewState["Status"] + "');return true;");
                        }
                        //cmdNoAttachment.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                        //                    + PhoenixModule.ACCOUNTS + "');return true;");
                        cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
                    }
                }
                //ImageButton iab = (ImageButton)e.Row.FindControl("cmdAttachment");
                //ImageButton inab = (ImageButton)e.Row.FindControl("cmdNoAttachment");
                //if (iab != null) iab.Visible = true;
                //if (inab != null) inab.Visible = false;
                //int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
                //if (n == 0)
                //{
                //    if (iab != null) iab.Visible = false;
                //    if (inab != null) inab.Visible = true;
                //}
            }
        }
    }

    protected void gvOwnersAccount_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
    }
    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("SELECT") || e.CommandName.ToUpper().Equals("ATTACHMENT") || e.CommandName.ToUpper().Equals("NOATTACHMENT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = e.Item.ItemIndex;
                RadLabel lbl = ((RadLabel)gvOwnersAccount.Items[nCurrentRow].FindControl("lblVoucherId"));
                ViewState["FLDVOUCHERDETAILID"] = lbl.Text;
                ViewState["DtKey"] = ((RadLabel)e.Item.FindControl("lblDtKey")).Text;
                ViewState["VoucherDtKey"] = ((RadLabel)e.Item.FindControl("lblVoucherDtKey")).Text;
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSoaCheckingLineItemDetails.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["DtKey"] + "&voucherdtkey=" + ViewState["VoucherDtKey"] + "&debitnoteid=" + ViewState["debitnotereferenceid"];
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = e.Item.ItemIndex;
                RadLabel lbl = ((RadLabel)gvOwnersAccount.Items[nCurrentRow].FindControl("lblVoucherId"));
                ViewState["FLDVOUCHERDETAILID"] = lbl.Text;
                Response.Redirect("../Accounts/AccountsSOAcheckingLineitemupdate.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&voucherdetailid=" + ViewState["FLDVOUCHERDETAILID"] + "&VesselId=" + Request.QueryString["VesselId"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "&Status=" + ViewState["Status"], true);
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
        ViewState["FLDVOUCHERDETAILID"] = "";
        BindData();
    }
   

    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvOwnersAccount.Items[rowindex];
            RadLabel lbl = ((RadLabel)gvOwnersAccount.Items[rowindex].FindControl("lblVoucherId"));
            ViewState["FLDVOUCHERDETAILID"] = lbl.Text;
            ViewState["DtKey"] = ((RadLabel)gvOwnersAccount.Items[rowindex].FindControl("lblDtKey")).Text;
            ViewState["VoucherDtKey"] = ((RadLabel)gvOwnersAccount.Items[rowindex].FindControl("lblVoucherDtKey")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuVoucherLI_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper() == "VOUCHERREPORT")
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvOwnersAccount_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvOwnersAccount.SelectedIndexes.Add(e.NewSelectedIndex);
        RadLabel lbl = ((RadLabel)gvOwnersAccount.Items[e.NewSelectedIndex].FindControl("lblVoucherId"));
        ViewState["FLDVOUCHERDETAILID"] = lbl.Text;
        ViewState["DtKey"] = ((RadLabel)gvOwnersAccount.Items[e.NewSelectedIndex].FindControl("lblDtKey")).Text;
        ViewState["VoucherDtKey"] = ((RadLabel)gvOwnersAccount.Items[e.NewSelectedIndex].FindControl("lblVoucherDtKey")).Text;
    }
    private void ShowReport()
    {
        string prams = "";
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        prams += "&debitnotereference=" + ViewState["debitnotereference"].ToString();
        prams += "&Ownerid=" + General.GetNullableInteger(ViewState["Ownerid"].ToString());
        prams += "&BudgetCode=" + Filter.CurrentOwnerBudgetCodeSelection;
        prams += "&accountid=" + General.GetNullableInteger(ViewState["accountid"].ToString());
        prams += exceloptions();

        //MenuVoucherLI.MenuList.Clear();
        toolbargrid.AddImageButton("javascript:openNewWindow('Report','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=SOACHECKINGVOUCHER" + prams + "');return false;", "Report", "task-list.png", "VOUCHERREPORT");
        MenuVoucherLI.AccessRights = this.ViewState;
        MenuVoucherLI.MenuList = toolbargrid.Show();
    }
    protected string exceloptions()
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 181);

        string options = "";

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["FLDSHORTNAME"].ToString().Equals("EXL"))
                options += "&showexcel=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("WRD"))
                options += "&showword=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("PDF"))
                options += "&showpdf=" + dr["FLDHARDNAME"].ToString();
        }
        return options;
    }
}
