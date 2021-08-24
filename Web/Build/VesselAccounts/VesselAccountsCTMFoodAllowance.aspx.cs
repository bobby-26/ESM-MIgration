using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using System.IO;

public partial class VesselAccountsCTMFoodAllowance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("Crew List", "CREWLIST");
            toolbarmain.AddButton("List", "LIST");
            MenuCTMMain.AccessRights = this.ViewState;
            MenuCTMMain.MenuList = toolbarmain.Show();
            MenuCTMMain.SelectedMenuIndex = 1;
            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuSub.AccessRights = this.ViewState;
            MenuSub.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["CTMID"] = Request.QueryString["CTMID"];
                ViewState["ACTIVEYN"] = Request.QueryString["a"];
                gvCTM.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        try
        {

            DataTable dt = PhoenixVesselAccountsCTM.ListCTMFoodAllowance(new Guid(ViewState["CTMID"].ToString()));
            gvCTM.DataSource = dt;
            gvCTM.VirtualItemCount = dt.Rows.Count;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void Rebind()
    {
        gvCTM.SelectedIndexes.Clear();
        gvCTM.EditIndexes.Clear();
        gvCTM.DataSource = null;
        gvCTM.Rebind();
    }
    protected void gvCTM_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName == "Save")
            {

                RadLabel EmployeeiD = (RadLabel)e.Item.FindControl("lblEmployeeid");
                RadLabel CaptainCashId = (RadLabel)e.Item.FindControl("lblCaptainCashId");
                RadLabel SignonoffId = (RadLabel)e.Item.FindControl("lblSignonoffid");
                UserControlMaskNumber Amount = (UserControlMaskNumber)e.Item.FindControl("txtAmount");
                RadTextBox Purpose = (RadTextBox)e.Item.FindControl("txtRemarks");
                RadLabel Bowid = (RadLabel)e.Item.FindControl("lblbowid");
                if (!IsCTMValid(Amount.Text, Purpose.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselAccountsCTM.InsertUpdateCCFoodAllowance(new Guid(CaptainCashId.Text), int.Parse(EmployeeiD.Text), decimal.Parse(Amount.Text), General.GetNullableGuid(Bowid.Text), General.GetNullableString(Purpose.Text), int.Parse(SignonoffId.Text));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    decimal r;
    protected void gvCTM_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridHeaderItem)
            r = 0;
        if (e.Item is GridEditableItem)
        {
            r = r + decimal.Parse((drv["FLDAMOUNT"].ToString() != string.Empty ? drv["FLDAMOUNT"].ToString() : "0"));
        }
        if (e.Item is GridFooterItem)
        {
            e.Item.Cells[5].Text = r.ToString();
        }
    }


    private bool IsCTMValid(string amount, string Purpose)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(amount).HasValue)
            ucError.ErrorMessage = "Amount is required.";
        if (Purpose == null || Purpose == "")
            ucError.ErrorMessage = "Purpose is required.";

        return (!ucError.IsError);
    }
    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvCTM$ctl00$ctl02$ctl01$chkSelectAll")
        {
            RadCheckBox chkAll = new RadCheckBox();
            foreach (GridHeaderItem headerItem in gvCTM.MasterTableView.GetItems(GridItemType.Header))
            {
                chkAll = (RadCheckBox)headerItem["Listcheckbox"].FindControl("chkSelectAll"); // Get the header checkbox
            }
            if (chkAll != null)
            {
                foreach (GridDataItem gv in gvCTM.Items)
                {
                    RadCheckBox sel = (RadCheckBox)gv.FindControl("chkSelect");
                    if (sel != null)
                    {
                        if (chkAll.Checked == true)
                            sel.Checked = true;
                        else
                            sel.Checked = false;
                    }
                }
            }
        }
    }
    protected void MenuCTMMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL") || ViewState["CTMID"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMGeneral.aspx";
            }
            else if (CommandName.ToUpper().Equals("DENOMINATOIN"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMDenomination.aspx";
            }
            else if (CommandName.ToUpper().Equals("CREWLIST"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMFoodAllowance.aspx";
            }
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsCTM.aspx", false);
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
    protected void MenuSub_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                DataSet ds = new DataSet();
                DataTable table = new DataTable();
                table.Columns.Add("FLDBOWID", typeof(Guid));
                table.Columns.Add("FLDEMPLOYEEID", typeof(int));
                table.Columns.Add("FLDSIGNONOFFID", typeof(int));

                foreach (GridDataItem gv in gvCTM.Items)
                {
                    RadCheckBox sel = (RadCheckBox)gv.FindControl("chkSelect");
                    if (sel.Checked == true)
                    {
                        // dtkey += ((RadLabel)gv.FindControl("lbldtKey")).Text + ",";
                        string ID = ((RadLabel)gv.FindControl("lblbowid")).Text;
                        string lblEmployeeid = ((RadLabel)gv.FindControl("lblEmployeeid")).Text;
                        string lblSignonoffid = ((RadLabel)gv.FindControl("lblSignonoffid")).Text;
                        table.Rows.Add(General.GetNullableGuid(ID), lblEmployeeid, lblSignonoffid);
                    }
                }
                ds.Tables.Add(table);
                StringWriter sw = new StringWriter();
                ds.WriteXml(sw);
                string resultstring = sw.ToString();
                PhoenixVesselAccountsCTM.BulkInsertUpdateCCFoodAllowance(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["CTMID"].ToString()), Decimal.Parse(txtAmount.Text), txtRemarks.Text, resultstring);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
