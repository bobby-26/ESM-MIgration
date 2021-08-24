using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsAdvanceRemittanceInstructionLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Post Remittance", "APPROVE");
            toolbarmain.AddButton("Export to CSV", "EXPORTCSV");

            MenuRemittanceInstructionMain.AccessRights = this.ViewState;
            MenuRemittanceInstructionMain.MenuList = toolbarmain.Show();
            MenuRemittanceInstructionMain.SetTrigger(pnlRemittanceInstruction);

            if (!IsPostBack)
            {
                if ((Request.QueryString["REMITTENCEID"] != null) && (Request.QueryString["REMITTENCEID"] != ""))
                {
                    ViewState["Remittenceid"] = Request.QueryString["REMITTENCEID"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            BindHeader();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindHeader()
    {
        DataSet ds = PhoenixAccountsAdvanceRemittance.Listremittance(ViewState["Remittenceid"].ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtRemittancNumber.Text = dr["FLDREMITTANCENUMBER"].ToString();
            txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
            txtAccountDescription.Text = dr["FLDDESCRIPTION"].ToString();
            txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
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

        string remittanceid = (ViewState["Remittenceid"] == null) ? null : (ViewState["Remittenceid"].ToString());

        ds = PhoenixAccountsAdvanceRemittance.RemittanceInstructionLineItemSearch(remittanceid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRemittanceInstructionLineItem.DataSource = ds;
            gvRemittanceInstructionLineItem.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvRemittanceInstructionLineItem);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void MenuRemittanceInstructionMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("APPROVE"))
            {

                PhoenixAccountsRemittance.PostRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                          , ViewState["Remittenceid"].ToString(), PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            }
            if (dce.CommandName.ToUpper().Equals("EXPORTCSV"))
            {
                string remittanceid = (ViewState["Remittenceid"] == null) ? null : (ViewState["Remittenceid"].ToString());
                DataSet ds = new DataSet();
                ds = PhoenixAccountsRemittance.RemittanceInstructionSupplierPayableAmountCSV(remittanceid);
                if (ds.Tables[0].Rows.Count > 0)
                {
                   // string strpath = HttpContext.Current.Request.MapPath("~/Attachments/ACCOUNTS/");
                    string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/ACCOUNTS/";
                    string filename = strpath + ds.Tables[0].Rows[0]["REMITTANCE NUMBER"].ToString().Replace("/", "-") + "_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                    CreateCSVFile(ds.Tables[0], filename);
                    InsertSupplierBankPayment(ds.Tables[0]);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertSupplierBankPayment(DataTable dt)
    {
        foreach (DataRow dr in dt.Rows)
        {
            PhoenixAccountsBankSupplierPayment.InsertBankSupplierPayment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null,
                            dr[1].ToString(), null,
                            dr[3].ToString(), dr[4].ToString(),
                            null, null,
                            General.GetNullableDateTime(dr[6].ToString()), dr[7].ToString(),
                            General.GetNullableDecimal(dr[8].ToString()), null);
        }
    }

    public void CreateCSVFile(DataTable dt, string strFilePath)
    {

        StreamWriter sw = new StreamWriter(strFilePath, false);
        int iColCount = dt.Columns.Count;

        // write the headers.
        for (int i = 1; i < iColCount; i++)
        {
            sw.Write(dt.Columns[i]);
            if (i < iColCount - 1)
            {
                sw.Write(",");
            }
        }
        sw.Write(sw.NewLine);

        //write all the rows.

        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 1; i < iColCount; i++)
            {
                if (!Convert.IsDBNull(dr[i]))
                {
                    sw.Write(dr[i].ToString());
                }
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
        }
        sw.Close();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
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

    protected void gvRemittanceInstructionLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        }
    }

    protected void gvRemittanceInstructionLineItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
    }

    protected void gvRemittanceInstructionLineItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
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

    protected void gvRemittanceInstructionLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
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

    protected void gvRemittanceInstructionLineItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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


    private bool IsValidRemittance(string accountcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int result;

        if (accountcode.Trim().Equals("Dummy"))
        {
            if (int.TryParse(accountcode, out result) == false)
                ucError.ErrorMessage = "Please select Account Code.";
        }
        return (!ucError.IsError);
    }
}
