using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsSoaGenerationQueriesView : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvOwnersAccount.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvOwnersAccount.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarlineitem = new PhoenixToolbar();
            toolbarlineitem.AddButton("Closed Queries", "CLOSED",ToolBarDirection.Right);
            toolbarlineitem.AddButton("Pending Queries", "PENDING", ToolBarDirection.Right);
          
            
            MenuSOALineItems.AccessRights = this.ViewState;
            MenuSOALineItems.MenuList = toolbarlineitem.Show();
           

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["RECORDNUMBER"] = General.ShowRecords(null);            

                if (General.GetNullableGuid(Request.QueryString["debitnoteid"].ToString()) != null)
                    ViewState["debitnotereferenceid"] = Request.QueryString["debitnoteid"].ToString();
                else
                    ViewState["debitnotereferenceid"] = "";          

                if (Request.QueryString["PendingYN"] != null)
                    ViewState["PendingYN"] = Request.QueryString["PendingYN"];                
            }          

            if (General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()) != null)
            {
                DataTable dt = PhoenixAccountsSOAGeneration.SOAGenerationDetails(new Guid(ViewState["debitnotereferenceid"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    ViewState["accountid"] = dt.Rows[0]["FLDACCOUNTID"].ToString();
                    ViewState["debitnotereference"] = dt.Rows[0]["FLDDEBITNOTEREFERENCE"].ToString();
                    ViewState["Ownerid"] = dt.Rows[0]["FLDOWNERID"].ToString();                    
                }
            }

            if (ViewState["PendingYN"] != null)              
                 lnkNumber.Text = ViewState["PendingYN"].ToString().Equals("1") ? "Pending Queries" : "Closed Queries";            
            else                
                 lnkNumber.Text = "Pending Queries";
            BindData();            
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
            if (CommandName.ToUpper().Equals("CLOSED"))
            {
                Response.Redirect("../Accounts/AccountsSoaGenerationQueriesView.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&debitnoteid=" + ViewState["debitnotereferenceid"]+ "&PendingYN=0", true);
            }
            if (CommandName.ToUpper().Equals("PENDING"))
            {
                Response.Redirect("../Accounts/AccountsSoaGenerationQueriesView.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&debitnoteid=" + ViewState["debitnotereferenceid"]+"&PendingYN=1", true);
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

        ds = PhoenixAccountsSOAGeneration.SOAGenerationViewQueries(new Guid(ViewState["debitnotereferenceid"].ToString()), int.Parse(ViewState["accountid"].ToString()),
                                        int.Parse(ViewState["PendingYN"]==null ? "1": ViewState["PendingYN"].ToString()), (int)ViewState["PAGENUMBER"]
                                                , (int)ViewState["RECORDNUMBER"]
                                                , ref iRowCount
                                                , ref iTotalPageCount);


        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["DtKey"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            ViewState["VoucherDtKey"] = ds.Tables[0].Rows[0]["FLDVOUCHERDTKEY"].ToString();          

            gvOwnersAccount.DataSource = ds;
            gvOwnersAccount.DataBind();
            OnDataBound(null, null);
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvOwnersAccount);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Order Form List", alCaptions, alColumns, ds);

    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            gvOwnersAccount.SelectedIndex = -1;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    protected void gvOwnersAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            ImageButton cmdDetails = (ImageButton)e.Row.FindControl("cmdDetails");
            if (cmdDetails != null)
            {
                cmdDetails.Visible = true;
                RadLabel lblDtkey = (RadLabel)e.Row.FindControl("lblDtkey");
                RadLabel lblVoucherDtkey = (RadLabel)e.Row.FindControl("lblVoucherDtkey");
                ViewState["DtKey"] = lblDtkey.Text;
                ViewState["VoucherDtKey"] = lblVoucherDtkey.Text;
                cmdDetails.Attributes.Add("onclick", "openNewWindow('codehelp','','"+Session["sitepath"]+"/Accounts/AccountsSoaCheckingLineItemDetails.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"]
                    + "&dtkey=" + ViewState["DtKey"] + "&voucherdtkey=" + ViewState["VoucherDtKey"] + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "');return true;");
            }        
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {        
        BindData();
    }

    protected void OnDataBound(object sender, EventArgs e)
    {
        string currtext = string.Empty;
        string prevtext = string.Empty;
        Table tbl = ((Table)(gvOwnersAccount.Controls[0]));
        for (int i = tbl.Rows.Count - 2; i > 1; i--)
        {
            TableRow row = tbl.Rows[i];
            TableRow previousRow = tbl.Rows[i - 1];
            for (int j = 0; j < row.Cells.Count - 1; j++)
            {
                if (row.Cells.Count != previousRow.Cells.Count) continue;
                if (row.Cells[j].Controls[0].GetType().Name == "DataBoundLiteralControl")
                {
                    currtext = ((DataBoundLiteralControl)row.Cells[j].Controls[0]).Text.Trim().ToString();
                    prevtext = (previousRow.Cells[j].Controls.Count == 0) ? "" : ((DataBoundLiteralControl)previousRow.Cells[j].Controls[0]).Text.Trim().ToString();
                }
                else
                {
                    currtext = ((LiteralControl)row.Cells[j].Controls[0]).Text.Trim();
                    prevtext = ((LiteralControl)previousRow.Cells[j].Controls[0]).Text.Trim();
                }
                if (currtext == string.Empty) continue;
                if (currtext == prevtext)
                {
                    if (previousRow.Cells[j].RowSpan == 0)
                    {
                        if (row.Cells[j].RowSpan == 0)
                        {
                            previousRow.Cells[j].RowSpan += 2;
                            previousRow.Cells[j+1].RowSpan += 2;                            
                            previousRow.Cells[j+3].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            previousRow.Cells[j+1].RowSpan = row.Cells[j+1].RowSpan + 1;                            
                            previousRow.Cells[j+3].RowSpan = row.Cells[j+3].RowSpan + 1;
                        }
                        row.Cells[j+1].Visible = false;
                        row.Cells[j+3].Visible = false;
                    }
                }
            }
        }
    }
    
 
}
