using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Framework;
using System.Globalization;
using System.Web.UI;
using Telerik.Web.UI;

public partial class VesselAccountAdminPortagebillList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("PB Correction", "PBCORRECTION");
            toolbar.AddButton("Leave Wages Correction", "LEAVEWAGECORRECTION");
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbar.Show();
            MenuPB.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
                PopulateLockedPortageBill();
                gvAdminPB.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LEAVEWAGECORRECTION"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountAdminPortagebillList.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("PBCORRECTION"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsEmployeePortageBillUpdate.aspx", false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void PopulateLockedPortageBill()
    {
        DataTable dt = PhoenixVesselAccountsPortageBill.ListPortageBillLocked(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlHistory.DataSource = dt;
        ddlHistory.DataTextFormatString = "{0:" + CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern + "}";
        ddlHistory.DataBind();
        ddlHistory.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ddlHistory_SelectedIndexChanged(object sender, EventArgs e)
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
    protected void Rebind()
    {
        gvAdminPB.SelectedIndexes.Clear();
        gvAdminPB.EditIndexes.Clear();
        gvAdminPB.DataSource = null;
        gvAdminPB.Rebind();
    }
    private void BindData()
    {
        DataTable dt = PhoenixVesselAccountsAdminPB.FetchPBEmployeeList(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            , General.GetNullableGuid(ddlHistory.SelectedValue));
        gvAdminPB.DataSource = dt;
        gvAdminPB.VirtualItemCount = dt.Rows.Count;
    }
    protected void gvAdminPB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvAdminPB_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          if (e.CommandName.ToUpper().Equals("CORRECTION"))
            {
             
                RadLabel lblclosingdate = ((RadLabel)e.Item.FindControl("lblclosingdate"));
                RadLabel lblSignoffyn = ((RadLabel)e.Item.FindControl("lblSignoffyn"));
                RadLabel lblPBid = ((RadLabel)e.Item.FindControl("lblPBid"));
                RadLabel lblsignonoffid = ((RadLabel)e.Item.FindControl("lblsignonoffid"));
                int Vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

                PhoenixVesselAccountsAdminPB.FetchPBLeaveWagesCorrection(Vesselid, new Guid(lblPBid.Text), int.Parse(lblsignonoffid.Text)
                                                          , int.Parse(lblSignoffyn.Text), DateTime.Parse(lblclosingdate.Text));
                Rebind();
                // int iRowno;
                //iRowno = int.Parse(e.CommandArgument.ToString());
                //Label lblclosingdate = ((Label)gvAdminPB.Rows[iRowno].FindControl("lblclosingdate"));
                //Label lblSignoffyn = ((Label)gvAdminPB.Rows[iRowno].FindControl("lblSignoffyn"));
                //Label lblPBid = ((Label)gvAdminPB.Rows[iRowno].FindControl("lblPBid"));
                //Label lblsignonoffid = ((Label)gvAdminPB.Rows[iRowno].FindControl("lblsignonoffid"));
                //ImageButton cmdBDetails = (ImageButton)gvAdminPB.Rows[iRowno].FindControl("cmdBDetails");
                //if (cmdBDetails != null)
                //{
                //    if (cmdBDetails.ImageUrl.Contains("sidearrow.png"))
                //        ViewState["SIGNOFFID"] = lblsignonoffid.Text;
                //    else if (cmdBDetails.ImageUrl.Contains("downarrow.png"))
                //        ViewState["SIGNOFFID"] = null;
                //}
                //DataBind();
                //if (ViewState["SIGNOFFID"] != null)
                //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>AjxGet('" + Session["sitepath"] + "/VesselAccounts/VesselAccountAdminLeaveWages.aspx?PBID=" + lblPBid.Text + "&SIGNONOFFID=" + lblsignonoffid.Text + "&SIGNOFFYN=" + lblSignoffyn.Text + "&CLOSINGDATE=" + lblclosingdate.Text + "', 'div" + ViewState["SIGNOFFID"].ToString() + "', false);</script>", false);
                // MenuOrderFormMain.SelectedMenuIndex = 1;
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
    protected void RadGrid1_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
    {
        //GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        //switch (e.DetailTableView.Name)
        //{
        //    case "Orders":
        //        {
        //            string CustomerID = dataItem.GetDataKeyValue("CustomerID").ToString();
        //            e.DetailTableView.DataSource = GetDataTable("SELECT * FROM Orders WHERE CustomerID = @customerID", "customerID", CustomerID);
        //            break;
        //        }

        //    case "OrderDetails":
        //        {
        //            string OrderID = dataItem.GetDataKeyValue("OrderID").ToString();
        //            e.DetailTableView.DataSource = GetDataTable("SELECT * FROM [Order Details] WHERE OrderID = @orderID", "orderID", OrderID);
        //            break;
        //        }
        //}
    }

    protected void gvAdminPB_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
    }
    protected void gvAdminPB_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridEditableItem)
        //{
        //    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
        //    if (db != null)
        //    {
        //        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        //        db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
        //    }
        //    LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        //    if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        //    LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
        //    if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

        //    LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
        //    if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        //}
    }
   
    private void AddNewRow(RadGrid gv, int row, string id, int headercount)
    {
        GridViewRow newRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
        TableCell cell = new TableCell();
        // cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = gv.Columns.Count;
        cell.Text = "<div id='div" + id + "'></div>";
        newRow.Cells.Add(cell);
        gv.Controls[0].Controls.AddAt(row + headercount + 1, newRow);
    } 
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void gvAdminPB_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        string PBID = dataItem.GetDataKeyValue("FLDPORTAGEBILLID").ToString();
        string SIGNONOFFID = dataItem.GetDataKeyValue("FLDSIGNONOFFID").ToString();
        string SIGNOFFYN = dataItem.GetDataKeyValue("FLDSIGNEDOFFYN").ToString();
        string CLOSINGDATE = dataItem.GetDataKeyValue("FLDCLOSINGDATE").ToString();
        DataSet ds = PhoenixVesselAccountsAdminPB.FetchPBLeaveWagesList(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                          , new Guid(PBID)
                                                          , int.Parse(SIGNONOFFID)
                                                          , int.Parse(SIGNOFFYN)
                                                          , DateTime.Parse(CLOSINGDATE));
        DataTable dtAll = new DataTable();
        dtAll = ds.Tables[0].Copy();
        dtAll.Merge(ds.Tables[1]);
        e.DetailTableView.DataSource = dtAll;

    }
}
