using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class AccountsOfficeAccrualAllotmentRequestDraftViewNotinPB : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Post", "POST");            
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
               
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("POST"))
            {
                if (!IsValidAllotment(txtVoucherDate.Text, txtDescription.Text))
                {
                    ucError.Visible = false;
                    return;
                }
                NameValueCollection nvc = Filter.CurrentOfficeAccrualCrew;
                int employeeid = int.Parse(nvc.Get("empid"));
                Guid componentid = new Guid(nvc.Get("cid"));
                string componentname = txtComponent.Text;
                decimal amt = decimal.Parse(nvc.Get("amt"));
                int acctid = int.Parse(nvc.Get("aid"));
                int vslid = int.Parse(nvc.Get("vslid"));
                int sgid = int.Parse(nvc.Get("sgid"));

                PhoenixAccountsOfficeAccrual.InsertOfficeAccrualAllotmentRequest(employeeid, componentid, componentname
                    , amt, acctid, vslid, null, 0, DateTime.Parse(txtVoucherDate.Text), txtDescription.Text, null, sgid);
                BindData();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "javascript:fnReloadList('codehelp1');", true);
            }            
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
            NameValueCollection nvc = Filter.CurrentOfficeAccrualCrew;
            DataSet ds = PhoenixAccountsOfficeAccrual.ListOfficeAccrualAllotmentDraftView(int.Parse(nvc.Get("empid"))
                                                , new Guid(nvc.Get("cid"))
                                                , int.Parse(nvc.Get("aid"))
                                                , int.Parse(nvc.Get("vslid"))
                                                , 0
                                                , decimal.Parse(nvc.Get("amt")));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPB.DataSource = ds;
                gvPB.DataBind();
                SetPrimaryData(ds.Tables[1]);
            }
            else
            {                
                ShowNoRecordsFound(ds.Tables[0], gvPB);                
            }                 
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void gvPB_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
        }
    }
    private void SetPrimaryData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtFileNo.Text = dr["FLDFILENO"].ToString();
            txtFileNo.ToolTip = dr["FLDFILENO"].ToString();
            txtVesselAccount.Text = dr["FLDDESCRIPTION"].ToString();
            txtVesselAccount.ToolTip = dr["FLDDESCRIPTION"].ToString();
            txtRank.Text = dr["FLDEMPLOYEENAME"].ToString();
            txtRank.ToolTip = dr["FLDEMPLOYEENAME"].ToString();
            txtComponent.Text = dr["FLDCOMPONENTNAME"].ToString();
            txtComponent.ToolTip = dr["FLDCOMPONENTNAME"].ToString();
            txtVoucherDate.Text = DateTime.Now.ToString();
            txtDescription.Text = "ALLOTMENT REQUEST FOR ex- [Name] [Component].".Replace("[Name]",dr["FLDEMPLOYEENAME"].ToString()).Replace("[Component]",dr["FLDCOMPONENTNAME"].ToString());
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidAllotment(string date, string description)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDateTime(date).HasValue)
            ucError.ErrorMessage = "Voucher Date is required.";
        if (description.Trim() == string.Empty)
            ucError.ErrorMessage = "Voucher Description is required.";      

        return (!ucError.IsError);
    }
}
