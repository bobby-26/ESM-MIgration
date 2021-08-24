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

public partial class AccountsAllotmentRemittanceBankProcess : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Generate", "GENERATE", ToolBarDirection.Right);
            //toolbarmain.AddButton("Generate All", "GENERATEALL");

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            //   MenuOrderFormMain.SetTrigger(pnlRemittance);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceBankProcess.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRemittence')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceBankProcess.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceBankProcess.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            // MenuOrderForm.SetTrigger(pnlRemittance);



            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REMITTENCEID"] = null;

                if (Session["CHECKED_ITEMS"] != null)
                    Session.Remove("CHECKED_ITEMS");

                if (Session["CHECKED_RBID"] != null)
                    Session.Remove("CHECKED_RBID");

                if (Request.QueryString["REMITTENCEID"] != null)
                {
                    ViewState["Remittenceid"] = Request.QueryString["REMITTENCEID"].ToString();
                }
                gvRemittence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        Rebind();
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Remittance No.", "File No.", "Name", "Remittance Currency", "Remittance Amount", "Benificiary Name", "Benificiary Bank SWIFT Code", "Benificiary Bank Name", "Benificiary Bank Acct.No." };
        string[] alColumns = { "FLDREMITTANCENUMBERLIST", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDCURRENCYCODE", "FLDREMITTANCEAMOUNT", "FLDBENEFICIARYNAME", "FLDSWIFTCODE", "FLDBANKNAME", "FLDACCOUNTNUMBER" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsAllotmentRemittance.RemittanceSearchforbankprocess(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, (int)ViewState["PAGENUMBER"], iRowCount, ref iRowCount, ref iTotalPageCount, sortexpression, sortdirection, General.GetNullableString(txtRemittanceSearch.Text.Trim()));

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountAllotmentRemittance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Allotment Remittance</h3></td>");
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtRemittanceSearch.Text = string.Empty;
                Rebind();
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
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRemittence.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsAllotmentRemittance.RemittanceSearchforbankprocess(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, (int)ViewState["PAGENUMBER"], gvRemittence.PageSize, ref iRowCount, ref iTotalPageCount, sortexpression, sortdirection, General.GetNullableString(txtRemittanceSearch.Text.Trim()));

        string[] alCaptions = { "Remittance No.", "File No.", "Name", "Remittance Currency", "Remittance Amount", "Benificiary Name", "Benificiary Bank SWIFT Code", "Benificiary Bank Name", "Benificiary Bank Acct.No." };
        string[] alColumns = { "FLDREMITTANCENUMBERLIST", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDCURRENCYCODE", "FLDREMITTANCEAMOUNT", "FLDBENEFICIARYNAME", "FLDSWIFTCODE", "FLDBANKNAME", "FLDACCOUNTNUMBER" };

        General.SetPrintOptions("gvRemittence", "Remittance", alCaptions, alColumns, ds);

        gvRemittence.DataSource = ds;
        gvRemittence.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvRemittence_ItemDataBound(Object sender, GridItemEventArgs e)
    {

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
                        foreach (Guid index in SelectedPvs)
                        { selectedagents = selectedagents + index + ","; }
                    }
                }

                string selectedRBId = ",";
                if (Session["CHECKED_RBID"] != null)
                {
                    ArrayList selectedARBId = (ArrayList)Session["CHECKED_RBID"];
                    if (selectedARBId != null && selectedARBId.Count > 0)
                    {
                        foreach (Guid index in selectedARBId)
                        { selectedRBId = selectedRBId + index + ","; }
                    }
                }

                if (selectedagents.Length > 10)
                {
                    PhoenixAccountsAllotmentRemittance.RemittanceInstructionBatchGenerate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, selectedagents.Length > 1 ? selectedagents : null, selectedRBId.Length > 1 ? selectedRBId : null);
                    Session["CHECKED_ITEMS"] = null;
                    Session["CHECKED_RBID"] = null;
                }
                Rebind();

                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += "fnReloadList('codehelp1', null, null);";
                //Script += "</script>" + "\n";


                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);



                //string Script = "";
                //Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
                //Script += "alert('hi');fnReloadList('codehelp1');";
                //Script += "</script>" + "\n";

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "BookMarkScript", Script, false);

                String scriptinsert = String.Format("fnReloadList('codehelp1', null, null);");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
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
        try
        {
            ArrayList SelectedPvs = new ArrayList();
            ArrayList selectedARBId = new ArrayList();
            //string index = "";
            Guid index = new Guid();
            Guid ArbId = new Guid();
            int i = 0;
            foreach (GridItem gvrow in gvRemittence.Items)
            {
                bool result = false;
                index = new Guid(gvRemittence.MasterTableView.Items[i].GetDataKeyValue("FLDREMITTANCEINSTRUCTIONIDLIST").ToString());
                ArbId = new Guid(gvRemittence.MasterTableView.Items[i].GetDataKeyValue("FLDALLOTMENTREMITTANCEBATCHID").ToString());

                //ArbId = new Guid((gvrow.FindControl("lblAllotmentRemittanceBatchId")).ToString());
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

                if (Session["CHECKED_RBID"] != null)
                    selectedARBId = (ArrayList)Session["CHECKED_RBID"];
                if (result)
                {
                    if (!selectedARBId.Contains(ArbId))
                        selectedARBId.Add(ArbId);
                }
                else
                    selectedARBId.Remove(ArbId);
                i = i + 1;
            }
            if (SelectedPvs != null && SelectedPvs.Count > 0)
                Session["CHECKED_ITEMS"] = SelectedPvs;

            if (selectedARBId != null && selectedARBId.Count > 0)
                Session["CHECKED_RBID"] = selectedARBId;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            RadCheckBox chkAll = new RadCheckBox();

            foreach (GridHeaderItem headerItem in gvRemittence.MasterTableView.GetItems(GridItemType.Header))
            {
                if((RadCheckBox)headerItem["Listcheckbox"].FindControl("chkAllRemittance") != null)
                    chkAll = (RadCheckBox)headerItem["Listcheckbox"].FindControl("chkAllRemittance"); // Get the header checkbox
            }
            foreach (GridItem row in gvRemittence.Items)
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

    private void GetSelectedPvs()
    {
        if (Session["CHECKED_ITEMS"] != null)
        {
            ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
            Guid index = new Guid();
            if (SelectedPvs != null && SelectedPvs.Count > 0)
            {
                foreach (GridItem row in gvRemittence.Items)
                {
                    CheckBox chk = (CheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(gvRemittence.MasterTableView.DataKeyValues[row.RowIndex].ToString());
                    if (SelectedPvs.Contains(index))
                    {
                        CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }

        if (Session["CHECKED_RBID"] != null)
        {
            ArrayList selectedARBId = (ArrayList)Session["CHECKED_RBID"];
            Guid ArbId = new Guid();
            if (selectedARBId != null && selectedARBId.Count > 0)
            {
                foreach (GridItem row in gvRemittence.Items)
                {
                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    ArbId = new Guid(gvRemittence.MasterTableView.DataKeyValues[row.RowIndex].ToString());
                    //if (selectedARBId.Contains(ArbId))
                    //{
                    //    CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelect");
                    //    myCheckBox.Checked = true;
                    //}
                }
            }
        }
    }

    protected void gvRemittence_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["Remittenceid"] = ((Label)gvRemittence.Items[rowindex].FindControl("lblRemittenceId")).Text;
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
            Session["New"] = "N";
            BindPageURL(0);
        }
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
