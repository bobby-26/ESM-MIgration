using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsVesselStatutoryDuesGroupbyMonth : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Head Count", "HEADCOUNT");
            toolbarmain.AddButton("Monthly", "NOOFDAYS");
            toolbarmain.AddButton("Sign Off", "SIGNONOFF");
            toolbarmain.AddButton("Summary For Month", "GROUPBYMONTH");
            toolbarmain.AddButton("Consolidated", "CBANOOFDAYS");
            MenuStatoryDuesMain.AccessRights = this.ViewState;
            MenuStatoryDuesMain.MenuList = toolbarmain.Show();
            MenuStatoryDuesMain.SelectedMenuIndex = 3;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsVesselStatutoryDuesGroupbyMonth.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageButton("../Accounts/AccountsVesselStatutoryDuesGroupbyMonth.aspx", "Find", "search.png", "FIND");
            MenuStock.AccessRights = this.ViewState;
            MenuStock.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["CLOSINGDATE"] = null;                              
                ddlComponent.Items.Clear();
                ddlComponent.Items.Insert(0, new RadComboBoxItem("-- No Component Found --", ""));
                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                ddlYear.SelectedValue = DateTime.Now.Year.ToString();
                ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStatoryDuesMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("HEADCOUNT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesHeadCount.aspx";
            }
            if (CommandName.ToUpper().Equals("NOOFDAYS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesNoofDays.aspx";
            }
            if (CommandName.ToUpper().Equals("CBANOOFDAYS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesVesselWiseNoofCrew.aspx";
            }
            if (CommandName.ToUpper().Equals("GROUPBYMONTH"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesGroupbyMonth.aspx";
            }
            if (CommandName.ToUpper().Equals("SIGNONOFF"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesSignOffFormat1.aspx";
            }
            Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString(), false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStatoryDues_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("POST"))
            {
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        PopulateComponent();
    }
    private void PopulateComponent()
    {
        try
        {

            ddlComponent.Items.Clear();
            DataTable dt = PhoenixAccountsStatutoryDuesReports.ListStatutoryDuesComponent(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                            , General.GetNullableInteger(ddlMonth.SelectedValue)
                                                            , General.GetNullableInteger(ddlYear.SelectedValue)
                                                            , null);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    RadComboBoxItem item = new RadComboBoxItem(dr["FLDCOMPONENTNAME"].ToString(), dr["FLDCOMPONENTID"].ToString());
                    item.Attributes["OptionGroup"] = dr["FLDUNION"].ToString();
                    ddlComponent.Items.Add(item);
                }
                ddlComponent.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            }
            else
            {
                ddlComponent.Items.Insert(0, new RadComboBoxItem("-- No Component Found --", ""));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlComponent_Changed(object sender, EventArgs e)
    {
        string val = ddlComponent.SelectedValue;
        PopulateComponent();
        ddlComponent.SelectedValue = val;
        BindData();
    }
    protected void MenuStock_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                gvStock.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                DataSet ds = PhoenixAccountsStatutoryDuesReports.StatutoryDuesMFSWOwnerSearch(General.GetNullableGuid(ddlComponent.SelectedValue), General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                       , General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue));
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=StatutoryDues.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table>");
                stringwriter.Write("<tr>");
                stringwriter.Write("<td colspan='2' rowspan='2'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                stringwriter.Write("<td colspan=5><h3>" + ddlVessel.SelectedVesselName + " " + ddlMonth.SelectedItem.Text + " - " + ddlYear.SelectedItem.Text + "</h3></td>");
                stringwriter.Write("</tr><tr><td colspan=5><h3>" + ddlComponent.SelectedItem.Text.Substring(0, (ddlComponent.SelectedItem.Text.Length - 25)) + "</h3></td></tr>");
                stringwriter.Write("</table>");
                System.IO.StringWriter stringwriter1 = new System.IO.StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(stringwriter1);

                gvStock.RenderBeginTag(htw);
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].RenderControl(htw);
                foreach (GridDataItem row in gvStock.Items)
                {
                    row.RenderControl(htw);
                }
                gvStock.MasterTableView.GetItems(GridItemType.Footer)[0].RenderControl(htw);
                gvStock.RenderEndTag(htw);
                Response.Write(stringwriter + stringwriter1.ToString().Replace("table", "table border =\"1\"").Replace("td", "td valign=\"top\""));
                Response.End();
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
            DataSet ds = PhoenixAccountsStatutoryDuesReports.StatutoryDuesMFSWOwnerSearch(General.GetNullableGuid(ddlComponent.SelectedValue), General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                    , General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue));
            gvStock.DataSource = ds.Tables[0];

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
      
    
    protected void gvStock_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvStock_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (drv != null)
        {

            if (e.Item is GridHeaderItem)
            {


                gvStock.DataBind();
                ViewState["CLOSINGDATE"] = drv["FLDCLOSINGDATE"].ToString();
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].Cells[3].Text = "Rate per month (" + drv["FLDCURRENCYCODE"].ToString() + ")";
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].Cells[4].Text = "Amount  (" + drv["FLDCURRENCYCODE"].ToString() + ")";
            }
        }
        else
        {
            ViewState["RECORDCOUNT"] = "0";
            
        }
    }
}
