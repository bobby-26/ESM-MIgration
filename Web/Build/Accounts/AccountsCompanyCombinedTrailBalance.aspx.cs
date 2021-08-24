using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Text.RegularExpressions;
using System.IO;
using Telerik.Web.UI;


public partial class Accounts_AccountsCompanyCombinedTrailBalance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["EXCLUDE"] = "";
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            gvExcludedVouchers.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            BindCompany();
        }
        RadWindowManager1.Visible = false;
        // ucConfirmMessage.Visible = false;

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Excluded Vouchers", "EXCLUDED", ToolBarDirection.Right);
        toolbar.AddButton("Recalculate", "RECALCULATE", ToolBarDirection.Right);
        toolbar.AddButton("Reset", "RESET", ToolBarDirection.Right);
        MenuReportsFilter.Title = "Combined Trial Balance";
        MenuReportsFilter.AccessRights = this.ViewState;
        MenuReportsFilter.MenuList = toolbar.Show();
      
    }

    private void BindCompany()
    {
        chkCompanyList.DataTextField = "FLDCOMPANYNAME";
        chkCompanyList.DataValueField = "FLDCOMPANYID";

        chkCompanyList.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CompanyFinancialYearCompanyList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        chkCompanyList.DataBind();
    }
   
    protected void gvVender_ItemDataBound(object sender, GridItemEventArgs e)
    {

        decimal result;
        if (e.Item is GridDataItem)
          // if (e.Item.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Item.Cells.Count; i++)
            {
                e.Item.Cells[i].HorizontalAlign = HorizontalAlign.Right;
            }
            
        }
        if (e.Item is GridDataItem)
          //  if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Item.Cells.Count; i++)
            {
                if (decimal.TryParse(e.Item.Cells[i].Text, out result))
                {
                    e.Item.Cells[i].Text = result.ToString("0,0.00");
                }
            }

        }
    }
    //protected void gvVender_RowDataBound(object sender, GridViewRowEventArgs e)
    //{

    //    decimal result;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {            
    //        for (int i = 1; i < e.Row.Cells.Count; i++)
    //        {
    //            e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
    //        }
    //        //e.Row.Cells[1].Text = String.Format((e.Row.Cells[1].Text), "#,##");
    //    }              

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {            
    //        for (int i = 1; i < e.Row.Cells.Count; i++)
    //        {
    //            if (decimal.TryParse(e.Row.Cells[i].Text, out result))
    //            {
    //                e.Row.Cells[i].Text = result.ToString("0,0.00");
    //            }
    //        }

    //    }
    //}

    //protected void gvExcludedVouchers_OnItemCommand(object sender, GridItemEventArgs e)
    //{
    //    if(e.Commandname= Page)
    //}
    protected void MenuReportsFilter_TabStripCommand(object sender, EventArgs e)
    {

        string strAdvancePaymentCode = string.Empty;

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string selectedcompanies = ",";

        for (int i = 0; i < chkCompanyList.Items.Count; i++)
        {
            if (chkCompanyList.Items[i].Selected == true)
                selectedcompanies = selectedcompanies + chkCompanyList.Items[i].Value + ",";
        }

        if (selectedcompanies.Length <= 1)
            selectedcompanies = null;

        if (CommandName.ToUpper().Equals("RECALCULATE"))
        {
         
             gvExcludedVouchers.Visible = false;
            if (selectedcompanies == null)
            {
                ucError.ErrorMessage = "Atleast one company should be selected";
                ucError.Visible = true;
                return;
            }

            if (txtDate.Text == "__/__/____" || txtDate.Text == null)
            {
                ucError.ErrorMessage = "As on Date is manadatory";
                ucError.Visible = true;
                return;
            }

            DataTable dt = null;
            dt = PhoenixAccountsCompanyFinancialyearStatement.CombinedTrailBalanceOpeningExistYN(selectedcompanies, DateTime.Parse(txtDate.Text));


            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0].ToString() == "1")
                {
                    //ucConfirmMessage.Visible = true;
                    //ucConfirmMessage.Text = "Opening Balances has not been transferred for the Selected Company(s).Please confirm if you still want to see the report.";
                    //return;

                    RadWindowManager1.Visible = true;
                    RadWindowManager1.RadConfirm("Opening Balances has not been transferred for the Selected Company(s).Please confirm if you still want to see the report.", "confirm", 400, 250, null, "Confirm");
                    return;
                }
            }


            dt = null;
            dt = PhoenixAccountsCompanyFinancialyearStatement.CombinedTrailBalanceOutOfBalanceExistYN(selectedcompanies, DateTime.Parse(txtDate.Text));


            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0].ToString() == "1")
                {
                    //ucConfirmMessage.Visible = true;
                    //ucConfirmMessage.Text = "Some vouchers are not yet Balanced.Please see Excluded vouchers.Please confirm if you still want to see the Report.";
                    //return;
                    RadWindowManager1.Visible = true;
                    RadWindowManager1.RadConfirm("Some vouchers are not yet Balanced.Please see Excluded vouchers.Please confirm if you still want to see the Report.", "confirm", 400, 250, null, "Confirm");
                    return;
                }
            }


            lblAccurateDatedet.Text = System.DateTime.UtcNow.ToString();
            BindData();
        }

        if (CommandName.ToUpper().Equals("EXCLUDED"))
        {
            lblAccurateDatedet.Text = System.DateTime.UtcNow.ToString();
            ViewState["EXCLUDE"] = 1;
            BindExcludedVouchers();
            if (gvExcludedVouchers.Items.Count > 0)
            {
                gvExcludedVouchers.Visible = true;
                BindExcludedVouchers();
            }

        }

        if (CommandName.ToUpper().Equals("RESET"))
        {
            chkCompanyList.ClearSelection();
            txtDate.Text = null;
            lblAccurateDatedet.Text = string.Empty;
            gvExcludedVouchers.Visible = false;
            
        }

    }
  
    private void BindData()
    {
        try
        {
            string selectedcompanies = ",";

            for (int i = 0; i < chkCompanyList.Items.Count; i++)
            {
                if (chkCompanyList.Items[i].Selected == true)
                    selectedcompanies = selectedcompanies + chkCompanyList.Items[i].Value + ",";
            }

            if (selectedcompanies.Length <= 1)
                selectedcompanies = null;

            if (selectedcompanies == null)
            {
                ucError.ErrorMessage = "Atleast one company should be selected";
                ucError.Visible = true;
                return;
            }
            if (txtDate.Text == "__/__/____" || txtDate.Text == null)
            {
                ucError.ErrorMessage = "As on Date is manadatory";
                ucError.Visible = true;
                return;
            }

            DataSet ds = PhoenixAccountsCompanyFinancialyearStatement.CompaniesCombinedTrailBalance(selectedcompanies, DateTime.Parse(txtDate.Text));

            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView gridview1 = new GridView();

                gvVender.DataSource = ds;
                gvVender.DataBind();

                GridViewRow gvrow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell cell01 = new TableCell();
                cell01.Text = "";
                cell01.HorizontalAlign = HorizontalAlign.Left;
                cell01.ColumnSpan = 1;

                int j = 0;
                gvrow.Cells.AddAt(j, cell01);

                for (int i = 1; i < ds.Tables[0].Columns.Count; i = i + 1)
                {
                    TableCell cell02 = new TableCell();
                    cell02.Text = ds.Tables[1].Rows[j]["FLDCOMPANYCODE"].ToString();
                    cell02.HorizontalAlign = HorizontalAlign.Center;
                    cell02.ColumnSpan = 1;
                    j = j + 1;

                    gvrow.Cells.AddAt(j, cell02);
                }
                gvVender.Controls[0].Controls.AddAt(0, gvrow);
                foreach (GridDataItem gvHeaderRow in gvVender.Items)
                // GridViewRow gvHeaderRow = gvVender.HeaderRow;

                for (int i = 0; i < gvHeaderRow.Cells.Count; i++)
                {
                    String strHeaderText = gvHeaderRow.Cells[i].Text;
                    gvHeaderRow.Cells[i].Text = Regex.Replace(strHeaderText, @"[\d-]", string.Empty);
                  
                }


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ExportToExcel()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=CombinedTrailBalnce.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr></tr><tr>");
        Response.Write(@"<td colspan=""4""><h3>Combined Trial Balance</h3></td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            foreach (GridDataItem gvHR in gvVender.Items)
             // GridViewRow gvHR = gvVender.HeaderRow;
            foreach (TableCell cell in gvHR.Cells)
            {
                cell.Height = 15;
                cell.Attributes.Add("style", "font-size:13px");
            }
            foreach (GridDataItem row in gvVender.Items)
            // foreach (GridViewRow row in gvVender.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    cell.Height = 15;
                    cell.Attributes.Add("style", "font-size:13px");
                }
            }
            foreach (GridFooterItem gvFR in gvVender.Items)
              
          //  GridViewRow gvFR = gvVender.FooterRow;
            foreach (TableCell cell in gvFR.Cells)
            {
                cell.Height = 15;
                cell.Attributes.Add("style", "font-size:13px");
            }
            gvVender.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }

    //private void ExportToExcel()
    //{
    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition", "attachment;filename=CombinedTrailBalnce.xls");
    //    Response.Charset = "";
    //    Response.ContentType = "application/vnd.ms-excel";
    //    Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
    //    Response.Write("<tr>");
    //    Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
    //    Response.Write("<tr></tr><tr>");
    //    Response.Write(@"<td colspan=""4""><h3>Combined Trial Balance</h3></td>");
    //    Response.Write("</tr>");
    //    Response.Write("</TABLE>");
    //    using (StringWriter sw = new StringWriter())
    //    {
    //        HtmlTextWriter hw = new HtmlTextWriter(sw);
    //        GridViewRow gvHR = gvVender.HeaderRow;
    //        foreach (TableCell cell in gvHR.Cells)
    //        {
    //            cell.Height = 15;
    //            cell.Attributes.Add("style", "font-size:13px");
    //        }
    //        foreach (GridViewRow row in gvVender.Rows)
    //        {
    //            foreach (TableCell cell in row.Cells)
    //            {
    //                cell.Height = 15;
    //                cell.Attributes.Add("style", "font-size:13px");
    //            }
    //        }
    //        GridViewRow gvFR = gvVender.FooterRow;
    //        foreach (TableCell cell in gvFR.Cells)
    //        {
    //            cell.Height = 15;
    //            cell.Attributes.Add("style", "font-size:13px");
    //        }
    //        gvVender.RenderControl(hw);
    //        Response.Output.Write(sw.ToString());
    //        Response.Flush();
    //        Response.End();
    //    }
    //}

    protected void imgExcel_OnClientClick(object sender, EventArgs e)
    {
        BindData();
        if (gvVender.Items.Count > 0)
        {
            ExportToExcel();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void OnAction_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                lblAccurateDatedet.Text = System.DateTime.UtcNow.ToString();
                BindData();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   private void BindExcludedVouchers()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        string selectedcompanies = ",";

        for (int i = 0; i < chkCompanyList.Items.Count; i++)
        {
            if (chkCompanyList.Items[i].Selected == true)
                selectedcompanies = selectedcompanies + chkCompanyList.Items[i].Value + ",";
        }
    
            if (selectedcompanies.Length <= 1)
            selectedcompanies = null;

        if (ViewState["EXCLUDE"].ToString() == "1")
        {
            if (selectedcompanies == null)
            {
                ucError.ErrorMessage = "Atleast one company should be selected";
                ucError.Visible = true;
                return;
            }

            if (txtDate.Text == "__/__/____" || txtDate.Text == null)
            {
                ucError.ErrorMessage = "As on Date is manadatory";
                ucError.Visible = true;
                return;
            }

            gvExcludedVouchers.DataSource = PhoenixAccountsCompanyFinancialyearStatement.CombinedTrailBalanceExcluded(selectedcompanies, DateTime.Parse(txtDate.Text), (int)ViewState["PAGENUMBER"], gvExcludedVouchers.PageSize, ref iRowCount, ref iTotalPageCount);
            gvExcludedVouchers.VirtualItemCount = iRowCount;
            //  gvExcludedVouchers.DataBind();


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
    }

    protected void gvExcludedVouchers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvExcludedVouchers.CurrentPageIndex + 1;
           //  BindData();
            BindExcludedVouchers();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
