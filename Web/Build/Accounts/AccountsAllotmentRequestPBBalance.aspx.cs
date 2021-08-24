using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Accounts;

public partial class AccountsAllotmentRequestPBBalance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                ViewState["ALLOTMENTID"]    = "";
                ViewState["ALLOTMENTTYPE"] = "";
                ViewState["VESSELID"]       = "";
                ViewState["CHECKTYPE"]      = "1";
                ViewState["EMPLOYEEID"]     = "";                                               

                if (Request.QueryString["ALLOTMENTID"] != null)
                    ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();
                if (Request.QueryString["CHECKTYPE"] != null)
                    ViewState["CHECKTYPE"] = Request.QueryString["CHECKTYPE"].ToString();

                BindData();
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
        DataTable dtPB = new DataTable();

        dtPB = PhoenixAccountsAllotmentRequestSystemChecking.CrewBPBalanceAndRemarksSearch(General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()));

        if (dtPB.Rows.Count > 0)
        {
            gvPBBalance.DataSource = dtPB;
            gvPBBalance.DataBind();         
            ViewState["ALLOTMENTTYPE"] = dtPB.Rows[0]["FLDALLOTMENTTYPE"].ToString();

            ViewState["VESSELID"] = dtPB.Rows[0]["FLDVESSELID"].ToString();
            ViewState["EMPLOYEEID"] = dtPB.Rows[0]["FLDEMPLOYEEID"].ToString();          

        }
        else
        {
            ShowNoRecordsFound(dtPB, gvPBBalance);
        }
    }

    protected void gvPBBalance_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string dtkey = "";
                string remarksid = "";
                string remarks = "";
                string allotmenttype = "";
                string amount = "";
                string vslamount ="";

                dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
                remarksid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRemarksId")).Text;
                allotmenttype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentType")).Text;
                remarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBankVerificationRemarks")).Text;
                amount = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOfficeFinalBal")).Text;
                vslamount = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselFinalBal")).Text;

                if (General.GetNullableString(remarks) == null)
                {
                    ucError.ErrorMessage = "Remarks Required";
                    ucError.Visible = true;
                    return;
                }
                if (allotmenttype == PhoenixCommonRegisters.GetHardCode(1, 239, "SOF"))
                {
                    if (General.GetNullableDecimal(vslamount) != null && General.GetNullableDecimal(amount)!= null)
                    {
                        if (General.GetNullableDecimal(vslamount) != General.GetNullableDecimal(amount))
                        {
                            ucError.ErrorMessage = "For signoff allotment office balance should be matched with vessel balance";
                            ucError.Visible = true;
                            return;
                        }
                        if (General.GetNullableDecimal(vslamount) < 0 ||  General.GetNullableDecimal(amount)<0)
                        {
                            ucError.ErrorMessage = "For signoff allotment amount PB balance amount should be positive,please create recovery";
                            ucError.Visible = true;
                            return;
                        }
                    }                   
                    else
                    {
                        ucError.ErrorMessage = "For signoff allotment office balance should be matched with vessel balance";
                        ucError.Visible = true;
                        return;
                    }
                }
                if (allotmenttype == PhoenixCommonRegisters.GetHardCode(1, 239, "MAL") || allotmenttype == PhoenixCommonRegisters.GetHardCode(1, 239, "SPA"))
                {
                    if (General.GetNullableDecimal(amount) == null)
                    {
                        ucError.ErrorMessage = "Office PB final balance is missing";
                        ucError.Visible = true;
                        return;
                    }
                }
                if (General.GetNullableGuid(remarksid) == null)
                {
                    PhoenixAccountsAllotmentRequestSystemChecking.AllotmentCheckingRemarksInsert(
                           General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString())
                         //  , General.GetNullableInteger(allotmenttype)
                           , General.GetNullableGuid(dtkey)
                           //, General.GetNullableInteger(ViewState["VESSELID"].ToString())
                           //, General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString())
                           , General.GetNullableInteger(ViewState["CHECKTYPE"].ToString())
                           , General.GetNullableString(remarks));
                }
                else
                {
                    PhoenixAccountsAllotmentRequestSystemChecking.AllotmentCheckingRemarksUpdate(
                             General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString())
                             , General.GetNullableGuid(remarksid)
                             //, General.GetNullableInteger(ViewState["VESSELID"].ToString())
                             //, General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString())
                             , General.GetNullableInteger(ViewState["CHECKTYPE"].ToString())
                             , General.GetNullableString(remarks));
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                _gridView.EditIndex = -1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPBBalance_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = de.NewEditIndex;
            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPBBalance_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvPBBalance_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {        

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
