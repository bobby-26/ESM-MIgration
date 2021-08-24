using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewPortagebillHistory : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Summary", "LIST");
            toolbar.AddButton("Bond Issue & Phone Cards", "BONDPHONECARD");
            toolbar.AddButton("Earnings & Deductions", "EARNINGDEDUCTION");
            toolbar.AddButton("List", "BACK");
            MenuCrewContract.AccessRights = this.ViewState;
            MenuCrewContract.MenuList = toolbar.Show();
            MenuCrewContract.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                SetEmployeePrimaryDetails(Request.QueryString["empid"].ToString());
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewContract_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("BONDPHONECARD"))
            {
                Response.Redirect("../Crew/CrewBondSummary.aspx?empid=" + Request.QueryString["empid"].ToString() + "&Portagebillid=" + Request.QueryString["Portagebillid"].ToString(), false);
            }
            else if (dce.CommandName.ToUpper().Equals("EARNINGDEDUCTION"))
            {
                Response.Redirect("../Crew/CrewEarningdeductionSummary.aspx?empid=" + Request.QueryString["empid"].ToString() + "&Portagebillid=" + Request.QueryString["Portagebillid"].ToString(), false);
            }
            else if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewPortagebillHistory.aspx?empid=" + Request.QueryString["empid"].ToString() + "&Portagebillid=" + Request.QueryString["Portagebillid"].ToString(), false);
            }
            else if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Crew/CrewEmployeePortageBillList.aspx?empid=" + Request.QueryString["empid"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeePrimaryDetails(string empid)
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(empid));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
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
        DataSet ds1 = new DataSet();
        ds1 = PhoenixCrewPortagebill.CrewPortagebillBreakUp(int.Parse(Request.QueryString["empid"].ToString()), new Guid(Request.QueryString["Portagebillid"].ToString()));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            gvPortagebill.DataSource = ds1.Tables[0];
            gvPortagebill.DataBind();
        }
        else
        {
            ShowNoRecordsFound(ds1.Tables[0], gvPortagebill);
        } if (ds1.Tables[1].Rows.Count > 0)
        {
            txtvessel.Text = ds1.Tables[1].Rows[0]["FLDVESSELNAME"].ToString();
            txtTodate.Text =ds1.Tables[1].Rows[0]["FLDTODATE"].ToString();
            txtfromdate.Text =  ds1.Tables[1].Rows[0]["FLDFROMDATE"].ToString();
            txtdays.Text = ds1.Tables[1].Rows[0]["FLDDAYS"].ToString();
            txtsignonRank.Text = ds1.Tables[1].Rows[0]["FLDRANKNAME"].ToString();
        }
    }
    decimal r, p;
    protected void gvPortagebill_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            r = 0;
            p = 0;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (drv["FLDEARNINGDEDUCTION"].ToString() != string.Empty && ",1,".Contains("," + drv["FLDEARNINGDEDUCTION"].ToString() + ",")) r = r + decimal.Parse(drv["FLDAMOUNT"].ToString());
            if (drv["FLDEARNINGDEDUCTION"].ToString() != string.Empty && !",1,".Contains("," + drv["FLDEARNINGDEDUCTION"].ToString() + ",")) p = p + decimal.Parse(drv["FLDAMOUNT"].ToString());
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = r.ToString();
            e.Row.Cells[2].Text = p.ToString();
            txtDeduction.Text = p.ToString();
            txtEarning.Text = r.ToString();
            txtbalance.Text = (r - p).ToString();
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
    }

}
