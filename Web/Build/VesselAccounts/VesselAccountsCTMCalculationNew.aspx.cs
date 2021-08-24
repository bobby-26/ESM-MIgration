using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;

public partial class VesselAccounts_VesselAccountsCTMCalculationNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("BOW", "BOW");
            toolbarmain.AddButton("CTM", "CTMCAL");
            toolbarmain.AddButton("Denomination", "DENOMINATOIN");
            toolbarmain.AddButton("List", "LIST");
            MenuCTMMain.AccessRights = this.ViewState;
            MenuCTMMain.MenuList = toolbarmain.Show();
            MenuCTMMain.SelectedMenuIndex = 2;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            MenuCTM.AccessRights = this.ViewState;
            MenuCTM.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["CTMID"] = Request.QueryString["CTMID"];
                ViewState["ACTIVEYN"] = Request.QueryString["a"];
                ViewState["CSHONB"] = null;
                ViewState["CALAMT"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                SetCaptainCash(new Guid(ViewState["CTMID"].ToString()));
                if (ViewState["ACTIVEYN"].ToString() == "0") MenuCTM.Visible = false;
                txtCTM.Enabled = false;
            }
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {

                string cshonb = txtCashonBoard.Text;
                string CTMamt = txtCTM.Text;
                if ((String.IsNullOrEmpty(txtBalance.Text) ? 0 : decimal.Parse(txtBalance.Text)) > 0)
                {
                    if (!IsValidRoundOff(CTMamt, txtBalance.Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    string ramt = (decimal.Parse(txtBalance.Text) - decimal.Parse(CTMamt)).ToString();
                    PhoenixVesselAccountsCTMNew.UpdateCaptainCashCalculationRoundOff(new Guid(ViewState["CTMID"].ToString()), decimal.Parse(ramt), decimal.Parse(CTMamt));
                    SetCaptainCash(new Guid(ViewState["CTMID"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCTMMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GENERAL") || ViewState["CTMID"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMGeneralNew.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("BOW"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMBOWNew.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("DENOMINATOIN"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMDenominationNew.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("CTMCAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMCalculationNew.aspx";
            }
            if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsCTMNew.aspx", false);
            }
            else
                Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?CTMID=" + ViewState["CTMID"] + "&a=" + ViewState["ACTIVEYN"], false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {

        try
        {
            decimal d = 0;
            DataTable dt = PhoenixVesselAccountsCTMNew.ListCaptainCashCTMNew(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                        , new Guid(ViewState["CTMID"].ToString()), ref d);
            if (dt.Rows.Count > 0)
            {

                gvCTM.DataSource = dt;
                gvCTM.DataBind();
                txtCashonBoard.Text = d.ToString();
                txtOpening.Text = dt.Rows[0]["FLDOPENINGBALANCE"].ToString();
                txtBalance.Text = (decimal.Parse(ViewState["CALAMT"].ToString()) - d).ToString();

                {
                    if ((decimal.Parse(ViewState["CALAMT"].ToString()) - d) > 0)
                    {
                        txtCTM.Enabled = true;
                    }
                    else
                    {
                        if (ViewState["ACTIVEYN"].ToString() != "0")
                        {
                            ucError.ErrorMessage = "Short fall should be greater than Zero.";
                            ucError.Visible = true;
                        }
                        txtCTM.Enabled = false;
                    }
                    if (txtRoundOffAmount.Text == string.Empty)
                    {
                        txtRoundOffAmount.Text = String.IsNullOrEmpty(txtCTM.Text.Trim()) ? txtBalance.Text.Trim() : (decimal.Parse(txtCTM.Text.Trim()) - decimal.Parse(txtBalance.Text.Trim())).ToString();
                        //txtCTM.Text = (decimal.Parse(txtRoundOffAmount.Text) - d).ToString();
                    }
                }
            }
            else
            {
                ShowNoRecordsFound(dt, gvCTM);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    decimal r;
    protected void gvCTM_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            r = 0;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            if (drv["FLDTYPE"].ToString() == "2")
                if (ed != null) ed.Visible = false;
            if (ViewState["ACTIVEYN"].ToString() != "1")
            {
                if (ed != null) ed.Visible = false;
            }
            r = r + decimal.Parse(drv["FLDAMOUNT"].ToString());
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = r.ToString();
            ViewState["CALAMT"] = r.ToString();
            txtEstimate.Text = r.ToString();
        }
    }
    protected void gvCTM_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvCTM_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindData();
        ((TextBox)((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtAmount")).FindControl("txtNumber")).Focus();
    }
    protected void gvCTM_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid? id = General.GetNullableGuid(_gridView.DataKeys[nCurrentRow].Value.ToString());
            string type = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblType")).Text;
            string amt = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmount")).Text;
            if (!IsValidCTM(amt))
            {
                ucError.Visible = true;
                return;
            }
            if (!id.HasValue)
            {
                PhoenixVesselAccountsCTMNew.InsertCaptainCashCalculation(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["CTMID"].ToString())
                                                                            , byte.Parse(type), decimal.Parse(amt));
            }
            else
            {
                PhoenixVesselAccountsCTMNew.UpdateCaptainCashCalculation(id.Value, decimal.Parse(amt));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }
    private bool IsValidCTM(string amt)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDecimal(amt).HasValue)
        {
            ucError.ErrorMessage = "Amount is required.";
        }
        return (!ucError.IsError);
    }
    private bool IsValidRoundOff(string ctmAmt, string balance)
    {
        decimal ramt = 0;
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDecimal(ctmAmt).HasValue && balance != string.Empty)
        {
            ucError.ErrorMessage = "CTM to be arranged Amount is required.";
        }
        else
        {
            ramt = decimal.Parse(balance) - decimal.Parse(ctmAmt);
        }
        if (ramt > 1000 || ramt < -1000)
        {
            ucError.ErrorMessage = "CTM to be arranged cannot differ the short fall amount by more than $1000";
        }

        return (!ucError.IsError);
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
    private void SetCaptainCash(Guid CTMId)
    {
        DataTable dt = PhoenixVesselAccountsCTMNew.EditCTMRequest(CTMId);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDROUNDOFFAMOUNT"].ToString() != string.Empty)
            {
                txtRoundOffAmount.Text = dt.Rows[0]["FLDROUNDOFFAMOUNT"].ToString();
                txtCTM.Text = dt.Rows[0]["FLDAMOUNT"].ToString();
            }

        }
    }

}