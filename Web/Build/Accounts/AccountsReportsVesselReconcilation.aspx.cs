using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsReportsVesselReconcilation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {

                ViewState["ACCOUNT"] = "";
                ViewState["DEPT"] = "";
                BindCheckBoxList();
                EditUserAccessLevel();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "REPORT",ToolBarDirection.Right);

            MenuVesselReport.AccessRights = this.ViewState;
            MenuVesselReport.MenuList = toolbar.Show();

           // BindSelected();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVesselReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("REPORT"))
            {
                if (!IsValidAccountFilter(ViewState["ACCOUNT"].ToString(), ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELRECONCILATION&accountcode=" + ViewState["ACCOUNT"].ToString() + "&fromdate=" + ucFromDate.Text + "&todate=" + ucToDate.Text+
                    "&type=" + ddlType.SelectedValue, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCheckBoxList()
    {
        DataSet ds = new DataSet();

        try
        {
            ds = PhoenixRegistersAccount.VesselAccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                , txtAccountSearch.Text
                , ""
                , null
                , null
                , null
                , 1
                , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

            ds.Tables[0].Columns.Add("FLDACCOANDEPT");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["FLDACCOANDEPT"] = dr["FLDACCOUNTCODE"] + "-" + dr["FLDDESCRIPTION"];
            }

            cblAccount.DataTextField = "FLDACCOANDEPT";
            cblAccount.DataValueField = "FLDACCOUNTID";
            cblAccount.DataSource = ds;

            cblAccount.DataBind();
            SelectAccount();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindSelected()
    {
        DataSet ds = new DataSet();

        try
        {
            ds = PhoenixRegistersAccount.SelectedAccountSearch(ViewState["ACCOUNT"].ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSelectedAccount.DataSource = ds;
              
            }
            else
            {
                gvSelectedAccount.DataSource = ds;
                //ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                //gvSelectedAccount.DataSource = ds;
                //gvSelectedAccount.DataBind();

                //int colcount = gvSelectedAccount.Columns.Count;
                //gvSelectedAccount.Rows[0].Cells.Clear();
                //gvSelectedAccount.Rows[0].Cells.Add(new TableCell());
                //gvSelectedAccount.Rows[0].Cells[0].ColumnSpan = colcount;
                //gvSelectedAccount.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                //gvSelectedAccount.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
                //gvSelectedAccount.Rows[0].Cells[0].Font.Bold = true;
                //gvSelectedAccount.Rows[0].Cells[0].Text = "NO ACCOUNTS SELECTED";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AccountSelection(object sender, EventArgs e)
    {
        StringBuilder straccount = new StringBuilder();

        straccount.Append("," + ViewState["ACCOUNT"].ToString());

        foreach (ListItem item in cblAccount.Items)
        {
            if (item.Selected == true && !straccount.ToString().Contains("," + item.Value.ToString() + ","))
            {
                straccount.Append(item.Value.ToString());
                straccount.Append(",");
            }
            if (item.Selected == false && straccount.ToString().Contains("," + item.Value.ToString() + ","))
            {
                straccount.Replace("," + item.Value.ToString() + ",", ",");
            }
        }

        ViewState["ACCOUNT"] = straccount.ToString().StartsWith(",") ? straccount.ToString().Remove(0, 1) : straccount.ToString();
        BindSelected();
        gvSelectedAccount.Rebind();
    }

    protected void SelectAccount()
    {
        string str = "," + ViewState["ACCOUNT"].ToString();

        foreach (ListItem item in cblAccount.Items)
        {
            if (str.Contains("," + item.Value.ToString() + ","))
            {
                item.Selected = true;
            }
        }
    }

     
    private bool IsValidAccountFilter(string acc,  string fromdate, string todate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (acc.Trim().Length == 0)
            ucError.ErrorMessage = "Select one account.";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To date should be later than 'From Date'";
        }

        return (!ucError.IsError);
    }

    protected void cmdSearchAccount_Click(object sender, EventArgs e)
    {
        BindCheckBoxList();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void EditUserAccessLevel()
    {
        DataTable dt = new DataTable();

        try
        {
            dt = PheonixAccountsUserAccessList.UserAccountAccessEdit();

            if (dt.Rows.Count > 0)
            {
                txtUserAccess.Text = dt.Rows[0]["FLDACCESS"].ToString();
                //txtCompany.Text = PhoenixSecurityContext.CurrentSecurityContext.CompanyName;
            }
            else
                txtUserAccess.Text = "Normal";
            txtCompany.Text = PhoenixSecurityContext.CurrentSecurityContext.CompanyName;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSelectedAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindSelected();
    }

   
    protected void gvSelectedAccount_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        

        string str = "," + ViewState["ACCOUNT"].ToString();

        RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");

        str = str.Replace("," + lblAccountId.Text + ",", ",");
        ViewState["ACCOUNT"] = str.StartsWith(",") ? str.Remove(0, 1) : str;
        BindSelected();
        gvSelectedAccount.Rebind();
        BindCheckBoxList();
    }
}
