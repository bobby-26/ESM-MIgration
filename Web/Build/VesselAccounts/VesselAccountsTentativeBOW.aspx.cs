using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;

public partial class VesselAccountsTentativeBOW : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO");
            MenuBOWMain.AccessRights = this.ViewState;
            MenuBOWMain.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {


                ddlEmployee.DataBind();

                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBOWMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("GO"))
            {
                if (!IsValidBOW(txtDate.Text, ddlEmployee.SelectedEmployee))
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            if (!String.IsNullOrEmpty(ddlEmployee.SelectedEmployee))
            {
                DataTable dt = PhoenixVesselAccountsEmployee.EditVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID, Convert.ToInt32(ddlEmployee.SelectedEmployee));
                if (dt.Rows.Count > 0)
                {
                    DateTime signondate = Convert.ToDateTime(dt.Rows[0]["FLDSIGNONDATE"].ToString());

                    DataSet ds = PhoenixVesselAccountsBOW.CalculateBOW(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                     , General.GetNullableInteger(ddlEmployee.SelectedEmployee)
                                                     , signondate
                                                     , Convert.ToDateTime(txtDate.Text.Trim()));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvBOW.DataSource = ds.Tables[0];
                        gvBOW.DataBind();
                    }
                    else
                    {
                        ShowNoRecordsFound(ds.Tables[0], gvBOW);
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


    protected void gvBOW_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ViewState["TOTALAMOUNT"] = 0.00;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbltype = (Label)e.Row.FindControl("lblEarnDeduct");
                Label lblWage = (Label)e.Row.FindControl("lblWage");
                if (lbltype != null)
                {
                    if (lbltype.Text == "-1")
                    {
                        ViewState["TOTALAMOUNT"] = (decimal.Parse(ViewState["TOTALAMOUNT"].ToString()) - decimal.Parse(String.IsNullOrEmpty(lblWage.Text.Trim()) ? "0.00" : lblWage.Text.Trim()));
                        e.Row.BackColor = System.Drawing.Color.LightPink;
                    }
                    else
                    {
                        ViewState["TOTALAMOUNT"] = (decimal.Parse(ViewState["TOTALAMOUNT"].ToString()) + decimal.Parse(String.IsNullOrEmpty(lblWage.Text.Trim()) ? "0.00" : lblWage.Text.Trim()));
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbTotal = (Label)e.Row.FindControl("lblTotalAmountfooter");
                if (lbTotal != null) lbTotal.Text = decimal.Parse(ViewState["TOTALAMOUNT"].ToString()).ToString("######0.00");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidBOW(string date, string employeeid)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && resultDate < DateTime.Today)
        {
            ucError.ErrorMessage = "Date should be later than today date";
        }
        if (!General.GetNullableInteger(employeeid).HasValue)
        {
            ucError.ErrorMessage = "Select Employee from the list.";
        }
        return (!ucError.IsError);
    }
}
