using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections;
using Telerik.Web.UI;



public partial class AccountsRemittanceBankProcess : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Generate", "GENERATE",ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.Title = "Remittance";
            MenuOrderFormMain.MenuList = toolbarmain.Show();
          //  MenuOrderFormMain.SetTrigger(pnlRemittance);

            //PhoenixToolbar toolbargrid = new PhoenixToolbar();
            //toolbargrid.AddImageButton("../Accounts/AccountsRemittanceMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbargrid.AddImageLink("javascript:CallPrint('gvRemittence')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageButton("../Accounts/AccountsRemittanceFilter.aspx", "Find", "search.png", "FIND");

            //MenuOrderForm.AccessRights = this.ViewState;
            //MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlRemittance);

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REMITTENCEID"] = null;
                gvRemittence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["REMITTENCEID"] != null)
                {
                    ViewState["Remittenceid"] = Request.QueryString["REMITTENCEID"].ToString();
                    // ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?REMITTENCEID=" + ViewState["Remittenceid"];
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

    //protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //    try
    //    {
    //        if (dce.CommandName.ToUpper().Equals("REMITTANCE"))
    //        {
    //            // ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?REMITTENCEID=" + ViewState["Remittenceid"];
    //        }
    //        if (dce.CommandName.ToUpper().Equals("LINEITEMS") && ViewState["Remittenceid"] != null && ViewState["Remittenceid"].ToString() != string.Empty)
    //        {
    //            Response.Redirect("../Accounts/AccountsRemittanceRequestLineItem.aspx?REMITTENCEID=" + ViewState["Remittenceid"]);
    //        }
    //        if (dce.CommandName.ToUpper().Equals("INVOICE") && ViewState["Remittenceid"] != null && ViewState["Remittenceid"].ToString() != string.Empty)
    //        {
    //            Response.Redirect("../Accounts/AccountsRemittanceInvoiceMaster.aspx?REMITTANCEID=" + ViewState["Remittenceid"]);
    //        }

    //        if (dce.CommandName.ToUpper().Equals("SUBMITFORMDAPPROVAL"))
    //        {
    //            PhoenixAccountsRemittance.PrepareRemittanceInstruction(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //            String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
    //            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Remittance Number", "Supplier Code", "Supplier Name", "Status", "Payment mode", "Bank Charge Basis", "Account Code", "Account Description", "Currency", "Remittance Amount" };
        string[] alColumns = { "FLDREMITTANCENUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDREMITTANCESTATUS", "FLDREMITTANCEPAYMENTMODENAME", "FLDREMITTANCEBANKCHARGENAME", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDREMITTANCEAMOUNT" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsRemittance.RemittanceSearchforbankprocess(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 1, iRowCount, ref iRowCount, ref iTotalPageCount, sortexpression, sortdirection);


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
                Rebind();
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

        ds = PhoenixAccountsRemittance.RemittanceSearchforbankprocess(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, int.Parse(ViewState["PAGENUMBER"].ToString()), gvRemittence.PageSize, ref iRowCount, ref iTotalPageCount, sortexpression, sortdirection);

        string[] alCaptions = { "Remittance Number", "Supplier Code", "Supplier Name", "Status", "Payment mode", "Bank Charge Basis", "Account Code", "Account Description", "Currency", "Remittance Amount" };
        string[] alColumns = { "FLDREMITTANCENUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDREMITTANCESTATUS", "FLDREMITTANCEPAYMENTMODENAME", "FLDREMITTANCEBANKCHARGENAME", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDREMITTANCEAMOUNT" };

        General.SetPrintOptions("gvRemittence", "Remittance", alCaptions, alColumns, ds);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
            gvRemittence.DataSource = ds;
            gvRemittence.VirtualItemCount=iRowCount;

            if (ViewState["Remittenceid"] == null)
            {
                // ViewState["Remittenceid"] = ds.Tables[0].Rows[0]["FLDREMITTANCEID"].ToString();
              //  gvRemittence.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                // ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?REMITTENCEID=" + ViewState["Remittenceid"].ToString();
            }
          //  SetRowSelection();
        //}
        //else
        //{
        //    DataTable dt = ds.Tables[0];
        //    ShowNoRecordsFound(dt, gvRemittence);
        //    if (ViewState["PAGEURL"] == null)
        //    {
        //        //ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx";
        //    }
        //}
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

  


    protected void gvRemittence_RowDeleting(object sender, GridCommandEventArgs de)
    {
        Rebind();
    }

 

    protected void gvRemittence_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
              && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                string strRemittance = ((RadLabel)e.Item.FindControl("lblRemittenceId")).Text;
                ImageButton db1 = (ImageButton)e.Item.FindControl("cmdOnHold");
                if (db1 != null)
                    db1.Attributes.Add("onclick", "javascript:parent.Openpopup('REMONHOLD','','../ACCOUNTS/AccountsRemittanceOnHold.aspx" + "?REMITTENCEID=" + strRemittance + "'); return false;");
            }
        }
    }
    protected void Rebind()
    {
        gvRemittence.SelectedIndexes.Clear();
        gvRemittence.EditIndexes.Clear();
        gvRemittence.DataSource = null;
        gvRemittence.Rebind();
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
                        foreach (string  index in SelectedPvs)
                        { selectedagents = selectedagents + index + ","; }
                    }
                }

                if (selectedagents.Length > 10)
                {
                    PhoenixAccountsRemittance.RemittanceInstructionBatchGenerate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, selectedagents.Length > 1 ? selectedagents : null);
                    Session["CHECKED_ITEMS"] = null;
                }
                Rebind();

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1', null, null);";
                Script += "</script>" + "\n";


                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

                //String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                //Session["New"] = "Y";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
       // Response.Redirect("../Accounts/AccountsRemittanceGenerate.aspx");
    }


    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedPvs = new ArrayList();
        string index = "" ;
        //Guid index = new Guid();

        foreach (GridDataItem gvrow in gvRemittence.MasterTableView.Items)
        {
            bool result = false;

            index = gvrow.GetDataKeyValue("FLDREMITTANCEINSTRUCTIONIDLIST").ToString();

            // index = new Guid(gvRemittence.MasterTableView.Items[0].GetDataKeyValue("FLDREMITTANCEINSTRUCTIONIDLIST").ToString());

            //  index = gvRemittence.Items[gvrow.RowIndex].ToString();
            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!SelectedPvs.Contains(index))
                    SelectedPvs.Add(index);
            }
            else
                SelectedPvs.Remove(index);
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            Session["CHECKED_ITEMS"] = SelectedPvs;
    }



    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvRemittence$ctl00$ctl02$ctl01$chkAllRemittance")
        {
            GridHeaderItem headerItem = gvRemittence.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllRemittance");
            foreach (GridDataItem row in gvRemittence.MasterTableView.Items)
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
        //    if (gvRemittence.DataKeys[i].Value.ToString().Equals(ViewState["Remittenceid"].ToString()))
        //    {
        //        gvRemittence.SelectedIndex = i;
        //        PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Rows[i].FindControl("lnkRemittenceid")).Text;
        //    }
        //}
    }

   

    protected void gvRemittence_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            int iRowno;
            iRowno = e.Item.ItemIndex;
            SetRowSelection();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["Remittenceid"] = ((RadLabel)gvRemittence.Items[rowindex].FindControl("lblRemittenceId")).Text;
            PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Items[rowindex].FindControl("lnkRemittenceid")).Text;
            //ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?REMITTENCEID=" + ViewState["Remittenceid"].ToString();
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
        if (Session["New"].ToString() == "Y")
        {
           // gvRemittence.SelectedIndex = 0;
            Session["New"] = "N";
          //  BindPageURL(gvRemittence.SelectedIndex);
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
