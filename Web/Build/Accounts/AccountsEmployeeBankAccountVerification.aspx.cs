using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsEmployeeBankAccountVerification : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Accounts/AccountsEmployeeBankAccountVerification.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAllotment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Accounts/AccountsEmployeeBankAccountVerification.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Accounts/AccountsEmployeeBankAccountVerification.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuAllotment.AccessRights = this.ViewState;
            MenuAllotment.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["EMPLOYEEID"] = null;
                ViewState["VESSELID"] = null;
                ViewState["MONTH"] = null;
                ViewState["YEAR"] = null;
                BindVessel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindVessel()
    {
        DataSet ds = PhoenixAccountsEmployeeAllotmentRequest.AllotmentBankAccountVesselList(null, null, null);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlVessel.Items.Clear();
            ddlVessel.Text = "";
            ddlVessel.DataSource = ds.Tables[0];
            ddlVessel.DataBind();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            ddlmonthyear.Items.Clear();
            ddlmonthyear.Text = "";
            ddlmonthyear.DataSource = ds.Tables[1];
            ddlmonthyear.DataBind();
        }
    }
    protected void MenuAllotment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                string vessel = ddlVessel.SelectedValue;
                string mnthyr = ddlmonthyear.SelectedValue;
                if (!IsValidVessel(vessel, mnthyr))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    Rebind();
                }
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearFilter();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAllotment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            string[] alColumns = { "FLDREQUESTNUMBER", "FLDREQUESTDATE", "FLDNAME", "FLDFILENO", "FLDRANKCODE", "FLDAMOUNT", "FLDBANKNAME", "FLDACCOUNTNUMBER", "FLDBANKIFSCCODE", "FLDREASONFORAPPROVAL" };
            string[] alCaptions = { "Request Number", "Request Date", "Amount", "File No.", "Employee", "Rank", "Account Name", "Account No.", "Bank Name", "IFSC Code", "Reason of Approval" };

            DataSet ds = PhoenixAccountsEmployeeAllotmentRequest.EmployeeBankAccountVerification(General.GetNullableInteger((ddlVessel.SelectedValue == "" || ddlVessel.SelectedValue != "" || Request.QueryString["VESSELID"] == null) ? (Request.QueryString["VESSELID"] != null ? Request.QueryString["VESSELID"].ToString() : ddlVessel.SelectedValue) : Request.QueryString["VESSELID"].ToString()),
                                                                                                General.GetNullableInteger(lblmonth.Text),
                                                                                                General.GetNullableInteger(lblyear.Text));
            gvAllotment.DataSource = ds;
            
            General.SetPrintOptions("gvAllotment", "Allotment Bank Verification", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDREQUESTNUMBER", "FLDREQUESTDATE", "FLDAMOUNT", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDACCOUNTNUMBER", "FLDBANKNAME", "FLDBANKIFSCCODE", "FLDREASONFORAPPROVAL" };
            string[] alCaptions = { "Request Number", "Request Date", "Amount", "File No.", "Employee", "Rank", "Account Name", "Account No.", "Bank Name", "IFSC Code", "Reason of Approval" };

            DataSet ds = PhoenixAccountsEmployeeAllotmentRequest.EmployeeBankAccountVerification(General.GetNullableInteger(ddlVessel.SelectedValue),
                                                                                                General.GetNullableInteger(lblmonth.Text),
                                                                                                General.GetNullableInteger(lblyear.Text));

            Response.AddHeader("Content-Disposition", "attachment; filename=Allotment_Bank_Verification" + ".xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Employee Allotment Request</center></h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ClearFilter()
    {
        BindVessel();

        lblmonth.Text = string.Empty;
        lblyear.Text = string.Empty;
        Rebind();
    }
    protected void gvAllotment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                RadLabel lblEmployeeId = (RadLabel)e.Item.FindControl("lblEmployeeId");
                string lblallotmentid = ((RadLabel)e.Item.FindControl("lblallotmentid")).Text;
                RadComboBox ddlBankId = (RadComboBox)e.Item.FindControl("ddlBankId");
                PhoenixAccountsEmployeeAllotmentRequest.ORRBankDetailsUpdate(new Guid(ddlBankId.SelectedValue), new Guid(lblallotmentid));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblpaymentid")).Text.Trim();
                Rebind();
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
    protected void gvAllotment_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            LinkButton reb = (LinkButton)e.Item.FindControl("cmdReimRec");
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
            {
                if (drv["FLDALLOTMENTSHORTCODE"].ToString() == "ORR")
                    cmdEdit.Visible = true;
                else
                    cmdEdit.Visible = false;
            }
            if (reb != null)
            {
                reb.Attributes.Add("onclick", "javascript:openNewWindow('Accounts','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentApproveCrewEdit.aspx?ALLOTMENTID=" + drv["FLDALLOTMENTID"].ToString() + "&REQUESTNO=" + drv["FLDREQUESTNUMBER"].ToString() + "'); return false;");
            }
            RadComboBox ddlBankAccount = (RadComboBox)e.Item.FindControl("ddlBankId");
            if (ddlBankAccount != null)
            {
                DataSet ds = PhoenixAccountsEmployeeAllotmentRequest.EmployeeBankAccountList(int.Parse(drv["FLDEMPLOYEEID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlBankAccount.Items.Clear();
                    ddlBankAccount.DataSource = ds.Tables[0];
                    ddlBankAccount.DataBind();
                }
                //   ddlBankAccount.SelectedValue = drv["FLDBANKACCOUNTID"].ToString();
            }

        }

    }
    
    protected void gvAllotment_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow row = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            row.Attributes.Add("style", "position:static");
            TableCell cell = new TableCell();
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Request";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Employee";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Account";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Bank";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.ColumnSpan = 2;
            row.Cells.Add(cell);


            gvAllotment.Controls[0].Controls.AddAt(0, row);
            GridViewRow row1 = ((GridViewRow)gvAllotment.Controls[0].Controls[0]);

        }
    }


    private bool IsValidVessel(string vesselid, string mnthyr)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vesselid.ToString() == "" && mnthyr == "")
        {
            ucError.ErrorMessage = "Vessel or Month-Year is required.";
        }

        return (!ucError.IsError);

    }
    protected void ddlVessel_TextChanged(object sender, EventArgs e)
    {
        string strmonthyear = ddlmonthyear.SelectedValue;
        DataSet ds = PhoenixAccountsEmployeeAllotmentRequest.AllotmentBankAccountVesselList(
            General.GetNullableInteger(ddlVessel.SelectedValue), null, null);
        if (ds.Tables[1].Rows.Count > 0)
        {
            ddlmonthyear.Items.Clear();
            ddlmonthyear.DataSource = ds.Tables[1];

            ddlmonthyear.DataBind();

        }
        ddlmonthyear.SelectedValue = strmonthyear;
        Rebind();
    }
    protected void ddlmonthyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        string vessel = ddlVessel.SelectedValue;
        string monthyear = ddlmonthyear.SelectedValue;
        if (monthyear != "")
        {
            String[] substrings = monthyear.Split('-');
            lblmonth.Text = Convert.ToString(substrings[0]);
            lblyear.Text = Convert.ToString(substrings[1]);
        }
        else
        {
            lblmonth.Text = string.Empty;
            lblyear.Text = string.Empty;
        }
        DataSet ds = PhoenixAccountsEmployeeAllotmentRequest.AllotmentBankAccountVesselList(
           null, General.GetNullableInteger(lblmonth.Text), General.GetNullableInteger(lblyear.Text));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlVessel.Items.Clear();
            ddlVessel.DataSource = ds.Tables[0];
            ddlVessel.DataBind();
        }
        ddlVessel.SelectedValue = vessel;
        Rebind();
    }
    protected void Rebind()
    {
        gvAllotment.SelectedIndexes.Clear();
        gvAllotment.EditIndexes.Clear();
        gvAllotment.DataSource = null;
        gvAllotment.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
